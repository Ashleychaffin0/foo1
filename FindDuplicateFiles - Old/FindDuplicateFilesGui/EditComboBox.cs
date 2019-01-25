// Copyright (c) 2010 by Larry Smith

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FindDuplicateFilesGui {
	public partial class EditComboBox : Form {

		ComboBox			cb;
		bool				IsDir;

		public List<string> items;

//---------------------------------------------------------------------------------------

		public EditComboBox(ComboBox cb, bool isDir, string caption) {
			InitializeComponent();

			this.cb = cb;
			this.IsDir = isDir;
			this.Text = caption;

			var titlebox = new TransparentPanel(this, lblTitle);

		}

//---------------------------------------------------------------------------------------

		private void EditComboBox_Load(object sender, EventArgs e) {
			if (this.Text.StartsWith("Edit -- ")) {
				lblTitle.Text = this.Text.Substring(8);
			} else {
				lblTitle.Text = this.Text;
			}

			items = new List<string>();
			foreach (var item in cb.Items) {
				string text = item.ToString();
				items.Add(text);
				lbSelections.Items.Add(text);
			}
		}

//---------------------------------------------------------------------------------------

		private void EditComboBox_Paint(object sender, PaintEventArgs e) {
			if (e.ClipRectangle.IsEmpty)
				return;
			Color From, To;
			From = Color.AliceBlue;
			To = Color.CornflowerBlue;
			using (var lgb = new LinearGradientBrush(e.ClipRectangle, From, To, 45f, true)) {
				e.Graphics.FillRectangle(lgb, e.ClipRectangle);
			}
		}

//---------------------------------------------------------------------------------------

		private void btnAdd_Click(object sender, EventArgs e) {
			if (IsDir) {
				// TODO:
			} else {
				// TODO:
			}
		}

//---------------------------------------------------------------------------------------

		private void btnDelete_Click(object sender, EventArgs e) {
			// TODO: A prompt, or better yet, an undo capability would be nice.
			if (lbSelections.Items.Count == 0) {
				MessageBox.Show("There are no items to delete.", this.Text,
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			for (int i = lbSelections.SelectedIndices.Count - 1; i >= 0; i--) {
				lbSelections.Items.RemoveAt(i);
			}
		}

//---------------------------------------------------------------------------------------

		private void btnUp_Click(object sender, EventArgs e) {
			MoveItemUpDown(true, "Can't move first item up.", 0);
		}

//---------------------------------------------------------------------------------------

		private void btnDown_Click(object sender, EventArgs e) {
			MoveItemUpDown(false, "Can't move last item down.", lbSelections.Items.Count - 1);
		}

//---------------------------------------------------------------------------------------

		private void MoveItemUpDown(bool bMoveUp, string CantMove, int KeyItem) {
			if (lbSelections.Items.Count == 0) {
				MessageBox.Show("There are no items to move.", this.Text,
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			// TODO: Support multiple selection at some point. Need MultiSelect on LB
			int n = lbSelections.SelectedIndex;
			if (n < 0) {
				MessageBox.Show("No item selected.", this.Text,
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (n == KeyItem) {
				MessageBox.Show(CantMove, this.Text,
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			var cur = lbSelections.Items[n];
			lbSelections.Items.RemoveAt(n);
			int ix;
			if (bMoveUp) {
				ix = n - 1;
				lbSelections.Items.Insert(n - 1, cur);
			} else {
				ix = n + 1;
				lbSelections.Items.Insert(n + 1, cur);
			}
			lbSelections.SelectedIndex = ix;
		}

//---------------------------------------------------------------------------------------

		private void EditComboBox_FormClosing(object sender, FormClosingEventArgs e) {
			if (e.Cancel) {
				return;
			}
			items.Clear();
			foreach (var item in lbSelections.Items) {
				items.Add(item.ToString());
			}
		}
	}
}
