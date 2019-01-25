using System;
using System.Collections.Generic;
using System.ComponentModel;
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

using static CoreDownloadMsConferences.ScrapeChannel9Page;
using LRS.Utils;

// TODO: Make frame wider
// TODO: Put all files for a given session in their own folder
// TODO: Update documentation

// I've had a problem with my GetPiLectures program. DownloadStringTaskAsync
// hangs and never seems to return. A Microsoft Message Analyzer trace produced the
// message: "segment lost missing 3-way handshake". Searching on this gave, among others,
// https://support.microsoft.com/en-us/help/172983/explanation-of-the-three-way-handshake-via-tcp-ip
// So I wonder if that's why this program has a tendency to slowly lose active downloads.
// If so, I'm not at all sure how to detect this and how to automatically restart the
// failed downloads. Maybe implement some kind of heartbeat inside the UpdateProgress
// callback, and if the heartbeat stops, restart it by Posting a message to the host, or
// maybe signalling via a mutex. Whatever. But one thing I'm sure won't work is basing
// the restart on a traditional timeout. The timeout would be different for every file
// and depend on the size of the file, the speed of the Internet connection, perhaps the
// CPU speed, and maybe other things. Oh, and when the download is actually allowed to
// start (see the config parameter NumberOfSimultaneousDownloads).

// I ran across the following code that was able to use a timeout. I include it here
// just in case I need something like this in another program. It's from:
// https://stackoverflow.com/questions/27647942/cancelling-a-downloadstringtaskasync-and-restart-it
// The code is:
#if false
async void DoTasks()
{
	string output;

	int timeout = 5000;
	WebClient client = new WebClient() { Encoding = Encoding.UTF8 };
	Task<string> task = client.DownloadStringTaskAsync(url);
	if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
	{
		output = task.Result;
	}
	else
	{
		client.CancelAsync();
		DoTasks();
	}
}
#endif

// For a Powershell version of this program, see https://alexandrebrisebois.wordpress.com/2016/10/25/download-microsoft-ignite-2016-sessions-using-powershell/

// The HTML Agility Pack is an non-Microsoft open source package that can parse an HTML
// file and give us a Document Object Model (DOM) of it. We use this for web scraping.

// See http://www.codeproject.com/Articles/609053/AngleSharp
// See http://blogs.msdn.com/b/cdndevs/archive/2015/12/17/web-scraping-in-c.aspx
// See http://www.codeproject.com/Tips/804660/How-to-Parse-HTML-using-Csharp
// See http://www.alimozdemir.com/htmlagilitypack-csquery-and-anglesharp-comparison/

// For network logging, see https://msdn.microsoft.com/en-us/library/hyb3xww8%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396

namespace CoreDownloadMsConferences {
	public partial class CoreDownloadMsConferences : Form {

		// The following URL has a list of all conference sessions in RSS/XML format
		string RssUrl;

		// Where I want the videos and slides to go by default
		string TargetDir;

		// Configuration info
		ConfigData cfg;

		// Current Conference
		Conference CurConf;

		// For giving the user some progress info
		internal static int				nSessions;	// # of sessions

		internal static InterlockedInt	nFiles;     // # of files to download
		internal static InterlockedInt	nFilesDone;	// # of files left to download

		public static int nFilesSkipped;            // # of files that were skipped
		public static int nVideosNotFound;			// How many aren't up yet
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
		public static SynchronizationContext scGui;
		public static CoreDownloadMsConferences  Mainline;

		public FlowManager Flow;

//---------------------------------------------------------------------------------------

		public CoreDownloadMsConferences() {
			InitializeComponent();

			scGui = SynchronizationContext.Current;
			Mainline = this;

			cfg = ConfigData.LoadConfig();

			string MsgDefDirBad = SetupDefaultDownloadDirectory();
			if (MsgDefDirBad.Length > 0) {
				MessageBox.Show($"Default Directory invalid - {MsgDefDirBad}. Please update configuration file.",
					"Download Microsoft Conferences", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.Exit();
			}

			cmbConf.DataSource = cfg.Conferences;   // Fill combo box

			int NumSimDls = cfg.NumberOfSimultaneousDownloads;
			if (NumSimDls <= 0) { NumSimDls = Environment.ProcessorCount; }

			int ProcCountMult = cfg.ProcessorCountMultiplier;
			if (ProcCountMult == 0) { ProcCountMult = 2; }  // "2" is arbitrary

			NumSimulDownloads.Value = NumSimDls * ProcCountMult;

			var menu = new MenuStrip();
			var menu_CopyMsgs = new ToolStripMenuItem("Copy messages");
			menu_CopyMsgs.Click += new EventHandler((sender, e) => {
				DbgDumpMsgsToClipboard();
			});
			menu.Items.Add(menu_CopyMsgs);
			this.Controls.Add(menu);
			this.MainMenuStrip = menu;

			Flow = new FlowManager(flowLayoutPanel1);
		}

//---------------------------------------------------------------------------------------

		private string SetupDefaultDownloadDirectory() {
			cfg.DefaultTargetDir = cfg.DefaultTargetDir ?? @"C:\DownloadMsConferences\";
			cfg.DefaultTargetDir = cfg.DefaultTargetDir.Trim();
			try {
				Directory.CreateDirectory(cfg.DefaultTargetDir);
				return "";
			} catch (Exception ex) {
				return ex.Message;
			}
		}

//---------------------------------------------------------------------------------------

		private void DbgDumpMsgsToClipboard() {
			var sb = new StringBuilder();
			for (int i = lbMsgs.Items.Count - 1; i >= 0; i--) {
				sb.AppendLine((string)lbMsgs.Items[i]);
			}
			string text = sb.ToString();
			if (text != null) {
				try {
					Clipboard.SetText(text);
				} catch {
					// Ignore. Probable race condition
				}
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// This routine gets the RSS/XML file for the specified conference. Note that
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

		// We had a case in Build 2018 where, initially, they weren't replacing
		// an instance of "&" with "&amp;", where blew the attempt to read in
		// the RSS XML file. So here's a workaround. Fortunately, Microsoft
		// fixed it the next day, so we don't need this code any more.
#if false
			string xml;
			using (var wc = new WebClient()) {
				xml = wc.DownloadString(RssUrl);
				xml = FixupXmlAmpersands(xml, true);
				xml = xml.Replace("&", "&amp;");
				xml = FixupXmlAmpersands(xml, false);
			}
			var RssDoc = XDocument.Parse(xml);
#else
			var wreq        = WebRequest.CreateHttp(RssUrl);
			var RssResponse = await wreq.GetResponseAsync();
			var RssStream   = RssResponse.GetResponseStream();
			var RssDoc      = XDocument.Load(RssStream);
#endif
			return RssDoc;
		}

//---------------------------------------------------------------------------------------

		private string FixUpXmlAmpersands(string xml, bool bChange) {
			if (bChange) {
				xml = xml
					.Replace("&lt;",    "%%%%lt;")
					.Replace("&gt;",    "%%%%gt;")
					.Replace("&nbsp;",  "%%%%nbsp;")
					.Replace("&quot;",  "%%%%quot;")
					.Replace("&#",      "%%%%#")
					.Replace("&amp;",   "%%%%amp;")
				;
			} else {
				xml = xml
					.Replace("%%%%lt;",   "&lt;")
					.Replace("%%%%gt;",   "&gt;")
					.Replace("%%%%nbsp;", "&nbsp;")
					.Replace("%%%%quot;", "&quot;")
					.Replace("%%%%#",     "&#")
					.Replace("%%%%amp;",  "&amp;")
				;
			}
			return xml;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Go through the RSS/XML file and pick up just the info we need, which is the
		/// title of the session and the URL of its web page. We'll then scrape this
		/// looking for the video and/or slideshow file(s) we want
		/// </summary>
		/// <param name="RssDoc">The RSS file</param>
		private async void DownloadRssAsync(XDocument RssDoc) {
			// I've seen cases where more than one session (at least according to the
			// RSS feed) has the same name. Let's try to qualify it with the 
			// <iTunes:summary> Hmmm. It seems that the iTunes summary is the same as
			// Description. But I'll leave this in as an example of how to get at
			// an element with a namespace ("iTunes:") qualifier. Note that this
			// seems to depend on operator+ for an XNamespace.
			XNamespace iTunes = "http://www.itunes.com/dtds/podcast-1.0.dtd";
			var qrySessionItems = from item in RssDoc.Element("rss").Element("channel").Elements("item")
								  orderby item.Element("title")?.Value
								  select new RssItem {
									  Title       = CleanTitle(item.Element("title")?.Value?.Trim()),
									  Url         = item.Element("link")?.Value,
									  SessionID   = GetSessionID(item.Element("link")?.Value),
									  Description = item.Element("description")?.Value?.Trim()
								  };
			// Note: We only need the above four elements from each <item>

			var PageInfos = new Dictionary<string, ScrapedSessionInfo>();
			// GetFileSizes(qrySessionItems, PageInfos);

			nFiles			= new InterlockedInt(0, "nFiles");
			nFilesDone		= new InterlockedInt(0, "nFilesDone");
			nSessions		= 0;
			nFilesSkipped	= 0;
			nVideosNotFound = 0;
			// var AllSessions = qrySessionItems.ToList();
			foreach (var SessInfo in qrySessionItems) {
#if false	// For testing on just one session
				// if (item.Title != "Native iOS, Android, & Windows Apps from C# and XAML with Xamarin.Forms") {
				if (! SessInfo.Url.Contains("BRK3408")) {
					continue;
				}
#endif

				bool bSkipSession = VetSessionAgainstFilters(SessInfo);
				if (bSkipSession) {
					continue;
				}
				++nSessions;
				// string SessionID = GetSessionID(item.Url);
				if (!EssentiallyTheSame(SessInfo.Title, SessInfo.SessionID)) {
					SessInfo.Title += " - " + SessInfo.SessionID;
				}

				// I've seen cases where, on older conferences, the sessions
				// in the .xml file can no longer be found. So let's recover
				// from a problem in scraping a non-existent page.
				bool bGoAhead = true;
				ScrapedSessionInfo? PageInfo = null;
				try {
					var sc9p = new ScrapeChannel9Page();
					PageInfo = await sc9p.ScrapeAsync(SessInfo);
				} catch {
					bGoAhead = false;
					msg($"*** Problem scraping page <b>{SessInfo.Url}</b>");
				}

				if (bGoAhead) {
					ProcessSession(SessInfo, PageInfo);         // OK, look for our files
				}
			}
			bDoneEnumeratingFiles = true;
			string s = (nFiles != 1) ? "s" : "";	// TODO: Define ss(nFiles)
			msg($"Finished getting the list of {nSessions} sessions, {nFiles} file{s}, {nFilesSkipped} skipped, {nVideosNotFound} videos not found");
		}

//---------------------------------------------------------------------------------------

		private async void GetFileSizes(IEnumerable<RssItem> qrySessionItems, Dictionary<string, ScrapedSessionInfo> PageInfos) {
			// var TaskList = new List<System.Threading.Tasks.Task<ScrapedSessionInfo>>();
			var TaskList = new List<System.Runtime.CompilerServices.ConfiguredTaskAwaitable<ScrapedSessionInfo>>();
			foreach (var item in qrySessionItems) {
				var sc9p = new ScrapeChannel9Page();
				var t    = await sc9p.ScrapeAsync(item).ConfigureAwait(false);
				// TaskList.Add(t);
				// var y = TaskList[0];
				// Application.DoEvents();
				// y.Wait();
			}
			//     PageInfos[item.Link] = PageInfo;
			// var TaskArray = TaskList.ToArray();
			// var x = Task.WaitAny(TaskArray);
			// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter
			//  Task.WaitAll(TaskList.ToArray());

#if false
			foreach (var item in TaskList) {
				var xxx = await item.GetAwaiter();
			}
#endif
	   }

//---------------------------------------------------------------------------------------

		private bool VetSessionAgainstFilters(RssItem item) {
			var TitleWords = SplitWords(item.Title);
			int n = Index(TitleWords, cfg.GlobalFilters.Excludes);
			if (n >= 0) {
				// dbg($"***Vetting: Excluding <b>{item.Title}</b> - bad text: {item.Title}");	// TODO: Wrong message, since n == 0 or -1
				return true;
			}

			if (cfg.GlobalFilters.Includes.Count() == 0) {
				return false;           // Accept everything
			}
			n = Index(TitleWords, cfg.GlobalFilters.Includes);
			if (n >= 0 ) {
				// dbg($"***Vetting: Including {item.Title} - good text: {TitleWords[n]}");
				return false;

			}

			dbg($"***Vetting: Bypassing {item.Title} -- No vetting text found");
			return true;
		}

		//---------------------------------------------------------------------------------------

		private static List<string> SplitWords(string? s) {
			if (s is null) { s = ""; }
			return s.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
		}

//---------------------------------------------------------------------------------------

			/// <summary>
			/// 
			/// </summary>
			/// <param name="Target">A List&lt;string&gt; of the words in the Title of
			/// a session name. For example, "C#", "in", "Visual", "Studio", "Code"
			/// </param>
			/// <param name="Source">A List&lt;string&gt; of phrases representing
			/// words in a session title that we might (or might not) beinterested in.
			/// For exampke, "Azure", "SharePoint", "Microsoft Graph".
			/// </param>
			/// <returns>Either an index into the session title where any one of the
			/// phrases occurs, or -1 to show that none of the phrases were found
			/// </returns>
			// TODO: Should return bool?
		private static int Index(List<string> TitleWords, List<string> SourcePhrases) {
			// See if any entry in the Source is found in the Target. If so, return its
			// index, else -1.
			// Note: This routine won't get heavy usage, so it's not worth it trying a
			//       sophisticated algorithm (e.g. Boyer-Moore). I'll just brute force it
			// Note: Surprisingly this is an O(n^3) algorithm! We have to
			//  *   Go through each word in the Target (List of words in the Title) that
			//      can act as the start of the substring
			//  *   Match each Source (List of words) against the current Target position
			//  *   Match each word in the currnt Source against the Target
			// So it's an O(n^3) algorithm. But again this routine is called infrequently
			// enough that it's not going to noticeably slow us down, so yeah, I'm
			// willing to brute force it.
			// Note: We *could* speed things up by about a factor of 2. We call this
			//       routine twice, once for the Exclude list and once for the Include
			//       list. So we do essentially the same processing twice. But yet again
			//       the overhead of this routine is low and half low, at the expense
			//       of complicating the code, ain't worth it.
			// Oh yeah, forgot to mention. I could just do a .Contains on each entry
			// in Source, but suppose I'm looking for ".Net Framework" and either
			// the session title or my Config file has a double space in the middle:
			// ".Net  Framework". Then I wouldn't get the result I expect. So I'll
			// go through the extra work of searching for a list of words, rather
			// than just a raw string.
			// TODO: Can we use this later in CategorizeSessionTopics?
			// TODO: Make sure that Cloud\Azure in the config file is handled correctly
			foreach (var phrase in SourcePhrases) {
				var SrcWords = SplitWords(CleanTitle(phrase));
				for (int ixTitle = 0; ixTitle <= TitleWords.Count - SrcWords.Count; ixTitle++) {
					bool bGotMatch = PhraseMatchAt(SrcWords, TitleWords, ixTitle);
					if (bGotMatch) return 0;
				}
			}
			return -1;
		}

//---------------------------------------------------------------------------------------

		private static bool PhraseMatchAt(List<string> srcWords, List<string> titleWords, int ixTitle) {
			int i;
			for (i = 0; i < srcWords.Count; i++) {
				string TgtElem = titleWords[ixTitle + i];
				string SrcElem = srcWords[i];
				if (string.Compare(TgtElem, SrcElem, true) != 0) {
					return false;
				}
			}
			return true;
		}

//---------------------------------------------------------------------------------------

		private bool EssentiallyTheSame(string title, string sessionID) {
			// In Build2016, there are two sessions with the title "Keynote Presentation"
			// and this confuses this program. So I'll try to generate a unique name by
			// concatenating the session's ID to the title.
			// I was hoping that the last node of the SessionID was a simple ID like
			// "C123". But I see things like a Title of Python and node.js on Visual
			// Studio and a link of Python-and-nodejs-on-Visual-Studio. Which are
			// essentially the same. So if they're *not* the same, then the caller of
			// this routine will glue the Title and the SessionID together to (hopefully)
			// create a unique Title for the session (
			StringBuilder sb1 = new StringBuilder();
			StringBuilder sb2 = new StringBuilder();
			for (int i = 0; i < title.Length; i++) {
				char c = title[i];
				if (char.IsLetterOrDigit(c)) {
					sb1.Append(c);
				}
			}
			for (int i = 0; i < sessionID.Length; i++) {
				char c = sessionID[i];
				if (char.IsLetterOrDigit(c)) {
					sb2.Append(c);
				}
			}
			if (string.Equals(sb1.ToString(), sb2.ToString(), StringComparison.InvariantCultureIgnoreCase)) {
				return true;
			}
			return false;
		}

//---------------------------------------------------------------------------------------

		private string GetSessionID(string link) {
			int ix = link.LastIndexOf('/');
			return link.Substring(ix + 1);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Scrape the HTML for the current session page. We'll eventually wind up with
		/// a link to the highest-res video file (if we've been asked to download videos,
		/// and/or a link to the PowerPoint slide show for this session (if it exists).
		/// We'll then download either/both.
		/// </summary>
		/// <param name="item">An RSS item element</param>
		private void ProcessSession(RssItem item, ScrapedSessionInfo? PageInfo) {
			// if (PageInfo.Captions.Url != null) Debugger.Break();	// TODO: Debug
			if (PageInfo is null) { return; }
			CountFilesToDownload(item, PageInfo);

			// Compare each topic in the config file with the title of the session.
			// If we get an exact match (e.g. "Windows 10"), then call that the
			// primary topic and create a directory for it. Subsequently all
			// "Windows 10" sessions will go there. If no match is found, then
			// we'll use the default directory specified in the config file
			string PrimaryTopicPath = CurConf.CategorizeSessionTopics(item.Title, out List<String> SecondaryTopics);
			// if (PrimaryTopicPath == Conference.UnCatDirName) {
			// 	PrimaryTopicPath = TargetDir;
			// }
			PrimaryTopicPath = Path.Combine(TargetDir, PrimaryTopicPath, item.Title);
			Directory.CreateDirectory(PrimaryTopicPath);
			
			CreateShortcutToUrl(PrimaryTopicPath, item);

			// OK, download the Video and/or the slides for it
			var SkeletonOnly = chkSkeletonOnly.Checked;
			if (chkSlides.Checked) {
				DownloadFile(PageInfo.Slides, PrimaryTopicPath, item, SkeletonOnly);
				CreateSecondaryLinks(PrimaryTopicPath, SecondaryTopics, item, PageInfo.Slides);
			}
			if (chkVideos.Checked) {
				// TODO: Restore next line to download captions (.vtt)
				DownloadFile(PageInfo.Captions, PrimaryTopicPath, item, SkeletonOnly);
				DownloadFile(PageInfo.Video,    PrimaryTopicPath, item, SkeletonOnly);
				CreateSecondaryLinks(PrimaryTopicPath, SecondaryTopics, item, PageInfo.Video);
			}

			CreateLinksForTagsLevelSessionTypes(TargetDir, PageInfo, item);
		}

//---------------------------------------------------------------------------------------

		private static void CreateLinksForTagsLevelSessionTypes(string TargetDir, ScrapedSessionInfo PageInfo, RssItem Item) {
			// Let's pretend that a given session has tags: .Net, Visual Studio and
			// Diagnostics. So we want to create a top-level directory (but under
			// TargetDir (e.g. D:\MsConferences\Build 2015)) called $Tags. Under it will
			// be subdirs called .Net, Visual Studio and Diagnostics. And each of these
			// will have zero or more sets of links -- to the session itself, to the
			// .pptx file (if present) and to the .mp4 file (if present).
			foreach (var MetaName in PageInfo.TagsEtAl) {
				string InThisDir = Path.Combine(TargetDir, MetaName.Key);
				Directory.CreateDirectory(InThisDir);

				MakeMetaShortcuts(MetaName.Key, PageInfo, InThisDir, Item);
			}
		}

//---------------------------------------------------------------------------------------

		private static void MakeMetaShortcuts(string MetaName, ScrapedSessionInfo PageInfo, string InThisDir, RssItem Item) {
			foreach (var item in PageInfo.TagsEtAl[MetaName]) {
				string TargetDir = Path.Combine(InThisDir, item);
				Directory.CreateDirectory(TargetDir);
				CreateShortcutToUrl(TargetDir, Item);
			}
		}

//---------------------------------------------------------------------------------------

		private void CountFilesToDownload(RssItem item, ScrapedSessionInfo PageInfo) {
			// Count (carefully) the number of files we have to download.
			// Assume that all slides are .pptx (as opposed to, say, .ppt) and that
			// videos are .mp4. Also support .vtt
			// if (chkSlides.Checked && (null != PageInfo.Slides.Url?.EndsWith(".pptx"))) {
			if (chkSlides.Checked && (true == PageInfo.Slides.Url?.EndsWith(".pptx"))) {
				++nFiles;
				// nFiles.Increment();
			}
			// if (chkVideos.Checked && (null != PageInfo.Captions.Url?.EndsWith(".vtt"))) {
			if (chkVideos.Checked) {
				string url = PageInfo.Captions.Url;
				bool bCap = (url != null) &&
					(url.EndsWith(".vtt") || url.Contains("/captions?"));
				// bCap = false;					// TODO: Ignore .vtt for testing
				if (bCap) {
					++nFiles;
				}
			}
			// if (chkVideos.Checked && (null != PageInfo.Video.Url?.EndsWith(".mp4"))) {
			if (chkVideos.Checked && (true == PageInfo.Video.Url?.EndsWith(".mp4"))) {
				++nFiles;
			}
		}

//---------------------------------------------------------------------------------------

		private void CreateSecondaryLinks(string PrimaryTopicPath, List<string> SecondaryTopics, RssItem item, SessionDownloadInfo sdl) {
			if (sdl.Url == null) {
				return;
			}
			foreach (string topic in SecondaryTopics) {
				string InThisDir = Path.Combine(TargetDir, topic);
				string ToThisDir = Path.Combine(TargetDir, PrimaryTopicPath);
				Directory.CreateDirectory(InThisDir);
				CreateShortcutToFile(InThisDir, ToThisDir, item.Title, GetUrlSuffix(sdl.Url));
				CreateShortcutToUrl(InThisDir, item);
			}
		}

//---------------------------------------------------------------------------------------

		private static string TrimLongFilename(string FullFilename) {
			const int MAXPATHLENGTH = 260 - 4;  // Use GetVolumeInformation if you want
			// Bit of a kludge above. In some cases we want to append ".url" to a
			// filename. So we've cut down the nominal 260 to 256.
			int len = FullFilename.Length;
			if (len < MAXPATHLENGTH) {
				return FullFilename;
			}
			string fn                  = Path.GetFileNameWithoutExtension(FullFilename);
			int SurroundingCharsLength = len - fn.Length;
			int MaxFnLength            = MAXPATHLENGTH - SurroundingCharsLength;
			fn                         = fn.Substring(0, MaxFnLength);
			// Put things back together
			// Microsoft bug: Path.GetDirectoryName fails on a too-long pathname, even
			// if all it wants to return is the Directory part. We'll have to do it
			// ourselves
			// string Dirname = Path.GetDirectoryName(FullFilename);
			int ix            = FullFilename.LastIndexOf('\\');
			string Dirname    = FullFilename.Substring(0, ix);
			string Ext        = Path.GetExtension(FullFilename);
			fn                = Path.Combine(Dirname, fn) + "." + Ext;
			return fn;
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
			PageShortCut = PageShortCut.Replace('\\', '/').Replace(" ", "%20");
			if (Title.EndsWith(Suffix.Substring(1))) {
				Suffix = "";
			}
			string Filename = Path.Combine(InThisDirectory, TrimLongFilename($"{Title}{Suffix}") + ".url");
			if (Filename.EndsWith("APP-106.APP-106"))  {
				System.Diagnostics.Debugger.Break();
			}
			await WritePageShortcut(PageShortCut, Filename);
		}

//---------------------------------------------------------------------------------------

		private static async void CreateShortcutToUrl(string InThisDirectory, RssItem item) {
			// Note: This routine is almost identical to CreateShortcutToFile.
			string PageShortCut = $@"[InternetShortCut]
URL={item.Url}
";
			PageShortCut = HttpUtility.HtmlEncode(PageShortCut).Replace(" ", "%20");
			string Suffix = GetUrlSuffix(item.Url);
			if (item.Title.EndsWith(Suffix.Substring(1))) {
				Suffix = "";
			}
			string Filename = TrimLongFilename((Path.Combine(InThisDirectory,$"{item.Title}{Suffix}")) + ".url");
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

		private static string GetUrlSuffix(string Url) {
			int ix = Url.IndexOf('?');		// TODO: Captions?
			if (ix >= 0) {
				// Debugger.Break();
				Url = Url.Substring(0, ix);
			}
			string suf = "." + Url.Split('/').Last().Split('.').Last();
#if DEBUG
			if (! ((suf == ".mp4") || (suf == ".pptx") || (suf == ".vtt") || (suf.StartsWith(".captions?")))) {
				Application.DoEvents();
				// Debugger.Break();				// TODO: Shouldn't happen
			}
#endif
			return suf;
		} 

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Start the asynchronous download the specified file.
		/// </summary>
		/// <param name="sdi">Where on the web the file is and how big it is</param>
		/// <param name="TargetDir">Where on disk it will land</param>
		/// <param name="item">Has the session title, description, link</param>
		/// </param>
		private void DownloadFile(SessionDownloadInfo sdi, string TargetDir, RssItem item, bool SkeletonOnly) {
			if (sdi.Url == null) { return; }	// e.g. no Slides found

			try {
				// The suffix (.mp4 or .pptx) can be derived from the Url
				// For example, get ".mp4" from "http://video.ch9.ms/sessions/build/2015/2-612-LG.mp4"
				string Suffix = GetUrlSuffix(sdi.Url);
				if (Suffix.StartsWith(".captions")) {  // Check for Captions
					Suffix = ".vtt";
				}
				string filename = item.Title + Suffix;
				var FullName    = TrimLongFilename(Path.Combine(TargetDir, filename));
				if (File.Exists(FullName) && new FileInfo(FullName).Length > 0) {
					// This supports restartability of the app. Don't download things again.
					msg("        Skipping      " + filename);
					Interlocked.Increment(ref nFilesSkipped);
					--nFiles;
					return;
				}
				if (filename.EndsWith(".DownloadDocument")) {	// TODO: Debug
					Debugger.Break();
				}
				msg($"Starting file download for: <b>{filename}</b>");

				var twc = new TitledWebClient(filename, FullName, item.Description, item.Url, sdi.Url, null);
				// Our event handlers
				twc.DownloadProgressChanged += Wc_DownloadFileProgressChanged;
				twc.DownloadFileCompleted   += Wc_DownloadFileCompleted;

				twc.Progress = new DownloadFileProgress(twc, item.Title, msg, DoProgress);
				// TODO: Move this into parm list of TitledWebClient ctor

				Flow.Add(twc.Progress);

				Application.DoEvents();     // Make sure UI updates

				ShowFilesToGo();            // Status update

				// In addition to *.mp4 and *.pptx, write out <Title>.url that, when
				// clicked, will take you to the web page for this session

				// OK, here's where the download is actually done. Notice it's asynchronous,
				// so it will return immediately if we're not at the maximum number of
				// simulataneous downloads allowed. Then it will block until one of the
				// previous downloads has finished.

				// Note that this method returns void, so we can't "await" it (which needs a
				// method that returns a Task<>. So this is "fire and forget". But that's OK.
				// Our two events will fire with progress info and also when the download is
				// complete.
				if (! SkeletonOnly) {
					// TODO: TODO: Instead of calling DownloadFileAsync, perhaps try the following:
					// var stream = wc.OpenRead(Address);
					// long len = Convert.ToInt64(wc.ResponseHeaders["Content-Length"]);
					// Note: Set timeout only after first read has truly executed???
					// stream.ReadTimeout = 60 * 1000; // And check for timeout exception
					// var buf = new byte[4096];
					// Loop on the next, w/await, until all bytes read
						// Task<int> x = stream.ReadAsync(buf, 0, buf.Length);
						// var NumBytesRead = x.Result;

					twc.DownloadFileAsync(new Uri(sdi.Url), FullName);
				}
			} catch (Exception ex) {
				MessageBox.Show($"Exception {ex.Message}");
				Debugger.Break();
			}
		}

//---------------------------------------------------------------------------------------

		internal bool DoProgress(bool bIsCalledFromUpdate, object Tag, DownloadProgressChangedEventArgs EventData) {
			var twc = (TitledWebClient)Tag;
			var prog = twc.Progress;

			if (bIsCalledFromUpdate) {
				if (prog.BytesToDate == -1) {   // Start of download
					// TODO: Assumes prog has already been added to FlowLayoutPanel1.
					//		 Instead, add it here
					// Note: This next line looked like a good idea, but especially with
					//		 small files (especially .vtt), things are too chaotic
					// flowLayoutPanel1.Controls.SetChildIndex(prog, 0);
				} else {
					// TODO: Be careful if EventData.BytesReceived is too large
					if (EventData.TotalBytesToReceive > int.MaxValue) {
						prog.ProgressBar1.Value = (int)(EventData.BytesReceived >> 10);
					} else {
						prog.ProgressBar1.Value = (int)EventData.BytesReceived;
					}
				}
			} else {            // Must have been called from Wc_DownloadFileCompleted
				prog.IsDownloadDone = true;
				int nDone = nFilesDone;
				if (0 == nDone % cfg.MoveToTheRearOfTheBusPlease) {
					Flow.MoveCompletedDownloadsToEndOfList();
				}
				SetProgressBar();

				if (bDoneEnumeratingFiles && nDone == nFiles) {
					msg("Done!");
					tmr.Stop();
					btn_Go.Enabled = true;
					Flow.SortFlowPanel();
				}
			}

			return true;
		}

#if false       // TODO: Delete xxxSortFlowPanel
		//---------------------------------------------------------------------------------------

		private void xxxSortFlowPanel() {
			try {
				var ctls = flowLayoutPanel1.Controls;
				var dict = new Dictionary<string, DownloadFileProgress>();
				for (int i = 0; i < ctls.Count; i++) {
					var ctl = ctls[i] as DownloadFileProgress;
					dict[ctl.twc.Title] = ctl;
				}
				flowLayoutPanel1.Controls.Clear();
				flowLayoutPanel1.SuspendLayout();
				var keys = dict.Keys.OrderBy(key => key);
				foreach (var item in keys) {
					flowLayoutPanel1.Controls.Add(dict[item]);
				}
				flowLayoutPanel1.ResumeLayout();
			} catch (Exception ex) {
				Debugger.Break();		// TODO:
			}
		}
#endif

//---------------------------------------------------------------------------------------

		internal void SetProgressBar() {
			// TODO: Implement this
#if false
			lblProgress.Text = $"Downloaded {nLecturesDownloaded} of {nLecturesToDownload}";
			progressBar1.Maximum = nLecturesToDownload;
			progressBar1.Value = nLecturesDownloaded;
#endif
			Application.DoEvents();
		}

//---------------------------------------------------------------------------------------

		private void Wc_DownloadFileProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
			var wc = (sender as TitledWebClient)!;
			if (wc.Progress.BytesToDate == -1) {       // -1 means Start of download
				// The next line looked like a good idea at the time, but no, it
				// makes things too chaotic
				// flowLayoutPanel1.Controls.SetChildIndex(wc.Progress, 0);
			}
			wc.Progress.UpdateProgress(e);
		}

//---------------------------------------------------------------------------------------

		private void Wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e) {
			var wc = (sender as TitledWebClient)!;
			// wc.TransferCompletedOk = true;
			if (e.Error != null) {
				msg($"******** Error on {wc.FullFilename} -- ({e.Error}) in DownloadFileCompleted)");
				PaintItBlack_er_Cyan(wc);
				try {
					File.Delete(wc.FullFilename);
				} catch (Exception ex) {
					msg($"******** Error deleting {wc.FullFilename} - {ex.Message}");
				}
				// TODO: Retry...
				wc.Progress.ProgressBar1.ForeColor = Color.Cyan;
				wc.Progress.ProgressBar1.Value     = wc.Progress.ProgressBar1.Maximum;
			} else {
				// wc.TransferCompletedOk = true;
				// Set Last Write Time on file based on Response Header
				var lm = wc.ResponseHeaders["Last-Modified"];
				if (lm != null) {
					var LastUsed = DateTime.Parse(lm);
					File.SetLastWriteTime(wc.FullFilename, LastUsed);
				}
			}
			++nFilesDone;
			ShowFilesToGo();

		}

//---------------------------------------------------------------------------------------
		
		private static void PaintItBlack_er_Cyan(TitledWebClient wc) {
			// With apologies to Mick and Keith
			wc.Progress.lblTotalSize.BackColor        = Color.Cyan;
			wc.Progress.lblAmountDownloaded.BackColor = Color.Cyan;
			wc.Progress.lblDownloadSpeed.BackColor    = Color.Cyan;
			wc.Progress.lblPercentDone.BackColor      = Color.Cyan;
			wc.Progress.LinkLabel1.BackColor          = Color.Cyan;
		}

//---------------------------------------------------------------------------------------

		internal static string CleanTitle(string value) {
			if (value == null) {
				return string.Empty;
			}
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
			foreach (DownloadFileProgress dp in flowLayoutPanel1.Controls) {
				// TODO: Should this be await dp.wc.CancelAsync()?
				dp.twc.CancelAsync();
			}
		}
	}
}
