// Copyright (c) 2020 by Larry Smith
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

// g:\OneDrive\$Dev\C#\ZuneGenerateClasses\ZuneGenerateClasses
// c:\Program Files (x86)\Microsoft Visual Studio\2019\Community\VC\Tools\MSVC\14.27.29110\bin\Hostx86\x86

namespace ZuneTest01 {
	class Program {
		static void Main(string[] args) {
			string BackupDir = @"G:\lrs\ZuneDbBackup";
			Directory.CreateDirectory(BackupDir);

			// Assembly.l

			var za = new ZuneUtils();

			Console.WriteLine($"Zune intstalled in: {ZuneUtils.ZuneInstallationDir}");
			Console.WriteLine($"Zune database in: {ZuneUtils.ZuneDbDir}");
			string ts = DateTime.Now.ToString("yyyy-MM-dd-HH.mm.ss");
			string dbname = $"ZuneStore-{ts}.sdf";
			string src = Path.Combine(ZuneUtils.ZuneDbDir, "ZuneStore.sdf");
			string target = Path.Combine(BackupDir, dbname);
			Console.WriteLine($"About to copy to: {target}");
			File.Copy(src, target);
			Console.WriteLine("Copy done");

			// Console.WriteLine("PATH before: " + Environment.GetEnvironmentVariable("PATH"));
			string path = Environment.GetEnvironmentVariable("PATH")!;
			string newpath = ZuneUtils.ZuneInstallationDir + ";" + path;
			Environment.SetEnvironmentVariable("PATH", newpath);
			// Console.WriteLine("PATH after: " + Environment.GetEnvironmentVariable("PATH"));


			var dlls = Directory.EnumerateFiles(ZuneUtils.ZuneInstallationDir, "*.dll");
			// TODO: Delete next line
			// dlls = new string[] { Path.Combine(ZuneUtils.ZuneInstallationDir, "ZuneShell.dll") };

			var AsmList = new List<Assembly>();

			foreach (string dll in dlls) {
#if false
				string name = "\"" + dll + "\"";
				ProcessStartInfo psi = new ProcessStartInfo("dumpbin.cmd", $"/exports {name} >> g:\\lrs\\foo.txt");

				var proc = new Process();
				proc.StartInfo = psi;
				proc.Start();
				proc.WaitForExit();
#endif
				try {
					// SampleAssembly = Assembly.LoadFrom("c:\\Sample.Assembly.dll");
					// Console.WriteLine($"About to try to load {name}");
					var asm2 = Assembly.ReflectionOnlyLoad(dll);
					var asm = Assembly.LoadFile(dll);
					AsmList.Add(asm);
					Console.WriteLine($"********* File '{dll}'loaded");
					// var types = asm.GetExportedTypes();
#if false
					var types = asm.GetTypes();
					foreach (var type in types) {
						Console.WriteLine($"\t{type.FullName}");
					}
					int i = 1;
#endif
				} catch (BadImageFormatException) {
					Console.WriteLine($"\tBadImageFormatException: {dll}");
					// Ignore
				} catch (FileLoadException) {
					Console.WriteLine($"\tFileLoadException: {dll}");
				} catch (FileNotFoundException) {
					Console.WriteLine($"\tFileNotFoundException: {dll}");
				} catch (ReflectionTypeLoadException ex) {
					Console.WriteLine($"\tReflectionTypeLoadException: {dll} -- {ex.Message}");
				}
			}
			Console.WriteLine("Done");
			Console.ReadLine();
		}
	}
}
