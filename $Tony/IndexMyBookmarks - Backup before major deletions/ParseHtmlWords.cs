using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace IndexMyBookmarks {
	class ParseHtmlWords {
		static char[] blank = new char[] { ' ' };

		// Note: Because the next two fields are used only to see if a word has been
		// seen before (i.e. maps a string into a bool), we could have simplified
		// things, just a bit, by using a HashSet instead of a Dicitonary. I chose a
		// Dictionary just to show how associative arrays can work, even though
		// would be marginally faster HashSet.
		static Dictionary<string, bool> StopWords	   = new Dictionary<string, bool>();
		static Dictionary<string, bool> SpecialStrings = new Dictionary<string, bool>();

		// I've seen cases where Sqlite treats "1234e567" as (it seems) a floating point
		// number, which is invalid since the exponent is too large. But amazingly,
		// the error I get is, in effect, "I've already seen this number before", since
		// it violates the UNIQUE criterion in tblWords. Stupid Sqlite. I suppose I
		// should report this as a bug (bad floating point should be treated as a string
		// instead of treating it as (perhaps) a zero or maybe NaN. But since it seems
		// likely that this is a hex value (perhaps part of a GUID), let's see if we can
		// recover from this ourselves.
		static Regex reGuid;

		public HashSet<string> Words;

//---------------------------------------------------------------------------------------

		/// <summary>
		/// A static constructor is run once during program initialation, even
		/// before the Main() routine is called. It's to set up static fields in
		/// this class.
		/// </summary>
		static ParseHtmlWords() {
			ReadAuxStrings("StopWords.txt"   , StopWords);
			ReadAuxStrings("SpecialWords.txt", SpecialStrings);

			// 8, 4, 4, 4, and 12 digits
			string reGuidPattern = "[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}";
			reGuid = new Regex(reGuidPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// This routine is used to read in and populate both the StopWords and
		/// SpecialWords dictionaries
		/// </summary>
		/// <param name="filename">The name of the file to read</param>
		/// <param name="dict">The dictionary to populate</param>
		public static void ReadAuxStrings(string filename, Dictionary<string, bool> dict) {
			using (var sr = new StreamReader(filename)) {
				string line;
				while ((line = sr.ReadLine()) != null) {
					if (line.StartsWith("*")) { continue; } // Comments
					var words = Split(line);
					foreach (var word in words) {
						dict[word.ToUpper()] = true;
					}
				}
			}
		}

//---------------------------------------------------------------------------------------

		public ParseHtmlWords() {
			Reset();
		}

//---------------------------------------------------------------------------------------

		private void Reset() {
			Words = new HashSet<string>();
		}

//---------------------------------------------------------------------------------------

		public void ParseInnerText(string text) {
			text = text.Trim();
			if (text.Length == 0) { return; }
			IndexMyBookmarks.Main.Msg("ParseInnerText - " + text);
			text = ConvertToUtf8(text);
			text = text.ToUpper();      // This helps case-insensitive searches
			text = SplitOutSpecialStrings(text);
			text = AlphaNumOnlyPlease(text);
			string[] words = Split(text);
			foreach (var word in words) {
				if (!IsStopWord(word)) { Words.Add(word); }
			}
		}

//---------------------------------------------------------------------------------------

		public static string ConvertToUtf8(string text) {
			// Now convert special strings to UTF8 -- e.g. â€™ (a slanting	
			//	single quote -- ‘) => ' (a non-slanting quote), &amp; => &
			// See http://www.i18nqa.com/debug/utf8-debug.html
			// and https://stackoverflow.com/questions/10888040/how-to-convert-%C3%A2%E2%82%AC-to-apostrophe-in-c

			// A very nice description can be found at
			//	https://www.joelonsoftware.com/2003/10/08/the-absolute-minimum-every-software-developer-absolutely-positively-must-know-about-unicode-and-character-sets-no-excuses/
			text      = HttpUtility.HtmlDecode(text);
			var bytes = Encoding.Default.GetBytes(text);
			text      = Encoding.UTF8.GetString(bytes);
			return text;
		}

//---------------------------------------------------------------------------------------

		public static string AlphaNumOnlyPlease(string text) {
			var sb = new StringBuilder();
			foreach (char c in text) {
				if (char.IsLetterOrDigit(c)) {
					sb.Append(c);
				} else {
					sb.Append(' ');
				}
			}
			return sb.ToString();
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Scan the input string looking for "words" that contain special characters
		/// that will be removed during subsequent processing. These might include
		/// "C#", "C++", etc. Note that these special strings come from a user
		/// editable file. If we find any special words, we'll immediately place them
		/// into our "Words" List<> and edit them out of our input string.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		internal string SplitOutSpecialStrings(string s) {
			// Some words we might be called upon to index might have non-alphameric
			// characters in their names. For example, C# and C++. Which give us grief.
			// If we change all non-alphameric chars to blanks and then .split() them,
			// we lose the # and ++ and so on. 

			// And we might even want to match single letters, as in the programming
			// lanuage C, or be able to index the D in Bach's Tocatta and Fugue in 
			// D minor. But heaven help us if we did special processing for sCience 
			// or aDult!

			// Finally I've found what I assume to be a bug in that stupid Sqlite. I've
			// seen strings like 1234e567. Presumably this was part of a GUID. But
			// Sqlite seems to interpret it as a floating point number, given the "e"
			// among the numerals. But an exponent of 567 is too large. So instead of
			// just treating it as a string, it does something (I don't know what)
			// like saying it's 0. Which results in the UNIQUE constraint in the field
			// I want to put it in to be violated. Bottom line -- we'll handle GUIDs
			// specially and see if that helps.

			foreach (var key in SpecialStrings.Keys) {
				s = HandleSpecialStrings(s, key);
			}

			// Handle GUIDs
			var match = reGuid.Match(s);
			if (match.Success) {
				foreach (Match cap in match.Captures) {
					string val = cap.Value;
					s = HandleSpecialStrings(s, val);
				}
			}
			return s;
		}

//---------------------------------------------------------------------------------------

		private string HandleSpecialStrings(string s, string key) {
			int ix = 0;
			while (ix + 1 < s.Length) {
				ix = s.IndexOf(key, ix + 1);
				if (ix < 0) { break; }
				// Look at the characters just to the left and right of the key. If
				// either or both are non-alphameric, then the key is one of our special
				// words.
				// Careful about index errors, though.
				bool bOK = true;
				if (ix > 0) { bOK = !char.IsLetterOrDigit(s[ix - 1]); } // Left char
				if (bOK && (ix + key.Length < s.Length)) {
					bOK = !char.IsLetterOrDigit(s[ix + key.Length]);    // Right char
				}
				if (bOK) {
					Words.Add(s.Substring(ix, key.Length));
					s = s.Substring(0, ix) + s.Substring(ix + key.Length);
				}
			}
			return s;
		}

//---------------------------------------------------------------------------------------

		private static string[] Split(string text) {
			string[] words = text.Split(blank, StringSplitOptions.RemoveEmptyEntries);
			return words;
		}

//---------------------------------------------------------------------------------------

		private static bool IsSpecialWord(string word) {
			return SpecialStrings.TryGetValue(word.ToUpper(), out bool NotUsed);
		}

//---------------------------------------------------------------------------------------

		private static bool IsStopWord(string word) {
			return StopWords.TryGetValue(word.ToUpper(), out bool NotUsed);
		}

//---------------------------------------------------------------------------------------

		internal static bool IsGuid(string word) {
			var match = reGuid.Match(word);
			return match.Success;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Insert heuristic code here to map, say, dogs => dog, blended => blend,
		/// feeling => feel, and so on.
		/// But be careful. "dress" does not map to "dres", "shed" doesn't map to
		/// "sh" and neither does "bored" map to "bor". And "bring" doesn't map to
		/// "br". So for now, just return the word. But here's your hook if you want
		/// to try to try to figure out a good way to do this.
		/// </summary>
		/// <param name="word"></param>
		/// <returns></returns>
		public static string BaseWord(string word) {
			return word;
		}
	}
}
