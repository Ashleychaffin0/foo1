using System;
using System.Collections.Generic;
using System.Text;

using System.ServiceProcess;

namespace StartStopSQLServer {
	class Program {
		static void Main(string[] args) {
			ServiceController	sc = new ServiceController("SQL Server (MSSQLSERVER)");
			Console.WriteLine("Initial SQL Server (MSSQLSERVER) status is {0}.", sc.Status);
			if (sc.Status == ServiceControllerStatus.Running) {
				sc.Stop();
			} else {
				sc.Start();
			}
			Console.WriteLine("Final SQL Server (MSSQLSERVER) status is {0}.", sc.Status);
		}
	}
}
