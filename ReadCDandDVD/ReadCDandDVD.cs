// TODO: Create a class OpticalDrive (probably based on class DriveLetterDrive),
//		 then derive class CdDrive and class DvdDrive (and presumably later,
//		 class BluRayDrive, etc). These can have interesting drive-type specific
//		 features, such as a CDDB interface for CDs, extracting Titles etc from
//		 DVDs and so on.
// TODO: But for now, just read sectors sequentially

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.Win32;

public static class Drives {

	public static List<DriveInfo> GetDrives() {
		List<DriveInfo>	Drives = new List<DriveInfo>();
		for (int i = 0; i < 26; i++) {
// 				string DriveName = string.Format("{0}:", Convert.ToChar('A' + i));
				string DriveName = string.Format("{0}:", (char)('A' + i));
				DriveInfo info = new DriveInfo(string.Format("{0}", DriveName));
#if false
				if (info.DriveType == DriveType.NoRootDirectory) {
					continue;			// Ignore non-mounted drives
				}	
#endif
			Drives.Add(info);
		}
		return Drives;
	}


}

namespace ReadCDandDVD {
	public partial class ReadCDandDVD : Form {
		public ReadCDandDVD() {
			InitializeComponent();
		}

		private void ReadCDandDVD_Load(object sender, EventArgs e) {
			// TODO: Don't I have a Drive object around somewhere?
			int	ixFirstOptical = -1;

			var qryDrives = from drive in Drives.GetDrives()
							where drive.DriveType != DriveType.NoRootDirectory
							select drive;
			foreach (var drive in qryDrives) {
				if ((drive.DriveType == DriveType.CDRom) && (ixFirstOptical < 0)) {
					ixFirstOptical = cmbDrives.Items.Count;	// Do this before Add
				}
				cmbDrives.Items.Add(string.Format("{0} ({1})", drive.Name, drive.DriveType));
			}
			ixFirstOptical = Math.Max(ixFirstOptical, 0);
			cmbDrives.SelectedIndex = ixFirstOptical;
		}

		private void btnGo_Click(object sender, EventArgs e) {
			string	DriveName = cmbDrives.SelectedItem.ToString();
			DriveName = DriveName.Substring(0, 2);
			// var qryDrive = from 
		}
	}
}
