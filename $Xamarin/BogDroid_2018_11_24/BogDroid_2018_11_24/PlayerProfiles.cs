// Copyright (c) 2007 by Larry Smith

// TODO: Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace BogDroid_2019_03 {

	class PlayerProfiles {
		// TODO:
	}

//---------------------------------------------------------------------------------------

	public class PlayerProfile {
		public string   PlayerName;

		public bool     AllowJudieRules;

		public bool     Allow_Ss;
		public bool     Allow_ING;
		public bool     Allow_ED;

		public int      MinWordLength,
						MaxWordLength;  // This one for kids

		public int      MinimumScore,
						MinimumWords;

		public int      GoalWords,      // e.g. 50% of Pgms words = win
						GoalScore;

		public int      Rank;           // 5x5, or even 10x10. Max of 15x15. We use
										// byte's for TrackLetters and 16x16 would
										// overflow.

		// TODO: Add SavedBoardSize. Possibly several. And if so, mark
		//       one as the default size, even if the player resizes once, we don't
		//       lose the default


//---------------------------------------------------------------------------------------

		public PlayerProfile() {
			PlayerName      = "*Unknown*";
			AllowJudieRules = false;

			Allow_Ss  = false;
			Allow_ING = false;
			Allow_ED  = false;

			MinWordLength = 5;
			MaxWordLength = 12;

			MinimumScore = 0;
			MinimumWords = 0;

			GoalWords = 50;
			GoalScore = 50;

			Rank = 5;
		}
	}
}
