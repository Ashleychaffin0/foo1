using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using mshtml;
using SHDocVw;

// The following may be relevant/of interest: http://stackoverflow.com/questions/15419632/download-a-file-through-the-webbrowser-control

// See also urlmon.dll and UrlDownloadToFile -- https://msdn.microsoft.com/en-us/library/ms775123(v=vs.85).aspx

namespace GetSciAm_4 {

	public partial class GetSciAm_4 : Form {

		[DllImport("user32.dll")]
		public static extern IntPtr FindWindow(String sClassName, String sAppName);

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", SetLastError = true)]
		static extern bool PostMessage(IntPtr /* HandleRef */ hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		const string SciAmBaseUrl = "http://www.ScientificAmerican.com";

		const string TargetDir = @"G:\LRS-8500\$SciAm-Full Issues\Sciam";

		// SHDocVw.WebBrowser web2;
		SHDocVw.InternetExplorer	IE;

		FsmState State = FsmState.Login;

		enum FsmState {
			Login,
			LoggedIn
		}

		string			PdfHref = null;

		string			IssueUrl = null;
		IssueInfo		info = null;

		// string			ClipboardContents = null;

		SynchronizationContext sc = null;

		long			TotReceivedAtStart;

		bool			bDoSaveAs = false;

		bool			bWeeklyMode;

		System.Windows.Forms.Timer tmr = new System.Windows.Forms.Timer();

//---------------------------------------------------------------------------------------

		public GetSciAm_4() {
			InitializeComponent();

			sc = WindowsFormsSynchronizationContext.Current;
		}

//---------------------------------------------------------------------------------------

		private void GetSciAm_4_Load(object sender, EventArgs e) {
			IE = new SHDocVw.InternetExplorer();
			PositionWindows();
			
			bWeeklyMode = true;
			IE.Visible = true;

			IE.DocumentComplete += IE_DocumentComplete;

			SetUpComboBoxes();
			
			IE.Navigate(SciAmBaseUrl + "/my-account/login/");
		}

//---------------------------------------------------------------------------------------

		private void SetUpComboBoxes() {
			for (int Decade = 2010; Decade >= 1840; Decade -= 10) {
				cmbDecade.Items.Add(Decade + "'s");
			}

			for (int MonthNum = 0; MonthNum < MonthNames.Names.Length; MonthNum++) {
				cmbMonth.Items.Add(MonthNames.Names[MonthNum]);
			}

			cmbDecade.SelectedIndex = 0;
			cmbYear.SelectedIndex   = 0;
			cmbMonth.SelectedIndex  = 0;
		}

//---------------------------------------------------------------------------------------

		private void PositionWindows() {
			Rectangle rect = Screen.PrimaryScreen.Bounds;
			this.Top       = 0;
			this.Left      = 0;
			this.Width     = rect.Width;

			IE.Top    = this.Height;
			IE.Left   = 0;
			IE.Height = rect.Height - this.Height;
			IE.Width  = rect.Width;
		}
//---------------------------------------------------------------------------------------

		private void SetTimer() {
			// TODO: Do this only once
			var tmr      = new System.Windows.Forms.Timer();
			tmr.Interval = 1000;
			tmr.Tick += Tmr_Tick;
			tmr.Enabled  = true;
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			// var wc = new WebClient();			
			// var data = wc.DownloadData("http://www.scientificamerican.com/magazine/sa/2013/08-01/");
			// var html = Encoding.ASCII.GetString(data);
			// int ix = html.IndexOf("Download Full Issue PDF");
			GoToIssue();
		}

//---------------------------------------------------------------------------------------

		private void cmbYear_SelectedIndexChanged(object sender, EventArgs e) {
			int Year               = (int)cmbYear.SelectedItem;
			WeekPicker.Value       = new DateTime(Year, 1, 1);
			cmbMonth.SelectedIndex = 0;
		}

//---------------------------------------------------------------------------------------

		private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e) {
			// TODO: Isn't called if we change year, but don't change month. e.g. from
			//		 January 1979 to January 2014
			int Year         = (int)cmbYear.SelectedItem;
			int Month        = cmbMonth.SelectedIndex + 1;
			WeekPicker.Value = new DateTime(Year, Month, 1);
		}

//---------------------------------------------------------------------------------------

		private void GoToIssue() {
			sc.Post(txt => lbTrace.Items.Insert(0, txt), "------------------");
			TraceCallStackTop();
			sc.Post(o => btnSaveAs.Enabled = false, null);
			Clipboard.SetText(" ");
			// TODO: Do this only once
			// var tmr      = new System.Windows.Forms.Timer();
			// tmr.Interval = 1000;
			// tmr.Tick += Tmr_Tick;
			// tmr.Enabled  = true;
			// TODO: This is never disabled. Should it be, someplace?
			info = new IssueInfo((int)cmbYear.SelectedItem, cmbMonth.SelectedIndex + 1, WeekPicker);
			ProcessIssue(info);
		}

//---------------------------------------------------------------------------------------

		private void Tmr_Tick(object sender, EventArgs e) {
			// string txt = Clipboard.GetText();
			// if (txt != ClipboardContents) {
			// 	  ClipboardContents = txt;
			//	  txtIssueTitle.Text = txt;
			// }

			long HowMuch = GetBytesReceived();
			lblDownloadInMB.Text = fmt(HowMuch - TotReceivedAtStart);

			if (bDoSaveAs) {
				bDoSaveAs = false;
				const int BM_CLICK = 0x00F5;
				PostMessage(btnSaveAs.Handle, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
				// btnSaveAs.PerformClick();
				// DoSaveAs();
			}
		}

//---------------------------------------------------------------------------------------

		private void btnSaveAs_Click(object sender, EventArgs e) {
			// bDoSaveAs = true;
			DoSaveAs();
		}

//---------------------------------------------------------------------------------------

		private void DoSaveAs() {
			TraceCallStackTop();
			// GC.Collect();                   // Dunno if this will help
			lblDownloadInMB.Text = "";
			Application.DoEvents();
			SetCursorOnNextButton();
			string IssueName = "";
			string SaveAsBase;
			string SaveAs;

			if (bWeeklyMode) {
				string Day = info.WeekPicker.Value.Day.ToString("D2");
				IssueName  = $"{MonthNames.Names[info.WeekPicker.Value.Month - 1]} {Day}";
				SaveAsBase = $"{TargetDir} - {info.WeekPicker.Value.Year}";
				SaveAs     = SaveAsBase + $@"\Sciam - {info.WeekPicker.Value.Year}-{info.WeekPicker.Value.Month:D2}-{Day} - {IssueName}";
			} else {
				SaveAsBase = string.Format(TargetDir + " - {0}", info.Year);
				SaveAs     = SaveAsBase + string.Format(@"\Sciam - {0}-{1:D2}-{2} - {3}", info.Year, info.MonthNumber, info.MonthName, IssueName);
			}
			Directory.CreateDirectory(SaveAsBase);
			Clipboard.SetText(SaveAs);

			TraceMsg("***** About to tell IE to Save As *****");
			Application.DoEvents();
			// Note: Maybe https://forum.xojo.com/12096-internet-explorer-automation-oleobject-execwb/0 is of help
			// IE.ExecWB(OLECMDID.OLECMDID_SAVEAS, OLECMDEXECOPT.OLECMDEXECOPT_PROMPTUSER);
			IE.ExecWB(OLECMDID.OLECMDID_SAVEAS, OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER, SaveAs, null);

			// sc.Post(o => btnSaveAs.Enabled = true, null);

			// sc.Post(o => FillInSaveAsFilename(), null);
			FillInSaveAsFilename();
			// Application.DoEvents();

			OnToNextMonth();
		}

//---------------------------------------------------------------------------------------

		private void FillInSaveAsFilename() {
			Thread.Sleep(250);

			int tid = System.Threading.Thread.CurrentThread.ManagedThreadId;
			TraceMsg($"FillInSaveAsFilename - ThreadID = {tid}");

			TraceCallStackTop();
			IntPtr hWnd = WaitForSaveAs();
			if (hWnd != IntPtr.Zero) {
				bool bOK = SetForegroundWindow(hWnd);
				SendKeys.Send("^v");            // Paste + Enter
				if (bWeeklyMode) {
					// If weekly, no "title" for this issue. Just close the window
					SendKeys.Send("~");			// Enter
				}
				Application.DoEvents();
				// bDoSaveAs = true;
			}
		}

//---------------------------------------------------------------------------------------

		private IntPtr WaitForSaveAs() {
			TraceCallStackTop();
			IntPtr hWnd;
			int Delay      = 100;			// Milliseconds
			int MaxDelay   = 1000;			// Milliseconds
			int TotalDelay = 0;
			do {
				Thread.Sleep(Delay);
				Application.DoEvents();
				hWnd       = FindWindow(null, "Save As");
				TotalDelay += Delay;

			} while ((hWnd == IntPtr.Zero) && (TotalDelay <= MaxDelay));
			if (TotalDelay <= MaxDelay) {
				return hWnd;
			} else {
				Application.DoEvents();
				TraceMsg("Could not find Save As window");
				return IntPtr.Zero;
			}
		}

//---------------------------------------------------------------------------------------

		private void SetCursorOnNextButton() {
			TraceCallStackTop();
			var loc = btnNext.Location;
			loc.Offset(btnNext.Width / 2, btnNext.Height / 2 + 30);
			Cursor.Position = loc;
			Application.DoEvents();
		}

//---------------------------------------------------------------------------------------

		private void OnToNextMonth() {
			// Now update to next month. But if December, then increment year index. But
			// this may spill over into another decade. 

			if (bWeeklyMode) {
				return;
			}

			int ix = cmbMonth.SelectedIndex;
			if (++ix >= cmbMonth.Items.Count) {
				OnToNextYear();
			} else {
				cmbMonth.SelectedIndex = ix;
			}
		}

//---------------------------------------------------------------------------------------

		private void OnToNextYear() {
			int ix = cmbYear.SelectedIndex;
			if (++ix >= cmbYear.Items.Count) {
				OnToNextDecade();
			} else {
				cmbYear.SelectedIndex = ix;
			}
		}

//---------------------------------------------------------------------------------------

		private void OnToNextDecade() {
			int ix = cmbDecade.SelectedIndex;
			cmbDecade.SelectedIndex = ++ix;
		}

#if false
		//---------------------------------------------------------------------------------------

		private void ProcessYear(int Year) {
			for (int MonthNo = 1; MonthNo <= 12; MonthNo++) {
				info = new IssueInfo(Year, MonthNo);
				ProcessIssue(info);
				break;						// TODO:
			}
		}
#endif
//---------------------------------------------------------------------------------------

		private void ProcessIssue(IssueInfo info) {
			TraceCallStackTop();
			lblDownloadInMB.Text = "";
			Application.DoEvents();

			if (bWeeklyMode) {
				IssueUrl = SciAmBaseUrl + "/magazine/sa/" + info.WeekPicker.Value.Year + "/" + info.WeekPicker.Value.Month.ToString("D2") + "-" + info.WeekPicker.Value.Day.ToString("D2");
			} else {
				IssueUrl = SciAmBaseUrl + "/magazine/sa/" + info.Year + "/" + info.MonthNumber.ToString("D2") + "-01/";
			}
			IssueUrl = IssueUrl.ToLower();
			btnGoToPDF.Enabled = false;
			IE.Navigate(IssueUrl);
		}

//---------------------------------------------------------------------------------------

		private void IE_DocumentComplete(object pDisp, ref object URL) {
			// TODO: Do we sort of hang because the main document completes
			//		 (i.e. IssueUrl == URL) *before* all the other files are downloaded?
			if (IE.ReadyState != tagREADYSTATE.READYSTATE_COMPLETE) {
				return;
			}

			// int tid = System.Threading.Thread.CurrentThread.ManagedThreadId;
			// TraceMsg($"IE_DocumentComplete - ThreadID = {tid}");

			HTMLDocument doc;
			HTMLBody	 body;

			// If we're trying to load a non-HTML-document into the browser (e.g. a .pdf
			// file), we won't have an IE.Document. If so, just return.
#if true
			// Avoid exception handling
			doc = IE.Document as HTMLDocument;
			if (doc == null) {
				// bDoSaveAs = true;
				return;
			} else {
				body = doc.body as HTMLBody;
			}
			TraceCallStackTop();
#else
			try {
				doc = IE.Document;
				body = doc.body as HTMLBody;
			} catch (Exception /* ex */) {
				// It's not a web page, so must be our PDF file. Save it...
				// But we're in the IE address space. Set a flag and our timer routine
				// will call DoSaveAs();
				bDoSaveAs = true;
				return;
			}
#endif

			switch (State) {
			case FsmState.Login:
				DoLogin(body);
				break;
			case FsmState.LoggedIn:
				if ((IssueUrl + "/" == (string)URL) || (IssueUrl == (string)URL)) {     // Weekly: Added + "/"
					TraceMsg($"URL = {IssueUrl}");
					DownloadIssue(body);
				}
				break;
			default:
				break;
			}
		}

//---------------------------------------------------------------------------------------

		private void DownloadIssue(HTMLBody body) {
			TraceCallStackTop();
			var anchors = body.getElementsByTagName("a");
			foreach (HTMLAnchorElement anchor in anchors) {
				// Console.WriteLine("==================");
				// string name  = anchor.name;
				string inner = anchor.innerHTML;
				// string outer = anchor.outerHTML;
				// Console.WriteLine("Name={0}\r\nInner HTML={1}\r\nOuter HTML={2}",
				// 			name, inner, outer);

				// TODO: Bit of a kludge here...
				if ((inner != null) && inner.Contains("Download Issue PDF")) {
					PdfHref = anchor.href;
					// sc.Post(o => btnGoToPDF.Enabled = true, null);
					// See also UrlDownload{Something-or-other) in Urlmon.dll
					TotReceivedAtStart = GetBytesReceived();
					TraceMsg("About to go to " + PdfHref);
					IE.Navigate(PdfHref);
					sc.Post(o => btnSaveAs.Enabled = true, null);
					return;
				}

				// According to http://stackoverflow.com/questions/2048720/get-all-attributes-from-a-html-element-with-javascript-jquery,
				// "in IE7 elem.attributes lists *all possible attributes*. Quelle pain!
				// Which actually doesn't quite make sense. I might believe all *standard*
				// attributes, but what about custom ones???
				// foreach (var attr in anchor.attributes) {
				// 	Console.WriteLine("{0}", attr.name);
				// }
#if false
				var ClassAttr = anchor.getAttribute("class");
				if (! (ClassAttr is DBNull)) {
					int i = 1;
				}
#endif
			}
			TraceMsg("Could not find Download Issue PDF");
#if false
			var h4s = body.getElementsByTagName("h4");
			foreach (HTMLHeaderElement h4 in h4s) {
				string text = h4.innerText;
				var parentElement = h4.parentElement;	// HTMLElement
				var nextSibling = h4.nextSibling;
				var nextSibling2 = nextSibling.nextSibling;
				// var nextSibling3 = nextSibling.nextSibling2;
				// var nextSibling4 = nextSibling.nextSibling3;
				// var nextSibling5 = nextSibling.nextSibling4;
				var parentNode = h4.parentNode;			// HTMLDOMNode
			}
#endif
		}

//---------------------------------------------------------------------------------------

		private void TraceMsg(string text) {
			sc.Post(txt => lbTrace.Items.Insert(0, txt), text);
			Application.DoEvents();
		}

//---------------------------------------------------------------------------------------

		private void DoLogin(HTMLBody body) {
			TraceCallStackTop();
			string txt = body.innerText;
			if (txt.Contains("(sign out)")) {
				State = FsmState.LoggedIn;
				return;
			}
			bool bGotLogin                    = false;
			var forms                         = body.getElementsByTagName("form");
			foreach (var form in forms) {
				var f                         = form as mshtml.HTMLFormElement;
				// Console.WriteLine("Form ID = {0}", f.id);
				if (f.id                      == "login") {
					var inputs                = f.getElementsByTagName("input");
					foreach (mshtml.HTMLInputElement input in inputs) {
						// Console.WriteLine("{0}", input.name);
						switch (input.name) {
						case "emailAddress":
							input.innerText   = "lrs5@lrs5.net";
							bGotLogin         = true;
							continue;
						case "password":
							input.innerText = "lrs_5_Sciam";
							// Assume that if we got emailAddress, we got this too
							break;
						default:
							continue;
						}
					}
					if (bGotLogin) {
						State = FsmState.LoggedIn;
						// Now find the Sign In button
						var buttons = f.getElementsByTagName("button");
						// TODO: Check for exactly 1
						foreach (HTMLInputButtonElement button in buttons) {
							button.click();
							return;				// Assume the button we want is first
						}
					} else {
						MessageBox.Show("Could not autofill email address and password", "Get Scientific American - 4",
							MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					break;
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void GetSciAm_4_FormClosed(object sender, FormClosedEventArgs e) {
			IE.Quit();
		}

//---------------------------------------------------------------------------------------

		private void btnGoToPDF_Click(object sender, EventArgs e) {
			if (PdfHref != null) {
				IE.Navigate(PdfHref);
				// PdfHref = null;
			} else {
				MessageBox.Show("PDF not yet found");
			}
		}

//---------------------------------------------------------------------------------------

		private void cmbDecade_SelectedIndexChanged(object sender, EventArgs e) {
			string sDecade = (string)cmbDecade.SelectedItem;
			int Decade     = Convert.ToInt32(sDecade.Substring(0, 4));
			cmbYear.Items.Clear();
			for (int Year = Decade + 9; Year >= Decade; Year--) {
				cmbYear.Items.Add(Year);
			}
			cmbYear.SelectedIndex = 0;
		}

//---------------------------------------------------------------------------------------

		private long GetBytesReceived() {
			// TraceCallStackTop();
			long Received = 0;
			var ifaces = NetworkInterface.GetAllNetworkInterfaces();
			foreach (var iface in ifaces) {
				var stats = iface.GetIPv4Statistics();
				// if ((stats.BytesReceived + stats.BytesSent) == 0)
				// 	continue;
				Received += stats.BytesReceived;
				// Sent += stats.BytesSent;
			}
			return Received;
		}

//---------------------------------------------------------------------------------------

		private string fmt(long n) {
			long Factor = 1000 * 1000;
			long val = (n + Factor / 2) / Factor;
			return string.Format("{0:N0} MB", val);
		}

//---------------------------------------------------------------------------------------

		private void chkWeekly_CheckedChanged(object sender, EventArgs e) {
			bWeeklyMode = ! bWeeklyMode;
		}

//---------------------------------------------------------------------------------------

		private void btnNext_Click(object sender, EventArgs e) {
			TraceCallStackTop();
			WeekPicker.Value = WeekPicker.Value.AddDays(7);
			GoToIssue();
		}

//---------------------------------------------------------------------------------------

		private void TraceCallStackTop() {
			// return;
			var FullTrace = Environment.StackTrace.Split('\n');
			var st      = FullTrace[3];
			st          = st.Substring(0, st.IndexOf('('));
			string line = st.Substring(st.LastIndexOf('.') + 1);
			// line     = line.Substring(0, line.IndexOf('('));
			TraceMsg($"{line} -- Stacksize = {FullTrace.Length}");
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class IssueInfo {
		public int				Year;
		public int				MonthNumber;
		public string			MonthName;
		public bool				bIsArticle;			// If false, whole issue
		public string			ArticleName;
		public DateTimePicker	WeekPicker;

//---------------------------------------------------------------------------------------

	public IssueInfo(int Year, int MonthNumber, DateTimePicker WeekPicker) {
		this.Year        = Year;
		this.MonthNumber = MonthNumber;
		this.MonthName   = MonthNames.Names[MonthNumber - 1];
		this.WeekPicker  = WeekPicker;
		// TODO: Maybe more
	}
}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public static class MonthNames {
		public static string[] Names = { "January", "February", "March", "April", "May",
			"June", "July", "August", "September", "October", "November", "December"};

	}
}
