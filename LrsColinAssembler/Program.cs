// "A foolish consistency is the hobgoblin of little minds."
//		- Ralph Waldo Emerson

// Note: Since this is just a program to introduce my cousin Colin
//		 to a concrete example of machine/assembler language, it
//		 assumes that the source code is pretty well perfect and
//		 we'll do limited error detection. So no comments, please,
//		 along the lines of "But it could fail here!". Yep, you're
//		 right, it could. But this is hardly a production-class
//		 program!

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LrsColinAssembler {
	class Program {
		public static int Main(string[] args) {
			string filename = "FakeAssemblerProgram.asm";	// Default filename
			// Support filename on command line, or default
			if (args.Length == 0) {
				var names = from fn in Directory.EnumerateFiles(".")
							let ext = Path.GetExtension(fn)
							where ext == ".asm" || ext == ".bin"
							select fn;
				// Note: This code naively assumes that there is at least one
				//		 .asm or .bin file in the output folder. For this program
				//		 I'm going to assume that we have at least one.
				filename = SelectFromList(names);
			} else {
				filename = args[0];
			}

			const int RAMSIZE = 1024 * 10;          // 10K should be enough for our toy pgms
			byte[] Ram = new byte[RAMSIZE];         // Our actual Ram array

			string temp = Path.GetTempFileName();
			using (var cc = new ConsoleCopy(temp)) {
				var avengers = new Assembler(Ram);
				// Support processing .bin files
				if (Path.GetExtension(filename) == ".bin") {
					using (var rdr = new BinaryReader(File.OpenRead(filename))) {
						var fi = new FileInfo(filename);
						byte[] bin = rdr.ReadBytes((int)fi.Length);
						Array.Copy(bin, Ram, fi.Length);
					}
				} else {
					int rc = avengers.Assemble(filename);
					if (rc != 0) { return 1; }
				}
				avengers.Run();
			}

			// TODO: Get this working
			// DebugInfo.CopyToClipboard(File.ReadAllText(temp));
			return 0;
		}

//---------------------------------------------------------------------------------------

		private static string SelectFromList(IEnumerable<string> names) {
			string[] nms = names.ToArray();
			int num;
			bool bOK;
			do {
				Console.WriteLine("Please choose from the following list by number");
				int i = 0;
				foreach (string name in nms) {
					Console.WriteLine($"[{++i}] {name}");
				}
				Console.Write("Please enter your selection: ");
				// num = -1;
				bOK = int.TryParse(Console.ReadLine(), out num);
				bOK &= (num > 0) && (num <= nms.Count());
				if (! bOK) { 
					Console.WriteLine("Invalid response. Try again");
					Console.WriteLine();
				}
			} while (! bOK);
			return nms[num - 1];
		}
	}
}
