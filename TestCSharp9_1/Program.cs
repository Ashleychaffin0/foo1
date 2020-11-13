// https://devblogs.microsoft.com/dotnet/welcome-to-c-9-0/?ocid=3006539&MC=CSHARP&MC=WebDev&MC=.NET&MC=Vstudio&MC=MSAzure
// https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md

using System;
using System.Net;

//class Program {
	//public static void Main(string[] args) {
		CPoint p = new (3, 4, ConsoleColor.Red);
		Console.WriteLine(p);
		nint n = 2 * 1024 * 1024; 
		Console.WriteLine($"{n * 1024 + 1:N0}");
		Console.WriteLine(LifeStageAtAge(1));
		Console.WriteLine(nint.MaxValue);
		Console.WriteLine(long.MaxValue);

var f = new Foo(5, 7);

//}

#if false
(new WebClient()).DownloadFile("https://1a3k5t1s1nlq3nug3z23q9ed-wpengine.netdna-ssl.com/wp-content/uploads/2019/01/ft190127-foxtrot-comic-yelpings-roger-andy-marriage-food-yelp-reviews-1-star-rating-beetloaf-food.jpg", @"G:\lrs\Yelpings.jpg");
#endif

int LifeStageAtAge(int age) => age switch 	{
		< 0 => 0,
		< 2 => 1,
		//< 4 => LifeStage.Toddler,
		//< 6 => LifeStage.EarlyChild,
		//< 12 => LifeStage.MiddleChild,
		//< 20 => LifeStage.Adolescent,
		//< 40 => LifeStage.EarlyAdult,
		//< 65 => LifeStage.MiddleAdult,
		_ => 100,
	};
//}

record Foo(int X, int Y) {
	public int MyProperty { get; set; }

//---------------------------------------------------------------------------------------

	public Foo(int x, int y) {
		X = x;
		Y = y;
	}
}

class Point {
	public int X, Y;

//---------------------------------------------------------------------------------------

	public Point(int x, int y) {
		X = x;
		Y = y;
		if (x is not 1 or 2) {
			Console.WriteLine("OK");
		}
	}

//---------------------------------------------------------------------------------------

	public override string ToString() => $"({X}, {Y})";
}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

class CPoint : Point {
	public ConsoleColor Color;

//---------------------------------------------------------------------------------------

	public CPoint(int x, int y, ConsoleColor color) : base(x, y) {
		Color = color;
	}

//---------------------------------------------------------------------------------------

	public override string ToString() => $"{base.ToString()}, {Color}";
}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
