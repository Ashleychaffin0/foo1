using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Insanity {
	public partial class Insanity : Form {

		List<Cube> Cubes;

		//---------------------------------------------------------------------------------------

		public Insanity() {
			InitializeComponent();
			int num = 24 * 24 * 24 * 24 * 24;
			Console.WriteLine($"{num:N0}");

			// In this order: Top, Bottom, Left, Right, Front, Back
			Cubes = new List<Cube>();
			Cubes.Add(new Cube("SPSR", "RSSS", "RPSS", "PSSP", "SPRP", "SSSS"));
			Cubes.Add(new Cube("RRPS", "PRSP", "SSPS", "RRRS", "PSPS", "RSSR"));
			Cubes.Add(new Cube("SRPP", "RPPP", "PPPP", "RPRP", "PPRP", "SPPP"));
			Cubes.Add(new Cube("SRSR", "RRRR", "RSRP", "RRRP", "SPRS", "SPRR"));

			// Debug code
			Cube c0 = Cubes[0];
			int degrees = 0;
			foreach (var item in c0.Rotations()) {
				Console.WriteLine($"Rotation {degrees}");
				Console.WriteLine($"\tTop:    {item.Top}");
				Console.WriteLine($"\tBottom: {item.Bottom}");
				Console.WriteLine($"\tLeft:   {item.Left}");
				Console.WriteLine($"\tRight:  {item.Right}");
				Console.WriteLine($"\tFront:  {item.Front}");
				Console.WriteLine($"\tBack:   {item.Back}");
				degrees += 90;

				int n = 0;
				foreach (var x in GetPermutations<char>("ABCD", 4)) {
					Console.Write("{0}: ", ++n);
					foreach (var y in x) {
						Console.Write($"{y}");
					}
					Console.WriteLine();
				}
			}
		}

//---------------------------------------------------------------------------------------

		// From https://stackoverflow.com/questions/756055/listing-all-permutations-of-a-string-integer
		// Note: The <list> parm must have no repeated values
		static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length) {
			if (length == 1) {
				var res1 = list.Select(t => new T[] { t });
				return res1;
			}

			var res = GetPermutations(list, length - 1)
				.SelectMany(t => list.Where(e => !t.Contains(e)),
					(t1, t2) => t1.Concat(new T[] { t2 }));
			return res;

		}
#if false
		This function requires the elements to be unique. Try this: 
		const string s = "HALLOWEEN"; 
		var result = GetPermutations(Enumerable.Range(0, s.Length), s.Length)
			.Select(t => t.Select(i => s[i]));
#endif
	}
}
