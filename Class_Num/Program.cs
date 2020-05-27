using System;

namespace Class_Num {
	class Program {
		static void Main(string[] args) {
			var NumInt1    = new Num(3);
			var NumInt2    = new Num(5);
			var NumFloat1  = new Num(3.14f);

			// var foo = NumInt1 + NumInt2;
			// var foo = Num.operator$$Plus(NumInt1, NumInt2);

			Console.WriteLine($"{NumInt1} + {NumInt2} = {NumInt1 + NumInt2}");
			// operator$$Plus(NumInt1, NumInt2);
			Console.WriteLine($"{NumInt1} + {NumFloat1} = {NumInt1 + NumFloat1}");
			Console.WriteLine($"{NumFloat1} + {NumInt1} = {NumFloat1 + NumInt1}");
		}
	}

	class Num {
		readonly object num;

//---------------------------------------------------------------------------------------

		public Num(object num) {
			this.num = num;
		}

//---------------------------------------------------------------------------------------

		public override string ToString() {
			return num.ToString();			// Note: Lots of conversions under the hood
		}

//---------------------------------------------------------------------------------------

		public static object operator + (Num left, Num right) {
			if ((left.num is int) && (right.num is int)) {
				return HandleIntInt(left, right);
			}

			if ((left.num is int) && (right.num is float)) {
				return HandleIntFloat(left, right);
			}

			return int.MinValue;
		}

//---------------------------------------------------------------------------------------

		private static int HandleIntInt(Num left, Num right) {
			int i1 = Convert.ToInt32(left.num);
			int i2 = Convert.ToInt32(right.num);
			return i1 + i2;
		}

//---------------------------------------------------------------------------------------

		private static float HandleIntFloat(Num left, Num right) {
			int   i1 = Convert.ToInt32(left.num);
			float f1 = Convert.ToSingle(right.num);
			return i1 + f1;
		}

//---------------------------------------------------------------------------------------

		private static float HandleFloatInt(Num left, Num right) {
			float f1 = Convert.ToSingle(left.num);
			int   i1 = Convert.ToInt32(right.num);
			return f1 + i1;
		}
	}
}
