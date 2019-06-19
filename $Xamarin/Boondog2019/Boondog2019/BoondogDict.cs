using System;
using System.Collections.Generic;
using System.IO;

#if true
// Copyright (c) 2014 by Larry Smith

// TODO: Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591

// using System;
// using System.Collections.Generic;
// using System.IO;

namespace nsBoondog2019 {
	/// <summary>
	/// Supports finding words in either the program or the user dictionary
	/// </summary>
	public class BoondogDict {
		List<string> GameDict2;

		string       DictFilename;              // So we can rewrite the user dictionary

		// Column 1 of each line in the file has either a '_' (for a normal word) or
		// 'S' for a word (e.g. "DRESS") that isn't a plural. I suppose later we could
		// go through the entire 32K words file and flag (e.g. '+') those words ("OVA",
		// "APOCHRYPHA", etc) that are plurals but don't end in "S". Yeah, sure.

		// Note: I've run across the occasional word (AEROBICS) that seems like a plural
		//		 (since AEROBIC is a word). But it's a collective, so I'm going to mark
		//		 up AEROBICS as a not-plural word. Then again, "ARMS" is ambiguous; it
		//		 can be both singular and (collective) plural. I guess to be consistent I
		//		 have to give it the benefit of the doubt. Not plural it is. So is, sigh,
		//		 ARTS.
		// Note: We're not just filtering on simple -S plurals. Consider BANJO / BANJOES.
		//		 That's a plural. Third person singular doesn't get the nod. MATCHES.
		//		 Also BURIES.
		// Note: And some are just a coin toss. There is such a thing as a single BIFOCAL
		//		 and two of them are a pair of BIFOCALS. But unless you were an optician,
		//		 you wouldn't normally point to the individual lenses and talk about them
		//		 as individual BIFOCALS. So BIFOCALS gets the nod. I guess.
		public const char IsNormalWord = '_';
		public const char EndsInSButIsNotPlural = 'S';      // e.g. DRESS
		List<char>   Tags;

//---------------------------------------------------------------------------------------

		public BoondogDict() {
			// Only for XML serialization
		}

//---------------------------------------------------------------------------------------

		public BoondogDict(string DictName) {
			// TODO: Can't default this, since we now use this class for both dicts. Use
			//		 an enum so we can estimate the file sizes in the next cupla lines.
			// TODO: After we binary-ize the files, make sure it's BOGMAST.TEXTWDS
			//		 and BOGUSER.TEXTWORDS. Or something like that.

			DictFilename = DictName;

			GameDict2 = new List<string>(32774);    // # of words in file - 2014/2/27
			Tags = new List<char>(32774);           // # of words in file - 2014/2/27

			LoadDictFromDirectory(DictFilename);

			// The current dictionary file is sorted. Keep it that way!

			// TODO: I suppose we can check as we're reading in the file, and return
			//		 some kind of error flag if it's not. Or maybe just a warning and
			//		 sort and rewrite the file. Whatever.
			// TODO: When we're reading in the file, make sure that column 1 is one of
			//		 the tags we recognize
		}

//---------------------------------------------------------------------------------------

		public BoondogDict(string dictName, string[] dictContents) {
			// TODO: Make next few lines a common routine
			DictFilename = dictName;

			GameDict2 = new List<string>(dictContents.Length);
			Tags = new List<char>(dictContents.Length);

			for (int i = 1; i < dictContents.Length; i++) { // Skip first line
				ProcessLine(dictContents[i]);
			}
		}

//---------------------------------------------------------------------------------------

		private void LoadDictFromDirectory(string DictFilename) {
			if (!File.Exists(DictFilename)) {
				DictFilename = "..\\..\\" + DictFilename;       // During debugging
			}

			// I don't want to put a reference to Winforms in here via a MessageBox. So
			// if we can't find the file, at least for now we'll bomb. 
			// TODO: Set some kind of class flag in the ctor and have the caller check
			//		 it. Or, I suppose throw a FileNotFound exception

			int LineNo = 0;
			using (var sr = new StreamReader(DictFilename)) {
				string line;
				while ((line = sr.ReadLine()) != null) {
					if (0 == LineNo++) {
						if (line != "$1") {
							System.Diagnostics.Debugger.Break();
							throw new Exception("Program Dictionary (" + DictFilename + ") is invalid (does not start with \"$1\")");
						}
						continue;           // Ignore 1st line
					}
					line = ProcessLine(line);
				}
			}
		}

//---------------------------------------------------------------------------------------

		private string ProcessLine(string line) {
			char tag = line[0];
			Tags.Add(tag);
			line = line.Substring(1);
			GameDict2.Add(line);
			return line;
		}

#if false
//---------------------------------------------------------------------------------------

		private void Check(char tag, string line) {
			if (line.CompareTo("DABBERS") > 0) { 
				if ((line.EndsWith("S")) && (tag != 'S')) {
					bool bMore; 
					char TagType;
					string word = line.Substring(0, line.Length - 1);
					bool bOK = FindWord(word, out bMore, out TagType);
					if (! bOK) {
						int i = 1;
					}
				}
			}
		}
#endif

//---------------------------------------------------------------------------------------

		public bool FindWord(BogWord bw, out bool bMorePossibleWords) {
			string word = bw.Text;
			int n = GameDict2.BinarySearch(word);
			// According to the documentation for List<>.BinarySearch, if the word was
			// found, <n> will be >= 0. If not found it will be negative and will be
			// the -ve of the index of the next larger word. Careful if <word> is larger
			// than the very last word in the dictionary.r

			string NextWord;

			if (n > 0) {
				bw.Tag = Tags[n];
				if (n == GameDict2.Count - 1) {
					// Last word in dictionary, can't be any following
					bMorePossibleWords = false;
					return true;
				}
				NextWord = GameDict2[n + 1];
				bMorePossibleWords = NextWord.StartsWith(word);
				return true;
			}

			if (n <= -GameDict2.Count) {
				// Past the last word. Can't be any following it
				bMorePossibleWords = false;
				return false;
			}

			// Not a valid word, but adding another letter or so may be valid.
			NextWord = GameDict2[-n];
			bMorePossibleWords = NextWord.StartsWith(word);
			return false;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Add a word to the internal user dictionary. Also write it back out 
		/// immediately. Make sure we maintain the dictionary in sorted order so we can
		/// do a binary search.
		/// </summary>
		/// <param name="Word"></param>
		public void AddWordToDictionary(BogWord Word) {
			// Note: I *could* do a binary search and insert the word (and also the tag).
			//		 But this seems to run fast enough, so I think I'll keep the code
			//		 simple and just re-sort the List<>, then search it to find where to
			//		 update the TagType. If I ever find that this is too slow, I can come
			//		 back to it and do it a bit more efficiently.
			string text = Word.Text;
			GameDict2.Add(text);

			// TODO: Get rid of <Tags> variable and use a custom comparison routine
			//		 to sort a List<BogWord>.

			GameDict2.Sort();
			int n = GameDict2.BinarySearch(text);
			Tags.Insert(n, Word.Tag);

			WriteOutUserDict();
		}

//---------------------------------------------------------------------------------------

		private void WriteOutUserDict() {
			string BakName = DictFilename + ".BAK";
			File.Delete(BakName);
			File.Move(DictFilename, BakName);
			using (var sw = new StreamWriter(DictFilename, false)) {
				sw.WriteLine("$1");
				for (int i = 0; i < GameDict2.Count; i++) {
					sw.Write(Tags[i]);
					sw.WriteLine(GameDict2[i]);
				}
			}
		}
	}
}
#endif