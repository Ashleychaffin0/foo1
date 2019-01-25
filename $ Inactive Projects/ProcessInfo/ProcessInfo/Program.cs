using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ProcessInfo {

	class ProcessInfo {
		public void Run() {
			Process [] ps = Process.GetProcesses();

			string	fmt = "'{0}','{1:N0}','{2}','{3}','{4:N0}','{5:N0}','{6:N0}'";
			fmt = fmt.Replace("'", "\"");

			Console.WriteLine("Process Name, Working Set, Main Module, WindowTitle, Nonpaged System Memory Size, Paged Memory Size, Paged System Memory Size");

			foreach (Process p in ps) {
				try {
					Console.WriteLine(fmt, p.ProcessName, p.WorkingSet, p.MainModule.FileName, p.MainWindowTitle, p.NonpagedSystemMemorySize, p.PagedMemorySize, p.PagedSystemMemorySize);
				} catch (Exception ex) {
					Console.WriteLine("*** Can't access data for PID={0}, Name={1}", p.Id, p.ProcessName);
				}
			}
			System.Console.Error.Write("Press any key to exit");
			Console.ReadKey(true);
		}
	}

	class Program {
		static void Main(string[] args) {
			ProcessInfo	pi = new ProcessInfo();
			pi.Run();
		}
	}
}
