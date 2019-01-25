// Copyright (c) 2013-2014 by Larry Smith
// Last updated, July 2014

//	*	Modified to use Delimon to support long (32K) path names

using System.Collections.Generic;
using System.IO;

using Delimon;

// http://gallery.technet.microsoft.com/DelimonWin32IO-Library-V40-7ff6b16c

namespace DirectoryWalker {
	class DirectoryWalker {
		string[]	IncludedDirNames;
		string[]	IncludedFiletypes;

#if false	// TODO:
		class DirectoryWalkerInfo {
			// public DirectoryInfo di;
			public FileInfo fi;
			// public Path DirName;
			// public string Filename;
			// public bool bIsDirectory;
		}
#endif

//---------------------------------------------------------------------------------------

		public DirectoryWalker () {
			IncludedDirNames = new string[0];
			IncludedFiletypes = new string[0];
		}

//---------------------------------------------------------------------------------------

		public DirectoryWalker SetIncludedDirectoriess(params string[] DirNames) {
			IncludedDirNames = DirNames;
			return this;
		}

//---------------------------------------------------------------------------------------

		public DirectoryWalker SetIncludedFileTypes(params string[] FileTypes) {
			IncludedFiletypes = FileTypes;
			return this;
		}

//---------------------------------------------------------------------------------------

		public IEnumerable<Delimon.Win32.IO.FileInfo> Walk(params string[] DirNames) {
			// TODO: Should return IEnumerable<DirectoryWalkerInfo> ???
			string[] Dirs;
			if (DirNames.Length > 0) {
				Dirs = DirNames;
			} else {
				Dirs = IncludedDirNames; 
			}

			foreach (string DirName in Dirs) {

				foreach (var item in ProcessDir(DirName)) {
					yield return item;
				}
				// ProcessDir(DirName);

#if false
				foreach (var fname in Directory.EnumerateFiles(DirName)) {
					yield return new FileInfo(fname);
				}

				foreach (var dname in Directory.EnumerateDirectories(DirName)) {
					yield return new FileInfo(dname);
				}

				// yield return DirName;
#endif
			}
		}

//---------------------------------------------------------------------------------------

		private IEnumerable<Delimon.Win32.IO.FileInfo> ProcessDir(string DirName) {
			// FileInfo fi;
			Delimon.Win32.IO.FileInfo fi;
			foreach (var fname in Directory.EnumerateFiles(DirName)) {
				// = new FileInfo(fname);
				fi = new Delimon.Win32.IO.FileInfo(fname);
				// Console.WriteLine(fi.FullName + "    " + fi.Attributes.ToString());
				yield return fi;
			}

			foreach (var dname in Directory.EnumerateDirectories(DirName)) {
				fi = new Delimon.Win32.IO.FileInfo(dname);
				// Console.WriteLine(fi.FullName + "    " + fi.Attributes.ToString());
				yield return fi;
				foreach (var item in ProcessDir(dname)) {
					yield return item;
				}
				// ProcessDir(dname);
			}
		}
	}
}
