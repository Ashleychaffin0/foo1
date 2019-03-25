using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Microsoft.Win32;

// TODO: See list of valid queries at the end of this module.

namespace Zune3 {
	static class Program {

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		static extern int CreateHardLink(string lpFileName, string lpExistingFileName,
		   IntPtr lpSecurityAttributes);


		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
#if true

			RegistryKey zKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Zune");
			if (zKey == null) {
				MessageBox.Show("It looks like the Zune software isn't installed on this machine.");
				return;
			}
			string ZuneDirName = (string)zKey.GetValue("Installation Directory");
			zKey.Close();
			if (ZuneDirName == null) {
				MessageBox.Show("Unable to find Zune installation directory.");
				return;
			}

			if (!System.IO.Directory.Exists(ZuneDirName)) {
				MessageBox.Show("Can't find Zune directory.");
				return;
			}
			System.IO.Directory.SetCurrentDirectory(ZuneDirName);
			// var asm = Assembly.GetExecutingAssembly();
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
			string NewPath = ZuneDirName + ";" 
						   + ZuneDirName + @"\en-US;"
						   // Dunno if these next two are needed
						   // + FName + @"\Drivers;"
						   // + FName + @"\Network Sharing;"
				+ Environment.GetEnvironmentVariable("PATH");
			string[] paths = NewPath.Split(';');
			Environment.SetEnvironmentVariable("PATH", NewPath);

#if false
			string	ZuneDbDllName = "ZuneDb.dll";
			string	CurDir = Path.GetDirectoryName(Application.ExecutablePath);
			string	ZuneDbLinkName = Path.Combine(CurDir, ZuneDbDllName);
			string	RealZuneDbDllName = Path.Combine(ZuneDirName, ZuneDbDllName);
			int bRc = CreateHardLink(ZuneDbLinkName, RealZuneDbDllName, IntPtr.Zero);
			if (bRc == 0) {
				int rc = Marshal.GetLastWin32Error();
				MessageBox.Show("CreateHardLink failed, rc = " + rc);
			} else {
				MessageBox.Show("CreateHardLink succeeded");
			}
			if (!System.IO.File.Exists(ZuneDbLinkName)) {
				MessageBox.Show("Can't find ZuneDB.dll");
				return;
			}
#endif

#endif
			LRSZune3 f = new LRSZune3();
			Application.Run(f);
		}

		private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args) {
			throw new NotImplementedException();
		}
	}
}

#if false
  1: EQTYpe = eQueryTypeAllTracks / 0
	  1: smt = eQueryTypeAllTracks -- kiIndex_Abstract worked, Count=22332
	  2: smt = eQueryTypeAllTracks -- kiIndex_AcquiredContentRetrievalTransactionID worked, Count=22332

#endif
