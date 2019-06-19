using System;
using System.Collections.Generic;
using System.Text;

namespace RhythmAndClues {
	class CmdDeleteMeasures : ICommand {
		public bool CheckSyntax(Interpreter main, string[] tokens) {
			bool retval = true;
			if (!main.bLoadCommandFound) {
				main.SyntaxError($"Error: Can't {tokens[0]} because no LOAD was done");
				retval = false;
			}
			return retval;
		}

//---------------------------------------------------------------------------------------

		public bool Command(Interpreter main, string[] tokens) {
			var FirstPart = main.xml.FirstPart;
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
