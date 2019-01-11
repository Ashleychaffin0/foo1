using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Windows.Forms;

namespace TestSpeech2 {
	class Program {
		static void Main(string[] args) {
			var sp = new SpeechRecognitionEngine();
			var defaultDictationGrammar = new DictationGrammar();
			
			defaultDictationGrammar.Name = "Default Dictation";
			defaultDictationGrammar.Enabled = true;

			sp.LoadGrammar(defaultDictationGrammar);
			sp.SetInputToDefaultAudioDevice();

			var syn = new SpeechSynthesizer();

			Console.WriteLine("Start... 'quit' to end");

			while (true) {
				var res = sp.Recognize();
				if (res == null) {
					continue;
				}
				if (res.Text == "quit") {
					break;
				}
				Console.WriteLine("Text = {0}", res.Text);
				string pw = "";
				switch (res.Text) {
				case "Amazon":
					pw = "lrs_5_Amazon";
					break;
				default:
					syn.Speak("Didn't recognize " + res.Text);
					break;
				}
				SendKeys.SendWait(pw + "{Enter}");
			}
			/*
			 * 
open System
open System.Speech.Recognition
open System.Windows.Forms

let LRS = true

let GetPassword (args : SpeechRecognizedEventArgs) = 
    let pw = match args.Result.Text with
             | "Amazon" -> "lrs_5_Amazon"
             | _ -> ""
    pw + "{Enter}"

let sp = new SpeechRecognitionEngine()
let defaultDictationGrammar = new DictationGrammar()

defaultDictationGrammar.Name <- "Default Dictation"
defaultDictationGrammar.Enabled <- true

sp.LoadGrammar(defaultDictationGrammar)
sp.SetInputToDefaultAudioDevice()

sp.RecognizeAsync(RecognizeMode.Multiple)

if LRS = true then
    sp.SpeechRecognized.Add(fun args -> SendKeys.Send (* args.Result.Text *) "lrs_5_Amazon{Enter}")
else
    sp.SpeechRecognized.Add(fun args -> printfn "Recognized [%f] '%s'" args.Result.Confidence args.Result.Text)
    
//Console.

printfn ("(Just start talking. Press any key to quit.)")
Console.ReadKey(true) |> ignore

			 * */
		}
	}
}
