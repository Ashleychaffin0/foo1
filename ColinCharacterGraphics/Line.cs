using System;
using System.Collections.Generic;

namespace ColinCharacterGraphics {
	/// <summary>
	/// A class encapsulating drawing a line
	/// </summary>
	class Line {
		readonly Canvas			Can;		// The canvas this line is drawn on
		Point					Start;		// Where the line starts
		readonly ConsoleColor	Color;		// What color to draw the line
		readonly List<Point>	Points;		// Keep track of the points in this line so
											//   we can erase the line later.

//---------------------------------------------------------------------------------------

		/// <summary>
		/// ctor
		/// </summary>
		/// <param name="can">The canvas to create the line in</param>
		/// <param name="start">The starting coordinates of the line</param>
		public Line(Canvas can, Point start, ConsoleColor color = ConsoleColor.White) {
			Can    = can;
			Start  = start;
			Color  = color;
			Points = new List<Point>() { Start };
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Canvas Constructor
		/// </summary>
		/// <param name="can">The canvas to create the line in</param>
		/// <param name="start">The starting coordinates of the line</param>
		public Line(Canvas can, int x, int y, ConsoleColor color = ConsoleColor.White) {
			Can    = can;
			Start  = new Point(x, y);
			Color  = color;
			Points = new List<Point> { Start };
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Draws a line on the canvas, starting from Start to end
		/// </summary>
		/// <param name="end">The endpoint of the line</param>
		/// <param name="c">the character to use to mark the points on the line</param>
		/// <returns>The line</returns>
		public Line DrawTo(Point end, char c = '.') {
			// TODO: From (12,4) to (3, 14)
			// The basic idea is to march the x-coordinate from left to right,
			// calculating the corresponding y-coordinate and setting that pixel.
			// Note however that a vertical line always has the same x-coodinate
			// so our loop would stop after a single point. So we'll handle the two
			// cases separately.

			// TODO: Next line wrong?
			// Can.SetPixel(Start, Color, c);	// Display the starting point
			double slope = Slope(Start, end);   // The m in y = mx + b
			double yintercept = Start.Y - slope * Start.X;  // The b
			Console.WriteLine($"intercept = {yintercept}");
			
			// if (Start.X > end.X) { slope = -slope; }
			// Get start/end point, depending on which is to the left of the other
			int xfrom = Math.Min(Start.X, end.X);
			int xto   = Math.Max(Start.X, end.X);
			if (xfrom == xto) {
				DrawVerticalLine(end);
			} else {
				Draw(xfrom, xto, slope, yintercept, c);
				Points.Add(end);
				// TODO: Delete -- Can.SetPixel(end, Color, c);
			}
			Start = end;		// For DrawTo(x,y).DrawTo(p,q) support
			return this;
		}

//---------------------------------------------------------------------------------------

			/// <summary>
			/// Draws a non-vertical line
			/// </summary>
			/// <param name="from">The starting x coordinate + 1 (we handle the starting
			/// x coordinate separately)</param>
			/// <param name="to">The penultimate x coordinate (we handle the ending
			/// x coordinate separately</param>
			/// <param name="slope">The increment (or decrement) to be added to
			/// the y coordinate for each point</param>
			/// <param name="y">The beginning y coordinate</param>
			/// <param name="c">The character to draw each point</param>
		private void Draw(int from, int to, double slope, double y, char c) {
			for (int x = from; x < to; x++) {
				int y_as_int = Convert.ToInt32(Math.Round(y, MidpointRounding.ToEven));
				Points.Add(new Point(x, y_as_int));
				Can.SetPixel(x, y_as_int, Color, c);
				c++;			// TODO:
				y += slope;
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Draws a vertical line from the Start to the end
		/// </summary>
		/// <param name="end">The endpoint of the (vertical) line</param>
		private void DrawVerticalLine(Point end) {
			throw new NotImplementedException();
		}

//---------------------------------------------------------------------------------------

		public void Erase() {
			foreach (var p in Points) {
				// Note: if two shapes draw on the same point, and the other shape is
				//		 drawn second, this will erase the point on both shapes
				Can.SetPixel(p, Color, ' ');
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Given two Points, calculate the slope of the line connecting them
		/// </summary>
		/// <param name="start">The starting Point</param>
		/// <param name="end">The ending Point</param>
		/// <returns></returns>
		private float Slope(Point start, Point end) {
			// The slope of a line is (y2 - y1) / (x2 - x1)
			float  HorizontalDistance = end.X - start.X;
			float  VerticalDistance   = end.Y - start.Y;
			return VerticalDistance / HorizontalDistance;
		}
	}
}
