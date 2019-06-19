using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace RhythmAndClues {
	class CmdLoad : ICommand {
		public bool CheckSyntax(Interpreter main, string[] tokens) {
			bool retval = true;
			if (tokens.Length == 1) {
				main.SyntaxError("Syntax: LOAD <filename>");
				return false;
			}

			main.InputFilename = main.JoinParms(tokens);
			if (File.Exists(main.InputFilename)) {
				main.bLoadCommandFound = true;
			} else {
				main.SyntaxError($"Input file does not exist: {main.InputFilename}");
				retval = false;
			}
			return retval;
		}

//---------------------------------------------------------------------------------------

		public bool Command(Interpreter main, string[] tokens) {
			main.InputFilename        = main.JoinParms(tokens);
			main.xml                  = new MusicXml(main.InputFilename);
			main.NumNotes             = main.xml.GetNotes().Count;
			main.OriginalFirstMeasure = main.xml.Measures[0] as XmlElement;
			var div                   = main.OriginalFirstMeasure.GetElementsByTagName("divisions");
			main.Divisions            = int.Parse(div[0].InnerText);    // TODO: What if none?
			main.SetScoreDurations();

			main.SaveMeasures = new List<XmlElement>();
			foreach (var meas in main.xml.Measures) {
				main.SaveMeasures.Add(meas as XmlElement);
			}

			main.PermNotes = new List<XmlElement>();
			foreach (XmlElement note in main.xml.GetNotes()) {
				main.PermNotes.Add(note);
			}

			return true;
		}
	}
}
