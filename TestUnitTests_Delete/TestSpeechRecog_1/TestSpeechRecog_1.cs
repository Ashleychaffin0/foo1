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

namespace TestSpeechRecog_1 {
	public partial class TestSpeechRecog_1 : Form {
		public TestSpeechRecog_1() {
			InitializeComponent();
		}

		private void btnTest_Click(object sender, EventArgs e) {
			SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();
			Grammar dictationGrammar = new DictationGrammar();
			recognizer.LoadGrammar(dictationGrammar);
			try {
				btnTest.Text = "Speak Now";
				recognizer.SetInputToDefaultAudioDevice();
				RecognitionResult result = recognizer.Recognize();
				txtResult.Text = result.Text;
			} catch (InvalidOperationException exception) {
				txtResult.Text = String.Format("Could not recognize input from default aduio device. Is a microphone or sound card available?\r\n{0} - {1}.", exception.Source, exception.Message);
			} finally {
				recognizer.UnloadAllGrammars();
			}
		}
	}
}
