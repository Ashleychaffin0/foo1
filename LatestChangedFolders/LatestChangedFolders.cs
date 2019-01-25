using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LatestChangedFolders {
	public partial class LatestChangedFolders : Form {
		List<MyDirInfo> DirInfos;

		public LatestChangedFolders() {
			InitializeComponent();
			DirInfos = new List<MyDirInfo>();
			string BaseDir = @"G:\lrs\$Dev";
			foo(BaseDir);
			DirInfos.Sort((fi1, fi2) => fi2.LastModified.CompareTo(fi1.LastModified));
			DumpRecentDirs(DirInfos);
		}

//---------------------------------------------------------------------------------------

		private void DumpRecentDirs(List<MyDirInfo> dirInfos) {
			var LastDate = new DateTime(2017, 11, 1);
			foreach (var fi in DirInfos) {
				if (fi.LastModified < LastDate) return;
				Console.WriteLine($"{fi.LastModified} -- {fi.DirName}");
			}
		}

//---------------------------------------------------------------------------------------

		private void foo(string BaseDir) {
			foreach (var dir in Directory.EnumerateDirectories(BaseDir, "*", SearchOption.AllDirectories)) {
				if (!IsDirWeWant(dir)) continue;
				// Console.WriteLine(dir);
				var fis = from fn in Directory.EnumerateFiles(dir)
						  // where IsDirWeWant(dir)
						  select new { fn, lwt = new FileInfo(Path.Combine(dir, fn)).LastWriteTime };
				var Latest = (from fi in fis
							  // Note: A number of Andoid directories have lwt's >= 2040
							  where fi.lwt.Year < 2040
							  orderby fi.lwt descending
							  select new { fi.fn, fi.lwt }).FirstOrDefault();
				if (Latest != null) {
					DirInfos.Add(new MyDirInfo(dir, Latest.fn, Latest.lwt));
				}
			}
		}

//---------------------------------------------------------------------------------------

		private bool IsDirWeWant(string dir) {
			if (dir.Contains("\\obj")) return false;
			if (dir.Contains("\\bin")) return false;
			if (dir.EndsWith("debug")) return false;
			if (dir.EndsWith("release")) return false;
			if (dir.Contains("\\Properties")) return false;
			if (dir.Contains("\\packages")) return false;
			if (dir.Contains("\\.vs")) return false;
			if (dir.Contains("\\.git")) return false;
			if (dir.Contains(".git\\")) return false;
			if (dir.Contains("\\.metadata")) return false;
			if (dir.Contains("\\Android")) return false;	// May be \\Android3 etc
			if (dir.Contains(".Droid\\")) return false;	// May be \\Android3 etc
			if (dir.Contains(".Android\\")) return false;	// May be \\Android3 etc
			if (dir.Contains("\\arm")) return false;	// May be \\Android3 etc
			// There are a couple more for UWP apps; worry about them later
			if (dir.Contains("\\LatestChangedFolders")) return false;
			return true;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class MyDirInfo {
		public string	DirName;
		public string	FileName;
		public DateTime LastModified;

//---------------------------------------------------------------------------------------

		public MyDirInfo(string DirName, string FileName, DateTime LastModified) {
			this.DirName      = DirName;
			this.FileName     = FileName;
			this.LastModified = LastModified;
		}
	}
}
