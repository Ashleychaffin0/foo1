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
			main.Msg("\tLoad-Notes <filename>");
			main.Msg("\tLoad-Pattern <filename>");
			main.Msg("\tApply-Rhythm/AR");
			main.Msg("\tDump-Measures");
			main.Msg("\tSave <filename>");
			main.Msg();
			main.Msg("All commands are case-insensitive, except perhaps filenames");
			return true;
		}
	}
}
