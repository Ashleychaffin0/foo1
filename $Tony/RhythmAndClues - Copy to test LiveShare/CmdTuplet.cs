namespace RhythmAndClues {
	class CmdTuplet : ICommand {
		readonly Interpreter main;

//---------------------------------------------------------------------------------------

		public CmdTuplet(Interpreter main) {
			this.main = main;
		}

//---------------------------------------------------------------------------------------

		public bool CheckSyntax(string[] tokens) {
			main.Msg("Nonce on Check syntax for Tuplet");
			return true;
		}

//---------------------------------------------------------------------------------------

		public bool Execute(string[] tokens) {
			main.Msg("Nonce on execution of Tuplet");
			return true;
		}
	}
}
