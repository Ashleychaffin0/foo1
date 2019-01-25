using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pretext_NET_1 {
	public partial class Pretext_NET : Form {

		bool bAuto = true;

//---------------------------------------------------------------------------------------

		public Pretext_NET() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void Pretext_NET_SizeChanged(object sender, EventArgs e) {
			SetTopButtons();
			SetBottomButtons();

			// Note: I didn't bother taking into account the fact that the form needs to
			//		 be constrained to a minimum width to avoid the buttons overlapping
			//		 each other when the form is made too narrow.
		}

//---------------------------------------------------------------------------------------

		private void SetTopButtons() {
			int Margin = 80;							// Arbitrary, in pixels
			int Space = grpInput.Width - 2 * Margin;	// Left & Right margins
			Space -= btnBrowseInput.Width + btnBrowseOutput.Width;

			int Gutter = Space / 3;

			btnBrowseInput.Left  = grpInput.Left + Margin + Gutter;
			btnBrowseOutput.Left = btnBrowseInput.Right + Gutter;
		}

//---------------------------------------------------------------------------------------

		private void SetBottomButtons() {
			// We'll leave btnProcessFile where it is, aligned at design time with the
			// Progress group box above it.

			// We'll align the Quit button with the far right of the Progress group box.
			btnQuit.Left = grpProgress.Right - btnQuit.Width;

			// The About box will be centered with between the other two (give or take
			// a pixel because of rounding).
			int btnProcessRight  = btnProcessFile.Right;
			int SpareWidth       = btnQuit.Left - btnProcessRight;
			int Gutter           = (SpareWidth - btnAboutPretext.Width) / 2;
			btnAboutPretext.Left = btnProcessRight + Gutter;

		}

//---------------------------------------------------------------------------------------

		private void btnAuto_Click(object sender, EventArgs e) {
			if (bAuto) {
				bAuto = false;
				btnAuto.BackColor = System.Drawing.Color.Red;
			} else {
				bAuto = true;
				btnAuto.BackColor = System.Drawing.Color.LightGreen;
			}
			SetAnchors();
		}

//---------------------------------------------------------------------------------------

		private void SetAnchors() {
			AnchorStyles style;
			if (bAuto) {
				style = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			} else {
				style = AnchorStyles.Top | AnchorStyles.Left;
			}

			grpInput.Anchor		= style;
			grpOutput.Anchor	= style;
			grpProgress.Anchor	= style;

			txtInput.Anchor		= style;
			txtOutput.Anchor	= style;

			progressBar1.Anchor = style;
		}
	}
}
