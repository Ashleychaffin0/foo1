using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gao1 {
	class Program {
		static void Main(string[] args) {
			var Procs = System.Diagnostics.Process.GetProcesses();
			foreach (var p in Procs) {
				Console.WriteLine("Name = {0}, WS64 = {1}", p.ProcessName, p.WorkingSet64);
			}

			var q1 = from proc in Procs
					 where proc.Id > 0 && proc.WorkingSet64 > 1024 * 1024 * 200
					 select new { proc.ProcessName, proc.WorkingSet64 };

			var q2 = Procs
						.Where(proc => proc.WorkingSet64 > 1024 * 1024 * 200)
						.Select(p => new { p.ProcessName, p.WorkingSet64 });

			foreach (var p in q1) {
				try {
					Console.WriteLine("Name = {0}, WS = {1}", p.ProcessName, p.WorkingSet64);
				} catch {
					// Ignore any secure processes that give us trouble
				}
			}

			Console.WriteLine();

			foreach (var p in q2) {
				Console.WriteLine("Name = {0}, WS = {1}", p.ProcessName, p.WorkingSet64);
			}
		}
	}
}
