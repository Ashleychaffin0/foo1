using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace RhythmAndClues {
	class CmdApplyRhythm : ICommand {
		public bool CheckSyntax(Interpreter main, string[] tokens) {
			bool retval = true;
			if (!main.bLoadCommandFound) {
				main.SyntaxError($"Error: Can't {tokens[0]} because no valid LOAD was found");
				retval = false;
			}
			bool bNotesOK = main.ParsePatternNotes(tokens);	// TODO: Put this rtn in here
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
			return retval;
		}

//---------------------------------------------------------------------------------------

		public bool Command(Interpreter main, string[] tokens) {
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
