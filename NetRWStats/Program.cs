using System;
using System.Net.NetworkInformation;

namespace NetRWStats {
	class Program {
		static void Main(string[] args) {
			long TotSent = 0, TotReceived = 0;
			long OneMB = 1000 * 1000; // 1024 * 1024;
			var ifs = NetworkInterface.GetAllNetworkInterfaces();
			foreach (var iface in ifs) {
				var stat = iface.GetIPv4Statistics();
				if ((stat.BytesReceived + stat.BytesSent) == 0)
					continue;
				TotReceived += stat.BytesReceived;
				TotSent += stat.BytesSent;
				long recvMB = (stat.BytesReceived + OneMB / 2) / OneMB;
				long sentMB = (stat.BytesSent + OneMB / 2) / OneMB;
				Console.WriteLine("Recv = {0:N0}MB, Sent = {1:N0}MB, Desc={2}", 
					recvMB, sentMB, iface.Description);
			}
		}
	}
}
