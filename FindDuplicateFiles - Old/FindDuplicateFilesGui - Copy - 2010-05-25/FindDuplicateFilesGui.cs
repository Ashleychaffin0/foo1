// Copyright (c) 2010 by Larry Smith

using System;
using System.Collections.Generic;
using System.Collections;					// TODO: Move to new class
using System.IO;							// TODO: Move to new class
using System.Linq;							// TODO: Move to new class
using System.Text;
using System.Text.RegularExpressions;		// TODO: Move to new class
using System.Windows.Forms;

using LRSS;
using LrsFileUtils;
using Bartizan.Utils.CRC;

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
				+ "Copyright 2010 by Larry Smith";
			MessageBox.Show(msg, "Find Duplicate Files",
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

//---------------------------------------------------------------------------------------

		private void FindDuplicateFilesGui_Load(object sender, EventArgs e) {
			ShowConfigDialog();
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
				ProcessDirs(Parms);

				DeleteSingletons<long, FileEntry>(DictSizes, "DictSizes");

				CalcShortCRCs(DictSizes);

				Dictionary<uint, List<FileEntry>> DictCRC = ProcessSizeBuckets(DictSizes);
				dbgDump(DictCRC);
			} finally {
				this.Cursor = SaveCursor;
			}
			toolStripStatusLabel1.Text = "Done";
		}

		// TODO: Following code from original program. Class-ify them and move to their
		//		 own class files.

		Dictionary<long, List<FileEntry>> DictSizes = new Dictionary<long, List<FileEntry>>();

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

//---------------------------------------------------------------------------------------

		private void dbgDump(Dictionary<uint, List<FileEntry>> DictCRC) {
			// TODO: Maybe.
		}

//---------------------------------------------------------------------------------------

		void ProcessDirs(ConfigData Parms) {

			DictSizes.Clear();			// In case we File | Run more than once

			foreach (var dirname in Parms.DirectoriesToInclude) {
				ProcessDir(dirname, Parms);
			}
		}

//---------------------------------------------------------------------------------------

		void ProcessDir(string DirName, ConfigData Parms) {
			toolStripStatusLabel1.Text = "Processing directory " + DirName;
			Application.DoEvents();
			string[] DirFiles = null;
			try {
				DirFiles = Directory.GetFiles(DirName);
			} catch {
				// For security reasons (and maybe others), we may not be able to see the
				// files in this directory. If we can't, then silently ignore this dir.
				return;
			}
			foreach (string filename in DirFiles) {
				ProcessFile(DirName, filename);
			}

			string[] DirDirs = null;
			try {
				DirDirs = Directory.GetDirectories(DirName);
			} catch {
				// Security may also bite us here. Or maybe the above operation succeeds,
				// but this one may fail. Or the other way around. In either case, we'll
				// check for problems both here and above.
			}
			foreach (string dirname in DirDirs) {
				ProcessDir(dirname, Parms);
			}
		}

//---------------------------------------------------------------------------------------

		void ProcessFile(string DirName, string filename) {
			// TODO: Filter by FileMasksTo*
			// toolStripStatusLabel1.Text = "Processing file " + filename;
			string FilenameOnly = filename.Substring(DirName.Length + 1);
			FileInfo fi = null;
			try {
				fi = new FileInfo(filename);
			} catch {
				// We may not be able to look at this file. It may be opened exclusively,
				// we may not have security access to it, etc. So just silently ignore it.
				// TODO: This looks like a bug in the runtime. We have a file that is
				//		 exactly 260 characters long, but FileInfo complains that it must
				//		 must be < 260 chars.
				return;
			}
			// I don't quite understand this, but I've seen the above FileInfo work with
			// no errors, but when we try to get the file size, it reports File Not
			// Found?!? Maybe some security thing? Oh well, ignore it if we run across it
			if (!fi.Exists)
				return;
			long fsize = fi.Length;
			if ((fsize < Parms.MinSize) || (fsize > Parms.MaxSize))
				return;
			// Add to dictionary, creating entry if necessary
			if (!DictSizes.ContainsKey(fsize))
				DictSizes[fsize] = new List<FileEntry>();
			DictSizes[fsize].Add(new FileEntry(DirName, filename));
		}


//---------------------------------------------------------------------------------------

		void DeleteSingletons<T, V>(Dictionary<T, List<V>> Dict, string DictName) {
			// Delete all dictionary entries where we have only a single entry. However,
			// I'm wary of deleting entries in the dictionary while we're traversing it.
			// So we'll accumulate the keys to be deleted in a secondary List, then
			// use this to delete our dictionary entries at the end.
			if (Dict.Count == 1) {
				return;
			}
			var ToBeDeleted = new List<T>();
			foreach (T key in Dict.Keys) {
				List<V> items = Dict[key];
				if (items.Count == 1)
					ToBeDeleted.Add(key);
			}
			foreach (T key in ToBeDeleted) {
				Dict.Remove(key);
			}
#if DBG_MSGS
			WriteLine("Removed {0:N0} singleton key(s) from Hashtable {1}. Size went from {2:N0} to {3:N0}.", 
				ToBeDeleted.Count, htname, ht.Count + ToBeDeleted.Count, ht.Count);
#endif
			return;
		}

//---------------------------------------------------------------------------------------

		void CalcShortCRCs(Dictionary<long, List<FileEntry>> DictSizes) {
			int n = 0;
			int nSizes = DictSizes.Keys.Count;
			toolStripProgressBar1.Maximum = nSizes;
			toolStripProgressBar1.Visible = true;
			var SortedKeys = DictSizes.Keys.OrderBy(key => key);
			foreach (long key in SortedKeys) {
				var items = DictSizes[key];
				toolStripStatusLabel1.Text = string.Format("Calculating short CRC {0} of {1} for filesize {2:N0} - files = {3}",
					++n, nSizes, key, items.Count);
				Application.DoEvents();
				// We may not be able to get the CRC if, for example, the file is
				// opened exclusively. If so, set the corresponding Dictionary entry
				// to null. Note that this has ramifications. Everywhere we process
				// FileEntry's, we'll have to check for null, and skip the entry.
				// TODO: Put null checks elsewhere in the code
				FileEntry ent;
				for (int i = 0; i < items.Count; ++i) {
					ent = items[i];
					try {
						ent.GetShortCRC(ShortCRCByteLength);
					} catch {
						items[i] = null;
					}
				}
				toolStripProgressBar1.Value = n;
			}
			toolStripProgressBar1.Value = 0;		// Reset once done
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
				DeleteSingletons<uint, FileEntry>(DictCRC, string.Format("htCRC - filesize = {0:N0}", key));
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
								// Where's a labelled break when you need one. Sigh.
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
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class FileEntry {
		public string	DirName;
		public string	FileName;		// Sans the path prefix (i.e. DirName)
		public uint		ShortCRC;		// The CRC of the beginning of the file. 

//---------------------------------------------------------------------------------------
		
		public FileEntry(string DirName, string FileName) {

			// We may well have the same directory name in many, many of these class
			// instances. Save on storage by interning the name, so that they all refer
			// to the same string object.
			this.DirName  = string.Intern(DirName);
			this.FileName = Path.GetFileName(FileName);
			ShortCRC	  = 0;
		}

//---------------------------------------------------------------------------------------

		public void GetShortCRC(int nBytes) {
			string filename = Path.Combine(DirName, FileName);
			using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read)) {
				byte [] bytes = new byte[nBytes];
				int n = fs.Read(bytes, 0, nBytes);	// n = # of bytes read (in case 
													// filesize < nBytes
				BartCRC crc = new BartCRC();
				crc.AddData(bytes, n);
				ShortCRC = crc.GetCRC();
			}
		}
	}
}
