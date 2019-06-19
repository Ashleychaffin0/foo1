using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace RhythmAndClues {
	class Interpreter {
		readonly string			ProgramName;				// From command line
		int						CurrentLineNumber;			// For runtime error messages
		readonly List<string>	OutputLines;				// Saves Msg()'s
		Dictionary<string, ICommand> Commands;
		ICommand				CurrentCommand;

		// The following fields marked internal are implicit parameters to one or
		// more commands. In this quick(?) and dirty(!) program, I'll do it this way.
		// In a less personal program I'd approach things differently.

		// Further note: The problem is that the different Commands (LOAD, SAVE, RHYTHM,
		// etc) need to synchronize with each other. For example, LOAD needs a place to
		// save the input XML elements, APPLY-RHYTHM needs to modify them, and SAVE has
		// to write them out. Meanwhile, having to support multiple instances of
		// RHYTHM, interspersed with TUPLETs required further intercommunication. And
		// so on. I suppose I coult have tried to group things into several data-mostly
		// classes and passed them, but the project never evolved that way.
		internal MusicXml	 XmlScore;
		internal MusicXml	 XmlPattern;

		internal bool		 IsSyntaxChecking;      // In pass 1 (true) or pass 2 (false)

		internal string		 NotesFilename;   // "Remember where we parked" -- J. T. Kirk
		internal string		 PatternFilename;

		internal bool		 bLoadNotesFound;       // Some cmds need LOAD done first
		internal bool		 bLoadPatternFound;		// Ditto for the pattern

//---------------------------------------------------------------------------------------

		public Interpreter(string progName) {	// Constructor (ctor)
			ProgramName       = progName;
			bLoadNotesFound   = false;          // So we can check if LOAD-NOTES
												//	has been called
			bLoadPatternFound = false;			// Ditto for the Pattern
			CurrentLineNumber = 0;
			OutputLines       = new List<string>();
			SetupCommands();
		}

//---------------------------------------------------------------------------------------

		private void SetupCommands() {
			Commands = new Dictionary<string, ICommand> {
				["HELP"]           = new CmdHelp(this),
				["LOAD-NOTES"]     = new CmdLoadNotes(this),
				["LOAD-PATTERN"]   = new CmdLoadPattern(this),
				["APPLY-RHYTHM"]   = new CmdApplyRhythm(this),
				["SAVE"]           = new CmdSave(this),
				// The following is a semi-undocumented diagnostic command
				["DUMP-MEASURES"]  = new CmdDumpMeasures(this),
		};
			// Define aliases. Make sure they point to the same class instance as
			// the long forms.
			Commands.Add("AR", Commands["APPLY-RHYTHM"]);
		}

//---------------------------------------------------------------------------------------

		// The interpreter
		public bool Run() {
			var Code = File.ReadAllLines(ProgramName);
			Msg("Rhythm And Clues -- version 2019-05-02, 9:35 PM");
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
					case "LOAD-NOTES":
					case "LOAD-PATTERN":
						break;
					default:
						if (!bLoadNotesFound) {
							SyntaxError($"Error: Can't {tokens[0]} because no LOAD-NOTES was done");
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

		internal string JoinParms(string[] tokens, int StartingIndex = 1) {
			var span = tokens.AsSpan().Slice(StartingIndex);
			return string.Join(' ', span.ToArray());
		}

//---------------------------------------------------------------------------------------

		internal XmlElement GetFirstElement(XmlElement node, string elementName) {
			var xnode = node.SelectSingleNode(elementName);
			if (xnode is null) return null;
			return xnode as XmlElement;
		}

//---------------------------------------------------------------------------------------

		internal bool AreLoadCommandsIssued(string cmd) {
			if (!bLoadNotesFound) {
				SyntaxError($"Error: Can't {cmd} because no valid LOAD-NOTES was found");
				return false;
			}
			if (!bLoadPatternFound) {	// Todo Bug
				SyntaxError($"Error: Can't {cmd} because no valid LOAD-PATTERN was found");
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
	}
}
