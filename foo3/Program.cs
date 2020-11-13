using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace foo3 {
	class Program {
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		static extern bool GetVolumeInformation(string Volume, StringBuilder VolumeName,
	uint VolumeNameSize, out uint SerialNumber, out uint SerialNumberLength,
	out uint flags, StringBuilder fs, uint fs_size);

//---------------------------------------------------------------------------------------

		static void Main(string[] args) {
			Console.WriteLine(ZuneDbDir);
			Console.WriteLine(ZuneInstallationDir);
			Console.WriteLine("Done");
			var VolumeName = new StringBuilder(256);
			var fs = new StringBuilder(256);
			uint VolumeNameSize = 0;
			uint fs_size = (uint)fs.Capacity - 1;
			bool bOK = GetVolumeInformation("SSD-8920", VolumeName, VolumeNameSize, out uint SerialNumber, out uint SerialNumberLength, out uint flags, fs, fs_size);
			DriveInfo di = new DriveInfo("C:\\");
			Console.ReadLine();
		}

//---------------------------------------------------------------------------------------

		public static string ZuneDbDir {
			get {
				string user = Environment.GetEnvironmentVariable("USERPROFILE");
				return Path.Combine(user, @"Appdata\Local\Microsoft\Zune");
			}
		}

//---------------------------------------------------------------------------------------

		public static string ZuneInstallationDir {
			get {
				string ZuneNode = @"SOFTWARE\Microsoft\Zune";
				// ZuneReg = @"SOFTWARE\Microsoft";		// TODO: Delete
				RegistryKey key = Registry.LocalMachine.OpenSubKey(ZuneNode, RegistryKeyPermissionCheck.ReadSubTree);
				// RegistryKey ZuneKey = key.OpenSubKey(ZuneReg, false);
				return (string)key.GetValue("Installation Directory");
			}
		}
	}
}
