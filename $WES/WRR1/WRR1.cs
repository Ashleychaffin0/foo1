using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WRR1 {
	public partial class WRR1 : Form {
		public WRR1() {
			InitializeComponent();
		}

		private void btnPushMe_Click(object sender, EventArgs e) {
			MessageBox.Show("Don't do that. It tickles!");
		}

		private void btnHello_Click(object sender, EventArgs e) {
			string txt = txtName.Text.Trim();
			if (txt.Length == 0) {
				MessageBox.Show("And whom do I have the pleasure of addressing?");
			} else {
				string Hiya = "Hello " + txt;
				MessageBox.Show(Hiya);
			}
		}
	}
}
