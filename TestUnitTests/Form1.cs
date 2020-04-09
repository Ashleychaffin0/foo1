using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestUnitTests {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();

			var p = new Point(3, 4);
			var dist = p.Distance();
			Debug.WriteLine($"Distance = {dist}");
		}
	}

	class Point {
		int x, y;

		public Point(int x, int y) {
			this.x = x;
			this.y = y;
		}

		public double Distance() => Math.Sqrt(x * x + y * y);
	}
}
