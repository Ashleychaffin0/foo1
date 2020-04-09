using System;
using System.Speech.Recognition;

// From https://docs.microsoft.com/en-us/dotnet/api/system.speech.recognition.speechrecognitionengine?view=netframework-4.8
// See also https://www.c-sharpcorner.com/article/speech-recognition-using-c-sharp/

namespace SpeechRecognitionApp {
	class Program {
		static bool bQuitSwitch = false;

		static void Main(string[] args) {

			// Create an in-process speech recognizer for the en-US locale.  
			using (
			SpeechRecognitionEngine recognizer =
			  new SpeechRecognitionEngine(
				new System.Globalization.CultureInfo("en-US"))) {

				// Create and load a dictation grammar.  
				recognizer.LoadGrammar(new DictationGrammar());

				// Add a handler for the speech recognized event.  
				recognizer.SpeechRecognized +=
				  new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);

				// Configure input to the speech recognizer.  
				recognizer.SetInputToDefaultAudioDevice();

				// Start asynchronous, continuous speech recognition.  
				recognizer.RecognizeAsync(RecognizeMode.Multiple);

				Console.WriteLine("Speak something ('quit' to exit...");

				// Keep the console window open.  
				while (bQuitSwitch == false) {
					Console.ReadLine();
				}
			}
		}

		// Handle the SpeechRecognized event.  
		static void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e) {
			Console.WriteLine("Recognized text: " + e.Result.Text);
			if (e.Result.Text == "quit") { bQuitSwitch = true; }
		}
	}
}