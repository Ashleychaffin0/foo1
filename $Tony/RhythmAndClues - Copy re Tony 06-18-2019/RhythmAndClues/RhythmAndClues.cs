using System;

namespace RhythmAndClues {
	class RhythmAndClues {
		static int Main(string[] args) {
			const int Error_NoFilename = 1;
			const int Error_RunFailed = 2;

			if (args.Length == 0) {
				// Kludge
				// Console.WriteLine("No filename passed with the name of the program file");
				// return Error_NoFilename;
				args = new string[] { "pgm_beethoven.txt" };
			}

			if (args.Length > 1) {
				Console.WriteLine("Warning: Ignoring superfluous parameter(s) passed to this program");
			}

			var interp = new Interpreter(args[0]);
			bool ExecutedOK = interp.Run();
			if (!ExecutedOK) {
				return Error_RunFailed;
			}
			return 0;
		}
	}
}
