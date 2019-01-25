using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Media;

namespace RefreshWebsitePeriodically {
	public partial class RefreshWebSitePeriodically : Form {
		public RefreshWebSitePeriodically() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void timer1_Tick(object sender, EventArgs e) {
			web.Navigate(txtURL.Text);
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			web.Navigate(txtURL.Text);
			timer1.Enabled = true;
		}

//---------------------------------------------------------------------------------------

		private void web_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
			HtmlDocument doc = web.Document;
			if (doc.Body.InnerText.Contains("No Appointments")) {
				return;
			}
			timer1.Enabled = false;
			SoundPlayer	snd = new SoundPlayer(@"C:\Windows\Media\Tada.wav");
			snd.Play();
		}
	}
}