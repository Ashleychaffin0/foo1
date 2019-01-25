using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DownloadMsConferences {
	public partial class DownloadMsConferences : Form {

//---------------------------------------------------------------------------------------

		private void SetupElapsedTimeTimer() {
			// Start the time ticking to update the display
			tmr = new System.Windows.Forms.Timer();
			tmr.Interval = 1000;
			tmr.Tick += Tmr_Tick;
			StartTime = DateTime.Now;
			tmr.Start();
		}

//---------------------------------------------------------------------------------------

		private void Tmr_Tick(object sender, EventArgs e) {
			string msg = $"Elapsed: {(DateTime.Now - StartTime):hh\\:mm\\:ss}";
			if (sc != SynchronizationContext.Current) {
				sc.Send(o => { lblElapsed.Text = msg; Application.DoEvents(); }, null);
			} else {
				lblElapsed.Text = msg;
				Application.DoEvents();
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// OK, here's the routine that starts the ball rolling
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void btnGo_Click(object sender, EventArgs e) {
			btn_Go.Enabled = false;     // Can't re-click in middle of processing

			flowLayoutPanel1.Controls.Clear();

			var RssDoc = await GetRssDocAsync();

			CurConf = (Conference)cmbConf.SelectedItem;

			var Title = CurConf.Name;
			msg($"Retrieving list of sessions for conference {Title}");
			CurConf.SetupTopics(cfg);

			TargetDir = txtTargetDir.Text;
			// Create our main target directory. NOP if it already exists
			Directory.CreateDirectory(TargetDir);

			bDoneEnumeratingFiles = false;

			nFiles     = 0;
			nFilesDone = 0;

			lblElapsed.Text   = "";
			lblFilesToGo.Text = "";
			SetupElapsedTimeTimer();

			// Another timer for different elapsed time display
			var sw = new Stopwatch();
			sw.Start();

			// Call a major routine. It will read and parse the XML in the RSS file
			// and start the downloads for all relevant files
			DoDownloadsAsync(RssDoc);
			sw.Stop();

			bDoneEnumeratingFiles = true;
			Application.DoEvents();
			// msg($"Found {nFiles} file{s_nFiles} to download in {sw.Elapsed}");
			if (nFiles == 0) {
				btn_Go.Enabled = true;
			}
		}

//---------------------------------------------------------------------------------------

		private void btnOpenBuildFolder_Click(object sender, EventArgs e) {
			// See https://support.microsoft.com/en-us/kb/152457
			Process.Start("explorer.exe", "/root,\"" + txtTargetDir.Text + "\"");
		}

//---------------------------------------------------------------------------------------

		private void cmbConf_SelectedIndexChanged(object sender, EventArgs e) {
			var cmb  = sender as ComboBox;
			var conf = (Conference)cmb.SelectedItem;

			txtTargetDir.Text = conf.TargetDir;
			RssUrl            = conf.RssUrl;
		}

//---------------------------------------------------------------------------------------

		private void btnBrowse_Click(object sender, EventArgs e) {
			var fbd                 = new FolderBrowserDialog();
			fbd.SelectedPath        = txtTargetDir.Text;
			fbd.Description         = "Select the folder to download the files into";
			fbd.ShowNewFolderButton = true;
			var dlgResult           = fbd.ShowDialog();
			if (dlgResult == DialogResult.OK) {
				TargetDir = fbd.SelectedPath;
				txtTargetDir.Text = TargetDir;
			}
		}

//---------------------------------------------------------------------------------------

		private void Download_Build_2015_via_HtmlAgilityPack_2_Load(object sender, EventArgs e) {
			toolTip1.SetToolTip(btn_Go,				"Run the program");
			toolTip1.SetToolTip(btnOpenBuildFolder, "Open the download folder for the specified conference");
			toolTip1.SetToolTip(lbMsgs,				"Messages from the program, most recent ones at the beginning");
			toolTip1.SetToolTip(flowLayoutPanel1,	"Dynamic display of download progress");
			toolTip1.SetToolTip(chkSlides,			"Whether slides should be downloaded");
			toolTip1.SetToolTip(chkVideos,			"Whether videos should be downloaded");
			toolTip1.SetToolTip(chkWmv,				"Whether .wmv should be prferred over .mp4");
			toolTip1.SetToolTip(cmbConf,			"The list of conferences supported by the program");
			toolTip1.SetToolTip(lblFilesToGo,		"Summary of the number of files to download");
			toolTip1.SetToolTip(pbFilesToGo,		"Progress of the whole download process");
			toolTip1.SetToolTip(pbFilesToGo,		 "Progress of the whole download process");
			toolTip1.SetToolTip(lblElapsed,			"Elapsed time from clicking Go. Stops when all downloads completed");
			toolTip1.SetToolTip(txtTargetDir,		"The base directory where files are to be downloaded. Editable.");
			toolTip1.SetToolTip(btnBrowse,			"Browse for the base directory where files are to be downloaded");
		}

//---------------------------------------------------------------------------------------

		private void ShowFilesToGo() {
			lblFilesToGo.Text = $"Files done: {nFilesDone} of {nFiles}";
			if (nFiles > 0) {
				pbFilesToGo.Maximum = nFiles;
				pbFilesToGo.Value = nFilesDone;
			}
			Application.DoEvents();
		}

//---------------------------------------------------------------------------------------

		private void msg(string s) {
			// Add this message to the top of the listbox
			lbMsgs.Items.Insert(0, s);
			Application.DoEvents();
		}
	}
}

