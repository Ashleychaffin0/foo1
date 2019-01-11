using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Linq.Expressions;

namespace TestIQueryable {
	class Program {
		static void Main(string[] args) {

			IQueryable<string> qry1 = from proc in Process.GetProcesses().AsQueryable<Process>()
							 where proc.ProcessName.StartsWith("S")
							 orderby proc.ProcessName
							 select proc.ProcessName;

			List<int> ints = new List<int> { 1, 2, 3, 4, 5 };
			var qry = from MyNumber in ints.AsQueryable<int>()
					  select new {N = MyNumber * 2 / 3 + 5};

			var qry3 = from n in Enumerable.Range(1, 10)
								   select n * 2 / 3 + 5;
			var qry4 = from q in qry3.AsQueryable<int>()
					   select q;
									
			DumpIQueryable(qry1);
		}

//---------------------------------------------------------------------------------------

		private static void DumpIQueryable(IQueryable qry) {
			Console.WriteLine("\n----------------------------\n");
			Console.WriteLine("DumpIQueryable = {0}\n", qry);

			//Console.WriteLine("Element type = {0}", qry.ElementType);
			//Console.WriteLine("Provider     = {0}", qry.Provider);
			//Console.WriteLine("ToString     = {0}", qry.ToString());
			//Console.WriteLine("Expression   = {0}", qry.Expression);
			//Console.WriteLine("\tExpression Node Type = {0}", qry.Expression.NodeType);
			//Console.WriteLine("\tExpression Type      = {0}", qry.Expression.Type);
			//Console.WriteLine("\tExpression ToString  = {0}", qry.Expression.ToString());

			FormatExpression(qry.Expression, 0);
		}

//---------------------------------------------------------------------------------------

		private static void FormatExpression(Expression exp, int level) {
			string ind = Indent(level++);
			Console.WriteLine("\n{0}NodeType = {0}", exp.NodeType);
			switch (exp.NodeType) {
			case System.Linq.Expressions.ExpressionType.Add:
				break;
			case System.Linq.Expressions.ExpressionType.AddChecked:
				break;
			case System.Linq.Expressions.ExpressionType.And:
				break;
			case System.Linq.Expressions.ExpressionType.AndAlso:
				break;
			case System.Linq.Expressions.ExpressionType.ArrayIndex:
				break;
			case System.Linq.Expressions.ExpressionType.ArrayLength:
				break;
			case System.Linq.Expressions.ExpressionType.Call:
				var mc = exp as MethodCallExpression;
				for (int i = 0; i < mc.Arguments.Count; ++i ) {
					var arg = mc.Arguments[i];
					WriteLine(level, "Arg[{0}] = {1}, type = {2}", i, arg, arg.GetType());
					FormatExpression(arg, level);
				}
				break;
			case System.Linq.Expressions.ExpressionType.Coalesce:
				break;
			case System.Linq.Expressions.ExpressionType.Conditional:
				break;
			case System.Linq.Expressions.ExpressionType.Constant:
				WriteLine(level, "Constant = {0}", (exp as ConstantExpression).Value);
				break;
			case System.Linq.Expressions.ExpressionType.Convert:
				break;
			case System.Linq.Expressions.ExpressionType.ConvertChecked:
				break;
			case System.Linq.Expressions.ExpressionType.Divide:
				break;
			case System.Linq.Expressions.ExpressionType.Equal:
				break;
			case System.Linq.Expressions.ExpressionType.ExclusiveOr:
				break;
			case System.Linq.Expressions.ExpressionType.GreaterThan:
				break;
			case System.Linq.Expressions.ExpressionType.GreaterThanOrEqual:
				break;
			case System.Linq.Expressions.ExpressionType.Invoke:
				break;
			case System.Linq.Expressions.ExpressionType.Lambda:
				break;
			case System.Linq.Expressions.ExpressionType.LeftShift:
				break;
			case System.Linq.Expressions.ExpressionType.LessThan:
				break;
			case System.Linq.Expressions.ExpressionType.LessThanOrEqual:
				break;
			case System.Linq.Expressions.ExpressionType.ListInit:
				break;
			case System.Linq.Expressions.ExpressionType.MemberAccess:
				break;
			case System.Linq.Expressions.ExpressionType.MemberInit:
				break;
			case System.Linq.Expressions.ExpressionType.Modulo:
				break;
			case System.Linq.Expressions.ExpressionType.Multiply:
				break;
			case System.Linq.Expressions.ExpressionType.MultiplyChecked:
				break;
			case System.Linq.Expressions.ExpressionType.Negate:
				break;
			case System.Linq.Expressions.ExpressionType.NegateChecked:
				break;
			case System.Linq.Expressions.ExpressionType.New:
				break;
			case System.Linq.Expressions.ExpressionType.NewArrayBounds:
				break;
			case System.Linq.Expressions.ExpressionType.NewArrayInit:
				break;
			case System.Linq.Expressions.ExpressionType.Not:
				break;
			case System.Linq.Expressions.ExpressionType.NotEqual:
				break;
			case System.Linq.Expressions.ExpressionType.Or:
				break;
			case System.Linq.Expressions.ExpressionType.OrElse:
				break;
			case System.Linq.Expressions.ExpressionType.Parameter:
				break;
			case System.Linq.Expressions.ExpressionType.Power:
				break;
			case System.Linq.Expressions.ExpressionType.Quote:
				break;
			case System.Linq.Expressions.ExpressionType.RightShift:
				break;
			case System.Linq.Expressions.ExpressionType.Subtract:
				break;
			case System.Linq.Expressions.ExpressionType.SubtractChecked:
				break;
			case System.Linq.Expressions.ExpressionType.TypeAs:
				break;
			case System.Linq.Expressions.ExpressionType.TypeIs:
				break;
			case System.Linq.Expressions.ExpressionType.UnaryPlus:
				break;
			default:
				break;
			}
		}

//---------------------------------------------------------------------------------------

		private static void Write(int level, string fmt, params object[] parms) {
			Console.Write(Indent(level));
			Console.Write(fmt, parms);
		}

//---------------------------------------------------------------------------------------

		private static void WriteLine(int level, string fmt, params object[] parms) {
			Console.Write(Indent(level));
			Console.Write(fmt, parms);
		}

//---------------------------------------------------------------------------------------

		private static string Indent(int n) {
			return "".PadRight(n, '\t');
		}
	}
}
