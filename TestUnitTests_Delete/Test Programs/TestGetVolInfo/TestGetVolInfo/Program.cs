using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;


namespace TestGetVolInfo {


	class Program {


		[DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
extern static bool GetVolumeInformation(
  string RootPathName,
  StringBuilder VolumeNameBuffer,
  int VolumeNameSize,
  out uint VolumeSerialNumber,
  out uint MaximumComponentLength,
  out uint FileSystemFlags,
  StringBuilder FileSystemNameBuffer,
  int nFileSystemNameSize);

		static void Main(string[] args) {
			bool b;
			int		VolumeNameSize;
			uint	VolumeSerialNumber, MaximumComponentLength, FileSystemFlags, nFileSystemNameSize;
			StringBuilder	vnb = new StringBuilder(256),
							fsnb = new StringBuilder(256);
			b = GetVolumeInformation("C:\\", vnb, 256, out VolumeSerialNumber, out MaximumComponentLength,
				out FileSystemFlags, fsnb, 256);
		}
	}
}
