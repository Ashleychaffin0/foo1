using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
