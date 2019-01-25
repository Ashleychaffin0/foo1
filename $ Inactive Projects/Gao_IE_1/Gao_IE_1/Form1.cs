using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gao_IE_1 {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		private void btnGo_Click(object sender, EventArgs e) {
			web.Navigate(txtURL.Text);
		}

		private void btnBack_Click(object sender, EventArgs e) {
			web.GoBack();
		}

		private void web_Navigated(object sender, WebBrowserNavigatedEventArgs e) {
			MessageBox.Show("Navigated to " + e.Url);
		}
	}
}
