using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace LRS {

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class LargeFileEntry : IComparable {
		public string	filename;
		public long		size;

		public LargeFileEntry(FileInfo fi) {
			filename = fi.FullName;
			size     = fi.Length;
		}

		#region IComparable Members
		public int CompareTo(object obj) {
			LargeFileEntry	other = obj as LargeFileEntry;
			if (size > other.size)
				return 1;
			if (size < other.size)
				return -1;
			// File sizes are the same. Sort on filename, case insensitive.
			return string.Compare(this.filename, other.filename, true);
		}
		#endregion
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// Summary description for FindLargeFiles.
	/// </summary>
	class FindLargeFiles {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		///
 
		public bool				OK = true;

		long					MinFileSize = 0;

		WalkDirectoryTree		wdt;

		List<LargeFileEntry>	LargeFiles;

//---------------------------------------------------------------------------------------

		public FindLargeFiles(string [] args) {
			if (args.Length != 2) {
				Usage();
				OK = false;
				return;
			}

			// TODO: Verify that MinFilesize is numeric
			MinFileSize = int.Parse(args[1]) * 1000000;
			
			WalkDirectoryTree.ProcessDir pd  = new WalkDirectoryTree.ProcessDir(MyProcessDir);
			WalkDirectoryTree.DirError   de  = new WalkDirectoryTree.DirError(MyDirError);
			LargeFiles = new List<LargeFileEntry>();
			// wdt = new WalkDirectoryTree(args[0], args[2], args[3], pd);
			wdt = new WalkDirectoryTree(args[0], "*", "*", pd, de);
		}

//---------------------------------------------------------------------------------------

		void Usage() {
			Console.WriteLine("Usage: FindLargeFiles dirname minfilesize [in MB] (e.g. FindLargeFiles C:\\ 10)");
		}

//---------------------------------------------------------------------------------------

	public void Run() {
		try {
			if (! wdt.OK) {
				return;
			}
			Console.Error.WriteLine("Files  Dirs  Directory");
			wdt.Walk();
			Console.WriteLine();
			Console.Error.WriteLine();
			long TotalDirSize = SumSubDirs(wdt.DirEntries[0]);
			ShowLargeFiles();
			ShowDirectorySizes();
		} catch (Exception e) {
			Console.WriteLine("A fatal error occured during FindLargeFiles processing. The program will now "
				+ "terminate. The error message is {0}.\n\nThe traceback is {1}", 
				e.Message, e.ToString());
		}
	}

//---------------------------------------------------------------------------------------

		private void ShowLargeFiles() {
			LargeFiles.Sort();
			long TotalSize = 0;
			// Display files in decreasing size order
			for (int i = LargeFiles.Count - 1; i >= 0; --i) {
				LargeFileEntry lfe = (LargeFileEntry)LargeFiles[i];
				Console.WriteLine("{0,12:N0} {1}", lfe.size, lfe.filename);
				TotalSize += lfe.size;
			}
			Console.WriteLine("-------------------  Total");
			Console.WriteLine("{0,19:N0}", TotalSize);
		}

//---------------------------------------------------------------------------------------

		private void ShowDirectorySizes() {
			ShowSingleDirSizes(wdt.DirEntries[0], 0);
			ShowDirSizes(wdt.DirEntries[0], 0);
		}

//---------------------------------------------------------------------------------------

		private void ShowDirSizes(WalkDirectoryTree.DirectoryEntry des, int i) {
			foreach (WalkDirectoryTree.DirectoryEntry de in des.SubDirs) {
				ShowSingleDirSizes(de, i + 1);
				ShowDirSizes(de, i + 1);
			}
		}

//---------------------------------------------------------------------------------------

		private void ShowSingleDirSizes(WalkDirectoryTree.DirectoryEntry di, int i) {
			string		DirName;
			DirName = di.DirInfo.Name;
			long		DirSize, TotDirSize;
			const	long	MB1 = 1024 * 1024;
			DirSize = (di.SizeOfDir + MB1 - 1) / (1024 * 1024);		// In MB, rounded
			TotDirSize = (di.SizeOfDirPlusSubdirs + MB1 - 1) / (1024 * 1024);
			Console.Write("{0} ", "".PadRight(i * 4));
			Console.WriteLine("{0} {1:N0}MB {2:N0}MB", DirName, DirSize, TotDirSize);
		}

//---------------------------------------------------------------------------------------

		private long SumSubDirs(WalkDirectoryTree.DirectoryEntry DirEntry) {
			Console.WriteLine("SumSubDirs: {0:N0} / {1:N0} - {2}", DirEntry.SizeOfDir, DirEntry.SizeOfDirPlusSubdirs, DirEntry.DirInfo.FullName);
			long	DirSize = DirEntry.SizeOfDir;
			foreach (WalkDirectoryTree.DirectoryEntry de in DirEntry.SubDirs) {
				DirSize += SumSubDirs(de);
			}
			DirEntry.SizeOfDirPlusSubdirs = DirSize;
			return DirSize;
		}


//---------------------------------------------------------------------------------------

		bool MyProcessDir(DirectoryInfo CurDir, DirectoryInfo [] dirs, FileInfo [] files) {
			string s = CurDir.FullName;
			// Console.Error.Write("\r{0,5}  {1,4}  {2}", files.Length, dirs.Length, s);
#if false
			// Console.WriteLine("\nCurrent directory is {0}", CurDir.FullName);
			foreach (DirectoryInfo di in dirs) 
				; // Console.WriteLine("*Dir {0}", di.FullName);
#endif
			foreach (FileInfo fi in files) {
				if (fi.Length > MinFileSize) {
					// Console.WriteLine("    {0,12:N0} {1}", fi.Length, fi.FullName);
					LargeFiles.Add(new LargeFileEntry(fi));
				}
			}
			return true;
		}


//---------------------------------------------------------------------------------------

		bool MyDirError(DirectoryInfo CurDir, DirectoryInfo[] dirs, FileInfo[] files, Exception e) {
			Console.WriteLine("Error on dir {0}", CurDir.Name);	// TODO:
			return true;
		}

//---------------------------------------------------------------------------------------

		[STAThread]
		static void Main(string[] args)	{
		// TODO: Delete this comment - VSS testing 1
		// TODO: Delete this too - VSS testing 2
#if DEBUG
			// args = new string[] {@"C:\LRS\C#", "0"};
			args = new string[] {@"D:\", "0"};
#endif
			FindLargeFiles	flf = new FindLargeFiles(args);
			if (flf.OK)
				flf.Run();
		}
	}
}
