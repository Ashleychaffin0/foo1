// Copyright (c) 2007-2009 by Larry Smith

// "I inspected my imagination. He was right. It boggled." 
// -- "Right Ho, Jeeves", (c) 1934, last sentence of Chapter 16.
// -- P.G. Wodehouse

// TODO:

//	*	Generalize Addword - add bool for Player/Program
//	*	Get Player Logic working (e.g. must click adjacent square)
//	*	Get rid of GroupBox. Fix margins a bit.
//	*	Support rotating the board, especially with Recall and clicking listbox
//	*	After clicking word in Listbox to display the word, restore highlights
//	*	Icons. Add one to display Program words or not
//	*	Menu - Replay
//	*	Print
//	*	Get rid of *Done* boxes. Use Cursors.Wait. Maybe disable menu items until done.
//		(Except Stop and Exit)
//	*	Menu - Setup
//	*	Thermometers
//	*	Options dialog. Serialize them. Have top-level dialog that lets you pick the
//		person, etc. Tabs? Combo box? The latter, for CE reasons
//	*	CE-ize it, esepcially sizing and rotating the screen. Also maybe get rid of
//		status bar and icons.
//	*	Status bar (especially for mini-help)
//	*	Recall last word
//	*	Help file?
//	*	Option to highlight letters during generation. Need Stop button
//	*	Handle dynamic font size better
//	*	TrackLetters needs Click OK box to continue, as current version does
//	*	Web browser support. Maybe separate process (e.g. for CE). Maybe on "Is word
//		legal" dialog?
//	*	Clean up code
//	*	Use ControlPaint.DrawBorder[3D]

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Boondog2009 {
	public partial class Boondog2009 : Form {
		PlayerProfile   profile;
		Board           board;
		Game            game;

//---------------------------------------------------------------------------------------

		public Boondog2009() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			// TODO: Save game?
			Application.Exit();
		}

//---------------------------------------------------------------------------------------

		protected override void OnCreateControl() {
			// TODO: Try to get this working
			CreateParams cp = this.CreateParams;
			const int WS_CLIPCHILDREN	= 0x02000000;
			cp.Style |= WS_CLIPCHILDREN;
			// cp.Style |= 0x00000040; // BS_ICON value

			base.OnCreateControl();
		}

//---------------------------------------------------------------------------------------

		private void Boondog2009_Load(object sender, EventArgs e) {

			// this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

			// TODO: Handle multiple players; save/restore profiles; etc
			profile = new PlayerProfile();

			// TODO: Get font info from profile
			Font	fnt = new Font("Times New Roman", 12);
			BogSquare.Init(this, fnt);

			// lblPlayerStats.Text = "Words: 23\nScore: 123\nAvg: 12.1";

			// TODO: Will throw if dictionary not available
			GameDictionary dict = new GameDictionary("BOONDOG.DCT"); 
			// dict.FindWord("AZZ");

#if false
			foreach (GameDictionary.DictHeader hdr in dict) {
				Console.WriteLine("DictHeader - {0}, {1}", hdr.LetterEntryOffset, hdr.LetterEntryLength);
			}
#endif
			board = new Board(this, grpBogBox, profile, dict);
			
			game = new Game();

			lbProgramWords.Click += new EventHandler(lbProgramWords_Click);
	   }

//---------------------------------------------------------------------------------------

		void lbProgramWords_Click(object sender, EventArgs e) {
			ListBox	lb = sender as ListBox;
			BogWord bw = (BogWord)lb.Items[lb.SelectedIndex];
			board.Highlight(bw.TrackLetters);
		}

//---------------------------------------------------------------------------------------

		private void Boondog2009_Resize(object sender, EventArgs e) {
			if (board != null) {
				board.Resize(grpBogBox);
			}
		}

//---------------------------------------------------------------------------------------

		internal void ResetAuxControls() {
			lbPlayerWords.Items.Clear();
			lbProgramWords.Items.Clear();
			// TODO: Reset other controls
		}

//---------------------------------------------------------------------------------------

		internal bool AddWord(bool bIsProgram, string word, byte [] TrackLetters, int TrackLen) {
			if (word.Length < profile.MinWordLength) {
				return false;
			}
			ListBox	lb = bIsProgram ? lbProgramWords : lbPlayerWords;
			// See if the word is already in the listbox. We can't just use
			// Items.Contains since what's there isn't a string, it's a BogWord.
			bool	bFound = false;
			foreach (BogWord b in lb.Items) {
				if (word == b.word) {
					bFound = true;
					break;
				}
			}
			if (bFound) {
				return false;
			}
			BogWord	bword = new BogWord(word, TrackLetters, TrackLen);
			lb.Items.Add(bword);
			if (bIsProgram) {
				++game.ProgramWords;
				game.ProgramScore += game.Score(word);
				SetStats(lblProgramStats, game.ProgramWords, game.ProgramScore);
		   } else {
				++game.PlayerWords;
				game.PlayerScore += game.Score(word);
				SetStats(lblPlayerStats, game.PlayerWords, game.PlayerScore);
		   }
			return true;			// TODO:
		}

//---------------------------------------------------------------------------------------

		private void SetStats(Label lblStats, int nWords, int score) {
			string	txt = string.Format("Words: {0}\nScore: {1}\nAvg: {2:N1}",
				nWords, score, (float)score / nWords);
			lblStats.Text = txt;
		}

//---------------------------------------------------------------------------------------

		internal void DisplayWordLengths() {
			Dictionary<int, int>	Lengths = new Dictionary<int,int>();
			int []		lens;
			GetWordListLengths(lbPlayerWords, Lengths, out lens);

			// TODO: GetWordListLengths must be called first to set lengths. Poor design.
			//		 Fix later.
			StringBuilder	sb = new StringBuilder();
			sb.Append("Word Lengths");
			for (int i = 0; i < lens.Length; i++) {
				sb.AppendFormat("{0,5}", lens[i]);
			}

			sb.Append("+\r\n      Player");
			foreach (int len in lens) {
				sb.AppendFormat("{0,5}", Lengths[len]);
			}

			GetWordListLengths(lbProgramWords, Lengths, out lens);
			sb.Append("\r\n     Program");
			foreach (int len in lens) {
				sb.AppendFormat("{0,5}", Lengths[len]);
			}

			lblWordCounts.Text = sb.ToString();
		}

//---------------------------------------------------------------------------------------

		private void GetWordListLengths(ListBox lbProgramWords, 
							Dictionary<int, int> Lengths, out int [] lens) {
			int		MinLen = profile.MinWordLength;
			int		MaxLen = 8;				// Right now, the dict has only 8-char words,
											//   but the player might have longer
			Lengths.Clear();
			for (int i = MinLen; i <= MaxLen; i++) {
				Lengths[i] = 0;
			}
			foreach (BogWord bw in lbProgramWords.Items) {
				int	len = Math.Min(bw.word.Length, MaxLen);
				Lengths[len] = Lengths[len] + 1;
			}
			lens = new int [Lengths.Keys.Count];
			Lengths.Keys.CopyTo(lens, 0);
			Array.Sort(lens);
		}
	}
}