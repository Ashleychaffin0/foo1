using System;
using System.Collections.Generic;
using System.Text;

namespace ShowEnvData {
	class Program {
		static void Main(string[] args) {
			Console.WriteLine("Machine Name = {0}", Environment.MachineName);
			Console.WriteLine("OS Name      = {0}", Environment.OSVersion.ToString());
			System.IO.Directory.CreateDirectory("C:\\" + "foo");
		}
	}
}
