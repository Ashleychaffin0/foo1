// Copyright (c) 2008 by Bartizan Connects, LLC

using System;
using System.Windows.Forms;
using System.IO;

namespace LLConcurrencyTest {
	public partial class ImporterDefinition : Form {
		public string	Filename;
		public string	UserID;
		public string	TerminalID;

		public bool		DataIsExpanded;
		public bool		IgnoreFirstRecord;

		public int		MinRecsPerImport, MaxRecsPerImport;
		public int		ImportSleepMin, ImportSleepMax;

//---------------------------------------------------------------------------------------

		public ImporterDefinition() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void ImporterDefinition_FormClosing(object sender, FormClosingEventArgs e) {
			if (this.DialogResult == DialogResult.Cancel)
				return;

			Filename	= txtFilename.Text.Trim();
			UserID		= txtUserID.Text.Trim();
			TerminalID	= txtTerminalID.Text.Trim();
			
			DataIsExpanded = chkDataIsExpanded.Checked;
			IgnoreFirstRecord = chkIgnoreFirstRecord.Checked;

			if (! CheckFields()) {
				e.Cancel = true;
				return;
			}
		}

//---------------------------------------------------------------------------------------

		private bool CheckFields() {
			bool	bAllOK = (Filename.Length > 0) && (UserID.Length > 0) && (TerminalID.Length > 0);
			if (! bAllOK) {
				MessageBox.Show("Please fill in all the fields.", "LLConcurrency Test");
				return false;
			}

			bool	bOK = true;
			// A bit inefficient, sure, but I'm not going to sweat at most a microsecond
			bOK		&= int.TryParse(txtMinRecsPerImport.Text, out MinRecsPerImport);
			bOK		&= int.TryParse(txtMaxRecsPerImport.Text, out MaxRecsPerImport);
			bOK		&= int.TryParse(txtMinSecsBetweenImports.Text, out ImportSleepMin);
			bOK		&= int.TryParse(txtMaxSecsBetweenImports.Text, out ImportSleepMax);
			if (! bOK) {
				MessageBox.Show("Please make sure that all numeric fields are numeric", "LLConcurrency Test");
				return false;
			}

			if (MinRecsPerImport == 0) {
				MessageBox.Show("Minimum Records Per Import cannot be zero", "LLConcurrency Test");
				return false;
			}

			// Note; ImportSleepMin of zero is OK. Thread.Sleep(0) just yields the CPU

			// TODO: Other checking, such as min < max, unless max = 0, which means no
			//		 max.
			return true;
		}

//---------------------------------------------------------------------------------------

		private void btnBrowseFilename_Click(object sender, EventArgs e) {
			OpenFileDialog	ofd = new OpenFileDialog();
			ofd.CheckFileExists = true;
			ofd.Filter = "Text files|*.txt|All files|*.*";
			DialogResult res = ofd.ShowDialog();
			if (res != DialogResult.OK) {
				return;
			}
			txtFilename.Text = ofd.FileName;

			// Try to save some keystrokes. Scan the penultimate node for a Terminal
			// ID. Assume it is of the form *RTxxxx.*
			
			string fn = Path.GetFileNameWithoutExtension(ofd.FileName);
			if (fn.Length < 6) {
				return;
			}
			string TermID = fn.Substring(fn.Length - 6).ToUpper();
			if (! TermID.StartsWith("RT")) {
				return;
			}
			txtTerminalID.Text = TermID;
		}
	}
}