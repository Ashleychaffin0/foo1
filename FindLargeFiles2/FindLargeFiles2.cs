using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FindLargeFiles2 {
	public partial class FindLargeFiles2 : Form {

		int     MinSize, MaxSize;
		public FindLargeFiles2() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void FindLargeFiles2_Load(object sender, EventArgs e) {
			cmbMinSize.SelectedIndex = 1;			// MB
			cmbMaxSize.SelectedIndex = 1;			// MB
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			// Error checking
			if (txtMaxSize.Text.Trim().Length == 0) {
				MaxSize = -1;
			}
			if ) {

			}
		}
	}
}
