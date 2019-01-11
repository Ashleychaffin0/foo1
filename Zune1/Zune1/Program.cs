using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Microsoft.Win32;

namespace Zune1 {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			try {
				InitZuneEnvironment();
				Application.Run(new Form1());
			} catch (System.IO.FileNotFoundException e) {
				string s = e.ToString();
				MessageBox.Show(s, "File Not Found Exception - LRS");
			} catch (Exception ex) {
				string s = ex.ToString();
				MessageBox.Show(s);
			}
		}

		private static void InitZuneEnvironment() {
			RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Zune");
			string dir = (string)key.GetValue("Installation Directory");
			if (dir == null) {
				MessageBox.Show("Can't find Zune directory. Do you have the software installed?");
				Application.Exit();
			}
			Environment.SetEnvironmentVariable("PATH",
				dir + ";" + Environment.GetEnvironmentVariable("PATH"));
		}

	}
}
