using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Management;

using Bartizan.Utils;

namespace TestDongle_less {


	class Program {
		static void Main(string[] args) {
			MyTest me = new MyTest();
			me.Run();
		}
	}

	class MyTest {

		public void Run() {
			TimeSpan UpTime   = BartSysInfo.GetUptime();
			DateTime BootTime = BartSysInfo.GetBootTime();
			Console.WriteLine("The system has been up for {0}", UpTime);
			Console.WriteLine("The system was booted at   {0}", BootTime);

			BartNTP		ntp = new BartNTP();
			DateTime	TOD = ntp.GetNtpTod();
			Console.WriteLine("Current TOD is {0} (NTP)", TOD);
			Console.WriteLine("               {0} (Local time)", DateTime.Now);

#if true
			string[] drives = Environment.GetLogicalDrives();

			Bart_WMI_LogicalDisk	ld = new Bart_WMI_LogicalDisk();
			foreach (string drive in drives) {
				Console.WriteLine("\nProcessing drive {0}", drive);
				ld.GetDriveInfo(drive);
				Console.WriteLine("\tDrive type    = {0}", ld.GetDriveTypeName[0]);
				Console.WriteLine("\tFile System   = {0}", ld.FileSystem[0]);
				Console.WriteLine("\tProvider Name = {0}", ld.ProviderName[0]);
				Console.WriteLine("\tVolume Serial = {0}", ld.VolumeSerialNumber[0]);
				Console.WriteLine("\tDisk size     = {0:N0}", ulong.Parse(ld["Size"][0]));
			}
#endif

		}
	}
}
