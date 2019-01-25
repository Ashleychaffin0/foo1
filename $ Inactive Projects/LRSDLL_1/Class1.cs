using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LRSDLL_1 {
	public class Hello1 {
		string	Person;

//---------------------------------------------------------------------------------------

		public Hello1(string s) {
			Person = s;
		}

//---------------------------------------------------------------------------------------

		public int Hiya(string s) {
			DoHiya(s);
			return s.Length;
		}

//---------------------------------------------------------------------------------------

		public int Hiya() {
			DoHiya(Person);
			return Person.Length;
		}

//---------------------------------------------------------------------------------------

		private void DoHiya(string who) {
			Console.WriteLine("Hello " + who);
			;
		}
	}
}
