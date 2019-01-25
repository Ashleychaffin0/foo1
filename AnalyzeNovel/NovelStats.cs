using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AnalyzeNovel {
	public class NovelStats {
		public List<KeyValuePair<string, int>>	SortedWords;
		public List<KeyValuePair<int, int>>		SortedLengths;
		public List<KeyValuePair<int, int>>		SortedSentenceLengths;

		string	 FullFilename;        // Not currently needed, but keep around
		string[] NovelLines;

		// Here's a small but valid example of why fields shouldn't be global (or at
		// least visible) by default. This class needs a list of "small" words. But if
		// someone else wanted a list of small words, should s/he just grab this one?
		// No. This would constrain the current user (this class) from adding/deleting
		// words from the list, in case it messes up the second user. Meanwhile, the
		// second user shouldn't be tempted to edit the list of words, for the same
		// reason.

		// One example that comes to mind is that "As" is both a preposition and the
		// abbreviation for Arsenic. Removing it could well affect one program and
		// not the other, but adding it could do the same.

		// If you need a globally valid list of small words, then the team must all
		// agree that this is the list and can only be changed after all approve.
		// Alternatively, provide a way to return a copy of the list of small words,
		// then the caller can do with it whatever s/he likes. And perhaps add a way
		// to present this class with a custom list of small words.
		static string SmallWordsString =
			"A AND AS AT BE BY FOR IN HAS HAD IS IT NOT OF ON THAT THE TO";
		HashSet<string> SmallWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		static readonly char[] blank = new char[] { ' ' };  // Utility vals. Alloc once.
		static readonly char[] SentenceEnders = new char[] { '.', '?', '!' };

//---------------------------------------------------------------------------------------

		public NovelStats(string Filename) {
			var words = SmallWordsString.Split(blank, StringSplitOptions.RemoveEmptyEntries);
			foreach (var word in words) {
				SmallWords.Add(word);
			}

			FullFilename = Filename;
			NovelLines   = File.ReadAllLines(Filename);
		}

//---------------------------------------------------------------------------------------

		public void ProcessFile() {
			var AllWords        = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			var WordLengths     = new Dictionary<int, int>();
			var SentenceLengths = new Dictionary<int, int>();

			int nWordsSoFar               = 0;
			int ixEndOfMostRecentSentence = 0;
			int SentenceLength            = 0;

			// Go through each line in the file and find all words, the number of times
			// they each occur, ditto for sentences.
			foreach (string line in NovelLines) {
				var words = line.Split(blank, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < words.Length; i++) {
					++nWordsSoFar;
					string word = Clean(words[i]);
					if (IsEndOfSentence(ref word)) {
						SentenceLength = nWordsSoFar - ixEndOfMostRecentSentence;
						SentenceLengths.TryGetValue(SentenceLength, out int SentenceLengthCount);
						SentenceLengths[SentenceLength] = ++SentenceLengthCount;
						ixEndOfMostRecentSentence       = nWordsSoFar;
					}
					if (IsSmallWord(word))
						continue;

					AllWords.TryGetValue(word, out int count);  // Count == 0 if not found
					AllWords[word] = ++count;

					int WordLength = word.Length;
					WordLengths.TryGetValue(WordLength, out int LengthCount);
					WordLengths[WordLength] = ++LengthCount;
				}
			}

			// The following code is courtesy of LINQ, the Language Integrated Query
			// feature added to C# and VB languages in release 3. It's designed to
			// be similar (although hardly identical) to SQL, the Structured Query
			// Language used by relational database systems.

			SortedWords   = (from entry in AllWords    orderby entry.Value descending select entry).ToList();
			SortedLengths = (from entry in WordLengths orderby entry.Value descending select entry).ToList();
			SortedSentenceLengths = (from entry in SentenceLengths orderby entry.Value descending select entry).ToList();
		}

//---------------------------------------------------------------------------------------

		private bool IsEndOfSentence(ref string word) {
			bool bEnd = word.IndexOfAny(SentenceEnders) >= 0;
			// Now finish cleaning the word
			// Strictly speaking this next line has a potential bug. What if we add
			// another SentenceEnders character? Maybe ';'? We should go through a
			// loop over each end-of-sentence character, deleting it from <word>.
			word = word.Replace(".", "").Replace("?", "").Replace("!", "");
			return bEnd;
		}

//---------------------------------------------------------------------------------------

		private string Clean(string word) {
			// Note: I've seen instances of formatting issues in the source that
			//		 confuse matters. For example, <Darcy:--but>. This routine
			//		 would "clean" that into the single word "Darcybut" that
			//		 is obviously wrong. So to do a proper job we'd need a
			//		 more sophisticated parsing/cleaning routine. But it's not
			//		 needed for this simplie analysis program.
			word = word
				.Replace(",", "")
				.Replace("\"", "")
				.Replace("”", "")
				.Replace("'", "")
				.Replace("_", "")
				.Replace("--", "")
				.Replace(":", "")
				.Replace(";", "")
				;
			return word;
			//				var IsNormal = word.All<char>(p => char.IsLetterOrDigit(p));
			//				return IsNormal;
		}

//---------------------------------------------------------------------------------------

		private bool IsSmallWord(string word) => (word.Length <= 1) || (SmallWords.Contains(word));

//---------------------------------------------------------------------------------------

		// TODO: I really hate to have a dependency on System.Drawing for the PointF
		//		 struct. I really should have used KeyValuePair or an array of tuples.
		public bool linreg(PointF[] Points,
				out float m, out float b, out float r) {
			float sumx  = 0.0f;                     // sum of x    
			float sumx2 = 0.0f;                     // sum of x**2 
			float sumxy = 0.0f;                     // sum of x * y
			float sumy  = 0.0f;                     // sum of y    
			float sumy2 = 0.0f;                     // sum of y**2 

			float Sqr(float w) => w * w;			// Local method

			int n = Points.Length;					// # of points
			for (int i = 0; i < n; i++) {
				sumx  += Points[i].X;
				sumx2 += Sqr(Points[i].X);
				sumxy += Points[i].X * Points[i].Y;
				sumy  += Points[i].Y;
				sumy2 += Sqr(Points[i].Y);
			}

			float denom = (n * sumx2 - Sqr(sumx));
			if (denom == 0) {
				// Singular matrix. can't solve the problem.
				m = 0;
				b = 0;
				r = 0;
				return false;
			}

			m = (n * sumxy - sumx * sumy) / denom;
			b = (sumy * sumx2 - sumx * sumxy) / denom;
			r = (sumxy - sumx * sumy / n) /    // compute correlation coeff
					(float)Math.Sqrt((sumx2 - Sqr(sumx) / n) *
					(sumy2 - Sqr(sumy) / n));

			return true;
		}
	}

}
