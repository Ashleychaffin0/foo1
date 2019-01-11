using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotQuiteSoDumbInterpreter {
	class Lexer {
		Source Src;

//---------------------------------------------------------------------------------------

		public Lexer(Source ProgramSource) {
			this.Src = ProgramSource;
		}

//---------------------------------------------------------------------------------------

		public IEnumerable<Symbol> NextToken() {
			while (true) {
				SkipWhiteSpace();
				string token = GetToken();
				Console.WriteLine($"Line {Src.CurLineNum}, Token = '{token}'");
			}
		}

//---------------------------------------------------------------------------------------

		private void SkipWhiteSpace() {
`																								while (true) {
				char c = Src.NextChar();
				// TODO: Check for \n and increment LineNumber if necessary
				if (c == Source.EOF) return;
				if (! char.IsWhiteSpace(c)) {
					Src.UnGetChar();
					return;
				}
			}
		}

//---------------------------------------------------------------------------------------

		private string GetToken() {
			while (true) {
				char c = Src.NextChar();
				// if (c == Source.EOF) return Source.EOF;
				if (char.IsLetter(c)) return ScanName(c);
				if (char.IsDigit(c)) return ScanNumber(c);  // Note: -ve #s not supported
				return ScanOperator(c);
			}
		}

//---------------------------------------------------------------------------------------

		private string ScanName(char chr) {
			var sb = new StringBuilder();
			sb.Append(chr);
			while (true) {
				char c = Src.NextChar();
				if (c == Source.EOF) return sb.ToString();

				if (char.IsLetterOrDigit(c) || (c == '_')) {
					sb.Append(c);
				} else {
					Src.UnGetChar();
					return sb.ToString();
				}
			}
		}

//---------------------------------------------------------------------------------------

		private string ScanNumber(char c) => throw new NotImplementedException();

//---------------------------------------------------------------------------------------

		private string ScanOperator(char c) => throw new NotImplementedException();
	}

	internal enum LineEnd {			// TODO: Find better name. Also, still used/useful?
		Midst,
		LineEnd,
		EndOfFile
	}
}
