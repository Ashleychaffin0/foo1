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
	}
}
