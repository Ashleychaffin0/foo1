using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xxxTest_____GuiBuilder {
	public partial class TestGuiBuilder : Form {
		public TestGuiBuilder() {
			InitializeComponent();

			object x = this;
			if (x == null) x = "goo";
			x = null;
			x ??= "hoo";
			Console.WriteLine(x);
        }

		private void Foo(int a, int b, string s, Dictionary<int, string> dict) {
			var ss = s.Substring(1, 7);
			int[] c = new int[20];
			if (a == 6) {
				Console.WriteLine(a);
			}
			Console.WriteLine(ss);
		}

		private void ButtonExit_Click(object sender, EventArgs e) {
            Application.Exit();
        }
    }
}
