using System;
using System.IO;
using System.Diagnostics;

namespace ConsoleRedirect
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			if (args.Length == 0) {
				Console.WriteLine("Usage: ConsoleRedirect commandline. Output will go to stdout.txt");
				return;
			}
			TextWriter	stdout = new StreamWriter("stdout.txt");

			ProcessStartInfo	psi = new ProcessStartInfo();
			psi.FileName = args[0];
			psi.RedirectStandardOutput = true;
			psi.UseShellExecute = false;
			Process p = Process.Start(psi);
			StreamReader srstdout = p.StandardOutput;
			string	s;
			s = srstdout.ReadToEnd();
			stdout.WriteLine(s);
			stdout.Close();
			// Console.WriteLine("stdout len = {0}, value = {1}", s.Length, s);
			p.WaitForExit();
		}
	}
}
