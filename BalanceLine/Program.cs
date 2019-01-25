using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Bartizan;

namespace BalanceLine {
	class Program {
		static void Main(string[] args) {

			var left = new List<int> { 1, 3, 4, 5, 8, 10 };
			var right = new int[] { 2, 4, 6, 8 };
			StringBuilder sb = new StringBuilder();
			BalanceLine<int>.Balance(left, right,
				// (x, y) => { if (x < y) return -1; else if (x == y) return 0; return 1; },
					// or
				//(x, y) => 
				//       x < y ? -1 :
				//       x == y ? 0 :
				//       1,
					// or
				(x, y) => { return x.CompareTo(y); },
				(x, bx, y, by) => {
					string msg = string.Format("x={0}, bx={1}, y={2}, by={3}\r\n",
						x, bx, y, by); 
					// Note use of closure here to access variable <sb>.
					sb.Append(msg);
				}
			);
			string s = sb.ToString();
			Console.WriteLine("{0}", s);
		}
	}
}
