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
		// TODO: Group internals; align comments
		string	 ProgramName;			// From command line
		internal MusicXml xml;
		internal bool	IsSyntaxChecking;      // In pass 1 (true) or pass 2 (false)

		internal string InputFilename;     // "Remember where we parked" -- J. T. Kirk

		// cmd name & ref to routine and to syntax checking routine for that cmd
		// Dictionary<string, (Func<string[], bool>, Func<string[], bool>)> Commands;
		Dictionary<string, ICommand> Commands;		// TODO:

		// List<(string duration, string name)> ValidDurations; // Valid parms Pattern
		internal Dictionary<string, DurationDef> ValidDurations; // Valid parms Pattern

		internal List<DurationDef> PatternDurations;	// Parms from APPLY-PATTERN

		int CurrentLineNumber;				// For runtime error messages
		internal bool bLoadCommandFound;				// Some cmds need a LOAD done first

		List<string> OutputLines;           // Saves Msg()'s

		internal int NumNotes;

		internal int Divisions;

		internal List<XmlElement>	SaveMeasures;
		internal XmlElement			OriginalFirstMeasure;
		internal List<XmlElement>	PermNotes;

//---------------------------------------------------------------------------------------

		public Interpreter(string progName) {	// Constructor (ctor)
			ProgramName             = progName;
			bLoadCommandFound       = false;   // So we can check if LOAD has been called
			CurrentLineNumber       = 0;
			OutputLines             = new List<string>();
			// SetupCommands();
			SetupCommands();
		}

#if false
		//---------------------------------------------------------------------------------------

		private void SetupCommands() {
			Commands = new Dictionary<string, (Func<string[], bool>, Func<string[], bool>)> {
				["LOAD"]           = (CheckSyntax_Load,			CmdLoadMusicFile),
				["TIME-SIGNATURE"] = (CmdNonce,					CmdNonce),	// TODO:
				// ["RHYTHM"]		   = (CheckSyntax_Rhythm,		CmdRhythm),	// TODO:
				["TUPLET"]		   = (CmdNonce, CmdNonce),	// TODO:
				["APPLY-RHYTHM"]   = (CheckSyntax_ApplyRhythm,	CmdApplyRhythm),
				["SAVE"]           = (CheckSyntax_Save,			CmdSaveMusicFile),
				["HELP"]           = (CmdNOP,					CmdHelp),
									
				// The following are semi-undocumented diagnostic commands
				["DUMP-RHYTHM"]		 = (CmdNonce, CmdNonce),	// TODO:
				["DUMP-MEASURES"]    = (CheckSyntax_dbgDumpMeasures,	dbgDumpMeasures),
				["DELETE-MEASURES"]  = (CheckSyntax_dbgDeleteMeasures,	dbgDeleteMeasures),
				["RESTORE-MEASURES"] = (CheckSyntax_dbgRestoreMeasures, dbgRestoreMeasures)
			};
		}
#endif

//---------------------------------------------------------------------------------------

		private void SetupCommands() {
			Commands               = new Dictionary<string, ICommand> {
				["HELP"]           = new CmdHelp(),
				["LOAD"]           = new CmdLoad(),
				// TODO: All below only work if after LOAD. Put in check.
				["TIME-SIGNATURE"] = new CmdTimeSignature(),
				["RHYTHM"]         = new CmdRhythm(),
				["TUPLET"]         = new CmdTuplet(),
				["APPLY-RHYTHM"]   = new CmdApplyRhythm(),
				["SAVE"]           = new CmdSave(),

				// The following are semi-undocumented diagnostic commands
				["DUMP-RHYTHM"]      = new CmdDumpRhythm(),
				["DUMP-MEASURES"]    = new CmdDumpMeasures(),
				["DELETE-MEASURES"]  = new CmdDeleteMeasures(),
				["RESTORE-MEASURES"] = new CmdRestoreMeasures()
			};
		}

#if false
		//---------------------------------------------------------------------------------------

		private bool CmdNonce(string[] tokens) {
			Msg($"Nonce error on {tokens[0]}");
			return true;
		}

//---------------------------------------------------------------------------------------

		private bool CmdNOP(string[] tokens) {
			return true;
		}
#endif

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

			bool bFound = Commands.TryGetValue(tokens[0].ToUpper(), out ICommand cmd2);
			if (bFound) {
				if (IsSyntaxChecking) {
					return cmd2.CheckSyntax(this, tokens);
				} else {
					return cmd2.Command(this, tokens);
				}
			} else {
				SyntaxError($"Command '{tokens[0]}' not recognized");
				return false;
			}

#if false
			(Func<string[], bool> SyntaxChecker, Func<string[], bool>  command) cmd;
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
#endif
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

#if false
		//---------------------------------------------------------------------------------------

		// Checks the syntax of the LOAD command
		private bool CheckSyntax_Load(string[] tokens) {
			bool retval = true;
			if (tokens.Length == 1) {
				SyntaxError("Syntax: LOAD <filename>");
				return false;
			}

			InputFilename  = JoinParms(tokens);
			if (File.Exists(InputFilename)) {
				bLoadCommandFound = true;
			} else { 
				SyntaxError($"Input file does not exist: {InputFilename}");
				retval = false;
			}
			return retval;
		}
#endif

//---------------------------------------------------------------------------------------

		// The LOAD command
		private bool CmdLoadMusicFile(string[] tokens) {
			InputFilename        = JoinParms(tokens, 2);
			xml                  = new MusicXml(InputFilename);
			NumNotes             = xml.GetNotes().Count;
			OriginalFirstMeasure = xml.Measures[0] as XmlElement;
			var div = OriginalFirstMeasure.GetElementsByTagName("divisions");			
			Divisions = int.Parse(div[0].InnerText);    // TODO: What if none?
			SetScoreDurations();
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

#if false
		//---------------------------------------------------------------------------------------

		// Checks the syntax of the APPLY-RHYTHM command
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
#endif

#if false
		//---------------------------------------------------------------------------------------

		// The APPLY-RHYTHM command
		private bool CmdApplyRhythm(string[] tokens) {
			var notes = xml.GetNotes();
			var enumerator = NextPatternDuration().GetEnumerator();
			int NoteNum = 0;

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
#endif

//---------------------------------------------------------------------------------------

		static int Gcd(int a, int b) {
			while (b != 0) {
				int t = b;
				b = a % b;
				a = t;
			}
			return a;
		}

#if false
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
			// TODO: Redo this
			Msg("\tLoad <filename>");
			Msg("\tApply-Pattern <list of note durations>. They are:");
			Msg("\t\t" + string.Join(", ", ValidDurations.Keys));
			Msg("\tDump-Measures");
			Msg("\tSave <filename>");
			return true;
		}
#endif

#if false

		//---------------------------------------------------------------------------------------

		private bool CheckSyntax_dbgDumpMeasures(string[] tokens) {
			bool retval = true;
			if (!bLoadCommandFound) {
				SyntaxError($"Error: Can't {tokens[0]} because no LOAD was done");
				retval = false;
			}
			return retval;
		}
#endif

#if false
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
#endif

#if false
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
#endif

#if false
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
#endif

//---------------------------------------------------------------------------------------

		internal void SetScoreDurations() {
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
			var Names = new Dictionary<string, string> {
#if true
				["1"]	= "whole",
				["1."]	= "whole",

				["2"]	= "half",
				["2."]	= "half",

				["4"]	= "quarter",
				["4."]	= "quarter",

				["8"]	= "eighth",
				["8."]	= "eighth",

				["16"]	= "16th",
				["16."] = "16th",

				["32"]	= "32nd",
				["32."] = "32nd",

				["64"]	= "64th",
				["64."] = "64th"
			};

			ValidDurations = new Dictionary<string, DurationDef>();
			foreach (var key in Names.Keys) {
				ValidDurations.Add(key, new DurationDef(key, Names[key]));
			}

#else
				["1"]      = new DurationDef(16, "whole"),
				["1."]     = new DurationDef(8, "whole"),

				["2"]	   = new DurationDef(4, "half"),
				["2."]     = new DurationDef(2, "half"),

				["4"]      = new DurationDef(1,  "quarter"),
				["4."]     = new DurationDef(1.5, "quarter"),

				["8"]      = new DurationDef(1.0 / 2,  "eighth"),
				["8."]     = new DurationDef(.25,  "eighth"),

				["16"]     = new DurationDef(.125,  "16th"),
				["16."]    = new DurationDef(.0625,  "16th"),

				["32"]	   = new DurationDef(1,  "32nd"),
				["32."]    = new DurationDef(1,  "32nd"),

				["64"]     = new DurationDef(1,  "64th"),
				["64."]    = new DurationDef(1, "64th")
			};
#endif
		}

//---------------------------------------------------------------------------------------

		internal string JoinParms(string[] tokens, int StartingIndex = 1) {
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
			// TODO: Figure out where this fits in: DeleteAllMeasures();

			XmlElement BaseForMeasures = xml.FirstPart;
			foreach (var notes in NextMeasureNotes(measureLength)) {
				Msg($"***Inserting new measure [{++nMeasure}] -- {notes.Count} notes");
				XmlElement NewMeasure = InsertNewMeasure(BaseForMeasures, nMeasure);

				int nnote = 0;
				foreach (var note in notes) {
					// TODO: Support <dot/>
					// TODO: Support <time-modification>
					var dur        = GetFirstElement(note, "duration");
					dur.InnerText  = PatternDurations[nnote].Duration.ToString();
					var type       = GetFirstElement(note, "type");
					type.InnerText = PatternDurations[nnote].Name;
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

		internal XmlElement GetFirstElement(XmlElement node, string elementName) {
			var xnode = node.SelectSingleNode(elementName);
			if (xnode is null) return null;
			return xnode as XmlElement;
		}


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

#if false
		//---------------------------------------------------------------------------------------

		private bool CheckSyntax_dbgDeleteMeasures(string[] tokens) {
			bool retval = true;
			if (!bLoadCommandFound) {
				SyntaxError($"Error: Can't {tokens[0]} because no LOAD was done");
				retval = false;
			}
			return retval;
		}
#endif

#if false
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
#endif

#if false
		//---------------------------------------------------------------------------------------

		private bool CheckSyntax_dbgRestoreMeasures(string[] tokens) {
			bool retval = true;
			if (!bLoadCommandFound) {
				SyntaxError($"Error: Can't {tokens[0]} because no LOAD was done");
				retval = false;
			}
			return retval;
		}
#endif

#if false
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
#endif

//---------------------------------------------------------------------------------------

		private void DeleteElementNodes(XmlNode elem, string nodeName) {
			var nodes = elem.SelectNodes(nodeName);
			foreach (XmlNode node in nodes) {
				elem.RemoveChild(node);
			}
		}

//---------------------------------------------------------------------------------------

		// Parse parameters for APPLY-PATTERN command
		internal bool ParsePatternNotes(string[] tokens) {
			bool bAllParmsOK = true;
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
			return bAllParmsOK;
		}

//---------------------------------------------------------------------------------------

		// Return next pattern parameter in turn. Wrap around if necessary
		// private IEnumerable<(string duration, string type)> NextPatternDuration() {
		internal IEnumerable<DurationDef> NextPatternDuration() {
			int i = 0;
			while (true) {
				yield return PatternDurations[i++];
				if (i >= PatternDurations.Count) { i = 0; }
			}
		}

//---------------------------------------------------------------------------------------

		// Generic message routine
		internal void Msg(string s = null, bool addNewline = true) {
			s ??= "";				// aka if (s is null) s = ""
			OutputLines.Add(s);
			if (addNewline) {
				Console.WriteLine(s);
			} else {
				Console.Write(s);
			}
		}

//---------------------------------------------------------------------------------------

		internal string GetYesNo() {
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
	}
}
