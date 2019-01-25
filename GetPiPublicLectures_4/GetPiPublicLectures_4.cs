/*
	For a long time I"ve suffered with the symptom that as downloads continued, sometimes
	a WebClient instance would hang. I"ve tried to dig into it to a certain extent, but
	I"ve never tried really hard since I could just restart the program and restart any
	stalled downloads. My guess was that there was a bug, probably deeply buried, in the
	.Net Framework. And I stumbled upon the following bug fix in an update to 
	.Net Core 2.1, which sounds like a perfect fit. The article at can be found at
	https://github.com/dotnet/corefx/commit/7ce9270ac7

	This article says: "On Windows, if Socket.Send/ReceiveAsync is used, and there"s a 
	race condition where the Socket is disposed of between the time that the Socket 
	checks for disposal and attempts to get a NativeOverlapped for use with the
	operation, the getting of the NativeOverlapped can throw an exception and cause us 
	to leave a field in an inconsistent state, which in turn can cause the operation to
	hang, waiting for that state to be reset."

	Now all we have to do is to wait as patiently as we can for this bug to be fixed
	in the main .Net Framework. But it"s now the end of June 2018, and according to
	Rich Landers comment in https://blogs.msdn.microsoft.com/dotnet/2018/06/06/announcing-net-framework-4-8-early-access-build-3621/,
	4.8 isn"t expected to ship until 2019!

	So hang in there (pun intended) for the time being, folks.
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using LRS.Utils;

/*
// TODO: Update these comments
The goal here is to programmatically download all the Perimeter Institute (PI) lectures.
We also want to be able to run this program periodically to be able to pick up new
lectures, without re-downloading ones we already have.

The programming is complicated by several facts.

	*	The information we need is kept in multiple sets of web pages
	*	The actual video files are on a different web site
	*	Some files are in .mp4 format, one is in .f4v (Flash) and some in .wmv
	*	There"s no API. We have to do screen scraping
	*	The PI web site is organized to view the lectures in streaming format. But we
		want to actually download them. To do this we need the PIRSA (see below) number
		and to get this we have to wander through several sub-pages.

//---------------------------------------------------------------------------------------

The basic information is the list of public lectures. This is kept in (as of the time
I"m writing this program, June 2018) 10 web pages, each with up to 10 lecture summaries.
Each page has the title of the lecture, the author, the date of the lecture, a 
description of the lecture (in most cases) and links to the files in various file
formats, the most useful being .mp4 and maybe .pdf

The Perimeter Institute records all their public lectures, but also many internal
seminars. All these videos are available for download at the Perimeter Institute Recorded
Seminar Archive (PIRSA.org), indexed by the PIRSA#. The good news is that the PIRSA# is
available from the streaming page. It"s the job of method GetPirsaNumbers() to go to each
Streaming Page and scrape that to find the PIRSA#. (Side note: Since we ultimately will
save the video file with the name of the lecture (and speaker and date stamp), we"ll
bypass the whole PIRSA processing if the target filename already exists.)

OK, we now have, for each lecture that needs downloading, the PIRSA#. We navigate to the
PIRSA.org site and plug 
TODO: Finish this

	TODO: Can we add Cancel support (see CancellationTokenSource?
	TODO: Get rid of all calls to Interlocked.*

*/

namespace GetPiPublicLectures_4 {
	public partial class GetPiPublicLectures_4 : Form {

		public string PiDir;				// Where the lectures are saved to disk

		SynchronizationContext sc;

		public List<Lecture> AllLectures;

		public static GetPiPublicLectures_4 Main;   // For communicating between classes

		// We refer to these as <ref> in Lecture.cs. Marking them as public static
		// means they'll be readily available to other classes.
		public static int nLecturesToDownload;
		public static int nLecturesDownloaded;

		internal PiConfigData cfg;

		internal StreamWriter wtr;

//---------------------------------------------------------------------------------------

		public GetPiPublicLectures_4() {
			InitializeComponent();

			Main = this;

			sc   = SynchronizationContext.Current;

			GetConfigParms();

#if DEBUG // Generate random directory name
			var rnd = new Random();
			while (true) {
				int num = rnd.Next(1_000, 1_000_000);
				var NewPath = Path.Combine(txtTargetDir.Text, $"---{num}");
				if (Directory.Exists(NewPath)) { continue; }
				txtTargetDir.Text = NewPath;
				break;
			}
#endif
		}

//---------------------------------------------------------------------------------------

		private void GetConfigParms() {
			cfg                  = PiConfigData.LoadConfig();
			cfg.DefaultTargetDir = cfg.DefaultTargetDir.Trim();
			txtTargetDir.Text    = cfg.DefaultTargetDir;
			int nCoreMultiplier  = cfg.ProcessorCountMultiplier;
			nCoreMultiplier		 = nCoreMultiplier == 0 ? 2 : nCoreMultiplier;
			int nSimul           = cfg.NumberOfSimultaneousDownloads;
			nSimul               = nSimul == 0 ? nCoreMultiplier * Environment.ProcessorCount : nSimul;
			UdConcurrency.Value  = nSimul;

			// Set filetype check boxes. Not the most efficient code in the world,
			// but good enough
			foreach (var item in cfg.FileTypes) {
				ChkMp4.Checked      |= item == ".mp4";
				ChkLoResMp4.Checked |= item == ".LoRes.mp4";
				ChkMp3.Checked      |= item == ".mp3";
				ChkPdf.Checked      |= item == ".pdf";
			}
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			// First, reset everything that needs resetting
			AllLectures = new List<Lecture>();

			PiDir = txtTargetDir.Text;

			if (wtr != null) { CloseWtr(); }

			nLecturesToDownload = 0;
			nLecturesDownloaded = 0;
			SetProgressBar();

			lbMsg.Items.Clear();
			flowLayoutPanel1.Controls.Clear();
			Directory.CreateDirectory(PiDir);       // Just in case...
			ServicePointManager.DefaultConnectionLimit = (int)UdConcurrency.Value; // Simultaneous downloads

			// OK, now get to work!
#pragma warning disable CS4014		// Wants an await
			GetPiPages();
#pragma warning restore CS4014
		}

//---------------------------------------------------------------------------------------

		private void CloseWtr() {
			if (wtr is null) { return; }
			wtr.WriteLine("</body>");
			wtr.WriteLine("</html>");
			wtr.Close();
			wtr = null;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Downloads all PI lecture pages asynchronously. Returns when all are done.
		/// Also, for each page, updates <code>AllLectures</code> with all the
		/// relevant (e.g. .mp4, but perhaps not .mp3) lecture files.
		/// </summary>
		/// <returns>Task, which is effectively the async version of void</returns>
		private async Task GetPiPages() {
			var PageTasks  = new List<Task<ScrapePiPage>>();

			// Process the first page specially so we know how many pages to process
			var spp0 = new ScrapePiPage();		// spp0 = ScrapePiPage0
			await spp0.GetPiPage(0, 0);
			int MaxPageNum = spp0.MaxPageNum;
			spp0.DoAllDownloads();

			// Now do the rest of the pages
			for (int PageNum = 1; PageNum <= MaxPageNum; PageNum++) {
				var spp = new ScrapePiPage();
				Task<ScrapePiPage> t = spp.GetPiPage(PageNum, MaxPageNum);
				// t.ContinueWith(xxnn => Msg($"ContinueWith[page {PageNum}] -- MaxPages = {xxnn}"));
				PageTasks.Add(t);
				Application.DoEvents();
			}

			// We"ve already started downloading from Page 0. Now wait for each
			// page in turn to finish and start downloading them
			foreach (var task in PageTasks) {
				var spp = await task;		// spp = ScrapedPiPage
				spp.DoAllDownloads();
			}
		}

//---------------------------------------------------------------------------------------

		internal void SetProgressBar() {
			lblProgress.Text     = $"Downloaded {nLecturesDownloaded} of {nLecturesToDownload}";
			progressBar1.Maximum = nLecturesToDownload;
			progressBar1.Value   = nLecturesDownloaded;
			Application.DoEvents();
		}

//---------------------------------------------------------------------------------------

		internal void Msg(string text) {
			string hdr = DateTime.Now.ToLongTimeString() + " ";
			PerformUIOperation(lbMsg, () => lbMsg.Items.Insert(0, hdr + text));
			Application.DoEvents();
		}

//---------------------------------------------------------------------------------------

		[Conditional("DEBUG")]
		public void DebugMsg(string text) {
			string hdr = "DEBUGMSG: " + DateTime.Now.ToLongTimeString() + " ";
			PerformUIOperation(lbMsg, () => {
				lbMsg.Items.Insert(0, hdr + text);
				Application.DoEvents();
			});
		}

//---------------------------------------------------------------------------------------

		internal void PerformUIOperation(Control ctl, Action action) {
			if (ctl.InvokeRequired) { ctl.Invoke(action); } 
			else { action(); }
		}

//---------------------------------------------------------------------------------------

		private void btnBrowse_Click(object sender, EventArgs e) {
			var dlg = new FolderBrowserDialog();
			MessageBox.Show("Nonce on Browse");
			// dlg.RootFolder = "";
			// TODO: Implement btnBrowse_Click
		}

//---------------------------------------------------------------------------------------

		private void CopyMsgsToClipboard_Click(object sender, EventArgs e) {
			var msgs = new List<string>();
			for (int i = lbMsg.Items.Count - 1; i >= 0; i--) {
				msgs.Add(lbMsg.Items[i].ToString());
			}
			Clipboard.SetText(string.Join(Environment.NewLine, msgs));
		}

//---------------------------------------------------------------------------------------

		private void BtnDownloadFolder_Click(object sender, EventArgs e) {
			Process.Start("Explorer.exe", txtTargetDir.Text);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Go through all items in the FlowLayoutPanel. Move all that are flagged as
		/// done to the end of the panel, thus keeping the active downloads visible
		/// at the front of the list.
		/// </summary>
		public void MoveCompletedDownloadsToEndOfList() {
			lock (flowLayoutPanel1) {
				int n = flowLayoutPanel1.Controls.Count;
				flowLayoutPanel1.SuspendLayout();
				int i = -1;
				foreach (DownloadFileProgress item in flowLayoutPanel1.Controls) {
					++i;
					if (item.IsDownloadDone && !item.IsRelocated) {
						// Move to end
						flowLayoutPanel1.Controls.SetChildIndex(item, n - 1);
						item.IsRelocated = true;
					}
				}
				flowLayoutPanel1.ResumeLayout();
			}
			Application.DoEvents();
		}

//---------------------------------------------------------------------------------------

		private void GetPiPublicLectures_4_FormClosed(object sender, FormClosedEventArgs e) {
			CloseWtr();
		}
	}
}
