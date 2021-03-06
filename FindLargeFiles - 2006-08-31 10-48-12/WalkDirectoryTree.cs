using System;
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

		DirectoryInfo	InitialDir;
		string			DirFilterName;
		string			FileFilterName;
		ProcessDir		pd;
		DirError		de;

//---------------------------------------------------------------------------------------

		public WalkDirectoryTree(string StartingDir, string DirFilterName, string FileFilterName, 
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
		}

//---------------------------------------------------------------------------------------

		public void Walk() {
			WalkInternal(InitialDir);
		}

//---------------------------------------------------------------------------------------

		void WalkInternal(DirectoryInfo di) {
			string		DirName = "";
			DirectoryInfo [] dirs  = null;
			FileInfo []		 files = null;
			bool			 bOK;
			try {
				dirs	= di.GetDirectories(DirFilterName);
				files	= di.GetFiles(FileFilterName);
				bOK = pd(di, dirs, files);
				if (! bOK) {
					// TODO: Doesn't  work more than one level down
					return;
				}

				foreach (DirectoryInfo dir in dirs) {
					DirName = dir.FullName;			// For catch clause
					WalkInternal(dir);
				}
			} catch (Exception e) {
				bOK = de(di, dirs, files, e);
				if (! bOK) {
					return;
					// TODO: See comment above for bOK
				// Console.Error.WriteLine("Exception '{0}' walking directory tree. Current directory = {1}",
				//	e.Message, DirName);
				// MsgBox, but tacitly ignore for now
				// TODO: Look into this in more detail later
			}
		} 
	}
}
