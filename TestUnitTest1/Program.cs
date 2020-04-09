using System;

namespace TestUnitTests {
	public partial class TestUnitTest1  {
		public static void Main() {
			var p = new Point(3, 4);
			var dist = p.Distance();
			Console.WriteLine($"Distance = {dist}");
		}
	}

	public class Point {
		int x, y;

		public Point(int x, int y) {
			this.x = x;
			this.y = y;
		}

		public double Distance() => Math.Sqrt(x * x + y * y);
	}
}
