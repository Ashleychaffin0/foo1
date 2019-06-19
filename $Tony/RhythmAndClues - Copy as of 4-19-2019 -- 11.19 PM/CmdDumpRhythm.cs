using System;
using System.Collections.Generic;
using System.Text;

namespace RhythmAndClues {
	class CmdDumpRhythm : ICommand {
		public bool CheckSyntax(Interpreter main, string[] tokens) {
			main.Msg("Nonce of Check syntax for Dump-Rhythm");
			return true;
		}

//---------------------------------------------------------------------------------------

		public bool Command(Interpreter main, string[] tokens) {
			main.Msg("Nonce on execution of Dump-Rhythm");
			return true;
		}
	}
}
