using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace TestSplitter {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e) {
			Panel pan = sender as Panel;
			if (pan.DisplayRectangle.IsEmpty) {
				return;
			}
			var lgb = new LinearGradientBrush(this.DisplayRectangle,
				Color.Red, Color.Blue, 45.0f);
			e.Graphics.FillRectangle(lgb, pan.DisplayRectangle);
		}
	}
}
