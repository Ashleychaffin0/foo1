#nullable enable

// Copyright (c) 2005-2019 by Larry Smith

// TODO: Jan/Feb 2009
//	*	Allow scanning multiple directories/drives. See point2 in next section.
//	*	Support exclusion dirs (e.g. C:\Windows, Recycle bin, maybe Program Files),
//		and a fortiori, their subdirs

// TODO: 
//	*	Need a way to search more than one dir, especially whole drives. See
//		Directory.GetLogicalDrives(). Note that it would be nice to distinguish local 
//		drives from network drives from	CDs from floppy	drives, etc. Also want to 
//		support \\machinename\dir.
//	*	Need file type support. e.g. *.cs;*.jpg;*.jpeg, etc
//	*	Use FileInfo and DirectoryInfo, rather than what we have now.
//	*	Add stats to form. # of Dirs, # of files, Unique File Sizes, # of potentially 
//		unique files
//	*	Calculating Short CRC message should have count of total # of files, so it
//		can display a progress bar. (And cool would be an estimated time to completion.)
//	*	Add more display info to the Process file size message.
//	*	Add the ability to bypass processing of specified directories. This would likely
//		include the system (C:\Windows) directories, and the recycle bin. And maybe TEMP
//		and TMP. Provide checkboxes for these (initialized to checked). Also provide a 
//		general add/remove directory feature (e.g. to bypass system directories/recycle 
//		bins on other hard drives on multi-boot systems).
//	*	Once a file entry has been stricken off the list (unique filesize, unique
//		filesize/CRC, file didn't match any others, etc), free up its space.
//	*	Add another pane to the status bar, with elapsed time in it (and maybe another 
//		with (probably laughably inaccurate) how much time left before the program 
//		finishes.
//	*	Rethink the whole CRC aspect. Calculating the CRCs seem to be slowing things 
//		down a lot,	as we open (at times) literally almost every file on the system. 
//		Maybe we don't do it for files too small. Or maybe too large? If file is small
//		enough (<=1K?), then if we calc CRCs for 1K, we don't have to open files to
//		compare. We just need to compare CRCs. For larger files, we'll just do the
//		straight file comparisons (although that leads to an n^2 set of comparisons).
//		Note: I just saw one bucket with 742 files in it, all of size 10K or so!
//	*	Hmmm. Big idea. Keep the raw data in a database. Basically we need the filename,
//		directory, date last modified and short CRC. This way we can bypass opening most
//		of the files. Hey, we could even calculate a full CRC if necessary (but it
//		probably wouldn't really help. Oh, and don't forget to delete database entries
//		if we don't find them on the drive (but that may be a separate button on 
//		the GUI).

#define DBG_DUMPHT
#define DBG_MSGS
#define DBG_SHOW_EQUAL_FILES
// #define		DBG_ECHO_TO_CONSOLE

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FindDuplicateFiles {

	/// <summary>
	/// Summary description for FindDuplicateFiles.
	/// </summary>
	public class FindDuplicateFiles : System.Windows.Forms.Form {
		private System.Windows.Forms.Button btnFolderBrowse;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtMinSize;
		private System.Windows.Forms.TextBox txtMaxSize;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radBytes;
		private System.Windows.Forms.RadioButton radKB;
		private System.Windows.Forms.RadioButton radGB;
		private System.Windows.Forms.RadioButton radMB;
		private System.Windows.Forms.RadioButton radTB;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Label lblDir;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.StatusBar StatBar;
		private Label txtOutputFilename;
		private Button btnOutputFile;

		//Note: FileInfo gives file sizes in terms of long's, not ulongs, so we'll do
		//		things its way (even though the filesize *really* should be a ulong).
		long	MinSize, MaxSize;
		long	ScaleFactor = 0;

		Dictionary<long, List<FileEntry>> DictSizes;

		// TextWriterTraceListener myTextListener;

		StreamWriter OutputFile;
		private CheckBox chkFullFileCompare;

		// We'll read in this many bytes from the start of the file to calculate a
		// "short" (i.e. rough cut) CRC for the file. We don't want to spend all the time
		// to read the entire file to get the real CRC. The first couple of hundred bytes
		// should be more than enough, especially since we'll use the combination of file
		// size and (short) CRC as the fingerprint.
		// Note: I chose 512 as being one physical sector on the (hard) disk. Floppies,
		//		 CD/DVDs, network drives, etc -- YMMV.
		const int	ShortCRCByteLength = 512;

		string OutputFilename = "FindDuplicateFiles.txt";

		List<string> DirectoriesToIgnore = new List<string>();

//---------------------------------------------------------------------------------------

		public FindDuplicateFiles() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

#if true
			lblDir.Text = @"G:\LRS\$Dev\C#";		// TODO:
			btnGo.Enabled = true;
#endif

			// Create a file for output named FindDuplicateFiles.txt.
			OutputFile = new StreamWriter(OutputFilename);
#if false
			/* Create a new text writer using the output stream, and add it to
			 * the trace listeners. */
			myTextListener = new TextWriterTraceListener(OutputFile);
			Trace.Listeners.Add(myTextListener);
#endif
			// Don't bother processing these
			DirectoriesToIgnore.AddRange(new List<string> {
				@"C:\Windows",
				@"C:\Program Files",
				@"F:\Program Files",
				@"G:\Program Files",
				@"C:\$Recycle.Bin",
				@"F:\$Recycle.Bin",
				@"G:\$Recycle.Bin",
			});
		}

//---------------------------------------------------------------------------------------

		private void btnFolderBrowse_Click(object sender, System.EventArgs e) {
			folderBrowserDialog1.SelectedPath = @"G:\";
			folderBrowserDialog1.Description = "Select directory to scan for duplicates";
			DialogResult res = folderBrowserDialog1.ShowDialog();
			if (res == DialogResult.Cancel)
				return;
			lblDir.Text = folderBrowserDialog1.SelectedPath;
			btnGo.Enabled = true;
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, System.EventArgs e) {

			DictSizes = new Dictionary<long, List<FileEntry>>();

			long	MinVal, MaxVal;

			GetScaleFactor();
			if (!CheckMinMaxSizes(out MinVal, out MaxVal)) {
				return;
			}
			MinSize = MinVal * ScaleFactor;
			MaxSize = MaxVal * ScaleFactor;

			WriteLine("Processing directory {0} at {1}\r\n", lblDir.Text, DateTime.Now);
			WriteLine("Running from {0}\r\n", Assembly.GetExecutingAssembly().GetName().CodeBase);

			var sw = new Stopwatch();
			sw.Start();

			var Entries = FileEntries.GetEntries(MinSize, MaxSize, ShortCRCByteLength, new List<string> { @"G:\Downloads\" });
			var qry = from ent in Entries
					  group ent by new {
						  size = ent.FileSize,
						  crc = ent.ShortCRC
					  }
					  into buckets
					  where buckets.Count() > 1
					  orderby buckets.Key.size descending
					  select new Bucket {
						  Size = buckets.Key.size,
						  Entries = buckets.ToList<FileEntry>()
					  };

			foreach (Bucket bucket in qry) {   // Each bucket has the same size/crc
				foreach (var entry in bucket.Entries) {
					Console.WriteLine($"{bucket.Size,14:N0} {entry.PathName}");
				}
				Console.WriteLine("=================");
			}

#if false
			xxxGetFilesWithinSize();

			DeleteSingletons(DictSizes, "DictSizes");

			CalcShortCRCs();

			ProcessSizeBuckets();

			// dbgDumpHTSizes();
#endif

			sw.Stop();
			StatBar.Text = $"Done in {sw.Elapsed} seconds";

			// WriteLine($"Done in {sw.Elapsed:N0} seconds", sw.Elapsed);
			System.Diagnostics.Debugger.Break();

			// Trace.Flush();

			OutputFile.Flush();

			Process.Start(OutputFilename);
		}

//---------------------------------------------------------------------------------------

#if false
		private void xxxGetFilesWithinSize() {
			var fis = from fi in ProcessDir(lblDir.Text)
					  where fi != null && fi.Length >= MinSize && fi.Length <= MaxSize
					  select fi;

			foreach (var fi in fis) {
				var fsize = fi.Length;
				if (!DictSizes.ContainsKey(fsize))	// Create new entry if fsize not yet seen
					DictSizes[fsize] = new List<FileEntry>();
				(DictSizes[fsize]).Add(new FileEntry(fi.DirectoryName, fi.FullName));
			}
		}

//---------------------------------------------------------------------------------------

		IEnumerable<FileInfo> ProcessDir(string DirName) {
			StatBar.Text = "Processing directory " + DirName;
			Application.DoEvents();

			bool bIsBadDir = false;
			foreach (var BadDdirname in DirectoriesToIgnore) {
				if (BadDdirname == DirName ) {
					bIsBadDir = true;
					break;
				}
			}

			if (bIsBadDir) {
				yield return null;
			} else {
				string[] DirFiles = null;
				try {
					DirFiles = Directory.GetFiles(DirName);
				} catch {
					// For security reasons (and maybe others), we may not be able to see the
					// files in this directory. If we can't, then silently ignore this dir.
				}
				if (DirFiles == null) {		// Still null? Must've failed in try clause
					yield return null;
				} else {
					foreach (string filename in DirFiles) {
						yield return ProcessFile(DirName, filename);
					}

					string[] DirDirs = null;
					try {
						DirDirs = Directory.GetDirectories(DirName);
					} catch {
						// Security may also bite us here. Or maybe the above operation
						// succeeds, but this one may fail. Or the other way around. In
						// either case, we'll check for problems both here and above.
					}
					if (DirDirs != null) {
						foreach (string dirname in DirDirs) {
							foreach (var fi in ProcessDir(dirname)) {
								yield return fi;
							}
						}
					}
				}
			}
		}
		
//---------------------------------------------------------------------------------------

		FileInfo ProcessFile(string DirName, string filename) {
			// StatBar.Text = "Processing file " + filename;
			FileInfo	fi = null;
			try {
				fi = new FileInfo(filename);
			} catch {
				// We may not be able to look at this file. It may be opened exclusively,
				// we may not have security access to it, etc. So just silently ignore it.
				// TODO: This looks like a bug in the runtime. We have a file that is
				//		 exactly 260 characters long, but FileInfo complains that it must
				//		 must be < 260 chars.
				return null;
			}
			// I don't quite understand this, but I've seen the above FileInfo work with
			// no errors, but when we try to get the file size, it reports File Not
			// Found?!? Maybe some security thing? Oh well, ignore it if we run across it
			if (! fi.Exists)
				return null;
			return fi;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Go through each entry in htSizes. Partition the List into a Dictionary
		/// based on the CRC. Remove any singletons, then process the remaining entries
		/// to check for true duplicate data.
		/// </summary>
		void ProcessSizeBuckets() {
			var	DictCRC = new Dictionary<uint, List<FileEntry>>();
			int nSizes = DictSizes.Keys.Count;
			int			n = 0;
			long[] Keys = GetSortedKeys(DictSizes);
			foreach (long key in Keys) {
				string msg = string.Format("\r\n\r\nProcessing file size {0:N0} ({1} of {2})",
					key, ++n, nSizes);
#if DBG_MSGS
				WriteLine(msg);
#endif
				StatBar.Text = msg;
				Application.DoEvents();
				DictCRC.Clear();			// Start fresh for each entry in DictSizes
				var Entries = DictSizes[key];
				foreach (FileEntry ent in Entries) {
					if (ent == null)
						continue;
					if (!DictCRC.ContainsKey(ent.ShortCRC)) {
						DictCRC[ent.ShortCRC] = new List<FileEntry>();
					}
					DictCRC[ent.ShortCRC].Add(ent);
				}
				DeleteSingletons(DictCRC, string.Format("DictCRC - filesize = {0:N0}", key));
				if (DictCRC.Count == 0)			// Anything left?
					continue;					// No, we're done with this one
				dbgDumpHTCRC(DictCRC);

				if (chkFullFileCompare.Checked) {
					ProcessCRCBuckets(DictCRC);
				}
			}
		}

//---------------------------------------------------------------------------------------

		void ProcessCRCBuckets(Dictionary<uint, List<FileEntry>> DictCRC) {
			// Each DictCRC entryis a List of FileEntry's (FEs), each of which has
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
			//		 first entry in the bucket's List of FEs, and compares it
			//		 against each of the other List entries. This means that if we
			//		 have, say, 10 entries in a bucket, we'll read (and re-read and
			//		 re-re-read) file[0] nine times, comparing it against each other
			//		 file. A more efficient approach would be to process, say, 5 files
			//		 in parallel, stopping processing of a given file if it ever differs
			//		 from file[0]. So we would read file[0] a max of twice, rather than
			//		 up to 9 times. But (initially at least) we're not going to do that
			//		 for four reasons:
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
			//  4)	Doing things in parallel is probably a bad idea. The disk head will
			//		probably be jumping all over the place, which might well make things
			//		slower than doing them sequentially.

			foreach (uint CRC in DictCRC.Keys) {
				ProcessSingleCRCBucket(DictCRC[CRC]);
				Application.DoEvents();
			}
		}

//---------------------------------------------------------------------------------------

		void ProcessSingleCRCBucket(List<FileEntry> FileEntries) {
			// See comments in ProcessCRCBuckets()

			// We're guaranteed that there are > 1 FileEntries. Compare element 0 with
			// each of the rest, in turn.

			const int BufSize = 64 * 1024;	// Assume files are dup's, so large blksize

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
				fe0 = (FileEntry)FileEntries[0];
				FoundDuplicate = false;
				using (FileStream fs0 = new FileStream(fe0.FileName, FileMode.Open, FileAccess.Read, FileShare.Read)) {
					// Hopefully we'll read to EOF and remove the FileEntry from the
					// ArrayList. To facilitate that, we'll process the ArrayList from
					// right-to-left.
					for (int i=FileEntries.Count - 1; i>0; --i) {
						FilesEqual = true;			// Ever the optimist
						fs0.Position = 0;			// Keep the file open, but rewind
						fe1 = (FileEntry)FileEntries[i];
						using  (FileStream fs1 = new FileStream(fe1.FileName, FileMode.Open, FileAccess.Read, FileShare.Read)) {
							while (true) {		// Until EOF
								Application.DoEvents();
								nBytes = fs0.Read(buf0, 0, BufSize);
								if (nBytes == 0)		// Check for EOF
									break;
								fs1.Read(buf1, 0, BufSize);
								// TODO: There's *gotta* be a better way to compare buf0
								// to buf1. But for now...
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
							if (FoundDuplicate == false)	// Check for first time through
								WriteLine("* Equal Files: ");
#endif
							FoundDuplicate = true;
							FileEntries.RemoveAt(i);
#if DBG_SHOW_EQUAL_FILES
							WriteLine("\t== {0}, ", fe1.FileName);
#endif
						}
					}
				}
				if (FoundDuplicate) {
#if DBG_SHOW_EQUAL_FILES
					WriteLine("\t== {0}", fe0.FileName);
#endif
				}
				FileEntries.RemoveAt(0);
			}
		}

//---------------------------------------------------------------------------------------

		void DeleteSingletons<TKey, TValue>(Dictionary<TKey, TValue> Dict, string DictName)
				where TValue : IList {
			// Delete all Dictionary entries where we have only a single entry. However,
			// I'm wary of deleting entries in the Dictionary while we're traversing it.
			// So we'll accumulate the keys to be deleted in a secondary List, then
			// use this to delete our hashtable entries at the end.
			// Note: This routine will work on any Dictionary as long as it's Value
			//		 parameter is of type IList. Which in this program means it's
			//		 a List<>.
			if (Dict.Count == 1) {
				return;
			}
			var ToBeDeleted = new List<TKey>();
			foreach (var key in Dict.Keys) {
				var Entries = Dict[key];
				if (Entries.Count == 1)
					ToBeDeleted.Add(key);
			}
			foreach (var key in ToBeDeleted) {
				Dict.Remove(key);
			}
#if DBG_MSGS
			WriteLine("Removed {0:N0} singleton key(s) from Dictionary {1}. Size went from {2:N0} to {3:N0}.", 
				ToBeDeleted.Count, DictName, Dict.Count + ToBeDeleted.Count, Dict.Count);
#endif
			return;
		}

//---------------------------------------------------------------------------------------

		void CalcShortCRCs() {
			int		n = 0;
			int nSizes = DictSizes.Keys.Count;
			long[] Keys = GetSortedKeys(DictSizes);
			foreach (var key in Keys) {
				var FileEntries = DictSizes[key];
				StatBar.Text = string.Format("Calculating short CRC {0} of {1} for filesize {2:N0} - files = {3}",
					++n, nSizes, key, FileEntries.Count);
				Application.DoEvents();
				// We may not be able to get the CRC if, for example, the file is
				// opened exclusively. If so, set the corresponding ArrayList entry
				// to null. Note that this has ramifications. Everywhere we process
				// FileEntry's, we'll have to check for null, and skip the entry.
				// TODO: Put null checks elsewhere in the code
				FileEntry	ent;
				for (int i=0; i<FileEntries.Count; ++i) {
					ent = FileEntries[i];
					try {
						// TODO: ent.GetShortCRC(ShortCRCByteLength);		
					} catch {
						FileEntries[i]= null;
					}
				}
			}
		}
#endif

//---------------------------------------------------------------------------------------

		long[] GetSortedKeys(Dictionary<long, List<FileEntry>> ht) {
			long [] Keys = new long [ht.Keys.Count];
			// htSizes.Keys.CopyTo(Keys, 0);
			ht.Keys.CopyTo(Keys, 0);
			Array.Sort(Keys);
			return Keys;
		}

//---------------------------------------------------------------------------------------

		private bool CheckMinMaxSizes(out long MinVal, out long MaxVal) {
			bool	bOK;
			string	msg;
			long	MaxValue = long.MaxValue / ScaleFactor;
			string	txtSize;

			MinVal = MaxVal = -1;	// For <out> reasons

			txtSize = txtMinSize.Text.Trim();			
			if (txtSize.Length == 0) {
				MinVal = 0;
			} else {
				bOK = GetVal(txtSize, 0, MaxValue, out MinVal);
				if (!bOK) {
					msg = string.Format("The minimum value must be integral, between 0 and {0}.", MaxValue);
					MessageBox.Show(msg, "Find Duplicate Files");
					return false;
				}
			}

			txtSize = txtMaxSize.Text.Trim();
			if (txtSize.Length == 0) {
				MaxVal = MaxValue;
			} else {
				bOK = GetVal(txtSize, MinVal + 1, MaxValue, out MaxVal);
				if (!bOK) {
					msg = string.Format("The minimum value must be integral, between {0} and {1}.", MinSize + 1, MaxValue);
					MessageBox.Show(msg, "Find Duplicate Files");
					return false;
				}
			}
			return true;
		}

//---------------------------------------------------------------------------------------

		bool GetVal(string txt, long MinVal, long MaxVal, out long Val) {
			bool	bOK = long.TryParse(txt, out Val);
			if (! bOK)
				return false;
			if (Val < MinVal || Val > MaxVal)
				return false; 
			return true;
		}

//---------------------------------------------------------------------------------------

		void GetScaleFactor() {
			int		ShiftFactor = 20;		// Default to MB, but shouldn't happen
			if (radBytes.Checked)	ShiftFactor = 0;
			else if (radKB.Checked)	ShiftFactor = 10;
			else if (radMB.Checked)	ShiftFactor = 20;
			else if (radGB.Checked)	ShiftFactor = 30;
			else if (radTB.Checked)	ShiftFactor = 40;
			ScaleFactor = 1L << ShiftFactor;
		}

#if false
		//---------------------------------------------------------------------------------------

		[Conditional("DBG_DUMPHT")]
		void dbgDumpHTSizes() {
			long[] Keys = GetSortedKeys(DictSizes);
			foreach (long key in Keys) {
				var FileEntries = DictSizes[key];
				WriteLine("***** File length = {0:N0}, entries = {1:N0}", key, FileEntries.Count);
				foreach (FileEntry entry in FileEntries) {
					// WriteLine("{0}\\{1}", ent.DirName, Path.GetFileName(ent.FileName));
					WriteLine("CRC = {0:X8} - {1}", entry.ShortCRC, entry.FileName);
				}
			}
		}

//---------------------------------------------------------------------------------------

		[Conditional("DBG_DUMPHT")]
		void dbgDumpHTCRC(Dictionary<uint, List<FileEntry>> DictCRC) {
			uint [] Keys = new uint [DictCRC.Keys.Count];
			DictCRC.Keys.CopyTo(Keys, 0);
			Array.Sort(Keys);
			foreach (uint key in Keys) {
				var al = DictCRC[key];
				WriteLine("***** CRC = {0:X08}, entries = {1:N0}", key, al.Count);
				foreach (FileEntry ent in al) {
					// WriteLine("{0}\\{1}", ent.DirName, Path.GetFileName(ent.FileName));
					WriteLine("    {0}", ent.FileName);
				}
			}
		}
#endif

//---------------------------------------------------------------------------------------

		private void FindDuplicateFiles_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
#if false
			// Flush and close the output.
			Trace.Flush(); 
			myTextListener.Flush();
			myTextListener.Close();
#endif
			OutputFile.Close();
		}

//---------------------------------------------------------------------------------------

		private void Write(string fmt, params object[] parms) {
			string s = string.Format(fmt, parms);
			OutputFile.Write("{0}", s);
#if DBG_ECHO_TO_CONSOLE
			Console.Write("{0}", s);
#endif
		}

//---------------------------------------------------------------------------------------

		private void WriteLine(string fmt, params object[] parms) {
			string s = string.Format(fmt, parms);
			OutputFile.WriteLine("{0}", s);
#if DBG_ECHO_TO_CONSOLE
			Console.WriteLine("{0}", s);
#endif
		}

//---------------------------------------------------------------------------------------

#region Windows Form Designer generated code

		/// <summary>
		/// Clean up any resources being used.
		/// </summary
		protected override void Dispose(bool disposing) {
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.btnFolderBrowse = new System.Windows.Forms.Button();
			this.lblDir = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtMinSize = new System.Windows.Forms.TextBox();
			this.txtMaxSize = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radTB = new System.Windows.Forms.RadioButton();
			this.radGB = new System.Windows.Forms.RadioButton();
			this.radMB = new System.Windows.Forms.RadioButton();
			this.radKB = new System.Windows.Forms.RadioButton();
			this.radBytes = new System.Windows.Forms.RadioButton();
			this.btnGo = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.StatBar = new System.Windows.Forms.StatusBar();
			this.txtOutputFilename = new System.Windows.Forms.Label();
			this.btnOutputFile = new System.Windows.Forms.Button();
			this.chkFullFileCompare = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnFolderBrowse
			// 
			this.btnFolderBrowse.Location = new System.Drawing.Point(16, 24);
			this.btnFolderBrowse.Name = "btnFolderBrowse";
			this.btnFolderBrowse.Size = new System.Drawing.Size(112, 32);
			this.btnFolderBrowse.TabIndex = 0;
			this.btnFolderBrowse.Text = "Folder Browse";
			this.btnFolderBrowse.Click += new System.EventHandler(this.btnFolderBrowse_Click);
			// 
			// lblDir
			// 
			this.lblDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblDir.Location = new System.Drawing.Point(144, 24);
			this.lblDir.Name = "lblDir";
			this.lblDir.Size = new System.Drawing.Size(536, 32);
			this.lblDir.TabIndex = 1;
			this.lblDir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 126);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "Min Size";
			// 
			// txtMinSize
			// 
			this.txtMinSize.Location = new System.Drawing.Point(88, 126);
			this.txtMinSize.Name = "txtMinSize";
			this.txtMinSize.Size = new System.Drawing.Size(80, 22);
			this.txtMinSize.TabIndex = 1;
			this.txtMinSize.Text = "1";
			// 
			// txtMaxSize
			// 
			this.txtMaxSize.Location = new System.Drawing.Point(288, 126);
			this.txtMaxSize.Name = "txtMaxSize";
			this.txtMaxSize.Size = new System.Drawing.Size(80, 22);
			this.txtMaxSize.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(200, 126);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(104, 23);
			this.label3.TabIndex = 4;
			this.label3.Text = "Max Size";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radTB);
			this.groupBox1.Controls.Add(this.radGB);
			this.groupBox1.Controls.Add(this.radMB);
			this.groupBox1.Controls.Add(this.radKB);
			this.groupBox1.Controls.Add(this.radBytes);
			this.groupBox1.Location = new System.Drawing.Point(16, 166);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(432, 64);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Units";
			// 
			// radTB
			// 
			this.radTB.Location = new System.Drawing.Point(344, 32);
			this.radTB.Name = "radTB";
			this.radTB.Size = new System.Drawing.Size(72, 24);
			this.radTB.TabIndex = 4;
			this.radTB.Text = "TB";
			// 
			// radGB
			// 
			this.radGB.Location = new System.Drawing.Point(264, 32);
			this.radGB.Name = "radGB";
			this.radGB.Size = new System.Drawing.Size(72, 24);
			this.radGB.TabIndex = 3;
			this.radGB.Text = "GB";
			// 
			// radMB
			// 
			this.radMB.Checked = true;
			this.radMB.Location = new System.Drawing.Point(184, 32);
			this.radMB.Name = "radMB";
			this.radMB.Size = new System.Drawing.Size(72, 24);
			this.radMB.TabIndex = 2;
			this.radMB.TabStop = true;
			this.radMB.Text = "MB";
			// 
			// radKB
			// 
			this.radKB.Location = new System.Drawing.Point(104, 32);
			this.radKB.Name = "radKB";
			this.radKB.Size = new System.Drawing.Size(72, 24);
			this.radKB.TabIndex = 1;
			this.radKB.Text = "KB";
			// 
			// radBytes
			// 
			this.radBytes.Location = new System.Drawing.Point(24, 32);
			this.radBytes.Name = "radBytes";
			this.radBytes.Size = new System.Drawing.Size(72, 24);
			this.radBytes.TabIndex = 0;
			this.radBytes.Text = "Bytes";
			// 
			// btnGo
			// 
			this.btnGo.Enabled = false;
			this.btnGo.Location = new System.Drawing.Point(472, 182);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(88, 32);
			this.btnGo.TabIndex = 5;
			this.btnGo.Text = "Go";
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// btnStop
			// 
			this.btnStop.Enabled = false;
			this.btnStop.Location = new System.Drawing.Point(584, 182);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(88, 32);
			this.btnStop.TabIndex = 6;
			this.btnStop.Text = "Stop";
			// 
			// StatBar
			// 
			this.StatBar.Location = new System.Drawing.Point(0, 238);
			this.StatBar.Name = "StatBar";
			this.StatBar.Size = new System.Drawing.Size(696, 22);
			this.StatBar.TabIndex = 7;
			// 
			// txtOutputFilename
			// 
			this.txtOutputFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtOutputFilename.Location = new System.Drawing.Point(144, 62);
			this.txtOutputFilename.Name = "txtOutputFilename";
			this.txtOutputFilename.Size = new System.Drawing.Size(536, 32);
			this.txtOutputFilename.TabIndex = 9;
			this.txtOutputFilename.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnOutputFile
			// 
			this.btnOutputFile.Location = new System.Drawing.Point(16, 62);
			this.btnOutputFile.Name = "btnOutputFile";
			this.btnOutputFile.Size = new System.Drawing.Size(112, 32);
			this.btnOutputFile.TabIndex = 8;
			this.btnOutputFile.Text = "Output File";
			this.btnOutputFile.Click += new System.EventHandler(this.button1_Click);
			// 
			// chkFullFileCompare
			// 
			this.chkFullFileCompare.AutoSize = true;
			this.chkFullFileCompare.Location = new System.Drawing.Point(472, 128);
			this.chkFullFileCompare.Name = "chkFullCrcChecking";
			this.chkFullFileCompare.Size = new System.Drawing.Size(168, 21);
			this.chkFullFileCompare.TabIndex = 10;
			this.chkFullFileCompare.Text = "Use Full File Compare";
			this.chkFullFileCompare.UseVisualStyleBackColor = true;
			// 
			// FindDuplicateFiles
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(696, 260);
			this.Controls.Add(this.chkFullFileCompare);
			this.Controls.Add(this.txtOutputFilename);
			this.Controls.Add(this.btnOutputFile);
			this.Controls.Add(this.StatBar);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.txtMaxSize);
			this.Controls.Add(this.txtMinSize);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lblDir);
			this.Controls.Add(this.btnFolderBrowse);
			this.Name = "FindDuplicateFiles";
			this.Text = "Find Duplicate Files";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FindDuplicateFiles_Closing);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new FindDuplicateFiles());
		}
#endregion

		private void button1_Click(object sender, EventArgs e) {

		}
	}
}
