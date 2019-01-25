// Copyright (c) 2006 Bartizan Connects, LLC

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;  
using System.Text;
using System.Windows.Forms;

namespace WipeDrive {
	public partial class WipeDrive : Form {

		List<string>	Drives = null;
		string			CurDrive;
		DriveInfo		CurDriveInfo;
		string			fooName = "foo.bin";

		string []		Formats = {"FAT32", "NTFS", "FAT"};

		public WipeDrive() {
			InitializeComponent();

			cmbFormat.DataSource = Formats;
			// TestInterceptSysout();
		}

		private void TestInterceptSysout() {
			Process	p = new Process();
			// ProcessStartInfo	psi = new ProcessStartInfo("sort.exe", @"c:\foo1.xml");
			ProcessStartInfo	psi = new ProcessStartInfo("ldb.exe", @"""C:\Documents and Settings\larrys\Application Data\SharpReader"" F:\");
			psi.RedirectStandardOutput = true;
			psi.RedirectStandardError  = true;
			psi.UseShellExecute = false;
			p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
			p.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);
			p.StartInfo = psi;
			bool isOK = p.Start();
			p.BeginOutputReadLine();
			p.BeginErrorReadLine();
			while (! p.HasExited) {
				System.Threading.Thread.Sleep(500);
			}
			Console.WriteLine("Hi");
		}

		void p_ErrorDataReceived(object sender, DataReceivedEventArgs e) {
			if (e.Data == null) {
				return;
			}
			AddMsg("Syserr {0}", e.Data);
		}

		void p_OutputDataReceived(object sender, DataReceivedEventArgs e) {
			if (e.Data == null) {
				return;
			}
			AddMsg("Stdout {0}", e.Data);
		}

		private void WipeDrive_Load(object sender, EventArgs e) {
			RefreshDrives();
		}

		private void RefreshDrives() {
			string [] LogDrives = Environment.GetLogicalDrives();
			Drives = new List<string>(LogDrives);
			for (int i = Drives.Count - 1; i >= 0; i--) {
				if ((Drives[i] == @"C:\") && (! chkIncludeCDrive.Checked)) {
					Drives.RemoveAt(i);
					continue;		// C: is unlikely to be a network drive
				}
				DriveInfo	inf = new DriveInfo(Drives[i]);
				if ((inf.DriveType == DriveType.Network) && (!chkIncludeNetworkDrives.Checked)) {
					Drives.RemoveAt(i);
				}
			}
			cmbDrives.DataSource = Drives;
		}

		private void cmbDrives_SelectedIndexChanged(object sender, EventArgs e) {
			CurDrive = (string)cmbDrives.SelectedItem;
			ShowDriveInfo();
		}

		private void ShowDriveInfo() {
			CurDriveInfo = new DriveInfo(CurDrive);
			// There may be no media in the drive (e.g. empty floppy or CD). Need try.
			lblDriveType.Text = CurDriveInfo.DriveType.ToString();
			try {
				lblDriveLabel.Text = CurDriveInfo.VolumeLabel;
				lblFileSystem.Text = CurDriveInfo.DriveFormat;
				lblDriveSize.Text = string.Format("{0:N0}", CurDriveInfo.TotalSize / (1024 * 1024));
				long SpaceUsed = CurDriveInfo.TotalSize - CurDriveInfo.TotalFreeSpace;
				lblDriveSpaceUsed.Text = string.Format("{0:N0}", SpaceUsed / (1024 * 1024));
			} catch {
				lblDriveLabel.Text = "";
				lblFileSystem.Text = "";
				lblDriveSize.Text = "";
			}
		}

		private void btnRefresh_Click(object sender, EventArgs e) {
			RefreshDrives();
		}

		private void chkIncludeCDrive_Click(object sender, EventArgs e) {
			RefreshDrives();
		}

		private void chkIncludeNetworkDrives_Click(object sender, EventArgs e) {
			RefreshDrives();
		}

		private void btnGo_Click(object sender, EventArgs e) {
			DialogResult	rc = ConfirmFormat();
			if (rc != DialogResult.Yes) {
				MessageBox.Show("Smart move on your part. The drive is safe (for now).", 
					"You almost blew it, but not quite", 
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			} else {
				bool bOK = FormatDrive();
				if (bOK) {
					WipeIt();
				}
			}
		}

		private bool FormatDrive() {
			// FORMAT volume [/FS:file-system] [/V:label] [/Q] [/A:size] [/C] [/X]
			string drive = CurDrive.Substring(0, 2);		// Strip of \ in C:\
			string parms = drive + " /FS:" + cmbFormat.SelectedItem;
			parms += " /Q";
			string label = CurDriveInfo.VolumeLabel;
			if (label.Trim().Length != 0) {
				parms += " /V:" + label;
			}
			AddMsg("Running FORMAT command with parms -- {0}", parms);
#if true		// TODO:
			// string SysDir = Environment.GetFolderPath(Environment.SpecialFolder.System);
			// cmd = SysDir + "\\" + cmd;
			Process fmt = Process.Start("FORMAT.com", parms);
			fmt.WaitForExit();
			int	rc = fmt.ExitCode;
			if (rc != 0) {
				string	msg = string.Format("The FORMAT failed with return code {0}", rc);
				MessageBox.Show(msg, "Format Failure", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}
#else
			AddMsg("Bypassing actual format");
#endif
			AddMsg("The drive {0} has now been formatted as FAT32", CurDrive);
			return true;
		}

		private void WipeIt() {
			const int Bufsize = 1024 * 1024;	// Arbitrary
			byte[] buf = new byte[Bufsize];
			prgWipe.Maximum = (int)(CurDriveInfo.TotalSize / Bufsize);
			FileStream FileOut = new FileStream(CurDrive + fooName, FileMode.CreateNew);
			BinaryWriter wtr = new BinaryWriter(FileOut);
			for (int i = 0; i < udWipeCount.Value; i++) {
				lblWipePass.Text = string.Format("Pass {0} of {1}", i + 1, udWipeCount.Value);
				FillDrive(i, buf, wtr);
			}
			wtr.Close();
			AddMsg("The drive {0} has been wiped", CurDrive);
			File.Delete(CurDrive + fooName);	
			ShowDriveInfo();
		}

		private void FillDrive(int i, byte[] buf, BinaryWriter wtr) {
			if (i > 0) {	// Don't bother first time through
				// Alternate writing all 0's and all 1's
				byte fill = (byte)(((i % 2) == 0) ? 0 : 0xff);
				for (int j = 0; j < buf.Length; j++) {
					buf[j] = fill;
				}
			}
			wtr.Flush();
			wtr.Seek(0, SeekOrigin.Begin);
			prgWipe.Value = 0;
			int		nBufs = 0;
			try {
				while (true) {
					wtr.Write(buf);
					prgWipe.Value = ++nBufs;
					float	pctDone = ((float)nBufs) / prgWipe.Maximum;
					lblPctDone.Text = string.Format("{0:P} done", pctDone);
					Application.DoEvents();
				}
			} catch {
				// We expect to throw once the drive is full. The last write
				// failed on Disk Full, so write out the remainder
				DriveInfo	info = new DriveInfo(CurDrive);
				if (info.AvailableFreeSpace > 0) {
					wtr.Write(buf, 0, (int)info.AvailableFreeSpace);
				}
				prgWipe.Value = prgWipe.Maximum;
				lblPctDone.Text = string.Format("{0:P} done", 1.0f);
			}
		}

		private DialogResult ConfirmFormat() {
			string msg = string.Format("Are you sure you want to format drive {0}"
				+ " with label {1}?", CurDrive.Substring(0, 2), 
					CurDriveInfo.VolumeLabel);
			DialogResult rc = MessageBox.Show(msg, "WARNING!!!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
			if (rc != DialogResult.Yes) {
				return DialogResult.No;
			}
			msg = string.Format("Last Chance. Are you ***SURE*** you want to format drive {0}"
				+ " with label {1}?????", CurDrive.Substring(0, 2),
					CurDriveInfo.VolumeLabel);
			rc = MessageBox.Show(msg, "WARNING!!!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
			if (rc != DialogResult.Yes) {
				return DialogResult.No;
			}
			rc = MessageBox.Show("OK, you asked for it", "You didn't need that hard drive anyway, right?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
			if (rc != DialogResult.Yes) {
				return DialogResult.No;
			}
			return DialogResult.Yes;
		}

		private void btnBrowseDrive_Click(object sender, EventArgs e) {
			Process.Start(CurDrive);
		}

		private void AddMsg(string fmt, params object[] parms) {
			lbMsgs.Items.Add(string.Format(fmt, parms));
		}
	}
}