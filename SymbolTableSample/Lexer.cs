// This is the toyest of toy lexical analyzers.
// Given a line of source code, we'll strip away all but
// the possible keywords, variable names and numbers and
// return a stream (OK, vector) of tokens. So, for example,
//		for (int i = 1; i < 10; ++i) {
// would return
//	"for", "int", "i", "1", "i", "10", "i"

using System;

namespace SymbolTableSample {
	internal class Lexer {

		internal static string[] Tokenize_Alternate_1(string line) {
			line = line
				.Replace("(", "")
				.Replace(")", "")
				.Replace("+", "")
				// And so on
				;
			// Tokenize the line on blanks
			return line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
		}

//---------------------------------------------------------------------------------------

		internal static string[] Tokenize_Alternate_2(string line) {
			foreach (char c in "().+-*/{}\"[];<>:?,='") {
				line = line.Replace(c.ToString(), " ");
			}
			// Tokenize the line on blanks
			return line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
		}

//---------------------------------------------------------------------------------------

		internal static string[] Tokenize(string line) {
			foreach (char c in line) {
				if (! (char.IsLetterOrDigit(c)) || (c == ' ')) {
					line = line.Replace(c.ToString(), " ");
				}
			}

			// Tokenize the line on blanks
			return line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
		}
	}
}