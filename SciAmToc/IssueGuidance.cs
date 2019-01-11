using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SciAmToc {
	public class IssueGuidance {
		public int						TocPageStartNumber;     // 0 for auto-find TOC
		public int                      TocPageEndNumber;
		public Dictionary<int, string>	TextReplacements;
		public HashSet<int>				Swaps;
		public Dictionary<int, string>  SplitPageNos;
		public HashSet<int>             Glues;
		public static Dictionary<(int Year, int Month), IssueGuidance> IssueData;
		public List<string>             ArticleNamesOfInterest;
		public List<string>             OcrResults;

		// Q-type helix, er info (with apologies to Doc Smith)
		public List<ArticleInfo>        QInfo;

		public static List<FormatInfo>  FormatInfos = new List<FormatInfo>();

		private static char[] blank = new char[] { ' ' };

//---------------------------------------------------------------------------------------

		public class FormatInfo {
			public int FormatNumber;		// Which format it is (0..n)
			public int StartYear;
			public int StartMonth;
			public int EndYear;
			public int EndMonth;

//---------------------------------------------------------------------------------------

		public FormatInfo(string[] Info) {
			FormatNumber = Convert.ToInt32(Info[1]);
			StartYear    = Convert.ToInt32(Info[2]);
			StartMonth   = Convert.ToInt32(Info[3]);
			EndYear      = Convert.ToInt32(Info[4]);
			EndMonth     = Convert.ToInt32(Info[5]);
		}
	}

//---------------------------------------------------------------------------------------

		public IssueGuidance() {
			TocPageStartNumber     = 0;
			TocPageEndNumber       = 0;
			TextReplacements       = new Dictionary<int, string>();
			ArticleNamesOfInterest = new List<string>();
			Swaps                  = new HashSet<int>();
			Glues                  = new HashSet<int>();
			SplitPageNos           = new Dictionary<int, string>();
			OcrResults             = new List<string>();
			QInfo                  = new List<ArticleInfo>();
		}

//---------------------------------------------------------------------------------------

		public static void ReadInfoFile(string Filename) {
			// Note: For now, we'll do little (if any) error checking. Wish us luck.
			IssueGuidance ish = null;
			if (IssueData == null) {
				IssueData = new Dictionary<(int Year, int Month), IssueGuidance>();
			} else {
				IssueData.Clear();
			}
			bool bIsInOcrText = false;
			using (var sr = new StreamReader(Filename)) {
				string line;
				int LineNo = 0;
				while ((line = sr.ReadLine()) != null) {
					++LineNo;
					if ((line = line.Trim()).Length == 0) continue;
					if (line == "$") {
						bIsInOcrText = false;
						continue;
					}
					if (bIsInOcrText) {
						ish.OcrResults.Add(line);
						continue;
					}
					switch (line[0]) {
						// TODO: Maybe introduce '/' for search-and-replace?
						case ';':		// Comment
							continue;
						case 'A':       // Article of interest
							ish.ArticleNamesOfInterest.Add(line.Substring(2).Trim().ToUpper());
							break;
						case 'I':       // Start of new issue
							var iym = line.Split(blank, StringSplitOptions.RemoveEmptyEntries);
							if (iym.Length != 3) {
								MessageBox.Show($"I command must have 3 values, line = {LineNo}", "Guidance");
								return;		// TODO: Should return false; i.e. don't continue
							}
							ish = new IssueGuidance();
							(int Year, int Month) YearMonth = (Convert.ToInt32(iym[1]), Convert.ToInt32(iym[2]));
							IssueData.Add(YearMonth, ish);
							break;
						case 'T':       // TOC page number (0 for auto-find)
							var StartEnd = line.Split(blank, StringSplitOptions.RemoveEmptyEntries);
							ish.TocPageStartNumber = Convert.ToInt32(StartEnd[1]);
							if (StartEnd.Length == 2) {
								ish.TocPageEndNumber = ish.TocPageStartNumber;
							} else {
								ish.TocPageEndNumber = Convert.ToInt32(StartEnd[2]);
							}
							break;
						case 'L':       // Fmt: L <linenum> <replacement text>
							Do_L_M(ish, line);
							break;
						case 'M':       // Same as L, but deletes (X's) the next line
							Do_L_M(ish, line, true);
							break;
						case 'X':       // 'Delete' (i.e. replace by "") line
							var xline = Convert.ToInt32(line.Substring(1));
							ish.TextReplacements.Add(xline, "");
							break;
						case 'W':       // sWap <Author>\n<Title> or vice versa
							var SwapLineNo = Convert.ToInt32(line.Substring(1));
							ish.Swaps.Add(SwapLineNo);
							break;
						case 'P':
							// For some reason, there are a *lot* of instances where the
							// page number at the end of an article description has been
							// split. For example, if the article started on page 70, the
							// 7 would be on one line and the 0 would be, by itself, on
							// the next line. What I used to do was to do an L for the
							// penultimate line, append the second digit and X the second
							// line. This tag will do both: P 43 0 (line #, extra digit)
							var pVals = line.Split(blank, StringSplitOptions.RemoveEmptyEntries);
							int pValLineNo = Convert.ToInt32(pVals[1]);
							ish.SplitPageNos.Add(pValLineNo, pVals[2]);
							break;
						case 'F':       // Fmt: L <linenum> <replacement text>
							var fmts = line.Split(blank, 6, StringSplitOptions.RemoveEmptyEntries);
							FormatInfos.Add(new FormatInfo(fmts));
							break;
						case 'G':       // Glue this line and the next together
										// (with ' ' separating them
							var gline = Convert.ToInt32(line.Substring(1));
							ish.Glues.Add(gline);
							break;
						case 'O':       // Multiline from OCR
							bIsInOcrText = true;
							ish.OcrResults.Add("ARTICLES");
							break;
						case 'Q':       // Like O, but start of 4 lines each
									// Line 1 = Page # -- e.g. "Q 30"
									// Line 2 = Article Name
									// Line 3 = Author(s) (or empty)
									// Line 4 = Description (or empty)
									// These 4 lines repeat until we get to $ terminator
							break;
						// tHE FOLLOWING ARE CURRENTLY UNDEFINED
						case 'B':
						case 'C':
						case 'D':
						case 'E':
						case 'H':	// The reverse of G; swap with =next
						case 'J':
						case 'K':
						case 'N':
						case 'R':
						case 'U':
						case 'V':
						case 'Y':
						case 'Z':
						default:
							System.Windows.Forms.MessageBox.Show($"Unknown command '{line[0]}' on line {LineNo}", "Guidance");
							// System.Diagnostics.Debugger.Break();    // TODO: Should return false
							break;
					}
				}
			}
		}

//---------------------------------------------------------------------------------------

		private static void Do_L_M(IssueGuidance ish, string line, bool bDeleteNextLine = false) {
			var dat = line.Split(blank, 3, StringSplitOptions.RemoveEmptyEntries);
			string replacement;
			if (dat.Length == 3) {
				replacement = dat[2];
			} else {
				replacement = "";
			}
			int LineNo = Convert.ToInt32(dat[1]);
			ish.TextReplacements.Add(LineNo, replacement);
			if (bDeleteNextLine) {
				ish.TextReplacements.Add(LineNo + 1, "");

			}
		}
	}
}
