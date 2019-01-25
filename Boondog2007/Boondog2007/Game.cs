// Copyright (c) 2007 by Larry Smith

using System;
using System.Collections.Generic;
using System.Text;

namespace Boondog2009 {

	// TODO: Finally figured out what <Game> is. It's something that can be Saved
	//		 and restored. Implement this at some point.
	class Game {

		public int []	Points = new int[Board.MaxWordLen];
 
		public int		PlayerWords,  PlayerScore;
        public int		ProgramWords, ProgramScore;

//---------------------------------------------------------------------------------------

		public Game() {
			Reset();

			Points[0] = 0;          // Words of length 0 score 0 points
			Points[1] = 0;
			Points[2] = 0;
			Points[3] = 1;          // Words of length 3 score 1 point
			Points[4] = 1;
			Points[5] = 2;          // Words of length 5 score 2 points
			Points[6] = 3;
			Points[7] = 5;
			Points[8] = 11;

			for (int i = 9; i < Board.MaxWordLen; i++)
				Points[i] = Points[i - 1] + 4;        // Why not ???

            BogSquare.game = this;
		}

//---------------------------------------------------------------------------------------

		public void Reset() {
			PlayerWords = 0;
			PlayerScore = 0;

			ProgramWords = 0;
			ProgramScore = 0;
		}

//---------------------------------------------------------------------------------------

		public int Score(string word) {
			return Points[word.Length];
		}

//---------------------------------------------------------------------------------------

        internal void NewPlayerWord(string CurrentWord) {
            // TODO: Unused, maybe unnecessary
            // TODO: Check to see if it's already there
            // TODO: Check to see if it's too long (and thus avoid an index error
            //       in Points.
            ++PlayerWords;

            PlayerScore += Score(CurrentWord);
        }
    }
}
