using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

// TODO: 
//	* QuitSwitch - User set to tell us to shut up and go away.
//	* Use enum SearchOption?
//	* Lambda support for filtering
//	* An Empty/Dispose method to free up mem?
//	* Support <recurse> option.

//---------------------------------------------------------------------------------------

namespace DirectoryWalker_2 {

	public static class DirectoryWalkerExtensions {

		public static IEnumerable<FileInfo> LrsGetFileInfo(this DirectoryWalker dw) {
			// <i> is a dummy variable. Can't seem to have an extension method without it
			FileInfo[] fis = null;
			try {
				fis = dw.DirInfo.GetFiles();
			} catch (Exception) {
				yield break;
				;
			}
			foreach (var fi in fis) {
				yield return fi;
			}
		}

	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class DirectoryWalker {

		protected string		PathName;
		protected bool			bRecurse;

		// TODO: Make properties?
		// public object			UserData;
		public bool				IsDirectory;
		public DirectoryInfo	DirInfo;
		// Next files only defined if IsDirectory is false
		public FileInfo			FileInfo;
		public bool				IsFirst, IsLast;
		public int				Level;

//---------------------------------------------------------------------------------------

		public DirectoryWalker(string PathName) {
			CtorFull(PathName, true);
		}

//---------------------------------------------------------------------------------------

		public DirectoryWalker(string PathName, bool bRecurse) {
			CtorFull(PathName, bRecurse);
		}

//---------------------------------------------------------------------------------------

		private void CtorFull(string PathName, bool bRecurse) {
			this.PathName = PathName;
			this.bRecurse = bRecurse;

			if (! Directory.Exists(PathName)) {
				throw new ArgumentException("Path name (" + PathName + ") does not exist");
			}
		}

//---------------------------------------------------------------------------------------

		public IEnumerable<DirectoryWalker> Walk() {
			var di = new DirectoryInfo(PathName);
			return ProcessWalk(di, 0);
		}

//---------------------------------------------------------------------------------------

		private IEnumerable<DirectoryWalker> ProcessWalk(DirectoryInfo di, int Level) {
			var dw = new DirectoryWalker(di.FullName);
			InitDirectoryWalker(dw, di, true, null, 0, Level);
			yield return dw;

			FileInfo[] files = null;
			try {
				files = di.GetFiles();
			} catch {
				yield break;
			}
			for (int n = 0; n < files.Length; ++n) {
				// Note: We *could* just reinitialize <dw>, which would save a bit of
				// overhead (i.e. object creation and later garbage collection). But if
				// we did that, the user couldn't stash the instances away for
				// subsequent processing of his/her own. So we'll live with the
				// overhead.
				dw = new DirectoryWalker(di.FullName);
				InitDirectoryWalker(dw, di, false, files, n, Level);
				yield return dw;
			}

			var dirs = di.GetDirectories();
			foreach (var dir in dirs) {
				dw = new DirectoryWalker(dir.FullName);
				InitDirectoryWalker(dw, dir, true, null, 0, Level);
				foreach (var item in ProcessWalk(dir, Level + 1)) {
					yield return item;
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void InitDirectoryWalker(DirectoryWalker dw, DirectoryInfo di, bool IsDirectory, FileInfo[] fi,
			int n, int Level) {
				// TODO: Handle UserData
			dw.Level		= Level;
			dw.IsDirectory	= IsDirectory;
			dw.DirInfo		= di;
			if (! IsDirectory) {
				dw.FileInfo = fi[n];
				dw.IsFirst	= n == 0;
				dw.IsLast	= n == fi.Length - 1;
			}
		}
	}
}
