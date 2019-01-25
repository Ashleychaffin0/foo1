using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml.Linq;

// Note: To activate some proposed C# 7.0 features, see https://www.visualstudio.com/news/vs15-preview-vs
//		 Also see http://www.strathweb.com/2016/03/enabling-c-7-features-in-visual-studio-15-preview/
// Note: Known problems in VS15 - https://msdn.microsoft.com/en-US/library/05e0e20e-7ace-41fe-b0bc-3becc37ae618

// The HTML Agility Pack is an non-Microsoft open source package that can parse an HTML
// file and give us a Document Object Model (DOM) of it. We use this for web scraping.
using HAP = HtmlAgilityPack;

// See http://www.codeproject.com/Articles/609053/AngleSharp
// See http://blogs.msdn.com/b/cdndevs/archive/2015/12/17/web-scraping-in-c.aspx
// See http://www.codeproject.com/Tips/804660/How-to-Parse-HTML-using-Csharp
// See http://www.alimozdemir.com/htmlagilitypack-csquery-and-anglesharp-comparison/

// For network logging, see https://msdn.microsoft.com/en-us/library/hyb3xww8%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396

// We want to wind up with as follows (assuming all user options turned on)
// TODO: Check these
//	*	In the primary directory
//		*	*.pptx - CHECK
//		*	*.mp4 (or .wmv) - CHECK
//		*	*.url (link back to Channel 9 page for this session) - CHECK
//	*	In the secondary directory(s)
//		*	Link to the .pptx file - CHECK
//		*	Link to the .mp4 file - CHECK
// TODO:		*	Link to the Channel 9 page for this session (url)
// TODO:	* Tags
//			* Level
//			* Session Types

namespace DownloadMsConferences {
	public partial class DownloadMsConferences : Form {

		// The following URL has a list of all conference sessions in RSS/XML format
		string RssUrl;

		// Where I want the videos and slides to go by default
		string TargetDir;

		// Configuration info
		ConfigData cfg;

		// Current Conference
		Conference CurConf;

		// For giving the user some progress info
		int  nSessions;                  // # of sessions
		int  nFiles;					 // # of files to download
		int  nFilesDone;				 // # of files left to download
		bool bDoneEnumeratingFiles;      // Set when we've started all download we're going to

		// For status updates and elapsed time. Note that we've fully qualified the "tmr"
		// variable with all it's namespaces, since there's more than one Timer class in
		// .Net. This is the one we want.
		System.Windows.Forms.Timer tmr;
		DateTime StartTime;

		// Note: At one point during development, somehow we got off the UI thread and
		//		 needed to update a control (usually a Label or TextBox). So I threw in
		//		 the following SynchronizationContext. But now that everything's back on
		//		 the UI thread, we don't need this any more. But I think I'll keep it in
		//		 as an example of how to use it, if/when necessary.
		SynchronizationContext sc;

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

		private class RssItem {
			// Just the fields we need from each <item> in the RSS feed
			public string Title;
			public string Link;
			public string Description;
		}

//---------------------------------------------------------------------------------------

		public DownloadMsConferences() {
			InitializeComponent();

			ServicePointManager.DefaultConnectionLimit = 10;

			sc = SynchronizationContext.Current;

			// PutConfigInfo();						// TODO:
			cfg = ConfigData.GetConfig();

			cmbConf.DataSource = cfg.Conferences;   // Fill combo box

#if DEBUG  // Note: Keep this #if. Feature not in Release builds, but is in Debug builds
			var menu = new MenuStrip();
			var menu_CopyMsgs = new ToolStripMenuItem("Copy messages");
			menu_CopyMsgs.Click += new System.EventHandler((sender, e) => {
				dbgDumpMsgsToClipboard();
			});
			menu.Items.Add(menu_CopyMsgs);
			this.Controls.Add(menu);
			this.MainMenuStrip = menu;
#endif
		}

//---------------------------------------------------------------------------------------

		[Conditional("DEBUG")]
		private void dbgDumpMsgsToClipboard() {
			var sb = new StringBuilder();
			for (int i = lbMsgs.Items.Count - 1; i >= 0; i--) {
				sb.AppendLine((string)lbMsgs.Items[i]);
			}
			Clipboard.SetText(sb.ToString());
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// This routine gets the RSS/XML file for the specidied conference. Note that
		/// I didn't *have to* use await. Nothing is really gained by making this routine
		/// asynchronous. But I haven't used 'await' much, and I'm just trying to get a
		/// bit more experience with it.
		/// </summary>
		/// <returns>
		/// A Task<XDocument>. IOW, at some point in the future, an XDocument will be
		/// available
		/// </returns>
		private async Task<XDocument> GetRssDocAsync() {
			lbMsgs.Items.Clear();           // Erase all previous entries

			// The RssDoc contents would be (in small part)
			//	<item>
			//		<title> Windows Design for Developers: Getting the Balance Right </title>
			//		<description><![CDATA[<p> Come hear more about Microsoft’s
			//			approach to design: ... and so on ...</ description>
			//			<img src ="http://m.webtrends.com/...">]]>
			//		</description>
			//		<comments>http://s.ch9.ms/Events/Build/2015/2-791</comments>
			//		<link>http://s.ch9.ms/Events/Build/2015/2-791</link>
			// Other elements...
			//	</item>
			//	<item> ... </item>
			//	<item> ... </item>
			//	<item> ... </item>

			var wreq        = (HttpWebRequest)WebRequest.CreateHttp(RssUrl);
			var RssResponse = await wreq.GetResponseAsync();
			var RssStream   = RssResponse.GetResponseStream();
			var RssDoc      = XDocument.Load(RssStream);
			return RssDoc;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Go through the RSS/XML file and pick up just the info we need, which is the
		/// title of the session and the URL of its web page. We'll then scrape this
		/// looking for the video and/or slideshow file(s) we want
		/// </summary>
		/// <param name="RssDoc">The RSS file</param>
		private async void DoDownloadsAsync(XDocument RssDoc) {
			var qrySessionItems = from item in RssDoc.Element("rss").Element("channel").Elements("item")
								  select new RssItem {
									  Title       = CleanTitle(item.Element("title").Value),
									  Link        = item.Element("link").Value,
									  Description = item.Element("description").Value
								  };
			// Note: We only need the three elements, Title, Link and Description from
			//		 each <item>

			nFiles    = 0;
			nSessions = 0;
			foreach (var item in qrySessionItems) {
#if false	// For testing on just one session
				// Visual Studio 2015 Final Release Event Sessions
				// if (item.Title != "Building cross-platform mobile apps using C# and Visual Studio 2015") {
				if (! item.Title.ToUpper().Contains("NODE.JS")) {
					continue;
				}
#endif
				++nSessions;
				var PageInfo = await ScrapeChannel9Page.ScrapeAsync(item.Link);
				ProcessSession(item, PageInfo);			// OK, look for our files
			}
			msg($"Finished getting the list of {nSessions} sessions");
		}

		//---------------------------------------------------------------------------------------

		/// <summary>
		/// Scrape the HTML for the current session page. We'll eventually wind up with
		/// a link to the highest-res video file (if we've been asked to download videos,
		/// and/or a link to the PowerPoint slide show for this session (if it exists).
		/// We'll then download either/both.
		/// </summary>
		/// <param name="item">An RSS item element</param>
		private void ProcessSession(RssItem item, ScrapeChannel9Page.ScrapedInfo PageInfo) {
			CountFilesToDownload(PageInfo);

			if (PageInfo.VideoLink == null) {
				msg($"Can't find a video to download for Title={item.Title}");
				return;
			}

			List<string> SecondaryTopics;
			string PrimaryTopicPath = CurConf.CategorizeSessionTopics(item.Title, out SecondaryTopics);
			if (PrimaryTopicPath == null) {
				PrimaryTopicPath = TargetDir;
			}
			PrimaryTopicPath = Path.Combine(TargetDir, PrimaryTopicPath);
			Directory.CreateDirectory(PrimaryTopicPath);

			CreateShortcutToUrl(PrimaryTopicPath, item);

			// OK, download the Video and/or the slides for it
			if (chkSlides.Checked) {
				DownloadFile(PageInfo.SlidesLink, PrimaryTopicPath, item.Title, item.Description, item.Link);
				CreateSecondaryLinks(PrimaryTopicPath, SecondaryTopics, item, PageInfo.SlidesLink);
			}
			if (chkVideos.Checked) {
				DownloadFile(PageInfo.VideoLink, PrimaryTopicPath, item.Title, item.Description, item.Link);
				CreateSecondaryLinks(PrimaryTopicPath, SecondaryTopics, item, PageInfo.VideoLink);
			}

			CreateLinksForTagsLevelSessionTypes(TargetDir, PageInfo, item);
		}

//---------------------------------------------------------------------------------------

		private static void CreateLinksForTagsLevelSessionTypes(string TargetDir, ScrapeChannel9Page.ScrapedInfo PageInfo, RssItem Item) {
			// Let's pretend that a given session has tags: .Net, Visual Studio and
			// Diagnostics. So we want to create a top-level directory (but under
			// TargetDir (e.g. D:\MsConferences\Build 2015)) called $Tags. Under it will
			// be subdirs called .Net, Visual Studio and Diagnostics. And each of these
			// will have zero or more triples of links -- to the session itself, to the
			// .pptx file (if present) and to the .mp4/.wmv file (if present).
			foreach (var TagEtAl in PageInfo.TagsEtAl) {
				string InThisDir = Path.Combine(TargetDir, TagEtAl.Key);
				Directory.CreateDirectory(InThisDir);
				// TODO: Refactor. Code seems the same for all 3 cases
				switch (TagEtAl.Key) {
				case ScrapeChannel9Page.TagName:
					foreach (var tag in PageInfo.TagsEtAl[TagEtAl.Key]) {
						string TagDir = Path.Combine(InThisDir, tag);
						Directory.CreateDirectory(TagDir);
						CreateShortcutToUrl(TagDir, Item);
					}
					break;
				case ScrapeChannel9Page.LevelName:
					foreach (var tag in PageInfo.TagsEtAl[TagEtAl.Key]) {
						string TagDir = Path.Combine(InThisDir, tag);
						Directory.CreateDirectory(TagDir);
						CreateShortcutToUrl(TagDir, Item);
					}
					break;
				case ScrapeChannel9Page.SessionTypeName:
					foreach (var tag in PageInfo.TagsEtAl[TagEtAl.Key]) {
						string TagDir = Path.Combine(InThisDir, tag);
						Directory.CreateDirectory(TagDir);
						CreateShortcutToUrl(TagDir, Item);
					}
					break;
				default:
					break;
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void CountFilesToDownload(ScrapeChannel9Page.ScrapedInfo PageInfo) {
			// Count (carefully) the number of files we have to download.
			// Assume that all slides are .pptx (as opposed to, say, .ppt) and that
			// videos are .mp4 or .wmv
			if (chkSlides.Checked && null != PageInfo.SlidesLink?.EndsWith(".pptx")) {
				Interlocked.Increment(ref nFiles);
			}
			if (chkVideos.Checked &&
					   ((null != PageInfo.VideoLink?.EndsWith(".mp4"))
					||  (null != PageInfo.VideoLink?.EndsWith(".wmv")))) {
				Interlocked.Increment(ref nFiles);
			}
		}

//---------------------------------------------------------------------------------------

		private void CreateSecondaryLinks(string PrimaryTopicPath, List<string> SecondaryTopics, RssItem item, string Url) {
			if (Url == null) {
				return;
			}
			foreach (string topic in SecondaryTopics) {
				string InThisDir = Path.Combine(TargetDir, topic);
				string ToThisDir = Path.Combine(TargetDir, PrimaryTopicPath);
				Directory.CreateDirectory(InThisDir);
				CreateShortcutToFile(InThisDir, ToThisDir, item.Title, GetUrlSuffix(Url));
				CreateShortcutToUrl(InThisDir, item);
			}
		}

//---------------------------------------------------------------------------------------

		private static async void CreateShortcutToFile(
			// See http://www.fmtz.com/formats/url-file-format/article
			string InThisDirectory,
			string RefersToThatDirectory,
			string Title,
			string Suffix) {
			string PageShortCut = $@"[InternetShortCut]
URL=file:///{RefersToThatDirectory}/{Title}{Suffix}
";
			PageShortCut    = PageShortCut.Replace('\\', '/').Replace(" ", "%20");
			string Filename = Path.Combine(InThisDirectory, $"{Title}{Suffix}.url");
			await WritePageShortcut(PageShortCut, Filename);
		}

//---------------------------------------------------------------------------------------

		private static async void CreateShortcutToUrl(string InThisDirectory, RssItem item) {
			// Note: This routine is almost identical to CreateShortcutToFile.
			string PageShortCut = $@"[InternetShortCut]
URL={item.Link}
";
			PageShortCut = HttpUtility.HtmlEncode(PageShortCut).Replace(" ", "%20");
			string Suffix = GetUrlSuffix(item.Link);
			string Filename = Path.Combine(InThisDirectory, $"{item.Title}{Suffix}.url");
			await WritePageShortcut(PageShortCut, Filename);
		}

//---------------------------------------------------------------------------------------

		private static async Task WritePageShortcut(string PageShortCut, string Filename) {
			if (!File.Exists(Filename)) {
				using (StreamWriter sw = File.CreateText(Filename)) {
					await sw.WriteAsync(PageShortCut);
				}
			}
		}

//---------------------------------------------------------------------------------------

		private static string GetUrlSuffix(string Url) => "." + Url.Split('/').Last().Split('.').Last();

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Start the asynchronous download the specified file.
		/// </summary>
		/// <param name="Url">Where on the web the file is</param>
		/// <param name="TargetDir">Where on disk it will land</param>
		/// <param name="Title">The session title</param>
		/// <param name="Description">The session description</param>
		/// <param name="SessionUrl">The Url of the web page for the session, so we can
		/// write out a .url link to it
		/// </param>
		private void DownloadFile(string Url, string TargetDir, string Title, string Description, string SessionUrl) {
			if (Url == null) {
				return;             // e.g. no Slides found
			}

			// The suffix (.mp4 or .wmv or .pptx) can be derived from the Url
			// For example, get ".mp4" from "http://video.ch9.ms/sessions/build/2015/2-612-LG.mp4"
			string Suffix   = GetUrlSuffix(Url);
			string filename = Title + Suffix;
			var FullName    = Path.Combine(TargetDir, filename);
			if (File.Exists(FullName) && new FileInfo(FullName).Length > 0) {
				// This supports restartability of the app. Don't download things again.
				msg("        Skipping " + filename);
				Interlocked.Decrement(ref nFiles);
				return;
			}
			msg($"Starting file download for: {filename}");

			var wc = new TitledWebClient(filename, FullName, Description, SessionUrl);
			// Our event handlers
			wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
			wc.DownloadFileCompleted   += Wc_DownloadFileCompleted;

			wc.Progress = new DownloadProgress(wc);
			// Add a new custom progress control to our Flow Layout Panel
			flowLayoutPanel1.Controls.Add(wc.Progress);
			Application.DoEvents();     // Make sure UI updates
			
			ShowFilesToGo();            // Status update

			// In addition to *.mp4/*.wmv and *.pptx, write out <Title>.url that, when
			// clicked, will take you to the web page for this session
			// TODO: WritePageShortcutFile(SessionUrl, TargetDir, Title, "");
			// TODO: Is the line above implemented somewhere else? Or at all?

			// OK, here's where the download is actually done. Notice it's asynchronous,
			// so it will return immediately if we're not at the maximum number of
			// simulataneous downloads allowed. Then it will block until one of the
			// previous downloads has finished.

			// Note that this method returns void, so we can't "await" it (which needs a
			// method that returns a Task<>. So this is "fire and forget". But that's OK.
			// Our two events will fire with progress info and also when the download is
			// complete. 
			wc.DownloadFileAsync(new Uri(Url), FullName);
		}

//---------------------------------------------------------------------------------------

		private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
			var wc = sender as TitledWebClient;
			wc.Progress.UpdateProgress(e);
		}

//---------------------------------------------------------------------------------------

		private void Wc_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e) {
			var wc = sender as TitledWebClient;
			if (e.Error != null) {
				msg($"******** Error on {wc.FullFilename} -- ({e.Error}) in DownloadFileCompleted)");
				PaintItBlack_er_Cyan(wc);
				File.Delete(wc.FullFilename);
			} else {
				// Set Last Write Time on file based on Response Header
				var LastUsed = DateTime.Parse(wc.ResponseHeaders["Last-Modified"]);
				File.SetLastWriteTime(wc.FullFilename, LastUsed);
			}
			Interlocked.Increment(ref nFilesDone);
			ShowFilesToGo();

			wc.Progress.DownloadDone(e);

			if (bDoneEnumeratingFiles && (nFilesDone == nFiles)) {
				tmr.Stop();
				msg("Done!");
				btn_Go.Enabled = true;
			}
		}

//---------------------------------------------------------------------------------------
		
		private static void PaintItBlack_er_Cyan(TitledWebClient wc) {
			wc.Progress.lblTotalSize.BackColor        = Color.Cyan;
			wc.Progress.lblAmountDownloaded.BackColor = Color.Cyan;
			wc.Progress.lblDownloadSpeed.BackColor    = Color.Cyan;
			wc.Progress.lblPercentDone.BackColor      = Color.Cyan;
			wc.Progress.linkLabel1.BackColor          = Color.Cyan;
		}

		//---------------------------------------------------------------------------------------

		private string CleanTitle(string value) {
			// Some <Title>'s have characters that aren't valid in filenames. Replace
			// the bad chars with something more acceptable
			value = value.Replace(":", " --")
				.Replace("?", "$Q$")
				.Replace("|", "!")
				.Replace("\"", "'")
				.Replace("/", "$")
				.Replace("’", "'");
			foreach (char c in Path.GetInvalidFileNameChars()) {
				value = value.Replace(c, '.');
			}
			return value;
		}

//---------------------------------------------------------------------------------------

		private void DownloadMsConferences_FormClosing(object sender, FormClosingEventArgs e) {
			foreach (DownloadProgress dp in flowLayoutPanel1.Controls) {
				dp.wc.CancelAsync();
			}
		}
	}
}
