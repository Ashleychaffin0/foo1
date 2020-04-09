using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCallCtorFromCtor {
	class Program {
		static void Main(string[] args) {
			var f = new foo();
		}
	}

	class foo {
		public foo() {
			// foo(0);
		}

		public foo(int n) {
			Console.WriteLine("ctor parm was {0}", n);
		}
	}
}
