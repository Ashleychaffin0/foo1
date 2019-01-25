using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test30_1 {
	public partial class Test30_1 : Form {
		public Test30_1() {
			InitializeComponent();
		}

		private void buttonExit_Click(object sender, EventArgs e) {
			string s = "foo";
			if (string.IsNullOrEmpty(s)) { }
			Application.Exit();
		}
	}
}
