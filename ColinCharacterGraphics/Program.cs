using System;

namespace ColinCharacterGraphics {
	class Program {
		static void Main() {
			var can = new Canvas(25, 100);  // 25 rows by 100 columns. So x ranges from 0..24
											// and y ranges from 0..14

			// var l1 = new Line(can, 5, 13);	// Line starts at x=5, y=13
			// l1.DrawTo(new Point(10, 3));    // Draw line from (5,1) to (10,3)
			// l1.DrawTo(new Point(12, 4), '*');
			// can.Draw("First lines");

			can.Erase();
			// can.Draw("Start over");

			// var l1 = new Line(can, 5, 5);
			// l1.DrawTo(new Point(0, 0), 'a');
			// can.Draw("Simple -- From (5,5) to (0,0)");
			// l1.Erase();
			// can.Draw("Line should be erased");

			// var l2 = new Line(can, new Point(12, 4), ConsoleColor.Yellow);
			// l2.DrawTo(new Point(3, 14), 'a');
			// can.Draw("Draw from (12,4) to (3,14) -- first way");

			var l3 = new Line(can, 10, 4, ConsoleColor.Red);
			l3.DrawTo(new Point(12, 4), 'a').DrawTo(new Point(3, 14), '1');
			can.Draw("Second way to join the lines");
		}

		/*	The compilation process for Java and C#
			javac foo.java
				=> foo.class
			java foo.class
		-------------------------
			csc foo.cs
				=> foo.dll
			// The next line can be run days (weeks? years?) later
			dotnet foo.dll
			// Or alternatively days (weeks centuries?) later call a previously-compiled version of your program 
				with (on a windows machine anyway)...
			foo.exe
		*/
	}
}