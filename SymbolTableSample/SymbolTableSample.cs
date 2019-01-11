using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// This program is meant as a sample program to show how it can be useful to break
// a program up into distinct data structures (i.e. clases) and hide implementation
// details from the users of these classes (i.e. the programmer, not the end user).
// It also shows various aspects of the C# lanuage and the .Net Runtime class library

namespace SymbolTableSample {
	public partial class SymbolTableSample : Form {
		public SymbolTableSample() {
			InitializeComponent();

			var MySymTab = new SymbolTable();
			var cmp = new MyCompiler(MySymTab);
			IEnumerable<string> txt = GetSymbolTableSource();

			foreach (string line in txt) {
				string[] Tokens = Lexer.Tokenize(line);
				// cmp.Compile(Tokens);
			}

		}

//---------------------------------------------------------------------------------------

		// Reads the source for our SymbolTable.cs file
		private static IEnumerable<string> GetSymbolTableSource_Sample() {
			string ExePath = Application.ExecutablePath;    // Full name of our .exe
			int ix = ExePath.LastIndexOf("/bin/");
			string SrcDir = ExePath.Substring(0, ix);
			string SrcPath = Path.Combine(SrcDir, "SymbolTable.cs");

			var txt = File.ReadLines(SrcPath);
			return txt;
		}

//---------------------------------------------------------------------------------------

		private static IEnumerable<string> GetSymbolTableSource() {
			string Source = @"
class Class1
	int	i, j;
	float x1, y2;

	proc Method1(int Parm1, double Parm2)
		i = 1;
		foo = goo;
	endproc;

	proc Method2()
		int i, j;
	endproc;

endclass Main;

class ClassTwo {
	proc Method1()
		Class1 foo = 1;
	endproc;
endclass;
";
			// Source = Source.Replace('\r', ' ').Replace('\n', ' ').Replace('\t', ' ');

			return Source.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
		}
	}
}
