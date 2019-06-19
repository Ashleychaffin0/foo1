#nullable enable

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
				main.bLoadCommandFound = true;		// TODO: Need count, not bool
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
			main.PatternNotes    = main.XmlPattern.GetNotes();

			// Apply - foreach note, delete element <rest>, add <pitch>

#if false
			var PatternDoc = main.XmlPattern.xdoc;
			// var note = main.ScoreNotes[0];
			// XmlElement pitchScore = note["pitch"];

			var pitch = PatternDoc.CreateElement("pitch");
			pitch.InnerXml = pitchScore.InnerXml;

			foreach (XmlNode node in main.ScoreNotes) {
				main.DeleteElementNodes(node, "rest");
				node.AppendChild(pitch);
			}
#endif
			return true;
		}
	}
}
