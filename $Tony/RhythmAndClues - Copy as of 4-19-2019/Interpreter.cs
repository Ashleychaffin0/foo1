using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

// NOTE: This module assumes 
//		*	The input files are in valid MusicXML format (note sure if V1/V2/V3 are OK)
//		*	There are no REST notes in the score

// TODO:
//	*	Need Time-Signature command
//	*	Pattern (rename to Rhythm) must be "multiple" of time signature
//	*	

namespace RhythmAndClues {
	class Interpreter {
		string	 ProgramName;			// From command line
		MusicXml xml;
		bool	 IsSyntaxChecking;      // In pass 1 (true) or pass 2 (false)

		string   InputFilename;         // "Remember where we parked" -- J. T. Kirk

		// cmd name & ref to routine and to syntax checking routine for that cmd
		Dictionary<string, (Func<string[], bool>, Func<string[], bool>)> Commands;

		// List<(string duration, string name)> ValidDurations; // Valid parms Pattern
		Dictionary<string, DurationDef> ValidDurations; // Valid parms Pattern

#if false
		List<(string duration, string type)> PatternDurations;	// Parms from APPLY-PATTERN
#else
		List<DurationDef> PatternDurations;	// Parms from APPLY-PATTERN
#endif

		int CurrentLineNumber;				// For runtime error messages
		bool bLoadCommandFound;				// Some cmds need a LOAD done first

		List<string> OutputLines;           // Saves Msg()'s

		int NumNotes;

		List<XmlElement>	SaveMeasures;
		XmlElement			OriginalFirstMeasure;
		List<XmlElement>	PermNotes;

//---------------------------------------------------------------------------------------

		public Interpreter(string progName) {	// Constructor (ctor)
			ProgramName             = progName;
			bLoadCommandFound       = false;   // So we can check if LOAD has been called
			CurrentLineNumber       = 0;
			OutputLines             = new List<string>();
#if false
			xxxValidDurations = new List<(string duration, string name)> {
				("96", "whole"),
				("48", "half"),
				("36", "quarter"),		// dotted quarter
				("24", "quarter"),
				("18", "eighth"),		// dotted eighth
				("12", "eighth"),
				("6",  "16th"),
				("3",  "32nd"),
				("1",  "64th")
			};
#endif
			SetupCommands();
		}

//---------------------------------------------------------------------------------------

		private void SetupCommands() {
			Commands = new Dictionary<string, (Func<string[], bool>, Func<string[], bool>)> {
				["LOAD"]          = (CmdLoadMusicFile, CheckSyntax_Load),
				["TIME-SIGNATURE"] = (CmdNonce, CmdNonce),	// TODO:
				["RHYTHM"]		   = (CmdNonce, CmdNonce),	// TODO:
				["TUPLE"]		   = (CmdNonce, CmdNonce),	// TODO:
				["APPLY-RHYTHM "] = (CmdApplyRhythm,   CheckSyntax_ApplyRhythm),
				["SAVE"]          = (CmdSaveMusicFile, CheckSyntax_Save),
				["HELP"]          = (CmdHelp, CmdNOP),
									
				// The following are semi-undocumented diagnostic commands
				["DUMP-RHYTHM"]		 = (CmdNonce, CmdNonce),	// TODO:
				["DUMP-MEASURES"]    = (dbgDumpMeasures,	CheckSyntax_dbgDumpMeasures),
				["DELETE-MEASURES"]  = (dbgDeleteMeasures,  CheckSyntax_dbgDeleteMeasures),
				["RESTORE-MEASURES"] = (dbgRestoreMeasures, CheckSyntax_dbgRestoreMeasures)
			};
		}

//---------------------------------------------------------------------------------------

		private bool CmdNonce(string[] tokens) {
			Msg($"Nonce error on {tokens[0]}");
			return true;
		}

//---------------------------------------------------------------------------------------

		private bool CmdNOP(string[] tokens) {
			return true;
		}

//---------------------------------------------------------------------------------------

		// The interpreter
		public bool Run() {
			var Code = File.ReadAllLines(ProgramName);
			Msg("Rhythm And Clues -- version 2019-04-19, 10:20 AM");
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

			// tokens[0] is the command (e.g. Load). Find the method address in the 
			// Commands dictionary and its syntax-checking routine.
			(Func<string[], bool> command, Func<string[], bool> SyntaxChecker) cmd;
			bool bFound = Commands.TryGetValue(tokens[0].ToUpper(), out cmd);
			if (bFound) {
				// Call the routine (e.g. ApplyRhythm) for the command ("APPLY-RHYTHM")
				// This will be either the syntax checking routine, or the execution code
				if ((cmd.SyntaxChecker != null) && IsSyntaxChecking) {     // Just check syntax
					return cmd.SyntaxChecker(tokens);
				} else {
					return cmd.command(tokens);
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
		private void SyntaxError(string msg) {
			Msg($"Syntax error at line {CurrentLineNumber} -- {msg}");
		}

//---------------------------------------------------------------------------------------

		// Checks the syntax of the LOAD command
		private bool CheckSyntax_Load(string[] tokens) {
			bool retval = true;
			if (tokens.Length == 1) {
				SyntaxError("Syntax: LOAD <filename>");
				return false;
			}

			InputFilename  = JoinParms(tokens, 1);
			if (File.Exists(InputFilename)) {
				bLoadCommandFound = true;
			} else { 
				SyntaxError($"Input file does not exist: {InputFilename}");
				retval = false;
			}
			return retval;
		}

//---------------------------------------------------------------------------------------

		// The LOAD command
		private bool CmdLoadMusicFile(string[] tokens) {
			InputFilename        = JoinParms(tokens, 2);
			xml                  = new MusicXml(InputFilename);
			NumNotes             = xml.GetNotes().Count;
			OriginalFirstMeasure = xml.Measures[0] as XmlElement;
			InputFilename        = JoinParms(tokens);

			SaveMeasures = new List<XmlElement>();
			foreach (var meas in xml.Measures) {
				SaveMeasures.Add(meas as XmlElement);
			}

			PermNotes = new List<XmlElement>();
			foreach (XmlElement note in xml.GetNotes()) {
				PermNotes.Add(note);
			}

			return true;
		}

//---------------------------------------------------------------------------------------

		// Checks the syntax of the APPLY-PATTERN command
		private bool CheckSyntax_ApplyRhythm(string[] tokens) {
			bool retval = true;
			if (!bLoadCommandFound) {
				SyntaxError($"Error: Can't {tokens[0]} because no valid LOAD was found");
				retval = false;
			}
			bool bNotesOK = ParsePatternNotes(tokens);
			if (bNotesOK) {
				int NumPatternDurations = PatternDurations.Count;
				int Remainder = NumNotes % NumPatternDurations;
				if (Remainder != 0) {
					// TODO: Pad the input notes to a multiple of the pattern length
					Msg($"Warning: The number of notes ({NumNotes}) in '{InputFilename}' is not a multiple of the pattern length ({NumPatternDurations})");
				}
			} else {
				retval = false;
			}
			return retval;
		}

//---------------------------------------------------------------------------------------

		// The APPLY-PATTERN command
		private bool CmdApplyRhythm(string[] tokens) {
#if false
			bool bNotesOK = ParsePatternNotes(tokens);
			if (bNotesOK) {
				int Remainder = NumNotes % PatternDurations.Count;
				if (Remainder != 0) {
					throw new Exception($"Number of notes ({NumNotes}) in '{InputFilename}' is not a multiple of pattern length ({NumPatternDurations})");
				}
			}
#endif

			var notes = xml.GetNotes();
			var enumerator = NextPatternDuration().GetEnumerator();
			int NoteNum = 0;

#if false   // ?Something like this?
			// foreach (var duration in GetNextPatternNote()) {	// Wrap around if more
			// 													// notes than pattern length
			for i from 1 to Math.Ceil(notes.count / pattern.Length)
			{

			}
				var note = GetNextNote();			// Wrap around if not multiple
													// of pattern length
			}
				
#endif

			foreach (XmlElement note in notes) {
				var dur = GetFirstElement(note, "duration");
				if (dur is null) {
					Msg("No duration found. Grace Note?");
					continue;
				}
				var type = GetFirstElement(note, "type");
				if (type is null) {
					Msg("No note type found. Grace Note?");
					continue;
				}
				string OriginalDuration = dur.InnerText;
				enumerator.MoveNext();
				var NewDurType  = enumerator.Current;
				dur.InnerText   = NewDurType.Duration.ToString();

				string OriginalType = type.InnerText;
				type.InnerText      = NewDurType.Name;
				Msg($"Converted note[{++NoteNum}] duration/type from {OriginalDuration}/{OriginalType} to {dur.InnerText}/{type.InnerText}");
			}

#if true
			Msg();
			Msg("Nonce error on setting the time signature. I don't believe this " +
				"can be done uniquely. To take just one example, a passage with, " +
				"say, 5 quarter notes. Is its time signature 5/4 or 1/4?");
#else
			int Remainder = NumNotes % PatternDurations.Count;
			if (Remainder != 0) {
				Debugger.Break();				// TODO: Insert rests (of what duration?)
			}

			RemeasureScore(PatternDurations.Count);
#endif

			return true;
		}

//---------------------------------------------------------------------------------------

		static int Gcd(int a, int b) {
			while (b != 0) {
				int t = b;
				b = a % b;
				a = t;
			}
			return a;
		}

//---------------------------------------------------------------------------------------

		// The HELP command
		private bool CmdHelp(string[] tokens) {
			if (IsSyntaxChecking) {
				if (tokens.Length != 1) {
					SyntaxError($"The {tokens[0]} command does not take any parameters.");
					return false;
				}
				return true;
			}
			Msg("The valid commands are:");
			Msg("\tLoad (MuseScore | Finale) <filename>");
			Msg("\tApply-Pattern <list of note durations>. They are:");
			Msg("\t\t" + string.Join(", ", ValidDurations.Keys));
			Msg("\tDump-Measures");
			Msg("\tSave <filename>");
			return true;
		}

//---------------------------------------------------------------------------------------

		private bool CheckSyntax_dbgDumpMeasures(string[] tokens) {
			bool retval = true;
			if (!bLoadCommandFound) {
				SyntaxError($"Error: Can't {tokens[0]} because no LOAD was done");
				retval = false;
			}
			return retval;
		}

//---------------------------------------------------------------------------------------

		// The DUMP-MEASURES command
		private bool dbgDumpMeasures(string[] tokens) {
			foreach (XmlElement meas in xml.Measures) {
				// A bunch of messing around just to make things look pretty
				int MeasNumWidth = (int)(Math.Ceiling(Math.Log10(xml.Measures.Count)));
				MeasNumWidth += "Measure[]".Length + 4;	// 4 -> subscripts up to 9999
				string num = meas.GetAttribute("number");
				Msg();
				string MeasureText = $"Measure[{num}]".PadRight(MeasNumWidth);
				int n = 0;
				string hdr = $"{MeasureText} Pitch  Duration     Type";
				Msg("".PadLeft(hdr.Length + 1, '='));
				Msg(hdr);
				Msg( "               -----  --------  -------");
				foreach (XmlElement note in meas.GetElementsByTagName("note")) {
					string subN     = $"  [{++n}]".PadRight(6);
					// A rest has no pitch or type
					var pitchElem = GetFirstElement(note, "pitch");
					var dur       = GetFirstElement(note, "duration");
					string duration = (dur is null ? "N/A" : dur.InnerText).PadLeft(8);
						string pitch = " rest";       // Probably not, but just in case
						string type  = "  rest";
					if (!(pitchElem is null)) {       // Check for a non-rest
						pitch = pitchElem.InnerText.PadLeft(5);
						type  = GetFirstElement(note, "type").InnerText.PadLeft(6);
					} 
					Msg($" {subN}        {pitch}  {duration}  {type}");
				}
			}
			return true;
		}

//---------------------------------------------------------------------------------------

		private bool CheckSyntax_Save(string[] tokens) {
			bool retval = true;
			if (IsSyntaxChecking) {
				if (!bLoadCommandFound) {
					SyntaxError($"Error: Can't SAVE because no LOAD was done");
					retval = false;
				}
				var OutputFilename = JoinParms(tokens);
				if (InputFilename == OutputFilename) {
					SyntaxError($"SAVE filename the same as most recent LOAD filename");
					retval = false;
				}
			}
			return retval;
		}

//---------------------------------------------------------------------------------------

		// The SAVE command
		private bool CmdSaveMusicFile(string[] tokens) {
			var OutputFilename = JoinParms(tokens);
			if (File.Exists(OutputFilename)) {
				Msg($"The output filename exists.");
				string YN = GetYesNo();
				if (YN == "N") {
					SyntaxError("Output file will not be overwritten");
					return false;
				}
			}
			// int MeasureLength = RecalculateTimeSignature();
			// RemeasureScore(PatternDurations.Count);
			try {
				xml.Save(OutputFilename);
				Msg($"File saved successfully to {OutputFilename}");
				return true;
			} catch (Exception ex) {
				throw new Exception($"Error: Could not save to {OutputFilename} -- '{ex.Message}'");
			}
		}

#if false
//---------------------------------------------------------------------------------------

		private bool CheckSyntax_Target(string[] tokens) {
			// TODO: No Target command any more
			if (tokens.Length != 2) {
				Msg($"The TARGET command takes one parameter, either MuseScore or Finale");
				return false;
			}
			switch (tokens[1].ToUpper()) {
			case "MuseScore":
				SetFinaleDurations();
				break;
			case "FINALE":
				SyntaxError("Nonce error: Finale not supported");	// TODO:
				return false;
				// break;
			default:
				Console.WriteLine($"Invalid TARGET parameter: {tokens[1]}");
				return false;
			}
			return true;
		}
#endif

//---------------------------------------------------------------------------------------

		private void SetMuseScoreDurations() {
			ValidDurations = new Dictionary<string, DurationDef> {
				["1"]      = new DurationDef(32, "whole",	false),
				["1."]     = new DurationDef(48, "whole",	true),
				["1/2"]    = new DurationDef(16, "half",	false),
				["1/2."]   = new DurationDef(24, "half",	true),
				["1/4"]    = new DurationDef(8,  "quarter", false),
				["1/4."]   = new DurationDef(12, "quarter", true),
				["1/8"]    = new DurationDef(4,  "eighth",	false),
				["1/8."]   = new DurationDef(6,  "eighth",	true),
				["1/16"]   = new DurationDef(2,  "16th",	false),
				["1/16."]  = new DurationDef(3,  "16th",	true),
				["1/32"]   = new DurationDef(1,  "32nd",	false)
				// No support for dotted 32nd. Duration would have to be 1.5
			};
		}

//---------------------------------------------------------------------------------------

		private string JoinParms(string[] tokens, int StartingIndex = 1) {
			var span = tokens.AsSpan().Slice(StartingIndex);
			return string.Join(' ', span.ToArray());
		}

//---------------------------------------------------------------------------------------

		// After applying a pattern, we may well have a different time signature
		// https://en.wikipedia.org/wiki/Time_signature
		// TODO: Get rid of next comment
		// The time signature is a notational convention used in Western musical notation
		// to specify how many beats (pulses) are contained in each measure (bar), and
		// which note value is equivalent to a beat.
		private int RecalculateTimeSignature() {
			int SumDurations = PatternDurations.Sum(p => Convert.ToInt32(p.Duration));
			// TODO: Rates is wrong. Use ValidDurations
			var Rates = new List<(int div, int rate)> { (24, 4), (12, 8), (6, 16) };
			foreach (var item in Rates) {
				int quot = Math.DivRem(SumDurations, item.div, out int rem);
				if (rem == 0) {
					SetTimeSignature(quot, item.rate);
					return quot;
				}
			}
			throw new Exception("Can't determine the time signature");
		}

//---------------------------------------------------------------------------------------

		private void SetTimeSignature(int numerator, int denominator) {
			OriginalFirstMeasure = xml.Measures[0] as XmlElement;
			// Note: I *could* do error checking here (e.g. if (beats is null)), but I'll
			//		 assume that the XML is always right in this case (unlike, say,
			//		 <note> which might not have a duration (e.g. a rest))
			var beats           = OriginalFirstMeasure.SelectSingleNode("attributes/time/beats");
			beats.InnerText     = numerator.ToString();
			var beat_type       = OriginalFirstMeasure.SelectSingleNode("attributes/time/beat-type");
			beat_type.InnerText = denominator.ToString();
			Msg($"Time signature set to {numerator} / {denominator}");
		}

#if false   // MeasureNotes
//---------------------------------------------------------------------------------------

		private IEnumerable<List<XmlElement>> MeasureNotes() {

			var NoteList = new List<XmlElement>();
			var Notes    = xml.GetNotes();
			int nNotes   = Notes.Count;
			var MeasureElement = xml.xdoc.CreateElement("measure");
			for (int i = 0; i < Notes.Count; i++) {
				NoteList.Add(Notes[i] as XmlElement);
				if ((i % PatternDurations.Count) == 0) {
					// TODO: Add NoteList to measure
					yield return NoteList;
					NoteList.Clear();
				}
			}
		}
#endif

//---------------------------------------------------------------------------------------

			/// <summary>
			/// Basically, delete all the cuurent measures (keeping a copy of the first
			/// measure that has extra info in it, such as the time signature). Then
			/// create as many measures (except the first with the extra info)
			/// </summary>
			/// <param name="measureLength"></param>
		private void RemeasureScore(int measureLength) {
			int nMeasure = 0;
			int MeasureLength = RecalculateTimeSignature();
			DeleteAllMeasures();

			XmlElement BaseForMeasures = xml.FirstPart;
			foreach (var notes in NextMeasureNotes(measureLength)) {
				Msg($"***Inserting new measure [{++nMeasure}] -- {notes.Count} notes");
				XmlElement NewMeasure = InsertNewMeasure(BaseForMeasures, nMeasure);

				int nnote = 0;
				foreach (var note in notes) {
					var dur        = GetFirstElement(note, "duration");
					dur.InnerText  = PatternDurations[nnote].Duration.ToString();
					var type       = GetFirstElement(note, "type");
					type.InnerText = PatternDurations[nnote].Name;
					var pitch = GetFirstElement(note, "pitch");
					string pit = pitch.InnerText;

					// No idea what to put in <stem> or <beam>. Hopefully Finale will
					// do that.

					NewMeasure.AppendChild(note);
					Msg($"\t[{++nnote}]: {dur.InnerText,2} {pit}");
				}
				BaseForMeasures.AppendChild(NewMeasure);
			}

#if false
			// var MeasureInfos = new List<ElementInfo>();
			// foreach (XmlElement meas in xml.Measures) {
			// 	MeasureInfos.Add(new ElementInfo(meas));
			// }

			/*
			Delete existing measures
			int MeasureNum = 0;
			int NoteNum = 0;
			while (true) {
				meas = MeasureInfos[MeasureNum++];
				meas.updateWithNoteInfo(GetNotesForMeasure());
				xdoc.AddElement(meas);	// Does it matter where we put this?
				if (consumed all Notes), break;
			}
			*/

			var NoteInfos = new List<ElementInfo>();
			foreach (XmlElement note in xml.GetNotes()) {
				NoteInfos.Add(new ElementInfo(note));
			}
#endif
		}

//---------------------------------------------------------------------------------------

		private XmlElement InsertNewMeasure(XmlElement BaseElement, int nMeasure) {
			XmlElement NewMeasure;
			if (nMeasure == 1) {
				// BaseElement.InsertAfter(OriginalFirstMeasure, null);
				NewMeasure = BaseElement.AppendChild(OriginalFirstMeasure) as XmlElement;
				DeleteElementNodes(NewMeasure, "note");
			} else {
				string comment = "========================================================";
				var xcomment = xml.xdoc.CreateComment(comment);
				BaseElement.AppendChild(xcomment);
				NewMeasure = ShinyNewMeasure(nMeasure);
				BaseElement.AppendChild(NewMeasure);
			}

			return NewMeasure;
		}

//---------------------------------------------------------------------------------------

		private XmlElement ShinyNewMeasure(int measureNumber) {
			var meas = xml.xdoc.CreateElement("measure");
			meas.SetAttribute("number", measureNumber.ToString());
			return meas as XmlElement;
		}

//---------------------------------------------------------------------------------------

		private XmlElement GetFirstElement(XmlElement node, string elementName) {
			var xnode = node.SelectSingleNode(elementName);
			if (xnode is null) return null;
			return xnode as XmlElement;
		}

#if false
		//---------------------------------------------------------------------------------------

		// A measure is simply a set of notes. The number of notes (regardless of
		// duration) is simply the number of parameters to APPLY-PATTERN. If there
		// are, say, 6 parameters (6 is the measureLength parameter), this routine,
		// each time it's called, will return the next 6 <note>s.
		private IEnumerable<List<XmlElement>> xxx_Old_NextMeasure(int measureLength) {
			var notes = new List<XmlElement>();
			int dur = 0;
			foreach (XmlElement note in xml.GetNotes()) {
				var NoteDur = GetFirstElement(note, "duration");
				if (NoteDur is null) { continue; /* presumably a rest */ }
				notes.Add(note);
				dur += Convert.ToInt32(NoteDur.InnerText);
				if (dur == measureLength) {
					yield return notes;
					notes.Clear();
					dur = 0;
				} else if (dur > measureLength) {
					throw new Exception("Can't find a measure");	// TODO: Better msg, w/measure, note #'s
				}
			}
		}
#endif

//---------------------------------------------------------------------------------------

		// A measure is simply a set of notes. The number of notes (regardless of
		// duration) is simply the number of parameters to APPLY-PATTERN. If there
		// are, say, 6 parameters (6 is the measureLength parameter), this routine,
		// each time it's called, will return the next 6 <note>s.
		private IEnumerable<List<XmlElement>> NextMeasureNotes(int measureLength) {
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

		private bool CheckSyntax_dbgDeleteMeasures(string[] tokens) {
			bool retval = true;
			if (!bLoadCommandFound) {
				SyntaxError($"Error: Can't {tokens[0]} because no LOAD was done");
				retval = false;
			}
			return retval;
		}

//---------------------------------------------------------------------------------------

		// The DELETE-MEASURES command (normally called automatically)
		private bool dbgDeleteMeasures(string[] tokens) {
			DeleteAllMeasures();
			return true;
		}

//---------------------------------------------------------------------------------------

		private void DeleteAllMeasures() {
			var FirstPart = xml.FirstPart;
			while (true) {
				var measure = GetFirstElement(FirstPart, "measure");
				if (measure is null) { break; }     // No measures found
				FirstPart.RemoveChild(measure);
				var FirstKid = FirstPart.FirstChild;
				if ((FirstKid != null) && (FirstKid.Name == "#comment")) {
					FirstPart.RemoveChild(FirstPart.FirstChild);
				}
			}
		}

//---------------------------------------------------------------------------------------

		private bool CheckSyntax_dbgRestoreMeasures(string[] tokens) {
			bool retval = true;
			if (!bLoadCommandFound) {
				SyntaxError($"Error: Can't {tokens[0]} because no LOAD was done");
				retval = false;
			}
			return retval;
		}

//---------------------------------------------------------------------------------------

		// The RESTORE-MEASURES command
		private bool dbgRestoreMeasures(string[] tokens) {
			var FirstPart = xml.FirstPart;
			var Parent = FirstPart.ParentNode;
			for (int i = 0; i < SaveMeasures.Count; i++) {
				Parent.InsertAfter(SaveMeasures[i], FirstPart);
			}
			return true;
		}

//---------------------------------------------------------------------------------------

		private void DeleteElementNodes(XmlNode elem, string nodeName) {
			var nodes = elem.SelectNodes(nodeName);
			foreach (XmlNode node in nodes) {
				elem.RemoveChild(node);
			}
		}

//---------------------------------------------------------------------------------------

		// Parse parameters for APPLY-PATTERN command
		private bool ParsePatternNotes(string[] tokens) {
			bool bAllParmsOK = true;
#if false
			PatternDurations = new List<(string duration, string type)>();
			bool bOK;
			// All parameters must be integers and one of our valid ones
			for (int i = 1; i < tokens.Length; i++) {
				bOK = int.TryParse(tokens[i], out int n);
				var dur_type = IsValidNoteDuration(tokens[i]);
				if (bOK && (dur_type.duration != null)) {
					PatternDurations.Add(dur_type);
				} else {
					SyntaxError($"Error: Parameter {i} to {tokens[0]} is invalid: {tokens[i]}");
					bAllParmsOK = false;
				}
			}
#else
			PatternDurations = new List<DurationDef>();
			for (int i = 1; i < tokens.Length; i++) {
				bool bFound = ValidDurations.TryGetValue(tokens[i], out DurationDef dur);
				if (bFound) {
					PatternDurations.Add(dur);
				} else {
					SyntaxError($"Error: Parameter {i} to {tokens[0]} is invalid: {tokens[i]}");
					bAllParmsOK = false;
				}
			}
#endif
			return bAllParmsOK;
		}

#if false
//---------------------------------------------------------------------------------------

		// Check for typos in APPLY-PATTERN arguments
		private (string duration, string name) xxxIsValidNoteDuration(string parm) {
			foreach (var dur_type in ValidDurations) {
				if (parm == dur_type.duration) { return dur_type; }
			}
			return (null, null);
		}
#endif

//---------------------------------------------------------------------------------------

		// Return next pattern parameter in turn. Wrap around if necessary
		// private IEnumerable<(string duration, string type)> NextPatternDuration() {
		private IEnumerable<DurationDef> NextPatternDuration() {
			int i = 0;
			while (true) {
				yield return PatternDurations[i++];
				if (i >= PatternDurations.Count) { i = 0; }
			}
		}

//---------------------------------------------------------------------------------------

		// Generic message routine
		private void Msg(string s = null, bool addNewline = true) {
			s = s ?? "";				// aka if (s is null) s = ""
			OutputLines.Add(s);
			if (addNewline) {
				Console.WriteLine(s);
			} else {
				Console.Write(s);
			}
		}

//---------------------------------------------------------------------------------------

		private string GetYesNo() {
			while (true) {
				Msg("Enter Y to overwrite or N to quit: ", false);
				var c = Console.ReadKey();
				Console.WriteLine();
				switch (c.Key) {
				case ConsoleKey.Y:
					return "Y";
				case ConsoleKey.N:
					return "N";
				default:
					Msg("Invalid response");
					break;
				}
			}
		}

#if false   // Routines from Version 1


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
						SyntaxError($"Parameter {i+1}: Invalid numeric; must be an integer > 0");
						bAllParmsOK = false;
					}
					break;
				case 'F':
					bOK = float.TryParse(parm, out float f);
					if (!bOK) {
						SyntaxError($"Parameter {i+1}: Invalid numeric; must be > 0");
						bAllParmsOK = false;
					}
					break;
				case 'S':
					if (tokens.Length <= 1) {
						SyntaxError($"Command {tokens[0]} must have at least one parameter");
					}
					break;
				default:    // Shouldn't happen
					throw new Exception("Internal Error -- Wrong parameter configuration for internal routine ParmsOK");
				}
			}
			return bAllParmsOK;
		}

//---------------------------------------------------------------------------------------

		private void Summary(string[] tokens) {
			// TODO: Check tokens length to ensure no extraneous parms
			if (IsSyntaxChecking) { return; }
			int n = 0;
			Console.WriteLine("Part Names:");
			foreach (var name in xml.GetPartNames()) {
				Msg($"[{++n}]: {name}");
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
			Msg($"Part '{PartNum}' selected");
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
			Msg($"Measures selected: [{ixStartMeasure}{'.'}.{ixEndMeasure}]");
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
				Msg($"Measure {i + 1}:");
				var mes = xml.Measures[i] as XmlElement;
				var notes = mes.GetElementsByTagName("note");
				int nNote = 0;
				foreach (XmlElement note in notes) {
					Msg($"\tNote[{++nNote}]: ");
					// TODO: I've seen some notes with <rest> or <grace> tags. What else?
					//		 The grace notes have no duration. The <rest> I can just skip
					var dur = note.GetElementsByTagName("duration");
					if (dur.Count == 0) {
						Msg("No duration found. Grace Note?");
						continue;
					}
					// Presumably there's only one duration tag per note
					int OriginalDuration = Convert.ToInt32(dur[0].InnerText);
					int NewDuration      = (int)Math.Round(OriginalDuration * factor);
					dur[0].InnerText     = NewDuration.ToString();
					Msg($"Converted duration from {OriginalDuration} to {NewDuration}");
				}
			}
		}
#endif
	}
}
