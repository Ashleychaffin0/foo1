using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AhmedBuckets {
	class Ahmed {

		public int	nElements;				// e.g. 30,000 or 50,000
		public int	MaxKey;					// Keys run from 0..MaxKey (e.g. 5,000)
											// Note: This is for the simulation only
		List<int>	Input;

//---------------------------------------------------------------------------------------

		public Ahmed(int nElements, int MaxKey) {
			this.nElements = nElements;
			this.MaxKey    = MaxKey;

			Input = new List<int>(nElements);
			var rnd = new Random();
			for (int i = 0; i < nElements; i++) {
				Input.Add(rnd.Next(MaxKey));
			}

			Console.WriteLine("Using {0} CPU(s)", Environment.ProcessorCount);
		}

//---------------------------------------------------------------------------------------

		public void Test() {
			var sw = new Stopwatch();

			sw.Start();
			var Counts1 = Test1();
			sw.Stop();
			PrintResults(Counts1, sw, "Results of the naive algorithm");

			sw.Restart();
			var Counts2 = Test2();
			PrintResults(Counts2, sw, "Results of ThreadPool -- not implemented");

			sw.Restart();
			var Counts3 = Test3();
			PrintResults(Counts3, sw, "Results of Parallel.For");
		}

//---------------------------------------------------------------------------------------

		public Dictionary<int, int> Test1() {
			var Counts = new Dictionary<int, int>();		// We know there are roughly this number of buckets
			foreach (var key in Input) {
				UpdateCountsNonThreaded(Counts, key);
			}
			return Counts;
		}

//---------------------------------------------------------------------------------------

		private static void UpdateCountsNonThreaded(Dictionary<int, int> Counts, int key) {
			int value;
			if (Counts.TryGetValue(key, out value)) {
				Counts[key] = ++value;
			} else {
				Counts[key] = 1;
			}
		}

//---------------------------------------------------------------------------------------

		public Dictionary<int, int> Test2() {
			var Counts = new Dictionary<int, int>();		// We know there are roughly this number of buckets
			ThreadPool.SetMaxThreads(Environment.ProcessorCount, 0);
			// I didn't get a chance to finish this.
			// Basically, carve the input data up into <n> ranges, where <n> is the number
			// of processors you've got (e.g. 4). Instantiate <n> instances a utility class
			// with a starting and ending index into the Input Data. For example, if you
			// have 4,003 buckets, and Util[0].Start = 0; Util[0].End = 999; And so on
			// up to Util[3].Start = 3000; Util[3].End = -1; This latter .End means to
			// continue on until the end of the data.
			//
			// Then loop <n> times, calling ThreadPool.QueueUserWorkItem(), passing
			// Util[i] to each thread.
			//
			// IOW, assign a range for each thread to work through, with the final thread
			// processing perhaps a bit more than the others, since you don't know exactly
			// how many buckets you'll have.
			//
			// You'll probably have to use method UpdateCountsWithLocks below.
			return Counts;
		}

//---------------------------------------------------------------------------------------

		private void UpdateCountsWithLock(Dictionary<int, int> Counts, int key) {
			int value;
			lock (this) {
				if (Counts.TryGetValue(key, out value)) {
					Counts[key] = ++value;
				} else {
					Counts[key] = 1;
				}
			}
		}

//---------------------------------------------------------------------------------------

		public ConcurrentDictionary<int, int> Test3() {
			// Needs .Net 4 and "using System.Collections.Concurrent;"
			int ConcurrencyLevel = Environment.ProcessorCount;
			var Counts = new ConcurrentDictionary<int, int>(ConcurrencyLevel, 5000);		// We know there are roughly this number of buckets
			// Needs "using System.Threading.Tasks;"

			// Note: In principle, Parallel.For should be just a little bit faster than
			//		 Parallel.ForEach since you don't have to go through the Enumerator.
			//		 Test it each way yourself.
#if false
			Parallel.ForEach(Input, key => UpdateCountsThreaded(Counts, key));
#else
			Parallel.For(0, Input.Count, key => UpdateCountsThreaded(Counts, key)); 
#endif
			return Counts;
		}

//---------------------------------------------------------------------------------------

		private static void UpdateCountsThreaded(ConcurrentDictionary<int, int> Counts, int key) {
			Counts.AddOrUpdate(key, 1, (Key, OldValue) => OldValue + 1);
		}

//---------------------------------------------------------------------------------------

		private static void PrintResults(IDictionary<int, int> Counts, Stopwatch sw, string msg) {
			Console.WriteLine("\r\n\r\n{0} {1}", sw.Elapsed, msg);
			foreach (int key in Counts.Keys) {
				// Console.WriteLine("{0,7:N0}: {1}", key, Counts[key]);
			}

			// TODO: Especially for the threaded versions, you could/should add code
			//		 here to add up all the Values in Counts and make sure they add up
			//		to <nElements>.
		}
	}
}
