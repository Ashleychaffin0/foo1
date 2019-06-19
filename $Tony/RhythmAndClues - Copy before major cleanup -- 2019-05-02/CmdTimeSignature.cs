namespace RhythmAndClues {
	class CmdTimeSignature : ICommand {
		readonly Interpreter main;
		int Numerator;
		int Denominator;

//---------------------------------------------------------------------------------------

		public CmdTimeSignature(Interpreter main) {
			this.main = main;
		}

//---------------------------------------------------------------------------------------
		public bool CheckSyntax(string[] tokens) {
			bool retval = true;
			if (tokens.Length != 3) {
				main.SyntaxError($"Syntax: Time-Signature numerator denominator");
				retval = false;
			}

			// There are only exactly 2 parms; Brute force it;
			bool bOK = int.TryParse(tokens[1], out Numerator);
			if ((!bOK) || (Numerator <= 1)) {
				main.SyntaxError($"Set-Timesignature paramter 1 is invalid: {tokens[1]}");
				retval = false;
			}

			 bOK = int.TryParse(tokens[2], out Denominator);
			if ((!bOK) || (Numerator <= 1)) {
				main.SyntaxError($"Set-Timesignature paramter 2 is invalid: {tokens[2]}");
				retval = false;
			}

			return retval;
		}

//---------------------------------------------------------------------------------------

		public bool Execute(string[] tokens) {
			var beats           = main.OriginalFirstMeasure.SelectSingleNode("attributes/time/beats");
			beats.InnerText     = Numerator.ToString();
			var beat_type       = main.OriginalFirstMeasure.SelectSingleNode("attributes/time/beat-type");
			beat_type.InnerText = Denominator.ToString();
			main.Msg($"Time signature set to {Numerator} / {Denominator}");
			return true;
		}
	}
}
