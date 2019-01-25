// #define OUTPUT_HTML

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

using LRS.Utils;

namespace CoreDownloadMsConferences {
	public partial class CoreDownloadMsConferences : Form {

//---------------------------------------------------------------------------------------

		private void SetupElapsedTimeTimer() {
			// Start the time ticking to update the display
			tmr = new System.Windows.Forms.Timer() {
				Interval = 1000
			};
			tmr.Tick += Tmr_Tick;
			StartTime = DateTime.Now;
			tmr.Start();
			// Thread.Sleep(tmr.Interval);
		}

//---------------------------------------------------------------------------------------

		private void Tmr_Tick(object sender, EventArgs e) {
			string msg = $"Elapsed: {(DateTime.Now - StartTime):hh\\:mm\\:ss}";
			if (scGui != SynchronizationContext.Current) {
				scGui.Send(o => { lblElapsed.Text = msg; Application.DoEvents(); }, null);
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
		private async void BtnGo_Click(object sender, EventArgs e) {
			btn_Go.Enabled = false;     // Can't re-click in middle of processing

			flowLayoutPanel1.Controls.Clear();
			pbFilesToGo.Value = 0;

			var RssDoc = await GetRssDocAsync();

			CurConf = (Conference)cmbConf.SelectedItem;

			int nSimDls = (int)NumSimulDownloads.Value;
			ServicePointManager.DefaultConnectionLimit = nSimDls;

			var Title = CurConf.Name;
			msg($"Retrieving list of sessions for conference {Title}");
			CurConf.SetupTopics(cfg);

			TargetDir = txtTargetDir.Text;
			// Create our main target directory. NOP if it already exists
			Directory.CreateDirectory(TargetDir);

			bDoneEnumeratingFiles = false;

			nFiles			= new InterlockedInt(0, "nFiles");
			nFilesDone		= new InterlockedInt(0, "nFilesDone");
			nFilesSkipped	= 0;
			nVideosNotFound = 0;

			lblElapsed.Text   = "";
			lblFilesToGo.Text = "";
			SetupElapsedTimeTimer();

			// Another timer for different elapsed time display
			var sw = new Stopwatch();
			sw.Start();

			// Call a major routine. It will read and parse the XML in the RSS file
			// and start the downloads for all relevant files
			DownloadRssAsync(RssDoc);
			sw.Stop();

			Application.DoEvents();
		}

//---------------------------------------------------------------------------------------

		private void BtnOpenBuildFolder_Click(object sender, EventArgs e) {
			// See https://support.microsoft.com/en-us/kb/152457
			Process.Start("explorer.exe", "/root,\"" + txtTargetDir.Text + "\"");
		}

//---------------------------------------------------------------------------------------

		private void CmbConf_SelectedIndexChanged(object sender, EventArgs e) {
			var cmb  = (sender as ComboBox)!;
			var conf = ((Conference)cmb.SelectedItem)!;

			if (conf.TargetDir == null) {
				txtTargetDir.Text = Path.Combine(cfg.DefaultTargetDir, conf.Name);
			} else {
				txtTargetDir.Text = conf.TargetDir;
			}
			RssUrl = conf.RssUrl;
		}

//---------------------------------------------------------------------------------------

		private void BtnOpenHomePage_Click(object sender, EventArgs e) {
			string BaseUrl = ((cmbConf.SelectedItem as Conference))!.RssUrl.ToUpper();
			Process.Start(BaseUrl.Replace("/RSS", "").Replace("MP4", ""));
		}

//---------------------------------------------------------------------------------------

		private void ChkSkeletonOnly_CheckedChanged(object sender, EventArgs e) {
			chkSlides.Enabled = !chkSkeletonOnly.Checked;
			chkVideos.Enabled = !chkSkeletonOnly.Checked;
		}

//---------------------------------------------------------------------------------------

		private void BtnBrowse_Click(object sender, EventArgs e) {
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

		private void DownloadMsConferences_Load(object sender, EventArgs e) {
			toolTip1.SetToolTip(btn_Go,				"Run the program");
			toolTip1.SetToolTip(btnOpenBuildFolder, "Open the download folder for the specified conference");
			toolTip1.SetToolTip(btnOpenHomePage,    "Open the home page for the specified conference");
			toolTip1.SetToolTip(lbMsgs,				"Messages from the program, most recent ones at the beginning");
			toolTip1.SetToolTip(flowLayoutPanel1,	"Dynamic display of download progress");
			toolTip1.SetToolTip(chkSlides,			"Whether slides should be downloaded");
			toolTip1.SetToolTip(chkVideos,			"Whether videos should be downloaded");
			toolTip1.SetToolTip(chkSkeletonOnly,	"Don't download data, just create links");
			toolTip1.SetToolTip(lblSimDownloads,	"Number of files to download in parallel");
			toolTip1.SetToolTip(NumSimulDownloads,	"Number of files to download in parallel");
			toolTip1.SetToolTip(cmbConf,			"The list of conferences supported by the program");
			toolTip1.SetToolTip(lblFilesToGo,		"Summary of the number of files to download");
			toolTip1.SetToolTip(pbFilesToGo,		"Progress of the whole download process");
			toolTip1.SetToolTip(lblElapsed,			"Elapsed time from clicking Go. Stops when all downloads completed");
			toolTip1.SetToolTip(txtTargetDir,		"The base directory where files are to be downloaded. Editable.");
			toolTip1.SetToolTip(btnBrowse,			"Browse for the base directory where files are to be downloaded");
		}

//---------------------------------------------------------------------------------------

		private void ShowFilesToGo() {
			lblFilesToGo.Text = $"Files done: {(int)nFilesDone} of {(int)nFiles}";
			if (nFiles > 0) {
				if (nFilesDone > pbFilesToGo.Maximum) {
					dbg($"In ShowFilesToGo -- nFiles={(int)nFiles}, pbFileToGo.Maximum={pbFilesToGo.Maximum}");
					DbgDumpMsgsToClipboard();
					pbFilesToGo.Maximum = nFiles * 2;   // TODO: BandAid
					Debugger.Break();
				} else {
					pbFilesToGo.Maximum = nFiles;
					pbFilesToGo.Value = nFilesDone;
				}
			}
			// Application.DoEvents();
		}

		[Conditional("DEBUG")]
		public void dbg(string s) {
			msg(s);
		}

//---------------------------------------------------------------------------------------

		public void msg(string s /*, string Tag = "" */) {
			// TODO: Worry about OUTPUT_HTML later
#if OUTPUT_HTML
			s = $"<{tag}>";
			s = s.Replace(" ", "&nbsp;");
			s += $"</{tag}>";
#endif
			// Add this message to the top of the listbox
			lbMsgs.Items.Insert(0, s);
			DbgDumpMsgsToClipboard();
			Thread.Sleep(10);
			Application.DoEvents();
		}
	}
}

