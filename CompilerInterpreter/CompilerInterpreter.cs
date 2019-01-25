using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MyDumbInterpreter {
	public partial class MyDumbInterpreter : Form {

		Interpreter Interp;

		List<string> Pgm;						// The source code
		string[] SourceLineDelimeters = new string[] { "\r\n" };

		int ContinuationIP			  = 0;		// Support "Continue" button

//---------------------------------------------------------------------------------------

		public MyDumbInterpreter() {
			InitializeComponent();

			SetDefaultProgram();

			Interp = new Interpreter(this);
		}

//---------------------------------------------------------------------------------------

		private void SetDefaultProgram() {
			// TODO: Come up with better sample program
			TxtProgram.Text =
@"M=5       Max
M=M*2
T=0         Total
N=1
:L
T=T+N
""The sum of the numbers from 1 to `N is `T
.
N=N+1
>LN<=M
""
""Now do Fibonnaci numbers
A=1
""Fib[1] = `A
B=1
""Fib[2] = `B
N=2
:F
C=A+B
""Fib[`N] = `C
A=B
B=C
N=N+1
>FN<=M
""
";
			TxtProgram.Select(0, 0);
		}

//---------------------------------------------------------------------------------------

		private void BtnRun_Click(object sender, EventArgs e) {
			Pgm = GetSourceLines();

			LbOutput.Items.Clear();
			ContinuationIP = Interp.Run(Pgm, -1);
			SetButtons();
		}

//---------------------------------------------------------------------------------------

		private void BtnContinue_Click(object sender, EventArgs e) {
			Pgm = GetSourceLines();
			ContinuationIP = Interp.Run(Pgm, ContinuationIP);
			SetButtons();
		}

//---------------------------------------------------------------------------------------

		private List<string> GetSourceLines() {
			var lines = TxtProgram.Text.Split(SourceLineDelimeters, StringSplitOptions.RemoveEmptyEntries);
			return new List<string>(lines);
		}

//---------------------------------------------------------------------------------------

		private void SetButtons() {
			if (ContinuationIP >= 0) {
				BtnRun.Text = "Restart";
				BtnContinue.Visible = true;
			} else {
				BtnRun.Text = "Run";
				BtnContinue.Visible = false;
			}
		}
	}
}
