using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotQuiteSoDumbInterpreter {
	class Source {
		private string Src;

		public const char EOF = unchecked((char)-1);

		// Next few fields are for NextChar and error reporting
		public int CurLineNum;
		public int CurCol;

//---------------------------------------------------------------------------------------

		public Source(string src) {
			Src        = src;
			CurLineNum = 1;
			CurCol     = 0;
		}

//---------------------------------------------------------------------------------------

		public char NextChar() {
			if (IsEof()) {
				CurLineNum = -1;
				CurCol = 0;
				return EOF;
			}
			char c = Src[CurCol++];
			if (c == '\n') ++CurLineNum;
			return c;

		}

//---------------------------------------------------------------------------------------

		public void UnGetChar() {
			if (CurCol <= 0) throw new IndexOutOfRangeException("UnGetChar: Backing up before start of text");
			// TODO: Check if we have to decrement CurLineNum
			--CurCol;
		}

//---------------------------------------------------------------------------------------

		private bool IsEof() => CurCol >= Src.Length;
	}
}
