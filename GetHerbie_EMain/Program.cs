// Copyright (c) 2020 by Larry Smith
//

using System;
using System.Windows.Forms;

namespace nsGetHerbie_EMan {
	static class Program {
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new GetHerbie_EMan());
		}
	}
}
