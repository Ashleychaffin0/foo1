// Copyright (c) 2010-2011 by Larry Smith

// #define NO_UI_FOR_PROFILING
// #define PARALLEL
// #define PARTITIONING
#define DO_SHORT_CRC_CALC
// #define PARALLEL_TOLIST

using System;
using System.Collections.Generic;
using System.Collections;					// TODO: Move to new class
using System.Diagnostics;
using System.Drawing;
using System.IO;							// TODO: Move to new class
using System.Linq;							// TODO: Move to new class
using System.Text;
using System.Text.RegularExpressions;		// TODO: Move to new class
using System.Windows.Forms;

using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

using DirectoryWalker_2;
using LRSS;
using LrsFileUtils;

// TODO:

// High proiotity
//	*	Get Config dlg fully functional (incl Add button), and tested.

//	*	Cut down solution references
//	*	Menu Tools -> Options
//	*	About box (see template)
//	*	Options dlg -- Reset
//	*	Get the whole Parms thing working, willya?
//	*	Define class (in a file) for fast, buffered, overlapped file reads (OPTCD=Q)
//	*	EditComboBox must take flag to say if Add prompts for directory or not
//		* Current Edit button always just shows folder dialog
//	*	Status Bar
//		*	Make subroutine to display stuff
//		*	Seems to not just clip, but not display text that's too long. Stretch?
//		*	For CalcShortCRCs et al, show both text and progress bar

namespace FindDuplicateFilesGui {
	public partial class FindDuplicateFilesGui : Form {

		ConfigData Parms;

		// We'll read in this many bytes from the start of the file to calculate a
		// "short" (i.e. rough cut) CRC for the file. We don't want to spend all the time
		// to read the entire file to get the real CRC. The first couple of hundred bytes
		// should be more than enough, especially since we'll use the combination of file
		// size and (short) CRC as the fingerprint.
		// Note: I chose 512 as being one physical sector on the (hard) disk. Floppies,
		//		 CD/DVDs, network drives, etc -- YMMV.
		// Note: Even with new hard drives coming out with 4K sector sizes, we'll still
		//		 keep this at 512.
		int ShortCRCByteLength = 512;

		bool bShowPlainBackgroundColor = false;


//---------------------------------------------------------------------------------------

		public FindDuplicateFilesGui() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void configureToolStripMenuItem_Click(object sender, EventArgs e) {
			ShowConfigDialog();
		}

//---------------------------------------------------------------------------------------

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
			string msg = "Find Duplicate Files\n\nVersion 1.0\n\n"
				+ "Copyright 2010-2011 by Larry Smith";
			MessageBox.Show(msg, "Find Duplicate Files",
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

//---------------------------------------------------------------------------------------

		private void FindDuplicateFilesGui_Load(object sender, EventArgs e) {
#if NO_UI_FOR_PROFILING			// TODO:
			string MyDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			MyDocs = Path.Combine(MyDocs, "FindDuplicateFilesParms.xml");
			Parms = GenericSerialization<ConfigData>.Load(MyDocs);
			Parms.MinSize = 1 * 1024 * 1024;
			Parms.MaxSize = 1024 * 1024 * 1024;

			ProcessAll(Parms);
			Application.Exit();
#else
			ShowConfigDialog();
#endif
		}

//---------------------------------------------------------------------------------------

		private void ShowConfigDialog() {
			string MyDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			MyDocs = Path.Combine(MyDocs, "FindDuplicateFilesParms.xml");
			if (!File.Exists(MyDocs)) {
				Parms = new ConfigData();
				Parms.SetupDefaults();
				GenericSerialization<ConfigData>.Save(MyDocs, Parms);
			} else {
				// TODO: try/catch, or whatever
				Parms = GenericSerialization<ConfigData>.Load(MyDocs);
			}
			var dlg = new FdfOptions(Parms);
			var res = dlg.ShowDialog();
			if (res != DialogResult.OK) {
				return;
			}
			GenericSerialization<ConfigData>.Save(MyDocs, Parms);
		}

//---------------------------------------------------------------------------------------

		private void runToolStripMenuItem_Click(object sender, EventArgs e) {
			var SaveCursor = this.Cursor;
			try {
				this.Cursor = Cursors.WaitCursor;
				ProcessAll(Parms);
			} finally {
				this.Cursor = SaveCursor;
			}
			toolStripStatusLabel1.Text = "Done";
		}

//---------------------------------------------------------------------------------------

		private void ProcessAll(ConfigData Parms) {
			Stopwatch sw = new Stopwatch();
			sw.Start();
			var fbs = new FilesBySizeNormalDict();
			// TODO: Take out some of the debugging stuff, like Stopwatch and nFiles.
			int nFiles = 0;
#if true
			var cp = System.Diagnostics.Process.GetCurrentProcess();
			var tpt = cp.TotalProcessorTime;
			var upt = cp.UserProcessorTime;
			var ppt = cp.PrivilegedProcessorTime;

#if false
			Regex reExclude = new Regex(Parms.FileMasksToExclude, RegexOptions.Compiled | RegexOptions.IgnoreCase);
			Regex reInclude = new Regex(Parms.FileMasksToInclude, RegexOptions.Compiled | RegexOptions.IgnoreCase);
#else
			Regex reExclude = new Regex(@"^(.*\.dll)|(.*\.exe)|(\.user)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
			Regex reInclude = new Regex(@"^(.*\.pdf)|(.*\.zip)|(.*\.wmv)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
#endif
			foreach (var dir in Parms.DirectoriesToInclude) {
				var walker = new DirectoryWalker(dir, true);
				var q1 = from dirwalk in walker.Walk().AsParallel()//.WithDegreeOfParallelism(Environment.ProcessorCount)
						 where dirwalk.IsDirectory
						 // let foo = ShowStatMsg("Processing directory " + dirwalk.DirInfo.FullName)
						 from fi in dirwalk.LrsGetFileInfo()
						 where reInclude.IsMatch(fi.Name) && ! reExclude.IsMatch(fi.Name)
						 // TODO: Following <select> clause is too general
						 select new { Dirname = string.Intern(dirwalk.DirInfo.Name), Filename = fi.FullName, Dirinfo = dirwalk.DirInfo, fileinfo = fi, IsDir = dirwalk.IsDirectory };
				ShowStatMsg(this, "Starting...");
				foreach (var item in q1) {
					// Console.WriteLine("Dir={0}, file={1}", item.Dirname, item.Filename);
					if (item.IsDir) 
						ShowStatMsg(this, "Processing directory " + item.Dirinfo.FullName);
					++nFiles;
					var fe = new FileEntry(item.Dirname, item.Filename);
					// TODO: TODO: TODO: Need ConcurrentDictionary or other locking on this!!!
					ProcessFile(fe, fbs);
				}
			}

			// TODO: fbs.ShortCRC is always zero
			// BUG:  fbs.ShortCRC is always zero
			ProcessSizeBuckets(fbs.DictBySize);

			sw.Stop();
			Console.WriteLine("Elapsed={0}, Total={1}, User={2}, Priv={3}", 
				sw.Elapsed,
				cp.TotalProcessorTime - tpt,
				cp.UserProcessorTime - upt,
				cp.PrivilegedProcessorTime - ppt);
#endif
#if PARALLEL
			var ParOpts = new ParallelOptions();
			ParOpts.MaxDegreeOfParallelism = 4;
#if PARTITIONING
			var x = Partitioner.Create<string>(ff.FindFiles(Parms.DirectoriesToInclude));
			var y = x.SupportsDynamicPartitions;
			var z = x.GetPartitions(100);
			/*
			public static ParallelLoopResult ForEach<TSource, TLocal>(
					Partitioner<TSource>							 source, 
					ParallelOptions									 parallelOptions, 
					Func<TLocal>									 localInit, 
					Func<TSource, ParallelLoopState, TLocal, TLocal> body, 
					Action<TLocal>									 localFinally);
			*/
			Parallel.ForEach(
				z,														// Source
				ParOpts,												// Options
				() => new FilesBySizeNormalDict(),						// LocalInit
				(fns, pls, lcl) => {									// Body
					while (fns.MoveNext()) {
						Interlocked.Increment(ref nFiles);
						var fe = new FileEntry(fns.Current);
						ProcessFile(fe, lcl);
						
					}
					return lcl;
				},
				localFinally => {										// localFinally
					MergeFbs(fbs, localFinally);
				}
			);
#else
			/*
			public static ParallelLoopResult ForEach<TSource, TLocal>(
					IEnumerable<TSource>							 source,
					ParallelOptions									 parallelOptions,
					Func<TLocal>									 localInit,
					Func<TSource, ParallelLoopState, TLocal, TLocal> body,
					Action<TLocal>									 localFinally
				)
			*/
			Parallel.ForEach(
				ff.FindFiles(Parms.DirectoriesToInclude),				// Source
				ParOpts,												// Options
				() => new FilesBySizeNormalDict(),						// LocalInit
				(fn, pls, lcl) => {										// Body
					Interlocked.Increment(ref nFiles);
					var fe = new FileEntry(fn);
					ProcessFile(fe, lcl);
					return lcl;
				},
				localFinally => {										// localFinally
					MergeFbs(fbs, localFinally);
				}
			);
#endif
#else
			/*
			var ff = new FilesFinder(Parms.DirectoriesToExclude, Parms.FileMasksToInclude, Parms.FileMasksToExclude);
			foreach (var fn in ff.FindFiles(Parms.DirectoriesToInclude)) {
				++nFiles;
				var fe = new FileEntry(fn);
				ProcessFile(fe, fbs);
			}
			*/
#endif
			DeleteSingletons.Delete(fbs.DictBySize);
			sw.Stop();
			var part1 = sw.Elapsed;
			sw.Restart();
#if DO_SHORT_CRC_CALC
			fbs.CalcShortCRCs_Parallel(this, ShortCRCByteLength);
			// fbs.CalcShortCRCs(ShortCRCByteLength);
#endif
			sw.Stop();
			var part2 = sw.Elapsed;
			Console.WriteLine("{0:N0} Files, Part 1 = {1}, Part 2 = {2}", nFiles, part1, part2);

			DisplayFinalData(fbs);
		 }

//---------------------------------------------------------------------------------------

		public static int ShowStatMsg(FindDuplicateFilesGui form, string msg) {
			if (form.InvokeRequired) {
				form.Invoke((MethodInvoker)delegate {
					form.toolStripStatusLabel1.Text = msg;
					Application.DoEvents();
				});
			} else {
				form.toolStripStatusLabel1.Text = msg;
				Application.DoEvents();
			}
			// Note: Returns an <int> that's not used for anything, but the <let> clause
			//		 in a FROM clause needs (I think) a return type
			// Note: When I try to call this routine in a <let> statement, the 
			//		 application seems to hang.
			return 0;
		}

//---------------------------------------------------------------------------------------

		private void DisplayFinalData(FilesBySizeNormalDict fbs) {
			Directory.CreateDirectory(@"C:\LRS");		// In case running on C:\Win8
			// var outFile = File.CreateText(@"C:\LRS\FDF.txt");
			var fbsKey = from key in fbs.Keys
					   orderby key descending
					   select key;
			foreach (var key in fbsKey) {
				AddGridEntries(fbs[key]);
			}
		}

//---------------------------------------------------------------------------------------

		private void AddGridEntries(List<FileEntry> list) {
			// TODO: Next test shouldn't be necessary
			if (list.Count == 0) return;
			Color bgColor = bShowPlainBackgroundColor ? Color.White : Color.LightGreen;
			int ix = 0;
			foreach (var item in list) {
				ix                             = Grid.Rows.Add();
				Grid.Rows[ix].DefaultCellStyle.BackColor = bgColor;
				DataGridViewRow row            = Grid.Rows[ix];
				row.Cells["DeleteBox"].Value   = false;
				row.Cells["FileName"].Value    = Path.GetFileName(item.FileName);
				row.Cells["DirName"].Value     = Path.GetDirectoryName(item.FileName);
				row.Cells["FileSize"].Value    = string.Format("{0:N0}", item.FileSize);
			}
			bShowPlainBackgroundColor = !bShowPlainBackgroundColor;
		}

//---------------------------------------------------------------------------------------

		private void MergeFbs(FilesBySizeNormalDict fbs, FilesBySizeNormalDict localFinally) {
			if (localFinally.DictBySize.Count > 1) {
				lock (fbs) {
					foreach (var key in localFinally.DictBySize.Keys) {
						foreach (FileEntry fe in localFinally.DictBySize[key]) {
							fbs.Add(key, fe);		 
						}
					}
				}
			}
			
#if false			// TODO:
			Console.WriteLine("****** MergeFbs - TID = {0}, size = {1}", 
				Thread.CurrentThread.ManagedThreadId, localFinally.DictBySize.Count);
			if (localFinally.DictBySize.Count > 1) {
				int i = 1;
			}
#endif
		}

//---------------------------------------------------------------------------------------

		private void dbgDump(Dictionary<uint, List<FileEntry>> DictCRC) {
			// TODO: Maybe.
		}

//---------------------------------------------------------------------------------------

		void ProcessFile(FileEntry fe, FilesBySizeNormalDict fbs) {
			// toolStripStatusLabel1.Text = "Processing file " + filename;
			string FullPath = Path.Combine(fe.DirName, fe.FileName);
			FileInfo fi = null;
			long fsize = 0;
			try {
				fi = new FileInfo(FullPath);
				// I don't quite understand this, but I've seen the above FileInfo work
				// with no errors, but when we try to get the file size, it reports File
				// Not Found?!? Maybe some security thing? Oh well, ignore it if we run
				// across it.
				fsize = fi.Length;
			} catch {
				// We may not be able to look at this file. It may be opened exclusively,
				// we may not have security access to it, etc. So just silently ignore it.
				// TODO: This looks like a bug in the runtime. We have a file that is
				//		 exactly 260 characters long, but FileInfo complains that it
				//		 must be < 260 chars.
				return;
			}

			// Filter the file entries by size
			if ((fsize < Parms.MinSize) || (fsize > Parms.MaxSize))
				return;

			// Console.WriteLine("{0}: {1} -- {2}", Thread.CurrentThread.ManagedThreadId, fe.DirName, fe.FileName);
			fbs.Add(fsize, fe);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Go through each entry in DictSizes. Partition the Dictionary into a
		/// Dictionary based on the CRC. Remove any singletons, then process the
		/// remaining entries to check for true duplicate data.
		/// </summary>
		Dictionary<uint, List<FileEntry>> ProcessSizeBuckets(Dictionary<long, List<FileEntry>> DictSizes) {
			var			DictCRC = new Dictionary<uint, List<FileEntry>>();
			int			nSizes = DictSizes.Keys.Count;
			toolStripProgressBar1.Maximum = nSizes;
			int			n = 0;
			var			SortedKeys = DictSizes.Keys.OrderBy(key => key);
			foreach (long key in SortedKeys) {
				string msg = string.Format("Processing file size {0:N0} ({1} of {2})",
					key, ++n, nSizes);
				toolStripProgressBar1.Value = n;
#if DBG_MSGS
				WriteLine(msg);
#endif
				toolStripStatusLabel1.Text = msg;
				Application.DoEvents();
				DictCRC.Clear();			// Start fresh for each entry in htSizes
				List<FileEntry> items = DictSizes[key];
				foreach (FileEntry ent in items) {
					if (ent == null)
						continue;
					if (! DictCRC.ContainsKey(ent.ShortCRC)) {
						DictCRC[ent.ShortCRC] = new List<FileEntry>();
					}
					DictCRC[ent.ShortCRC].Add(ent);
				}
				DeleteSingletons.Delete(DictCRC);
				if (DictCRC.Count == 0)			// Anything left?
					continue;					// No, we're done with this one
				// dbgDumpHTCRC(DictCRC);
				ProcessCRCBuckets(DictCRC);
			}
			toolStripProgressBar1.Value = 0;
			return DictCRC;
		}

//---------------------------------------------------------------------------------------

		void ProcessCRCBuckets(Dictionary<uint, List<FileEntry>> DictCRC) {
			// Each DictCRC entryis a List<> of FileEntry's (FEs), each of which has
			// .Count > 1, the same file size, and the same ShortCRC. We want to save
			// which of these are truly duplicate files. The bad news is that if there
			// are, say, 10 FEs in a bucket, 3 of these may be duplicates of one file,
			// two may be duplicates of another, and 4 of yet another, while the
			// remaining field doesn't match any of the others. So we must be prepared
			// to partition the entries in this bucket into multiple sub-buckets.

			// The good news is that if the file size and short CRC match, the odds
			// favor the files being the same. So our algorithm is going to be based on
			// the assumption that all the files are indeed duplicates of each other. If
			// we're wrong, we accept that there will be some amount of additional
			// overhead that could have been avoided with a different algorithm.

			// Note: Our current algorithm has a known performance problem. It takes the
			//		 first entry in the bucket's List<> of FEs, and compares it
			//		 against each of the other List entries. This means that if we
			//		 have, say, 10 entries in a bucket, we'll read (and re-read and
			//		 re-re-read) file[0] nine times, comparing it against each other
			//		 file. A more efficient approach would be to process, say, 5 files
			//		 in parallel, stopping processing of a given file if it ever differs
			//		 from file[0]. So we would read file[0] a max of twice, rather than
			//		 up to 9 times. But (initially at least) we're not going to do that
			//		 for three reasons:
			//	1)	We'll keep the code simple until it's proved we need the more
			//		efficient algorithm. (We'll keep stats to let us know if we do.)
			//	2)	We'll isolate the logic so that if we change our minds, we'll just
			//		have to update a single routine.
			//	3)	I don't really expect that many duplicates of very large files. Just
			//		how many 3.7GB copies of VS2005 August CTP are there likely to be on
			//		a system? I expect duplicates to be more modest in size, so hopefully
			//		the file data will remain in the cache, and thus speed things up. 
			//		Note to self: Look into using the Win32 functions to specify that 
			//		caching is turned off for other than file[0].

			foreach (uint CRC in DictCRC.Keys) {
				ProcessCRCBucket(DictCRC[CRC]);
				Application.DoEvents();
			}
		}

//---------------------------------------------------------------------------------------

		void ProcessCRCBucket(List<FileEntry> FileEntries) {
			// See comments in ProcessCRCBuckets()

			// We're guaranteed that there are > 1 FileEntries. Compare element 0 with
			// each of the rest, in turn.

			const int BufSize = 64 * 1024;	// Assume files are duplicates, so large blksize

			byte [] buf0 = new byte[BufSize];
			byte [] buf1 = new byte[BufSize];

			int		nBytes;					// Number of bytes read. Since the file sizes
											//   are the same, we need only one variable

			FileEntry	fe0, fe1;

			bool		FilesEqual;
			bool		FoundDuplicate;		// Found (at least) one dup of fe0

			// Each pass will find the duplicates of FileEntries[0], and remove them. 
			// We'll keep looping while we've still got 2 or more files to compare.

			// TODO: Worry about I/O (and maybe security, etc) errors later.
			while (FileEntries.Count > 1) {
				fe0 = FileEntries[0];
				FoundDuplicate = false;
				string filename_0 = Path.Combine(fe0.DirName, fe0.FileName);
				using (FileStream fs0 = new FileStream(filename_0, FileMode.Open, FileAccess.Read, FileShare.Read)) {
					// Hopefully we'll read to EOF and remove the FileEntry from the
					// ArrayList. To facilitate that, we'll process the ArrayList from
					// right-to-left.
					for (int i=FileEntries.Count - 1; i>0; --i) {
						FilesEqual = true;			// Ever the optimist
						fs0.Position = 0;			// Keep the file open, but rewind
						fe1 = FileEntries[i];
						string filename_1 = Path.Combine(fe1.DirName, fe1.FileName);
						using (FileStream fs1 = new FileStream(filename_1, FileMode.Open, FileAccess.Read, FileShare.Read)) {
							while (true) {		// Until EOF
								nBytes = fs0.Read(buf0, 0, BufSize);
								if (nBytes == 0)		// Check for EOF
									break;
								fs1.Read(buf1, 0, BufSize);
								// There's *gotta* be a better way to compare buf0 to buf1. TODO:
								// But for now...
								for (int n=0; n<BufSize; ++n) {
									if (buf0[n] != buf1[n]) {
										FilesEqual = false;
										break;
									}
								}
								// Where's a labelled break when you need one? Sigh.
								if (! FilesEqual)
									break;
							}
						}
						if (FilesEqual) {
#if DBG_SHOW_EQUAL_FILES
							if (FoundDuplicate == false)	// DBG: Check for first time through
								WriteLine("* Equal Files: ");
#endif
							FoundDuplicate = true;
							FileEntries.RemoveAt(i);
							// Major TODO: Add fe1 to some kind of list.
							// TODO: For now, just ...
#if DBG_SHOW_EQUAL_FILES
							WriteLine("\t{0}, ", fe1.FileName);	// TODO: Remove
#endif
						}
					}
				}
				if (FoundDuplicate) {
#if DBG_SHOW_EQUAL_FILES
					// Major TODO: See if we added any duplicates above, and add fe0 to the same list
					WriteLine("\t{0}", fe0.FileName);	// TODO: Remove
#endif
				}
				FileEntries.RemoveAt(0);
			}
		}

//---------------------------------------------------------------------------------------

		private void btnDeleteSelectedFiles_Click(object sender, EventArgs e) {
			foreach (DataGridViewRow row in Grid.Rows) {
				if ((bool)row.Cells["DeleteBox"].Value == true)
					// TODO:
					Console.WriteLine("Row={0}, Path={1}", row.Index, Path.Combine((string)row.Cells["DirName"].Value, (string)row.Cells["FileName"].Value));
			}
		}
	}
}
