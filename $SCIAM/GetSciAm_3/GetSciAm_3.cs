using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using mshtml;
using SHDocVw;

namespace GetSciAm_3 {
	public partial class GetSciAm_3 : Form {

		const string SciAmBaseUrl = "http://www.ScientificAmerican.com";

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

		string			ClipboardContents = null;

//---------------------------------------------------------------------------------------

		public GetSciAm_3() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void GetSciAm_3_Load(object sender, EventArgs e) {
			IE = new SHDocVw.InternetExplorer();
			PositionWindows();

			IE.Visible = true;

			IE.DocumentComplete += IE_DocumentComplete;

			SetUpComboBoxes();
			
			IE.Navigate(SciAmBaseUrl + "/my-account/login/");
		}

//---------------------------------------------------------------------------------------

		private void SetUpComboBoxes() {
			for (int Decade = 2010; Decade >= 1840; Decade -= 10) {
				cmbDecade.Items.Add(Decade);
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

		private void btnGo_Click(object sender, EventArgs e) {
			// var wc = new WebClient();			
			// var data = wc.DownloadData("http://www.scientificamerican.com/magazine/sa/2013/08-01/");
			// var html = Encoding.ASCII.GetString(data);
			// int ix = html.IndexOf("Download Full Issue PDF");
			GoToIssue();
		}

//---------------------------------------------------------------------------------------

		private void cmbYear_SelectedIndexChanged(object sender, EventArgs e) {
			cmbMonth.SelectedIndex = 0;
		}

//---------------------------------------------------------------------------------------

		private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e) {
			// GoToIssue();
		}

//---------------------------------------------------------------------------------------

		private void GoToIssue() {
			Clipboard.SetText(" ");
			var tmr = new Timer();
			tmr.Interval = 1000;
			tmr.Tick += Tmr_Tick;
			tmr.Enabled = true;
			info = new IssueInfo((int)cmbYear.SelectedItem, cmbMonth.SelectedIndex + 1);
			ProcessIssue(info);
		}

//---------------------------------------------------------------------------------------

		private void Tmr_Tick(object sender, EventArgs e) {
			string txt = Clipboard.GetText();
			if (txt != ClipboardContents) {
				ClipboardContents = txt;
				txtIssueTitle.Text = txt;
			}
		}

//---------------------------------------------------------------------------------------

		private void btnSaveAs_Click(object sender, EventArgs e) {
			// string IssueName = Clipboard.GetText();
			string IssueName = "";
			string SaveAsBase = string.Format(@"D:\LRS\$SciAm-Full Issues\Sciam - {0}",
				info.Year);
			Directory.CreateDirectory(SaveAsBase);
			string SaveAs = SaveAsBase + string.Format(@"\Sciam - {0}-{1:D2}-{2} - {3}", info.Year, info.MonthNumber, info.MonthName, IssueName);
			Clipboard.SetText(SaveAs);
			MessageBox.Show("Issue name on clipboard");
			IE.ExecWB(OLECMDID.OLECMDID_SAVEAS, OLECMDEXECOPT.OLECMDEXECOPT_PROMPTUSER);

			OnToNextMonth();
		}

//---------------------------------------------------------------------------------------

		private void OnToNextMonth() {
			// Now update to next month. But if December, then increment year index. But
			// this may spill over into another decade. 

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
			IssueUrl = SciAmBaseUrl + "/magazine/sa/" + info.Year + "/" + info.MonthNumber.ToString("D2") + "-01/";
			IssueUrl = IssueUrl.ToLower();
			IE.Navigate(IssueUrl);
		}

		//---------------------------------------------------------------------------------------

		private void IE_DocumentComplete(object pDisp, ref object URL) {
			if (IE.ReadyState != tagREADYSTATE.READYSTATE_COMPLETE) {
				return;
			}

			HTMLDocument doc;
			HTMLBody	 body;

			// If we're trying to load a non-HTML-document into the browser (e.g. a .pdf
			// file), we won't have an IE.Document. If so, just return.
			try {
				doc = IE.Document;
				body = doc.body as HTMLBody;
			} catch (Exception /* ex */) {
				// IE.ExecWB(OLECMDID.OLECMDID_SAVEAS, OLECMDEXECOPT.OLECMDEXECOPT_PROMPTUSER);
				return;
			}

			if (doc != null) {
				// return;				// TODO: TODO: TODO:
			}

			switch (State) {
			case FsmState.Login:
				DoLogin(body);
				break;
			case FsmState.LoggedIn:
				if (IssueUrl == (string)URL) {
					DownloadIssue(body);
				}
				break;
			default:
				break;
			}
		}

//---------------------------------------------------------------------------------------

		private void DownloadIssue(HTMLBody body) {
			string FullHTML = body.outerHTML;
#if DEBUG
			// We get a System.Threading.ThreadStateException when trying
			// to put text on the clipboard. It claims we need the STAThread attribute
			// set on Main. But clearly that's for the IE process. So we'll kludge our
			// way around it.
			// Clipboard.SetText(FullHTML);
			using (var f = new StreamWriter(@"D:\LRS\GetSciam3.txt", false)) {
				f.Write(FullHTML);
			}
#endif
			var anchors = body.getElementsByTagName("a");
			foreach (HTMLAnchorElement anchor in anchors) {
				// Console.WriteLine("==================");
				string name  = anchor.name;
				string inner = anchor.innerHTML;
				string outer = anchor.outerHTML;
				Console.WriteLine("Name={0}\r\nInner HTML={1}\r\nOuter HTML={2}",
							name, inner, outer);

				// TODO: Bit of a kluge here...
				if (inner == "Download Full Issue PDF") {
					PdfHref = anchor.href;
					break;
				}

				// According to http://stackoverflow.com/questions/2048720/get-all-attributes-from-a-html-element-with-javascript-jquery,
				// "in IE7 elem.attributes lists *all possible attributes*. Quelle pain!
				// Which actually doesn't quite make sense. I might believe all *standard*
				// attributes, but what about custom ones???
				// foreach (var attr in anchor.attributes) {
				// 	Console.WriteLine("{0}", attr.name);
				// }
				var ClassAttr = anchor.getAttribute("class");
				if (! (ClassAttr is DBNull)) {
					int i = 1;
				}
			}
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

		private void DoLogin(HTMLBody body) {
			bool bGotLogin = false;
			var forms = body.getElementsByTagName("form");
			foreach (var form in forms) {
				var f = form as mshtml.HTMLFormElement;
				if (f.id == "login") {
					var inputs = f.getElementsByTagName("input");
					foreach (mshtml.HTMLInputElement input in inputs) {
						Console.WriteLine("{0}", input.name);
						switch (input.name) {
						case "emailAddress":
							input.innerText = "lrs5@lrs5.net";
							bGotLogin = true;
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
						MessageBox.Show("Could not autofill email address and password", "Get Scientific American - 3",
							MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					break;
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void GetSciAm_3_FormClosed(object sender, FormClosedEventArgs e) {
			IE.Quit();
		}

//---------------------------------------------------------------------------------------

		private void btnGoToPDF_Click(object sender, EventArgs e) {
			IE.Navigate(PdfHref);
		}

//---------------------------------------------------------------------------------------

		private void cmbDecade_SelectedIndexChanged(object sender, EventArgs e) {
			int Decade = (int)cmbDecade.SelectedItem;
			cmbYear.Items.Clear();
			for (int Year = Decade + 9; Year >= Decade; Year--) {
				cmbYear.Items.Add(Year);
			}
			cmbYear.SelectedIndex = 0;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class IssueInfo {
	public int Year;
	public int MonthNumber;
	public string MonthName;
	public bool bIsArticle;			// If false, whole issue
	public string ArticleName;


//---------------------------------------------------------------------------------------

	public IssueInfo(int Year, int MonthNumber) {
		this.Year = Year;
		this.MonthNumber = MonthNumber;
		this.MonthName = MonthNames.Names[MonthNumber - 1];
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
