using System;
using System.Collections.Generic;
using System.Text;

namespace RhythmAndClues {
	class CmdRhythm : ICommand {
		public bool CheckSyntax(Interpreter main, string[] tokens) {
			main.Msg("Nonce on Check syntax for Rhythm");
			return true;
		}

//---------------------------------------------------------------------------------------

		public bool Command(Interpreter main, string[] tokens) {
			main.Msg("Nonce on execution of Rhythm");
			return true;
		}
	}
}
