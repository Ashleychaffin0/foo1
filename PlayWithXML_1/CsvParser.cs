// IGNORE. NEVER COMPLETED. DOESN'T WORK WITH OUR CRUDDY IMPLEMENTATION THAT ALLOWS
// DOUBLE-QUOTES INSIDE STRINGS

using System;
using System.Collections.Generic;
using System.Text;

namespace PlayWithXML_1 {
	public class CsvParser {
		enum State {
			StartOfField,
			InNormalString,			// Field without starting double-quote
			InQuotedString,			// Field started with double-quote
		}

		enum CharacterType {
			DoubleQuote,
			Comma,
			Other
		}
		
		/// <summary>
		/// A simple finite state machine to scan a string in CSV format.
		/// </summary>
		/// <param name="s">The string to be scanned</param>
		/// <param name="Values">A list of Values (the "V" in CSV)</param>
		/// <param name="FailureIndex">If the parsing failed, returns the index in "s" of where the problem was</param>
		/// <returns>true if everything worked, false otherwise</returns>
		public static bool TryParse(string s, out List<string> Values, out int FailureIndex) {
			State			CurState = State.StartOfField;
			CharacterType	CharType;
			char			c;
			StringBuilder	val = new StringBuilder();

			Values = new List<string>();
			FailureIndex = -1;

			for (int i = 0; i < s.Length; i++) {
				c = s[i];
				CharType = c == '"' ? CharacterType.DoubleQuote :
						   c == ',' ? CharacterType.Comma :
									  CharacterType.Other;
				switch (CurState) {
				case State.StartOfField:
					val.Length = 0;					// Restart field capture
					CurState = c == '"' ? State.InQuotedString : State.InNormalString;
					break;
				case State.InNormalString:
					if (c == ',') {
						// TODO:
					} else {
						// TODO:
					}
					break;
				case State.InQuotedString:
					// TODO:
					break;
				default:
					break;
				}
			}
		}
	}
}
