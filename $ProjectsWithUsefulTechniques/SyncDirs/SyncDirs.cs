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

using LrsUtils;

namespace SyncDirs {
	public partial class SyncDirs : Form {

//---------------------------------------------------------------------------------------

		public SyncDirs() {
			InitializeComponent();

			TxtMasterFolder.Text = @"G:\LRS\$Dev\";
			TxtBackupFolder.Text = @"G:\OneDrive\$Dev\";
		}

//---------------------------------------------------------------------------------------

		private void BtnGo_Click(object sender, EventArgs e) {
			int LenMaster = TxtMasterFolder.Text.Length;
			int LenBackup = TxtBackupFolder.Text.Length;
			var Master = Directory.EnumerateFiles(TxtMasterFolder.Text, "*", SearchOption.AllDirectories);
			var Backup = Directory.EnumerateFiles(TxtBackupFolder.Text, "*", SearchOption.AllDirectories);

			var SortedMaster = from f in Master
						  orderby f.Substring(LenMaster)
						  select f.Substring(LenMaster);
			var SortedBackup = from f in Backup
						  orderby f.Substring(LenBackup)
						  select f.Substring(LenBackup);
			var sb = new StringBuilder();

			BalanceLine<string>.Balance(SortedMaster, SortedBackup,
				(MastName, BackupName) => { return MastName.CompareTo(BackupName); },
				(MastName, bInMaster, BackupName, bInBackup) => {
					if (bInMaster && bInBackup) {
						var MasterInfo = new FileInfo(mast(MastName));
						var BackupInfo = new FileInfo(back(BackupName));
						if (MasterInfo.LastWriteTime > BackupInfo.LastWriteTime) {
							sb.AppendLine($"xcopy /Y \"{mast(MastName)}\" \"{back(BackupName)}\"");
						}
					} else if (!bInMaster) {
						sb.AppendLine($"del \"{back(BackupName)}\"");
					} else if (!bInBackup) {
						sb.AppendLine($"echo F | xcopy \"{mast(MastName)}\" \"{back(MastName)}\"");
					}
				}
			);
			var lines = sb.ToString();
			var CRLF = new string[] { Environment.NewLine };
			var ls = lines.Split(CRLF, StringSplitOptions.RemoveEmptyEntries);
			Clipboard.SetText(lines);
		}

//---------------------------------------------------------------------------------------

		private string mast(string name) => Path.Combine(TxtMasterFolder.Text, name);

//---------------------------------------------------------------------------------------

		private string back(string name) => Path.Combine(TxtBackupFolder.Text, name);
	}
}
