using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LRSJAS_Zigzag_1 {

	// Notes:

	// This class created by right-clicking on LRSJAS_Zigzag_1 in the Solution Explorer
	// and doing an Add | Class [type = Windows Form] and giving it the name
	// AboutDialog.

	// The zigzag.gif embedded resource was added by finding a graphic on the web,
	// copying it to this directory, then, in the Solution Explorer, right-clicking
	// the project (LRSJAS_ZigZag_1), Add-ing an existing item, namely zigzag.gif.
	// Then, in the PictureBox control, click on its Image property and add it to the
	// list of embedded resources.
	//		Note: Clearly, the above isn't quite right. But I've never tried to embed a
	//			  graphic before.

//---------------------------------------------------------------------------------------

	public partial class AboutDialog : Form {
		public AboutDialog() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			linkLabel1.LinkVisited = true;
			System.Diagnostics.Process.Start("http://www.sanfransys.com");
		}
	}
}
