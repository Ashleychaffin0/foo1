// Copyright (c) 2020 by Larry Smith
//

using System;

Console.WriteLine("Hello World!");
Foo x = new(4, "ljksdf", Math.PI);
Console.WriteLine(x.ToString());

record Foo(int A, string B, double C);
