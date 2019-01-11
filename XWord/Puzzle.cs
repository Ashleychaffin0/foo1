// Copyright (c) 2009 by Larry Smith



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace XWord {
	class Puzzle {
		public int				Order;				// e.g. 15 for 15x15, etc
		public XWordSquare[,]	Board;				// Well, not quite "Board", but close enough

//---------------------------------------------------------------------------------------

		public Puzzle(Form form, int Order) {
			this.Order = Order;
			XWordSquare.Order = Order;
			Board = new XWordSquare[Order, Order];
			for (int row = 0; row < Order; row++)	{
				for (int col = 0; col < Order; col++) {
					//Board[row, col] = new XWordSquare(" ", row, col);
					Board[row, col] = new XWordSquare(
						string.Format("({0},{1})", row, col), row, col);
				}
			}
		}

//---------------------------------------------------------------------------------------

		public void LoadEmptyPuzzle() {		// aka "Clear Board"
		}

//---------------------------------------------------------------------------------------

		public void ResizeBoard() {
			XWordSquare.ReLayout(this);
		}
	}
}
