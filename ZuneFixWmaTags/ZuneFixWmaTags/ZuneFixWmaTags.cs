// TODO:
//	1)	Make Artist/Album combo boxes drop-down lists. Can't with DataSource.


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

using WMFSDKWrapper;

using LRS.WMA;

namespace ZuneFixWmaTags {
	public partial class ZuneFixWmaTags : Form {

		string	CurPath = "";

//---------------------------------------------------------------------------------------

		public ZuneFixWmaTags() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void btnBrowsePath_Click(object sender, EventArgs e) {
			FolderBrowserDialog	dlg = new FolderBrowserDialog();
			dlg.SelectedPath = GetZuneDirectory();
			DialogResult res = dlg.ShowDialog();
			if (res == DialogResult.OK) {
				txtPath.Text = dlg.SelectedPath;
				CurPath = txtPath.Text;
				FillArtist(CurPath);
			}
		}

//---------------------------------------------------------------------------------------

		private static string GetZuneDirectory() {
			// Makes several assumptions.
			//	1) Zune software is installed so that the registry has these keys
			//	2) We have a monitored audio directory
			//	3) We haven't moved it and not told Zune
			//	4) The first directory in the list is the main Zune audio directory
			//	5) And probably others
			RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Zune\Groveler");
			string [] s = (string[])key.GetValue("MonitoredAudioFolders");
			return s[0];
		}

//---------------------------------------------------------------------------------------

		private void FillArtist(string path) {
			string [] dirs = Directory.GetDirectories(path);
			string [] Artists = new string[dirs.Length];
			int	pathlen = path.Length + 1;	// +1 for trailing backslash
			for (int i = 0; i < dirs.Length; i++) {
				Artists[i] = dirs[i].Substring(pathlen);
			}
			cmbArtist.DataSource = Artists;
		}

//---------------------------------------------------------------------------------------

		private void cmbArtist_SelectedIndexChanged(object sender, EventArgs e) {
			string path = Path.Combine(txtPath.Text, (string)cmbArtist.SelectedItem);
			string []	files = Directory.GetDirectories(path);
			string []	Albums = new string[files.Length];
			int pathlen = path.Length + 1;	// See FillArtist for +1
			for (int i = 0; i < files.Length; i++) {
				Albums[i] = files[i].Substring(pathlen);
			}
			cmbAlbums.DataSource = Albums;
		}

//---------------------------------------------------------------------------------------

		private void cmbAlbums_SelectedIndexChanged(object sender, EventArgs e) {
			string	AlbumName = (string)cmbAlbums.SelectedItem;
			// string s = cmbArtist.SelectedText;	// TODO:
			string	path = Path.Combine(txtPath.Text, (string)cmbArtist.SelectedItem);
			path = Path.Combine(path, AlbumName);
			string[] TrackNames = Directory.GetFiles(path);
			for (int i = 0; i < TrackNames.Length; i++) {
				TrackNames[i] = TrackNames[i].Substring(path.Length + 1);
			}
			cmbTrack.DataSource = TrackNames;
		}

//---------------------------------------------------------------------------------------

		private void cmbTrack_SelectedIndexChanged(object sender, EventArgs e) {

			CurPath = Path.Combine(txtPath.Text, cmbArtist.Text);
			CurPath = Path.Combine(CurPath, cmbAlbums.Text);
			string TrackName = ((ComboBox)sender).Text;
			List<WmaTagBase>	tags = WmaTagBase.GetTags(Path.Combine(CurPath, TrackName));

			foreach (var tag in tags) {
				object o = tag.GetValue();
				Console.WriteLine("{0} = {1}", tag.TagName, tag.GetValue());
			}

			dataGridView1.DataSource = tags;
		}

//---------------------------------------------------------------------------------------

		private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			var x = dataGridView1.CurrentCell.Value;
			string Filename = Path.Combine(CurPath, cmbAlbums.Text);

#if false
			IWMMetadataEditor MetadataEditor;
			IWMHeaderInfo3 HeaderInfo3;

			WMFSDKFunctions.WMCreateEditor(out MetadataEditor);
			MetadataEditor.Open(Filename);
			HeaderInfo3 = (IWMHeaderInfo3)MetadataEditor;

			MetadataEditor.Close();
#endif
		}
	}
}
