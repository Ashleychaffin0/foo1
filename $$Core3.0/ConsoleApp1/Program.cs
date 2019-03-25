using System;
using System.IO;
using System.IO.Compression;

namespace ConsoleApp1 {
	class Program {
		static void Main(string[] args) {
			sse42 foo;
			Console.WriteLine("Hello World!"[1..^2]);
			var tmpout = Console.Out;
			Console.SetOut(new StreamWriter(@"G:\lrs\foo.txt"));
			int nDirs = 0;
			foreach (var dir in Directory.EnumerateDirectories("G:\\", "*", new EnumerationOptions { RecurseSubdirectories = true })) {
				if (0 == ((++nDirs) % 1_000)) { Console.Error.WriteLine($"{nDirs:N0} -- {dir}"); }
				// Console.WriteLine($"{++nDirs:N0}: {dir}");
				ProcessDir(dir);
			}

			Console.SetOut(tmpout);
			Console.WriteLine("Press any key to exit");
			Console.ReadKey();
		}

		private static void ProcessDir(string dir) {
			var opt = new EnumerationOptions { IgnoreInaccessible = true };
			foreach (var file in Directory.EnumerateFiles(dir, "*.zip", opt)) {
				using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read)) {
					try {
						var zip = new ZipArchive(fs);
						foreach (var entry in zip.Entries) {
							string name = entry.Name.ToLower();
							if (name.EndsWith(".dll")) {
								if (name.Contains("Portable")) {
									Console.WriteLine($"*** {file}[{name}]");
								}
							}
						}
					} catch (Exception ex) {
						Console.WriteLine($"??? {ex.Message} -- {file}");
					}
				}
			}
		}
	}
}
