using System;
using System.IO;

namespace foo1 {
	class Program {
		static void Main(string[] args) {
			var EnvPath = Environment.GetEnvironmentVariable("PATH");
			int i = 0;
			foreach (var path in EnvPath.Split(';')) {
				Console.WriteLine($"[{i++}] {path}");
				if (!Directory.Exists(path)) {
					Console.WriteLine($"\t**** {path} does not exist");
				}
			}
		}
	}
}
