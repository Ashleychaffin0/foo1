// Copyright (c) 2020 by Larry Smith
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp1 {
	class Program {
		static void Main(string[] args) {
			string TargetDir;
			if (args.Length == 0) {
				TargetDir = @"G:\OneDrive\$Dev\";
			} else {
				TargetDir = args[0];
			}

			var infos = new Dictionary<DateTime, string>();

			var dirs = Directory.EnumerateDirectories(TargetDir, "*", SearchOption.AllDirectories);
			foreach (string dir in dirs) {
				string[] slns = Directory.GetFiles(dir, "*.sln");
				if (slns.Length == 0) { continue; }
				if (slns.Length > 1) {
					Console.WriteLine($"\n******** WARNING: More than one .sln in {dir}. IGNORED");
					foreach (string sln2 in slns) {
						Console.WriteLine($"\t{sln2}");
					}
					continue;
				}
				string sln = slns[0];
				var info = new FileInfo(sln);
				string name = dir[TargetDir.Length..];
				// Console.WriteLine($"{name}, {info.LastWriteTime}");
				infos[info.LastWriteTime] = name;
			}
			var keys = from key in infos.Keys
					   orderby key descending
					   select new { key, fn = infos[key] };
			Console.WriteLine();
			foreach (var item in keys.Take(50)) {
				Console.WriteLine($"{item.key.ToShortDateString(),10}  {item.key.ToShortTimeString(),8}, {item.fn}");
			}
			int i = 1;
		}
	}
}
