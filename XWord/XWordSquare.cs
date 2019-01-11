// Copyright (c) 2009 by Larry Smith

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XWord {
	class XWordSquare : Label {
		public static Panel ParentPanel;
		public static int	Order;

		int			row, col;
        int         WordNumber;         // The little number in the upper left corner

//---------------------------------------------------------------------------------------

		public XWordSquare() {
			Text = "X";					// If "", then black square // TODO: " ", not "X"
			row = 0;
			col = 0;
			CommonInit();
		}

//---------------------------------------------------------------------------------------

		public XWordSquare(string text, int row, int col) {
			this.Text	= text;
			this.row	= row;
			this.col	= col;
			CommonInit();
		}

//---------------------------------------------------------------------------------------

		private void CommonInit() {
			PlaceMyself();
		}

//---------------------------------------------------------------------------------------

		public static void ReLayout(Puzzle puz) {
			for (int row = 0; row < puz.Order; row++) {
				for (int col = 0; col < puz.Order; col++) {
					puz.Board[row, col].PlaceMyself(false);
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void PlaceMyself() {
			this.Click += new EventHandler(XWordSquare_Click);
			this.PreviewKeyDown += new PreviewKeyDownEventHandler(XWordSquare_PreviewKeyDown);
            this.Paint += new PaintEventHandler(XWordSquare_Paint);
			PlaceMyself(true);
		}

//---------------------------------------------------------------------------------------

        void XWordSquare_Paint(object sender, PaintEventArgs e) {
            int x = 1;              // TODO:
        }

//---------------------------------------------------------------------------------------

		void XWordSquare_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
			MessageBox.Show("Key Down - " + Convert.ToChar(e.KeyValue));
		}

//---------------------------------------------------------------------------------------

		void XWordSquare_Click(object sender, EventArgs e) {
			MessageBox.Show(string.Format("[{0}, {1}]", row, col));	// TODO:
		}

//---------------------------------------------------------------------------------------

		private void PlaceMyself(bool bAddToParentControls) {
			// OK, we know our row/col, and we know our ParentPanel. Place and size
			// ourselves
		
#if true	// TODO: Debug code to set the BackColor
			if (((row + col) % 2) == 0) {
				BackColor = Color.Red;
			} else {
				BackColor = Color.Beige;
			}
#endif

			this.BorderStyle = BorderStyle.FixedSingle;
			
			int w = ParentPanel.Width;
			int h = ParentPanel.Height;
			// TODO: Assume square pixels for now
			int size = Math.Min(w, h) / Order;
			this.Location = new Point(col * size, row * size);
			Location.Offset(ParentPanel.Left, ParentPanel.Top);
			this.Size = new Size(size, size);
			if (bAddToParentControls) {
				ParentPanel.Controls.Add(this);
			}
		}
	}
}
