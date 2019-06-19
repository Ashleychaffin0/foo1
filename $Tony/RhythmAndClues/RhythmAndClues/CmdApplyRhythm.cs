using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace RhythmAndClues {
	class CmdApplyRhythm : ICommand {
		readonly Interpreter main;

//---------------------------------------------------------------------------------------

		public CmdApplyRhythm(Interpreter main) {
			this.main = main;
		}

//---------------------------------------------------------------------------------------

		public bool CheckSyntax(string[] tokens) {
			bool retval = true;
			if (!main.AreLoadCommandsIssued("APPLY-RHYTHM")) { retval = false; }
			if (tokens.Length > 1) {
				main.SyntaxError($"The Apply-Rhythm command takes no parameters");
				retval = false;
			}
			return retval;
		}

//---------------------------------------------------------------------------------------

		public bool Execute(string[] tokens) {
			var PatDoc       = main.XmlPattern.xdoc;
			var PatternNotes = main.XmlPattern.GetNotes();
			var ScoreNotes   = main.XmlScore.xdoc.SelectNodes("//note");

			// We'll want to go through all the Score notes, then see if there are are
			// any pattern notes left over. We'll calculate how many there are (if any),
			// and keep re-using the last Score note to finish the final copy of the
			// pattern
			int ixPattern    = 0;

			int NoteNum = 0;
			foreach (XmlElement ScoreNote in ScoreNotes) {
				// Check for missing <pitch> (e.g. for a <rest>)
				if (! ScoreNote.InnerXml.Contains("<pitch>")) {
					string txt = $"Error: Note {NoteNum} missing a <pitch> element." +
						$" XML={ScoreNote.OuterXml}";
					main.Msg(txt);
					return false;
				}
				ProcessNote(PatDoc, PatternNotes[ixPattern], ScoreNote);
				if (++ixPattern >= PatternNotes.Count) { ixPattern = 0; }
			}

			// If the number of Score notes is a perfect multiple of the pattern length,
			// we're done.

			if ((ScoreNotes.Count % PatternNotes.Count) == 0) { return true; }
			// OK, we now know that there is a non-zero number of pattern notes
			// left to process

			// Let's assume we have 22 Score notes, with a pattern length of 7. We'll
			// have done 21 Score notes, repeating the pattern 22/7 (Pi? Well, anyway,
			// integer division), giving us 3. We need to pad this out to 28 Score notes
			// to finish the last part of the pattern. Note that the following calcs
			// are all done in integer arithmetic, with potential truncation whenever
			// we do division.

			int snc = ScoreNotes.Count;		// Make the next calculation
			int pnc = PatternNotes.Count;   // Fit on one line
			int round_up = (snc + pnc - 1) / pnc;
			int nExtraScoreNotes = round_up * pnc - snc;
			// Laura's asked that if we need extra notes, we should just continue using
			// the last note os the Score
			var LastNote = ScoreNotes[ScoreNotes.Count - 1] as XmlElement;

			for (int n = ixPattern; n < PatternNotes.Count; n++) {
				ProcessNote(PatDoc, PatternNotes[n], LastNote);
			}

			return true;
		}

//---------------------------------------------------------------------------------------

		private static void ProcessNote(XmlDocument PatDoc, XmlNode note, XmlElement ScoreNote) {
			var PatPitch      = PatDoc.CreateElement("pitch");
			PatPitch.InnerXml = ScoreNote["pitch"].InnerXml;
			var rest          = note.SelectSingleNode("rest");
			note.RemoveChild(rest);
			note.AppendChild(PatPitch);
			Debug.WriteLine(note.InnerXml);
		}
	}
}
