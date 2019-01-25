// Copyright (c) 2002 by Larry Smith

// TODO: Sort by date, not name
// TODO: LastDateSeen <> EndDate, but last date printed
// TODO: Add database support. Each puzzle, as printed, gets added to the database,
//		 and the listbox shows only those not already printed, unless the
//		 Show Previously Printed button is displayed.

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml.Serialization;
using System.Text;
using Microsoft.Win32;
using System.Drawing.Printing;

namespace PrintPuz
{

	[Serializable]
	// TODO: See if we can change this from public to, say, internal
	public class PuzParms {
		public string		PuzDir;
		public int			ShortDelay, LongDelay;
		public DateTime		LastDateSeen;
		public int			TimeSpan;
		public bool []		bDaysWanted;
		public bool			bShowOnlyCheckedFiles;

		public const int DefaultShortDelay = 1000;
		public const int DefaultLongDelay  = 2000;
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// Summary description for PrintPuz.
	/// </summary>
	public class PrintPuz : System.Windows.Forms.Form {
		#region	Definitions of controls on the form
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnEnd;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtDateSpan;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtShortDelay;
		private System.Windows.Forms.TextBox txtLongDelay;
		private System.Windows.Forms.CheckBox chkMonday;
		private System.Windows.Forms.CheckBox chkTuesday;
		private System.Windows.Forms.CheckBox chkWednesday;
		private System.Windows.Forms.CheckBox chkSaturday;
		private System.Windows.Forms.CheckBox chkFriday;
		private System.Windows.Forms.CheckBox chkThursday;
		private System.Windows.Forms.CheckBox chkSunday;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label txtPuzDir;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.CheckedListBox lbFiles;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.DateTimePicker calStartDate;
		private System.Windows.Forms.DateTimePicker calEndDate;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chkShowChecked;
		private System.Windows.Forms.ColumnHeader lbcolFilename;
		private System.Windows.Forms.ColumnHeader lbcolDOW;
		#endregion
		private System.Windows.Forms.ListView lvFiles;
		private System.Windows.Forms.ColumnHeader lbcolSelected;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		internal class FileInfo {
			public string	filename;
			public DateTime	date;
			public int		dow;			// Day of week, 0 = Mnday

			public FileInfo(string filename, DateTime date, int dow) {
				this.filename = filename;
				this.date	  = date;
				// Whoops. DayOfWeek goes from Sunday=0 to Saturday=6.
				// Sigh. Kludge it here.
				if (dow == 0)
					dow = 6;
				else
					--dow;
				// End of kludge
				this.dow	  = dow;
			}
		}

		[DllImport("User32.dll")]
		public static extern int BringWindowToTop(int HWND);

		static readonly string	PuzXMLFile = "PuzParms.xml";
		static DateTime	MagicDate = new DateTime(1949, 11, 24);	// Kludge for invalid date

		PuzParms	parms;
		bool		bSaveLastDateSeen = false;
		// The filenames listbox gets regenerated, a *lot* during startup.
		// Define a flag that will suppress this until we're good and ready.
		bool		bEnableFillBoxes = false;
		DateTime	StartDate, EndDate;

		ArrayList	fileinfo;

		int			nPrinters;

//---------------------------------------------------------------------------------------

		public PrintPuz() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			parms = LoadPuzParms(PuzXMLFile);

			// Set the form from the parms.
			// Note: The order of these are important. For example, you must set the
			// puzzle directory before setting the DateTimePicker controls, since the 
			// Changed event for these latter refer to it.
			txtPuzDir.Text			= parms.PuzDir;
			calStartDate.Value		= parms.LastDateSeen;
			txtDateSpan.Text		= parms.TimeSpan.ToString();
			txtShortDelay.Text		= parms.ShortDelay.ToString();
			txtLongDelay.Text		= parms.LongDelay.ToString();
			// I suppose we could/should do this as a control array...
			chkMonday.Checked		= parms.bDaysWanted[0];
			chkTuesday.Checked		= parms.bDaysWanted[1];
			chkWednesday.Checked	= parms.bDaysWanted[2];
			chkThursday.Checked		= parms.bDaysWanted[3];
			chkFriday.Checked		= parms.bDaysWanted[4];
			chkSaturday.Checked		= parms.bDaysWanted[5];
			chkSunday.Checked		= parms.bDaysWanted[6];
			chkShowChecked.Checked	= parms.bShowOnlyCheckedFiles;

			StartDate = GetDate(calStartDate, true);
			EndDate   = GetDate(calEndDate,   false);

			fileinfo = new ArrayList();

			nPrinters = PrinterSettings.InstalledPrinters.Count;

			bEnableFillBoxes = true;
			SetFileList();
			FillFilesBox();

			this.CenterToScreen();

		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

//---------------------------------------------------------------------------------------

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.btnGo = new System.Windows.Forms.Button();
			this.calStartDate = new System.Windows.Forms.DateTimePicker();
			this.calEndDate = new System.Windows.Forms.DateTimePicker();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnEnd = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.txtDateSpan = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.txtShortDelay = new System.Windows.Forms.TextBox();
			this.txtLongDelay = new System.Windows.Forms.TextBox();
			this.chkMonday = new System.Windows.Forms.CheckBox();
			this.chkTuesday = new System.Windows.Forms.CheckBox();
			this.chkWednesday = new System.Windows.Forms.CheckBox();
			this.chkSaturday = new System.Windows.Forms.CheckBox();
			this.chkFriday = new System.Windows.Forms.CheckBox();
			this.chkThursday = new System.Windows.Forms.CheckBox();
			this.chkSunday = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txtPuzDir = new System.Windows.Forms.Label();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.lbFiles = new System.Windows.Forms.CheckedListBox();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chkShowChecked = new System.Windows.Forms.CheckBox();
			this.lvFiles = new System.Windows.Forms.ListView();
			this.lbcolSelected = new System.Windows.Forms.ColumnHeader();
			this.lbcolDOW = new System.Windows.Forms.ColumnHeader();
			this.lbcolFilename = new System.Windows.Forms.ColumnHeader();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(23, 23);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(169, 46);
			this.btnGo.TabIndex = 0;
			this.btnGo.Text = "Go";
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// calStartDate
			// 
			this.calStartDate.Location = new System.Drawing.Point(169, 162);
			this.calStartDate.Name = "calStartDate";
			this.calStartDate.Size = new System.Drawing.Size(355, 22);
			this.calStartDate.TabIndex = 1;
			this.calStartDate.ValueChanged += new System.EventHandler(this.StartDate_ValueChanged);
			// 
			// calEndDate
			// 
			this.calEndDate.Location = new System.Drawing.Point(169, 220);
			this.calEndDate.Name = "calEndDate";
			this.calEndDate.Size = new System.Drawing.Size(355, 22);
			this.calEndDate.TabIndex = 2;
			this.calEndDate.ValueChanged += new System.EventHandler(this.calEndDate_ValueChanged);
			// 
			// label1
			// 
			this.label1.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.label1.Location = new System.Drawing.Point(12, 162);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 28);
			this.label1.TabIndex = 3;
			this.label1.Text = "Start Date";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 220);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(107, 28);
			this.label2.TabIndex = 4;
			this.label2.Text = "End Date";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnEnd
			// 
			this.btnEnd.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.btnEnd.Location = new System.Drawing.Point(728, 24);
			this.btnEnd.Name = "btnEnd";
			this.btnEnd.Size = new System.Drawing.Size(166, 46);
			this.btnEnd.TabIndex = 5;
			this.btnEnd.Text = "End";
			this.btnEnd.Click += new System.EventHandler(this.btnEnd_Click);
			// 
			// label3
			// 
			this.label3.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.label3.Location = new System.Drawing.Point(561, 162);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(96, 28);
			this.label3.TabIndex = 6;
			this.label3.Text = "Date Span";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtDateSpan
			// 
			this.txtDateSpan.Location = new System.Drawing.Point(654, 162);
			this.txtDateSpan.Name = "txtDateSpan";
			this.txtDateSpan.Size = new System.Drawing.Size(34, 22);
			this.txtDateSpan.TabIndex = 7;
			this.txtDateSpan.Text = "";
			this.txtDateSpan.TextChanged += new System.EventHandler(this.txtDateSpan_TextChanged);
			// 
			// label4
			// 
			this.label4.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.label4.Location = new System.Drawing.Point(12, 276);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(156, 30);
			this.label4.TabIndex = 8;
			this.label4.Text = "Short Delay (1000)";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(276, 276);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(167, 30);
			this.label5.TabIndex = 9;
			this.label5.Text = "Long Delay (2000)";
			// 
			// txtShortDelay
			// 
			this.txtShortDelay.Location = new System.Drawing.Point(168, 276);
			this.txtShortDelay.Name = "txtShortDelay";
			this.txtShortDelay.Size = new System.Drawing.Size(83, 22);
			this.txtShortDelay.TabIndex = 10;
			this.txtShortDelay.Text = "";
			this.txtShortDelay.TextChanged += new System.EventHandler(this.txtShortDelay_TextChanged);
			// 
			// txtLongDelay
			// 
			this.txtLongDelay.Location = new System.Drawing.Point(443, 276);
			this.txtLongDelay.Name = "txtLongDelay";
			this.txtLongDelay.Size = new System.Drawing.Size(84, 22);
			this.txtLongDelay.TabIndex = 11;
			this.txtLongDelay.Text = "";
			this.txtLongDelay.TextChanged += new System.EventHandler(this.txtLongDelay_TextChanged);
			// 
			// chkMonday
			// 
			this.chkMonday.Location = new System.Drawing.Point(36, 334);
			this.chkMonday.Name = "chkMonday";
			this.chkMonday.Size = new System.Drawing.Size(132, 35);
			this.chkMonday.TabIndex = 12;
			this.chkMonday.Text = "Monday";
			this.chkMonday.CheckedChanged += new System.EventHandler(this.chkMonday_CheckedChanged);
			// 
			// chkTuesday
			// 
			this.chkTuesday.Location = new System.Drawing.Point(36, 381);
			this.chkTuesday.Name = "chkTuesday";
			this.chkTuesday.Size = new System.Drawing.Size(132, 34);
			this.chkTuesday.TabIndex = 13;
			this.chkTuesday.Text = "Tuesday";
			this.chkTuesday.CheckedChanged += new System.EventHandler(this.chkTuesday_CheckedChanged);
			// 
			// chkWednesday
			// 
			this.chkWednesday.Location = new System.Drawing.Point(36, 427);
			this.chkWednesday.Name = "chkWednesday";
			this.chkWednesday.Size = new System.Drawing.Size(132, 34);
			this.chkWednesday.TabIndex = 14;
			this.chkWednesday.Text = "Wednesday";
			this.chkWednesday.CheckedChanged += new System.EventHandler(this.chkWednesday_CheckedChanged);
			// 
			// chkSaturday
			// 
			this.chkSaturday.Location = new System.Drawing.Point(192, 427);
			this.chkSaturday.Name = "chkSaturday";
			this.chkSaturday.Size = new System.Drawing.Size(131, 34);
			this.chkSaturday.TabIndex = 17;
			this.chkSaturday.Text = "Saturday";
			this.chkSaturday.CheckedChanged += new System.EventHandler(this.chkSaturday_CheckedChanged);
			// 
			// chkFriday
			// 
			this.chkFriday.Location = new System.Drawing.Point(192, 381);
			this.chkFriday.Name = "chkFriday";
			this.chkFriday.Size = new System.Drawing.Size(131, 34);
			this.chkFriday.TabIndex = 16;
			this.chkFriday.Text = "Friday";
			this.chkFriday.CheckedChanged += new System.EventHandler(this.chkFriday_CheckedChanged);
			// 
			// chkThursday
			// 
			this.chkThursday.Location = new System.Drawing.Point(192, 334);
			this.chkThursday.Name = "chkThursday";
			this.chkThursday.Size = new System.Drawing.Size(131, 35);
			this.chkThursday.TabIndex = 15;
			this.chkThursday.Text = "Thursday";
			this.chkThursday.CheckedChanged += new System.EventHandler(this.chkThursday_CheckedChanged);
			// 
			// chkSunday
			// 
			this.chkSunday.Location = new System.Drawing.Point(347, 334);
			this.chkSunday.Name = "chkSunday";
			this.chkSunday.Size = new System.Drawing.Size(132, 35);
			this.chkSunday.TabIndex = 18;
			this.chkSunday.Text = "Sunday";
			this.chkSunday.CheckedChanged += new System.EventHandler(this.chkSunday_CheckedChanged);
			// 
			// label6
			// 
			this.label6.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.label6.Location = new System.Drawing.Point(12, 104);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(120, 29);
			this.label6.TabIndex = 19;
			this.label6.Text = "Directory";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtPuzDir
			// 
			this.txtPuzDir.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtPuzDir.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.txtPuzDir.Location = new System.Drawing.Point(169, 104);
			this.txtPuzDir.Name = "txtPuzDir";
			this.txtPuzDir.Size = new System.Drawing.Size(479, 29);
			this.txtPuzDir.TabIndex = 20;
			this.txtPuzDir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.txtPuzDir.TextChanged += new System.EventHandler(this.txtPuzDir_TextChanged);
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.btnBrowse.Location = new System.Drawing.Point(728, 104);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(166, 29);
			this.btnBrowse.TabIndex = 21;
			this.btnBrowse.Text = "Browse..";
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// lbFiles
			// 
			this.lbFiles.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.lbFiles.CheckOnClick = true;
			this.lbFiles.Location = new System.Drawing.Point(358, 59);
			this.lbFiles.Name = "lbFiles";
			this.lbFiles.Size = new System.Drawing.Size(95, 38);
			this.lbFiles.TabIndex = 22;
			this.lbFiles.ThreeDCheckBoxes = true;
			this.lbFiles.Visible = false;
			this.lbFiles.SelectedIndexChanged += new System.EventHandler(this.lbFiles_SelectedIndexChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.lbFiles});
			this.groupBox1.Location = new System.Drawing.Point(12, 322);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(515, 151);
			this.groupBox1.TabIndex = 23;
			this.groupBox1.TabStop = false;
			// 
			// chkShowChecked
			// 
			this.chkShowChecked.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.chkShowChecked.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkShowChecked.Location = new System.Drawing.Point(744, 160);
			this.chkShowChecked.Name = "chkShowChecked";
			this.chkShowChecked.Size = new System.Drawing.Size(152, 23);
			this.chkShowChecked.TabIndex = 24;
			this.chkShowChecked.Text = "Show only checked";
			this.chkShowChecked.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkShowChecked.CheckedChanged += new System.EventHandler(this.chkShowChecked_CheckedChanged);
			// 
			// lvFiles
			// 
			this.lvFiles.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.lvFiles.CheckBoxes = true;
			this.lvFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					  this.lbcolSelected,
																					  this.lbcolDOW,
																					  this.lbcolFilename});
			this.lvFiles.FullRowSelect = true;
			this.lvFiles.GridLines = true;
			this.lvFiles.Location = new System.Drawing.Point(563, 206);
			this.lvFiles.Name = "lvFiles";
			this.lvFiles.Size = new System.Drawing.Size(333, 367);
			this.lvFiles.TabIndex = 25;
			this.lvFiles.View = System.Windows.Forms.View.Details;
			// 
			// lbcolSelected
			// 
			this.lbcolSelected.Text = "Selected";
			this.lbcolSelected.Width = 70;
			// 
			// lbcolDOW
			// 
			this.lbcolDOW.Text = "Day of week";
			this.lbcolDOW.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.lbcolDOW.Width = 95;
			// 
			// lbcolFilename
			// 
			this.lbcolFilename.Text = "Filename";
			this.lbcolFilename.Width = 160;
			// 
			// PrintPuz
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(952, 594);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.lvFiles,
																		  this.chkShowChecked,
																		  this.btnBrowse,
																		  this.txtPuzDir,
																		  this.label6,
																		  this.chkSunday,
																		  this.chkSaturday,
																		  this.chkFriday,
																		  this.chkThursday,
																		  this.chkWednesday,
																		  this.chkTuesday,
																		  this.chkMonday,
																		  this.txtLongDelay,
																		  this.txtShortDelay,
																		  this.label5,
																		  this.label4,
																		  this.txtDateSpan,
																		  this.label3,
																		  this.btnEnd,
																		  this.label2,
																		  this.label1,
																		  this.calEndDate,
																		  this.calStartDate,
																		  this.btnGo,
																		  this.groupBox1});
			this.Name = "PrintPuz";
			this.Text = "PrintPuz";
			this.Closed += new System.EventHandler(this.PrintPuz_Closed);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new PrintPuz());
		}

//---------------------------------------------------------------------------------------

		int FindAcrossLite() {
			Process	[]		procs;
			Process			p;
			procs = Process.GetProcessesByName("ACROSSL");
			if (procs.Length > 0) {
				p = procs[0];			// Take the first. Why not?
			} else {
				p = StartupAcrossLite();
				if (p == null)
					return 0;
			}
			
			int	HWND = (int)p.MainWindowHandle;
			return HWND;
		}

//---------------------------------------------------------------------------------------

		Process StartupAcrossLite() {
			try {
				RegistryKey	HKCRKey = Registry.ClassesRoot;
				RegistryKey PuzKey = HKCRKey.OpenSubKey(".puz");
				string PuzKeyDefaultValue = (string)PuzKey.GetValue("");
				RegistryKey	key = HKCRKey.OpenSubKey(PuzKeyDefaultValue);
				key = key.OpenSubKey("shell");
				key = key.OpenSubKey("open");
				key = key.OpenSubKey("command");
				// Note: There's *gotta* be a way to issue the above in one shot!
				string exe = (string)key.GetValue("");
				if (exe.EndsWith(" \"%1\"")) {
					exe = exe.Substring(0, exe.Length - 5);
				}
				Process p = Process.Start(exe);
				p.WaitForInputIdle();
				BringWindowToTop((int)p.MainWindowHandle);
				Thread.Sleep(parms.ShortDelay);
				SendKeys.SendWait("{ESC}");		// Get rid of open file box
				Thread.Sleep(parms.ShortDelay);
				return p;
			} catch {
				return null;
			}
		}

//---------------------------------------------------------------------------------------

		ArrayList GetPuzzleFilenames() {
#if false
			ArrayList AllChecked = new ArrayList(lbFiles.CheckedItems);
			string [] files = new string[AllChecked.Count];
			AllChecked.CopyTo(files);
			return files;
#endif
			ArrayList	result = new ArrayList();
			// The next line is perhaps efficient, but gives us a list of
			// ListViewItems, and I'd rather have an array of our own FileInfo's.
			// But I'll leave the code as an example of the .Net classes at work.
			//	result.AddRange(lvFiles.CheckedItems);

			ListView.CheckedListViewItemCollection col = lvFiles.CheckedItems;
			foreach (ListViewItem item in lvFiles.CheckedItems) {
				result.Add((string)item.SubItems[2].Text);	// [2] = filename
			}
			return result;
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, System.EventArgs e) {
			// First, make sure we have some puzzles to process
			ArrayList	PuzzleNames = GetPuzzleFilenames();
			if (PuzzleNames == null || PuzzleNames.Count == 0) {
				MessageBox.Show("There are no puzzles selected to print", "Print Puzzles", 
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			int	HWND = FindAcrossLite();
			if (HWND == 0) {
				MessageBox.Show("Could not find Across Lite. Please try again.", "Print Puzzles", 
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			BringWindowToTop(HWND);

			// If there's a puzzle open, close it by sending Ctrl-W. Then delay a
			// ShortInterval, and send a "n". These characters will have no effect
			// if no puzzle is currently loaded, or if there is an unmodified puzzle
			// loaded. But if there is a modified puzzle, these keystrokes will
			// close (Ctrl-W) it, and tell it not ("n") to save the puzzle.

			SendKeys.SendWait("^w");
			Thread.Sleep(parms.ShortDelay);
			SendKeys.SendWait("n");
			Thread.Sleep(parms.ShortDelay);

			foreach (string filename in PuzzleNames) {
				LoadPuzzle(filename);
				PrintCurrentPuzzle();
			}
			bSaveLastDateSeen |= PuzzleNames.Count > 0;

			// Now shut down Across Lite. We're done with it
			SendKeys.SendWait("%{F4}");		// Alt-F4
			MessageBox.Show("Done", "Print Puzzles", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

//---------------------------------------------------------------------------------------

		void LoadPuzzle(string puzname) {
			SendKeys.SendWait("^o");			// Ctrl-O == Open
			Thread.Sleep(parms.ShortDelay);
			SendKeys.SendWait(parms.PuzDir + "\\" + puzname);
			Thread.Sleep(parms.ShortDelay);
			SendKeys.SendWait("{ENTER}");		// Close file open box
			Thread.Sleep(parms.ShortDelay);
		}

//---------------------------------------------------------------------------------------

		void PrintCurrentPuzzle() {
			SendKeys.SendWait("^p");			// Ctrl-P == Print
			Thread.Sleep(parms.ShortDelay);
			SendKeys.SendWait("{ENTER}");		// Take defaults in print dialog
			Thread.Sleep(parms.LongDelay);
			if (nPrinters > 1) {
				SendKeys.SendWait("{ENTER}");	// Select physical printer dlg
				Thread.Sleep(parms.LongDelay);
			}
		}

//---------------------------------------------------------------------------------------

		void SaveParms() {
			if (bSaveLastDateSeen)
				parms.LastDateSeen = calEndDate.Value.AddDays(1);
			parms.bDaysWanted[0] = chkMonday.Checked;
			parms.bDaysWanted[1] = chkTuesday.Checked;
			parms.bDaysWanted[2] = chkWednesday.Checked;
			parms.bDaysWanted[3] = chkThursday.Checked;
			parms.bDaysWanted[4] = chkFriday.Checked;
			parms.bDaysWanted[5] = chkSaturday.Checked;
			parms.bDaysWanted[6] = chkSunday.Checked;
			parms.bShowOnlyCheckedFiles = chkShowChecked.Checked;

			TextWriter tw = new StreamWriter(PuzXMLFile);
			XmlSerializer sr = new XmlSerializer(typeof(PuzParms));
			sr.Serialize(tw, parms);
			tw.Close();
		}

//---------------------------------------------------------------------------------------

		PuzParms LoadPuzParms(string filename) {
			PuzParms	parms;
			try {
				FileStream	f = new FileStream(filename, FileMode.Open);
				XmlSerializer	sr = new XmlSerializer(typeof(PuzParms));
				parms = (PuzParms)sr.Deserialize(f);
				f.Close();
			} catch {
				parms = new PuzParms();
				parms.PuzDir		= Application.StartupPath;
				parms.ShortDelay	= PuzParms.DefaultShortDelay;
				parms.LongDelay		= PuzParms.DefaultLongDelay; 
				parms.LastDateSeen	= DateTime.Now;
				parms.TimeSpan		= 14;
				parms.bDaysWanted = new bool[7] {false, false, true, true, true, true, true};	// MTWTFSS
				parms.bShowOnlyCheckedFiles = true;
			}
			return parms;
		}

//---------------------------------------------------------------------------------------

		void SetFileList() {
			DateTime		dt;
			fileinfo.Clear();		// Clear right away, in case directory has gone away

			if (! Directory.Exists(parms.PuzDir)) {
				MessageBox.Show("The specified directory (" + parms.PuzDir + 
					") does not exist.", "Print Puzzles", 
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			string [] files = Directory.GetFiles(parms.PuzDir, "*.puz");
			for (int i=0; i<files.Length; ++i) {
				files[i] = Path.GetFileName(files[i]);
				dt = ValidatePuzzleName(files[i]);
				if (dt != MagicDate) {
					fileinfo.Add(new FileInfo(files[i], dt, (int)dt.DayOfWeek));
				}
			}
			// TODO: Must now sort it
		}

//---------------------------------------------------------------------------------------

		void FillFilesBox() {
			if (! bEnableFillBoxes)
				return;
			AddFilenames();
		}

//---------------------------------------------------------------------------------------

		void AddFilenames() {
			DateTime	dt;
			int			dow;
			bool		bWithinDateRange;
			FileInfo	fi;

			lbFiles.Items.Clear();	
			lvFiles.Items.Clear();

			for (int i=0; i<fileinfo.Count; ++i) {
				fi  = (FileInfo)fileinfo[i];
				dt  = fi.date;
				dow = fi.dow;

				if (dt >= StartDate && dt <= EndDate)
					bWithinDateRange = true;
				else
					bWithinDateRange = false;

				if (parms.bDaysWanted[dow] && bWithinDateRange) {
					AddFile(fi, true);
				} else {
					if (! parms.bShowOnlyCheckedFiles) {
						AddFile(fi, false);
					}
				}
			}
		}

//---------------------------------------------------------------------------------------

		void AddFile(FileInfo fi, bool bChecked) {
			lbFiles.Items.Add(fi.filename, bChecked);
			ListViewItem	lvi = new ListViewItem();
			lvi.Checked = bChecked;
			lvi.SubItems.Add("MonTueWedThuFriSatSun".Substring(fi.dow*3, 3));
			lvi.SubItems.Add(fi.filename);
			lvFiles.Items.Add(lvi);
		}

//---------------------------------------------------------------------------------------

		DateTime ValidatePuzzleName(string fname) {
			string filename = Path.GetFileNameWithoutExtension(fname);
			if ((filename.Length == 7) ||
				(filename.Length == 9 && filename.EndsWith(".2"))) {
				; // Filename OK; do nothing
			} else
				return MagicDate;

			string	mmm, dd, yy;
			int		m, d, y;
			mmm = filename.Substring(0, 3);
			dd  = filename.Substring(3, 2);
			yy  = filename.Substring(5, 2);
			try {
				d = int.Parse(dd);
				y = int.Parse(yy);
				if (y >= 80)
					y += 1900;
				else
					y += 2000;
			} catch {
				return MagicDate;
			}
			switch (mmm) {
			case "Jan":	m = 1;	break;
			case "Feb":	m = 2;	break;
			case "Mar":	m = 3;	break;
			case "Apr":	m = 4;	break;
			case "May":	m = 5;	break;
			case "Jun":	m = 6;	break;
			case "Jul":	m = 7;	break;
			case "Aug":	m = 8;	break;
			case "Sep":	m = 9;	break;
			case "Oct":	m = 10;	break;
			case "Nov":	m = 11;	break;
			case "Dec":	m = 12;	break;
			default:
				return MagicDate;
			}
			return new DateTime(y, m, d);
		}

//---------------------------------------------------------------------------------------

		DateTime GetDate(DateTimePicker date, bool bStartOfDay) {
			DateTime date2 = date.Value;
			DateTime newDate;
			int		y, m, d;
			y = date2.Year;
			m = date2.Month;
			d = date2.Day;
			newDate = new DateTime(y, m, d);
			if (bStartOfDay)
				return newDate;
			TimeSpan EndOfDay = new TimeSpan(0, 23, 59, 59, 999);
			return newDate + EndOfDay;
		}

//---------------------------------------------------------------------------------------

		private void btnEnd_Click(object sender, System.EventArgs e) {
			SaveParms();	
			Application.Exit();
		}

//---------------------------------------------------------------------------------------

		private void PrintPuz_Closed(object sender, System.EventArgs e) {
			SaveParms();	
		}

//---------------------------------------------------------------------------------------

		private void StartDate_ValueChanged(object sender, System.EventArgs e) {
			StartDate = GetDate(calStartDate, true);
			calEndDate.MinDate = StartDate;
			calEndDate.Value = calStartDate.Value.AddDays(parms.TimeSpan);
			FillFilesBox();
		}

//---------------------------------------------------------------------------------------

		private void calEndDate_ValueChanged(object sender, System.EventArgs e) {
			EndDate = GetDate(calEndDate, false);
			FillFilesBox();
		}

//---------------------------------------------------------------------------------------

		private void txtDateSpan_TextChanged(object sender, System.EventArgs e) {
			try {
				int span = int.Parse(txtDateSpan.Text);
				txtDateSpan.Text = span.ToString();
				parms.TimeSpan = span;
				calEndDate.Value = calStartDate.Value.AddDays(parms.TimeSpan);
				FillFilesBox();
			} catch {
				// Really should beep or something...
				txtDateSpan.Text = parms.TimeSpan.ToString();
			}
		}

//---------------------------------------------------------------------------------------

		private void txtShortDelay_TextChanged(object sender, System.EventArgs e) {
			try {
				int delay = int.Parse(txtShortDelay.Text);
				txtShortDelay.Text = delay.ToString();
				parms.ShortDelay = delay;
			} catch {
				// Really should beep or something...
				txtShortDelay.Text = parms.ShortDelay.ToString();
			}	
		}

//---------------------------------------------------------------------------------------

		private void txtLongDelay_TextChanged(object sender, System.EventArgs e) {
			try {
				int delay = int.Parse(txtLongDelay.Text);
				txtLongDelay.Text = delay.ToString();
				parms.LongDelay = delay;
			} catch {
				// Really should beep or something...
				txtLongDelay.Text = parms.LongDelay.ToString();
			}		
		}

//---------------------------------------------------------------------------------------

		private void txtPuzDir_TextChanged(object sender, System.EventArgs e) {
			parms.PuzDir = txtPuzDir.Text;
		}

//---------------------------------------------------------------------------------------

		private void btnBrowse_Click(object sender, System.EventArgs e) {
			// TODO: I don't know how to specify just a directory. I almost suspect
			// the open file dialog can't do it.
			openFileDialog1.InitialDirectory = txtPuzDir.Text;
			if (! openFileDialog1.CheckPathExists) {
				MessageBox.Show("Specified path does not exist. Reverting to application directory", "Print Puzzles", 
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				openFileDialog1.InitialDirectory = Application.StartupPath;
			}
			openFileDialog1.Filter = "Puzzle files (*.puz)|*.puz|All files (*.*)|*.*"; 
			openFileDialog1.Multiselect = true;			// Only need one, though
			openFileDialog1.CheckPathExists = true;
			openFileDialog1.RestoreDirectory = true;
			DialogResult dr = openFileDialog1.ShowDialog();
			if (dr == DialogResult.OK) {
				string [] files = openFileDialog1.FileNames;
				parms.PuzDir = Path.GetDirectoryName(files[0]);
				txtPuzDir.Text = parms.PuzDir;
			}
			SetFileList();
			FillFilesBox();
		}

//---------------------------------------------------------------------------------------

		void SetDayCheck(object sender, int n) {
			parms.bDaysWanted[n] = ((CheckBox)sender).Checked;
			FillFilesBox();
		}

//---------------------------------------------------------------------------------------

		private void chkMonday_CheckedChanged(object sender, System.EventArgs e) {
			SetDayCheck(sender, 0);
		}

		private void chkTuesday_CheckedChanged(object sender, System.EventArgs e) {
			SetDayCheck(sender, 1);		
		}

		private void chkWednesday_CheckedChanged(object sender, System.EventArgs e) {
			SetDayCheck(sender, 2);		
		}

		private void chkThursday_CheckedChanged(object sender, System.EventArgs e) {
			SetDayCheck(sender, 3);		
		}

		private void chkFriday_CheckedChanged(object sender, System.EventArgs e) {
			SetDayCheck(sender, 4);		
		}

		private void chkSaturday_CheckedChanged(object sender, System.EventArgs e) {
			SetDayCheck(sender, 5);		
		}

		private void chkSunday_CheckedChanged(object sender, System.EventArgs e) {
			SetDayCheck(sender, 6);		
		}

//---------------------------------------------------------------------------------------

		private void chkShowChecked_CheckedChanged(object sender, System.EventArgs e) {
			parms.bShowOnlyCheckedFiles = chkShowChecked.Checked;
			FillFilesBox();
		}

//---------------------------------------------------------------------------------------

		private void lbFiles_SelectedIndexChanged(object sender, System.EventArgs e) {
			if (parms.bShowOnlyCheckedFiles)
				FillFilesBox();					// In case checked status has changed
		}
	}
}
