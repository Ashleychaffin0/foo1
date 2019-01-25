using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

// This program will simulate shuffling a deck of cards many times,
// looking for the maximum length of a run of either red or black
// cards. This data will be used to calculate the expected length
// of the maximum run length in a random deck.

namespace ExpectedRunLength {
	public partial class ExpectedRunLength : Form {

		// Note that because we're planning to run many thousands of
		// simulatation runs, I'm going to declare one deck and use
		// that repeatedly, obviating the need to constantly create
		// a deck then almost immediately delete it, again and again
		// and again.
		Card[]	Deck;
		int[]	Buckets;

		// In principle we could vary the deck size, just for fun. But then anomalies
		// would arise. For example, with our current logic, we start out with a
		// sorted deck and the first, say, 10 cards are always spades. So we'd
		// *always* get a max (and for that matter, min) run length of 10. And if
		// we made DeckSize = 100, then what color is Deck[80]?
		// I'm not saying these obstacles can't be overcome, but for now the
		// deck size is frozen at 52.
		const int DeckSize = 52;

		Random Rand = new Random();

//---------------------------------------------------------------------------------------

		public ExpectedRunLength() {
			InitializeComponent();

			this.WindowState = FormWindowState.Maximized;
		}

//---------------------------------------------------------------------------------------

		private (int MaxRunIndex, int MaxRunLength) FindLongestRun() {
			var CurColor     = Deck[0].color;
			int CurRunIndex  = 0;		// Where the current run starts
			int CurRunLength = 1;
			int MaxRunIndex  = 0;		// Where the max run starts
			int MaxRunLength = 1;

			for (int i = 1; i < Deck.Length - 1; i++) {
				Card card = Deck[i];
				var color = card.color;
				if (color == CurColor) {
					++CurRunLength;
				} else {
					// Note that we check the MaxRunLength only on a color
					// change. But what if the max run is at the end of
					// the deck? See below.
					if (CurRunLength > MaxRunLength) {	// New max run?
						MaxRunIndex  = Math.Max(CurRunIndex, MaxRunIndex);
						MaxRunLength = CurRunLength;
					}
					CurColor     = color;
					CurRunLength = 1;
					CurRunIndex  = i;
				}
			}

			// See if the max run was at the end of the deck
			if (CurRunIndex + CurRunLength >= Deck.Length) {
				MaxRunIndex  = CurRunIndex;
				MaxRunLength = CurRunLength;
			}

			return (MaxRunIndex, MaxRunLength);
		}

//---------------------------------------------------------------------------------------

		private void Shuffle() {
			// Shuffles the deck of cards
			// From https://en.wikipedia.org/wiki/Fisher–Yates_shuffle
			for (int i = 0; i < Deck.Length - 1; i++) {
				int j = i + Rand.Next(0, Deck.Length - i);
				SwapCards(i, j);
			}

#if false
			bool bOK = TestDeck();
			if (! bOK) {
				System.Diagnostics.Debugger.Break();
			}
#endif
		}

//---------------------------------------------------------------------------------------

		private bool TestDeck() {
			// Make sure all 52 cards are unique
			// Used only during development to ensure that the shuffle routine worked
			bool[] slots = new bool[52];    // Initialized to false
			foreach (var c in Deck) {
				slots[c.n] = true;
			}

			// OK, this requires some explanation, but it will take more than I want to
			// get into at this point. Suffice it to say that the next line will go
			// through all slots and check to see if each entries is "true" and will
			// return true only if all are true.
			return slots.All(p => p == true);
		}

//---------------------------------------------------------------------------------------

		private void SwapCards(int i, int j) {
			var tmp = Deck[i];
			Deck[i] = Deck[j];
			Deck[j] = tmp;
		}

//---------------------------------------------------------------------------------------

		private void BtnGo_Click(object sender, EventArgs e) {
			string txt = TxtIterations.Text.Replace(",", "");	// Allow "," in input
			bool bOK = int.TryParse(txt, out int nIterations);
			if (!bOK || (bOK && (nIterations <= 0))) {
				MessageBox.Show("Invalid number of iterations", "Expected Run Length",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			ResetSimilation();
			DateTime StartTime = DateTime.Now;

			RunTheSimulation(nIterations);
			ShowElapsedTime(DateTime.Now - StartTime);

			ChartBuckets(Buckets);
		}

//---------------------------------------------------------------------------------------

		private void ResetSimilation() {
			Deck = new Card[DeckSize];
			for (int i = 0; i < Deck.Length; i++) {
				Deck[i] = new Card(i);
			}

			int nBucks = (DeckSize + 1) / 2;	// "+1" if nCards is odd
			Buckets = new int[nBucks];  // nCards/2 = max run length, init to 0
		}

//---------------------------------------------------------------------------------------

		private void RunTheSimulation(int nIterations) {
			for (int i = 0; i < nIterations; i++) {
				Shuffle();
				var stats = FindLongestRun();
				++Buckets[stats.MaxRunLength];
			}
		}

//---------------------------------------------------------------------------------------

		private void ChartBuckets(int[] buckets) {
			var RunLengths = ChtStats.Series["Max Run Lengths"];
			RunLengths.Points.Clear();
			ChtStats.Annotations.Clear();
			// Let's show just the data
			int Start = 0;
			for (int i = 0; i < Buckets.Length; i++) {
				if (Buckets[i] > 0) {
					Start = i;
					break;
				}
			}
			int End = 0;
			for (int i = Buckets.Length - 1; i >= 0; i--) {
				if (Buckets[i] > 0) {
					End = i;
					break;
				}
			}
			for (int i = Start; i <= End; i++) {
				RunLengths.Points.AddXY(i, Buckets[i]);
				var c = new CalloutAnnotation() {
					Text	= $"{Buckets[i]:N0}",
					ToolTip = $"{Buckets[i]:N0}"	// Tooltip on the callout
				};
				ChtStats.Annotations.Add(c);
				ChtStats.Annotations[i - Start].AnchorDataPoint = RunLengths.Points[i - Start];
			}
		}

//---------------------------------------------------------------------------------------

		private void ShowElapsedTime(TimeSpan elapsed) {
			LblElapsedTime.Text = elapsed.TotalMilliseconds.ToString("N0");
			Application.DoEvents();
		}
	}
}
