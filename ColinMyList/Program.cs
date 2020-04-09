using System;

namespace ColinMyList {
	class Program {
		static void Main(string[] args) {
			var ml = new MyList();
			for (int n = 0; n < 10; n++) {
				ml.Add(n);
			}
			for (int i = 0; i < ml.Length; i++) {
				Console.WriteLine(ml[i]);
			}
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class MyList<T> {
		T[] list;
		int NextIndex;
		int NumberOfCopiedElements;

//---------------------------------------------------------------------------------------

		public MyList(int n = 5) {
			list = new T[n];
			NextIndex = 0;
			NumberOfCopiedElements = 0;
		}

//---------------------------------------------------------------------------------------

		public int Length { get => NextIndex - 1; }

//---------------------------------------------------------------------------------------

		public int ElementsCopiedCount { get => NumberOfCopiedElements; }

//---------------------------------------------------------------------------------------

		public int Add(T value) {
			if (NextIndex >= list.Length) {
				int newlength = list.Length + 1;   // Make room for another
				Console.WriteLine($"Copying {list.Length} elements");
				NumberOfCopiedElements += list.Length;
				T[] newlist = new T[newlength];
				for (int i = 0; i < list.Length; i++) {
					newlist[i] = list[i];
				}
				list = newlist;
			}
			list[NextIndex++] = value;
			return NextIndex - 1;
		}

//---------------------------------------------------------------------------------------

		public T this[int n] {
			get { return list[n]; }
			set { list[n] = value; }	// Should check for index out of range
		}
	}
}
