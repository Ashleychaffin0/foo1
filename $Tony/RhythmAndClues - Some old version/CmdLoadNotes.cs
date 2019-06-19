using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace RhythmAndClues {
	class CmdLoadNotes : ICommand {
		readonly Interpreter main;

//---------------------------------------------------------------------------------------

	public CmdLoadNotes(Interpreter main) {
		this.main = main;
	}

//---------------------------------------------------------------------------------------

		public bool CheckSyntax(string[] tokens) {
			bool retval = true;
			if (tokens.Length == 1) {
				main.SyntaxError("Syntax: LOAD-NOTES <filename>");
				retval = false;
			}

			main.NotesFilename = main.JoinParms(tokens);
			if (File.Exists(main.NotesFilename)) {
				main.bLoadNotesFound = true;
			} else {
				main.SyntaxError($"Input file does not exist: {main.NotesFilename}");
				retval = false;
			}
			return retval;
		}

//---------------------------------------------------------------------------------------

		public bool Execute(string[] tokens) {
			main.NotesFilename = main.JoinParms(tokens);
			main.XmlScore      = new MusicXml(main.NotesFilename);
			return true;
		}
	}
}
