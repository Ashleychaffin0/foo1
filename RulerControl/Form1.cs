using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RulerControl {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();

#if false
			RulerOptions opts = new RulerOptions();
			RulerControl rc = new RulerControl(btnParentForRuler, opts);
			this.Controls.Add(rc);
#endif

#if true
            RulerControl rc2 = new RulerControl(listBox1);
			this.Controls.Add(rc2);
			listBox1.Items.Add("Now is the time for all good men");
			listBox1.Items.Add("for all good men to come to the aid of their party");
#endif

#if false
            RulerControl rc3 = new RulerControl(webBrowser1);
            this.Controls.Add(rc3);
            webBrowser1.Navigate("http://www.microsoft.com");
#endif
		}

		private void Form1_Paint(object sender, PaintEventArgs e) {

		}

		private void btnParentForRuler_Click(object sender, EventArgs e) {
			MessageBox.Show("Button clicked");
		}
	}
}
