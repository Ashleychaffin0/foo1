namespace RhythmAndClues {
	class CmdDumpRhythm : ICommand {
		readonly Interpreter main;

//---------------------------------------------------------------------------------------

		public CmdDumpRhythm(Interpreter main) {
			this.main = main;
		}

//---------------------------------------------------------------------------------------

		public bool CheckSyntax(string[] tokens) {
			main.Msg("Nonce of Check syntax for Dump-Rhythm");
			return true;
		}

//---------------------------------------------------------------------------------------

		public bool Execute(string[] tokens) {
			main.Msg("Nonce on execution of Dump-Rhythm");
			return true;
		}
	}
}
