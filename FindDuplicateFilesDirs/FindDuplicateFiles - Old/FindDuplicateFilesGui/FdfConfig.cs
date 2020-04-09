// Copyright (c) 2010 by Larry Smith

// TODO:
//	*	Don't allow ExcludeMask *
//	*	Check for DirsToInclude = C:\, C:\foo -- this is redundant
//		*	Unless Inc=C:\, C:\foo\goo; Exc=C:\foo
//		*	Quelle mess! Worry about this later.
//	*	Similar comments about FileMasks
//	*	DirsToInclude can't be empty

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FindDuplicateFilesGui {
	public partial class FdfOptions : Form {

		ConfigData parms;

		long ScaleFactor;			// 1 << 0, 10, 20, etc

		// We need these here so they don't get inadvertently garbage collected
		TransparentPanel tlblDirInc, tlblDirExc;
		TransparentPanel tlblFileMasksInc, tlblFileMasksEx;

//---------------------------------------------------------------------------------------

		public FdfOptions(ConfigData cfgData) {
			InitializeComponent();

			this.parms = cfgData;

			// Yuck!
			tlblDirInc       = new TransparentPanel(this, lblDirInc);
			tlblDirExc       = new TransparentPanel(this, lblDirExc);
			tlblFileMasksInc = new TransparentPanel(this, lblFileMasksInc);
			tlblFileMasksEx  = new TransparentPanel(this, lblFileMasksEx);

			toolTip1.SetToolTip(tlblDirInc, "Hello from Include");	// TODO:
			toolTip1.SetToolTip(tlblDirExc, "Hello from Exclude");	// TODO:
		}

//---------------------------------------------------------------------------------------

		private void FdfOptions_Load(object sender, EventArgs e) {

			// Copy the data from the ConfigData instance into the form

			// Set up the combo boxes
			cbDirectoriesToInclude.Items.AddRange(parms.DirectoriesToInclude.ToArray());
			cbDirectoriesToExclude.Items.AddRange(parms.DirectoriesToExclude.ToArray());
			cbFileMasksToInclude.Items.AddRange(parms.FileMasksToInclude.ToArray());
			cbFileMasksToExclude.Items.AddRange(parms.FileMasksToExclude.ToArray());

			// Show first element of each combo box, if indeed it has any
			if (cbDirectoriesToInclude.Items.Count > 0) 
				cbDirectoriesToInclude.SelectedIndex = 0;
			if (cbDirectoriesToExclude.Items.Count > 0)
				cbDirectoriesToExclude.SelectedIndex = 0;
			if (cbFileMasksToInclude.Items.Count > 0)
				cbFileMasksToInclude.SelectedIndex   = 0;
			if (cbFileMasksToExclude.Items.Count > 0)
				cbFileMasksToExclude.SelectedIndex   = 0;

			// Set the relevant radio button
			switch (parms.ShiftFactor) {
			case 0:
				radSizeInBytes.Checked = true;
				break;
			case 10:
				radSizeInK.Checked = true;
				break;
			case 20:
			default:			// If somehow not found, default to MB
				radSizeInMB.Checked = true;
				break;
			case 30:
				radSizeInGB.Checked = true;
				break;
			case 40:
				radSizeInTB.Checked = true;
				break;
			}
		}

//---------------------------------------------------------------------------------------

		private void FdfOptions_Paint(object sender, PaintEventArgs e) {
			if (e.ClipRectangle.IsEmpty)
				return;
			Color From, To;
			From = Color.Red;
			To = Color.CornflowerBlue;
			using (var lgb = new LinearGradientBrush(e.ClipRectangle, From, To, 45f, true)) {
				e.Graphics.FillRectangle(lgb, e.ClipRectangle);
			}
		}

//---------------------------------------------------------------------------------------

		private void btnEditDirectoriesToInclude_Click(object sender, EventArgs e) {
			EditCombo(cbDirectoriesToInclude, "Edit -- Directories to include", true);
			// TODO:
			//ProcessAddToDirectories(cbDirectoriesToInclude);
		}

//---------------------------------------------------------------------------------------

		private void btnEditDirectoriesToExclude_Click(object sender, EventArgs e) {
			EditCombo(cbDirectoriesToExclude, "Edit -- Directories to exclude", true);
			// TODO:
			// ProcessAddToDirectories(cbDirectoriesToExclude);
		}

//---------------------------------------------------------------------------------------

		private void btnEditFileMasksToInclude_Click(object sender, EventArgs e) {
			EditCombo(cbFileMasksToInclude, "Edit -- File masks to include", false);
		}

//---------------------------------------------------------------------------------------

		private void btnFileMasksToExclude_Edit_Click(object sender, EventArgs e) {
			EditCombo(cbFileMasksToExclude, "Edit -- File masks to exclude", false);
		}

//---------------------------------------------------------------------------------------

		private void EditCombo(ComboBox cb, string msg, bool isDir) {
			EditComboBox ecb = new EditComboBox(cb, isDir, msg);
			DialogResult res = ecb.ShowDialog();
			if (res == DialogResult.OK) {
				cb.Items.Clear();
				foreach (string item in ecb.items) {
					cb.Items.Add(item);
				}
			}
			if (cb.Items.Count > 0) {
				cb.SelectedIndex = 0;
			}
		}

//---------------------------------------------------------------------------------------

		// TODO:
		private void xxxProcessAddToDirectories(ComboBox cb) {
			var res = folderBrowserDialog1.ShowDialog();
			if (res != DialogResult.OK) {
				return;
			}
			string dirname = folderBrowserDialog1.SelectedPath;
			if (xxxIsNameInComboBox(cb, dirname)) {
				MessageBox.Show("That directory (" + dirname + ") is already on your list and has been ignored.",
					"Find Duplicate Files",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			int n = cb.Items.Add(dirname);
			cb.SelectedIndex = n;
		}

//---------------------------------------------------------------------------------------

		// TODO:
		private bool xxxIsNameInComboBox(ComboBox cb, string dirname) {
			foreach (var item in cb.Items) {
				if (item.ToString() == dirname) {
					return true;
				}
			}
			return false;
		}

//---------------------------------------------------------------------------------------

		private void FdfOptions_FormClosing(object sender, FormClosingEventArgs e) {
			if (this.DialogResult != DialogResult.OK) {
				return;					// Don't validate unless he clicked OK
			}

			int ShiftFactor;
			string sShiftFactor = GetScaleFactor(out ShiftFactor);		// "", "KB", "GB", etc

			long MaxValue = long.MaxValue / ScaleFactor;
			string msg;
			bool bOK;
			long MinVal, MaxVal;

			string txtSize;

			txtSize = txtMinFileSize.Text.Trim();
			if (txtSize.Length == 0) {
				MinVal = 0;
			} else {
				bOK = GetVal(txtSize, 0, MaxValue, out MinVal);
				if (!bOK) {
					msg = string.Format("The minimum value must be integral, between 0 and {0:N0} {1}.", MaxValue, sShiftFactor);
					MessageBox.Show(msg, "Find Duplicate Files");
					e.Cancel = true;
					return;
				}
			}

			txtSize = txtMaxFileSize.Text.Trim();
			if (txtSize.Length == 0) {
				MaxVal = MaxValue;
			} else {
				bOK = GetVal(txtSize, MinVal + 1, MaxValue, out MaxVal);
				if (!bOK) {
					msg = string.Format("The maximum value must be integral, between {0:N0} and {1:N0} {2}.", MinVal + 1, MaxValue, sShiftFactor);
					MessageBox.Show(msg, "Find Duplicate Files");
					e.Cancel = true;
					return;
				}
			}


			// All the error checking is done, and we're fine. Update the parms data.
			parms.ShiftFactor = ShiftFactor;
			parms.MinSize = MinVal * ScaleFactor;
			parms.MaxSize = MaxVal * ScaleFactor;

			// Copy contents of combo boxes
			CopyCombo(cbDirectoriesToInclude, parms.DirectoriesToInclude);
			CopyCombo(cbDirectoriesToExclude, parms.DirectoriesToExclude);
			CopyCombo(cbFileMasksToInclude, parms.FileMasksToInclude);
			CopyCombo(cbFileMasksToExclude, parms.FileMasksToExclude);
		}

//---------------------------------------------------------------------------------------

		private void CopyCombo(ComboBox cb, List<string> list) {
			list.Clear();					// Empty list, then recreate it
			foreach (var item in cb.Items) {
				list.Add(item.ToString());
			}
		}

//---------------------------------------------------------------------------------------

		bool GetVal(string txt, long MinVal, long MaxVal, out long Val) {
			Val = 0;
			bool bOK = long.TryParse(txt, out Val);
			if (!bOK)
				return false;
			if (Val < MinVal)
				return false;
			if (Val > MaxVal)
				return false;
			return true;
		}

//---------------------------------------------------------------------------------------

		string GetScaleFactor(out int ShiftFactor) {
			string suffix = "MB";
			ShiftFactor = 20;				// Default to MB
			if (radSizeInBytes.Checked) {
				ShiftFactor = 0;
				suffix = "";
			} 
			if (radSizeInK.Checked) {
				ShiftFactor = 10;
				suffix = "KB";
			} 
			if (radSizeInMB.Checked) {
				ShiftFactor = 20;
				suffix = "MB";
			} 
			if (radSizeInGB.Checked) {
				ShiftFactor = 30;
				suffix = "GB";
			} else if (radSizeInTB.Checked) {
				ShiftFactor = 40;
				suffix = "TB";
			}
			ScaleFactor = 1L << ShiftFactor;
			return suffix;
		}

//---------------------------------------------------------------------------------------

		// TODO:
		private void xxxbtnDirectoriesToInclude_Remove_Click(object sender, EventArgs e) {
			RemoveSelectedDirectory(cbDirectoriesToInclude);
		}

//---------------------------------------------------------------------------------------

		// TODO:
		private void xxxbtnDirectoriesToExclude_Remove_Click(object sender, EventArgs e) {
			RemoveSelectedDirectory(cbDirectoriesToExclude);
		}

//---------------------------------------------------------------------------------------

		private void RemoveSelectedDirectory(ComboBox cb) {
			if (cb.Items.Count == 0) {
				MessageBox.Show("There are no items to remove", "Find Duplicate Files",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			string msg = "Are you sure you want to remove " + cb.SelectedItem.ToString() + "?";
			var YesNo = MessageBox.Show(msg, "Find Duplicate Files", 
				MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (YesNo != DialogResult.Yes) {
				return;
			}

			cb.Items.RemoveAt(cb.SelectedIndex);

			// Redisplay the top item, if any
			if (cb.Items.Count == 0) {
				cb.Text = "";
			} else {
				cb.SelectedIndex = 0;
			}
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------


	/// <summary>
	/// This more than kinda sucks. In WIALink, all we needed to do was to create a 
	/// Label and sets its BackColor to Color.Transparent. Sigh.
	/// <para>
	/// BTW, I tried retargeting this application (with the label's BackColor set to
	/// Transparent) to CLR 3.5 and 2.0. Nope. No such luck. Thus this kludge.
	/// </para>
	/// <para>
	/// It's enough to drive a man to WPF! (Is it easy there?)
	/// </para>
	/// </summary>
	public class TransparentPanel : /*UserControl*/ /*Label*/ Panel {

		Label BaseLabel;

//---------------------------------------------------------------------------------------

		public TransparentPanel(Form frm, Label BaseLabel) {
			this.BaseLabel = BaseLabel;

			BaseLabel.Visible = false;

			this.Text        = BaseLabel.Text;
			this.Top         = BaseLabel.Top;
			this.Left        = BaseLabel.Left;
			this.Width       = BaseLabel.Width;
			this.Height      = BaseLabel.Height;
			//this.BackColor = Color.Transparent;
			this.Paint      += new PaintEventHandler(TransparentPanel_Paint);
			frm.Controls.Add(this);
		}

//---------------------------------------------------------------------------------------

		protected override CreateParams CreateParams {
			// From http://www.pcreview.co.uk/forums/thread-1312555.php
			// See also http://social.msdn.microsoft.com/Forums/en-US/winforms/thread/95b93920-9bb4-484a-8947-75719b8dce2d
			get {
				CreateParams cp = base.CreateParams;

				cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT
				return cp;
			}
		}

//---------------------------------------------------------------------------------------

		void TransparentPanel_Paint(object sender, PaintEventArgs e) {
			Panel pnl = sender as Panel;
			e.Graphics.DrawString(BaseLabel.Text, BaseLabel.Font, Brushes.Black, 0, 0);
		}
	}
}
