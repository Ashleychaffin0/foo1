using System;

namespace RhythmAndClues {
	class RhythmAndClues {
		static int Main(string[] args) {
			if (args.Length == 0) {
				Console.WriteLine("No filename passed with the name of the program file");
				return 1;
			}
			if (args.Length > 1) {
				Console.WriteLine("Warning: Ignoring superfluous parameter(s) passed to this program");
			}
			var interp = new Interpreter(args[0]);
			bool ExecutedOK = interp.Run();
			if (!ExecutedOK) {
				return 2;				// TODO: Magic #
			}
			return 0;
		}

//---------------------------------------------------------------------------------------

#if VERSION_1
		private static int CheckParms(string[] args, out string ProgramFilename, out string OutputFilename) {
			ProgramFilename    = null;
			OutputFilename = null;
			int retcode    = 0;

			switch (args.Length) {
			case 3:
				OutputFilename = args[2];
				if (File.Exists(OutputFilename)) {
					Console.WriteLine($"Warning: Output filename '{OutputFilename}' exists. Replace?");
					string YN = GetYesNo();
					if (YN == "N") {
						retcode = 1;
					}
				}
				goto case 2;
			case 2:
				ProgramFilename = args[1];
				if (!File.Exists(ProgramFilename)) {
					Console.WriteLine($"Error: Program file '{ProgramFilename}' not found");
					retcode = 1;
				}
				break;
			case 1:
				if (!File.Exists(args[0])) {
					Console.WriteLine($"Error: Music file '{args[0]}' not found");
					retcode = 1;
				}
				break;
			default:
				Console.WriteLine("Syntax: RhythmAndClues InputFilename ProgramFilename [OutputFilename]");
				retcode = 1;
				break;
			}
			return retcode;
		}

//---------------------------------------------------------------------------------------

		private static string GetYesNo() {
			while (true) {
				Console.Write("Enter Y to overwrite or N to quit: ");
				var c = Console.ReadKey();
				Console.WriteLine();
				switch (c.Key) {
				case ConsoleKey.Y:
					return "Y";
				case ConsoleKey.N:
					return "N";
				}
			}
		}
#endif
	}
}
