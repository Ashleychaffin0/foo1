using System;
using System.Collections.Generic;
using System.Text;

namespace RhythmAndClues {
	class CmdRestoreMeasures : ICommand {
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
			var Parent = FirstPart.ParentNode;
			for (int i = 0; i < main.SaveMeasures.Count; i++) {
				Parent.InsertAfter(main.SaveMeasures[i], FirstPart);
			}
			return true;
		}
	}
}
