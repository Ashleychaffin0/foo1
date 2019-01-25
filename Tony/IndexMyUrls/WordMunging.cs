using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace IndexMyBookmarks {
	static class WordMunging {
		static char[] blank                          = new char[] { ' ' };

		// Note: Because the next two fields are used only to see if a word has been
		// seen before (i.e. maps a string into a bool), we could have simplified things,
		// just a bit, by using a Hashtable instead of a Dicitonary. I chose a
		// Dictionary just to show how associative arrays can work.
		static Dictionary<string, bool> StopWords    = new Dictionary<string, bool>();
		static Dictionary<string, bool> SpecialWords = new Dictionary<string, bool>();

//---------------------------------------------------------------------------------------

			/// <summary>
			/// A static constructor is run once during program initialation, even
			/// before the Main() routine is called. It's to set up static fields in
			/// this class.
			/// </summary>
		static WordMunging() {
			ReadAuxStrings("StopWords.txt", StopWords);
			ReadAuxStrings("SpecialWords.txt", SpecialWords);
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

			/// <summary>
			/// Take a string, s, which may well be the inner text (i.e. without tags)
			/// of an entire web page, and pick out just the words (blank delimited) and
			/// return a List<string> of them. Can you say "heuristics"?
			/// </summary>
			/// <param name="s"></param>
			/// <returns></returns>
		public static List<string> ParseAndCleanString(string s) {
			s = SplitOutSpecialWords(s); // Handle things like "C#" specially
			// OK, split the string into words, clean them, and put them into a List
			var words = Split(s.ToUpper());
			var CleanedWords = new List<string>();
			foreach (var word in words) {
				if (IsSpecialWord(word)) {	// Leave special words (e.g. "C#") as-is
					CleanedWords.Add(word);
					continue;
				}
				if (IsStopWord(word)) { continue; }	// Ignore it
				// A word may be, say, "READ/EVAL/PRINT", which after replacing the
				// special characters with blanks would be "READ EVAL PRINT". So
				// the CleanString() method must return a list of 3 words
				foreach (var SingleWord in CleanString(word)) {
					if (SingleWord.Length > 0) {
#if DEBUG
						// We've had bugs whereby even after cleaning, words still came
						// back with trailing blanks/newlines. Try to catch that here
						if (!IsValidateWord(SingleWord)) {
							if (!DateTime.TryParse(SingleWord, out DateTime Unused)) {
								Debugger.Break();
							}
						}
#endif
						// if (SingleWord.Contains("OUR")) Debugger.Break();	// TODO:
						CleanedWords.Add(SingleWord);
					}
				}
			}
			return CleanedWords;
		}

//---------------------------------------------------------------------------------------

		public static string ConvertToUtf8(string text) {
			text = WebUtility.HtmlDecode(text); // e.g. &amp; => &

			// Now convert special strings to UTF8 -- e.g. â€™ (a slanting	
			//	single quote -- ‘) => ' (a non-slanting quote)
			// See http://www.i18nqa.com/debug/utf8-debug.html
			// and https://stackoverflow.com/questions/10888040/how-to-convert-%C3%A2%E2%82%AC-to-apostrophe-in-c

			// A very nice description can be found at
			//	https://www.joelonsoftware.com/2003/10/08/the-absolute-minimum-every-software-developer-absolutely-positively-must-know-about-unicode-and-character-sets-no-excuses/
			var bytes = Encoding.Default.GetBytes(text);
			text      = Encoding.UTF8.GetString(bytes);
			return text;
		}

//---------------------------------------------------------------------------------------

		public static bool IsValidateWord(string singleWord) {
			if (IsSpecialWord(singleWord)) { return true; }
			if (DateTime.TryParse(singleWord, out DateTime NotUsed)) { return true; }
			foreach (char c in singleWord) {
				if (!char.IsLetterOrDigit(c)) { return false; }
			}
			return true;
		}

//---------------------------------------------------------------------------------------

		private static string SplitOutSpecialWords(string s) {
			// Some words we might be called upon to index might have non-alphameric
			// characters in their names. For example, C# and C++. Which gives us grief.
			// If we change all non-alphameric chars to blanks and then .split() them,
			// we lose the # and ++ and so on. So we'll replace, say, "C#" with "C# ",
			// which will look like a separate word after the .split(). Then when we
			// process each word, we'll check each one to see if it's one of our special
			// ones and leave it alone, rather than replacing the non-alphameric chars
			// with blanks.
			foreach (var key in SpecialWords.Keys) {
				s = s.Replace(key, key + " ");
			}
			return s;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Take a string (word) and turn 
		/// </summary>
		/// <param name="word"></param>
		/// <returns></returns>
		private static IEnumerable<string> CleanString(string word) {
			// Handle a couple of special cases first
			if (IsSpecialWord(word)) { yield return word; }
			if (IsStopWord(word))   { yield return ""; }	// Ignore "an,the,..."
			if (DateTime.TryParse(word, out DateTime NotUsed)) { yield return word; }
			if ((word.Length == 1) && !char.IsNumber(word[0])) { // e.g. "S" from "JIM'S"
				yield return "";
			}	

			// Replace all non-alphameric characters with blanks
			var sb = new StringBuilder();
			foreach (char c in word.Trim()) {
				sb.Append(char.IsLetterOrDigit(c) ? c : ' ');
			}
			var NewWords = Split(sb.ToString());
			foreach (var item in NewWords) {
				yield return BaseWord(item);
			}
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

//---------------------------------------------------------------------------------------

		private static string[] Split(string text) {
			string[] words = text.Split(blank, StringSplitOptions.RemoveEmptyEntries);
			return words;
		}

//---------------------------------------------------------------------------------------

		private static bool IsSpecialWord(string word) {
			return SpecialWords.TryGetValue(word.ToUpper(), out bool NotUsed);
		}

//---------------------------------------------------------------------------------------

		private static bool IsStopWord(string word) {
			return StopWords.TryGetValue(word.ToUpper(), out bool NotUsed);
		}
	}
}
