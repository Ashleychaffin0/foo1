using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

// TODO: Implement a syntax-checking phase

namespace TheRhythymMethod {
	class Interpreter {
		MusicXml	xml;
		string		filename;
		bool IsSyntaxChecking;

		char[] blank = new char[] { ' ' };

//---------------------------------------------------------------------------------------

		public Interpreter(MusicXml xml, string filename = null ) {
			this.xml = xml;
			this.filename = filename;
		}

//---------------------------------------------------------------------------------------

		public bool Run(string filename = null) {
			filename = filename ?? this.filename;
			if (filename is null) {
				throw new Exception("No program filename specified");
			}
			IsSyntaxChecking = true;
			var Code = File.ReadAllLines(filename);
			// TODO: Write standard beginning stuff - version, date/time, pgm names, etc
			bool retcode = Run(Code);
			return retcode;
		}

//---------------------------------------------------------------------------------------

		private bool Run(string[] code) {
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
			var tokens = line.Split(blank, StringSplitOptions.RemoveEmptyEntries);
			string command = tokens[0];
			// TODO: Make this table driven, rather than a brute-force switch
			switch (command.ToUpper()) {
			case "PARTS":
				ListParts(tokens);
				break;
			case "PART":
				IsSyntaxChecking = false;
				SelectPart(tokens);
				break;
			case "SELECT-MEASURES":
				SelectMeasures(tokens);
				break;
			case "FACTOR-DURATION":
				SetFactorDuration(tokens);
				break;
			default:
				SyntaxError($"Command '{command}' not recognized");
				break;
			}
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
			return line.Trim();
		}

//---------------------------------------------------------------------------------------

		private void ListParts(string[] tokens) {	
			// TODO: Check tokens length to ensure no extraneous parms
			if (IsSyntaxChecking) { return; }
			int n = 0;
			Console.WriteLine("Part Names:");
			foreach (var name in xml.GetPartNames()) {
				Console.WriteLine($"[{++n}]: {name}");
			}
		}

//---------------------------------------------------------------------------------------

		private void SelectPart(string[] tokens) {
			// Note restriction. Part names must not have extra blanks
			// TODO: Ensure at least one parm
			if (IsSyntaxChecking) { return; }
			string PartName = string.Join(" ", tokens, 1, tokens.Length - 1);
			xml.SelectPart(PartName);
			Console.WriteLine($"Part '{PartName}' selected");
		}

//---------------------------------------------------------------------------------------

		private void SelectMeasures(string[] tokens) {
			if (tokens.Length != 3) {
				SyntaxError("Select-Measures");		// TODO: Bad message
			}
			// TODO: Check data type of parms. Must be int and > 0
			if (IsSyntaxChecking) { return; }
			int ixStartMeasure = Convert.ToInt32(tokens[1]);
			int ixEndMeasure = Convert.ToInt32(tokens[2]);
			// TODO: End must be >= Start and within range of Measures.Count
			xml.SelectMeasures(ixStartMeasure-1, ixEndMeasure-1);
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
				Console.WriteLine($"Measure {i+1}:");
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
					int NewDuration = (int)Math.Round(OriginalDuration * factor);
					dur[0].InnerText = NewDuration.ToString();
					Console.WriteLine($"Converted duration from {OriginalDuration} to {NewDuration}");
				}
			}
		}
	}
}
