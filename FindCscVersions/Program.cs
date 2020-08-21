using System;
using System.Diagnostics;
using System.IO;

namespace FindCscVersions {
	class Program {
		static void Main(string[] args) {
			string outfile = @"G:\lrs\foo.txt";

			var opts = new EnumerationOptions();
			opts.IgnoreInaccessible = true;
			opts.RecurseSubdirectories = true;
			var cscs = Directory.EnumerateFiles("C:\\", "csc.exe", opts);

			Console.SetOut(new StreamWriter(outfile));

			foreach (var csc in cscs) {
				Directory.SetCurrentDirectory(Path.GetDirectoryName(csc));
				Run(csc);
				Console.WriteLine("\n*******************\n");
			}

			Console.Out.Close();
			Process.Start("Code.bat", outfile);
		}

//---------------------------------------------------------------------------------------

		private static void Run(string csc) {
			Console.Error.WriteLine($"Running {csc}");
			try {
				var si = new ProcessStartInfo() {
					FileName = csc,
					Arguments = "foo.cs",
					RedirectStandardOutput = true,
					RedirectStandardError  = true
				};
				var proc = new Process();
				proc.StartInfo = si;
				proc.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
				proc.ErrorDataReceived  += (sender, e) => Console.WriteLine(e.Data);

				proc.Start();
				proc.WaitForExit();
			} catch (Exception ex) {
				Console.Error.WriteLine($"\tError -- {ex.Message}");
			}
		}
	}
}
