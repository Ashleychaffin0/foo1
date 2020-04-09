using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MonitorClipboardChange {
	internal class VintageBase {
		static String[] Months = new string[] { "January", "February", "March", "April", "May",
				"June", "July", "August", "September", "October", "November", "December"};

		static char[] NewLines = new char[] { '\r', '\n' };

		internal List<string>       Lines;
		internal List<ArticleInfo>  Articles;
		internal List<ArticleInfo>  Departments;

//---------------------------------------------------------------------------------------

		public VintageBase(List<string> lines) {
			Lines       = lines;
			Articles    = new List<ArticleInfo>();
			Departments = new List<ArticleInfo>();
		}

		// See the actual pdf file, but starting in 1987-09, the format of the TOC
		// changed. Reading from left-to-right we had (a) the page #, (b) a
		// thumbnail of the article's first page, and (c) the Title / Author(s) /
		// description. Now normally the thumbnail doesn't show up in the OCR
		// results, but in 1987-09, there is text in the image that shows up in the
		// OCR results. Which we thus want to ignore. The .txt file (as opposed to
		// what shows up on the web page) is nicely aligned, with blanks where the
		// image normally is. So to skip over any // image-with-text, we'll make a
		// a quick preliminary pass to find out where the real text starts. We can
		// then ignore the spurious image text and do a proper parsing of the text.
		// Note also that for multi-page TOCs, the "margin" with the images can be
		// different from page-to-page. We detect a new page by a line that has a
		// formfeed (\f) character.

		internal List<int> GetMarginSizes(Regex re) {
			var result = new List<int>();
			int size = 0;
			for (int i = 0; i < Lines.Count; i++) {
				string line = Lines[i];
				if (line.Contains("DEPARTMENTS")) {
					result.Add(size);
					break;
				}
				// if (line.Length == 0) continue;
				if (line[0] == '\f') {
					result.Add(size);
					size = 0;
				}
				var m = re.Match(line);
				if (m.Success) size = Math.Max(size, m.Length);
			}
			return result;              // TODO:
		}

//---------------------------------------------------------------------------------------

		internal static VintageBase? VintageFactory(string s) {
			// Note: RemoveEmptyEntries guarantees we have no zero-length Lines. But
			//		 a line could still be composed entirely of whitespace, so be careful
			//		 if/when you line.Trim().
			var Lines = s.Split(NewLines, StringSplitOptions.RemoveEmptyEntries).ToList();
			string MonthsPat = string.Join("|", Months);
			var PatMonthYear = $@"({MonthsPat})\s+\d+";
			var re = new Regex(PatMonthYear);
			for (int i = 0; i < Lines.Count; i++) {
				var m = re.Match(Lines[i]);
				if (!m.Success) continue;
				var words = m.Value.Split(new string[] { " " },  2, StringSplitOptions.RemoveEmptyEntries);
				int Month = Array.IndexOf(Months, words[0]) + 1;
				int Year  = Convert.ToInt32(words[1]);
				return Factory(Year, Month, Lines);
			}
			return null;            // TODO:
		}

//---------------------------------------------------------------------------------------

		private static VintageBase Factory(int year, int month, List<string> lines) {
			if (DateCompare(year, month, 1989, 9) >= 0) {
				return new Vintage_1989_09(lines);
			}
			if (DateCompare(year, month, 1948, 5) >= 0) {
				return new Vintage_1948_05(lines);
			}
			// TODO: Put in rea test based on next issue the TOC format changes
			throw new ArgumentException($"Bad year/month passed to VintageBase.{nameof(Factory)}({year}/{month})");
		}

//---------------------------------------------------------------------------------------

		private static int DateCompare(int year, int month, int StartYear, int StartMonth) {
			if (year > StartYear) return 1;
			if ((year == StartYear) && (month == StartMonth)) return 0;
			if ((year == StartYear) && (month > StartMonth)) return 1;
			return -1;
		}

//---------------------------------------------------------------------------------------

		internal virtual void ParseToc() { }

//---------------------------------------------------------------------------------------

		internal virtual string GetData() { return "";  }
	}
}
