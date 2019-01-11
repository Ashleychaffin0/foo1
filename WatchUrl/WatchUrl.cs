// TODO: 

//	*	Play music (repeatedly) when changed.

//	*	Need reset, if changed version isn't what we want

//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace WatchUrl {
	public partial class WatchUrl : Form {

		string BaseText = null;

//---------------------------------------------------------------------------------------

		public WatchUrl() {
			InitializeComponent();

			// txtBoxUrl.Text = "http://books.wpl.ca/search~S3/tmoon/tmoon;m=d/1,10,,B/frameset&FF=tmoon;m=d&3,3,";
			// txtBoxUrl.Text = "http://www.refdesk.com/";
			txtBoxUrl.Text = "http://news.google.com/";

			var wc = new WebClient();
			BaseText = wc.DownloadString(txtBoxUrl.Text);
		}

//---------------------------------------------------------------------------------------

		private void btnStart_Click(object sender, EventArgs e) {
			timer1.Interval = 60 * 1000 * int.Parse(maskedTextBox1.Text);
			timer1.Start();
		}

//---------------------------------------------------------------------------------------

		private void timer1_Tick(object sender, EventArgs e) {
			string msg = string.Format("Timer tick - now {0:t}", DateTime.Now);
			Console.WriteLine(msg);
			using (var wc = new WebClient()) {
				try {
					string s = wc.DownloadString(txtBoxUrl.Text);
					if (BaseText != s) {
						// TODO: Play music, repeatedly (or something)
						SystemSounds.Exclamation.Play();
						Console.WriteLine("Hey, something's changed!");
						MessageBox.Show("Hey, something's changed!");
						Application.DoEvents();
						timer1.Stop();
					}
				} catch (Exception ex) {
					SystemSounds.Question.Play();
					Console.WriteLine("Exception - msg = {0}", ex.Message);
					// Ignore any errors
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void btnStop_Click(object sender, EventArgs e) {
			timer1.Stop();
		}
	}
}
