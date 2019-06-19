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
			main.Msg("\t\tPut tuplets in [ brackets ]");
			main.Msg("\tApply-Rhythm/AR");
			main.Msg("\tTime-Signature/TimeSig numerator denominator");
			main.Msg("\tDump-Measures");
			main.Msg("\tSave <filename>");
			main.Msg();
			main.Msg("All commands are case-insensitive, except perhaps filenames");
			return true;
		}
	}
}
