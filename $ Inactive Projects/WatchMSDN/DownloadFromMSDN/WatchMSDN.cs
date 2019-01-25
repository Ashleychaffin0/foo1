using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Media;

namespace DownloadFromMSDN {
	public partial class WatchMSDN : Form {

		//string	TargetURL = "https://msdn.one.microsoft.com/home.aspx?ApplicationID=4E9B12B3-3B2C-4697-9873-A8D8E5B96364&CultureCode=en-US&BenefitDetailGuid=81D9D769-CE8B-4F3F-A54F-B0867BE6A569&AccessGuid=D2111E84-53E8-4EA3-AFC6-A57AB429A684&SourceSystemCode=SMC&SubscriptionStatus=Active";
		string	TargetURL = "https://msdn.one.microsoft.com/home.aspx?ApplicationID=4E9B12B3-3B2C-4697-9873-A8D8E5B96364&CultureCode=en-US&BenefitDetailGuid=81D9D769-CE8B-4F3F-A54F-B0867BE6A569&AccessGuid=D2111E84-53E8-4EA3-AFC6-A57AB429A684&SourceSystemCode=SMC&SubscriptionStatus=Active";
		string	MusicDir, MusicFilename;
		int		PeekPeriod = 2 * 1000;

		Timer timer;

		SoundPlayer sp;

		public WatchMSDN() {
			InitializeComponent();

			this.WindowState = FormWindowState.Maximized;

			if (Environment.MachineName == "LRS-P4-1") {
				MusicDir = @"C:\Program Files\WS_FTP\";
			} else {
				MusicDir = @"E:\WS_FTP\";
			}
			MusicFilename = MusicDir + "connect.wav";
			sp = new SoundPlayer(MusicFilename);

			timer = new Timer();
			timer.Interval = 60 * PeekPeriod;
			timer.Tick += new EventHandler(timer_Tick);

			Clipboard.SetText("bartset1");
		}

		void timer_Tick(object sender, EventArgs e) {
			// btnGo_Click(btnGo, new EventArgs());	
			timer.Stop();
		}

		private void btnGo_Click(object sender, EventArgs e) {
			webMSDN.Navigate("https://msdn2.microsoft.com/en-US/subscriptions/manage/default.aspx");
		}

		private void webMSDN_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
			txtURL.Text = e.Url.ToString();
			Console.WriteLine("\n{0}\n", "".PadRight(80, '='));
			// Console.WriteLine("webMSDN_DocumentCompleted - Sender={0}", sender);
			Console.WriteLine("{0} webMSDN_DocumentCompleted - URL={1} - {2}", DateTime.Now, e.Url, "".PadRight(80, '*'));
			// Console.WriteLine("Doc.InnerHTML={0}", webMSDN.Document.Body.InnerHtml);
			// Console.WriteLine("Doc.InnerText={0}", webMSDN.Document.Body.InnerText);
			if (e.Url.ToString() == TargetURL) {
				sp.PlayLooping();
			} else {
				HtmlElementCollection links = webMSDN.Document.Links;
				foreach (HtmlElement elem in links) {
					if (elem.InnerText == "Subscriber Downloads and Product Keys") {
						elem.InvokeMember("Click");
						return;
					}
				}
				timer.Start();
			}
		}

		private void btnStop_Click(object sender, EventArgs e) {
			sp.Stop();
		}
	}
}