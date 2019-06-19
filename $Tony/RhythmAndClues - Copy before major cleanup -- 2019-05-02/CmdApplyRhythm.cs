using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace RhythmAndClues {
	class CmdApplyRhythm : ICommand {
		readonly Interpreter main;

		// TODO: Can't save on top of Pattern file

//---------------------------------------------------------------------------------------

		public CmdApplyRhythm(Interpreter main) {
			this.main = main;
		}

//---------------------------------------------------------------------------------------

		public bool CheckSyntax(string[] tokens) {
			bool retval = true;
			if (!main.IsLoadCommandIssued("Apply-Rhythm")) { retval = false; }
			if (tokens.Length > 1) {
				main.SyntaxError($"The Apply-Rhythm command takes no parameters");
				retval = false;
			}
			return retval;
		}

//---------------------------------------------------------------------------------------

		public bool Execute(string[] tokens) {
			if (Test()) return true;
			var PatDoc   = main.XmlPattern.xdoc;

			var PatNotes1 = PatDoc.SelectNodes("//note");
			// var PatNotes2 = main.XmlPattern.GetNotes();

			var PatNotes = PatDoc.SelectNodes("//note").GetEnumerator();
			// var PatNotes = main.XmlPattern.GetNotes().GetEnumerator();
			var PatPitch = PatDoc.CreateElement("pitch");
			PatPitch.InnerXml = "<step>A</step><octave>4</octave>";

			var note0 = main.XmlPattern.GetNotes()[0];
			// note0.InnerXml = "<foo>Hello there</foo>";
			note0.AppendChild(PatPitch);
			// main.XmlPattern.GetNotes()[0].InnerXml = "<step>A</step><octave>4</octave>";
			PatDoc.Save(Console.Out);
			if (!(tokens is null)) return true;

			var ScoreNotes = main.XmlScore.xdoc.SelectNodes("//note");
			var p          = main.XmlPattern.GetNotes();
			var d          = main.XmlPattern.xdoc;


			int xx         = 0;
			foreach (XmlElement ScoreNote in ScoreNotes) {
				string ScorePitch = ScoreNote["pitch"].InnerXml;
				Debug.WriteLine($"{xx++}: {ScorePitch}");
				if (!PatNotes.MoveNext()) {
					Debug.WriteLine("Wrapping");
					PatNotes.Reset();
				}
				var cur = PatNotes.Current as XmlElement;
				var b = object.ReferenceEquals(PatNotes.Current, p[0]);
				var b2 = object.ReferenceEquals(cur, p[0]);
				var b3 = object.ReferenceEquals(cur, GetNextPatternNote(PatNotes1));
				cur.RemoveChild(cur["rest"]);
				PatPitch.InnerXml = ScorePitch;
				cur.AppendChild(PatPitch);
				Debug.WriteLine($"{cur.InnerXml}");
			}
			PatDoc.Save(Console.Out);
			return true;
		}

//---------------------------------------------------------------------------------------

		private bool Test() {
			var PatDoc = main.XmlPattern.xdoc;
			var notes = main.XmlPattern.GetNotes();
			int ix = 0;
			var ScoreNotes = main.XmlScore.xdoc.SelectNodes("//note");
			foreach (XmlElement ScoreNote in ScoreNotes) {
				var PatPitch      = PatDoc.CreateElement("pitch");
				PatPitch.InnerXml = ScoreNote["pitch"].InnerXml;
				var note          = notes[ix++];
				var rest = note.SelectSingleNode("rest");
				note.RemoveChild(rest);
				note.AppendChild(PatPitch);
				if (ix >= notes.Count) { ix = 0; }
			}
			PatDoc.Save(Console.Out);

			return true;
		}

//---------------------------------------------------------------------------------------

			XmlElement GetNextPatternNote(XmlNodeList PatNotes1) {
			return PatNotes1[0] as XmlElement;
		}

//---------------------------------------------------------------------------------------

			public bool xxxExecute(string[] tokens) {
			int xx       = 0;
			var PatDoc  = main.XmlPattern.xdoc;
			var PatEnumNotes = PatDoc.GetElementsByTagName("note").GetEnumerator();
			var PatPitch = PatDoc.CreateElement("pitch");

			foreach (XmlElement ScoreNote in main.ScoreNotes) {
				var next = PatEnumNotes.MoveNext();
				if (!next) { PatEnumNotes.Reset(); }	// Wrap around. Need MoveNext as well?
				var PatElem = PatEnumNotes.Current as XmlElement;
				var PatXml  = PatElem.InnerXml;

				Debug.WriteLine($"Score: [{xx++}]: {ScoreNote.InnerXml}");
				Debug.WriteLine($"Pattern: {PatElem.InnerXml}");
				Debug.WriteLine("");

				XmlNode xn = PatElem.SelectSingleNode("rest");
				xn.InnerXml = "Hello dere";

				main.DeleteElementNodes(PatElem, "rest");

				Debug.WriteLine($"Pattern 2: \t{PatElem.InnerXml}");
				PatPitch.InnerXml = ScoreNote["pitch"].InnerXml;
				PatElem.AppendChild(PatPitch);

				// Debug.WriteLine($"Pattern 2: \t{PatDoc.getele}");
#if false
				enumerator.MoveNext();
				var NewDurType = enumerator.Current;
				dur.InnerText = NewDurType.Duration.ToString();

				string OriginalType = type.InnerText;
				type.InnerText = NewDurType.Name;
				main.Msg($"Converted note[{++NoteNum}] duration/type from {OriginalDuration}/{OriginalType} to {dur.InnerText}/{type.InnerText}");
			}
				return true;
#endif
			}
			return true;
		}

//---------------------------------------------------------------------------------------

		// Return next pattern parameter in turn. Wrap around if necessary
		private IEnumerable<XmlElement> xxxNextPatternNote() {
			int i = 0;
			while (true) {
				yield return main.PatternNotes[i] as XmlElement;
				if (i >= main.PatternNotes.Count) { i = 0; }
			}
		}

	}
}
