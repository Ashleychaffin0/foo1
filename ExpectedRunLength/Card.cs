// This is overkill for the ExpectedRunLength program, but I thought I'd show how a more
// general card in a Deck of cards might be modeled. Also note the use of enums.

namespace ExpectedRunLength {
	//---------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------

	class Card {
		public enum Suit {
			// Note that we don't assign values to any of these, so the
			// compiler will default the first one to numeric 0, the next
			// to 1, then 2, then 3
			Spade,
			Heart,
			Diamond,
			Club
		}
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

		public enum Color {
			Red,
			Black
		}
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

		public enum Rank {
			// We don't really need to know that, for example, a Jack
			// is "11". So we'll actually wind up with Ace == 0,
			// Duece == 1, Trey == 3, etc. And, in this program at
			// least, things will work out fine.
			Ace,
			Duece,
			Trey,
			Four,
			Five,
			Six,
			Seven,
			Eight,
			Nine,
			Ten,
			Jack,
			Queen,
			King
		}

//---------------------------------------------------------------------------------------

		public int		n;          // Used only in debug routine TestDeck
		public Suit		suit;
		public Rank		rank;
		public Color	color;

//---------------------------------------------------------------------------------------

		public Card(int n) {		// constructor (ctor)
			this.n = n;
			suit   = (Suit)(n / 13);
			rank   = (Rank)(n % 13);
			color  = ((suit == Suit.Heart) || (suit == Suit.Diamond))
				? Color.Red : Color.Black;
		}

//---------------------------------------------------------------------------------------

		public override string ToString() =>
			$"{n}: {color} {suit} {rank}";
	}
}
