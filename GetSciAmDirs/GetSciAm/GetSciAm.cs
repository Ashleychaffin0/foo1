using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace GetSciAm {
	public partial class GetSciAm : Form {

		enum FsmState {
			Login,
			Unknown
		}

		FsmState State = FsmState.Login;

//---------------------------------------------------------------------------------------

		public GetSciAm() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void GetSciAm_Load(object sender, EventArgs e) {
			SetDefaultValues();

			ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
		}

//---------------------------------------------------------------------------------------

		private void SetDefaultValues() {
			// TODO: Get some/all of these from some kind of config file
			txtURL.Text = "https://www.sciamdigital.com/index.cfm?fa=Account.LoginUser";
			txtUserID.Text = "lrs5";
			txtPassword.Text = "lrs5_Sciam";

			// Set up Year combo box
			var now = DateTime.Now;
			int year = now.Year;	// January issue comes out in December
			for (int i = now.Year; i >= 1993; i--) {
				cmbYear.Items.Add(i);
			}
			cmbYear.SelectedIndex = 0;

			// Set up Month combo box
			// TODO: Make an intelligent choice based on <year.Month>
			cmbMonth.SelectedIndex = now.Month - 1;
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			webBrowser1.Navigate(txtURL.Text);
		}

//---------------------------------------------------------------------------------------

		private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
			var web = sender as WebBrowser;
			var doc = web.Document;
			switch (State) {
			case FsmState.Login:
				ProcessState_Login(doc);
				break;
			case FsmState.Unknown:
				break;
			default:
				break;
			}
		}

//---------------------------------------------------------------------------------------

		private void ProcessState_Login(HtmlDocument doc) {
			var usernames = doc.All.GetElementsByName("USERNAME_CHAR");
			var username = usernames[0];
			usernames[0].InnerText = txtUserID.Text;
			Console.WriteLine("usernames[0].InnerText = {0}", usernames[0].InnerText);
			var passwords = doc.All.GetElementsByName("PASSWORD_CHAR");
			passwords[0].InnerText = txtPassword.Text;
			Console.WriteLine("passwords[0].InnerText = {0}", passwords[0].InnerText);
			Application.DoEvents();

			var submits = doc.All.GetElementsByName("submit");
			submits[0].InvokeMember("Click");
			State = FsmState.Unknown;
		}

//---------------------------------------------------------------------------------------

		public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { 
			return true; 
		} 
	}
}
