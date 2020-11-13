

using System;
using System.Collections.Generic;
//using Extreme.Mathematics;                  // as per: https://www.extremeoptimization.com/QuickStart/CSharp/BasicVectors.aspx

namespace LRS010_Bubble_Sort {
	class Program {
		static void Main() {
			Console.WriteLine("Program starts...");

			//var ints = Vector.Create();     // as per: https://www.extremeoptimization.com/QuickStart/CSharp/BasicVectors.aspx
			//int[] ddata = new int[] { 11, 32, 9, -12, 19, 23, 0, 42, 573 };    // a new int array
			//List<int> data = new List<int> { 11, 32, 9, -12, 19, 23, 0, 42, 573, -1 };

			Console.WriteLine("How many integers would you like to sort?");
			string total_str = Console.ReadLine();
			bool bOK = int.TryParse(total_str, out int total);
			if (!bOK) {
				Console.WriteLine("Input not numeric");
				return;
			}

			List<int> data = Gen_data(total);
			DumpData("The random, unsorted list of integers is: ", data);

			List<int> sorted_data = Sort(data);

			DumpData("\n\nThe same list sorted lowest to highest is: ", sorted_data);
		}

//---------------------------------------------------------------------------------------

		private static List<int> Gen_data(int total) {
			var data = new List<int> { };
			var rand = new Random();
			for (int i = 0; i < total; i++) {
				data.Add(rand.Next(-100, 100));
			}
			return data;
		}

//---------------------------------------------------------------------------------------

		private static List<int> Sort(List<int> data) {
			// var data2 = new int[data.Count];
			// int[] sorted_data = new int[] { };      // quite the realization, "arrays cannot be resized" https://stackoverflow.com/questions/25806361/create-empty-array-and-add-value-causes-array-index-is-out-of-range
			var sorted_data = new List<int>(data.Count) { };
#if true
			sorted_data.Add(data[0]);
			int sortedLength = 1;

// Algorithm:
//	*	Create empty list, sorted_data
//	*	Make data.Count passes through the data, setting Curval to the current value
//	*	In each pass, go through the sorted data, comparing each value with CurVal
//		*	If CurVal is >= all values in sorted_data, append it

			for (int pass = 1; pass < data.Count; pass++) {
				int CurVal = data[pass];
				Console.WriteLine($"Pass {pass}; Curval = {CurVal}");
#else
			foreach (int CurVal in data) {
				if (sortedLength == 0) {
					sorted_data.Add(CurVal);
					++sortedLength;
					continue;
				} else {
#endif
				bool bInserted = false;
				Console.WriteLine($"Scanning current sorted_data for the largest index >= {CurVal}");
				for (int ixLarge = 0; ixLarge < sortedLength; ixLarge++) {
					Dbg1(sorted_data, CurVal, ixLarge);

					if (CurVal < sorted_data[ixLarge]) {       // Insert the new integer into 'sorted_data' if it
																// finally hits an integer in there that is larger
						Dbg2(CurVal, ixLarge);
						sorted_data.Insert(ixLarge, CurVal);
						bInserted = true;
						DumpData("Sorted data is now:", sorted_data);
						break;
					}
#if false
					else if (sortedLength == InsPoint + 1) {      // Insert the new integer at the end of
																	// 'sorted_data' if there wasn't a larger
																	// integer already in there
						sorted_data.Add(CurVal);
						break;
					}
#endif
				}
				if (!bInserted) {
					sorted_data.Add(CurVal);
					DumpData("Inserted new value at the end: ", sorted_data);
				}
				++sortedLength;
				DumpData($"End of pass {pass}, sortedlentgh={sortedLength}:", sorted_data);
				Console.WriteLine();
			}
			return sorted_data;
		}

//---------------------------------------------------------------------------------------

		private static void Dbg2(int CurVal, int ixLarge) {
			Console.WriteLine($"Inserting {CurVal} at {ixLarge}");
		}

//---------------------------------------------------------------------------------------

		private static void Dbg1(List<int> sorted_data, int CurVal, int ixLarge) {
			Console.WriteLine($"CurVal={CurVal}, ixLarge={ixLarge}, sorted_data[ixLarge]={sorted_data[ixLarge]}");
		}

//---------------------------------------------------------------------------------------

		private static void DumpData(string msg, List<int> data) {
			Console.Write(msg);
			string sep = "";
			for (int i = 0; i < data.Count; i++) {
				Console.Write($"{sep} {data[i]}");
				sep = ",";
			}
			Console.WriteLine();
		}
	}
}
