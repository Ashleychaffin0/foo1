using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WaitForURL {
	public partial class WaitForURL : Form {
		public WaitForURL() {
			InitializeComponent();
		}

		private void btnGo_Click(object sender, EventArgs e) {
			web.Navigate(txtURL.Text);
		}

		private void web_Navigated(object sender, WebBrowserNavigatedEventArgs e) {
			MessageBox.Show("Got it!", "WaitForURL", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

		private void web_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e) {
			statMsg.Text = e.ToString();
		}
	}
}