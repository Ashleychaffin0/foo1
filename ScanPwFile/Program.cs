// Copyright (c) 2020 by Larry Smith
//

using System;
using System.IO;

namespace ScanPwFile {
	class Program {
		static void Main(string[] args) {
			string InfileName = @"G:\LRS\PWs.txt";
			string OutfileName = @"G:\LRS\Pwtest.txt";
			using var rdr = new StreamReader(InfileName);
			using var wtr = new StreamWriter(OutfileName);
			string line;
			int LineNum = 0;
			while (null != (line = rdr.ReadLine())) {
				++LineNum;
				string text = line.Trim();
				if (text.Length == 0) { continue; }
				string[] tokens = text.Split(' ', 2);
				// Console.WriteLine(tokens[0] + " -- " + tokens[1]);
				switch (tokens[0]) {
				case "User:":
					break;
				case "Category:":
					break;
				case "SiteName:":
					break;
				case "SiteUrl:":
					break;
				case "LogonID:":
					break;
				case "Password:":
					break;
				case "Comment:":
					break;
				default:
					Console.WriteLine($"Error on line {LineNum}");
					break;
				}
			}
		}
	}
}
