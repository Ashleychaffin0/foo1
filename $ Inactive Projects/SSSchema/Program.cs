using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SSSchema {
	static class Program {
		/// <summary>
		/// The main entry point for the application.	System.Windows.Forms.dll	C:\Windows\assembly\GAC_MSIL\System.Windows.Forms\2.0.0.0__b77a5c561934e089\System.Windows.Forms.dll	Yes	No	Skipped loading symbols.		3	2.0.50727.1433 (REDBITS.050727-1400)	10/23/2007 10:43 PM	7AFD0000-7BC6C000	[7552] SSSchema.vshost.exe: Managed	

		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new SSSChema());
		}
	}
}
