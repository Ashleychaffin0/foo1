using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

// TODO: Commands are listed twice, once for syntax checking, then for execution
// TODO: Syntax error: line 5, command {cmd all} -- error msg

namespace RhythmAndClues {
	class Interpreter {
		MusicXml xml;
		string	 filename;	// TODO: Drop filename
		bool	 IsSyntaxChecking;

		char[]	 blank = new char[] { ' ' };

		Dictionary<string, (Action<string[]>, string)> Commands;

//---------------------------------------------------------------------------------------

		public Interpreter(MusicXml xml, string filename = null) {	// TODO: Drop filename
			this.xml = xml;
			this.filename = filename;
			SetupCommands();
		}

//---------------------------------------------------------------------------------------

		private void SetupCommands() {
			Commands = new Dictionary<string, (Action<string[]>, string)> {
				["LOAD"]			= (LoadFile, "S"),
				["PATTERN"]			= (ApplyPattern, ""),
				["SAVE"]			= (SaveFile, "S"),

				// TODO: explicit command REMEASURE?
				// TODO: Not needed any more?
				["SUMMARY"]         = (Summary, ""),
				["PART"]            = (SelectPart, "N"),
				["SELECT-MEASURES"] = (SelectMeasures, "NN"),
				["FACTOR-DURATION"] = (SetFactorDuration, "F")
			};
			
		}

//---------------------------------------------------------------------------------------

		public bool Run(string filename = null) {
			filename = filename ?? this.filename;
			if (filename is null) {
				throw new Exception("No program filename specified");
			}
			var Code = File.ReadAllLines(filename);
			// TODO: Write standard beginning stuff - version, date/time, pgm names, etc
			bool retcode = Run(Code);
			return retcode;
		}

//---------------------------------------------------------------------------------------

		private bool Run(string[] code) {
			// Pass 1 -- mostly syntax checking
			IsSyntaxChecking = true;
			bool bOK = RunPass(code);
			if (!bOK) {
				return false;
			}
			// Pass 2 -- go for it!
			IsSyntaxChecking = false;
			return RunPass(code);
		}

//---------------------------------------------------------------------------------------

		private bool RunPass(string[] code) {
			try {
				foreach (var line in code) {
					ExecuteLine(line);
				}
				return true;
			} catch (Exception ex) {
				Console.WriteLine($"Runtime error - {ex.Message}");
				return false;
			}
		}

//---------------------------------------------------------------------------------------

		private void ExecuteLine(string line) {
			Console.WriteLine(">> " + line);
			line = Decomment(line);
			if (line.Length == 0) { return; }

			// Simple parsing
			var tokens = line.Split(blank, StringSplitOptions.RemoveEmptyEntries);

			(Action<string[]> cmd, string ParmInfo) command;
			bool bFound = Commands.TryGetValue(tokens[0].ToUpper(), out command);
			if (bFound) {
				// TODO: Only syntax check on first pass
				if (!ParmsOK(tokens, command.ParmInfo)) {
					// TODO:
					Console.WriteLine("Parms not OK");  // TODO:
					return;		// TODO: Shouldn't this return true/false, especially during syntax checking
				}
				command.cmd(tokens);
			} else {
				SyntaxError($"Command '{command}' not recognized");

			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Validates the parameters passed to a command. 
		/// </summary>
		/// <param name="parmInfo">
		/// This string consists of n chars, one for each parameter that a given
		/// command supports. They are; 'N' for a natural number, greater than zero;
		/// 'Z' for a natural number or zero; 'I' for an integer (positive or negative);
		/// 'F' for floating point greater than zero; 'S' for a string, which must be
		/// the last parameter as it will joing all remaining parameters together,
		/// separated by a single blank. Note that we don't currently support all these
		/// since the current commands don't need them all. This routine will be updated
		/// (and hopefully so will these comments!) as the command language grows. 
		/// </param>
		/// <returns></returns>
		private bool ParmsOK(string[] tokens, string parmInfo) {
			// TODO: Check for superfluous parameters: parmInfo.Length > tokens.Length + 1
			// TODO: $ is special case, re checking # of parms
			// TODO: Make sure we have enough parameters
			bool bOK;
			bool bAllParmsOK = true;
			for (int i = 0; i < parmInfo.Length; i++) {
				string parm = tokens[i + 1];    // tokens[0] is the command itself
				switch (parmInfo[i]) {
				case 'N':
					bOK = int.TryParse(parm, out _);
					if (!bOK) {
						Console.WriteLine($"Parameter {i+1}: Invalid numeric; must be an integer > 0");
						bAllParmsOK = false;
					}
					break;
				case 'F':
					bOK = float.TryParse(parm, out float f);
					if (!bOK) {
						Console.WriteLine($"Parameter {i+1}: Invalid numeric; must be > 0");
						bAllParmsOK = false;
					}
					break;
				default:	// Shouldn't happen
					// TODO: But it's invalid if it does
					break;
				}
			}
			return bAllParmsOK;
		}

//---------------------------------------------------------------------------------------

		private void SyntaxError(string msg) {
			// TODO: I don't like this. We shouldn't bail at the first typo.
			//		 At least complain about all the typos. Set an error flag and don't
			//		 execute any more.
			// TODO: Maybe even do an initial pass, makeing sure we have the right
			//		 paramters as well.
			// TODO: Message needs to better express that it's a syntax error
			if (IsSyntaxChecking) {
				Console.WriteLine(msg);
			} else {
				throw new Exception(msg);
			}
		}

//---------------------------------------------------------------------------------------

		private static string Decomment(string line) {
			int ix = line.IndexOf('#');
			if (ix >= 0) {
				line = line.Substring(0, ix);
			}
			return line.Replace("\t", " ").Trim();
		}

//---------------------------------------------------------------------------------------

		private void LoadFile(string[] tokens) {
			xml.load
		}

//---------------------------------------------------------------------------------------

		private void ApplyPattern(string[] tokens) {
			// TODO:
		}

//---------------------------------------------------------------------------------------

		private void SaveFile(string[] tokens) {
			// TODO:
		}

//---------------------------------------------------------------------------------------

			private void Summary(string[] tokens) {
			// TODO: Check tokens length to ensure no extraneous parms
			if (IsSyntaxChecking) { return; }
			int n = 0;
			Console.WriteLine("Part Names:");
			foreach (var name in xml.GetPartNames()) {
				Console.WriteLine($"[{++n}]: {name}");
				// TODO: Display measure count for each part
			}
		}

//---------------------------------------------------------------------------------------

		private void SelectPart(string[] tokens) {
			// TODO: Ensure at least one parm
			if (IsSyntaxChecking) { return; }
			// string PartName = string.Join(" ", tokens, 1, tokens.Length - 1);
			int PartNum = Convert.ToInt32(tokens[1]);
			xml.SelectPart(PartNum);
			Console.WriteLine($"Part '{PartNum}' selected");
		}

//---------------------------------------------------------------------------------------

		private void SelectMeasures(string[] tokens) {
			if (tokens.Length != 3) {
				SyntaxError("Select-Measures");     // TODO: Bad message
			}
			// TODO: Check data type of parms. Must be int and > 0 and b >= a
			if (IsSyntaxChecking) { return; }
			int ixStartMeasure = Convert.ToInt32(tokens[1]);
			int ixEndMeasure = Convert.ToInt32(tokens[2]);
			// TODO: End must be >= Start and within range of Measures.Count
			xml.SelectMeasures(ixStartMeasure - 1, ixEndMeasure - 1);
			// Next line. ".." seems to be some kind of ligature. Hence the kludge
			Console.WriteLine($"Measures selected: [{ixStartMeasure}{'.'}.{ixEndMeasure}]");
		}

//---------------------------------------------------------------------------------------

		private void SetFactorDuration(string[] tokens) {
			// Check data type of its single parm. Must be float and > 0
			if (tokens.Length != 2) {
				SyntaxError("Factor-Duration must have only one parameter");
			}
			bool bOK = double.TryParse(tokens[1], out double factor);
			if ((!bOK) || (factor <= 0)) {
				SyntaxError($"The duration factor ({tokens[1]}) must be numeric and " +
					$"greater than 0. But can have a decimal point.");
			}
			if (IsSyntaxChecking) { return; }
			for (int i = xml.StartMeasure; i <= xml.EndMeasure; i++) {
				Console.WriteLine($"Measure {i + 1}:");
				var mes = xml.Measures[i] as XmlElement;
				var notes = mes.GetElementsByTagName("note");
				int nNote = 0;
				foreach (XmlElement note in notes) {
					Console.Write($"\tNote[{++nNote}]: ");
					// TODO: I've seen some notes with <rest> or <grace> tags. What else?
					//		 The grace notes have no duration. The <rest> I can just skip
					var dur = note.GetElementsByTagName("duration");
					if (dur.Count == 0) {
						Console.WriteLine("No duration found. Grace Note?");
						continue;
					}
					// Presumably there's only one duration tag per note
					int OriginalDuration = Convert.ToInt32(dur[0].InnerText);
					int NewDuration      = (int)Math.Round(OriginalDuration * factor);
					dur[0].InnerText     = NewDuration.ToString();
					Console.WriteLine($"Converted duration from {OriginalDuration} to {NewDuration}");
				}
			}
		}
	}
}
