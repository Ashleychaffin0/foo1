// Note: Whenever the "Book" is mentioned, this refers to
//       "C# 6.0 and the .Net 4.6 Framework"

// Task: 
//  *   Loop, prompting the user for the value we want
//      to find the number of primes less than or equal to the
//      specified value
//  *   Quit if the specified number is 0. Give error
//      messages for invalid numbers.
//  *   Count the number of primes using two algorithms
//      (Sieve and use division to find divisors). Display
//      the counts to ensure that they agree.

// Note: Number of primes less than
//      100         -> 25
//      100,000     -> 9,592
//      1,000,000   -> 78,498
//      10,000,000  -> 664,579
//      100,000,000 -> 5,761,455
// Note: Python took 7 seconds for n=100,000. 
//       In 6.75 seconds, C# found primes up to 15,000,000
//       But that 6.75 seconds was with debugging enabled.
//       In "Release" mode, primes up to 15,000,000 were found in 4.2 seconds!

using System;
// Some of the following <using> statements were generated
// by Visual Studio when you created this project. The ones
// that are grayed out indicate that none of the classes in
// those namespaces were used by the program, and thus can
// be deleted (although there's no penalty for their being there).
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCount1 {
	class PrimeCount1 {
		static void Main(string[] args) {
			while (true) {
				Console.Write("Enter prime count limit (0 to quit): ");
				string s = Console.ReadLine();
				// Get rid of any commas in the input, just to let
				// the user type in, say, "1,000,000".
				// Side note: <s.Replace(...)> returns a new
				// string value, but does not modify the original
				// variable, <s>. Hence we have to write
				// s = s.Replace(...) to change <s>.
				s = s.Replace(",", "");
				// The TryParse static method of the <int> class
				// returns a bool indicating whether it's been
				// given a valid integer. But it also wants to 
				// return the actual integer value (if one is found).
				// So it uses the <out> modifier to indicate that
				// the value is to be placed in (in this case) <num>.
				// In C# 6.0 you'd have to write two separate statements:
				//      int num;        // Pre-declare output variable
				//      if (! int.TryParse(s, out num)) ...
				// C# 7.0 allows the two statements to be consolidated
				// into one. Since the Book goes only up to C# 6.0 you
				// won't find this syntax there.
				if (! int.TryParse(s, out int num)) {
					Console.WriteLine("Not a valid integer");
					continue;
				}
				if (num == 0) {         // Our quit signal
					return;
				}
				if (num < 0) {
					Console.WriteLine("Number must be > 0");
					continue;
				}
				CountPrimesUpTo(num);
			}
		}

		private static void CountPrimesUpTo(int limit) {
			// Handle trivial cases
			switch (limit) {
			case 1:
				Console.WriteLine("1 is not considered a prime, so the answer is 0");
				break;
			case 2:
				Console.WriteLine("2 is the only even prime, so the answer is 1");
				break;
			default:
				var sw = new Stopwatch();
				sw.Start();
				int nPrimeDiv = CountByDivision(limit);
				sw.Stop();
				// The original version of C# would require you to write
				// Console.WriteLine("Count by Division = {0:N0}", nPrimeDiv);
				// where the {0:N0} would be somewhat like the "%" syntax
				// in C/C++'s printf string. {0} would mean the first (zeroeth)
				// parameter after the format string (nPrimeDiv). {1}
				// would mean the second parameter (if any), etc. The ":N0"
				// formats the data with digit separators (e.g. "1,234"
				// instead of "1234") and no (the 0 in N0) decimal places
				// displayed. 
				// But the <$"..."> syntax (note the $ immediately before
				// the opening quote) allows String Interpolation
				// (Book, page 90) which mostly obsoletes the {0} syntax
				// (but which is still supported for backward compatibility
				// with existing code)
				Console.WriteLine($"Count by Division = {nPrimeDiv:N0} in {sw.Elapsed}");

				sw.Restart();
				int nPrimeSieve = CountBySieve(limit);
				sw.Stop();
				Console.WriteLine($"Count by Sieve    = {nPrimeSieve:N0} in {sw.Elapsed}");

				sw.Restart();
				int nPrimeSieve2 = CountBySieve2(limit);
				sw.Stop();
				Console.WriteLine($"Count by Sieve2   = {nPrimeSieve2:N0} in {sw.Elapsed}");
				break;
			}
		}

		private static int CountByDivision(int limit) {
			int numPrimesSoFar = 1;      // We know num > 2
			// Check all odd numbers up to and including <num>
			for (int PotentialPrime = 3; PotentialPrime <= limit; PotentialPrime += 2) {
				// In a nod to performance, we'll just check
				// potential divisors up to the square root of <n>.
				int DivLimit = (int)Math.Sqrt(PotentialPrime);
				bool bGotDivisor = false;
				// But despite the performance nod above, 
				// I'm not so worried about performance
				// since our real goal is to learn C#. So we'll
				// just divide by all odd numbers, not just
				// all odd primes
				for (int Divisor = 3; Divisor <= DivLimit; Divisor += 2) {
					if ((PotentialPrime % Divisor) == 0) {
						bGotDivisor = true;
						break;       // Can't be prime
					}
				}
				if (! bGotDivisor) {
					++numPrimesSoFar;
				}
			}
			return numPrimesSoFar;
		}

		private static int CountBySieve(int limit) {
			// Note: Could also have used bool[]. Or string[].
			//       Or even, I suppose, double[]. But byte[]
			//       is probably the most efficient (not that
			//       (as mentioned above) we're overly concerned
			//       with performance. But bool[] may involved
			//       bit twiddling (which would slow us down)
			//       and anything wider than a 1-byte field
			//       could incur cache miss hits.
			byte[] Sieve = new byte[limit + 1];
			// Initialize slots. We have <num + 1> of them
			// since (for simplicity of programming) we
			// include a slot for 0.
			for (int i = 0; i < limit + 1; i++) {
				Sieve[i] = 1;
			}

			Sieve[0] = Sieve[1] = 0;    // Neither of these is prime

			// TODO: Really should do a separate pass for n==2,
			//		 then have this next loop doing just odd numbers
			for (int i = 2; i <= Math.Sqrt(limit); i++) {
				for (int j = 2 * i; j < Sieve.Length; j += i) {
					Sieve[j] = 0;
				}
			}

			// OK, the fun's over. Count the number of 1's
			// Here's the traditional (i.e. hard) way. Loop,
			// incrementing a counter
			int nPrimes1 = 0;
			for (int i = 0; i < Sieve.Length; i++) {
				if (Sieve[i] == 1) {
					++nPrimes1;
				}
			}

			// A slightly simpler way
			int nPrimes2 = 0;
			foreach (byte item in Sieve) {
				nPrimes2 += item;
			}

			// This way uses an advanced C# feature
			// called LINQ (Language Integrated Query).
			// See Chapter 12 (page 439) in the Book

			var qry = from slot in Sieve
					  where slot == 1
					  select slot;
			int nPrimes3 = qry.Count();

			// An arguably simpler way of using LINQ
			// in this particular case. The "=>" symbol
			// represents a "lambda expression", a function
			// with no name, but a body. In this case the
			// lambda takes one parameter I call "slot",
			// compares it to 1, and returns a bool. The
			// Count method returns the number of elements
			// that satisfy the Boolean condition.
			// Lambda expressions are a compact way of
			// writing Anonymous Delegates. Delegates
			// (Anonymous and otherwise) are covered in
			// Chapter 10 of the book, starting on page
			// 355, although lambda expressions per se
			// start on page 388.
			int nPrimes4 = Sieve.Count(slot => slot == 1);

			// And the tersest of all...
			int nPrimes5 = Sieve.Sum(slot => slot);

			// We *could* check that nPrimes{1..5} are all
			// the same value, but I'm not going to bother

			return nPrimes1;
		}

		private static int CountBySieve2(int limit) {
			// This code is a copy of CountBySieve except
			// that we use a vector of bool, rather than int.
			// I'm curious to see what impact on performance
			// this has.
			// Since the code is essentially the same as
			// CountBySieve, I've removed the comments.
			var Sieve = new bool[limit + 1];
			for (int i = 0; i < limit + 1; i++) {
				Sieve[i] = true;
			}

			Sieve[0] = Sieve[1] = false;    // Neither of these is prime

			for (int i = 2; i <= Math.Sqrt(limit); i++) {
				for (int j = 2 * i; j < Sieve.Length; j += i) {
					Sieve[j] = false;
				}
			}

			// OK, the fun's over. Count the number of 1's
			// Here's the traditional (i.e. hard) way. Loop,
			// incrementing a counter
			int nPrimes1 = 0;
			for (int i = 0; i < Sieve.Length; i++) {
				if (Sieve[i]) {
					++nPrimes1;
				}
			}

			// A slightly simpler way
			int nPrimes2 = 0;
			foreach (var item in Sieve) {
				nPrimes2 += item ? 1 : 0;
			}

			// This way uses an advanced C# feature
			// called LINQ (Language Integrated Query).
			// See Chapter 12 (page 439) in the Book

			var qry = from slot in Sieve
					  where slot
					  select slot;
			int nPrimes3 = qry.Count();

			// An arguably simpler way of using LINQ
			// in this particular case. The "=>" symbol
			// represents a "lambda expression", a function
			// with no name, but a body. In this case the
			// lambda takes one parameter I call "slot",
			// compares it to 1, and returns a bool. The
			// Count method returns the number of elements
			// that satisfy the Boolean condition.
			// Lambda expressions are a compact way of
			// writing Anonymous Delegates. Delegates
			// (Anonymous and otherwise) are covered in
			// Chapter 10 of the book, starting on page
			// 355, although lambda expressions per se
			// start on page 388.
			int nPrimes4 = Sieve.Count(slot => slot);

			// We *could* check that nPrimes1-4 are all
			// the same value, but I'm not going to bother

			return nPrimes1;
		}
	}
}
