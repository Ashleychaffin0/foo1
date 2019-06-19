using System;
using System.Collections.Generic;
using System.Text;

namespace RhythmAndClues {
	class Utils {

//---------------------------------------------------------------------------------------

		public static bool ParseToken(Interpreter main, string[] tokens, int i, out DurationDef dur) {
			bool bFound = main.DurationIDs.TryGetValue(tokens[i], out dur);
			if (bFound) {
				return true;
			} else {
				main.SyntaxError($"Error: Parameter {i} to {tokens[0]} is invalid: {tokens[i]}");
				return false;
			}
		}
	}
}
