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
			if (!main.IsLoadCommandIssued("Apply-Rhythm")) { retval = false; }
			if (tokens.Length > 1) {
				main.SyntaxError($"The Apply-Rhythm command takes no parameters");
				retval = false;
			}
#if false   // TODO: Is this code needed here? Or elsewhere?
			bool bNotesOK = ParsePatternNotes(tokens);	// TODO: Put this rtn in here???
			if (bNotesOK) {
				int NumPatternDurations = main.PatternDurations.Count;
				int Remainder = main.NumNotes % NumPatternDurations;
				if (Remainder != 0) {
					// TODO: Pad the input notes to a multiple of the pattern length
					main.Msg($"Warning: The number of notes ({main.NumNotes}) in '{main.InputFilename}' is not a multiple of the pattern length ({NumPatternDurations})");
				}
			} else {
				retval = false;
			}
#endif
			return retval;
		}

//---------------------------------------------------------------------------------------

		public bool Execute(string[] tokens) {
/*
	1)	Check to see that rhythm pattern is consistent with the time signature
	2)	Get length of rhythm pattern
		2a) Where, if anywhere, do tuplets come in?
	3)	Pad input notes to a multiple of the length of the rhythm pattern
	4)	Scale notes by <divisions>
	5)	Apply pattern to notes, including <dot/> if necessary
	6)	Delete old measures from .xml file and write out new ones.

*/
			// TODO: OK, at this point we have the durations in main.RhythmDurations.
			//		 However they're unscaled via <divisions>.
			var notes = main.xml.GetNotes();
			var enumerator = main.NextPatternDuration().GetEnumerator();
			int NoteNum = 0;

			foreach (XmlElement note in notes) {
				var dur = main.GetFirstElement(note, "duration");
				if (dur is null) {
					main.Msg("No duration found. Grace Note?");
					continue;
				}
				var type = main.GetFirstElement(note, "type");
				if (type is null) {
					main.Msg("No note type found. Grace Note?");
					continue;
				}
				string OriginalDuration = dur.InnerText;
				enumerator.MoveNext();
				var NewDurType = enumerator.Current;
				dur.InnerText = NewDurType.Duration.ToString();

				string OriginalType = type.InnerText;
				type.InnerText = NewDurType.Name;
				main.Msg($"Converted note[{++NoteNum}] duration/type from {OriginalDuration}/{OriginalType} to {dur.InnerText}/{type.InnerText}");
			}

#if true
			main.Msg();
			main.Msg("Nonce error on setting the time signature. I don't believe this " +
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
	}
}
