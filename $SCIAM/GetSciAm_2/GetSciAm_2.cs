using System;
using System.Windows.Forms;

namespace GetSciAm_2 {
	public partial class GetSciAm_2 : Form {

		const string SciAmBaseUrl = "http://www.ScientificAmerican.com";

		enum FsmState {
			Login
		}

		FsmState State = FsmState.Login;

//---------------------------------------------------------------------------------------

		public GetSciAm_2() {

//---------------------------------------------------------------------------------------

			InitializeComponent();

			web.Navigate(SciAmBaseUrl + "/my-account/login/");
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			ProcessYear(2000);
		}

//---------------------------------------------------------------------------------------

		private void ProcessYear(int Year) {
			for (int MonthNo = 1; MonthNo <= 12; MonthNo++) {
				var info = new IssueInfo(Year, MonthNo);
				ProcessIssue(info);
			}
		}

//---------------------------------------------------------------------------------------

		private void ProcessIssue(IssueInfo info) {
			string url = SciAmBaseUrl + "/magazine/sa/" + info.Year + "/" + info.MonthNumber.ToString("D2") + "-01";
			web.Navigate(url);
		}

//---------------------------------------------------------------------------------------

		private void web_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
			if (web.ReadyState != WebBrowserReadyState.Complete) {
				return;
			}

			var doc  = web.Document;
			var body = doc.Body;

			if (doc != null) {
				// return;				// TODO: TODO: TODO:
			}

			switch (State) {
			case FsmState.Login:
				var forms = body.GetElementsByTagName("form");
				foreach (var form in forms) {
					var f = form as HtmlElement;
					if (f.Id == "login") {
						bool bGotLogin = false;
						var inputs = f.GetElementsByTagName("input");
						for (int i = 0; i < inputs.Count; i++) {
							var input = inputs[i];
							switch (input.Name) {
							case "emailAddress":
								input.InnerText = "lrs5@lrs5.net";
								bGotLogin = true;
								break;
							case "password":
								input.InnerText = "lrs_5_Sciam";
								// Assume that if we got emailAddress, we got this too
								break;
							default:
								break;
							}
						}
						// Now find the Sign In button
						var buttons = f.GetElementsByTagName("button");
						// TODO: Check for exactly 1
						// buttons[0].InvokeMember("Click");
						break;
					}
				}
				break;
			default:
				break;
			}
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class IssueInfo {
		public int		Year;
		public int		MonthNumber;
		public string	MonthName;
		public bool		bIsArticle;			// If false, whole issue
		public string	ArticleName;
		 
//---------------------------------------------------------------------------------------

		public IssueInfo(int Year, int MonthNumber) {
			this.Year = Year;
			this.MonthNumber = MonthNumber;
			// TODO: More
		}
	}
}
