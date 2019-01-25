using System;
using System.Collections.Generic;
using System.IO;

namespace LRS {
	/// <summary>
	/// Summary description for WalkDirectoryTree.
	/// </summary>
	public class WalkDirectoryTree {

		public delegate bool ProcessDir(DirectoryInfo di, DirectoryInfo [] dirs, FileInfo [] files);
		public delegate bool DirError(DirectoryInfo di, DirectoryInfo [] dirs, FileInfo [] files, Exception e);

		bool			_OK = true;
		public bool		OK {
			get { return _OK; }
			set { _OK = value; }
		}

		public List<DirectoryEntry> DirEntries;
		DirectoryInfo		 InitialDir;
		string				 DirFilterName;
		string				 FileFilterName;
		ProcessDir			 pd;
		DirError			 de;

//---------------------------------------------------------------------------------------

		public WalkDirectoryTree(string StartingDir, 
								 string DirFilterName, string FileFilterName, 
								 ProcessDir pd, DirError de) {
			// TODO: Should the delegate be part of the ctor, or the Walk method?
			// TODO: Ditto for StartingDir and FilterName
			if (! Directory.Exists(StartingDir)) {
				OK = false;
				return;
			}
			this.InitialDir		= new DirectoryInfo(StartingDir);
			this.DirFilterName	= DirFilterName;
			this.FileFilterName	= FileFilterName;
			this.pd				= pd;
			this.de				= de;

			this.DirEntries		= new List<DirectoryEntry>();
			this.DirEntries.Add(new DirectoryEntry(InitialDir));
		}

//---------------------------------------------------------------------------------------

		public void Walk() {
			WalkInternal(InitialDir, DirEntries[0]);
		}

//---------------------------------------------------------------------------------------

		void WalkInternal(DirectoryInfo di, DirectoryEntry Dir) {
			string			 DirName = "";
			DirectoryInfo [] dirs  = null;
			FileInfo []		 files = null;
			bool			 bOK;
			try {
				dirs	= di.GetDirectories(DirFilterName);
				files	= di.GetFiles(FileFilterName);
				foreach (FileInfo fi in files) {
					Dir.SizeOfDir += fi.Length;
				}
				bOK = pd(di, dirs, files);
				if (! bOK) {
					// TODO: Doesn't  work more than one level down
					return;
				}

				foreach (DirectoryInfo dir in dirs) {
					DirectoryEntry	SubDir = new DirectoryEntry(dir);
					Dir.SubDirs.Add(SubDir);
					DirName = dir.FullName;			// For catch clause
					WalkInternal(dir, SubDir);
				}
			} catch (Exception e) {
				bOK = de(di, dirs, files, e);
				if (! bOK)
					return;
					// TODO: See comment above for bOK
				// Console.Error.WriteLine("Exception '{0}' walking directory tree. Current directory = {1}",
				//	e.Message, DirName);
				// MsgBox, but tacitly ignore for now
				// TODO: Look into this in more detail later
			}
		}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

		// Internal class
		public class DirectoryEntry : IComparable {
			public DirectoryInfo		DirInfo;
			public long					SizeOfDir, SizeOfDirPlusSubdirs;
			public List<DirectoryEntry> SubDirs;

			public DirectoryEntry(DirectoryInfo di) {
				DirInfo = di;
				SizeOfDir = 0;
				SizeOfDirPlusSubdirs = 0;
				SubDirs = new List<DirectoryEntry>();
			}

			#region IComparable Members
			public int CompareTo(object obj) {
				DirectoryEntry other = obj as DirectoryEntry;
				if (SizeOfDirPlusSubdirs > other.SizeOfDirPlusSubdirs)
					return 1;
				if (SizeOfDirPlusSubdirs < other.SizeOfDirPlusSubdirs)
					return -1;
				// File sizes are the same. Sort on filename, case insensitive.
				return string.Compare(this.DirInfo.FullName, other.DirInfo.FullName, true);
			}
			#endregion
		}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	}
}
