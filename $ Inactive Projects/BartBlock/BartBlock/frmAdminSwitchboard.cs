using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BartBlockForms {
	internal partial class frmAdminSwitchboard : Form {
		public frmAdminSwitchboard() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void btnChangeAdminPassword_Click(object sender, EventArgs e) {
			frmChangeAdminPassword frm = new frmChangeAdminPassword();
			DialogResult res = frm.ShowDialog();
			if (res == DialogResult.OK) {
				MessageBox.Show("Nonce: Passwords match", "Security", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

//---------------------------------------------------------------------------------------

		private void btnEditAllowedSitesList_Click(object sender, EventArgs e) {
			frmEditAllowedSites frm = new frmEditAllowedSites();
			DialogResult res = frm.ShowDialog();
		}
	}
}