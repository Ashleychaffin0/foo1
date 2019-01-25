using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BogSpeechTest {
	public partial class BogSpeechTest : Form {
		public BogSpeechTest() {
			InitializeComponent();

			var sr = new SpeechRecognizer();

			Choices LettersAndCommands = new Choices();
			LettersAndCommands.Add(new string[] { "red", "green", "blue" });
			for (char i = 'A'; i <= 'Z'; i++) {
				LettersAndCommands.Add(i.ToString());
			}
			LettersAndCommands.Add(new string[] { "Done", "Back" });

			GrammarBuilder gb = new GrammarBuilder();
			gb.Append(LettersAndCommands);

			// Create the Grammar instance.
			Grammar g = new Grammar(gb);

			sr.LoadGrammar(g);
			sr.SpeechRecognized += Sr_SpeechRecognized;
		}

//---------------------------------------------------------------------------------------

		private void Sr_SpeechRecognized(object sender, SpeechRecognizedEventArgs e) {
			// MessageBox.Show(e.Result.Text);
			lbMsgs.Items.Insert(0, e.Result.Text);
		}
	}
}
