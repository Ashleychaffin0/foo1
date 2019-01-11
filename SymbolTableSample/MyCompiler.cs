using System;

// TODO: Lexer ignores (drops) all tokens on and after "/"

// OK, the rules are as follows:
//	*	Error checking is mostly non-existent
//	*	Rule 1: A blank line is ignored

//	*	Rule 2: A line that has the token "class" in it starts a class.
//		The token after it is the name of the class

/////////	*	A line that begins with "proc" is treated as a method
//	*	A line that begins with a keyword is handled specially. See below.
//	*	A line that starts with a known data type (int, float, etc) takes
//		all the non-numeric tokens to its right as things to be entered into
//		the symbol table
//	*	

namespace SymbolTableSample {
    internal class MyCompiler {
        private SymbolTable SymTab;

//---------------------------------------------------------------------------------------

        public MyCompiler(SymbolTable foo) {
            this.SymTab = foo;
        }

//---------------------------------------------------------------------------------------

        public void Compile(string[] Tokens) {
			// Rule 1: Ignore blank lines
            if (Tokens.Length == 0) {
                return;
            }

			// Rule 2: Handle "class" declaration
			int ixClass = IndexOfKeyword(Tokens, "class");
			if (ixClass >= 0) {
				DoClass(Tokens, ixClass);
				return;
			}

            var bIsDataType = SymTab.GetDataType(Tokens[0]);
			if (bIsDataType != SymbolTable.DataTypeValue.Unknown) {
				DoDeclaration(Tokens);                  // TODO:
			} else {
				bool bIsKeyword = SymTab.IsKeyword(Tokens[0]);
				if (bIsKeyword) {
					DoKeyword(Tokens);
				}
			}
        }

//---------------------------------------------------------------------------------------

		private void DoClass(string[] tokens, int ixClass) {
			throw new NotImplementedException();
		}

//---------------------------------------------------------------------------------------

		private void DoDeclaration(string[] tokens)	{
			throw new NotImplementedException();
		}

//---------------------------------------------------------------------------------------

		private void DoKeyword(string[] tokens) {
			throw new NotImplementedException();
		}

//---------------------------------------------------------------------------------------

		private int IndexOfKeyword(string[] Tokens, string KeyWord) {
			for (int i = 0; i < Tokens.Length; i++) {
				if (Tokens[i] == KeyWord) {
					return i;
				}
			}
			return -1;
		}
	}
}
