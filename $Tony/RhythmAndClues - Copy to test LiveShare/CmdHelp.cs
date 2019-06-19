namespace RhythmAndClues {
	// The HELP command
	class CmdHelp : ICommand {
		readonly Interpreter main;

//---------------------------------------------------------------------------------------

		public CmdHelp(Interpreter main) {
			this.main = main;
		}

//---------------------------------------------------------------------------------------

		public bool CheckSyntax(string[] tokens) {
			if (tokens.Length > 1) {
				main.SyntaxError("The HELP command takes no parameters");
				return false;
			}
			return true;
		}

//---------------------------------------------------------------------------------------

		public bool Execute(string[] tokens) {
			main.Msg("The valid commands are:");
			main.Msg("\tLoad <filename>");
			main.Msg("\tRhythm <list of note durations>. They are:");
			main.Msg("\t\t" + string.Join(", ", main.DurationIDs.Keys));
			main.Msg("\tTuplet number-of-notes notes");
			main.Msg("\tApply-Rhythm/AR");
			main.Msg("\tTime-Signature/TS numerator denominator");
			main.Msg("\tDump-Measures");
			main.Msg("\tSave <filename>");
			return true;
		}
	}
}
