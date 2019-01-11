using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotQuiteSoDumbInterpreter {
	class Source_Original {

		public const char EOF = (char)0xffff;

		private List<string> ProgramLines;
		private string CurrentLine;

		// Next few fields are for error reporting
		public int CurrentLineNumber;
		public int CurrentColumn;

//---------------------------------------------------------------------------------------

		public Source_Original(string src) {
			ProgramLines  = src.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
			CurrentLineNumber = 0;
			CurrentLine   = ProgramLines[0];  // TODO: Check for empty <src>
			CurrentColumn = 0;
		}

//---------------------------------------------------------------------------------------

		public char NextChar() {
			if (IsEof()) return EOF;
			// if (CurrentColumn >= CurrentLine.Length) return EOF;
			if (CurrentColumn >= CurrentLine.Length) {
				++CurrentLineNumber;
				CurrentLine = ProgramLines[CurrentLineNumber];
				CurrentColumn = 0;
			}
			char CurChar = CurrentLine[CurrentColumn++];
			return CurChar;
		}

//---------------------------------------------------------------------------------------

		private bool IsEof() {
			if (CurrentLineNumber >= ProgramLines.Count) return true;
			if ((CurrentLineNumber == ProgramLines.Count - 1) &&
					CurrentColumn >= CurrentLine.Length) {
				return true;
			}
			return false;
		}

//---------------------------------------------------------------------------------------

		internal void UnGetChar() {
			if (CurrentColumn == 0) {
				CurrentLine = ProgramLines[--CurrentLineNumber];    // TODO: Check for CurrentLineNumber == 0
				CurrentColumn = CurrentLine.Length - 1;
			} else {
				--CurrentColumn;
			}
		}

		// TODO: Method PeekNextChar?
	}
}
