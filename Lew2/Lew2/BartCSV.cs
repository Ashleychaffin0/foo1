// Copyright (c) 2008 Bartizan Connects LLC

// This class parses a single line of a CSV file as output from Excel. In particular,
// only those fields that have an embedded special character (notably commas and/or
// double-quotes) will be surrounded by double quotes (although any field may be
// surrounded by double-quotes, subject to the rule below about fields that contain
// double-quotes as part of their data). The rest of the fields are unquoted, and
// simply separated by commas. Note that leading and trailing blanks are significant.
// Thus a missing field must be specified by a ",," sequence, since ", ," will be taken
// as a field with a single blank. 

// A double-quote data character is encoded as two successive double-quotes. Thus a
// field with a value of <He said "foo"> would be represented as "He said ""foo""".

// The data is returned in a List<string>. All fields will be non-null, but any or all
// fields may be empty.

// This routine does not fully implement RFC 4180 at http://tools.ietf.org/html/rfc4180,
// titled "Common Format and MIME Type for Comma-Separated Values (CSV) Files". In
// particular, it doesn't allow records to span multiple lines. IOW it doesn't process
// CR/LF's inside fields. All CR/LF's are ignored.

// Also, a double-quote inside a "normal" (non-double-quote-delimeted) field is
// (according to RFC 4180) really an error. But we'll (sigh) accept that.

// And last (but worst) of all, if we find a (non-doubled) double-quote inside a quoted
// string, that is *not* followed immediately by a comma, we'll grit our teeth, grumble
// under our breath (hey, it's not easy doing both at once!) and accept the double-quote
// as a data character, and *not* produce an error.

// So this routine will make what sense it can out of just about anything. There are no
// error returns.

// Update, April 2008
// OK, it's kludge city time. Due to, well, let's be kind and call it an "oversight",
// the various flavors of Expo units can place a naked double-quote inside an already
// quoted string, as mentioned above. Where it gets hairy is the case where
//		* You have a quoted field which isn't the last field in the string. For example,
//		  "abc","def","ghi", and let's concentrate on the "def" field.
//		* But the data portion of the field ends with a double quote: 
//		  "abc","de"","ghi"
// Strictly speaking, this is syntactically invalid. Any decent compiler, given this
// input, wouldn't think twice about telling you to fix the stupid thing and then
// recompile. But no, this error pops up in our data far too frequently. I guess that
// maybe users are entering data and supplying the quotes (especially the trailing one)
// themselves?
// So as I said, we'll (sigh) put in another kludge. When we find a double-quote, we'll
// check to see if the next 3 characters are <",">. If so we'll interpret as the end of
// the field. Which, of course, could screw up our parsing later on in the string, but
// what do you expect when parsing a syntax error? Remember: Garbage-In-Garbage-Out.
// Finally, we'll provide a <SupportQuoteCommaQuote> Boolean parameter to tell if we're
// parsing a real CSV line (value = false, the default), or one of the Expo box's 
// screwy lines (value = true).

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Bartizan.Input.Utils {
	public static class BartCSV {

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

		private enum FsmState {
			Start,
			InsideNormalField,
			InsideQuotedField
		}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

		private class TryParseParms {
			internal List<string>	Values;

			StringBuilder	CurrentField;

//---------------------------------------------------------------------------------------

			internal TryParseParms(List<string> Values) {
				this.Values = Values;
				
				CurrentField = new StringBuilder();;
			}

//---------------------------------------------------------------------------------------

			internal void AddCurrentFieldToValues() {
				Values.Add(CurrentField.ToString());
				CurrentField.Length = 0;		// Reset for next time
			}

//---------------------------------------------------------------------------------------

			internal void AddCharToCurrentField(char c) {
				CurrentField.Append(c);
			}
		}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

		public static void Parse(string text, out List<string> Values) {
			Parse(text, out Values, false);
		}

//---------------------------------------------------------------------------------------

		public static void Parse(string text, out List<string> Values, bool SupportQuoteCommaQuote) {
			Values = new List<string>();
			if (string.IsNullOrEmpty(text)) {
				return;						// Return empty list
			}
			TryParseParms	Parms = new TryParseParms(Values);

			FsmState		State = FsmState.Start;

			StringPosition	sp = new StringPosition(text);

			for (int i = 0; i < text.Length; i++) {
				sp.SetPos(i);
				char	c = text[i];
				if ((c == '\n') || (c == '\r')) {	// Ignore CR/LF
					continue;
				}

				// Note: We're no longer using the variable <NextChar>, but I'm leaving
				//		 the following comments in just in case someone runs into the
				//		 problem again...
				// Looks like a compiler bug in VS2005/2008. The following line gives
				// "Type of conditional expression cannot be determined because there
				// is no implicit conversion between '<null>' and 'char'".
				// char?	NextChar = (i == (text.Length - 1)) ? null : text[i + 1];

				// Update: OK, it's not considered a bug. See 
				// https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=90776
				// https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=42599
				// (However, also note that the VS2005 team member who investigated
				// this issue said (in stilted English) "Something I agree - since
				// null-type is special case and very used often - this will be nice to 
				// make it special in this situation also." One workaround is:
#if false
				char?	NextChar = (i == (text.Length - 1)) ? (char?)null : text[i + 1];
#endif

				switch (State) {
				case FsmState.Start:
					Process_Start(Parms, c, ref State);
					break;
				case FsmState.InsideNormalField:
					Process_NormalField(Parms, c, ref State);
					break;
				case FsmState.InsideQuotedField:
					int SkipCount = Process_QuotedField(Parms, c, sp, ref State, SupportQuoteCommaQuote);
					i += SkipCount;
					break;
				// Currently no other states
				}
			}

			// OK, there'll be one more field stuck in the buffer. Add it in
			Parms.AddCurrentFieldToValues();
		}

//---------------------------------------------------------------------------------------

		private static void Process_Start(TryParseParms Parms, char c, ref FsmState State) {
			switch (c) {
			case '"':
				State = FsmState.InsideQuotedField;
				break;
			case ',':
				Parms.AddCurrentFieldToValues();
				// Stay in start state
				break;
			default:
				Parms.AddCharToCurrentField(c);
				State = FsmState.InsideNormalField;
				break;
			}
		}

//---------------------------------------------------------------------------------------

		private static void Process_NormalField(TryParseParms Parms, char c, ref FsmState State) {
			switch (c) {
			case ',':				// End of field
				Parms.AddCurrentFieldToValues();
				State = FsmState.Start;
				break;
			// Note: The default case includes a double-quote character. 
			// A double-quote inside a normal field is really an error, but we'll
			// (sigh) accept it. Note that the string <""> inside a normal field is
			// considered as two double-quotes, *not* as the escape sequence for a
			// single double-quote as we'd have in a quoted field.
			default:
				Parms.AddCharToCurrentField(c);
				// Stay in current state
				break;
			}
		}

//---------------------------------------------------------------------------------------

		private static int Process_QuotedField(
								TryParseParms	Parms, 
								char			c, 
								StringPosition	sp, 
								ref				FsmState State, 
								bool			SupportQuoteCommaQuote) {
			char?	NextChar = sp.LA(1);
			switch (c) {
			case '"':
				// This could be the close of the last field in the string. Check for
				// that.
				if (!NextChar.HasValue) {
					return 0;				// Very simple, just return.
				}
				// OK, here comes the above-mentioned April 2008 kludge. Look for next
				// 3 characters to see if they're <",">.
				if (SupportQuoteCommaQuote) {
					if ((sp.LA(1) == '"') && (sp.LA(2) == ',') && (sp.LA(3) == '"')) {
						Parms.AddCharToCurrentField('"');
						State = FsmState.Start;
						return 1;
					}
				}

				// OK, it's not the end of the string, or the kludge. So is it a 
				// double-double-quote escape sequence for a single-double-quote
				// (got that?).
				if (NextChar == '"') {
					Parms.AddCharToCurrentField('"');
					return 1;				// Don't process second '"'
				}
				// OK, it's not EOS or a <""> sequence. If the next character is a comma,
				// then the field is done. If the next character is *not* a comma, then
				// (grumble, grumble) add the double-quote as a data character.
				if (NextChar != ',') {
					Parms.AddCharToCurrentField(c);
					return 0;
				}
				// OK, finally. It's a <"," sequence. 
				Parms.AddCurrentFieldToValues();
				State = FsmState.Start;
				return 1;
			// Note: The default case includes a comma. Which is fine. Inside a quoted
			//		 field a comma is just one more data character.
			default:
				Parms.AddCharToCurrentField(c);
				// Stay in current state
				return 0;
			}
		}


//---------------------------------------------------------------------------------------

		/// <summary>
		/// Given a List of strings, glue them together into a single CSV string.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		///
		public static string MakeCsvData(List<string> data) {
			StringBuilder sb = new StringBuilder();
			foreach (string item in data) {
				AddDataToCsv(sb, item);
			}
			return sb.ToString();
		}

//---------------------------------------------------------------------------------------

		static void AddDataToCsv(StringBuilder sb, string item) {
			if (sb.Length > 0) {			// No leading comma
				sb.Append(",");
			}
			if (item.Contains(",") || item.Contains("\"")) {
				// Excel saves a field like <O'Hara> as <O'Hara>. Duh. But note that the
				// single quote doesn't require special processing. But change the data
				// to <O"Hara>, and Excel exports that as <"O""Hara">, with opening and
				// closing quotes, and any internal quotes doubled.
				sb.Append("\"" + item.Replace("\"", "\"\"") + "\"");
			} else {
				sb.Append(item);
			}
		}
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------


	// LRSTODO: Sorry Ahmed, but this doesn't work for quoted data. Delete.
	// LRSTODO: Update the copy of this in LeadsLightning as well.
#if false		// BartCSVExtended
    // Ahmed -- Prefer to use the regular expression I have been using in LL since I
	// haven't seen any problem with it. Besides I don't understand why Larry put all
	// this methods and overhead to just parse a line in csv format.
    public static class zzzBartCSVExtended {
        // Regular Expression for parsing a CSV file
        public const string REGULAR_EXP_PARSE_CSV_FILE = ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))";
        
        internal static void Parse(string line, out List<string> lineValues) {
            Regex r = new Regex(REGULAR_EXP_PARSE_CSV_FILE);
            string[] lineFields = r.Split(line);
            
            lineValues = new List<string>();
            lineValues.InsertRange(0, lineFields);            
        }

//---------------------------------------------------------------------------------------

        internal static void Parse(string line, out string[] lineFields) {
            Regex r = new Regex(REGULAR_EXP_PARSE_CSV_FILE);
            lineFields = r.Split(line);            
        }
    }
#endif


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------


	internal class StringPosition {
		internal string		s;
		internal int		pos;

//---------------------------------------------------------------------------------------

		internal StringPosition(string s, int pos) {
			this.s = s;
			SetPos(pos);
		}

//---------------------------------------------------------------------------------------

		internal StringPosition(string s)
			: this(s, 0) {
		}

//---------------------------------------------------------------------------------------

		internal void SetPos(int Position) {
			if ((Position >= s.Length) || (Position < 0)) {
				throw new ArgumentException("SetPos: value outside range (" + Position + ")", "Position");
			}
			this.pos = Position;
		}

//---------------------------------------------------------------------------------------

		internal void IncPos(int n) {
			pos += n;
		}

//---------------------------------------------------------------------------------------

		internal char? LA(int n) {		// LA = LookAhead
			if ((pos + n) >= s.Length) {
				return null;
			}
			return s[pos + n];
		}
	}
}
