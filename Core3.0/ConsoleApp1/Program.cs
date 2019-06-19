using System;
using System.Text.RegularExpressions;

namespace ConsoleApp1 {
	class Program {
		static void Main(string[] args) {
			Console.WriteLine("Hello World!");

			var re = new Regex(@"\d+(?'*sadf'asdf)");
		}
	}
}
