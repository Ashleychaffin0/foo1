using System;
using System.Collections.Generic;
using System.Text;

using MiscClasses.com.Bartizan;

namespace MiscClasses {
	class TestMiscClasses {

		static void Main(string[] args) {
			TestEnumerableBuffer test = new TestEnumerableBuffer();
		}
	}

	public class TestEnumerableBuffer {
		int		nFilled = 0;

		public TestEnumerableBuffer() {
			EnumerableBuffer<int>	buf = new EnumerableBuffer<int>(new EnumerableBuffer<int>.FillBuffer(BufFill));
			foreach (int i in buf) {
				Console.WriteLine("Value = {0}", i);
			}
			Console.WriteLine("Done");
		}

		private List<int> BufFill() {
			switch (nFilled) {
			case 0:
				++nFilled;
				return new List<int>(new int[] {3, 1, 4});
			case 1:
				++nFilled;
				return new List<int>(new int[] {1, 5});
			default:
				return null;
			}
		}
	}
}
