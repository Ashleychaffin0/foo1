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

namespace MyNotQuiteSoDumbInterpreter {
	public partial class MyNotQuiteSoDumbInterpreter : Form {
		public MyNotQuiteSoDumbInterpreter() {
			InitializeComponent();

			// File.Move(@"C:\Users\lrs5\Desktop\LRS Mergible docs\Answer.frm", @"C:\Users\lrs5\Desktop\LRS Mergible docs\Answer.wpd");

			string pgm = @"Now is
the time   
for    all  good
men";
			var src = new Source(pgm);
			var lex = new Lexer(src);
			Console.WriteLine("=========================");
			foreach (Symbol sym in lex.NextToken()) {

			}
			MessageBox.Show("EOF");
		}
	}
}
