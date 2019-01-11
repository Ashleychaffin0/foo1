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

using Roslyn.Compilers;
using Roslyn.Compilers.Common;
using Roslyn.Compilers.CSharp;

namespace Roslyn1 {
    public partial class Form1 : Form {

        string Source;              // Just so we can see what we're processing

//---------------------------------------------------------------------------------------

        public Form1() {
            InitializeComponent();
        }

//---------------------------------------------------------------------------------------

        private void btnGo_Click(object sender, EventArgs e) {
            msg("Hello");
            msg("Hello {0}", "World");
            TryIt(@"D:\LRS\SkyDrive\BalanceLine\BalanceLine.cs");
        }

//---------------------------------------------------------------------------------------

        private void TryIt(string path) {
            Source = File.ReadAllText(path);
            var st = Syntax.ParseCompilationUnit(Source);
            ProcessUsings(st.Usings);
        }

//---------------------------------------------------------------------------------------

        private void ProcessUsings(SyntaxList<UsingDirectiveSyntax> syntaxList) {
            throw new NotImplementedException();
        }

//---------------------------------------------------------------------------------------

        void msg(string text) {
            lbMsgs.Items.Insert(0, text);
        }

//---------------------------------------------------------------------------------------

        void msg(string fmt, params object[] parms) {
            msg(string.Format(fmt, parms));
        }
    }
}
