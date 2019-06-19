using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RhythmAndClues {
	class CmdSave : ICommand {
		public bool CheckSyntax(Interpreter main, string[] tokens) {
			bool retval = true;
			if (main.IsSyntaxChecking) {
				if (!main.bLoadCommandFound) {
					main.SyntaxError($"Error: Can't SAVE because no LOAD was done");
					retval = false;
				}
				var OutputFilename = main.JoinParms(tokens);
				if (main.InputFilename == OutputFilename) {
					main.SyntaxError($"SAVE filename the same as most recent LOAD filename");
					retval = false;
				}
			}
			return retval;
		}

//---------------------------------------------------------------------------------------

		public bool Command(Interpreter main, string[] tokens) {
			var OutputFilename = main.JoinParms(tokens);
			if (File.Exists(OutputFilename)) {
				main.Msg($"The output filename exists.");
				string YN = main.GetYesNo();
				if (YN == "N") {
					main.SyntaxError("Output file will not be overwritten");
					return false;
				}
			}
			// int MeasureLength = RecalculateTimeSignature();
			// RemeasureScore(PatternDurations.Count);
			try {
				main.xml.Save(OutputFilename);
				main.Msg($"File saved successfully to {OutputFilename}");
				return true;
			} catch (Exception ex) {
				throw new Exception($"Error: Could not save to {OutputFilename} -- '{ex.Message}'");
			}
		}
	}
}
