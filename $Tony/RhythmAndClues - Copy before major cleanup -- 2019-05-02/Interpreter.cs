using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

// NOTE: This module assumes 
//		*	The input files are in valid MusicXML format (not sure if V1/V2/V3 are OK)
//		*	There are no REST notes in the score

// TODO:
//	*	Pattern (rename to Rhythm) must be "multiple" of time signature
//	*	Look into accidentals. Don't think this is an issue, but look into different keys.

namespace RhythmAndClues {
	class Interpreter {
		readonly string			ProgramName;				// From command line
		int						CurrentLineNumber;			// For runtime error messages
		readonly List<string>	OutputLines;				// Saves Msg()'s
		Dictionary<string, ICommand> Commands;
		ICommand				CurrentCommand;

		// The following fields marked internal are implicit parameters to one or
		// more commands. In this quick(?) and dirt(!) program, I'll do it this way.
		// In a less personal program I'd approach things differently.

		// Further note: The problem is that the different Commands (LOAD, SAVE, RHYTHM,
		// etc) need to synchronize with each other. For example, LOAD needs a place to
		// save the input XML elements, APPLY-RHYTHM needs to modify them, and SAVE has
		// to write them out. Meanwhile, having to support multiple instances of
		// RHYTHM, interspersed with TUPLETs required further intercommunication. And
		// so on. I suppose I coult have tried to group things into several data-mostly
		// classes and passed the, but the project never evolved that way.
		internal MusicXml	 XmlScore;
		internal MusicXml	 XmlPattern;
		internal XmlNodeList ScoreNotes;
		internal XmlNodeList PatternNotes;

		internal bool		 IsSyntaxChecking;      // In pass 1 (true) or pass 2 (false)

		internal string		 ScoreFilename;   // "Remember where we parked" -- J. T. Kirk
		internal string		 PatternFilename;

		internal Dictionary<string, DurationDef> DurationIDs; // e.g. "4." for dotted qtr

		internal List<DurationDef> RhythmDurations;		// Parms from APPLY-PATTERN

		internal bool bLoadCommandFound;				// Some cmds need LOAD done first

		internal int NumNotes;

		internal int Divisions;

		internal XmlElement			OriginalFirstMeasure;
		internal List<XmlElement>	PermNotes;

		// The following field saves the original measures in the input .xml file. When
		// I first started to write the program, I thought that we'd have the same
		// number of measures in the output file as in the input file. Silly me...
		// TODO: So while we need the OriginalFirstMeasure, it's not clear if we need
		//		 the other measures. I'm assuming (spelled h-o-p-i-n-g) that MusScore
		//		 will fill in be able to reproduce the score from what little we wind
		//		 up gi
		internal List<XmlElement>	OriginalMeasures;

//---------------------------------------------------------------------------------------

		public Interpreter(string progName) {	// Constructor (ctor)
			ProgramName       = progName;
			bLoadCommandFound = false;			// So we can check if LOAD has been called
			CurrentLineNumber = 0;
			OutputLines       = new List<string>();
			RhythmDurations   = new List<DurationDef>();
			SetBasicDurations();
			SetupCommands();
		}

//---------------------------------------------------------------------------------------

		private void SetupCommands() {
			Commands = new Dictionary<string, ICommand> {
				["HELP"]           = new CmdHelp(this),
				["LOAD"]           = new CmdLoad(this),
				["LOAD-PATTERN"]   = new CmdLoadPattern(this),
				["APPLY-PATTERN"]  = new CmdApplyRhythm(this),
				["TIME-SIGNATURE"] = new CmdTimeSignature(this),
				["RHYTHM"]         = new CmdRhythm(this),
				["APPLY-RHYTHM"]   = new xxxCmdApplyRhythm(this),
				["SAVE"]           = new CmdSave(this),
#if DEBUG
				["TEST"]		   = new CmdTest(this),
#endif
				// The following are semi-undocumented diagnostic commands
				["DUMP-RHYTHM"]      = new CmdDumpRhythm(this),
				["DUMP-MEASURES"]    = new CmdDumpMeasures(this),
				["DELETE-MEASURES"]  = new CmdDeleteMeasures(this),
				["RESTORE-MEASURES"] = new CmdRestoreMeasures(this)
		};
			// Define aliases. Make sure they point to the same class instance as
			// the long forms.
			Commands.Add("AR", Commands["APPLY-RHYTHM"]);
			Commands.Add("TIMESIG", Commands["TIME-SIGNATURE"]);
		}

//---------------------------------------------------------------------------------------

		// The interpreter
		public bool Run() {
			var Code = File.ReadAllLines(ProgramName);
			Msg("Rhythm And Clues -- version 2019-04-30, 10:20 PM");
			Msg();
			bool bCodeRanOK = Run(Code);

			// OK, the code's been run (possibly with errors), and we're done.
			// There's no clipboard support in .Net Core, so we'll fake it here
			// TODO: See Xamarin.Essentials -- Clipboard (https://docs.microsoft.com/en-us/xamarin/essentials/clipboard?context=xamarin/xamarin-forms)
			var sb = new StringBuilder();
			foreach (var line in OutputLines) {
				sb.AppendLine(line);	// Glue lines together to make one big string
			}
			string fn = Path.GetTempFileName();
			using (var sw = new StreamWriter(fn)) {
				sw.Write(sb.ToString());
			}
			Msg($"Temp filename with the program messages is {fn}");
			return bCodeRanOK;
		}

//---------------------------------------------------------------------------------------

		// OK, the *real* interpreter, both passes
		private bool Run(string[] code) {
			// Pass 1 -- Syntax checking. The goal here is to find as many errors at
			//			 "compile" time as we can so the user isn't sadled with running
			//			 the program, fixing the first (and only) error message, then
			//			 running it again just to find the next one. And the next one.
			//			 And the next one. And so on.
			Msg("First pass -- checking syntax.");
			IsSyntaxChecking = true;
			bool bOK = RunPass(code);
			if (!bOK) {
				Msg();
				Msg("Syntax error(s) found. Try again...");
				return false;
			}

			// Pass 2 -- go for it!
			Msg();
			Msg("Syntax looks good. Running the program...");
			IsSyntaxChecking = false;
			return RunPass(code);
		}

//---------------------------------------------------------------------------------------

		// Invokes each line of user code, during pass 1 and pass 2
		private bool RunPass(string[] code) {
			CurrentLineNumber = 0;
			bool bPassOK = true;
			try {
				foreach (var line in code) {
#if DEBUG
					if (line.ToUpper().Contains("QUIT")) {
						Msg("Quitting...");
						return false;
					}
#endif
					++CurrentLineNumber;
					bPassOK &= ExecuteLine(line);	// Set to false on any error
				}
				return bPassOK;
			} catch (Exception ex) {
				Msg($"Runtime error at line {CurrentLineNumber} - {ex.Message}.");
#if DEBUG
				Msg("Traceback: " + ex.StackTrace);
#endif
				return false;
			}
		}

//---------------------------------------------------------------------------------------

		private bool ExecuteLine(string line) {
			if (!IsSyntaxChecking) {
				Msg();			// Blank line
			}
			Msg($"[{CurrentLineNumber}] >> " + line);
			line = Decomment(line);
			if (line.Length == 0) { return true; }

			// Simple parsing
			char[] blank = new char[] { ' ' };
			var tokens   = line.Split(blank, StringSplitOptions.RemoveEmptyEntries);

			// tokens[0] is the command (e.g. Load). Find the class instance that can
			// syntax check/execute this command
			tokens[0] = tokens[0].ToUpper();
			bool bFound = Commands.TryGetValue(tokens[0], out CurrentCommand);
			if (bFound) {
				if (IsSyntaxChecking) {
					// All commands except for HELP and LOAD require that a LOAD command
					// has been executed. Add that check here.
					switch (tokens[0]) {
					case "HELP":
					case "LOAD":
						break;
					default:
						if (!bLoadCommandFound) {
							SyntaxError($"Error: Can't {tokens[0]} because no LOAD was done");
						}
						break;
					}
					return CurrentCommand.CheckSyntax(tokens);
				} else {
					return CurrentCommand.Execute(tokens);
				}
			} else {
				SyntaxError($"Command '{tokens[0]}' not recognized");
				return false;
			}
		}

//---------------------------------------------------------------------------------------

		// Throw away the stuff we don't care about
		private static string Decomment(string line) {
			int ix = line.IndexOf('%');
			if (ix >= 0) {
				line = line.Substring(0, ix);
			}
			return line.Replace("\t", " ").Trim();
		}

//---------------------------------------------------------------------------------------

		// The name pretty well says it all
		internal void SyntaxError(string msg) {
			Msg($"Syntax error at line {CurrentLineNumber} -- {msg}");
		}


//---------------------------------------------------------------------------------------

		static int Gcd(int a, int b) {	// TODO: Use this
			while (b != 0) {
				int t = b;
				b = a % b;
				a = t;
			}
			return a;
		}

//---------------------------------------------------------------------------------------

		internal void SetBasicDurations() {
			/* Note: From https://www.musicxml.com/tutorial/hello-world/
			Each note in MusicXML has a duration element. The divisions element provided
			the unit of measure for the duration element in terms of divisions per
			quarter note. Since all we have in this file is one whole note, we never
			have to divide a quarter note, so we set the divisions value to 1.
			Musical durations are typically expressed as fractions, such as “quarter”
			and “eighth” notes. MusicXML durations are fractions, too. Since the 
			denominator rarely needs to change, it is represented separately in the 
			divisions element, so that only the numerator needs to be associated with 
			each individual note. This is similar to the scheme used in MIDI to
			represent note durations.

			In other words, imagine that the <divisions> tag says 120. Then a duration
			of 120 would mean that this is a quarter note. A duration of 60 would be
			an eigth note. A 90 would be a dotted 8th note.
			*/

			DurationIDs = new Dictionary<string, DurationDef>();
			string[] Names = new [] {
				"whole", "half", "quarter", "eighth", "16th", "32nd", "64th"
			};

			for (int i = 0; i < Names.Length; i++) {
				string s = $"{1 << i}";
				DurationIDs.Add(s, new DurationDef(s, Names[i]));
				s += ".";
				DurationIDs.Add(s, new DurationDef(s, Names[i]));
			}
		}

//---------------------------------------------------------------------------------------

		internal string JoinParms(string[] tokens, int StartingIndex = 1) {
			var span = tokens.AsSpan().Slice(StartingIndex);
			return string.Join(' ', span.ToArray());
		}

//---------------------------------------------------------------------------------------

		private XmlElement xxxInsertNewMeasure(XmlElement BaseElement, int nMeasure) {
			XmlElement NewMeasure;
			if (nMeasure == 1) {
				// BaseElement.InsertAfter(OriginalFirstMeasure, null);
				NewMeasure = BaseElement.AppendChild(OriginalFirstMeasure) as XmlElement;
				DeleteElementNodes(NewMeasure, "note");
			} else {
				string comment = " ======================================================== ";
				var xcomment = XmlScore.xdoc.CreateComment(comment);
				BaseElement.AppendChild(xcomment);
				NewMeasure = xxxShinyNewMeasure(nMeasure);
				BaseElement.AppendChild(NewMeasure);
			}

			return NewMeasure;
		}

//---------------------------------------------------------------------------------------

		private XmlElement xxxShinyNewMeasure(int measureNumber) {
			var meas = XmlScore.xdoc.CreateElement("measure");
			meas.SetAttribute("number", measureNumber.ToString());
			return meas as XmlElement;
		}

//---------------------------------------------------------------------------------------

		internal XmlElement GetFirstElement(XmlElement node, string elementName) {
			var xnode = node.SelectSingleNode(elementName);
			if (xnode is null) return null;
			return xnode as XmlElement;
		}


//---------------------------------------------------------------------------------------

		internal void DeleteElementNodes(XmlNode elem, string nodeName) {
			var nodes = elem.SelectNodes(nodeName);
			foreach (XmlNode node in nodes) {
				elem.RemoveChild(node);
			}
		}

//---------------------------------------------------------------------------------------

		// Return next pattern parameter in turn. Wrap around if necessary
		// private IEnumerable<(string duration, string type)> NextPatternDuration() {
		private IEnumerable<DurationDef> NextPatternDuration() {
			int i = 0;
			while (true) {
				yield return RhythmDurations[i++];
				if (i >= RhythmDurations.Count) { i = 0; }
			}
		}

//---------------------------------------------------------------------------------------

		internal bool IsLoadCommandIssued(string cmd) {
			if (!bLoadCommandFound) {
				SyntaxError($"Error: Can't {cmd} because no valid LOAD was found");
				return false;
			}
			return true;
		}

//---------------------------------------------------------------------------------------

		// Generic message routine
		internal void Msg(string s = "", bool addNewline = true) {
			OutputLines.Add(s);
			if (addNewline) {
				Console.WriteLine(s);
			} else {
				Console.Write(s);
			}
		}

//---------------------------------------------------------------------------------------

		// A measure is simply a set of notes. The number of notes (regardless of
		// duration) is simply the number of parameters to APPLY-PATTERN. If there
		// are, say, 6 parameters (6 is the measureLength parameter), this routine,
		// each time it's called, will return the next 6 <note>s.
		private IEnumerable<List<XmlElement>> xxxNextMeasureNotes(int measureLength) {
			var MeasureNotes = new List<XmlElement>();
			int n = 0;
			// This next line was once 'foreach (XmlElement note in xml.GetNotes())'
			// (i.e. all notes in the input file). But this led to runtime errors as we
			// deleted notes in the process of recreating the new measures. So we made a
			// non-dynamic copy of the original notes in PermNotes) that lets us run.
			foreach (XmlElement note in PermNotes) {
				MeasureNotes.Add(note);
				if (++n == measureLength) {
					yield return MeasureNotes;
					MeasureNotes.Clear();
					n = 0;
				}
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Basically, delete all the cuurent measures (keeping a copy of the first
		/// measure that has extra info in it, such as the time signature). Then
		/// create as many measures (except the first with the extra info)
		/// </summary>
		/// <param name="measureLength"></param>
		private void xxxRemeasureScore(int measureLength) {
			int nMeasure = 0;
			int MeasureLength = xxxRecalculateTimeSignature();
			// TODO: Figure out where this fits in: DeleteAllMeasures();

			XmlElement BaseForMeasures = XmlScore.FirstPart;
			foreach (var notes in xxxNextMeasureNotes(measureLength)) {
				Msg($"***Inserting new measure [{++nMeasure}] -- {notes.Count} notes");
				XmlElement NewMeasure = xxxInsertNewMeasure(BaseForMeasures, nMeasure);

				int nnote = 0;
				foreach (var note in notes) {
					// TODO: Support <dot/>
					// TODO: Support <time-modification>
					var dur        = GetFirstElement(note, "duration");
					dur.InnerText  = RhythmDurations[nnote].Duration.ToString();
					var type       = GetFirstElement(note, "type");
					type.InnerText = RhythmDurations[nnote].Name;
					var pitch = GetFirstElement(note, "pitch");
					string pit = pitch.InnerText;

					// No idea what to put in <stem> or <beam>. Hopefully MuseScore will
					// do that.

					NewMeasure.AppendChild(note);
					Msg($"\t[{++nnote}]: {dur.InnerText,2} {pit}");
				}
				BaseForMeasures.AppendChild(NewMeasure);
			}
		}

//---------------------------------------------------------------------------------------

		// After applying a pattern, we may well have a different time signature
		// https://en.wikipedia.org/wiki/Time_signature
		private int xxxRecalculateTimeSignature() {
			int SumDurations = RhythmDurations.Sum(p => Convert.ToInt32(p.Duration));
			// TODO: Rates is wrong. Use ValidDurations
			var Rates = new List<(int div, int rate)> { (24, 4), (12, 8), (6, 16) };
			foreach (var item in Rates) {
				int quot = Math.DivRem(SumDurations, item.div, out int rem);
				if (rem == 0) {
					xxxSetTimeSignature(quot, item.rate);
					return quot;
				}
			}
			throw new Exception("Can't determine the time signature");
		}

//---------------------------------------------------------------------------------------

		private void xxxSetTimeSignature(int numerator, int denominator) {	// TODO: Delete when you can
			// Note: I *could* do error checking here (e.g. if (beats is null)), but I'll
			//		 assume that the XML is always right in this case (unlike, say,
			//		 <note> which might not have a duration (e.g. a rest))
			var beats           = OriginalFirstMeasure.SelectSingleNode("attributes/time/beats");
			beats.InnerText     = numerator.ToString();
			var beat_type       = OriginalFirstMeasure.SelectSingleNode("attributes/time/beat-type");
			beat_type.InnerText = denominator.ToString();
			Msg($"Time signature set to {numerator} / {denominator}");
		}

	}
}
