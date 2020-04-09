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

namespace OneDriveStats {
	public partial class OneDriveStats : Form {
		public OneDriveStats() {
			InitializeComponent();

			SetupOwners();
		}

//---------------------------------------------------------------------------------------

		private void SetupOwners() {
			var Owners = new List<OneDriveOwner> {
				new OneDriveOwner("LRS Extra", @"C:\Users\lrs5O\OneDrive"),
				new OneDriveOwner("LRS", @"G:\OneDrive"),
				new OneDriveOwner("BGA", @"Y:\Users\B\OneDrive")
			};
			CmbOneDriveOwner.Items.AddRange(Owners.ToArray());
			CmbOneDriveOwner.SelectedIndex = 0;
		}

//---------------------------------------------------------------------------------------

		void OneDriveTest(string OneDrivePath) {
			int nRecallable    = 0;
			int nPending       = 0;
			int nUnknownStatus = 0;

			long FileSizeRecallable = 0;
			long FileSizePending    = 0;
			long FileSizeUnknown	= 0;

			foreach (var item in Directory.EnumerateFileSystemEntries(OneDrivePath, "*", SearchOption.AllDirectories)) {
				bool KnowStatus = false;
				var fi          = new FileInfo(item);
				var attrs       = (MyFileAttributes)fi.Attributes;
				string IsDir    = attrs.HasFlag(MyFileAttributes.Directory) ? "Directory " : "";
				if (IsDir.Length > 0) { continue; }
				// Console.Write(IsDir);
				if (attrs.HasFlag(MyFileAttributes.RecallOnDataAccess)) { 
					// Note: The <attrib> command shows files as Offline, but it's really
					//		 checking this flag
					KnowStatus = true;
					// Console.Write("Recall on Data Access ");
					++nRecallable;
					FileSizeRecallable += fi.Length;
				}
				if (attrs.HasFlag(MyFileAttributes.Unpinned)) {
					KnowStatus = true;
					// Console.Write("Unpinned ");
					++nPending;
					FileSizePending += fi.Length;
				}
				if (!KnowStatus) {
					// Console.Write("Status Unknown ");
					++nUnknownStatus;
					FileSizeUnknown += fi.Length;
				}

				// Console.WriteLine(item);
			}

			LblRecallableNumber.Text = $"{nRecallable:N0}";
			LblRecallableSize.Text   = $"{FileSizeRecallable:N0}";

			LblUnpinnedNumber.Text   = $"{nPending:N0}";
			LblUnpinnedSize.Text     = $"{FileSizePending:N0}";

			LblUnknownNumber.Text    = $"{nUnknownStatus:N0}";
			LblUnknownSize.Text      = $"{FileSizeUnknown:N0}";
		}

//---------------------------------------------------------------------------------------

		private void BtnGo_Click(object sender, EventArgs e) {
			var owner = CmbOneDriveOwner.SelectedItem as OneDriveOwner;
			OneDriveTest(owner.Path);
		}
	}
}
