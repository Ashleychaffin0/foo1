using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FloppyCopy2 {
	public partial class FloppyCopy2 : Form {

		int NextDirNo = 0;
		string TargetDir = @"C:\LRS\FloppyCopy2\";

//---------------------------------------------------------------------------------------

		public FloppyCopy2() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private string SetNextDirNo() {
			string path = "";
			while (true) {
				string DirName = string.Format("Floppy-{0:D4}", NextDirNo);
				path = Path.Combine(TargetDir, DirName);
				if (Directory.Exists(path)) {
					++NextDirNo;
					continue;
				} else {
					Directory.CreateDirectory(path);
					return path;
				}
			}
		}

//---------------------------------------------------------------------------------------

		private string DirNo() {
			return Path.Combine(TargetDir, string.Format("Floppy-{0:D4}\\", NextDirNo));
		}

//---------------------------------------------------------------------------------------

		private void FloppyCopy2_Load(object sender, EventArgs e) {
		}

//---------------------------------------------------------------------------------------

		private void btnCopy_Click(object sender, EventArgs e) {
			SetNextDirNo();
			Process p = Process.Start("xcopy.exe", @"/s /e A:\ " + DirNo());
			this.Cursor = Cursors.WaitCursor;
			try {
				p.WaitForExit();
			} finally {
				this.Cursor = Cursors.Arrow;
			}
		}
	}
}
