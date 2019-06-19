using System;
using System.Collections.Generic;
using System.Text;

namespace RhythmAndClues {
	class CmdTimeSignature : ICommand {
		bool ICommand.CheckSyntax(Interpreter main, string[] tokens) {
			main.Msg("Nonce of Check syntax for Time-Signature");
			return true;
		}

//---------------------------------------------------------------------------------------

		bool ICommand.Command(Interpreter main, string[] tokens) {
			main.Msg("Nonce on execution of Time-Signature");
			return true;
		}
	}
}
