using System;
using System.Collections.Generic;
using System.Text;

using Bartizan.Input.Utils;

namespace ScanCSV {
	class Program {
		static void Main(string[] args) {
			string				txt;
			List<string>		vals = new List<string>();

			txt = ""; //"A, b,c";
			txt = txt.Replace('$', '"');
			vals.Clear();
			BartCSV.TryParse(txt, out vals);
			DumpList(txt, vals);

			txt = "$Ab$cd$, b,c  ,$";
			txt = txt.Replace('$', '"');
			vals.Clear();
			BartCSV.TryParse(txt, out vals);
			DumpList(txt, vals);
		}

//---------------------------------------------------------------------------------------

		private static void DumpList(string txt, List<string> vals) {
			Console.WriteLine("txt = <{0}>", txt);
			for (int i = 0; i < vals.Count; i++) {
				Console.WriteLine("\t[{0}], L={1}, <{2}>", i, vals[i].Length, vals[i]);
			}
			Console.WriteLine();
		}
	}
}
