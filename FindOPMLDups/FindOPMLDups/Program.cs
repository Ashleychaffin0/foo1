using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FindOPMLDups {
	class Program {
		static int Main(string[] args) {
#if DEBUG
			string	InFile = @"C:\LRS\C#\LRSS\LRSS\SharpReader - 2006-06-19.opml";
			string	OutFile = "foo.txt";
#else
			if (args.Length != 2) {
				Console.WriteLine("Usage: FindOPMLDups File.opml outfile.txt");
				return 1;
			}
			string	InFile = args[0];
			string	OutFile = args[1];
#endif
			Dictionary<string, List<int>>	dict = new Dictionary<string,List<int>>();
			if (! File.Exists(InFile)) {
				Console.WriteLine("Input file does not exist - {0}", InFile);
				return 2;
			}
			try {
				StreamReader	sysin  = new StreamReader(InFile);
				StreamWriter	sysout = new StreamWriter(OutFile);
				string			FullText = sysin.ReadToEnd();
				sysin.BaseStream.Position = 0;		// Rewind
				Process(sysin, sysout, dict, FullText.Split(new string [] {"\r\n"}, StringSplitOptions.None));
			} catch (Exception ex) {
				Console.WriteLine("Unpexpected error. Program will terminate - {0}", ex.Message);
				return 3;
			}
			return 0;
		}

//---------------------------------------------------------------------------------------

		private static void Process(StreamReader sysin, StreamWriter sysout, 
					Dictionary<string, List<int>> dict, string [] FullText) {
			int		lineNo = 0;
			string	line;
			string	restring = @"title=""(?<title>).+?\""";
			string	title;
			Regex	re = new Regex(restring, RegexOptions.Compiled | RegexOptions.IgnoreCase);
			Match	m;
			while ((line = sysin.ReadLine()) != null) {
				++lineNo;
				if (0 == (lineNo % 100)) {
					Console.WriteLine("Processing line {0}", lineNo);
				}
				m = re.Match(line);
				if (m.Success) {
					title = m.Value;
					// Console.WriteLine("Found title at line {0} - {1}", lineNo, title);
					FindDup(title, dict, FullText);
				}
			}

			string [] keys = new string[dict.Keys.Count];
			dict.Keys.CopyTo(keys, 0);
			Array.Sort(keys);
			List<int>		lines;
			foreach (string key in keys) {
				lines = dict[key];
				Console.Write("Title {0} occurs {1} times on lines:", 
					key, lines.Count);
				for (int j = 0; j < lines.Count; j++) {
					Console.Write(" {0}", lines[j]);
				}
				Console.WriteLine();
			}
		}

//---------------------------------------------------------------------------------------

		private static void FindDup(string title, Dictionary<string, List<int>> dict, string[] FullText) {
			List<int>	LineNumbers = new List<int>();
			for (int i = 0; i < FullText.Length; i++) {
				if (FullText[i].Contains(title)) {
					LineNumbers.Add(i);
				}
			}
			if (LineNumbers.Count > 1) {
				// Yuck. Can't blindly add, in case the key's already there. Yuck.
				// dict.Add(title, LineNumbers);
				if (dict.ContainsKey(title)) {
					dict[title] = LineNumbers;
				} else {
					dict.Add(title, LineNumbers);
				}
			}
		}
	}
}
