// Copyright (c) 2007-2009 by Larry Smith

using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Boondog2009 {
	class Board {

		BogSquare[,]	Squares;

		PlayerProfile	prof;

		Boondog2009		MyForm;

		GameDictionary	dict;

		string []		Dice;

		int				timesThrough = 0;		// From GenerateWord

		public const int	MaxWordLen = 20;    // Overly generous (at least in practice)

//---------------------------------------------------------------------------------------

		public Board(Boondog2009 MyForm, GroupBox BogBox, PlayerProfile prof, GameDictionary dict) {
			this.MyForm = MyForm;
			this.prof = prof;
			this.dict = dict;

			BogSquare.SetBoard(this);

			SetupDice();
			AllocateSquares();
			Resize(BogBox);
			BogBox.SendToBack();
		}

//---------------------------------------------------------------------------------------

		// Used when first setting up an nxn board
		private void AllocateSquares() {
			RemoveSquaresFromForm();
			Squares = new BogSquare[prof.Rank, prof.Rank];
			for (int row = 0; row < prof.Rank; ++row) {
				for (int col = 0; col < prof.Rank; ++col) {
					BogSquare sq = new BogSquare(row, col);
					Squares[row, col] = sq;
					MyForm.Controls.Add(sq);
				}
			}
		}

//---------------------------------------------------------------------------------------

		public void Resize(GroupBox BogBox) {
			if (MyForm.WindowState == FormWindowState.Minimized) {
				return;
			}
			BogSquare sq;

			int rank = prof.Rank;

			int LRMargin = 16;			// Left-Right Margin
			int TBMargin = 16;			// Top-Bottom Margin
			int HGutter = 16;			// Horizontal Gutter
			int VGutter = 16;			// Vertical Gutter

			// OK, the calculation for each row is as follows.
			//	1.	The width available for squares is
			//		a.	The width of the surrounding BogBox,
			//		b.	minus the LRMargin
			//		c.	minus HGutter * (# of squares in row - 2)
			//	2.	So calculate that, and divide by <n>. This gives the width of each
			//		square. Note: Do this in floating point to avoid truncation errors.
			//	3.	The y coordinate is always the same for each row

			int StartCol = BogBox.Left + LRMargin;		// Rows start here
			int StartRow = BogBox.Top + TBMargin;		// Columns start here

			int AvailWidth = BogBox.Width - 2 * LRMargin - (rank - 1) * HGutter;
			int AvailHeight = BogBox.Height - 2 * TBMargin - (rank - 1) * VGutter;
			double w = (double)AvailWidth / rank;		// Width of a square
			double h = (double)AvailHeight / rank;		// Height of a square

			Font	fnt; // = BogSquare.SqFont;
			// TODO: Clean this up. Don't use public fields, get info from profile, 
			//		 resize more intelligently, etc
			// TODO: "Qu" doesn't show up if the box is too small.
			fnt = new Font("Times New Roman", (float)h / 2.5f, FontStyle.Bold);
			BogSquare.SqFont = fnt;

			BogSquare.BogForm.SuspendLayout();
			for (int row = 0; row < rank; ++row) {
				int y = (int)Math.Round(StartRow + row * (h + TBMargin));
				for (int col = 0; col < rank; ++col) {
					int x = (int)Math.Round(StartCol + col * (w + LRMargin));
					sq = Squares[row, col];
					sq.Size = new Size((int)w, (int)h);
					sq.Location = new Point(x, y);
					sq.Font = fnt;
				}
			}
			BogSquare.BogForm.ResumeLayout(true);
		}

//---------------------------------------------------------------------------------------

		// Used when, say, going from 5x5 to 6x6
		private void RemoveSquaresFromForm() {
			if (Squares == null) {
				return;
			}
			int rank = Squares.GetUpperBound(0) + 1;	// We know it's square
			for (int row = 0; row < rank; ++row) {
				for (int col = 0; col < rank; ++col) {
					BogSquare sq = Squares[row, col];
					sq.Visible = false;
					MyForm.Controls.Remove(sq);
				}
			}
		}

//---------------------------------------------------------------------------------------

		internal void NewGame() {
			// Reset the colors on all the squares. Note: If we change the rank of the
			// board (e.g. from 5x5 to 6x5), this is redundant. But it's not worth
			// optimizing.
			SetSquareColorsToDefault();

			ReRankBoard();

			MyForm.ResetAuxControls();			// Player and Program listboxes, etc

			ShakeDice();

			FindProgramWords();
		}

//---------------------------------------------------------------------------------------

		private void SetSquareColorsToDefault() {
			int rank = Squares.GetLength(0);
			for (int row = 0; row < rank; ++row) {
				for (int col = 0; col < rank; ++col) {
					Squares[row, col].Reset();
				}
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Will reallocate things if the user has changed from, say, 5x5 to 6x6.
		/// </summary>
		private void ReRankBoard() {
			int	rank = Squares.GetLength(0);
			if (rank == prof.Rank) {
				return;
			}
			AllocateSquares();
		}

//---------------------------------------------------------------------------------------

		void ShakeDice() {
#if false
			Squares[0, 0].Text = "C";
			Squares[0, 1].Text = "O";
			Squares[0, 2].Text = "R";
			Squares[0, 3].Text = "D";
			Squares[0, 4].Text = "Y";

			Squares[1, 0].Text = "B";
			Squares[1, 1].Text = "I";
			Squares[1, 2].Text = "D";
			Squares[1, 3].Text = "M";
			Squares[1, 4].Text = "D";

			Squares[2, 0].Text = "H";
			Squares[2, 1].Text = "E";
			Squares[2, 2].Text = "T";
			Squares[2, 3].Text = "N";
			Squares[2, 4].Text = "S";

			Squares[3, 0].Text = "A";
			Squares[3, 1].Text = "T";
			Squares[3, 2].Text = "I";
			Squares[3, 3].Text = "W";
			Squares[3, 4].Text = "P";

			Squares[4, 0].Text = "O";
			Squares[4, 1].Text = "Y";
			Squares[4, 2].Text = "N";
			Squares[4, 3].Text = "S";
			Squares[4, 4].Text = "F";
			if (MyForm != null)
				return;
#else
			// For each die, choose a random spot for it, then choose a random letter 
			// from that die. Make sure we don't put two dice in the same square!
			int		rank = prof.Rank;
			int		nSquares = rank * rank;
			int		nSq;
			bool []	bOccupied = new bool[nSquares];		// All start out as false
			Random	rand = new Random();
			foreach (string die in Dice) {
				// In theory, the following is an infinite loop. But it's a pretty poor
				// random number generator that doesn't generate all nSquares values
				// pretty quickly.
				while (true) {
					nSq = rand.Next(0, nSquares);
					if (! bOccupied[nSq]) {
						bOccupied[nSq] = true;
						break;
					}
				}
				int		row = nSq / rank;
				int		col = nSq % rank;
				string	txt = die.Substring(rand.Next(0, 6), 1);
				if (txt == "Q") {
					txt = "Qu";
				}
				Squares[row, col].Text = txt;
			}
#endif
			Application.DoEvents();		// Show letter before finding words
		}

//---------------------------------------------------------------------------------------

		private void FindProgramWords() {
			// listBox1.BeginUpdate() / EndUpdate();
			int	rank = prof.Rank;
			byte[] TrackLetters = new byte[MaxWordLen];
			for (int row = 0; row < rank; row++) {
				for (int col = 0; col < rank; col++) {
					// Console.WriteLine("\n***** GenerateWord[{0}, {1}]", row, col);
					GenerateWord("", row, col, TrackLetters, 0);
				}
			}

			MyForm.DisplayWordLengths();
		}

//---------------------------------------------------------------------------------------

		private void GenerateWord(string input, int row, int col,
							byte[] TrackLettersParm, int TrackLenParm) {
			int		rank = prof.Rank;
			if ((row < 0) || (col < 0) || (row >= rank) || (col >= rank))
				return;
			// Keep track of squares we've already used.
			bool	bReusePgmSquare = prof.JudieRulesProgram;
			BogSquare	sq = Squares[row, col];
			if ((!bReusePgmSquare) && (sq.VisitCount > 0))
				return;
			++sq.VisitCount;

			byte []	TrackLetters = null;
			// Note: We must use TrackLenParm. We can't just use input.Length since
			//		 "Qu" is only one square, but two characters.
			// Note: We really could get by with an array of bool's, not byte's. But
			//		 let's use int's, in case we want to use the visit count for
			//		 something. Dunno what, but maybe we give extra points the more times
			//		 you're able to reuse a square. Or maybe you lose points. But with
			//		 byte's we've got the option. (OK, we could have used int's or even
			//		 short's. And on the desktop it wouldn't matter. But I'm trying to
			//		 keep in mind how slow PDAs can be, and I'm hoping that bytes will
			//		 speed things up, just a bit (e.g. less data to Array.Copy, better
			//		 cache usage maybe, etc). And while it won't be much, this routine
			//		 is called so many times that it's worth trying to optimize.
			int		TrackLen = TrackLenParm;
			TrackLetters = new byte[MaxWordLen];
			Array.Copy(TrackLettersParm, TrackLetters, TrackLen);

			string	SoFar = input + Squares[row, col].Text.ToUpper();	// Qu->QU
			TrackLetters[TrackLen++] = unchecked((byte)(row * rank + col));
			if (SoFar.Length > 2) {			// Throw back small fish
				if ((++timesThrough % 50) == 0) {
					Application.DoEvents();
				}
				bool bMorePossibleWords;
				bool	bOK = dict.FindWord(SoFar, out bMorePossibleWords);
				if (bOK) {
					AddWord(true, SoFar, TrackLetters, TrackLen);			// TODO:
				} 
				if (!bMorePossibleWords) {
					--sq.VisitCount;
					return;
				}
			}

            // "To understand recursion, first of all you have to understand recursion."
			GenerateWord(SoFar, row - 1, col - 1, TrackLetters, TrackLen);
			GenerateWord(SoFar, row - 1, col	, TrackLetters, TrackLen);
			GenerateWord(SoFar, row - 1, col + 1, TrackLetters, TrackLen);
			GenerateWord(SoFar, row,     col - 1, TrackLetters, TrackLen);
			if (bReusePgmSquare)
				GenerateWord(SoFar, row, col, TrackLetters, TrackLen);
			GenerateWord(SoFar, row,     col + 1, TrackLetters, TrackLen);
			GenerateWord(SoFar, row + 1, col - 1, TrackLetters, TrackLen);
			GenerateWord(SoFar, row + 1, col	, TrackLetters, TrackLen);
			GenerateWord(SoFar, row + 1, col + 1, TrackLetters, TrackLen);

			--sq.VisitCount;
		}

//---------------------------------------------------------------------------------------

		public void NewWord() {
			string	word = BogSquare.CurrentWord;
			bool bMorePossibleWords;
			if (word.Length < prof.MinWordLength) {
				MessageBox.Show("Sorry, but the word must be at least " + word.Length +
					" characters long.", "Boondoggle", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			bool bFound = dict.FindWord(word, out bMorePossibleWords);
			BogSquare.CurrentWord = "";
			SetSquareColorsToDefault();
		}

#if false
//---------------------------------------------------------------------------------------

		private void FindProgramWords() {
			int	rank = prof.Rank;
			bool	QuitSwitch = false;			// TODO: ???
		for (int row=0; row<rank; row++) {
		for (int col=0; col<rank; col++) {
			string curLetter = Squares[row, col].Text;
			if (! QuitSwitch) {
				ReadDict(curLetter);
				int firstWordLen = Dict[1];
				for (int i=0; i<firstWordLen; i++)
					firstWord[i] = Dict[i + 2];
				firstWord[i] = '\0';
				GenerateWord("", row, col, firstWord, firstWordLen + 2, 1,
							 (int *) NULL,  0);
			}
#if false
			if (TimeLimitPgm == 0) {
				int     SqIndex;
				char    msgbuf[30];
				SqIndex = row * 5 + col + 1;
				wsprintf(msgbuf, "To Go: %d%%", 100 - SqIndex * 4);
				SetStatusLine(hWndTextPgmTogo, STATLINE_SHOWWINDOWTEXT, msgbuf);
			}
#endif
		}
		}
		}
#endif

#if false
//---------------------------------------------------------------------------------------

void GenerateWord(string	SoFarParm,
				  int       row,
				  int       col,
				  string	CurDictWordParm,
				  uint		NextWordPtr,
				  int       PrefixLen,
				  int []    TrackPgmLettersParm,
				  int       TrackLenParm) {

 int    i, nLetter1, nLetter2, NextWordLen, SoFarLen;

 string	SoFar, 
		DictPrefix,
		CurDictWord;
	int		TrackLen;

 LPSTR DictPtr;
 char	*CurDictPtr;
#if false
 char   *PrefixPtr;
#endif

	int		rank = prof.Rank;
 if ((row < 0) || (col < 0) || (row > rank) || (col > rank) || QuitSwitch)
	return;

 if ((! ReusePgmSquare) && Squares[row, col].PgmVisitCount)
	return;

 TrackLen = TrackLenParm;
 for (int i=0; i<TrackLen; i++)
	TrackPgmLetters[i] = TrackPgmLettersParm[i];
 TrackPgmLetters[TrackLen++] = row * 5 + col;   // What are the Map()
												// (i.e. Board Rotation)
												// implications of this?
												// i.e. should this be
												// Map(row*5+col) ???

	CurDictWord = CurDictWordParm;
	SoFar = SoFarParm;
	SoFar += Squares[row, col].Text;

 SoFarLen = SoFar.Length;

 if ((SoFarLen == 2)) {             //  && (SoFar[1] != ' ')
	nLetter1 = SoFar[0] - 'A';
	nLetter2 = SoFar[1] - 'A';
	NextWordPtr = SubDict[nLetter1][nLetter2] - 1;
	PrefixLen = 1;
 }

 HighLightLetter(true, row, col, SoFar);

 if (++timesThrough > 50) {
	 Application.DoEvents();
	timesThrough = 0;
 }

 while (true) {

#if true
	 DictPrefix = CurDictWord.Substring(0, SoFarLen);
#else
	for (i=0; i<SoFarLen; i++)
	DictPrefix[i] = CurDictWord[i];
	DictPrefix[i] = '\0';
---------
	strcpy(DictPrefix, CurDictWord);
	DictPrefix[SoFarLen] = '\0';
---------
	DictPrefix[SoFarLen] = '\0';
	for (i=SoFarLen-1; i>=0; i--)
		DictPrefix[i] = CurDictWord[i];
#endif

	if (DictPrefix > SoFar) {
		HighLightLetter(false, row, col, "");
		return;
	}

	if (SoFar == CurDictWord == 0) {			// Its a word!
		if ((! CanUsePluralsPgm) &&
			(SoFar[SoFarLen - 1] == 'S'))
			break;
		AddWord(SoFar, TrackPgmLetters, TrackLen);
		break;
	}

	if (SoFar == DictPrefix)
		break;

	if (NextWordPtr > DictLen) {
		HighLightLetter(false, row, col, "");
		return;
	}

	DictPtr = &Dict[NextWordPtr];
	PrefixLen   = *DictPtr++;
	NextWordLen = *DictPtr++;
#if true
	CurDictPtr = &CurDictWord[PrefixLen];
	for (int i=0; i<NextWordLen; i++)
		*CurDictPtr++ = *DictPtr++;
	*CurDictPtr = '\0';
#else
	for (i=0; i<NextWordLen; i++)
		CurDictWord[PrefixLen + i] = Dict[NextWordPtr + 2 + i];
	CurDictWord[PrefixLen + i] = '\0';
#endif
	NextWordPtr += NextWordLen + 2;
 }

 GenerateWord(SoFar, row-1, col-1, CurDictWord, NextWordPtr, PrefixLen,
								   TrackPgmLetters, TrackLen);
 GenerateWord(SoFar, row-1, col  , CurDictWord, NextWordPtr, PrefixLen,
								   TrackPgmLetters, TrackLen);
 GenerateWord(SoFar, row-1, col+1, CurDictWord, NextWordPtr, PrefixLen,
								   TrackPgmLetters, TrackLen);
 GenerateWord(SoFar, row  , col-1, CurDictWord, NextWordPtr, PrefixLen,
								   TrackPgmLetters, TrackLen);
 if (ReusePgmSquare)
	GenerateWord(SoFar, row, col,  CurDictWord, NextWordPtr, PrefixLen,
								   TrackPgmLetters, TrackLen);
 GenerateWord(SoFar, row  , col+1, CurDictWord, NextWordPtr, PrefixLen,
								   TrackPgmLetters, TrackLen);
 GenerateWord(SoFar, row+1, col-1, CurDictWord, NextWordPtr, PrefixLen,
								   TrackPgmLetters, TrackLen);
 GenerateWord(SoFar, row+1, col  , CurDictWord, NextWordPtr, PrefixLen,
								   TrackPgmLetters, TrackLen);
 GenerateWord(SoFar, row+1, col+1, CurDictWord, NextWordPtr, PrefixLen,
								   TrackPgmLetters, TrackLen);

 HighLightLetter(false, row, col, SoFar);

}
#endif

//---------------------------------------------------------------------------------------

		private void AddWord(bool bIsProgram, string SoFar, byte[] TrackPgmLetters, int TrackLen) {
			// TODO: Do somwthing with latter 2 parms
			MyForm.AddWord(bIsProgram, SoFar, TrackPgmLetters, TrackLen);
		}

//---------------------------------------------------------------------------------------

		private void HighLightLetter(bool flag ,int row, int col, string SoFar) {
			// TODO:
 			throw new Exception("The method or operation is not implemented.");
		}

//---------------------------------------------------------------------------------------

		void SetupDice() {
			string []	BasicDice = new string[25] {
				"XQZKBJ",
				"TOUOWN",
				"AAAFRS",
				"LRNHDO",
				"ITCLPE",
				"AEMEEE",
				"IIIETT",
				"NENNDA",
				"TILCIE",
				"SSNSEU",
				"HHTOND",
				"TOUOOT",
				"EEEEAA",
				"YPFIRS",
				"RVGROW",
				"EAUMGE",
				"FAYISR",
				"NGEANM",
				"SITCPE",
				"MTETOT",
				"PRIYRH",
				"ORDHLH",
				"NCCWST",
				"LONDRD",
				"RISFAA"
			};

			int	nSq = prof.Rank * prof.Rank;
			Dice = new string[nSq];
			if (prof.Rank <= 5) {
				Array.Copy(BasicDice, Dice, nSq);
				return;
			}
			// Rank is >= 6. We have a choice of how to proceed. We could either choose
			// dice from the BasicDice list (either randomly or by going again from the
			// top), or we can generate random dice. I'm going to try the latter, at 
			// least for now
			// Note: This doesn't work. We get far too many consonants and too few
			//		 vowels, and the consonants can't "connect" through the vowels, and
			//		 we actually get far fewer words than we'd expect. So we'll go back
			//		 to reusing the original dice.
			Array.Copy(BasicDice, Dice, 25);
			Random	rand = new Random();

			for (int i = 25; i < nSq; i++) {
				int		n = rand.Next(0, 25);
				Dice[i] = BasicDice[n];
			}


#if false	// This is the algorithm that doesn't work.
			for (int i = 25; i < nSq; i++) {
				string	die = "";
				for (int n = 0; n < 6; n++) {
					int	ix = rand.Next(0, 26);
					die += "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring(ix, 1);
				}
				Dice[i] = die;
			}
#endif
		}

//---------------------------------------------------------------------------------------

		public void Highlight(byte [] Track) {
			int	rank = prof.Rank;
			// Reset squares (e.g. from last click)
			for (int row = 0; row < rank; row++) {
				for (int col = 0; col < rank; col++) {
					Squares[row, col].SetDefaultColor();
				}
			}
			foreach (byte ix in Track) {
				int row = ix / rank;
				int col = ix % rank;
				Squares[row, col].SetHighlightColor();
			}
		}
	}
}
