using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ZuneGenerateClasses {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			try {
				string path = Environment.GetEnvironmentVariable("PATH");
				path = @"C:\Program Files\Zune;" + path;
				// Environment.SetEnvironmentVariable("PATH", path);
				Application.Run(new ZuneGenerateClasses());
			} catch (Exception ex) {
				string msg = ex.Message;
			}
		}
	}
}
