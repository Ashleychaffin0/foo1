using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestRoslyn_1 {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();

			var xx = new foo();
		}
	}

	class foo(int x, float y) {

		public void goo() {
			Console.WriteLine(x);
		}
	}
}
