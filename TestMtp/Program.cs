using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using PortableDeviceApiLib;
using PortableDeviceTypesLib;

// using PortableDevices;
// using WpdMtpLib;

// Note: https://blogs.msdn.microsoft.com/dimeby8/2006/12/05/c-and-the-wpd-api/


namespace TestMtp {
	class Program { 
		static void Main(string[] args) {
			TestPortableDeviceApiLib();

			// TestPortableDevices();
			// TestMediaDevices();
			// TestWpdMtpLib();

			Console.WriteLine("\r\nPress a key to exit");
			Console.ReadKey();

			// Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
		}

//---------------------------------------------------------------------------------------

		private static void TestWpdMtpLib() {
			// var wpd = new WpdMtpLib.DeviceInfo()
		}

//---------------------------------------------------------------------------------------

		private static void TestMediaDevices() {
			// var md = MediaDevices.MediaFileSystemInfo;
		}

#if true
//---------------------------------------------------------------------------------------

		private static void TestPortableDeviceApiLib() {
			var pdm = new LrsPdm();		// Will call Dump()
		}
#endif

//---------------------------------------------------------------------------------------

#if false
		private static void TestPortableDevices() {
			// var asmTypes = Assembly.LoadFrom(@"G:\LRS\$Dev\C#\TestMtp\obj\Debug\Interop.PortableDeviceTypesLib.dll");
			// var asmApi = Assembly.LoadFrom(@"G:\LRS\$Dev\C#\TestMtp\obj\Debug\Interop.PortableDeviceApiLib.dll");
			var devices = new PortableDeviceCollection();
			devices.Refresh();
			foreach (var dev in devices) {
				dev.Connect();
				string fn = dev.FriendlyName;
				Console.WriteLine(fn);
				var root = dev.GetContents();
				foreach (var resource in root.Files) {
					DisplayResourceContents(resource);
					Console.WriteLine("============================");
				}
			}
			// var x1 = new PortableDevice("Galaxy Tab A - T550");
		}

//---------------------------------------------------------------------------------------

		public static void DisplayResourceContents(PortableDeviceObject portableDeviceObject) {
			Console.WriteLine(portableDeviceObject.Name);
			if (portableDeviceObject is PortableDeviceFolder) {
				DisplayFolderContents((PortableDeviceFolder)portableDeviceObject);
			}
		}

//---------------------------------------------------------------------------------------

		public static void DisplayFolderContents(PortableDeviceFolder folder) {
			foreach (var item in folder.Files) {
				Console.WriteLine(item.Name);

				if (item is PortableDeviceFolder) {
					DisplayFolderContents((PortableDeviceFolder)item);
				}
			}
		}

#endif
	}
}
