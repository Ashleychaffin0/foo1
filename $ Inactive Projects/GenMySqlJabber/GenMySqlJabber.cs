using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace GenMySqlJabber {
	public partial class GenMySqlJabber : Form {
		public GenMySqlJabber() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void btnBrowseInputFilename_Click(object sender, EventArgs e) {
			OpenFileDialog	ofd = new OpenFileDialog();
			ofd.FileName = "JabberInfo.txt";
			ofd.RestoreDirectory = true;
			DialogResult res = ofd.ShowDialog();
			if (res != DialogResult.OK) {
				return;
			}
			txtInputFilename.Text = ofd.FileName;
		}

//---------------------------------------------------------------------------------------

		private void btnBrowseOutputFilename_Click(object sender, EventArgs e) {
			OpenFileDialog	ofd = new OpenFileDialog();
			ofd.FileName = "Sql58.sql";
			ofd.RestoreDirectory = true;
			ofd.CheckFileExists = false;
			DialogResult res = ofd.ShowDialog();
			if (res != DialogResult.OK) {
				return;
			}
			txtOutputFilename.Text = ofd.FileName;
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			StreamReader sr = new StreamReader(txtInputFilename.Text);
			StreamWriter sw = new StreamWriter(txtOutputFilename.Text);
			string	line;
			char [] sep = new char[] { ',' };
			string	InsertFmt = @"INSERT INTO `jabberd2`.`roster-items`
	(`collection-owner`, `jid`, `name`, `to`, `from`, `Ask`)
	Values('{0}', '{1}', '{2}', {3}, {4}, {5});
";
			while ((line = sr.ReadLine()) != null) {
				if (line.Trim().Length == 0) {
					continue;
				}
				string []	fields = line.Split(sep);
				// Assume fields are 
				//	0) From Jabber ID (e.g. Dan@jabber.imprinters.com)
				//	1) To Jabber ID (e.g. James@jabber.imprinters.com)
				//	2) To Name (e.g. James)
				//	3) From
				//	4) To
				//	5) Ask
				if (fields.Length != 6) {
					MessageBox.Show("Line in error is " + line, "Records must have exactly 6 fields",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					continue;	// Ignore line
				}
				sw.WriteLine(InsertFmt, fields[0], fields[1], fields[2], fields[3],
					fields[4], fields[5]);
			}
			sw.Close();
			sr.Close();
			MessageBox.Show("Done");
		}
	}
}