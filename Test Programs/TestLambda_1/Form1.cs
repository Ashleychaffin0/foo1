using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestLambda_1 {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void Form1_Load(object sender, EventArgs e) {
			CallRoutineWithRetries();
		}

//---------------------------------------------------------------------------------------

		private void CallRoutineWithRetries() {
			Console.WriteLine("About to call retry manager");
			RetryWithRandomDelays(() => WhatTimeIsIt(), 3, 1000);
		}

//---------------------------------------------------------------------------------------

		private bool WhatTimeIsIt() {
			Console.WriteLine("It is now {0}", DateTime.Now);
			return false;
		}

//---------------------------------------------------------------------------------------

		private bool RetryWithRandomDelays(Func<bool> routine, int nAttempts,
						int Delay) {
			for (int i = 0; i < nAttempts; i++) {
				Console.WriteLine("Attempt {0}", i);
				bool bSuccess = routine();
				if (bSuccess) {
					Console.WriteLine("Routine succeeded");
					return true;
				}
				System.Threading.Thread.Sleep(Delay);
			}
			Console.WriteLine("Routine failed");
			return false;
		}
	}

}
