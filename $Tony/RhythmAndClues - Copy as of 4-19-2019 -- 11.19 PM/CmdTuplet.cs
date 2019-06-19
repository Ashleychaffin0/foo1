using System;
using System.Collections.Generic;
using System.Text;

namespace RhythmAndClues {
	class CmdTuplet : ICommand {
		public bool CheckSyntax(Interpreter main, string[] tokens) {
			main.Msg("Nonce on Check syntax for Tuplet");
			return true;
		}

//---------------------------------------------------------------------------------------

		public bool Command(Interpreter main, string[] tokens) {
			main.Msg("Nonce on execution of Tuplet");
			return true;
		}
	}
}
