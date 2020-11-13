namespace TestC9
{
	class Program
	{
		static void Main(string[] args) {
			Console.WriteLine("Hello World!");
			foo x = new foo(4, "ljksdf", Math.Pi);
			Console.WriteLine(x);
			Half y;
		}
	}

	record Foo(int A, string B, Half C);
}
