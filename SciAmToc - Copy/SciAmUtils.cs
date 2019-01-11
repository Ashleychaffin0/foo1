using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SciAmToc {
	public partial class SciAmToc {

//---------------------------------------------------------------------------------------

		internal static bool IsAllUpCase(string title) {
			// 1950-03 has a title of "THE HYl)ROGEN BOMB". Note the 'l'.
			// To guard against false negatives, we'll get the ratio of the number
			// of upper-case to lower-case letters. If it's large enough, we'll
			// assume an OCR error and report that the string indeed is all upcase
			float nLower = 0;
			float nUpper = 0;
			foreach (char c in title) {
				if (!char.IsLetter(c)) continue;        // e.g. ' '
				if (char.IsUpper(c)) {
					++nUpper;
				} else {
					++nLower;
				}
			}
			if (nUpper == 0) return false;      // Not a single upcase letter
			if (nLower == 0) return true;       // Since nUpper > 0
			if ((nUpper / (nUpper + nLower)) < 0.9f) return false;  // > 90%
			return true;
		}

//---------------------------------------------------------------------------------------

		internal static string GetIssueTitle(int Year, int Month) {
			// TODO: Use SciAmToc's GetFullIssuePath
			int Decade = Year / 10 * 10;
			var dir    = Path.Combine(SciAmBaseDir, $"$SCIAM - {Decade}'S");
			dir = Path.Combine(dir, $"Sciam - {Year}");
			if (!Directory.Exists(dir)) return null;

			var pattern = $"Sciam - {Year}-{Month:d2}*.pdf";
			var fn = Directory.GetFiles(dir, pattern).FirstOrDefault();
			if (fn == null) return null;
			return Path.GetFileNameWithoutExtension(fn);
		}

//---------------------------------------------------------------------------------------

		internal static string GetPutativeTitle(string line, out string Authors) {
			int ixHy = line.IndexOf(" hy ");    // Common OCR error
			if (ixHy >= 0) line = line.Substring(0, ixHy) + " by " + line.Substring(ixHy + 4);
			int ixBy = line.IndexOf(" by ");
			string title;
			if (ixBy < 0) {
				title   = line;
				Authors = "(No authors found)";
			} else {
				title   = line.Substring(0, ixBy);
				if (title.EndsWith(",")) title = title.Substring(0, title.Length - 1);
				Authors = line.Substring(title.Length);
				if (Authors.StartsWith(" by ")) {
					Authors = Authors.Substring(4);
				} else if (Authors.StartsWith(", by ")) {
					Authors = Authors.Substring(5);
				}
			}
			return title;
		}

//---------------------------------------------------------------------------------------

		internal static List<string> GatherDescription(int n, List<string> lines) {
			var desc = new List<string>();
			bool bPreviousLineWasHyphenated = false;
			for (int i = n; i < lines.Count(); i++) {
				string line = lines[i].Trim();
				if (line.Length == 0) continue;
				string title = GetPutativeTitle(line, out string Authors);
				if (IsAllUpCase(title)) return desc;
				if (bPreviousLineWasHyphenated) {
					string PreviousLine = desc[desc.Count - 1];
					string ReplacementLine = PreviousLine.Substring(0, PreviousLine.Length - 1);    // Strip trailing hyphen
					bPreviousLineWasHyphenated = line.EndsWith("-");
					desc[desc.Count - 1] = ReplacementLine + line;
				} else {
					bPreviousLineWasHyphenated = line.EndsWith("-");
					desc.Add(line);
				}
			}
			return desc;
		}

	}
}
