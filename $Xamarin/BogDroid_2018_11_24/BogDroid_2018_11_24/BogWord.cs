#if true
// Copyright (c) 2007-2014 by Larry Smith

// TODO: Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591

using System.Collections.Generic;
using System.Text;

using BogDroid_2018_11_24;

// Note: At once point I derived a new class, BogWord_ListboxEntry. This held additional
//		 fields that were only useful when the BogWord was in a listbox, such as the
//		 Highlight property. I later came to my senses, moved the new stuff back into
//		 the basic BogWord, and deleted the old class.

// TODO: Add new flag -- IsFromUserDictionary

// TODO: Add IsPluralException

// TODO: Whether a word is valid should be part of a BogWord. See ValidateWord 
//		 and IsValidWord

namespace BogDroid_2018_11_24 {
	/// <summary>
	/// A BogWord encapsulates the concept a word found on the board and contains a
	/// List(BogCube). The ToString() method will get the string for the word from the
	/// List. A BogWord is what lives in each of the two Listboxes.
	/// It is also used for things like recalling the previous word.
	/// </summary>
	public class BogWord {
		public List<BogCube>    CubeLetters;

		public char             Tag;
		// public bool				bIsProgramWord;

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Ctor
		/// </summary>
		public BogWord()
			: this(new List<BogCube>()) {
			// bIsProgramWord = false;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name              ="TrackCubes"></param>
		public BogWord(List<BogCube> TrackCubes) {
			CubeLetters              = new List<BogCube>();
			Tag                      = BoondogDict.IsNormalWord;
			_IsFromProgramDictionary = false;
			_IsHighlighted           = false;
			_Score                   = 0;

			foreach (var item in TrackCubes) {
				CubeLetters.Add(new BogCube(item));
				_Score += BogBoard.TileScores[item.Text];
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="word"></param>
		public BogWord(BogWord word) {
			CubeLetters = new List<BogCube>(word.CubeLetters);
			Tag = word.Tag;
			_IsFromProgramDictionary = word._IsFromProgramDictionary;
			_Score = word._Score;
			_IsHighlighted = word._IsHighlighted;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// The word ends in 'S', but is not a plural (e.g. ABACUS)
		/// </summary>
		public bool IsPluralValid => Tag == BoondogDict.EndsInSButIsNotPlural;

//---------------------------------------------------------------------------------------

		int _Score;
		public int Score {
			get { return _Score; }
		}

//---------------------------------------------------------------------------------------

		bool    _IsHighlighted;
		public bool IsHighlighted {
			get { return _IsHighlighted; }
			set { _IsHighlighted = value; }
		}

//---------------------------------------------------------------------------------------

		private bool _IsFromProgramDictionary;
		public bool IsFromProgramDictionary {
			get { return _IsFromProgramDictionary; }
			set { _IsFromProgramDictionary = value; }
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Empties the ListOf BogCubes in this intance
		/// </summary>
		public void Clear() {
			CubeLetters.Clear();
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Appends a letter (or a string, "Qu") via a BogCube to the current word
		/// </summary>
		/// <param name="bc">The cube (string) to append. It will be converted to upper
		///  case, just in case (for Qu support)
		/// </param>
		public void AppendCube(BogCube bc) {
			CubeLetters.Add(bc);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Returns the word corresponding to this BogWord
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			var buf = new StringBuilder();
			foreach (var cube in CubeLetters) {
				buf.Append(cube.Text.ToUpper());
			}
			return buf.ToString();
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Gets the last (rightmost) cube in the word
		/// </summary>
		/// <returns></returns>
		public BogCube LastCube() {
			return CubeLetters[Count - 1];
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Used by the Backspace routine to delete the rightmost letter in the List
		/// </summary>
		public void DeleteLastCube() {
			CubeLetters.RemoveAt(Count - 1);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// The string version of the BogWord
		/// </summary>
		public string Text {
			get { return ToString(); }
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// The number of cubes in this BogWord. In general it is *not* the length of
		/// the word. "Quote" has 4 cubes, but 5 letters
		/// </summary>
		public int Count {
			get { return CubeLetters.Count; }
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// The number of letters in this BogWord. In general it is *not* the number of
		/// cubes. "Quote" has 4 cubes, but 5 letters
		/// </summary>
		public int LetterCount {
			get { return Text.Length; }
		}

//---------------------------------------------------------------------------------------

		public static int CompareByText(BogWord x, BogWord y) {
			return x.Text.CompareTo(y.Text);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Compares two BogWords by word length, and by Text descending within word
		/// length
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static int CompareByWordLength(BogWord x, BogWord y) {
			var xText = x.Text;
			var yText = y.Text;
			// Note: We're comparing y against x. This gives us a descending sort
			int cmp = yText.Length.CompareTo(xText.Length);
			if (cmp == 0) {
				// But within a given word length, we want the words in ascending order
				cmp = xText.CompareTo(yText);
			}
			// return y.Text.Length.CompareTo(x.Text.Length);
			return cmp;
		}

//---------------------------------------------------------------------------------------

		public static int CompareByScore(BogWord x, BogWord y) {
			// TODO: Write this at some point. For now, just sort by Text
			return CompareByText(x, y);
		}
	}
}
#endif