// #nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace RhythmAndClues {
	internal class CmdLoadPattern : ICommand {
		private Interpreter main;

//---------------------------------------------------------------------------------------

		public CmdLoadPattern(Interpreter main) {
			this.main = main;
		}

//---------------------------------------------------------------------------------------

		public bool CheckSyntax(string[] tokens) {
			bool retval = true;
			if (tokens.Length == 1) {
				main.SyntaxError("Syntax: LOAD-PATTERN <filename>");
				retval = false;
			}

			main.PatternFilename = main.JoinParms(tokens);
			if (File.Exists(main.PatternFilename)) {
				main.bLoadPatternFound = true;
			} else {
				main.SyntaxError($"Input file does not exist: {main.PatternFilename}");
				retval = false;
			}
			return retval;
		}

//---------------------------------------------------------------------------------------

		public bool Execute(string[] tokens) {
			main.PatternFilename = main.JoinParms(tokens);
			main.XmlPattern      = new MusicXml(main.PatternFilename);
			// main.PatternNotes    = main.XmlPattern.GetNotes();
			return true;
		}
	}
}
