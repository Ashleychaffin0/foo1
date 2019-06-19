namespace RhythmAndClues {
	class CmdRestoreMeasures : ICommand {
		readonly Interpreter main;

//---------------------------------------------------------------------------------------

		public CmdRestoreMeasures(Interpreter main) {
			this.main = main;
		}

//---------------------------------------------------------------------------------------

		public bool CheckSyntax(string[] tokens) {
			bool retval = true;
			if (!main.bLoadCommandFound) {
				main.SyntaxError($"Error: Can't {tokens[0]} because no LOAD was done");
				retval = false;
			}
			return retval;
		}

//---------------------------------------------------------------------------------------

		public bool Execute(string[] tokens) {
			var FirstPart = main.XmlScore.FirstPart;
			var Parent = FirstPart.ParentNode;
			for (int i = 0; i < main.OriginalMeasures.Count; i++) {
				Parent.InsertAfter(main.OriginalMeasures[i], FirstPart);
			}
			return true;
		}
	}
}
