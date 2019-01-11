using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.Win32;

// TODO: Show all users, including All Users (HKLM). ??????????????????

namespace ShowEmailClients {
	public partial class ShowEmailClients : Form {
		static string MailKey = @"Software\Clients\Mail";

//---------------------------------------------------------------------------------------

		public ShowEmailClients() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void ShowEmailClients_Load(object sender, EventArgs e) {
			ProcessUser();
		}


//---------------------------------------------------------------------------------------

		private void ProcessUser() {
			lbClients.Items.Clear();
			ShowDefaultClient();
		}

//---------------------------------------------------------------------------------------

		// Try to get the value from HKCU. If that fails, get it from HKLM.
		// If that fails, tough.
		private void ShowDefaultClient() {
			object client = null;
			RegistryKey RegKey;

			RegKey = Registry.CurrentUser.OpenSubKey(MailKey);
			client = RegKey.GetValue("");			// Get default value from HKCU
			if (client == null) {					// No default in HKCU
				RegKey = Registry.LocalMachine.OpenSubKey(MailKey);
				client = RegKey.GetValue("");		// Get default value from HKLM
			}
			RegKey.Close();
			if (client == null) {			// No default from either hive
				lblDefaultEMailClient.Text = "No default email client specified.";
				return;
			}

			ShowClients((string)client);
		}

//---------------------------------------------------------------------------------------

		private void ShowClients(string client) {
			lblDefaultEMailClient.Text = client;
			RegistryKey RegKey = Registry.LocalMachine.OpenSubKey(MailKey);
			string[] MailNames = RegKey.GetSubKeyNames();
			foreach (string name in MailNames) {
				lbClients.Items.Add(name);
			}
			RegKey.Close();

			// Now highlight the default email client
			for (int i = 0; i < lbClients.Items.Count; i++) {
				if (lbClients.Items[i].ToString() == client) {
					lbClients.SelectedIndex = i;
					break;
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void lbClients_SelectedIndexChanged(object sender, EventArgs e) {
			string name = (string)lbClients.SelectedItem;
			string key = MailKey + @"\" + name + @"\Shell\Open\Command";
			RegistryKey RegKey = Registry.LocalMachine.OpenSubKey(key);
			object cmd = RegKey.GetValue("");
			string txt = (string)(cmd ?? "N/A");
			txtProgram.Text = txt;
		}

//---------------------------------------------------------------------------------------

		private void ShowEmailClients_Paint(object sender, PaintEventArgs e) {
			if (e.ClipRectangle.IsEmpty)
				return;
			Color From, To;
			From = Color.Red;
			To = Color.CornflowerBlue;
			LinearGradientBrush lgb = new LinearGradientBrush(e.ClipRectangle, From, To, 45f, true);
			e.Graphics.FillRectangle(lgb, e.ClipRectangle);
		}
	}

}
