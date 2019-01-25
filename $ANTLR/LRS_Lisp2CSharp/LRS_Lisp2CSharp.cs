using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LRS_Lisp2CSharp {
	public partial class LRS_Lisp2CSharp : Form {

		public enum State : byte {
			START,
			S_LISTSTART,		// Just found opening (
			S_WS,				// White Space
			S_TOKEN,
			S_STRING,
			S_COMMENT,
		}		

//---------------------------------------------------------------------------------------

		public enum TokenType : byte {
			BLANK,
			SINGLEQUOTE,		// '
			DOUBLEQUOTE,		// "
			BACKSLASH,
			OPEN_PAREN,
			CLOSE_PAREN,
			DOT,				// For dotted pairs
			NEWLINE,
			WS,					// Whitespace
			SEMIC,				// Semicolon

			NORMAL,				// Alhpanum etc
			
			// TODO: Other possible types

			// TODO: Double-character, maybe
			HYPHEN,				// First part of -> (but also part of -123
			LESSTHAN,			// <= (But also just <)
			GREATERTHAN,	
			EQUAL,				// ==
		}

//---------------------------------------------------------------------------------------

		State CurState = State.START;
		TokenType CurTokenType;
		char CurChar;
		int CurInputLineNumber = 1;

		string inp;					// The complete source string
		int CurOffset = 0;			// Index into <inp>

//---------------------------------------------------------------------------------------

		string InputFileName = @"D:\Downloads\CLISP\JASDecomp\JAS Decomp -- 2013-06-16\Decompos.lisp";

//---------------------------------------------------------------------------------------

		public LRS_Lisp2CSharp() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, EventArgs e) {
			inp = File.ReadAllText(InputFileName);	// TODO: Needs try/catch if file not found, accessible, etc
			Listp2C();
		}

//---------------------------------------------------------------------------------------

		private void Listp2C() {
			var CurNode = new List<LispNode_Obsolete_1>();
			while (CurOffset < inp.Length) {
				SkipWhiteSpace();
				if (IsComment()) {
					string s = GetRestOfLineComment();
					CurNode.Add(new LispNode_Obsolete_1(LispNode_Obsolete_1.NodeType_Obsolete_1.COMMENT, s));
				} else if (CurChar == '(') {
					CurNode.Add (new LispNode_Obsolete_1(LispNode_Obsolete_1.NodeType_Obsolete_1.LIST, GetList(CurOffset)));
				}
			}
		}

//---------------------------------------------------------------------------------------

		private object GetList(int CurOffset) {
			SkipWhiteSpace();
		}

//---------------------------------------------------------------------------------------

		private void Listp2C_Obsolete() {
			while (CurOffset < inp.Length) {
				CurChar = inp[CurOffset];
				CurTokenType = GetTokenType();

				switch (CurState) {
				case State.START:
					break;
				case State.S_WS:
					break;
				case State.S_TOKEN:
					break;
				case State.S_STRING:
					break;
				case State.S_COMMENT:
					break;
				default:
					break;
				}
				++CurOffset;
			}
		}

//---------------------------------------------------------------------------------------

		private TokenType GetTokenType() {
			string s;

			switch (CurChar) {
			case '\n':
				++CurInputLineNumber;
				break;

			case '(':
				break;

			case ')':
				break;

			case ';':
				s = GetRestOfLineComment();
				break;

			default:
				break;
			}

			return TokenType.HYPHEN;		// TODO:
		}

//---------------------------------------------------------------------------------------

		private string GetRestOfLineComment() {
			++CurOffset;			// Skip over leading ';'
			string txt;
			int n = inp.IndexOf('\n', CurOffset);
			if (n < 0) {
				// TODO: Must have hit EOF
				// TODO:
				return "TODO: GetRestOfLineComment";
			} else {
				txt = inp.Substring(CurOffset, n - CurOffset - 1);
				CurOffset = n + 1;
				++CurInputLineNumber;
				return txt;
			}
		}

//---------------------------------------------------------------------------------------

		bool IsAtomCharacter(char c) {
			if (char.IsLetterOrDigit(c) || (c == '-')) {
				return true;
			}
			return false;
		}

//---------------------------------------------------------------------------------------

		bool IsComment() {
			return CurChar == ';';
		}

//---------------------------------------------------------------------------------------

		void SkipWhiteSpace() {
			while (char.IsWhiteSpace(CurChar)) {
				CurOffset++;
			}
			CurChar = inp[CurOffset];
		}

//---------------------------------------------------------------------------------------

		string GetString() {
			int Start = CurOffset;			// Position of quote
			while (inp[CurOffset++] != '"') {
				// Empty loop. While test does it all
			}
			return inp.Substring(Start + 1, CurOffset - Start + 1 - 1); // Ignore opening
											// and closing quotes
		}
	}
}
