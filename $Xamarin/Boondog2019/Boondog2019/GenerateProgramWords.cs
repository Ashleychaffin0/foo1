// Copyright (c) 2007-2019 by Larry Smith

// TODO: Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591

// Note: I moved this code out of the Board class. It's important enough to warrant its
//		 own module.
//
//		 One thing I don't like about it is its dependence on Board.AddWord. I'd like it
//		 better if it were more standalone. In some respects, I'd like to make
//		 GenerateWord return an IEnumerable<BogWord>. But with the 9 recursive calls, the
//		 code becomes noticeably more opaque. OTOH, I could accumulate a List<BogWord>
//		 and then just return the list to the caller (nominally a Board instance). But
//		 what if I wanted to do something with each word (such as highlighting words as
//		 they're found)? Waiting until the end disallows such things. So for now I'll
//		 leave the AddWord in.

namespace nsBoondog2019 {
	class GenerateProgramWords {
		BoondogForm BogForm;
		BogBoard board;
		PlayerProfile prof;
		BogCube[,] Cubes;
		BoondogDict dict;

		int TimesThrough;

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="BogForm"></param>
		/// <param name="prof"></param>
		/// <param name="Cubes"></param>
		/// <param name="dict"></param>
		/// <param name="board"></param>
		public GenerateProgramWords(Boondog2014 BogForm, PlayerProfile prof, BogCube[,] Cubes, BoondogDict dict, Board board) {
			this.BogForm = BogForm;
			this.prof = prof;
			this.Cubes = Cubes;
			this.dict = dict;
			this.board = board;

			TimesThrough = 0;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Takes the board and finds all words that start with the letter(s) on each cube
		/// </summary>
		public void FindProgramWords() {
			BogForm.SetProgramListBoxBegin_EndUpdate(true);
			int rank = Cubes.GetLength(0);
			for (int row = 0; row < rank; row++) {
				for (int col = 0; col < rank; col++) {
					var TrackCubes = new BogWord();
					GenerateWord("", row, col, TrackCubes);
				}
			}
			BogForm.SetProgramListBoxBegin_EndUpdate(false);

			BogForm.DisplayWordLengths();
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// The heart of the word finder. A recursive routine that finds all the words
		/// that appear in the program dictionary (Boondog.dct). We explicitly do *not*
		/// look in the user dictionary, so we can offer the player a whiff of a chance
		/// to actually beat the program.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="row"></param>
		/// <param name="col"></param>
		/// <param name="TrackCubes"></param>
		private void GenerateWord(string input, int row, int col,
							BogWord TrackCubes) {
			int rank = Cubes.GetLength(0);
			if ((row < 0) || (col < 0) || (row >= rank) || (col >= rank))
				return;
			// Keep track of squares we've already used.
			bool bReusePgmSquare = prof.AllowJudieRules;
			BogCube sq = Cubes[row, col];
			if ((!bReusePgmSquare) && (sq.VisitCount > 0))
				return;
			++sq.VisitCount;

			// Note: We really could get by with an array of bool's, not byte's. But
			//		 let's use byte's, in case we want to use the visit count for
			//		 something. Dunno what, but maybe we give extra points the more times
			//		 you're able to reuse a square. Or maybe you lose points. But with
			//		 byte's we've got the option. (OK, we could have used int's or even
			//		 short's. And on the desktop it wouldn't matter. But I'm trying to
			//		 keep in mind how slow PDAs can be, and I'm hoping that bytes will
			//		 speed things up, just a bit (e.g. less data to Array.Copy, better
			//		 cache usage maybe, etc). And while it won't be much, this routine
			//		 is called so many times that it's worth trying to optimize.

			string SoFar = input + Cubes[row, col].Text.ToUpper();  // Qu->QU
			TrackCubes.AppendCube(sq);
			if (SoFar.Length > 2) {                 // Throw back small fish
				if ((++TimesThrough % 50) == 0) {
					BogForm.DoEvents();
				}
				// char TagType;
				// bool bOK = dict.FindWord(SoFar, out bMorePossibleWords, out TagType);
				bool bOK = dict.FindWord(TrackCubes, out bool bMorePossibleWords);
				// Note: We explicitly *don't* check the user dictionary here. Give the
				//		 poor user a chance to actually *win* the game!
				if (bOK) {
					board.ProgramAddWord(TrackCubes);           // TODO:
				}
				if (!bMorePossibleWords) {
					--sq.VisitCount;
					TrackCubes.DeleteLastCube();
					return;
				}
			}

			// "Recursion, n, see Recursion"
			// "To understand recursion, first of all you have to understand recursion."
			GenerateWord(SoFar, row - 1, col - 1, TrackCubes);
			GenerateWord(SoFar, row - 1, col, TrackCubes);
			GenerateWord(SoFar, row - 1, col + 1, TrackCubes);
			GenerateWord(SoFar, row, col - 1, TrackCubes);
			if (bReusePgmSquare)
				GenerateWord(SoFar, row, col, TrackCubes);
			GenerateWord(SoFar, row, col + 1, TrackCubes);
			GenerateWord(SoFar, row + 1, col - 1, TrackCubes);
			GenerateWord(SoFar, row + 1, col, TrackCubes);
			GenerateWord(SoFar, row + 1, col + 1, TrackCubes);

			--sq.VisitCount;
			TrackCubes.DeleteLastCube();
		}
	}
}
