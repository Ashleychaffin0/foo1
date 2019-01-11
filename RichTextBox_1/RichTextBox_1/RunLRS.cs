using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Media;

namespace RichTextBox_1 {
	class RunLRS : Run {

		Brush OriginalBackground;

//---------------------------------------------------------------------------------------

		public RunLRS(string s) {
			this.Text = s;
			OriginalBackground = this.Background;
			this.MouseEnter += new System.Windows.Input.MouseEventHandler(RunLRS_MouseEnter);
			this.MouseLeave += new System.Windows.Input.MouseEventHandler(RunLRS_MouseLeave);
		}

//---------------------------------------------------------------------------------------

		public RunLRS() {
			// The runtime seems to need a parameterless constructor
			this.MouseEnter += new System.Windows.Input.MouseEventHandler(RunLRS_MouseEnter);
			this.MouseLeave += new System.Windows.Input.MouseEventHandler(RunLRS_MouseLeave);
		}

//---------------------------------------------------------------------------------------

		void RunLRS_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) {
			this.Background = Brushes.Yellow; ;
		}

//---------------------------------------------------------------------------------------

		void RunLRS_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) {
			this.Background = OriginalBackground;
		}
	}
}
