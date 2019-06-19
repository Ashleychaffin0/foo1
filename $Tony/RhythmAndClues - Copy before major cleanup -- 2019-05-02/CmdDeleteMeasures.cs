namespace RhythmAndClues {
	class CmdDeleteMeasures : ICommand {
		readonly Interpreter main;

//---------------------------------------------------------------------------------------

		public CmdDeleteMeasures(Interpreter main) {
			this.main = main;
		}

//---------------------------------------------------------------------------------------

		public bool CheckSyntax(string[] tokens) {
			// TODO: Ensure that there are no parms. Here or on other commands.
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
			while (true) {
				var measure = main.GetFirstElement(FirstPart, "measure");
				if (measure is null) { break; }     // No measures found
				FirstPart.RemoveChild(measure);
				var FirstKid = FirstPart.FirstChild;
				if ((FirstKid != null) && (FirstKid.Name == "#comment")) {
					FirstPart.RemoveChild(FirstPart.FirstChild);
				}
			}
			return true;
		}
	}
}
