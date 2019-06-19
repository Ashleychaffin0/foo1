using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace RhythmAndClues {
	class CmdLoad : ICommand {
		readonly Interpreter main;

//---------------------------------------------------------------------------------------

	public CmdLoad(Interpreter main) {
		this.main = main;
	}

//---------------------------------------------------------------------------------------

		public bool CheckSyntax(string[] tokens) {
			bool retval = true;
			if (tokens.Length == 1) {
				main.SyntaxError("Syntax: LOAD <filename>");
				retval = false;
			}

			main.ScoreFilename = main.JoinParms(tokens);
			if (File.Exists(main.ScoreFilename)) {
				main.bLoadCommandFound = true;
			} else {
				main.SyntaxError($"Input file does not exist: {main.ScoreFilename}");
				retval = false;
			}
			main.RhythmDurations = new List<DurationDef>();
			return retval;
		}

//---------------------------------------------------------------------------------------

		public bool Execute(string[] tokens) {
			main.ScoreFilename        = main.JoinParms(tokens);
			main.XmlScore             = new MusicXml(main.ScoreFilename);
			main.NumNotes             = main.XmlScore.GetNotes().Count;
			main.OriginalFirstMeasure = main.XmlScore.Measures[0] as XmlElement;
			var div                   = main.OriginalFirstMeasure.GetElementsByTagName("divisions");
			main.Divisions            = int.Parse(div[0].InnerText);    // TODO: What if none?
			main.SetBasicDurations();

			main.ScoreNotes           = main.XmlScore.GetNotes();
			main.OriginalFirstMeasure = main.XmlScore.Measures[0] as XmlElement;

			main.OriginalMeasures = new List<XmlElement>();
			foreach (var meas in main.XmlScore.Measures) {
				main.OriginalMeasures.Add(meas as XmlElement);
			}

			main.PermNotes = new List<XmlElement>();
			foreach (XmlElement note in main.XmlScore.GetNotes()) {
				main.PermNotes.Add(note);
			}

			return true;
		}
	}
}
