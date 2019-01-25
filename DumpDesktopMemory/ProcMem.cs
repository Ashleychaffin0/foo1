using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace LRS {
	public class ProcMem {
		public IntPtr hProc { get; set; }
//---------------------------------------------------------------------------------------

		public ProcMem(IntPtr hProc) {
			this.hProc = hProc;
		}

//---------------------------------------------------------------------------------------

		void Dump(IntPtr Address, int Length) {
			// TODO:
			// TODO: Maybe rename to Read?
		}

		public bool EnsureAdministrator() {
			if (IsAdministrator()) {
				return true;
			}
			// Not administrator. Restart
			var exeName = Process.GetCurrentProcess().MainModule.FileName;
			ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
			startInfo.Verb = "runas";
			startInfo.UseShellExecute = true;
			Process.Start(startInfo);
			return false;                   // User is supposed to exit
											// Note: We can't restart ourselves. This
											//       code, with pehaps some new using
											//       statements and a few different API
											//       calls could be called from WinForms,
											//       WPF, UWP, or maybe something else.
											//       IOW we don't know how to restart;
											//       it's our caller who knows this.
		}

//---------------------------------------------------------------------------------------

		public static bool IsAdministrator() {
			WindowsIdentity identity = WindowsIdentity.GetCurrent();
			WindowsPrincipal principal = new WindowsPrincipal(identity);
			return principal.IsInRole(WindowsBuiltInRole.Administrator);
		}

//---------------------------------------------------------------------------------------

		public void GiveOutselvesDebugPrivilege() {
			// TODO:
		}

//---------------------------------------------------------------------------------------

		private void Write() {
			// TODO: Placeholder. Just maybe, some day
		}
	}
}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

