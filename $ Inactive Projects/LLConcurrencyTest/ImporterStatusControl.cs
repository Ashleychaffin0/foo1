// Copyright (c) 2008 by Bartizan Connects, LLC

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace LLConcurrencyTest {

	public delegate	void delSetStatusMsg(string msg);
	public delegate void delSetProgressBarMinMax(int min, int max);
	public delegate void delSetProgressBarValue(int value);
	public delegate void delDoEvents();

	public partial class ImporterStatusControl : UserControl {
		[XmlIgnore]
		public bool		InitOK;

		public string	Filename;
		public string	UserID;
		public string	TerminalID;

		public bool		DataIsExpanded;
		public bool		IgnoreFirstRecord;

		// If we have the IgnoreFirstLine switch on, but aren't sending the entire file,
		// we have to remember the first line and prepend it to any subsequent sections
		public string	FirstLine;

		public int		MinRecsPerImport, MaxRecsPerImport;
		public int		ImportSleepMin, ImportSleepMax;		// In milliseconds

		[XmlIgnore]
		public List<string>	FileLines;

//---------------------------------------------------------------------------------------

		public ImporterStatusControl() {
			InitializeComponent();

			InitOK = false;			// Assume the worst
			ImporterDefinition def = new ImporterDefinition();
			DialogResult res = def.ShowDialog();
			if (res != DialogResult.OK) {
				return;
			}

			lblFilename.Text = Filename		= def.Filename.Trim();
			lblTermID.Text	 = TerminalID	= def.TerminalID.Trim();
			lblUserID.Text	 = UserID		= def.UserID.Trim();

			chkDataIsExpanded.Checked	 = DataIsExpanded	 = def.DataIsExpanded;
			chkIgnoreFirstRecord.Checked = IgnoreFirstRecord = def.IgnoreFirstRecord;

			MinRecsPerImport = def.MinRecsPerImport;
			MaxRecsPerImport = def.MaxRecsPerImport;
			ImportSleepMin	 = def.ImportSleepMin;
			ImportSleepMax	 = def.ImportSleepMax;

			lblRecsPerImportMin.Text		= MinRecsPerImport.ToString();
			lblRecsPerImportMax.Text		= MaxRecsPerImport.ToString();
			lblSleepBetweenImportsMin.Text	= ImportSleepMin.ToString();
			lblSleepBetweenImportsMax.Text	= ImportSleepMax.ToString();

			try {
				FileLines = new List<string>();
				string FileContents = File.ReadAllText(Filename);
				// As in the real importer, we have to get rid of all \r's. Still don't
				// know why. But do so. And rather than just doing .Replace, we'll do it
				// ourselves, and count the number of lines while we're at it. Also we'll
				// delete any empty rows.
				StringBuilder	sb = new StringBuilder(500);	// 500 = arbitrary
				foreach (char c in FileContents) {
					if (c == '\r') {
						continue;
					}
					if (c == '\n') {
						string	s = sb.ToString();
						if (s.Length == 0) {
							continue;
						}
						FileLines.Add(sb.ToString());
						sb.Length = 0;
						continue;
					}
					sb.Append(c);
				}
				// Check for last line not \n-terminated
				if (sb.Length > 0) {
					FileLines.Add(sb.ToString());
				}
			} catch (Exception ex) {
				string msg = string.Format("Unable to read contents of file '{0}'. Exception info = {1}",
					Filename, ex.ToString());
				MessageBox.Show(msg, "LLConcurrency Test");
				return;
			}

			if (FileLines.Count == 0) {
				string	msg = string.Format("File '{0}' is empty or has no non-empty lines in it. File rejected.", Filename);
				return;
			}

			if (IgnoreFirstRecord) {
				FirstLine = FileLines[0];
				FileLines.RemoveAt(0);			// Remove first line. We'll append it back later
			}

			lblNRecs.Text = FileLines.Count.ToString();

			progressBar1.Minimum = 0;
			progressBar1.Maximum = FileLines.Count;
			progressBar1.Step = 1;

			InitOK = true;
		}

//---------------------------------------------------------------------------------------

		public void SetProgressBarMinMax(int min, int max) {
			if (progressBar1.InvokeRequired) {
				delSetProgressBarMinMax del =
					delegate(int delmin, int delmax) {
						SetProgressBarMinMax(delmin, delmax);
					};
				progressBar1.BeginInvoke(del, min, max);
			} else {
				progressBar1.Minimum = min;
				progressBar1.Maximum = max;
				// Application.DoEvents();
			}
		}

//---------------------------------------------------------------------------------------

		public void SetProgressBarProgress(int value) {
			if (progressBar1.InvokeRequired) {
				delSetProgressBarValue del = 
					delegate(int delvalue) {
						SetProgressBarProgress(delvalue);
					};
				progressBar1.BeginInvoke(del, value);
			} else {
			progressBar1.Value = value;
				// Application.DoEvents();
			}
		}

//---------------------------------------------------------------------------------------

		public void SetStatusMsg(string msg) {
			if (lblStatMsg.InvokeRequired) {
				delSetStatusMsg del = delegate(string delmsg) {
					SetStatusMsg(delmsg);
				};
				lblStatMsg.BeginInvoke(del, msg);
			} else {
				lblStatMsg.Text = msg;
#if false
				if (res != null) {
					lblStatMsg.EndInvoke(res);
				}
#endif
			}
		}

//---------------------------------------------------------------------------------------

		public void DoEvents() {
			if (this.InvokeRequired) {
				delDoEvents del = delegate() { DoEvents(); };
				this.BeginInvoke(del);
			} else {
				Application.DoEvents();
			}
		}
	}
}
