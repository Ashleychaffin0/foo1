
using System.Collections.Generic;

using BogDroid_2019_03;
using Xamarin.Forms;

namespace BogDroid_2019_03 {

	/// <summary>
	/// The playing board. Note that this does *not* include most of the UI, mostly just
	/// the cubes. Everything else is owned by the main Boondog2014 class.
	/// </summary>
	public class BogBoard {
		public static BogCube[,] Cubes;
		public static Dictionary<string, int> TileScores;
		public BoondogDict   PgmDict;
		public BoondogDict   UserDict;
		public List<string>  PgmWords;
		public List<string>  UserWords;
		public PlayerProfile prof;

		public string        ErrorMessage;

		public BogBoard() {
			Cubes = new BogCube[,] {
				{ App.BogMain.Sq00, App.BogMain.Sq01, App.BogMain.Sq02, App.BogMain.Sq03, App.BogMain.Sq04 },
				{ App.BogMain.Sq10, App.BogMain.Sq11, App.BogMain.Sq12, App.BogMain.Sq13, App.BogMain.Sq14 },
				{ App.BogMain.Sq20, App.BogMain.Sq21, App.BogMain.Sq22, App.BogMain.Sq23, App.BogMain.Sq24 },
				{ App.BogMain.Sq30, App.BogMain.Sq31, App.BogMain.Sq32, App.BogMain.Sq33, App.BogMain.Sq34 },
				{ App.BogMain.Sq40, App.BogMain.Sq41, App.BogMain.Sq42, App.BogMain.Sq43, App.BogMain. Sq44 }
			};
			SetupTileScores();
		}

//---------------------------------------------------------------------------------------

		private void SetupTileScores() {
			TileScores = new Dictionary<string, int> {
				["A"] = 1,
				["B"] = 3,
				["C"] = 3,
				["D"] = 2,
				["E"] = 1,
				["F"] = 4,
				["G"] = 2,
				["H"] = 4,
				["I"] = 1,
				["J"] = 8,
				["K"] = 5,
				["L"] = 1,
				["M"] = 3,
				["N"] = 1,
				["O"] = 1,
				["P"] = 3,
				["Qu"] = 10,
				["QU"] = 10,
				["R"] = 1,
				["S"] = 1,
				["T"] = 1,
				["U"] = 1,
				["V"] = 4,
				["W"] = 4,
				["X"] = 8,
				["Y"] = 4,
				["Z"] = 10
			};

			// TODO: Add entires for new dipthongs
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Called from GenerateWord to add a word to the program's listbox
		/// </summary>
		/// <param name="NewWord"></param>
		public void ProgramAddWord(BogWord NewWord) {
			if (IsValidWord(true, NewWord)) {
				// TODO: Replace this with some kind of call to IsValidWord2
				// Note: The Addword routine will filter out cases where the same word might
				//		 have come from different paths
				NewWord.IsFromProgramDictionary = true;
				AddWordToListView(true, NewWord, false);
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// The routine that actually adds a word to the specified listbox
		/// </summary>
		/// <param name ="bIsProgram"></param>
		/// <param name ="NewWord"></param>
		/// <param name="bIsProgramWord"></param>
		/// <returns></returns>
		public bool AddWordToListView(bool bIsProgram, BogWord NewWord, bool bIsProgramWord) {
			// Note: This routine can be void instead of bool, but maybe some day we
			//		 might want to know, so leave it as bool
			string word = NewWord.Text;
			ListView lv;
			List<string>    wds;
			if (bIsProgram) {
				lv = App.BogMain.lvProgramWords;
				wds = PgmWords;
			} else {
				lv = App.BogMain.lvPlayerWords;
				wds = UserWords;
			}

			if (wds.Contains(word)) {
				if (bIsProgram) { return false; }
				App.BogMain.DisplayAlert("Boondoggle", "You've already used that word", "OK");
				return false;
			}
			BogWord bword = new BogWord(NewWord);
#if false
			var bwl = new BogWord_ListboxEntry(bword) {
				IsMostRecentWord = true
			};
			int nItem        = lv.Items.Add(bwl);   // TODO: TODO: TODO:
			lv.SelectedIndex = nItem;           // Make sure word is visible in listbox
#else
			lv.ItemsSource = null;      // TODO: Isn't there a better way to do this?
			wds.Add(word);
			wds.Sort();
			lv.ItemsSource = wds;
#endif
				// lb.SelectedIndex  = ListBox.NoMatches;	// Now get rid of highlight

#if false       // TODO: TODO: TODO: Must implement this
				if (bIsProgram) {
				++game.ProgramWords;
				FoundWords[bword.Text] = 0;         // Add word to dictionary
				game.ProgramScore += game.Score(word);
				// SetStats(lblProgramStats, game.ProgramWords, game.ProgramScore);
			} else {
				// int n;			// Needed for TryGetValue, but not used
				bool bOK = FoundWords.TryGetValue(bword.Text, out int n);
				if (bOK) {
					FoundWords.Remove(bword.Text);
					lblPgmUnfound.Text = string.Format("Unfound: {0}", FoundWords.Keys.Count);
				}
				++game.PlayerWords;
				game.PlayerScore += game.Score(word);
				SetStats(false);
				DisplayWordLengths();
				ListboxUtils.SetHighlight(lvPlayerWords, false);

				ListboxUtils.SortListBox(lvPlayerWords, true);
			}
#endif

			return true;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Looks up the specified BogWord in the specified dictionary
		/// </summary>
		/// <param name="dict"></param>
		/// <param name="Word"></param>
		/// <param name="bIsAcceptable"></param>
		/// <returns></returns>
		private bool IsValidWord2(BoondogDict dict, BogWord Word, out bool bIsAcceptable) {
			bIsAcceptable = false;
			bool bOK = dict.FindWord(Word, out bool bMorePossibleWords);
			if (!bOK) {
				return false;
			}

			// Word found in dictionary.
			if (prof.Allow_Ss) {
				// With this setting, *any* word in the dictionary is valid (even if it
				// doesn't end in S)
				bIsAcceptable = true;
				return true;
			}

			// Word found, and we care about whether it ends in S.
			if (!Word.Text.EndsWith("S")) {
				bIsAcceptable = true;
			} else {
				bIsAcceptable = Word.IsPluralValid;         // e.g. DRESS
			}
			return true;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Checks to see if word is valid, e.g. is long enough, doesn't end in 'S'
		/// unless allowed in the profile, etc.
		/// </summary>
		/// <param name="bIsProgramWord"></param>
		/// <param name="CurWord"></param>
		/// <returns>true if this is a valid word, else false</returns>
		private bool IsValidWord(bool bIsProgramWord, BogWord CurWord) {
			// TODO: TODO: TODO: Get rid of this
			PlayerProfile prof;
			if (bIsProgramWord) {
				prof = App.BogForm.ProgramProf;
			} else {
				prof = App.BogForm.PlayerProf;
			}

			if (CurWord.LetterCount < prof.MinWordLength) {
				if (!bIsProgramWord) {
					string msg = string.Format("The word must be at least {0} letters long", App.BogForm.PlayerProf.MinWordLength);
					App.BogMain.DisplayActionSheet("Boondoggle", msg, "OK");
					// MessageBox.Show(msg, "Boondoggle", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				return false;
			}

			if ((!prof.Allow_Ss) && CurWord.Text.EndsWith("S")) {
				// Check for exception
				if (CurWord.IsPluralValid) {
					return true;
				}
				if (!bIsProgramWord) {
					string msg = "The word may not end in 'S'";
					App.BogMain.DisplayActionSheet("Boondoggle", msg, "OK");
					//MessageBox.Show(msg, "Boondoggle", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				return false;
			}

			return true;
		}
	}
}




#if false
// Copyright (c) 2007-2014 by Larry Smith

// TODO: Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Threading;

using Xamarin.Forms;

namespace BogDroid_2019_03 {

	/// <summary>
	/// The playing board. Note that this does *not* include most of the UI, mostly just
	/// the cubes. Everything else is owned by the main Boondog2014 class.
	/// </summary>
	public partial class Board {

		Boondog2014             BogForm;

		PlayerProfile           prof;                   // TODO: Move to BogForm

		Game                    game;                   // TODO: Move to BogForm

		BogCube[,]              Cubes;

		public BoondogDict      ProgramDict;            // TODO: Move to BogForm

		Point[,]                Locations;

		public static Dictionary<string, int> TileScores;

		/// <summary>
		/// The word (so far) that the user is in the process of entering (or, 
		/// technically, the word just entered)
		/// </summary>
		public BogWord          CurrentWord;

		/// <summary>
		/// The last word the player formed (re recall purposes)
		/// </summary>
		public BogWord          LastWord;

		private BoardOrientation Orientation;

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Track how the board has been rotated. By definition, the initial orientation
		/// is North. Rotating the board clockwise brings West, then South, the East,
		/// then North again. Rotation in the other direction works in reverse.
		/// </summary>
		public enum BoardOrientation {
			First,
			North,
			East,
			South,
			West,
			Last                // If we get here, we know we have to restart
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="BogForm"></param>
		/// <param name="game"></param>
		/// <param name="BogBox"></param>
		/// <param name="prof"></param>
		/// <param name="dict"></param>
		public Board(Boondog2014 BogForm, Game game, GroupBox BogBox, PlayerProfile prof, BoondogDict dict) {
			this.BogForm = BogForm;
			// TODO: Gets these through BogForm
			this.game = game;
			this.prof = prof;
			this.ProgramDict = dict;
			this.Orientation = BoardOrientation.North;

			Locations = new Point[prof.Rank, prof.Rank];
			CurrentWord = new BogWord();

			BogDice.SetupDice(prof);
			SetupTileScores();
			AllocateCubes();
			Resize(BogBox);                     // TODO: Move BogBox to BogForm
			BogBox.SendToBack();
		}

//---------------------------------------------------------------------------------------

		private void SetupTileScores() {
			TileScores = new Dictionary<string, int> {
				["A"] = 1,
				["B"] = 3,
				["C"] = 3,
				["D"] = 2,
				["E"] = 1,
				["F"] = 4,
				["G"] = 2,
				["H"] = 4,
				["I"] = 1,
				["J"] = 8,
				["K"] = 5,
				["L"] = 1,
				["M"] = 3,
				["N"] = 1,
				["O"] = 1,
				["P"] = 3,
				["Qu"] = 10,
				["QU"] = 10,
				["R"] = 1,
				["S"] = 1,
				["T"] = 1,
				["U"] = 1,
				["V"] = 4,
				["W"] = 4,
				["X"] = 8,
				["Y"] = 4,
				["Z"] = 10
			};

			// TODO: Add entires for new dipthongs
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// This routine either (a) recalls the previous word if there are no letters
		/// currently selected, or (b) drops one letter off the end of the current word
		/// </summary>
		public void Backspace() {
			// TODO: Must be called from right-clicking on the background
			if (CurrentWord.Count == 0) {
				return;
			}
			var cube = CurrentWord.LastCube();
			cube.Reset();
			CurrentWord.DeleteLastCube();
			if (CurrentWord.Count == 0) {
				BogForm.DeselectAllPlayerWords();
			}
			BogForm.ShowWordSoFar(CurrentWord);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// The player has indicated (e.g. by double-clicking) that he's finished
		/// forming his word. Check it against the main dictionary, the user dictionary,
		/// etc. before adding it.
		/// </summary>
		public void EndPlayerWord() {
			// The logic in this routine isn't as straightforward as one might like.
			// Look it up in one dictionary and if it's not there try the other. And if
			// all else fails, look for it on the web.
			//		WRONG!
			// It's those damned "plurals". Has the user set the No 'S option on? And if
			// so, even if the word is in a dictionary, we may or may not want to allow
			// it.

			if (CurrentWord.LetterCount < BogForm.PlayerProf.MinWordLength) {
				string msg = string.Format("The word must be at least {0} letters long", BogForm.PlayerProf.MinWordLength);
				// TODO: MessageBox.Show(msg, "Boondoggle", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			// TODO: Put in try/finally, with NewWord() in finally clause
			if (IsValidWord2(ProgramDict, CurrentWord, out bool bIsAcceptable)) {
				// It's found in the main dictionary. Ah, but is it acceptable?
				if (bIsAcceptable) {
					CurrentWord.IsFromProgramDictionary = true;
					BogForm.AddWordToListbox(false, CurrentWord, false);
				} else {
					string msg = "The word may not end in 'S'";
					// TODO: MessageBox.Show(msg, "Boondoggle", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				NewWord();
				return;
			}

			// It's not in the main dictionary,. Check User Dict
			if (IsValidWord2(BogForm.UserDict, CurrentWord, out bIsAcceptable)) {
				// It's found in the user dictionary. Ah, but is it acceptable?
				if (bIsAcceptable) {
					BogForm.AddWordToListbox(false, CurrentWord, true);
				}
				NewWord();
				return;
			}

			DialogResult res = BogForm.LookupWordOnWeb(CurrentWord);
			if (res == DialogResult.Yes) {
				BogForm.UserDict.AddWordToDictionary(CurrentWord);
				BogForm.AddWordToListbox(false, CurrentWord, true);
			}

#if false
			bool bOK = dict.FindWord(CurrentWord, out bMorePossibleWords);
			if (bOK && IsValidWord(false, CurrentWord)) {
				BogForm.AddWord(false, CurrentWord);
				NewWord();
				return;
			}

			// Not in main dictionary. Check user dict
			bOK = BogForm.UserDict.FindWord(CurrentWord, out bMorePossibleWords);
			if (bOK && IsValidWord(false, CurrentWord)) {
				BogForm.AddWord(false, CurrentWord);
				NewWord();
				return;
			}

			// Not in either dictionary. Check web
			DialogResult res = BogForm.LookupWordOnWeb(CurrentWord);
			if (res == DialogResult.Yes) {
				BogForm.UserDict.AddWord(CurrentWord);
				BogForm.AddWord(false, CurrentWord);
				NewWord();
				return;
			} 
#endif
			NewWord();
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Looks up the specified BogWord in the specified dictionary
		/// </summary>
		/// <param name="dict"></param>
		/// <param name="Word"></param>
		/// <param name="bIsAcceptable"></param>
		/// <returns></returns>
		bool IsValidWord2(BoondogDict dict, BogWord Word, out bool bIsAcceptable) {
			bIsAcceptable = false;
			bool bOK = dict.FindWord(Word, out bool bMorePossibleWords);
			if (!bOK) {
				return false;
			}

			// Word found in dictionary.
			if (prof.Allow_Ss) {
				// With this setting, *any* word in the dictionary is valid (even if it
				// doesn't end in S)
				bIsAcceptable = true;
				return true;
			}

			// Word found, and we care about whether it ends in S.
			if (!Word.Text.EndsWith("S")) {
				bIsAcceptable = true;
			} else {
				bIsAcceptable = Word.IsPluralValid;         // e.g. DRESS
			}
			return true;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Checks to see if word is valid, e.g. is long enough, doesn't end in 'S'
		/// unless allowed in the profile, etc.
		/// </summary>
		/// <param name="bIsProgramWord"></param>
		/// <param name="CurWord"></param>
		/// <returns>true if this is a valid word, else false</returns>
		private bool IsValidWord(bool bIsProgramWord, BogWord CurWord) {
			// TODO: TODO: TODO: Get rid of this
			PlayerProfile prof;
			if (bIsProgramWord) {
				prof = BogForm.ProgramProf;
			} else {
				prof = BogForm.PlayerProf;
			}

			if (CurWord.LetterCount < prof.MinWordLength) {
				if (!bIsProgramWord) {
					string msg = string.Format("The word must be at least {0} letters long", BogForm.PlayerProf.MinWordLength);
					MessageBox.Show(msg, "Boondoggle", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				return false;
			}

			if ((!prof.Allow_Ss) && CurWord.Text.EndsWith("S")) {
				// Check for exception
				if (CurWord.IsPluralValid) {
					return true;
				}
				if (!bIsProgramWord) {
					string msg = "The word may not end in 'S'";
					MessageBox.Show(msg, "Boondoggle", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				return false;
			}

			return true;
		}


		/// <summary>
		///  Used when first setting up an nxn board
		/// </summary>
		private void AllocateCubes() {
			RemoveCubesFromForm();
			Cubes = new BogCube[prof.Rank, prof.Rank];
			for (int row = 0; row < prof.Rank; ++row) {
				for (int col = 0; col < prof.Rank; ++col) {
					BogCube cube = new BogCube(row, col);
					Cubes[row, col] = cube;
					BogForm.Controls.Add(cube);
				}
			}
		}

#if false      // public void Resize(GroupBox BogBox)
		//---------------------------------------------------------------------------------------

		/// <summary>
		/// Calculate new font sizes, locations, etc when board is resized
		/// </summary>
		/// <param name="BogBox"></param>
		public void Resize(GroupBox BogBox) {
			if (BogForm.WindowState == FormWindowState.Minimized) {
				return;
			}

			int rank = prof.Rank;

			int LRMargin = 16;          // Left-Right Margin
			int TBMargin = 16;          // Top-Bottom Margin
			int HGutter  = 16;          // Horizontal Gutter
			int VGutter  = 16;          // Vertical Gutter

			// OK, the calculation for each row is as follows.
			//	1.	The width available for squares is
			//		a.	The width of the surrounding BogBox,
			//		b.	minus the LRMargin
			//		c.	minus HGutter * (# of squares in row - 2)
			//	2.	So calculate that, and divide by <n>. This gives the width of each
			//		square. Note: Do this in floating point to avoid truncation errors.
			//	3.	The y coordinate is always the same for each row

			int StartCol = BogBox.Left + LRMargin;      // Rows start here
			int StartRow = BogBox.Top  + TBMargin;      // Columns start here

			int AvailWidth  = BogBox.Width  - 2 * LRMargin - (rank - 1) * HGutter;
			int AvailHeight = BogBox.Height - 2 * TBMargin - (rank - 1) * VGutter;

			double w = (double)AvailWidth  / rank;      // Width of a square
			double h = (double)AvailHeight / rank;      // Height of a square

			Font    fnt; // = BogSquare.SqFont;
						 // TODO: Clean this up. Don't use public fields, get info from profile, 
						 //		 resize more intelligently, etc
						 // Note: "Qu" doesn't show up if the box is too small.
			fnt = new Font("Times New Roman", (float)h / 2.0f, FontStyle.Bold);
			Boondog2014.SqFont = fnt;

			BogCube cube;
			for (int row = 0; row < rank; ++row) {
				int y = (int)Math.Round(StartRow + row * (h + TBMargin));
				for (int col = 0; col < rank; ++col) {
					int x = (int)Math.Round(StartCol + col * (w + LRMargin));
					cube = Cubes[row, col];
					cube.Size = new Size((int)w, (int)h);
					Locations[row, col] = new Point(x, y);
					cube.Font = fnt;
				}
			}
			PlaceCubes();
		}
#endif

#if false       // public void PlaceCubes()
		//---------------------------------------------------------------------------------------

		/// <summary>
		/// We have all these nice Cubes. Now place them where they should go.
		/// </summary>
		public void PlaceCubes() {
			int rank = prof.Rank;
			int ofs;                // Offset
			BogForm.SuspendLayout();

			switch (Orientation) {

				case BoardOrientation.North:
					for (int row = 0; row < rank; row++) {
						for (int col = 0; col < rank; col++) {
							Cubes[row, col].Location = Locations[row, col];
						}
					}
					break;

				case BoardOrientation.East:
					// Cubes[0, 0].Location = Locations[0, rank - 1];
					// Cubes[0, 1].Location = Locations[1, rank - 1];
					// Cubes[0, 2].Location = Locations[2, rank - 1];
					// ...
					// Cubes[1, 0].Location = Locations[0, rank - 2];
					// Cubes[1, 1].Location = Locations[1, rank - 2];
					// Cubes[1, 2].Location = Locations[2, rank - 2];
					ofs = rank - 1;
					for (int row = 0; row < rank; row++) {
						for (int col = 0; col < rank; col++) {
							Cubes[row, col].Location = Locations[col, ofs];
						}
						--ofs;
					}
					break;

				case BoardOrientation.South:
					// Cubes[0, 0].Location = Locations[rank - 1, rank - 1];
					// Cubes[0, 1].Location = Locations[rank - 1, rank - 2];
					// Cubes[0, 2].Location = Locations[rank - 1, rank - 3];
					// ...
					// Cubes[1, 0].Location = Locations[rank - 2, rank - 1];
					// Cubes[1, 1].Location = Locations[rank - 2, rank - 2];
					// Cubes[1, 2].Location = Locations[rank - 2, rank - 3];
					ofs = rank - 1;
					for (int row = 0; row < rank; row++) {
						for (int col = 0; col < rank; col++) {
							Cubes[row, col].Location = Locations[ofs, rank - 1 - col];
						}
						--ofs;
					}
					break;

				case BoardOrientation.West:
					// Cubes[0, 0].Location = Locations[rank - 1, 0];
					// Cubes[0, 1].Location = Locations[rank - 2, 1];
					// Cubes[0, 2].Location = Locations[rank - 3, 2];
					// ...
					// Cubes[1, 0].Location = Locations[rank - 1, 0];
					// Cubes[1, 1].Location = Locations[rank - 2, 1];
					// Cubes[1, 2].Location = Locations[rank - 3, 2];
					for (int row = 0; row < rank; row++) {
						ofs = rank - 1;
						for (int col = 0; col < rank; col++) {
							Cubes[row, col].Location = Locations[ofs--, row];
						}
					}
					break;
			}

			BogForm.ResumeLayout(true);
		}
#endif

#if false       // private void RemoveCubesFromForm()
		//---------------------------------------------------------------------------------------

		/// <summary>
		///  Used when, say, going from 5x5 to 6x6
		/// </summary>
		private void RemoveCubesFromForm() {
			if (Cubes == null) {
				return;
			}
			int rank = Cubes.GetUpperBound(0) + 1;  // We know it's square
			for (int row = 0; row < rank; ++row) {
				for (int col = 0; col < rank; ++col) {
					BogCube cube = Cubes[row, col];
					cube.Visible = false;
					BogForm.Controls.Remove(cube);
				}
			}
		}
#endif

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Starts a new game and keeps generating boards until a viable (with enough
		/// words, etc) board is found
		/// </summary>
		public void NewGame() {
			SetupForNewGame();

			// Loop, often repeatedly, generating new boards until we find one that
			// meets the user's criteria (min Score, min # of Words, etc)
			do {
				BogForm.ResetAuxControls();         // Player and Program listboxes, etc

				BogForm.ClearFoundWords();

				bool bManualSetup = BogDice.ShakeDice(prof, Cubes);
				DrawCubes();
				BogForm.DoEvents();

				// We might have changed the rank (e.g. 6x6) of the board since the last
				// game, so things (e.g. Cubes) might have changed. Re-instantiate things
				// just in case.
				var WordFinder = new GenerateProgramWords(BogForm, prof, Cubes, ProgramDict, this);

				game.ProgramWords = 0;
				game.ProgramScore = 0;
				WordFinder.FindProgramWords();
				BogForm.SetStats(true);
				BogForm.SetInitialUnfound();
				if (bManualSetup) {
					BogForm.PlayerProf.GoalWords = -1;      // Don't recreate board
				}
			} while (game.ProgramWords < BogForm.PlayerProf.GoalWords);

			DrawCubes();

			BogForm.SortProgramListBox(true);
		}

//---------------------------------------------------------------------------------------

		public void DrawCubes() {
			int rank = Cubes.GetUpperBound(0) + 1;  // We know it's square
			for (int row = 0; row < rank; ++row) {
				for (int col = 0; col < rank; ++col) {
					BogCube cube = Cubes[row, col];
					cube.Invalidate();              // Force redraw
				}
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Reset the colors on all the squares. Note: If we change the rank of the board
		///  (e.g. from 5x5 to 6x5), this is redundant. But it's not worth optimizing.
		/// </summary>
		private void SetupForNewGame() {
			SetSquareColorsToDefault();

			CurrentWord = new BogWord();
			LastWord = new BogWord();

			Orientation = BoardOrientation.North;

			ReRankBoard();

			game.ProgramWords = 0;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Rotates the board left (counterclockwise)
		/// </summary>
		public void RotateLeft() {
			if (Board.BoardOrientation.First == --Orientation) {
				Orientation = Board.BoardOrientation.West;
			}
			PlaceCubes();
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		///  Rotates the board to the right (clockwise)
		/// </summary>
		public void RotateRight() {
			if (Board.BoardOrientation.Last == ++Orientation) {
				Orientation = Board.BoardOrientation.North;
			}
			PlaceCubes();
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Used at the start of a game to set all cubes to their default colors
		/// </summary>
		public void SetSquareColorsToDefault() {
			int rank = Cubes.GetLength(0);
			for (int row = 0; row < rank; ++row) {
				for (int col = 0; col < rank; ++col) {
					Cubes[row, col].Reset();
				}
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Will reallocate things if the user has changed from, say, 5x5 to 6x6.
		/// </summary>
		private void ReRankBoard() {
			int rank = Cubes.GetLength(0);
			if (rank == prof.Rank) {
				return;
			}
			AllocateCubes();
		}

//---------------------------------------------------------------------------------------
		/// <summary>
		/// Starts a new word (resets square colors, etc). Also remembers previous word
		/// </summary>
		public void NewWord() {
			LastWord = CurrentWord;
			foreach (var cube in CurrentWord.CubeLetters) {
				cube.SetDefaultColor();
				cube.Reset();
				cube.Invalidate();
			}
			CurrentWord = new BogWord();
			// SetSquareColorsToDefault(); // Bad performance to reset all cubes
			BogForm.ShowWordSoFar(CurrentWord);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Called from GenerateWord to add a word to the program's listbox
		/// </summary>
		/// <param name="NewWord"></param>
		public void ProgramAddWord(BogWord NewWord) {
			if (IsValidWord(true, NewWord)) {
				// TODO: Replace this with some kind of call to IsValidWord2
				// Note: The Addword routine will filter out cases where the same word might
				//		 have come from different paths
				NewWord.IsFromProgramDictionary = true;
				BogForm.AddWordToListbox(true, NewWord, false);
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Used to show the user where the specified word is by highlighting its cubes
		/// </summary>
		/// <param name="bw"></param>
		public void Highlight(BogWord bw) {
			int rank = prof.Rank;
			// Reset squares (e.g. from last click)
#if true
			// TODO: See NewWord -- for performance reasons, just reset the cubes needed
			SetSquareColorsToDefault();
#else
			for (int row = 0; row < rank; row++) {
				for (int col = 0; col < rank; col++) {
					Cubes[row, col].SetDefaultColor();
				}
			}
#endif

			foreach (var cube in bw.CubeLetters) {
				cube.SetHighlightColor();
				Thread.Sleep(125);
				// Application.DoEvents();
			}
		}
	}
}
#endif
