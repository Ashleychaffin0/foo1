using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LRSNativeMethodsNamespace;

// Added COM references: 
//		PortableDeviceApi 1.0 Type Library
//		PortableDeviceTypes 1.0 Type Library
// Did NOT add (maybe later once I understand better what's going on):
//		PortableDeviceClassExtension 1.0 Type Library
//		PortableDeviceConnectApi 1.0 Type Library

// Web pages that might be useful:
//	https://stackoverflow.com/questions/6553290/how-do-i-create-an-instance-of-iportabledevicemanager
//	https://docs.microsoft.com/en-us/windows/desktop/api/portabledeviceapi/nn-portabledeviceapi-iportabledevicemanager
//	https://docs.microsoft.com/en-us/windows/desktop/wpd_sdk/transferring-content-from-the-device-to-a-pc
//	https://bitbucket.org/derekwilson/podcastutilities/src/b18a9926c1dcbfb884b34b9865ebaec96abfdb82/PodcastUtilities.PortableDevices/?at=default
//	https://cgeers.wordpress.com/2011/08/13/wpd-transferring-content/
//	https://docs.microsoft.com/en-us/windows/desktop/wpd_sdk/enumerating-devices

// For WMI providers (q.v.), see (among others) https://docs.microsoft.com/en-us/windows/desktop/CIMWin32Prov/win32-diskdrive
// Better yet, https://docs.microsoft.com/en-us/windows/desktop/CIMWin32Prov/computer-system-hardware-classes
//		https://docs.microsoft.com/en-us/previous-versions//aa394084(v=vs.85)
//		https://www.codeproject.com/Articles/18268/How-To-Almost-Everything-In-WMI-via-C-Part-Hardw

namespace TestMtp2 {
	public partial class TestMtp2 : Form {
		public TestMtp2() {
			InitializeComponent();

			var devMgr = new PortableDeviceApiLib.PortableDeviceManager();
			devMgr.GetDevices();

			string qry;
			// string qry = $"SELECT Name, ProcessID, CommandLine FROM Win32_Process WHERE Name LIKE '%Edge%'";
			// string qry = $"SELECT * FROM Win32_Process WHERE Name LIKE '%Edge%'";
			// string qry = $"SELECT * FROM Win32_DiskDrive";
			// qry = $"SELECT * FROM Win32_PhysicalMedia";
			qry = $"SELECT * FROM Win32_PNPDevice";
			qry = $"SELECT * FROM Win32_PhysicalMemory";
			qry = $"SELECT * FROM Win32_Printer";
			qry = $"SELECT * FROM MDM_GenericAppConfiguration";
			qry = $"SELECT * FROM win32_pnpentity";
			qry = $"SELECT * FROM win32_pnpentity where description like '%Bluetooth%'";
			var path = new ManagementPath(@"\root\cimv2");
			var opts = new ConnectionOptions {
				Timeout = ConnectionOptions.InfiniteTimeout
			};
			var ms = new ManagementScope(@"\root\cimv2", opts);
			var enumopts = new EnumerationOptions { Timeout = EnumerationOptions.InfiniteTimeout };
			var xxx = new ManagementObjectSearcher(@"\root\cimv2", qry, enumopts);
			var yyy = xxx.Get();
			Console.WriteLine($"# entries = {yyy.Count}");
			foreach (var item in xxx.Get()) {
				// DumpProcessInfo(item);
				// DumpDiskInfo();
				Console.WriteLine("--------------------------");
				foreach (var prop in item.Properties) {
					Console.WriteLine($"\t{prop.Name} = {prop.Value}");
					if (prop.Name == "CompatibleID") {
						string[] ids = prop.Value as string[];
						if (ids is null) { continue; }
						foreach (var id in ids) {
							Console.WriteLine($"\t\t{id}");
						}
					}
				}
				// Console.WriteLine(item["Description"]);
			}
		}

		private static void DumpProcessInfo(ManagementBaseObject item) {
			Console.WriteLine($"{item["Name"]}, {item["ProcessID"]}, {item["CommandLine"]}");
			int hWnd = Convert.ToInt32(item["Handle"]);
			var sb = new StringBuilder(1000);
			LRSNativeMethods.GetWindowText(new IntPtr(hWnd), sb, sb.Capacity);
		}
	}
}
