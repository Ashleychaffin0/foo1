using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// using Roslyn.Compilers;
// using Roslyn.Compilers.CSharp;

namespace Roslyn2 {
	class Program {
		static void Main(string[] args) {
			var syntaxTree = SyntaxTree.ParseCompilationUnit(@"
using System;
class C
{
	static void M()
	{
		if (true)
			Console.WriteLine(""Hello, World!"");
	}
}");
			var x = Roslyn.Compilers.CSharp.SyntaxKind.IfKeyword;

			var KidNodes = syntaxTree.GetRoot().DescendantNodesAndSelf();

			ProcessNodes(KidNodes, 0);

			foreach (var kid in KidNodes) {
				// Console.WriteLine("Kid.Debugger = {0}", kid.);
			}

			var ifStatement = (IfStatementSyntax)syntaxTree.
				GetRoot().
				DescendantNodes().
				First(n => n.Kind == SyntaxKind.IfStatement);
			Console.WriteLine("Condition is '{0}'.", ifStatement.Statement);
		}

//---------------------------------------------------------------------------------------

		private static void ProcessNodes(IEnumerable<SyntaxNode> Nodes, int Level) {
			ShowSeparator(Level);
			foreach (var kid in Nodes) {
				WriteLevel(Level);
				Console.Write("Kind = {0}", kid.Kind);

				if (kid.Kind == SyntaxKind.IdentifierName) {
					WriteLevel(Level);
					Console.Write(" - {0}", (kid as IdentifierNameSyntax).PlainName);
				} else if (kid.Kind == SyntaxKind.UsingDirective) {
					WriteLevel(Level);
					Console.Write(" - {0}", (kid as UsingDirectiveSyntax).Name);
				}

				Console.WriteLine();
				if (kid.ChildNodes().Count() > 0) {
					ProcessNodes(kid.DescendantNodes(), Level + 1);
				}
				if (kid.ChildNodes().Count() > 0) {
					var y = kid.DescendantNodes();
				}
			}
		}

//---------------------------------------------------------------------------------------

		private static void ShowSeparator(int Level) {
			WriteLevel(Level);
			Console.WriteLine("***************");
		}

//---------------------------------------------------------------------------------------

		private static void WriteLevel(int Level) {
			Console.Write("".PadLeft(Level, '\t'));
			Console.Write("{0} - ", Level);
		}
	}
}
