using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SplitAPLSource {
	class Program {
		static void Main(string[] args) {

#if DEBUG
			Doit(@"D:\Downloads\APL360_source_code\APL360_source_code.txt", @"D:\LRS\APL360");
#else
			if (args.Length != 2) {
				Console.WriteLine("Usage: SplitAPLSource inFile outDir");
				return;
			}
			Doit(args[0], args[1]);
#endif
		}

//---------------------------------------------------------------------------------------

		private static void Doit(string inFile, string OutDir) {
			var reAdd = new Regex(@"\./\s+ADD\s+NAME=(?<Name>.*)", RegexOptions.Compiled);
			var reTitle = new Regex(@"^\w*\s+TITLE\s+'(?<Title>[^']*)'", RegexOptions.Compiled);
			// Sample: MDIV     TITLE 'DOMINO --   M A T R I X     D I V I D E'  
			Directory.CreateDirectory(OutDir);
			StreamWriter sw = null;

			bool bFirstLine     = false;
			string MemberName   = null;
			string OriginalName = null;
			string Title        = null;

			using (var sr = new StreamReader(inFile)) {
				string line;
				while ((line = sr.ReadLine()) != null) {
					if (bFirstLine) {
						var TitleMatch = reTitle.Match(line);
						if (TitleMatch.Success) {
							Title = TitleMatch.Groups["Title"].Value;
						} else {
							Title = "";
						}
						bFirstLine = false;
					}
					if (line.StartsWith("./")) {
						if (sw != null) {
							sw.Close();
							DoNewName(OriginalName, Title);
						}
						var xxx = reAdd.Match(line);
						// Assume properly formed ./ ADD
						MemberName = xxx.Groups["Name"].Value;
						OriginalName = Path.Combine(OutDir, MemberName);
						sw = new StreamWriter(OriginalName);
						bFirstLine = true;			// Next line is 1st line of module
					} else {
						sw.WriteLine(line);
					}
				}
				sw.Close();
				DoNewName(OriginalName, Title);

			}
		}

//---------------------------------------------------------------------------------------

		private static void DoNewName(string OriginalName, string Title) {
			Title = Title.Replace('/', '-');
			string NewName;
			if (Title.Length == 0) {
				NewName = OriginalName;
			} else {
				NewName = OriginalName + " - " + Title;
			}
			NewName += ".asm";
			if (File.Exists(NewName)) {
				File.Delete(NewName);
			}
			File.Move(OriginalName, NewName);
		}
	}
}
