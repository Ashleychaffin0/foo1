// Copyright (c) 2014 by Larry Smith

// TODO: Move over SetupTileScores()

// #define		MANUAL_BOARD_SETUP

using System;

namespace nsBoondog2019 {
	class BogDice {

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Sets up the "Dice" variable. This isn't totally trivial if the rank of the
		/// board is > 5
		/// </summary>
		public static string[] SetupDice(PlayerProfile prof) {
			string[] BasicDice = new string[25] {
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

			string[] SuperBoggleDice = new string[36] {
				"OTOUTO",
				"CNDNLD",
				"HRVITS",
				"MEAEEE",
				"HNHWDO",
				"XKWQ0ZJ",				// 0 = Qu
				"120345",				// 1 = Th, 2 = He, 3 = In, 4 = An, 5 = Er
				"!!EO!I",				// ! = Word Stop
				"IISETL",
				"ESHRLI",
				"LIEANM",
				"TNHDOD",
				"BKZXJB",
				"ETCSCN",
				"NUEOIA",
				"LSTIEP",
				"USENSS",
				"PYIYRS",
				"CSITPE",
				"SAAFRS",
				"RHHLOD",
				"AFIYRS",
				"TTOTME",
				"BODEAI",
				"ETITIC",
				"NOLDHR",
				"EMGAEU",
				"AEDNNN",
				"MANGEN",
				"SEOOEA",
				"AEAEEE",
				"UNFGYC",
				"VORWRG",
				"TSHRPO",
				"SARAIF",
				"WONOTU"
			};

			int nSq = prof.Rank * prof.Rank;
			var Dice = new string[nSq];
			if (prof.Rank <= 5) {
				Array.Copy(BasicDice, Dice, nSq);
				return Dice;
			} else if (prof.Rank == 6) {
				Array.Copy(SuperBoggleDice, Dice, nSq);
				return Dice;
			}

			// Rank is >= 7. We have a choice of how to proceed. We could either choose
			// dice from the BasicDice list (either randomly or by going again from the
			// top), or we can generate random dice. I'm going to try the latter, at 
			// least for now
			// Note: This doesn't work. We get far too many consonants and too few
			//		 vowels, and the consonants can't "connect" through the vowels, and
			//		 we actually get far fewer words than we'd expect. So we'll go back
			//		 to reusing the original dice.


			// TODO: TODO: TODO: We now have == 6 dice
			Array.Copy(BasicDice, Dice, 25);
			Random rand = new Random();

			for (int i = 25; i < nSq; i++) {
				int n = rand.Next(0, 25);
				Dice[i] = BasicDice[n];
			}


#if false  // This is the algorithm that doesn't work.
			for (int i = 25; i < nSq; i++) {
				string	die = "";
				for (int n = 0; n < 6; n++) {
					int	ix = rand.Next(0, 26);
					die += "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring(ix, 1);
				}
				Dice[i] = die;
			}
#endif
			return Dice;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Not to be confused with your booty!
		/// </summary>
		static public bool ShakeDice(PlayerProfile prof, BogCube[,] Cubes) {
			string[] Dice = BogDice.SetupDice(prof);

			// For each die, choose a random spot for it, then choose a random letter 
			// from that die. Make sure we don't put two dice in the same square!
			int rank = prof.Rank;
			int nSquares = rank * rank;
			int nSq;
			bool[] bOccupied = new bool[nSquares];      // All start out as false
			Random rand = new Random();
			foreach (string die in Dice) {
				// In theory, the following is an infinite loop. But it's a pretty poor
				// random number generator that doesn't generate all nSquares values
				// pretty quickly.
				while (true) {
					nSq = rand.Next(0, nSquares);
					if (!bOccupied[nSq]) {
						bOccupied[nSq] = true;
						break;
					}
				}
				int row = nSq / rank;
				int col = nSq % rank;
				string txt = die.Substring(rand.Next(0, 6), 1);
				// if (txt == "Q") {
				// 	txt = "Qu";
				// }
				switch (txt) {
				case "Q":
				case "0":
					txt = "Qu";
					break;
				case "1":
					txt = "Th";
					break;
				case "2":
					txt = "He";
					break;
				case "3":
					txt = "In";
					break;
				case "4":
					txt = "An";
					break;
				case "5":
					txt = "Er";
					break;
				case "!":
					txt = " ";
					break;
				default:
					break;
				}
				Cubes[row, col].Text = txt;
			}

#if MANUAL_BOARD_SETUP // Conditionally set the board to a known configuration for debugging purposes
			string[] letters = { "ADIOS", "ABEDE", "TERAD", "NERAT", "zzzzz" };
			for (int i = 0; i < 5; i++) {
				for (int j = 0; j < 5; j++) {
					Cubes[i, j].Text = letters[i].ToUpper().Substring(j, 1);
				}
			}
			return true;
#else
			return false;
#endif
		}
	}
}
