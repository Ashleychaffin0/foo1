// Copyright (c) 2010 by Larry Smith

// TODO:

// *	Can we use ETW instead of FileSystemWatcher?

// *    Don't do file copy; just create Tuple<download name, name we want>,
//      then do a mass rename at the end.

// *	Re-download of file triggers Changed event, not Create. Maybe do something.

// *	Test accessing things immediately after login

// *	Check all FSM states, and see if we need all of them.

// *	Automate download of all articles, if we can.

// *	Clean up TODO:'s

// I've never really used CSS before, but I think I should/will. Here's 
// a starter from http://www.w3schools.com/css/tryit.asp?filename=trycss_default
// via http://www.w3schools.com/css/
#if false
<style type="text/css">
body
{
background-color:#d0e4fe;
}
h1
{
color:orange;
text-align:center;
}
p
{
font-family:"Times New Roman";
font-size:30px;
}
</style>
</head>

<body>

<h1>CSS example!</h1>
<p>This is a paragraph.</p>


========================
Also... (Note: $ used instead of # because C# compiler gets
antsy if a # appears as the first non-whitespace element on a line)

<style type="text/css">
$para1
{
text-align:center;
color:red;
} 
$bgcyan {
background-color:lightcyan;
}
</style>
</head>

<body>
<p id="para1">Hello World!</p>
<p id="bgcyan">This paragraph is not affected by the style.


#endif

// #define	CREATE_PARMS_FILE			// Normally commented out
#define	WRITELOG					// Normally commented out
// #define USE_FSW

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using mshtml;

namespace GetSciAm {
	public partial class GetSciAm : Form {

		// Where our trace output goes. Note that we *could* put this into the
		// parms file, and even do fancy things such as check to see if it has
		// a specific path (and if so, not put it into My Documents). But
		// why bother?
		string TraceFileName = "SciAmTrace.html";

#if USE_FSW
		enum FsmState {
			Login,
			LoggedIn,
			CurrentIssue,
			SelectIssueWithinYear,
			SpecificIssue,
			Idle
		}

		// Corresponds to FsmState. Keep in synch.
		Color[] FsmColors = {
				Color.Red, 				// Login
				Color.Green, 			// LoggedIn
				Color.Blue, 			// CurrentIssue
				Color.Chocolate, 		// SelectIssueWithinYear
				Color.Magenta,			// Specific Issue
				Color.Black				// Idle
			};

		FsmState _State;
		FsmState State  { get { return _State; }
						  set { 			
							  string msg = string.Format("Switching state from {0} to {1}", _State, value);
							  Log(msg);
							  statusStrip1.Text = msg;
							  _State = value;
							}
						}
#endif

		bool IsLoggedIn = false;

		// Holds links to each of 12 issues for the currently specified year
		List<string> MonthIssueURL = new List<string>();

		// Holds description (will be part of filename) of document, plus its href.

		IssueDocInfo CurDocInfo = null;

		// Our FileSystemWatcher callback runs on a different thread than the main
		// GUI. This means we can't (easily) access the Year and Month combo boxes.
		// So whenever those values are changed, we'll copy their values here.
		string 	CurYear, CurMonth;
		int 	CurMonthNumber;

		FileSystemWatcher fsw = new FileSystemWatcher();

		GetSciAmParms parms = null;

		// Avoid file being copied twice
		bool bIsFileCopied = false;

		// Used for Get All Articles
		bool bAllArticlesProcessed = false;

		// Map SciAm's filenames to ours
		Dictionary<string, string> MapFilenames = new Dictionary<string, string>();

		// For highlighting non-UI thread messages
		int GuiThreadId;

		// TODO: Test data for File.Move
		string RealMaxDir = @"D:\$$LRS-SciAm\SciAm - 2008\2008-01 - January";
		string RealMaxFilename = @"SciAm - 2008-01-Jan - 14 - News Scan Briefs; January 2008; by Charles Q. Choi, Nikhil Swaminathan, David Biello, JR Minkel; 2 Page(s) Bt-Beating Bugs; Weight Loss on Shaky Ground; Bone Sweet Bone; No Mercy.pdf";


//---------------------------------------------------------------------------------------

		public GetSciAm() {
			InitializeComponent();

			GuiThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;

			// Some users (e.g. non-Power Users) may not be able to write to the
			// GetSciAm installation directory. Put this file into My Documents.
			string MyDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			TraceFileName = Path.Combine(MyDocs, TraceFileName);
			File.Delete(TraceFileName);
			Trace.Listeners.Add(new TextWriterTraceListener(TraceFileName));
			Trace.AutoFlush = true;
			SetupHtmlIntro();

			SetupParms();
			
			SetupFileSystemWatcher();

			State = FsmState.Login;

#if false    // TODO: Test code for File.Move
			string MaxDir = "";
			string MaxFilename = "";
			int MaxLen = 0;
			foreach (var dirYears in Directory.GetDirectories(parms.TargetDirectory)) {
				foreach (var dir in Directory.GetDirectories(dirYears)) {
					foreach (var filename in Directory.GetFiles(dir)) {
						int len = filename.Length;
						if (len > MaxLen) {
							MaxDir = dir;
							MaxFilename = filename;
							MaxLen = len;
						}
					}
				 }
		   }
			Console.WriteLine("MaxLen = {0}, file = {1}",
				MaxLen, Path.Combine(MaxDir, MaxFilename));

			// TODO: End test code
#endif
		}

//---------------------------------------------------------------------------------------

		private static void SetupHtmlIntro() {
			Trace.WriteLine("<HTML>\r\n<HEAD>\r\n");
			Trace.WriteLine("<style type=\"text/css\">\r\n");
			AddStyles();
			Trace.WriteLine("</style>");
			Trace.WriteLine("</HEAD>\r\n<BODY>");
	   }

//---------------------------------------------------------------------------------------

		private static void AddStyles() {
		   var styles = new KeyValuePair<string, string>[] {
			   // TODO: Dependency in WriteLog()
				new KeyValuePair<string, string>("div", "background-color:cyan;"),
				new KeyValuePair<string, string>("s2", "color:\"red\"; font-size:20px;")
			};
		   foreach (var style in styles) {
				string txt = string.Format("{0} {{\r\n\t{1}\r\n}}", style.Key, style.Value);
				Trace.WriteLine(txt);
			}
		}

//---------------------------------------------------------------------------------------

		private void SetupParms() {
#if CREATE_PARMS_FILE
			parms = new GetSciAmParms();
			StreamWriter sw = new StreamWriter("GetSciAmParms.xml");
			XmlSerializer xs = new XmlSerializer(typeof(GetSciAmParms));
			xs.Serialize(sw, parms);
			sw.Close();
			Application.Exit();
#else
			StreamReader sr = new StreamReader("GetSciAmParms.xml");
			XmlSerializer xs = new XmlSerializer(typeof(GetSciAmParms));
			object o = xs.Deserialize(sr);
			parms = (GetSciAmParms)o;
#endif
		}

//---------------------------------------------------------------------------------------

		private void SetupFileSystemWatcher() {
			fsw.Filter = "*.pdf";
			fsw.Path = parms.SourceDirectory;
			// fsw.IncludeSubdirectories = true;
			fsw.NotifyFilter = NotifyFilters.FileName
				| NotifyFilters.LastWrite
#if false
				| NotifyFilters.Attributes
				| NotifyFilters.CreationTime
				| NotifyFilters.DirectoryName
				| NotifyFilters.LastAccess
				| NotifyFilters.Security
				| NotifyFilters.Size
#endif
;
			// fsw.Changed += new FileSystemEventHandler(fsw_Changed);
			fsw.Created += new FileSystemEventHandler(fsw_Created);
			// fsw.Deleted += new FileSystemEventHandler(fsw_Deleted);
			// fsw.Renamed += new RenamedEventHandler(fsw_Renamed);

			fsw.EnableRaisingEvents = true;
		}

//---------------------------------------------------------------------------------------

		delegate void delGetNextArticle();

//---------------------------------------------------------------------------------------

		void fsw_Created(object sender, FileSystemEventArgs e) {
			string msg;
			if (bIsFileCopied) {
				Log(Color.DarkBlue, "File copy already done -- skipped");
				return;
			}
#if false
			msg = string.Format("<br/>******* fsw_Created -- ChangeType={0}, Name={1}, FullPath={2}", e.ChangeType, e.Name, e.FullPath);
			Log(Color.DarkGreen, msg);
#endif
			int MaxTries = parms.MaxFileCopyTries;
			// TODO: Setup code. Refactor and place, once, in caller
			string TargetFilename = SetupFilenameForCopy();
			MapFilenames[e.Name] = TargetFilename;
			Log("MapFileNames[" + e.Name + "] = " + TargetFilename);

			bool QuitSwitch = false;
			while (--MaxTries > 0 && !QuitSwitch) {
				try {
					Application.DoEvents();
#if false       // TODO: LRS
					QuitSwitch = DoFileCopy(e.FullPath, TargetFilename, MaxTries);
#else
					QuitSwitch = true;
#endif
					if (QuitSwitch) {
#if true
						btnGetNextArticle.BeginInvoke(new delGetNextArticle(GetNextArticle));
#else
						// TODO: Get rid of delegate delGetNextArticle above
						btnGetNextArticle.BeginInvoke(delegate() {
							GetNextArticle();
						});
#endif
					}
				} catch (Exception ex) {
					// Ignore and retry, after waiting a bit
					msg = string.Format("fsw_Created -- Exception: {0}", ex.Message);
					Log(Color.DarkGreen, msg);
					System.Threading.Thread.Sleep(1000);
				}
			}
		}

//---------------------------------------------------------------------------------------

#if false
The Windows API has many functions that also have Unicode versions to permit an 
extended-length path for a maximum total path length of 32,767 characters. This type of 
path is composed of components separated by backslashes, each up to the value returned 
in the lpMaximumComponentLength parameter of the GetVolumeInformation function (this 
value is commonly 255 characters). To specify an extended-length path, use the 
"\\?\" prefix. For example, "\\?\D:\<very long path>". (The characters < > are used 
here for visual clarity and cannot be part of a valid path string.)

From: http://msdn.microsoft.com/en-us/library/aa365247(VS.85).aspx#maxpath

Note: File.Copy doesn't seem to support \\?\ as a prefix.
#endif

		private bool DoFileCopy(string SourcePath, string TargetFilename, int MaxTries) {
			string msg = string.Format("File.Copy From={0}, To=\"{1}\", MaxTries={2}, To Length={3}", 
				SourcePath, TargetFilename, MaxTries, TargetFilename.Length);
			Log(Color.DarkGreen, msg);
			// Log(Color.DarkRed, "*** File.Copy commented out ***");
			File.Copy(SourcePath, TargetFilename, true);
			bIsFileCopied = true;
			var fi = new FileInfo(TargetFilename);
			Log(Color.DarkGreen, "DoFileCopy succeeded, filesize = " + fi.Length);
			// File.Delete(SourcePath);		// Does this work? Nope!
			return true;			// QuitSwitch
		}

//---------------------------------------------------------------------------------------

		private string SetupFilenameForCopy() {
			int Year = int.Parse(CurYear);		// 1993 onwards
			int Month = CurMonthNumber + 1;		// 1..12
			string MonthName = CurMonth.Substring(0, 3);	// Jan, Feb, etc
			int SeqNo = CurDocInfo.SeqNo;
			string Prefix = Path.Combine(parms.TargetDirectory, "SciAm - " + Year);
			Prefix = Path.Combine(Prefix, CurYear + "-" + Month.ToString("0#") + " - " + CurMonth);
			Directory.CreateDirectory(Prefix);
			Prefix = Path.Combine(Prefix, parms.FilenamePrefix
				.Replace("%YM", Year.ToString() + "-" + Month.ToString("0#"))
				.Replace("%M", MonthName)
				.Replace("%n", SeqNo.ToString("0#")));
			string TargetFilename = Prefix + CurDocInfo.Description;
			int ExtensionLength = 4;				// ".pdf"

			const int MaxFilenameLength = 255;			// Technically, should call GetVolumeInformation
			if (TargetFilename.Length > MaxFilenameLength - ExtensionLength) {
				TargetFilename = TargetFilename.Substring(0, MaxFilenameLength - ExtensionLength);
			}
			TargetFilename += ".pdf";
			return TargetFilename;
		}

//---------------------------------------------------------------------------------------

		void fsw_Renamed(object sender, RenamedEventArgs e) {
			Console.WriteLine("fsw_Renamed -- ChangeType={0}, Name={1}, FullPath={2}, OldName={3}, OldFullPath={4}", 
				e.ChangeType, e.Name, e.FullPath,
				e.OldName, e.OldFullPath);
		}

//---------------------------------------------------------------------------------------

		void fsw_Deleted(object sender, FileSystemEventArgs e) {
			Console.WriteLine("fsw_Deleted -- ChangeType={0}, Name={1}, FullPath={2}", e.ChangeType, e.Name, e.FullPath);
		}

//---------------------------------------------------------------------------------------

		void fsw_Changed(object sender, FileSystemEventArgs e) {
			// Console.WriteLine("<br/>fsw_Changed -- ChangeType={0}, Name={1}, FullPath={2}", e.ChangeType, e.Name, e.FullPath);
		}

//---------------------------------------------------------------------------------------

		private void GetSciAm_Load(object sender, EventArgs e) {
			SetDefaultValues();

			ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
		}

//---------------------------------------------------------------------------------------

		private void SetDefaultValues() {
			// Set up Year combo box
			var now = DateTime.Now;
			int year = now.Year;	// January issue comes out in December
			for (int i = now.Year; i >= 1993; i--) {
				cmbYear.Items.Add(i);
			}
			cmbYear.SelectedIndex = 0;

			// Set up Month combo box
			// Note: Not perfect; the next issue seems to come out about 3 weeks into
			//		 the previous month (e.g. the February issue comes out about the
			//		 third week of January). But this is close enough.
			cmbMonth.SelectedIndex = now.Month - 1;
		}

//---------------------------------------------------------------------------------------

		private void btnLogin_Click(object sender, EventArgs e) {
			State = FsmState.Login;
			webBrowser1.Navigate(parms.SciAmDigitalUrl);
		}

//---------------------------------------------------------------------------------------

		private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
			string msg = string.Format("In DocumentCompleted, IsBusy={0}, ReadyState={1}",
				webBrowser1.IsBusy, webBrowser1.ReadyState);
			Log(msg);
			Application.DoEvents();

			var web = sender as WebBrowser;
			var doc = web.Document;
			switch (State) {
			case FsmState.Login:
				ProcessState_Login(doc);
				break;
			case FsmState.LoggedIn:
				ProcessState_LoggedIn(web, doc);
				break;
			case FsmState.CurrentIssue:
				ProcessState_CurrentIssue(web, doc);
				break;
			case FsmState.SelectIssueWithinYear:
				ProcessState_SelectIssueWithinYear(doc);
				break;
			case FsmState.SpecificIssue:
				ProcessState_SpecificIssue(doc);
				break;
			case FsmState.Idle:			// No processing to be done. Fall into default
			default:
				break;
			}
		}

//---------------------------------------------------------------------------------------

		private void ProcessState_SpecificIssue(HtmlDocument doc) {
			lbArticles.Items.Clear();
			int SeqNo = 0;
			for (int i = 0; i < doc.Links.Count; i++) {
				var link = doc.Links[i];
				string desc = link.Parent.Parent.OuterText;
				desc = desc.Replace("\r", "").Replace("\n", "");
				var anchor = link.DomElement as mshtml.IHTMLAnchorElement; // HTMLAnchorElementClass;
				if (anchor == null) {
					continue;
				}
				string href = anchor.href;
				// We don't want all the links (e.g. Search, Subscribe, Help, etc). 
				// Empirically, the ones we want have an href that starts with a
				// specific URL prefix.
				if (href.StartsWith(parms.ArticleUrlPrefix)) {
					var info = new IssueDocInfo(++SeqNo, desc, anchor.href);
					Log("IssueDocInfo - " + info.ToString());
					lbArticles.Items.Add(info);
					// Console.WriteLine("link[{0}]: OuterText={1}, href={2}", i, desc, href);
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void ProcessState_SelectIssueWithinYear(HtmlDocument doc) {
			var div = doc.GetElementById("category");
			// Console.WriteLine("div.Children.Count = {0}", div.Children.Count);
			MonthIssueURL.Clear();
			for (int i = 0; i < div.Children.Count; i++) {
				if (i == 6)						// There's an extra <div> tag here. Ignore it.
					continue;
				if (i > 12)						// 12, not 11; we skipped 6
					break;
				var kid = div.Children[i];
				if (kid.FirstChild != null) {
					var anchor = kid.FirstChild.DomElement as IHTMLAnchorElement;
					MonthIssueURL.Add(anchor.href);
					// Console.WriteLine("kid[{0}] InnerText=\"{1}\", href={2}", i, StripWhiteSpace(kid.InnerText), anchor.href);
				}
			}
			State = FsmState.Idle;
		}

//---------------------------------------------------------------------------------------

		private void ProcessState_LoggedIn(WebBrowser web, HtmlDocument doc) {
			var div = doc.GetElementById("pod");
			var dom = div.DomElement as HTMLDivElement;
			var link = dom.firstChild as HTMLAnchorElement;
			string href = link.href;
			State = FsmState.Idle;
			cmbYear.Enabled = true;
			cmbMonth.Enabled = true;
			web.Navigate(href);
		}

//---------------------------------------------------------------------------------------

		private void ProcessState_CurrentIssue(WebBrowser web, HtmlDocument doc) {
			// TODO: Needed? See btnGetEntireIssue_Click
			// TODO: Needed at all?
			var frmIssue = FindForm(doc, "issueAdd_1");
			CurDocInfo = new IssueDocInfo(0, "Entire Issue", null);
			frmIssue.InvokeMember("submit");

#if false
			for (int i = 0; i < 3; i++) {
				var frm = FindForm(doc, "download" + i);
				frm.InvokeMember("submit");
			}
#endif
			State = FsmState.Idle;
		}

//---------------------------------------------------------------------------------------

		private HtmlElement FindForm(HtmlDocument doc, string frmName) {
			for (int i = 0; i < doc.Forms.Count; i++) {
				var frm = doc.Forms[i];
				if (frm.Name == frmName) {
					return frm;
				}
			}
			return null;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// We're at the login page. Fill in our Userid and Password and click Submit.
		/// </summary>
		/// <param name="doc"></param>
		private void ProcessState_Login(HtmlDocument doc) {
			var usernames = doc.All.GetElementsByName("USERNAME_CHAR");
			var username = usernames[0];
			usernames[0].InnerText = parms.UserId;
			var passwords = doc.All.GetElementsByName("PASSWORD_CHAR");
			passwords[0].InnerText = parms.Password;
			Application.DoEvents();

			var submits = doc.All.GetElementsByName("submit");
			IsLoggedIn = true;
			State = FsmState.LoggedIn;
			submits[0].InvokeMember("Click");
		}

//---------------------------------------------------------------------------------------

		public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { 
			return true; 
		}

//---------------------------------------------------------------------------------------

		private void btnGetEntireIssue_Click(object sender, EventArgs e) {
			// TODO: Copied from ProcessState_CurrentIssue
			var doc = webBrowser1.Document;
			bIsFileCopied = false;
			bAllArticlesProcessed = false;
			var frmIssue = FindForm(doc, "issueAdd_1");
			CurDocInfo = new IssueDocInfo(0, "Entire Issue", null);
			State = FsmState.Idle;
			frmIssue.InvokeMember("submit");
		}

//---------------------------------------------------------------------------------------

#if USE_FSW
		private void Log(FsmState state, string msg) {
			string color = FsmColors[(int)state].Name;
			string txt = string.Format("<FONT COLOR=\"{0}\">Current state: {1}, {2}</FONT><br/>", color, state, msg);
			WriteLog(txt);
		}

//---------------------------------------------------------------------------------------

		private void Log(string msg) {
			Log(State, msg);
		}

//---------------------------------------------------------------------------------------

		private void Log(Color color, string msg) {
			string txt = string.Format("<FONT COLOR=\"{0}\">{1}</FONT><br/>", color, msg);
			WriteLog(txt);
		}

//---------------------------------------------------------------------------------------

		[Conditional("WRITELOG")]
		private void WriteLog(string txt) {
			bool bGuiThread = GuiThreadId == System.Threading.Thread.CurrentThread.ManagedThreadId;
			if (!bGuiThread) {
				// TODO: Shouldn't be hardcoded 
				Trace.Write("<div>");
			}
			string tstamp = DateTime.Now.ToLongTimeString() + " TID=" + System.Threading.Thread.CurrentThread.ManagedThreadId;
			Trace.WriteLine(tstamp + " " + txt);
			if (!bGuiThread) {
			   // TODO: Shouldn't be hardcoded 
				Trace.WriteLine("</div>");
			}
		}
#endif

//---------------------------------------------------------------------------------------

		private string StripWhiteSpace(string s) {
			s = s.Replace("\r", "").Replace("\n", "").Replace("\t", "");
			return s.Trim();
		}

//---------------------------------------------------------------------------------------

		private void GetSciAm_FormClosed(object sender, FormClosedEventArgs e) {
			Process.Start(TraceFileName);
		}

//---------------------------------------------------------------------------------------

		private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e) {
			string msg = "Navigating to " + e.Url.ToString();
			Log(msg);
		}

//---------------------------------------------------------------------------------------

		private void cmbYear_SelectedValueChanged(object sender, EventArgs e) {
			cmbMonth.SelectedIndex = 0;         // Restart month at January
			CurYear = cmbYear.Text;
			string url = parms.SelectYearUrl;
			url = string.Format(url, cmbYear.Text);
			State = FsmState.SelectIssueWithinYear;
			webBrowser1.Navigate(url);
		}

//---------------------------------------------------------------------------------------

		private void cmbMonth_SelectedValueChanged(object sender, EventArgs e) {
			int nIssues = MonthIssueURL.Count;
			if (nIssues == 0) {			// We may not have logged in yet, or chosen a Year
				return;
			}
			bAllArticlesProcessed = false;
			CurMonth = cmbMonth.Text;
			CurMonthNumber = cmbMonth.SelectedIndex;
			// If we're in the latest year, then we might not have all 12 issues yet.
			// (In fact, unless this is December (or late Novemeber), we're pretty well
			// guaranteed not to have them all). So check to see if we're asking for
			// a non-existent issue)
			if (CurMonthNumber >= nIssues) {
				MessageBox.Show("Sorry, but that issue doesn't exist yet", "GetSciAm",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			State = FsmState.SpecificIssue;
			webBrowser1.Navigate(MonthIssueURL[CurMonthNumber]);
		}

//---------------------------------------------------------------------------------------

		private void lbArticles_DoubleClick(object sender, EventArgs e) {
			bAllArticlesProcessed = false;
			GetNextArticle();
		}

//---------------------------------------------------------------------------------------

		private void GetArticle() {
			try {
				Log(Color.Blue, "In GetArticle -- lbArticles.SelectedIndex = " + lbArticles.SelectedIndex 
					+ ", wb IsBusy=" + webBrowser1.IsBusy);
#if false        // TODO:
				for (int i = 0; i < 10; i++) {
					if (webBrowser1.IsBusy == true) {
					   Log(Color.Red, "Web IsBusy = " + webBrowser1.IsBusy);
					   Application.DoEvents();
					   System.Threading.Thread.Sleep(1000);
					}
				}
#endif
				if (bAllArticlesProcessed) {
					return;
				}
				if (lbArticles.SelectedIndex < 0) {
					lbArticles.SelectedIndex = 0;
				}
				bIsFileCopied = false;
				CurDocInfo = (IssueDocInfo)lbArticles.SelectedItem;
				int n = lbArticles.SelectedIndex + 1;	// Forms are download1, download2, etc
				Log(Color.LightBlue, "About to get article " + n);
				var doc = webBrowser1.Document;
				var frm = FindForm(doc, "download" + n);
				frm.InvokeMember("submit");
				//frm.AttachEventHandler
			} catch (Exception ex) {
				string msg = string.Format("Exception in GetArticle - {0}", ex.Message);
				Log(Color.DarkRed, msg);
				MessageBox.Show(msg, "Exception in GetArticle");
			}
		}

//---------------------------------------------------------------------------------------

		private void btnGetAllArticles_Click(object sender, EventArgs e) {
			GetNextArticle();
		}

//---------------------------------------------------------------------------------------

		private void GetNextArticle() {
			GetArticle();
			if (lbArticles.SelectedIndex + 1 < lbArticles.Items.Count) {
				lbArticles.SelectedIndex = lbArticles.SelectedIndex + 1;
			} else {
				bAllArticlesProcessed = true;
			}
			Log(Color.Purple, "In GetNextArticle -- lbArticles.SelectedIndex = " + lbArticles.SelectedIndex);
		}

//---------------------------------------------------------------------------------------

		private void btnCleanSciAmTempDirectory_Click(object sender, EventArgs e) {
			foreach (string filename in Directory.GetFiles(parms.SourceDirectory, "*.pdf")) {
				Log(Color.DeepSkyBlue, "Deleting file " + filename);
				File.Delete(Path.Combine(parms.SourceDirectory, filename));
			}
			MapFilenames.Clear();
			// MessageBox.Show("Directory cleaned", "GetSciAm");
		}

#if false
//---------------------------------------------------------------------------------------

		private string /* CcLeadsDataStatus */ SendViaHttpPost(string Url, WebHeaderCollection Headers, string Data) {
			// See http://geekswithblogs.net/rakker/archive/2006/04/21/76044.aspx
			// Url = "https://www-test.wingateweb.com/leadretrieval/redirect/sendBartizanLeads.jsp";
			int TimeoutPeriod = 15000;          // 15 seconds. May be too little for longer articles
			try {
				// LogCcLeadsError("In SendViaHttpPost, Url=" + Url);	// TODO:
				UTF8Encoding encoding = new UTF8Encoding();
				byte[] PostData = encoding.GetBytes(Data);

				// Prepare web request...
				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
				req.Method = "POST";
				req.Timeout	= TimeoutPeriod;
				req.AllowAutoRedirect = false;			
				req.ContentType = "application/x-www-form-urlencoded";
				req.ContentLength = PostData.Length;
				req.Headers = Headers;
				// req.CookieContainer = 
				Stream newStream = req.GetRequestStream();
				// Send the data.
				newStream.Write(PostData, 0, PostData.Length);
				newStream.Close();

				string	RemoteData = "";
				using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse()) {
					// We may be redirected. Check for this and re-issue the request. 
					// Note: Requires AllowAutoRedirect = false above.
					if (resp.StatusCode == HttpStatusCode.Redirect) {
						// Call ourselves recursively, then immediately exit this frame
						string NewUrl = resp.Headers["Location"];  
						return SendViaHttpPost(NewUrl, Headers, Data);
					}
					using (Stream responseStream = resp.GetResponseStream()) {
						using (StreamReader readStream = new StreamReader(responseStream, Encoding.UTF8)) {
							RemoteData = readStream.ReadToEnd();
						}
					}
				}
				return RemoteData;
			} catch (WebException wex) {
				// LogCcLeadsError("URL = " + Url, wex);
				// LogCcLeadsError("POST data = " + Data);
				Log(Color.Red, "WebException in SendViaHttpPost - " + wex.Message);
				return null;
			} catch (Exception ex) {
				// LogCcLeadsError(ex);
				// LogCcLeadsError("POST data = " + Data);
				Log(Color.Red, "Exception in SendViaHttpPost - " + ex.Message);
				return null;
			}
		}
#endif

//---------------------------------------------------------------------------------------

		private void btnTest_Click(object sender, EventArgs e) {
#if false
			WebHeaderCollection hdrs = new WebHeaderCollection();
			string data = "Hello world";
			string result = SendViaHttpPost(parms.SciAmDigitalUrl, hdrs, data);
			UTF8Encoding encoding = new UTF8Encoding();
			byte[] pdfData = encoding.GetBytes(result);
			var pdf = File.Create("foo.pdf");
			pdf.Write(pdfData, 0, pdfData.Length);
			pdf.Close();
#endif
		}

//---------------------------------------------------------------------------------------

		private void cmbYear_SelectedIndexChanged(object sender, EventArgs e) {
			cmbMonth.SelectedIndex = 0;     // Reset to January
		}

//---------------------------------------------------------------------------------------

		private void btnRenameFiles_Click(object sender, EventArgs e) {
			foreach (string fname in MapFilenames.Keys) {
				Console.WriteLine("Moving {0} to {1}", fname, MapFilenames[fname]);
				// We may be restarting the download for some reason. Delete target file
				// if present.
				if (! File.Exists(MapFilenames[fname])) {
					File.Delete(MapFilenames[fname]);
				}
				File.Move(Path.Combine(parms.SourceDirectory, fname), MapFilenames[fname]);
			}
		}

//---------------------------------------------------------------------------------------

		private void webBrowser1_FileDownload(object sender, EventArgs e) {
			// TODO:
		}
	}
}