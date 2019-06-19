using System;
using System.Collections.Generic;
using System.Text;

namespace RhythmAndClues {
	// The HELP command
	class CmdHelp : ICommand {
		public bool CheckSyntax(Interpreter main, string[] tokens) {
			if (tokens.Length > 1) {
				main.SyntaxError("The HELP command takes no parameters");
				return false;
			}
			return true;
		}

//---------------------------------------------------------------------------------------

		public bool Command(Interpreter main, string[] tokens) {
			main.Msg("The valid commands are:");
			// TODO: Redo this
			main.Msg("\tLoad <filename>");
			main.Msg("\tRhythm <list of note durations>. They are:");
			main.Msg("\t\t" + string.Join(", ", main.ValidDurations.Keys)); // TODO:
			main.Msg("\tTuplet number-of-notes notes");
			main.Msg("\tApply-Rhythm");
			main.Msg("\tDump-Measures");
			main.Msg("\tSave <filename>");
			return true;
		}
	}
}
