using System;

namespace ColinCharacterGraphics {
	/// <summary>
	/// A Trivial Point class. Note that we may enhance it later.
	/// </summary>
	class Point {
		public int X;
		public int Y;

//---------------------------------------------------------------------------------------

		public Point(int x, int y) {
			X = x;
			Y = y;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Calculates and returns the distance of this Point from the origin
		/// </summary>
		/// <returns>The distance</returns>
		public double DistanceFromOrigin() {
			return Math.Sqrt(X * X + Y * Y);	// Thank you Pythagoras
		}

//---------------------------------------------------------------------------------------

		public void Offset(int x, int y) {
			// Not necessarily a good idea to change what a Point points to!
			X += x;
			Y += y;
		}

//---------------------------------------------------------------------------------------

		public override string ToString() => $"({X},{Y})";
	}
}
