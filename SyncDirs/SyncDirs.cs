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
// using Microsoft.

using LrsUtils;

namespace SyncDirs {
	public partial class SyncDirs : Form {
		// List<(FileInfo, FileInfo)> Files;

//---------------------------------------------------------------------------------------

		public SyncDirs() {
			InitializeComponent();

			TxtMasterFolder.Text = @"G:\LRS\$Dev\";
			TxtBackupFolder.Text = @"G:\OneDrive\$Dev\";
			// TxtFolder1.Text = Environment.GetEnvironmentVariable("OneDrive") + "\\";
			// TxtFolder2.Text = @"G:\LRS-8500\SkyDrive\";
		}

//---------------------------------------------------------------------------------------

		private void BtnGo_Click(object sender, EventArgs e) {
			// Files = new List<(FileInfo, FileInfo)>();
			int LenMaster = TxtMasterFolder.Text.Length;
			int LenBackup = TxtBackupFolder.Text.Length;
			var AllMaster = Directory.EnumerateFiles(TxtMasterFolder.Text, "*", SearchOption.AllDirectories);
			var AllBackup = Directory.EnumerateFiles(TxtBackupFolder.Text, "*", SearchOption.AllDirectories);

			var SortedMaster = from f in AllMaster
						  orderby f.Substring(LenMaster)
						  select f.Substring(LenMaster);
			var SortedBackup = from f in AllBackup
						  orderby f.Substring(LenBackup)
						  select f.Substring(LenBackup);
			// Dump(SortedMaster, "Sorted Master");
			// Dump(SortedBackup, "Sorted Backup");
			// var s3 = All1.OrderBy(p => p);
			var sb = new StringBuilder();

			BalanceLine<string>.Balance(SortedMaster, SortedBackup,
				(MastName, BackupName) => { return MastName.CompareTo(BackupName); },
				(MastName, bInMaster, BackupName, bInBackup) => {
					if (bInMaster && bInBackup) {
						var MasterInfo = new FileInfo(mast(MastName));
						var BackupInfo = new FileInfo(back(BackupName));
						if (MasterInfo.LastWriteTime > BackupInfo.LastWriteTime) {
							sb.AppendLine($"xcopy {mast(MastName)} {back(BackupName)}");
							// Files.Add((MasterInfo, BackupInfo));
							// sb.AppendLine($"{TxtMasterFolder.Text + MastName}");
							// sb.AppendLine($"\t\tlwtMaster={MasterInfo.LastWriteTime}, lwtBackup={BackupInfo.LastWriteTime}");
							// System.Diagnostics.Debugger.Break();
						}
					} else if (!bInMaster) {
						sb.AppendLine($"del {back(BackupName)}");
						//sb.AppendLine($"InBackup={bInBackup}, BackupName={back(BackupName)}"); }
					} else if (!bInBackup) {
						// sb.AppendLine($"bInMaster={bInMaster}");
						sb.AppendLine($"xcopy {mast(MastName)} {back(BackupName)}");
					}
				}
			);
			var lines = sb.ToString();
			var CRLF = new string[] { "\r\n" };
			var ls = lines.Split(CRLF, StringSplitOptions.RemoveEmptyEntries);
			Clipboard.SetText(lines);
		}

//---------------------------------------------------------------------------------------

		private string mast(string name) => Path.Combine(TxtMasterFolder.Text, name);

//---------------------------------------------------------------------------------------

		private string back(string name) => Path.Combine(TxtBackupFolder.Text, name);

//---------------------------------------------------------------------------------------

		private void Dump(IEnumerable<string> sorted, string name, int n = 10) {
			Console.WriteLine($"\r\n\r\n ----- {name}");
			int i = 0;
			foreach (var item in sorted) {
				Console.WriteLine(item);
				if (++i >= n) { return; }
			}
		}
	}
}
