using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SaveIeStatus {
	public partial class SaveIeStatus : Form {
		public SaveIeStatus() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			var qProc = from p in Process.GetProcesses()
						where p.ProcessName == "iexplore"
						select p;
			Console.WriteLine("------------");
			foreach (Process p in qProc) {
				Console.WriteLine("PID={0}, HWND={1}, Title={2}", p.Id, p.MainWindowHandle, p.MainWindowTitle);
				var x = p.Handle;
			}
		}
	}
}
