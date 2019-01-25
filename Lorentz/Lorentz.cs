using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lorentz {
	class Lorentz {
		const double c = 299792458;

		static void Main(string[] args) {
			var twice = Math.Sqrt(.75);
			double v = 1_000_000;
			double sum = AddVelocities(100, c);
			// v = .96 * c;
			// Console.WriteLine($"v = {v}");
			var b = Beta(v);
			// Console.WriteLine($"Beta() = {b}");
			var gam = Gamma(v);
			// Console.WriteLine($"Gamma() = {gam}");
			// var vs = new List<double> { 1_000_000, 10_000, 100_000, 1_000_000 };
			var vs = new List<double> { 1_000, 1_000_000 };
			var approx = GammaApprox(vs);
			for (int i = 0; i < approx.Count; i++) {
				// Console.WriteLine($"Approx[{i}] = {approx[i]}");
			}
		}

		private static double AddVelocities(double u, double v) {
			return (u + v) / (1 + u * v / (c * c));
		}

		static double Beta(double v) {
			return v / c;
		}

		static double Gamma(double v) {
			var b = Beta(v);
			return 1 / Math.Sqrt(1 - b * b);
		}

		static List<double> GammaApprox(List<double> vs) {
			var Coefficients = new List<double> {
				1.0, 1.0/2, 3.0/8, 5.0/16 /* , 35.0/128, 63.0/256 */ };
			foreach (var v in vs) {
				var b = Beta(v);
				var g = Gamma(v);
				Console.WriteLine($"v = {v:N0} m/sec, Beta = {b}, real gamma = {g:N12}");
				var vals = new List<double>();
				double sum = 1;
				double pow = b * b;
				for (int i = 1; i < Coefficients.Count; i++) {
					sum += Coefficients[i] * pow;
					var pct = (g - sum) / g * 100;
					Console.WriteLine($"\tAfter {i + 1} terms: {sum:n12}, Dif = {pct:N5}%");
					vals.Add(sum);
					pow *= b * b;
				}
				var x = string.Format("N", 6);
			}
			return null;
		}
	}
}
