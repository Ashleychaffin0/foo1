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

namespace ReadPerdr {

	public partial class ReadPerdr : Form {

		Dictionary<uint	/*  Address */, string   /*  Name */> PeReaderDlls;

//---------------------------------------------------------------------------------------
		public ReadPerdr() {
			InitializeComponent();

			txtFilename.Text = @"D:\Downloads\perdr.0-0108b.win32\win\perdr-LRS.asm";

			PeReaderDlls = new Dictionary<uint, string>();
		}

//---------------------------------------------------------------------------------------

		private void btnBrowseFilename_Click(object sender, EventArgs e) {
			openFileDialog1.Filter = "asm|*.asm|(All files)|*.*";
			openFileDialog1.ValidateNames = true;
			var dr = this.openFileDialog1.ShowDialog();
			if (dr == DialogResult.OK) {
				txtFilename.Text = openFileDialog1.FileName;
			}
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			btnGo.Enabled = false;
			using (var sr = new StreamReader(txtFilename.Text)) {
				ReadImports(sr);
			}
			btnGo.Enabled = true;
		}

//---------------------------------------------------------------------------------------

		private void ReadImports(StreamReader sr) {
			string line = SkipInputUntil(sr, "++++++++++++++++++++++++++++++++ IMPORTS +++++++++++++++++++++++++++++++++");
			if (line == null) {
				// I suppose it's valid not to have an Imports section. So be it.
				// Note: If this were a real program, I'd probably make it an option to
				//		 require the Imports section.
			} else {
				ReadDlls(sr);
			}
		}

//---------------------------------------------------------------------------------------

		private void ReadDlls(StreamReader sr) {
			string line = SkipInputUntil(sr, "DLL: ");
			if (line == null) {
				return;
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Looks for lines that begin with the specified string, then returns that line
		/// </summary>
		/// <param name="sr">The StreamReader to read from</param>
		/// <param name="Prefix">If we're looking for a line that says, oh, "DLL: ", then
		/// that's what we'll send in.</param>
		/// <returns>The full line read if the prefix is found, else null</returns>
		private string SkipInputUntil(StreamReader sr, string Prefix) {
			string line;
			while ((line = sr.ReadLine()) != null) {
				if (line.StartsWith(Prefix)) {
					return line;
				}
			}
			return null;
		}
	}
}
