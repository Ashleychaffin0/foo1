// Copyright (c) 2009 by Larry Smith

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;

namespace XWord {
	public partial class frmXWord : Form {

		Puzzle puz;

		public frmXWord() {
			InitializeComponent();
        }

//---------------------------------------------------------------------------------------

		private void frmXWord_Load(object sender, EventArgs e) {
			XWordSquare.ParentPanel = XWordPanel;
			puz = new Puzzle(this, 15);
		}

//---------------------------------------------------------------------------------------

		private void savePuzzleAsToolStripMenuItem_Click(object sender, EventArgs e) {

		}

//---------------------------------------------------------------------------------------

		private void newPuzzleToolStripMenuItem_Click(object sender, EventArgs e) {

		}

//---------------------------------------------------------------------------------------

		private void XWordPanel_Resize(object sender, EventArgs e) {
			puz.ResizeBoard();
		}

//---------------------------------------------------------------------------------------

		private void XWordPanel_Paint(object sender, PaintEventArgs e) {
			
		}

//---------------------------------------------------------------------------------------

		private void XWordPanel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
			MessageBox.Show("Panel Key Down - " + Convert.ToChar(e.KeyValue));
		}
	}
}
