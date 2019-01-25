using System;
using System.Windows.Forms;

namespace LRSLibraryBooks {
	public partial class PromptForString : Form {
		public string NewName;

//---------------------------------------------------------------------------------------

		public PromptForString(string LabelName) {
			InitializeComponent();

			lblName.Text = LabelName;
		}

//---------------------------------------------------------------------------------------

		private void btnOK_Click(object sender, EventArgs e) {
			NewName = txtName.Text;
		}

//---------------------------------------------------------------------------------------

		private void txtName_KeyPress(object sender, KeyPressEventArgs e) {
			if (e.KeyChar == '\r') {
				btnOK.PerformClick();
			}
		}
	}
}
