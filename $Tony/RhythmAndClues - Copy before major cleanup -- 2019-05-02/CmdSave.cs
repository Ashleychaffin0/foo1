using System;
using System.IO;

namespace RhythmAndClues {
	class CmdSave : ICommand {
		readonly Interpreter main;

//---------------------------------------------------------------------------------------

		public CmdSave(Interpreter main) {
			this.main = main;
		}

//---------------------------------------------------------------------------------------

		public bool CheckSyntax(string[] tokens) {
			bool retval = true;
			if (main.IsSyntaxChecking) {
				if (!main.bLoadCommandFound) {
					main.SyntaxError($"Error: Can't SAVE because no LOAD was done");
					retval = false;
				}
				var OutputFilename = main.JoinParms(tokens);
				if (main.ScoreFilename == OutputFilename) {
					main.SyntaxError($"SAVE filename the same as most recent LOAD filename");
					retval = false;
				}
			}
			return retval;
		}

//---------------------------------------------------------------------------------------

		public bool Execute(string[] tokens) {
			var OutputFilename = main.JoinParms(tokens);
			if (File.Exists(OutputFilename)) {
				main.Msg($"The output filename exists.");
				string YN = GetYesNo();
				if (YN == "N") {
					main.SyntaxError("Output file will not be overwritten");
					return false;
				}
			}
			// int MeasureLength = RecalculateTimeSignature();
			// RemeasureScore(PatternDurations.Count);
			try {
				// main.XmlScore.Save(OutputFilename);
				main.XmlPattern.Save(OutputFilename);
				main.Msg($"File saved successfully to {OutputFilename}");
				return true;
			} catch (Exception ex) {
				throw new Exception($"Error: Could not save to {OutputFilename} -- '{ex.Message}'");
			}
		}

//---------------------------------------------------------------------------------------

		string GetYesNo() {
			while (true) {
				main.Msg("Enter Y to overwrite or N to quit: ", false);
				var c = Console.ReadKey();
				Console.WriteLine();
				switch (c.Key) {
				case ConsoleKey.Y:
					return "Y";
				case ConsoleKey.N:
					return "N";
				default:
					main.Msg("Invalid response");
					break;
				}
			}
		}
	}
}
