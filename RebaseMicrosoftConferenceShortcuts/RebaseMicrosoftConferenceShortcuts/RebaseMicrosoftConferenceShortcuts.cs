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

namespace RebaseMicrosoftConferenceShortcuts {
	public partial class RebaseMicrosoftConferenceShortcuts : Form {
		public RebaseMicrosoftConferenceShortcuts() {
			InitializeComponent();

			string[] Drives = Environment.GetLogicalDrives();
			cmbFromDrive.Items.AddRange(Drives);
			cmbFromDrive.SelectedIndex = 0;

			cmbToDrive.Items.AddRange(Drives);
			cmbToDrive.SelectedIndex = 0;

			txtTargetDir.Text = @"D:\MsConferences\Build 2016-yyy";
		}

//---------------------------------------------------------------------------------------

		private void btnBrowse_Click(object sender, EventArgs e) {
			MessageBox.Show("Donce");
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			// TODO: Do error checking. txtbox not empty; different from/to drives;
			//		 txtbox is actually the name of a directory (not file, not garbage)
			string FromDrive = cmbFromDrive.Text.Replace('\\', '/');
			string ToDrive = cmbToDrive.Text.Replace('\\', '/');
			var dir = new DirectoryInfo(txtTargetDir.Text);
			foreach (var file in dir.EnumerateFileSystemInfos("*.url", SearchOption.AllDirectories)) {
				Process(file.FullName, FromDrive, ToDrive);
			}
			MessageBox.Show("Done");
		}

//---------------------------------------------------------------------------------------

		private void Process(string fullName, string FromDrive, string ToDrive) {
			string s;
			using (var sr = new StreamReader(fullName)) {
				s = sr.ReadToEnd();
				Console.WriteLine(s);
			}
			if (s.Contains(FromDrive)) {
				s = s.Replace(FromDrive, ToDrive);
				using (var sw = new StreamWriter(fullName)) {
					sw.Write(s);
				}
			}
		}
	}
}
