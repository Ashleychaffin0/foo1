using System;
using System.Windows.Forms;

namespace FindDuplicateFiles {
	public partial class FindDuplicateFiles : Form {
		// We'll read in this many bytes at the start of the file to calculate a
		// "short" (i.e. rough cut) CRC for the file. We don't want to spend all the time
		// to read the entire file to get the real CRC. The first couple of hundred bytes
		// should be more than enough, especially since we'll use the combination of file
		// size and (short) CRC as the fingerprint.
		// Note: I chose 512 as being one physical sector on the (hard) disk. Floppies,
		//		 CD/DVDs, network drives, etc -- YMMV.
		public const int ShortCRCLength = 512;

		//Note: FileInfo gives file sizes in terms of long's, not ulongs, so we'll do
		//		things its way (even though the filesize *really* should be a ulong).
		long MinSize, MaxSize;
		long ScaleFactor = 0;

//-------------------------------------------------------------------------------
		public FindDuplicateFiles() {
			InitializeComponent();
		}

//-------------------------------------------------------------------------------

		private void BtnGo_Click(object sender, System.EventArgs e) {
			GetScaleFactor();
			if (!CheckMinMaxSizes(out long MinVal, out long MaxVal)) {
				return;
			}
			MinSize = MinVal * ScaleFactor;
			MaxSize = MaxVal * ScaleFactor;
		}

//-------------------------------------------------------------------------------

		void GetScaleFactor() {
			int ShiftFactor = 20;       // Default to MB, but shouldn't happen
			if (radBytes.Checked) ShiftFactor = 0;
			else if (radKB.Checked) ShiftFactor = 10;
			else if (radMB.Checked) ShiftFactor = 20;
			else if (radGB.Checked) ShiftFactor = 30;
			else if (radTB.Checked) ShiftFactor = 40;
			ScaleFactor = 1L << ShiftFactor;
		}

//-------------------------------------------------------------------------------

		private bool CheckMinMaxSizes(out long MinVal, out long MaxVal) {
			bool bOK;
			string msg;
			long MaxValue = long.MaxValue / ScaleFactor;
			string txtSize;
			
			MinVal = MaxVal = -1;   // For <out> reasons

			txtSize = txtMinSize.Text.Trim();
			if (txtSize.Length == 0) {
				MinVal = 0;
			} else {
				bOK = GetVal(txtSize, 0, MaxValue, out MinVal);
				if (!bOK) {
					// TODO: Folowing msg is wrong for min=133, max=3
					msg = $"The minimum value must be integral, between 0 and {MaxValue}.";
					MessageBox.Show(msg, "Find Duplicate Files");
					return false;
				}
			}

			txtSize = txtMaxSize.Text.Trim();
			if (txtSize.Length == 0) {
				MaxVal = MaxValue;
			} else {
				bOK = GetVal(txtSize, MinVal + 1, MaxValue, out MaxVal);
				if (!bOK) {
					msg = $"The minimum value must be integral, between {MinSize + 1} and {MaxValue}.";
					MessageBox.Show(msg, "Find Duplicate Files");
					return false;
				}
			}
			return true;
		}

//-------------------------------------------------------------------------------

		bool GetVal(string txt, long MinVal, long MaxVal, out long Val) {
			bool bOK = long.TryParse(txt, out Val);
			if (!bOK)
				return false;
			if (Val < MinVal || Val > MaxVal)
				return false;
			return true;
		}

//-------------------------------------------------------------------------------

		private void btnFolderBrowse_Click(object sender, EventArgs e) {
			folderBrowserDialog1.SelectedPath = @"G:\";
			folderBrowserDialog1.Description = "Select directory to scan for duplicates";
			DialogResult res = folderBrowserDialog1.ShowDialog();
			if (res == DialogResult.Cancel)
				return;
			lblDir.Text = folderBrowserDialog1.SelectedPath;
			btnGo.Enabled = true;
		}
	}
}
