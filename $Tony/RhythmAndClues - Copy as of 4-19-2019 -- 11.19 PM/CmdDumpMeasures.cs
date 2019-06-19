using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace RhythmAndClues {
	class CmdDumpMeasures : ICommand {
		public bool CheckSyntax(Interpreter main, string[] tokens) {
			bool retval = true;
			if (!main.bLoadCommandFound) {	// TODO: Make into common routine
				main.SyntaxError($"Error: Can't {tokens[0]} because no LOAD was done");
				retval = false;
			}
			return retval;
		}

//---------------------------------------------------------------------------------------

		public bool Command(Interpreter main, string[] tokens) {
			foreach (XmlElement meas in main.xml.Measures) {
				// A bunch of messing around just to make things look pretty
				int MeasNumWidth = (int)(Math.Ceiling(Math.Log10(main.xml.Measures.Count)));
				MeasNumWidth += "Measure[]".Length + 4; // 4 -> subscripts up to 9999
				string num = meas.GetAttribute("number");
				main.Msg();
				string MeasureText = $"Measure[{num}]".PadRight(MeasNumWidth);
				int n = 0;
				string hdr = $"{MeasureText} Pitch  Duration     Type";
				main.Msg("".PadLeft(hdr.Length + 1, '='));
				main.Msg(hdr);
				main.Msg("               -----  --------  -------");
				foreach (XmlElement note in meas.GetElementsByTagName("note")) {
					string subN = $"  [{++n}]".PadRight(6);
					// A rest has no pitch or type
					var pitchElem = main.GetFirstElement(note, "pitch");
					var dur = main.GetFirstElement(note, "duration");
					string duration = (dur is null ? "N/A" : dur.InnerText).PadLeft(8);
					string pitch = " rest";       // Probably not, but just in case
					string type = "  rest";
					if (!(pitchElem is null)) {       // Check for a non-rest
						pitch = pitchElem.InnerText.PadLeft(5);
						type = main.GetFirstElement(note, "type").InnerText.PadLeft(6);
					}
					main.Msg($" {subN}        {pitch}  {duration}  {type}");
				}
			}
			return true;
		}
	}
}
