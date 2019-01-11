using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LRSS {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
            LRSS pgm = new LRSS();
            if (!pgm.bInitOK)
                return;
			Application.Run(pgm);
		}
	}
}