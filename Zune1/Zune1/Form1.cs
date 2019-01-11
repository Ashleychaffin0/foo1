using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using Microsoft.Win32;


// using ZuneUI;
//using MicrosoftZuneInterop;
using MicrosoftZuneLibrary;
using Microsoft.Zune.Shell;
//using Microsoft.Zune.
// using MicrosoftZunePlayback;
 

namespace Zune1 {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();

			// var pb = new MicrosoftZuneInterop.QueryPropertyBag();
			// MicrosoftZuneLibrary.ZuneLibraryCDRecorder rec = 
#if false
			int	nCols = 10;
			int [] colIndexes = new int[nCols];
			object [] fieldValues = new object[nCols];
			QueryPropertyBag	propBag = new QueryPropertyBag();
		object o = MicrosoftZuneLibrary.ZuneLibrary.GetFieldValues(1, EListType.eAlbumList,
			nCols, colIndexes, fieldValues, propBag);
#endif
		}

		private void button1_Click(object sender, EventArgs e) {
			var zl = new ZuneLibrary();
			bool dbRebuilt;
			int rc = zl.Initialize(GetZuneDirectory(), out dbRebuilt);
		}

		private void button2_Click(object sender, EventArgs e) {
			var q1 = from x in typeof(ZuneLibrary).GetMembers()
					 select x;
		}

		private void btnLaunchZuneApp_Click(object sender, EventArgs e) {
#if false
			// SyncEngineState s = SyncEngineState.sesInitial;
			// ZuneLibraryExports.StartupZuneNativeLib()
			Device dev = new Device();
			int n = dev.StartEnumeration();
			
			ZuneApplication app = new ZuneApplication();
#endif
		}

		string GetZuneDirectory() {
			RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Zune");
			string dir = (string)key.GetValue("Installation Directory");
			return dir;
		}
	}
}
