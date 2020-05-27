using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace FindMiddleValue {
	class Program {
		static void Main(string[] args) {
			TestViaFile(3);
			// TestViaYield(100, 3);
		}

//---------------------------------------------------------------------------------------

		private static void TestViaFile(int every) {
			var sr = new StreamReader(@"G:\OneDrive\$Dev\C#\FindMiddleValue\Program.cs");
			string Mid = "";
			int LineNum = 0;
			string line;
			string PrevLine = "";
			while (((line = sr.ReadLine()) != null)) {
				if (++LineNum % every == 0) {
					Mid = PrevLine;
					Debug.WriteLine($"[{LineNum}] Middle line = {Mid}");
					PrevLine = line;
				}
			}
		}

//---------------------------------------------------------------------------------------

		private static void TestViaYield(int limit, int every) {
			int Mid = 0;
			foreach (var item in foo(limit)) {
				if ((item % every) == 0) {
					++Mid;
				}
				Console.WriteLine($"{item} / {Mid}");
			}
		}

//---------------------------------------------------------------------------------------

		private static IEnumerable<int> foo(int n) {
			for (int i = 1; i <= n; i++) {
				yield return i;
			}
		}
	}
}


