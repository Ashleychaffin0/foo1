using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Microsoft.Win32;

using BartBlock;
using Bartizan.Utils.CRC;

namespace BartBlockForms {
	internal partial class frmChangeAdminPassword : Form {
		public frmChangeAdminPassword() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void btnOK_Click(object sender, EventArgs e) {
			if (txtNewPassword.Text == txtNewPassword2.Text) {
				if (SetPassword()) {
					this.DialogResult = DialogResult.OK;
					this.Close();
				}
			return;
			}

			MessageBox.Show("Passwords don't match. Please correct, or click Cancel.", "Security", 
				MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

//---------------------------------------------------------------------------------------

		private bool SetPassword() {
			string	msg;

			try {

				RegistryKey myKey;
				myKey = Registry.LocalMachine.OpenSubKey(BartBlock.BartBlock.RegKey);

				if (myKey == null) {
					msg = "An unexpected error occurred accessing the current password."
					+ " The password was not updated.";
					MessageBox.Show(msg, "Security", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

				BartCRC crc = new BartCRC();
				crc.AddData(txtNewPassword.Text);
				string data = crc.GetCRC().ToString("X8");
				myKey.SetValue("AdminPassword", data);

				return true;

			} catch (Exception ex) {
				msg = "An unexpected error ({0}) occurred accessing the current password."
				+ " The password was not updated.";
				msg = string.Format(msg, ex.Message);
				MessageBox.Show(msg, "Security", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}
	}
}