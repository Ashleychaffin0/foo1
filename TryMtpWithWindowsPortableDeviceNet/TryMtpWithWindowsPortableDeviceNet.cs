using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsPortableDeviceNet;
using WindowsPortableDeviceNet.Model;

using PortableDeviceApiLib;
using System.Reflection;

namespace TryMtpWithWindowsPortableDeviceNet {
	public partial class TryMtpWithWindowsPortableDeviceNet : Form {
		public TryMtpWithWindowsPortableDeviceNet() {
			InitializeComponent();

			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

			var ute = new Utility();
			var x = ute.Get();
			Device dev = null;
			for (int i = 0; i < x.Count; i++) {
				dev = x[i];
				Console.WriteLine($"[{i}]: Name={dev.Name}");
				if (dev.Name.Value == "LRS G7") {
					break;
				}
			}

			foreach (var item in dev.DeviceItems) {
				if (item.Name.Value == "SD card") {
					DumpSdCardMusic(item);
				}
			}

		}

		private void DumpSdCardMusic(Item item) {
			foreach (var folder in item.DeviceItems) {
				if (folder.Name.Value == "Music") {
					DumpMusicFolder(folder);
				}
			}
			
		}

		private void DumpMusicFolder(Item folder) {
			foreach (var item in folder.DeviceItems) {
				DumpArtist(item);

			}
		}

		private void DumpArtist(Item item) {
			// Console.WriteLine($"{item.Name.Value}");
			DumpAlbums(item);
		}

		private void DumpAlbums(Item item) {
			foreach (var album in item.DeviceItems) {
				// Console.WriteLine($"\t{album.Name.Value}");
				DumpCuts(album.DeviceItems);
			}
		}

		private void DumpCuts(List<Item> deviceItems) {
			foreach (var item in deviceItems) {
				// Console.WriteLine($"\t\t{item.Name.Value}");
			}
		}

		private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args) {
			return Assembly.LoadFile(@"G:\LRS\$Dev\$$$$$ PortableDeviceApiLib.dll FIX\Modified\Interop.PortableDeviceApiLib.dll");
		}
	}
}
