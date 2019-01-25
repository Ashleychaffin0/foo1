using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Delimon.Win32.IO;
using DirectoryWalker;

using TagLib;

using WMPLib;

// http://stackoverflow.com/questions/9091/accessing-audio-video-metadata-with-net
// http://stackoverflow.com/questions/220097/read-write-extended-file-properties-c/2096315#2096315
// http://www.geekchamp.com/articles/reading-and-writing-metadata-tags-with-taglib
// https://www.nuget.org/packages/taglib/
//		Install-Package taglib
//		http://stackoverflow.com/questions/4361587/where-can-i-find-tag-lib-sharp-examples



namespace FindMusicDupsByName {
	public partial class FindMusicDupsByName : Form {

		Dictionary<string, List<MusicTrack>> MasterTracksList;

//---------------------------------------------------------------------------------------

		public FindMusicDupsByName() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void FindMusicDupsByName_Load(object sender, EventArgs e) {
#if DEBUG
			txtDirName.Text = @"D:\$ Zune Master - Copy for Phone Sync\$Folk\The New Lost City Ramblers";
#endif
		}

//---------------------------------------------------------------------------------------

		private void btnBrowse_Click(object sender, EventArgs e) {
			var fd = new FolderBrowserDialog();
			fd.SelectedPath = txtDirName.Text;
			var res = fd.ShowDialog();
			if (res == DialogResult.OK) {
				txtDirName.Text = fd.SelectedPath;
			}
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			MasterTracksList = new Dictionary<string, List<MusicTrack>>();

			if (! System.IO.Directory.Exists(txtDirName.Text)) {
				MessageBox.Show("The specified directory does not exist", "Find Music Dups By Name", 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			var dw = new DirectoryWalker.DirectoryWalker();
			var results = dw.Walk(txtDirName.Text);

#if false
			int n = 0;
			int nDir = 0;
#endif
			foreach (var item in results) {
				ProcessItem(item);
#if false
				++n;
				// Console.WriteLine("{0} {1}", n, item.FullName);
#endif
			}

			PruneSingletons();

			int nTracks = 0;
			foreach (var key in MasterTracksList.Keys) {
				nTracks += MasterTracksList[key].Count;
			}

			lblNumDupSongs.Text = string.Format("Number of duplicate songs: {0:N0}, duplicate track names: {1:N0}", 
				MasterTracksList.Keys.Count, nTracks);

			FillTree();
		}

//---------------------------------------------------------------------------------------

		private void FillTree() {
			tvTracks.Nodes.Clear();

			if (MasterTracksList.Count == 0) {
				MessageBox.Show("No duplicates found");
				return;
			}
#if false
			var Tracks = from track in masterTracksList.Keys
						 orderby track.t
#endif
			foreach (var key in MasterTracksList.Keys) {
				var MusicTrackList = MasterTracksList[key];
				string SongName = key + string.Format(" ({0} files)", MasterTracksList[key].Count);
				var SongNode = tvTracks.Nodes.Add(SongName);
#if false
				// TODO: Should use LINQ GroupBy
				var ByArtist = from track in MusicTrackList
							   orderby track.Artist, track.Title
							   select track;
#endif

				var ByArtistGrouped = from track in MusicTrackList
									  orderby track.Artist, track.Title
									  group track by track.Artist into grp
									  select grp;

				foreach (var grp in ByArtistGrouped) {
					int nGroups = ByArtistGrouped.Count();
					// var ArtistNode = SongNode.Nodes.Add(grp.Key + string.Format("( {0} group{1})",
					// 	nGroups, nGroups != 1 ? "s" : ""));
					var ArtistNode = SongNode.Nodes.Add(grp.Key);
					int nFiles = 0;
					foreach (var item in grp) {
						++nFiles;
						TreeNode FileNode = ArtistNode.Nodes.Add(item.DirectoryName);
						FileNode.Tag = item;
					}
					ArtistNode.Text = grp.Key + string.Format("( {0} file{1})",
						nFiles, nFiles != 1 ? "s" : "");
				}

#if false
				string PrevArtist   = "";
				int nFileByArtist   = 0;
				foreach (var item in ByArtist) {
					++nFileByArtist;
					if (PrevArtist != item.Artist) {
						ArtistNode = SongNode.Nodes.Add(item.Artist);
						PrevArtist = item.Artist;
					}
#if false
					else if (SongNode.Parent != null) {
						SongNode = SongNode.Parent;
					}
#endif
					TreeNode FileNode = ArtistNode.Nodes.Add(item.DirectoryName);
					FileNode.Tag = item;
				}
#endif
			}
		}


//---------------------------------------------------------------------------------------

		private void PruneSingletons() {
			string[] Keys = new string[MasterTracksList.Keys.Count];
			MasterTracksList.Keys.CopyTo(Keys, 0);
			for (int i = 0; i < Keys.Length; i++) {
				var key = Keys[i];
				if (MasterTracksList[key].Count < 2) {
					MasterTracksList.Remove(key);
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void ProcessItem(Delimon.Win32.IO.FileInfo item) {
			if ((item.Attributes & Delimon.Win32.IO.FileAttributes.Directory) == Delimon.Win32.IO.FileAttributes.Directory) {
				txtCurDir.Text = item.FullName;
				Application.DoEvents();
				return;
			}

			var Track = new MusicTrack(item);
			if (! Track.IsValid) {
				return;
			}
			string key = Track.Title;
			bool bIsThere = MasterTracksList.TryGetValue(key, out List<MusicTrack> list);
			if (bIsThere) {
				list.Add(Track);
			} else {
				MasterTracksList[key] = new List<MusicTrack>() { Track };
			}
		}

//---------------------------------------------------------------------------------------

		private void btnPlay_Click(object sender, EventArgs e) {
			Play();
		}

//---------------------------------------------------------------------------------------

		private void Play() {
			// http://stackoverflow.com/questions/15025626/playing-a-mp3-file-in-a-winform-application
			TreeNode node = tvTracks.SelectedNode;
			MusicTrack track = node.Tag as MusicTrack;
			if (track == null) {
				MessageBox.Show("Must select a subnode", "Play");
				return;
			}

			string filename = System.IO.Path.Combine(track.DirectoryName, track.Filename);
			axWindowsMediaPlayer1.URL = filename;
		}


//---------------------------------------------------------------------------------------

		private void btnStop_Click(object sender, EventArgs e) {
			Stop();
		}

//---------------------------------------------------------------------------------------

		private void Stop() {
			axWindowsMediaPlayer1.Ctlcontrols.stop();
		}

//---------------------------------------------------------------------------------------

		private void tvTracks_DoubleClick(object sender, EventArgs e) {
			Play();
		}

//---------------------------------------------------------------------------------------

		private void btnDelete_Click(object sender, EventArgs e) {
			TreeNode node = tvTracks.SelectedNode;
			MusicTrack track = node.Tag as MusicTrack;
			if (track == null) {
				MessageBox.Show("Must select a subnode", "btnDelete");
				return;
			}

			string filename = System.IO.Path.Combine(track.DirectoryName, track.Filename);
			var res = MessageBox.Show("About to delete " + filename, "Find Dups", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (res != DialogResult.Yes) {
				return;
			}
			Stop();
			Delimon.Win32.IO.File.Delete(filename);
			var Parent = node.Parent;
			Parent.Nodes.Remove(node);

			// TODO: Was trying to delete entire node if down to 1 entry.
			if (Parent.Nodes.Count < 2) {
				tvTracks.Nodes.Remove(Parent);
			}
		}

//---------------------------------------------------------------------------------------

		private void tvTracks_Click(object sender, EventArgs e) {
			TreeNode node = tvTracks.SelectedNode;
			if (node == null) {
				return;
			}
			MusicTrack track = node.Tag as MusicTrack;
			if (track == null) {
				// MessageBox.Show("Must select a subnode", "tvTracks_Click");
				return;
			}

			lblDuration.Text = string.Format(@"Duration: {0:dd\.hh\:mm\:ss}", track.Duration);
			lblFileSize.Text = string.Format("File Size: {0:N0}", track.FileSize);
			lblArtist.Text   = track.Artist;
		}
	}
}
