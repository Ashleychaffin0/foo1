using System;
using System.Text;
using System.Windows.Forms;

namespace TestWigglyLine {
	public partial class TestWigglyLine : Form {
		public TestWigglyLine() {
			InitializeComponent();

			Test1();

			Test0(1, "asdf"); ;
		}

//------------------------------------------------------------------------------------
		private void Test1() {
			var sb = new StringBuilder();
			sb.Append(@"{\rtf1\ansi{\colortbl;\red255\green0\blue0;\red0\green255\blue0;\red0\green0\blue255;}
 This is some {\b \i \cf1 \ulwave bold italicized} text.");
			string s = sb.ToString();
			richTextBox1.Rtf = s;
		}
	}
}
