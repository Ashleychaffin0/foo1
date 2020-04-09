using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// TODO: We don't want to instantiate up to 21 * 21 Fonts that are the same.
//		 And maybe more for acrostics. So have some way of instantiating the
//		 font once, and have it used in all squares. But there's a problem.
//		 Normally I'd just make the Font a static property. But I can see the
//		 possibility that we might need instances of this control in different
//		 sizes (e.g. for screen display but also for printing). So we'll have
//		 come back to this later.

namespace TestXwordSquare {
	public partial class XWordSquare_UserControl : UserControl {

		int BorderSize = 0;
		string text;


		public XWordSquare_UserControl() {
			InitializeComponent();
		}

		// The text is displayed in the lower-right of the square (to leave 
		// room for the square ID (clue number) in the upper left.

		// There will be a small border inside the square, so the text doesn't
		// push all the way to the edge. It will have a minimum value of 1 
		// pixel.

		// For now, the border will be a constant 8 pixels. We'll see later
		// whether this should be constant (either 8 or something else), or
		// perhaps some percentage of the size of the square, or maybe
		// something else. TODO:

		// TODO:

		private void XWordSquare_UserControl_Paint(object sender, PaintEventArgs e) {
			var w = e.ClipRectangle.Width;
			var h = e.ClipRectangle.Height;

			this.BackColor = Color.Red;
			text = "y";
			
			Font main = new Font("Times New Roman", 100);
			var fgh = main.GetHeight(96);
			var fh = main.Height;
			var fs = main.Size;
			var fsip = main.SizeInPoints;
			var fst = main.Style;
			var fu = main.Unit;

			var g = e.Graphics;
			SizeF size = g.MeasureString(text, main);
			var x = w - (size.Width + BorderSize);
			var y = h - (size.Height + BorderSize);
			g.DrawRectangle(Pens.Black, e.ClipRectangle);
			Rectangle TextRect = new Rectangle((int)x, (int)y, (int)size.Width, (int)size.Height);
			g.DrawRectangle(Pens.Black, TextRect);
			g.DrawString(text, main, Brushes.Green, new PointF(x, y));
		}
	}
}
