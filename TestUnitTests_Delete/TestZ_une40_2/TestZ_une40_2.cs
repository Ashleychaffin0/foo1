using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.Win32;
using System.Reflection;

namespace TestZ_une40_2 {
	public partial class TestZ_une40_2 : Form {
		public TestZ_une40_2() {
			InitializeComponent();
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
			// System.IO.Directory.SetCurrentDirectory(ZuneDirName);
			string NewPath = ZuneDirName + ";"
						   + ZuneDirName + @"\en-US;"
				// Dunno if these next two are needed
				// + FName + @"\Drivers;"
				// + FName + @"\Network Sharing;"
				+ Environment.GetEnvironmentVariable("PATH");
			// string[] paths = NewPath.Split(';');
			Environment.SetEnvironmentVariable("PATH", NewPath);
			Directory.SetCurrentDirectory(ZuneDirName);
#if true
			var asm = Assembly.LoadFile(Path.Combine(ZuneDirName, "ZuneDBApi.dll"));
#endif

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

			var mzl = new MyTestZ_une(ZuneDirName);
			System.Diagnostics.Debugger.Break();
		}
	}
}
