using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GrabPix {
	public partial class GrabPix : Form {

//---------------------------------------------------------------------------------------

		public GrabPix() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void GrabPix_Load(object sender, EventArgs e) {
			this.Visible = false;
			GetPics();
			// MessageBox.Show("GrabPix shutting down");	// TODO:
			Application.Exit();
		}

//---------------------------------------------------------------------------------------

		private void GetPics() {
			var pixDir = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
			var curDir = Path.Combine(Application.StartupPath, "LRSDat");
			CopyAll(new DirectoryInfo(pixDir), new DirectoryInfo(curDir));
		}

//---------------------------------------------------------------------------------------

		public static void CopyAll(DirectoryInfo source, DirectoryInfo target) {
			// Check if the target directory exists, if not, create it.
			if (Directory.Exists(target.FullName) == false) {
				Directory.CreateDirectory(target.FullName);
			}

			// Copy each file into it’s new directory.
			foreach (FileInfo fi in source.GetFiles()) {
				Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
				fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
			}

			// Copy each subdirectory using recursion.
			foreach (DirectoryInfo diSourceSubDir in source.GetDirectories()) {
				DirectoryInfo nextTargetSubDir =
					target.CreateSubdirectory(diSourceSubDir.Name);
				CopyAll(diSourceSubDir, nextTargetSubDir);
			}
		}
	}
}
