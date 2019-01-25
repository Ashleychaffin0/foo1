using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace Sudoku {
	/// <summary>
	/// Summary description for Sudoku.
	/// </summary>
	public class Sudoku : System.Windows.Forms.Form {
		private System.Windows.Forms.ToolTip toolTip1;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Panel TemplatePanel;
		private System.Windows.Forms.Button btnRecalc;
		private Button btnNewGame;

		Font		KnownFont = new Font("Arial", 24);
		Font		SmallFont = new Font("Arial", 8);
		
		Cell [,]	Board;

		// A Cell is a Panel, which doesn't support focus. So when we 
		// display a context menu, we can't see who had the focus, to be
		// able to tell what Cell to apply it to. So we'll fake it a bit here.
		// When the cell is clicked, we'll set this variable. The menu event
		// handler can find out whence it was clicked from here.
		Cell		FocusCell = null;

		// State variable -- Remember if we've left or right-clicked a cell.
		MouseButtons CurButton;

//---------------------------------------------------------------------------------------

		public Sudoku() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			Init();
			
			SetupInitialBoard(true);
		}

//---------------------------------------------------------------------------------------

		private void btnRecalc_Click(object sender, System.EventArgs e) {
			Recalc();
		}

//---------------------------------------------------------------------------------------

		private void Recalc() {
			// TODO: Another technique to be implemented later:
			//		 Suppose we have a Block with two cells, each of which has, say, the
			//		 values of {2, 6}. Then any other cell in that block with either a
			//		 2 or a 6 (or both) can have those values removed from it.
			bool	bFoundFinal;	// Did we find a "final" value?
			Cell	CurCell;
			do {
				bFoundFinal = false;
				for (int row = 0; row < 9; ++row) {
					for (int col = 0; col < 9; ++col) {
						CurCell = Board[row, col];
						if (CurCell.Given || CurCell.Values.Count == 1)
							continue;
						bFoundFinal |= RemoveKnown(CurCell, row, col, row, 0, row, 8);
						bFoundFinal |= RemoveSingletons(CurCell, row, col, row, 0, row, 8);

						bFoundFinal |= RemoveKnown(CurCell, row, col, 0, col, 8, col);
						bFoundFinal |= RemoveSingletons(CurCell, row, col, 0, col, 8, col);

						int		TTTRow = row / 3 * 3;
						int		TTTCol = col / 3 * 3;
						bFoundFinal |= RemoveKnown(CurCell, row, col, TTTRow, TTTCol, TTTRow + 2, TTTCol + 2);
						bFoundFinal |= RemoveSingletons(CurCell, row, col, TTTRow, TTTCol, TTTRow + 2, TTTCol + 2);
					}
				}
			} while (bFoundFinal);
			// TODO: Also check for singletons, namely a cell with Values.Length > 1,
			//		 but one of the Values is the only instance in the given rectangle
			//		 (row, col, TTT). For example, if a Cell in a row has Values 
			//		 {1, 2, 4}, and it's the only cell in the rectangle with a {2},
			//		 then this cell must contain a "2".
			//		 Implementation (very roughly). Create an array of length 9, 
			//		 initialized to 0's. Go through each Cell in the rect, and add 1 to
			//		 the counter for each Value. Then go through the array, looking for
			//		 a count of 1. For each, scan the rect for a Cell with Values.Count
			//		 > 1, with that value as one of its values. Make sure all singleton
			//		 values are processed; don't stop with the first one.
			// Note: This comment should be inside the do {...} while() loop above.

			InvalidateAllCells();
		}

//---------------------------------------------------------------------------------------

		private bool RemoveSingletons(Cell CurCell, int CurRow, int CurCol, int StartRow, 
							int StartCol, int EndRow, int EndCol) {
			Cell	TempCell;
			int[] Counts = new int[9 + 1];		// Will be init'ed to 0. Fine.
			for (int r = StartRow; r <= EndRow; ++r) {
				for (int c = StartCol; c <= EndCol; ++c) {
					TempCell = Board[r, c];
					foreach (int val in TempCell.Values) {
						++Counts[val];
					}
				}
			}
			// OK, we now know that, say, the value 4 occurs only once. Look
			// through CurCell and see if one of our values is 4. If so we can
			// set the value of that square to 4, and ignore all other possible
			// Values for that cell.
			foreach (int val in CurCell.Values) {
				if (Counts[val] == 1) {
					CurCell.SetValue(val);
					InvalidateAllCells();
					return true;
				}
			}
			return false;
		}

//---------------------------------------------------------------------------------------

		private bool RemoveKnown(Cell CurCell, int CurRow, int CurCol, 
						int StartRow, int StartCol, int EndRow, int EndCol) {
			// TODO: Replace these 6 int's by 3 Point's? Or maybe 1 Point and 1 Rect?
			Cell	TempCell;
			string	msg;
			for (int r = StartRow; r <= EndRow; ++r) {
				for (int c = StartCol; c <= EndCol; ++c) {
					if (CurRow == r && CurCol == c)
						continue;
					TempCell = Board[r, c];
					if (TempCell.Values.Count > 1)
						continue;			// Ignore cells we're not sure of
					// TODO: If we have an inconsistent board, TempCell.Values may 
					//		 have no entries, so the next line will blow
					CurCell.Values.Remove(TempCell.Values[0]);
					if (CurCell.Values.Count == 1) {
						if (! CurCell.Given && ! CurCell.MsgShown) {
							InvalidateAllCells();
							msg = string.Format("Firm value found. Board[{0}, {1}] must be {2}",
								CurRow + 1, CurCol + 1, CurCell.Values[0]);
							// TODO: Add reason
							// MessageBox.Show(msg, "Sudoku", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							CurCell.MsgShown = true;
						}
						return true;
					}
					if (CurCell.Values.Count == 0) {
						msg = string.Format("Internal inconsistency. Board[{0}, {1}] has no possibilities.",
							CurRow + 1, CurCol + 1);
						MessageBox.Show(msg, "Sudoku", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}
				}
			}
			return false;
		}

//---------------------------------------------------------------------------------------

		private void InvalidateAllCells() {
			for (int row = 0; row < 9; ++row) {
				for (int col = 0; col < 9; ++col) {
					Board[row, col].Invalidate();
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void Cell_Paint(object sender, System.Windows.Forms.PaintEventArgs e) {
			Cell		c = (Cell)sender;
			string		txt;
			Brush		brush;
			Font		font;

#if false
			if (c.Values.Count == 1) {
				txt	  = c.Values[0].ToString();
				font  = KnownFont;
				if (c.Given)
					brush = Brushes.Black;
				else
					brush = Brushes.Red;
			} else {
				txt = "{";
				for (int n=0; n<c.Values.Count; ++n) {
					if (n > 0)
						txt += ", ";
					txt += c.Values[n].ToString();
				}
				txt  += "}";
				font  = SmallFont;
				brush = Brushes.Blue;
			}
			// TODO: Figure out how to center
			// e.Graphics.DrawString(txt, font, brush, c.Bounds, new StringFormat(StringFormatFlags.FitBlackBox));
			e.Graphics.DrawString(txt, font, brush, new Rectangle(new Point(0, 0), c.Size));
#else
			if (c.Values.Count == 1) {
				txt = c.Values[0].ToString();
				font = KnownFont;
				if (c.Given)
					brush = Brushes.Black;
				else
					brush = Brushes.Red;
				// TODO: Figure out how to center
				// e.Graphics.DrawString(txt, font, brush, c.Bounds, new StringFormat(StringFormatFlags.FitBlackBox));
				e.Graphics.DrawString(txt, font, brush, new Rectangle(new Point(0, 0), c.Size));
			} else {
				// Size of sub-cell
				int w = c.Width / 3;
				int h = c.Height / 3;

				for (int n = 0; n < c.Values.Count; ++n) {
					int val = c.Values[n];
					// Figure out what sub-square this goes in
					int x = (val - 1) % 3 * w;
					int y = (val - 1) / 3 * h;
					Point pt = new Point(x, y);
					Size sz = new Size(w, h);
					Rectangle rect = new Rectangle(pt, sz);
					e.Graphics.DrawString(val.ToString(), SmallFont, Brushes.Blue, rect);
				}
			}
#endif
		}

//---------------------------------------------------------------------------------------

		private void Cell_Click(object sender, System.EventArgs e) {
			Cell		c = (Cell)sender;
			MouseEventArgs mea = e as MouseEventArgs;
			CurButton = mea.Button;
			FocusCell = c;
			MenuItem[] MenuItems = new MenuItem[c.Values.Count + 1];
			EventHandler eh = new EventHandler(CellContextMenu_Click);
			if (mea.Button == MouseButtons.Left) {
				MenuItems[0] = new MenuItem("Set", eh);
			} else {			// All other buttons (esp Right) are Remove ops
				MenuItems[0] = new MenuItem("Remove", eh);
			}
			for (int i = 1; i <= c.Values.Count; ++i)
				MenuItems[i] = new MenuItem(c.Values[i - 1].ToString(), eh);
			ContextMenu cMenu = new ContextMenu(MenuItems);
			cMenu.Show(c, new Point(c.Width / 2, c.Height / 2));
		}

//---------------------------------------------------------------------------------------

		private void CellContextMenu_Click(object sender, EventArgs e) {
			MenuItem	item = (MenuItem)sender;
			string	txt = item.Text;
			int num;
			if (!int.TryParse(txt, out num))		// i.e. if "Add" or "Remove"
				return;
			if (CurButton == MouseButtons.Left) {
				FocusCell.SetValue(num);
			} else {
				FocusCell.RemoveValue(num);
			}
			Recalc();
		}

//---------------------------------------------------------------------------------------

		private void Init() {
			int		row, col;
			// Board = new GameBoard();
			Board = new Cell[9, 9];

			// Now layout the cells
			int		w, h;					// Cell width and height
			w = TemplatePanel.Width;
			h = TemplatePanel.Height;
			int		basex, basey;			// Where template starts
			basex = TemplatePanel.Location.X;
			basey = TemplatePanel.Location.Y;
			for (row = 0; row < 9; ++row) {
				for (col = 0; col < 9; ++col) {
					Cell c = new Cell();
					int		x, y;
					x = basex + col * w;
					y = basey + row * h;
					c.Location = new Point(x, y);
					c.Size = TemplatePanel.Size;
					c.BorderStyle = BorderStyle.Fixed3D;
					c.Name = string.Format("{0}-{1}", row, col);
					c.Paint += new PaintEventHandler(Cell_Paint);
					c.Click += new System.EventHandler(Cell_Click);
					this.Controls.Add(c);
					Board[row, col] = c;
				}
			}

			// Color the 3x3 sub-boards
			ColorSubBoard(0, 3);
			ColorSubBoard(3, 0);
			ColorSubBoard(3, 6);
			ColorSubBoard(6, 3);
		}

//---------------------------------------------------------------------------------------

		private void ColorSubBoard(int r, int c) {
			for (int row=r; row<r+3; ++row) {
				for (int col=c; col<c+3; ++col) {
					Board[row, col].BackColor = Color.LightGray;
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void SetRow(int row, string vals) {
			// TODO: Really should do some error checking
			string	val;
			int		n = 0;
			for (int i=0; i<vals.Length; ++i) {
				val = vals.Substring(i, 1);
				if ((val == " ") || (val == "\t"))
					continue;
				if (val != "_")
					 Board[row, n].SetGiven(int.Parse(val));
				++n;
			}
		}

//---------------------------------------------------------------------------------------

		private void SetupInitialBoard(bool bSetGame) {
			if (bSetGame) {
				SetGame12();
			}
		}

//---------------------------------------------------------------------------------------

		private void SetGame12() {
			SetRow(0, "___   _23   47_");
			SetRow(1, "1_7   ___   ___");
			SetRow(2, "___   ___   ___");

			SetRow(3, "5__   71_   ___");
			SetRow(4, "_4_   ___   3__");
			SetRow(5, "___   63_   72_");

			SetRow(6, "___   5__   _17");
			SetRow(7, "_2_   ___   ___");
			SetRow(8, "___   ___   __8");
		}

//---------------------------------------------------------------------------------------

		private void SetGame11() {
			SetRow(0, "___   6__   _9_");
			SetRow(1, "___   _58   7__");
			SetRow(2, "__1   ___   ___");

			SetRow(3, "9_6   _8_   __1");
			SetRow(4, "5__   3__   _4_");
			SetRow(5, "__4   ___   _7_");

			SetRow(6, "68_   _7_   _5_");
			SetRow(7, "___   _6_   4__");
			SetRow(8, "__3   __9   _2_");
		}

//---------------------------------------------------------------------------------------

		private void SetGame10() {
			SetRow(0, "__1   __5   _39");
			SetRow(1, "___   _6_   __2");
			SetRow(2, "___   __2   ___");

			SetRow(3, "_8_   63_   5__");
			SetRow(4, "4__   ___   ___");
			SetRow(5, "562   ___   ___");

			SetRow(6, "___   87_   _5_");
			SetRow(7, "___   ___   21_");
			SetRow(8, "__3   ___   76_");
		}

//---------------------------------------------------------------------------------------

		private void SetGame9() {
			SetRow(0, "1__   _73   ___");
			SetRow(1, "_9_   4__   ___");
			SetRow(2, "___   _6_   __8");

			SetRow(3, "_2_   ___   _6_");
			SetRow(4, "___   _1_   ___");
			SetRow(5, "_54   ___   8_7");

			SetRow(6, "9__   _8_   _7_");
			SetRow(7, "_3_   ___   __9");
			SetRow(8, "__5   94_   2__");
		}

//---------------------------------------------------------------------------------------

		private void SetGame8() {
			SetRow(0, "___   _2_   __8");
			SetRow(1, "2_4   _6_   _17");
			SetRow(2, "__3   8__   2_4");

			SetRow(3, "7__   5__   ___");
			SetRow(4, "8__   __2   _91");
			SetRow(5, "6__   93_   7__");

			SetRow(6, "___   ___   _7_");
			SetRow(7, "3_6   7_5   4__");
			SetRow(8, "_72   ___   153");
		}

//---------------------------------------------------------------------------------------

		private void SetGame7()
		{
			SetRow(0, "_7_   14_   ___");
			SetRow(1, "6_2   ___   ___");
			SetRow(2, "_41   8_6   ___");

			SetRow(3, "8_7   ___   9__");
			SetRow(4, "2__   _1_   __5");
			SetRow(5, "__9   ___   1_7");

			SetRow(6, "___   5_3   28_");
			SetRow(7, "___   ___   6_4");
			SetRow(8, "___   _67   _5_");
		}

//---------------------------------------------------------------------------------------

		private void SetGame6()
		{
			SetRow(0, "_9_   1_7    _5_");
			SetRow(1, "_8_   4_3    _9_");
			SetRow(2, "5__   ___    __3");

			SetRow(3, "__3   5_8   1__");
			SetRow(4, "6__   ___   __8");
			SetRow(5, "__9   3_1   5__");

			SetRow(6, "1__   ___   __9");
			SetRow(7, "_7_   2_5   _4_");
			SetRow(8, "_6_   8_9   _1_");
		}

//---------------------------------------------------------------------------------------

		private void SetGame5()
		{
			SetRow(0, "___   6__    49_");
			SetRow(1, "_5_   7_4    6__");
			SetRow(2, "_8_   ___    __7");

			SetRow(3, "71_   ___   _3_");
			SetRow(4, "__3   ___   2__");
			SetRow(5, "_2_   ___   _74");

			SetRow(6, "8__   ___   _4_");
			SetRow(7, "__5   9_1   _8_");
			SetRow(8, "_94   __2   ___");
		}

//---------------------------------------------------------------------------------------

		private void SetGame4() {
			SetRow(0, "6_7   82_   ___");
			SetRow(1, "__2   _75   __6");
			SetRow(2, "___   __9   ___");

			SetRow(3, "5_3   ___   62_");
			SetRow(4, "29_   ___   _14");
			SetRow(5, "_14   ___   3_8");

			SetRow(6, "___   6__   ___");
			SetRow(7, "4__   53_   8__");
			SetRow(8, "___   _47   1_5");
		}

//---------------------------------------------------------------------------------------

		private void SetGame3() {
			SetRow(0, "___   7__   _3_");
			SetRow(1, "__4   __9   ___");
			SetRow(2, "15_   8__   __9");

			SetRow(3, "___   57_   _2_");
			SetRow(4, "6__   ___   __4");
			SetRow(5, "_7_   _18   ___");

			SetRow(6, "5__   __2   _61");
			SetRow(7, "___   3__   2__");
			SetRow(8, "_8_   __5   ___");
		}

//---------------------------------------------------------------------------------------

		private void SetGame2() {
			Set(0, 0, 6);
			Set(0, 2, 7);
			Set(0, 3, 8);
			Set(0, 4, 2);

			Set(1, 2, 2);
			Set(1, 4, 7);
			Set(1, 5, 5);
			Set(1, 8, 6);

			Set(2, 5, 9);

			Set(3, 0, 5);
			Set(3, 2, 3);
			Set(3, 6, 6);
			Set(3, 7, 2);

			Set(4, 0, 2);
			Set(4, 1, 9);
			Set(4, 7, 1);
			Set(4, 8, 4);

			Set(5, 1, 1);
			Set(5, 2, 4);
			Set(5, 6, 3);
			Set(5, 8, 8);

			Set(6, 3, 6);

			Set(7, 0, 4);
			Set(7, 3, 5);
			Set(7, 4, 3);
			Set(7, 6, 8);
			
			Set(8, 4, 4);
			Set(8, 5, 7);
			Set(8, 6, 1);
			Set(8, 8, 5);
		}

//---------------------------------------------------------------------------------------

		private void SetGame1() {
			Board[0, 1].SetGiven(1);
			Board[0, 2].SetGiven(2);
			Board[0, 6].SetGiven(3);

			Board[1, 0].SetGiven(4);
			Board[1, 3].SetGiven(5);
			Board[1, 4].SetGiven(2);
			Board[1, 6].SetGiven(6);

			Board[2, 0].SetGiven(7);
			Board[2, 3].SetGiven(3);
			Board[2, 4].SetGiven(8);
			Board[2, 6].SetGiven(9);

			Board[3, 1].SetGiven(6);
			Board[3, 3].SetGiven(1);
			Board[3, 5].SetGiven(2);
			Board[3, 7].SetGiven(7);

			Board[4, 0].SetGiven(3);
			Board[4, 1].SetGiven(2);
			Board[4, 7].SetGiven(1);
			Board[4, 8].SetGiven(6);

			Board[5, 1].SetGiven(7);
			Board[5, 3].SetGiven(9);
			Board[5, 5].SetGiven(5);
			Board[5, 7].SetGiven(3);

			Board[6, 2].SetGiven(7);
			Board[6, 4].SetGiven(1);
			Board[6, 5].SetGiven(3);
			Board[6, 8].SetGiven(5);

			Board[7, 2].SetGiven(5);
			Board[7, 4].SetGiven(4);
			Board[7, 5].SetGiven(6);
			Board[7, 8].SetGiven(8);

			Board[8, 2].SetGiven(1);
			Board[8, 6].SetGiven(7);
			Board[8, 7].SetGiven(2);
		}

//---------------------------------------------------------------------------------------

		private void Set(int row, int col, int val){
			Board[row, col].SetGiven(val);
		}

//---------------------------------------------------------------------------------------

		private void btnNewGame_Click(object sender, EventArgs e) {
			for (int row = 0; row < 9; row++) {
				for (int col = 0; col < 9; col++) {
					this.Controls.Remove(Board[row, col]);
				}
			}
			Init();
			InvalidateAllCells();
		}

//---------------------------------------------------------------------------------------

		#region Compiler generated stuff
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing) {
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.TemplatePanel = new System.Windows.Forms.Panel();
			this.btnRecalc = new System.Windows.Forms.Button();
			this.btnNewGame = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// TemplatePanel
			// 
			this.TemplatePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.TemplatePanel.Location = new System.Drawing.Point(72, 48);
			this.TemplatePanel.Name = "TemplatePanel";
			this.TemplatePanel.Size = new System.Drawing.Size(64, 56);
			this.TemplatePanel.TabIndex = 0;
			this.TemplatePanel.Visible = false;
			this.TemplatePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.Cell_Paint);
			// 
			// btnRecalc
			// 
			this.btnRecalc.Location = new System.Drawing.Point(16, 8);
			this.btnRecalc.Name = "btnRecalc";
			this.btnRecalc.Size = new System.Drawing.Size(96, 32);
			this.btnRecalc.TabIndex = 1;
			this.btnRecalc.Text = "Recalc";
			this.btnRecalc.Click += new System.EventHandler(this.btnRecalc_Click);
			// 
			// btnNewGame
			// 
			this.btnNewGame.Location = new System.Drawing.Point(140, 8);
			this.btnNewGame.Name = "btnNewGame";
			this.btnNewGame.Size = new System.Drawing.Size(96, 32);
			this.btnNewGame.TabIndex = 2;
			this.btnNewGame.Text = "New Game";
			this.btnNewGame.Click += new System.EventHandler(this.btnNewGame_Click);
			// 
			// Sudoku
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(736, 632);
			this.Controls.Add(this.btnNewGame);
			this.Controls.Add(this.btnRecalc);
			this.Controls.Add(this.TemplatePanel);
			this.Name = "Sudoku";
			this.Text = "Sudoku";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Sudoku());
		}
		#endregion
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class Cell : Panel {		// Individual square, contains 1 char (or list of candidates)
		public bool			Given;		// If part of game given at start
		public bool			MsgShown;	// When firm value found
		public List<int>	Values;		// If length==1, we're sure of it

//---------------------------------------------------------------------------------------

		public Cell() {
			BackColor = Color.White;
			Given = false;
			MsgShown = false;
			Values = new List<int>(9);
			SetDefaults();
		}

//---------------------------------------------------------------------------------------

		public void SetDefaults() {
			for (int i=1; i<=9; ++i) {
				Values.Add(i);
			}
		}

//---------------------------------------------------------------------------------------

		public void SetGiven(int val) {
			Given = true;
			Values.Clear();
			Values.Add(val);
		}

//---------------------------------------------------------------------------------------

		public void SetValue(int val) {
			Values.Clear();
			Values.Add(val);
		}

//---------------------------------------------------------------------------------------

		public void RemoveValue(int val) {
			Values.Remove(val);
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class GameBoard {			// TODO: Unused
		public Cell [,] Cells;

//---------------------------------------------------------------------------------------

		public GameBoard() {
			Cells = new Cell[9, 9];
		}
	}
}
