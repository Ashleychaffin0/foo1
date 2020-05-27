#define DEBUG_SETPIXEL

using System;
using System.Diagnostics;

namespace ColinCharacterGraphics {
	/// <summary>
	/// A class to represent something to draw on
	/// </summary>
	class Canvas {
		readonly int		Cols;	// Number of Columns (x coordinate)
		readonly int		Rows;	// Number of Rows (y coordinate)
		readonly char[,]	Board;  // As we say in the Bronx, "Da Board"

#if false
		Assume Board is at location 1000 in memory. And we'll make it simple and assume that
		every character takes up exactly 1 byte (it doesn't in C#; it's 2).
		If Board is 1 dimensional, of length 10, then to find Board[5], we add 5 to 1000 and
		get the character at location 1005.
		If Board is 2-dimensional, 1 row, 10 columns, then Board[0, 5] would be at location
		0 * 10 + 5 (where 10 is the length of a row).
		If Board is 2-dimensional, 2 rows by 10 columns, the Board[0, 5] would still be at
		1005. Where would Board[1, 5] be? Answer: at 1000 + 1 * 10 + 5 = 1015.

		Whereas in Python, using lists of lists, Board[1, 5], it would have to look up
		where the first list is stored, then figure out where the second list is, and 
		so on.
#endif

//---------------------------------------------------------------------------------------

		public Canvas(int height, int width) {
			Rows  = height;
			Cols  = width;
			Board = new char[Rows, Cols];
#if false
		In psuedo-Python:
		Board = |[1, 2, 3]
				 [4, 5, 6]
				 [7, 8, 9]|
// Board is now (because of the |syntax| I've just invented) a 3x3 array
#endif
			Erase();
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Blank out the Board
		/// </summary>
		public void Erase() {
			for (int row = 0; row < Rows; row++) {
				for (int col = 0; col < Cols; col++) {
					Board[row, col] = ' ';
				}
			}
		}

//---------------------------------------------------------------------------------------
 
			/// <summary>
			/// Set the specified point to the specified character
			/// </summary>
			/// <param name="x">The x coordinate</param>
			/// <param name="y">The y coordinate</param>
			/// <param name="c">The character to use (defaults to '.'</param>
		public void SetPixel(int x, int y, ConsoleColor color, char c = '.') {
			bool xWrong = (x < 0) || (x >= Cols);
			bool yWrong = (y < 0) || (y >= Rows);
			if (xWrong || yWrong) { return; } // Ignore pixels out of range
			Console.ForegroundColor = color;
			// Here's where it gets a little strange. If you create an array of size
			// [5, 10], it's 5 rows and 10 columns. And we normally consider the first
			// dimension (5) to be the number of rows, and the second (10) to be the number
			// of columns. But in graphics, the x coordinate is actually the column
			// number, and y is the row number. Hence the reversal of parameters in
			// the next line.
			Board[y, x] = c;
#if DEBUG_SETPIXEL
			Console.WriteLine($"Pixel ({x},{y}) '{c}' set");
			if (x == 3) Debugger.Break();	// TODO:
#endif
		}

//---------------------------------------------------------------------------------------

		public void SetPixel(Point p, ConsoleColor color, char c = '.') => SetPixel(p.X, p.Y, color, c);

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Returns the character (aka pixel)  at the specified coordinates
		/// </summary>
		/// <param name="x">The x coordinate</param>
		/// <param name="y">The y coordinate</param>
		/// <returns></returns>
		public char GetPixel(int x, int y) => Board[x, y];	// TODO: [x, y] backwards?

//---------------------------------------------------------------------------------------

			/// <summary>
			/// Draws the current contents of the canvas
			/// </summary>
			/// <param name="caption"></param>
		public void Draw(string caption = "Board ...") {
			Console.WriteLine(caption);
			// Put a border around the Board
			Console.WriteLine("".PadRight(Cols + 2, '-'));	// Top border
			// Note: We ordinarily think of positive x,y coordinates as representing
			//		 the first quadrant of a graph, with y values increasing. But when
			//		 we display the board one row at a time, that's backwards. So that
			//		 it will look right, we'll draw the rows in reverse order, so that
			//		 row 0 will appear at the bottom, and each succeeding row will
			//		 be displayed in ascending order
			for (int row = Rows - 1; row >= 0; row--) {
				Console.Write('|');		// Left border
				for (int col = 0; col < Cols; col++) {
					// TODO: make the FG/BG colors parms, plus default for borders
					Console.Write(Board[row, col]);
				}
				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine('|');	// Right border
			}
			Console.WriteLine("".PadRight(Cols + 2, '-'));	// Bottom border
		}
	}
}
