using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeCommentTsql {
	class LRSFSM2 {

//---------------------------------------------------------------------------------------

		enum TsqlStates {
			InText,
			GotPossibleStartOfComment,
			InComment,
			InString
		}

//---------------------------------------------------------------------------------------

		enum Token {
			Hyphen,
			SingleQuote,
			NewLine,
			Digit,
			Default
		}

//---------------------------------------------------------------------------------------

		static Token GetTokenType(char c) {
			switch (c) {
			case '-':
				return Token.Hyphen;
			case '\'':
				return Token.SingleQuote;
			case '\n':
				return Token.NewLine;
			// Note: The next token type (Digit) isn't used. It's just here to show that
			//		 sometimes you can get away with using constants inside your FSM
			//		 code, but other times it might be a good idea to use a Token class.
			case '0':
			case '1':
			case '2':
			case '3':
			case '4':
			case '5':
			case '6':
			case '7':
			case '8':
			case '9':
				return Token.Digit;
			default:
				return Token.Default;
			}
		}

//---------------------------------------------------------------------------------------

		public static string DeCommentTsql(string s) {
			StringBuilder sb = new StringBuilder(s.Length);
			TsqlStates State = TsqlStates.InText;
			foreach (char c in s) {
				Token tok = GetTokenType(c);
				switch (State) {
				case TsqlStates.InText:
					if (tok == Token.Hyphen) {
						State = TsqlStates.GotPossibleStartOfComment;
						break;
					} else if (tok == Token.SingleQuote) {	// TSQL strings start with ', right?
						State = TsqlStates.InString;
						sb.Append(c);
						break;
					}
					sb.Append(c);				// Ordinary character, output it
					break;


				// We're going to handle only -- comments, and not /* */ ones. Adding
				// support for the latter is left as an exercise for the student.
				//
				// BTW, there are two possible definitions for how /* */ comments work.
				// In one case, the first */ after a /* ends the comment. I *think* this
				// is how TSQL works. But in some C compilers, /* */ comments can *nest*.
				// IOW, if you have a block of code (with /* */ comments in it), and you
				// want to comment out the whole thing then you can just slap a /* just
				// before the code and an */ at the end, and the */ at the end of the
				// inner comment doesn't stop the outer comment.

				case TsqlStates.GotPossibleStartOfComment:
					if (tok == Token.Hyphen) {
						State = TsqlStates.InComment;
					} else {
						sb.Append('-');			// e.g. we found a minus sign, not the
												// start of a comment.
						sb.Append(c);
						State = TsqlStates.InText;
					}
					break;

				case TsqlStates.InComment:
					if (tok == Token.NewLine) {
						sb.Append(c);
						State = TsqlStates.InText;	// -- comments go only to EOL
						// Note: This code will swallow a \r just before the \n. Some
						// would validly consider this to be a bug. But in this
						// Let's-practice-our-FSM's exercise, I'm not going to worry
						// about it. If necessary, we can treat it as we did the other
						// doubled-characters (notably, --) situations.
													
					}
					break;

				// It's valid to have -- inside a string, but this isn't a comment. So we
				// must handle this case.
				//
				// Of course the problem with strings is that the delimeter (either
				// " or ') can appear within the string. Some languages double the
				// character (e.g. 'I can''t'). Others use an escape character
				// ('I can\'t). Others (like SQL) let you specify an escape character
				// by means of an ESCAPE keyword ('I can$t' ESCAPE '$'). Obviously we're
				// not going to do a full parse just to process an ESCAPE keyword.
				//
				// Now as far as I can tell / remember, TSQL normally doubles the string-
				// start character ("I can""t). So as we said with \r\n above, we could
				// handle this as we did with --, and I won't complicate the code here
				// again to handle that case.
				case TsqlStates.InString:
					sb.Append(c);
					if (tok == Token.SingleQuote) {
						State = TsqlStates.InText;
					}
					break;
				}
			}
			return sb.ToString();
		}
	}
}
