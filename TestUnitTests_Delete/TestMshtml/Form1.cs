using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

// using mshtml;

// using HtmlAgilityPack;

namespace TestMshtml {

	public partial class Form1 : Form {

//---------------------------------------------------------------------------------------

		internal enum FsmState {
			Login,
			LoggedIn,
			CurrentIssue,
			SelectIssueWithinYear,
			SpecificIssue,
			Idle
		}

		FsmState _State;
		internal FsmState State {
			get { return _State; }
			set {
				// string msg = string.Format("Switching state from {0} to {1}", _State, value);
				// Log(msg);
				// statusStrip1.Text = msg;
				_State = value;
			}
		}

//---------------------------------------------------------------------------------------

		public Form1() {
			InitializeComponent();

			State = FsmState.Login;
			webBrowser1.Navigate("https://www.sciamdigital.com/index.cfm?fa=Account.LoginUser");
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			var wc = new WebClient();
			var data = wc.DownloadData("http://www.lrs5.net");
			string s = ASCIIEncoding.ASCII.GetString(data);
			textBox1.Text = s;

			DoHtml(s);
		}

//---------------------------------------------------------------------------------------

		private void DoHtml(string s) {
			// var doc = new HtmlDocument();

			
		}

//---------------------------------------------------------------------------------------

		private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
#if true
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
#else
			switch (State) {
			case FsmState.Login:
				DoLogin();
				break;
			case FsmState.LoggedIn:
				break;
			case FsmState.CurrentIssue:
				break;
			case FsmState.SelectIssueWithinYear:
				break;
			case FsmState.SpecificIssue:
				break;
			case FsmState.Idle:
				break;
			default:
				break;
			}
#endif
		}

//---------------------------------------------------------------------------------------

		private void DoLogin() {
			var doc = webBrowser1.Document;
			var usernames = doc.All.GetElementsByName("USERNAME_CHAR");
			// if (usernames.Count == 0)
			// return;
			var username = usernames[0];
			usernames[0].InnerText = "lrs5@lrs5.net";
			var passwords = doc.All.GetElementsByName("PASSWORD_CHAR");
			passwords[0].InnerText = "lrs_5_Sciam";
			Application.DoEvents();

			var submits = doc.All.GetElementsByName("submit");
			// 		alt	"Submit Sign In"	string
			for (int i = 0; i < submits.Count; i++) {
				var x = submits[i];
				var y = x.DomElement; //  as mshtml.HTMLInputElement;+		DomElement	{mshtml.HTMLInputElementClass}	object {mshtml.HTMLInputElementClass}

			}
			// IsLoggedIn = true;
			State = FsmState.LoggedIn;
			submits[0].InvokeMember("Click");
		}
	}
}
