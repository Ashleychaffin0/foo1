using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Zune2 {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
#if true
			string		FName = @"C:\Program Files\Zune";
			if (! System.IO.Directory.Exists(FName)) {
				MessageBox.Show("Can't find Zune directory");
			}
			System.IO.Directory.SetCurrentDirectory(FName);
#endif
			LRSZune2	f = new LRSZune2();
			Application.Run(f);
		}
	}
}