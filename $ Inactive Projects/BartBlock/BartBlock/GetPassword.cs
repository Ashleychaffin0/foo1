using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Microsoft.Win32;

using Bartizan.Utils.CRC;

namespace BartBlock {
	public partial class GetPassword : Form {

		string		RegKey;

//---------------------------------------------------------------------------------------

		public GetPassword(string txt, string RegKey) {
			InitializeComponent();

			this.RegKey = RegKey;
			StringBuilder	sb = new StringBuilder();
			string	msg = @"You need to supply a password in order to ";
			msg += txt;
			msg += "\n\nPlease enter the password below and click OK, else click Cancel.";
			msg += "\n\nNote that the password is case-sensitive.";
			lblYouNeedAPasswordTo_.Text = msg;
		}

//---------------------------------------------------------------------------------------

		private void btnOK_Click(object sender, EventArgs e) {
			RegistryKey myKey;
			myKey = Registry.LocalMachine.OpenSubKey(RegKey);

			if (myKey == null)
				return;

			string pw = (string)myKey.GetValue("AdminPassword");
			if (pw == null)
				return;

			BartCRC	crc = new BartCRC();
			crc.AddData(txtPassword.Text);
			if (crc.GetCRC().ToString("X8") == pw) {
				this.DialogResult = DialogResult.Yes;			
			}
		}
	
	}
}