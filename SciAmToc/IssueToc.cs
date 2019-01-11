using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace SciAmToc {
	public class IssueToc {
		public SciAmIssue       Issue;
		public int              IssueYear;
		public int              IssueMonth;
		public int              FirstPage;      // Of TOC
		public int              LastPage;       // Of TOC
		public List<string>     Lines;			// TODO: Don't need this; it's in IssueToc

		static char[]   CrLf = new char[] {'\r', '\n'};

//---------------------------------------------------------------------------------------

		public IssueToc(SciAmIssue Issue,
				int IssueYear,
				int IssueMonth) {
			this.Issue      = Issue;
			Issue.Toc       = this;
			this.IssueYear  = IssueYear;
			this.IssueMonth = IssueMonth;
			Lines           = new List<string>();
		}

//---------------------------------------------------------------------------------------

		public static IssueToc Factory(SciAmIssue Issue, int IssueYear, int IssueMonth) {
			foreach (var fi in IssueGuidance.FormatInfos) {
				// See if the specified year/month is inside the current range
				if (IssueYear < fi.StartYear) continue;
				if (IssueYear > fi.EndYear) continue;
				// OK, the year's in the right year range, but we've got to take the 
				// month into consideration.
				if ((IssueYear == fi.StartYear) && (IssueMonth < fi.StartMonth)) continue;
				if ((IssueYear == fi.EndYear) && (IssueMonth > fi.EndMonth)) continue;
				// OK, it's in the range!
				int fmt = fi.FormatNumber;
				switch (fmt) {
					case 1:             // 1948-05 to 1959-12
						return new IssueToc_1(Issue, IssueYear, IssueMonth);
					case 2:             // 1956-01 to 1996-03
						return new IssueToc_2(Issue, IssueYear, IssueMonth);
					case 3:             // 1996-04 to 2020-12
						return new IssueToc_3(Issue, IssueYear, IssueMonth);
					default:
						return null;
				}
			}
			return null;
		}

//---------------------------------------------------------------------------------------

		internal virtual void ParseLines() {
		}

//---------------------------------------------------------------------------------------

		public List<string> GetIssueTocPages(ref IssueGuidance Guide) {
			// TODO: Make void; Lines is part of the class
			if (!File.Exists(Issue.FullPath)) return Lines; // e.g. issue not yet downloaded
			IssueGuidance.IssueData.TryGetValue((IssueYear, IssueMonth), out Guide);
			if ((Guide != null) && Guide.TocPageStartNumber > 0) {
				FirstPage = Guide.TocPageStartNumber;
				LastPage  = Guide.TocPageEndNumber;
				Lines	  = GetPageText(FirstPage, LastPage);
				FixupLines(ref Lines, IssueYear, IssueMonth);

				return Lines;
			}

			// TODO: Extract this method, call it ScanForToc or something
			try {
				// using (var pdoc = new PdfDocument(rdr)) {
				var Gotit = AutoFindToc();
				if (Gotit.StartPage > 0) {
					return GetPageText(Gotit.StartPage, Gotit.EndPage);
				} else {
					Issue.OutFile.WriteLine($"<error>TOC not found for {IssueYear   }/{IssueMonth:d2}</error><br/>");
					DumpIssueLines();
				}

				// }
			} catch (Exception ex) {
				if (!SciAmToc.Main.ChkFirstIssueOnly.Checked) {
					Issue.OutFile.WriteLine($"<error>Exception processing issue {IssueYear}/{IssueMonth} -- {ex.Message}</error><br/>");
				}
			}
			// };
			return Lines;       // Empty if we couldn't find the TOC
		}

//---------------------------------------------------------------------------------------

		private List<string> GetPageText(int FirstPage, int LastPage) {
			var AllLines = new List<string>();
			for (int i = FirstPage; i <= LastPage; i++) {
				AllLines.Add($"<error>Page {i}</error><br/>");
				var page = Issue.pDoc.GetPage(i);
				var text = PdfTextExtractor.GetTextFromPage(page);
				var lines = text.Split(CrLf, StringSplitOptions.RemoveEmptyEntries);
				AllLines.AddRange(lines);
			}
			return AllLines;
		}

//---------------------------------------------------------------------------------------

		private (int StartPage, int EndPage) AutoFindToc() {
			// TODO: Perhaps return lines[] or null instead of bool?
			int nPages = Issue.pDoc.GetNumberOfPages();
			for (int n = 1; n <= nPages; n++) {
				var page = Issue.pDoc.GetPage(n);
				try {
					var text = PdfTextExtractor.GetTextFromPage(page);
					var lines = text.Split(CrLf, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
					// TODO: Note that calling FixupLines here is rather indiscriminate.
					//		 We're looking for the TOC page, but the fixups will be
					//		 applied to every page! But with that said, what are the odds
					//		 that we'll get a false positive for the TOC?
					FixupLines(ref lines, IssueYear, IssueMonth);
					if (IsTocPage(lines, IssueYear, IssueMonth)) {
						return (n, n);          // TODO: Later support multi-page TOCs
					}
				} catch (Exception ex) {
					Issue.OutFile.WriteLine($"<b>Error extracting text on {IssueYear}/{IssueMonth}={n} -- {ex.Message}");
				}
			}
			return (-1, -1);
		}

//---------------------------------------------------------------------------------------

		private void DumpIssueLines() {
			// TODO: This no longer emits <error>Page Number</error>
			for (int i = 1; i <= Issue.pDoc.GetNumberOfPages(); i++) {
				// foreach (var line in GetPageText(pdoc, 1, pdoc.GetNumberOfPages())) {
				var Lines = GetPageText(i, i);
				foreach (var line in Lines) {
					Issue.OutFile.WriteLine($"{++i}: {line}<br/>");
				}
			}
		}

//---------------------------------------------------------------------------------------

		private bool IsTocPage(List<string> lines, int Year, int Month) {
			int nMatches = 0;
			for (int i = 0; i < lines.Count(); i++) {
				string line = lines[i].Trim();
				if (line == "ARTICLES") ++nMatches;
				if (line.StartsWith("Established ")) ++nMatches;
				string DateSignature = $"{SciAmToc.Months[Month - 1]}, {Year}";
				if (line.Contains(DateSignature) && line.Contains(" Volume ")) {
					nMatches = 2;
					break;
				}
			}
			return nMatches >= 2 ? true : false; ;
		}

//---------------------------------------------------------------------------------------

		private void FixupLines(ref List<string> lines, int Year, int Month) {
			// TODO: Pass in Guide
			bool bOK = IssueGuidance.IssueData.TryGetValue((Year, Month), out IssueGuidance IshData);
			if (!bOK) return;           // No fixup for that issue

			if (IshData.OcrResults.Count > 0) {
				lines = IshData.OcrResults;
				return;
			}
										
			FixUpLines_Swaps(lines, IshData);	// Do any swaps
			FixupLines_PageNos(lines, IshData);
			FixupLines_Glues(lines, IshData);
			foreach (var item in IshData.TextReplacements) {
				lines[item.Key] = item.Value;
			}
		}

//---------------------------------------------------------------------------------------

		private static void FixUpLines_Swaps(List<string> lines, IssueGuidance IshData) {
			foreach (var SwapLineNo in IshData.Swaps) {
				var swapline1 = lines[SwapLineNo];
				var swapline2 = lines[SwapLineNo + 1];
				if (SciAmToc.IsAllUpCase(swapline1)) {
					// Title followed by Author
					IshData.TextReplacements.Add(SwapLineNo, swapline1 + " " + swapline2);
				} else {
					// Author followed by Title
					IshData.TextReplacements.Add(SwapLineNo, swapline2 + " " + swapline1);
				}
				IshData.TextReplacements.Add(SwapLineNo + 1, "");
			}
		}

//---------------------------------------------------------------------------------------

		private void FixupLines_PageNos(List<string> lines, IssueGuidance ishData) {
			foreach (var PageFixup in ishData.SplitPageNos) {
				int lineNo    = PageFixup.Key;
				string orphan = PageFixup.Value.Trim();
				ishData.TextReplacements.Add(lineNo, lines[lineNo].Trim() + orphan);
				ishData.TextReplacements.Add(lineNo + 1, "");
			}
		}

//---------------------------------------------------------------------------------------

		// Glue together one line with the next (with ' ' between them
		private void FixupLines_Glues(List<string> lines, IssueGuidance ishData) {
			foreach (var GlueLineNo in ishData.Glues) {
				var line1 = lines[GlueLineNo];
				var line2 = lines[GlueLineNo + 1];
				var line = line1 + " " + line2;
				ishData.TextReplacements.Add(GlueLineNo, line);
				ishData.TextReplacements.Add(GlueLineNo + 1, "");
			}
		}

//---------------------------------------------------------------------------------------

		public void ProcessToc(IssueGuidance Guide) {
			// TODO: For now assume just 1 TOC page. Later support more
			ParseLines();
			Issue.FormatHtmlOutput();
			if (SciAmToc.Main.ChkRawLines.Checked) {
				for (int i = 0; i < Lines.Count(); i++) {
					string line = Lines[i].Trim();
					Issue.OutFile.WriteLine($"{i}: {line}<br/>");
				}
			}
		}
	}
}
