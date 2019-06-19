using System;
using System.Diagnostics;

namespace IndexMyBookmarks {
	public static class Timing {
		public static Stopwatch TimeIt(Action work) {
			var sw = new Stopwatch();
			sw.Start();
				work();
			sw.Stop();
			return sw;
		}
	}
}
