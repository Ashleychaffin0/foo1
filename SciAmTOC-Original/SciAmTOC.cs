// See https://www.codeproject.com/articles/686994/create-read-advance-pdf-report-using-itextsharp-in

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace SciAmTOC {
	public partial class SciAmTOC : Form {
		string SciAmBaseDir = @"G:\LRS-8500\$SciAm-Full Issues";    // \$SCIAM - 1970'S";
		string SciAmDecadeDir;
		StreamWriter    OutFile;
		string          OutFileName = @"G:\LRS\SciAmTOC.html";

//---------------------------------------------------------------------------------------

		public SciAmTOC() {
			InitializeComponent();

			InitialSetup();
		}

//---------------------------------------------------------------------------------------

		private void BtnGo_Click(object sender, EventArgs e) {
			ProcessIssues();
			OutFile.WriteLine(@"
</BODY>
</HTML>");
			OutFile.Close();
			System.Diagnostics.Process.Start(OutFileName);
		}

//---------------------------------------------------------------------------------------

		private void ProcessIssues() {
			var First = Convert.ToInt32(CmbFirstDecade.SelectedItem);
			int Last  = Convert.ToInt32(CmbLastDecade.SelectedItem);
			for (int Year = First; Year <= Last + 9; Year++) {
				// if (Year == 1950) System.Diagnostics.Debugger.Break();
				if (Year == 1951) break;
				int Decade = (Year / 10) * 10;
				SciAmDecadeDir = Path.Combine(SciAmBaseDir, $"$SCIAM - {Decade}'S");
				ProcessYear(Year);
			}
			// var dir = @"G:\LRS-8500\$SciAm-Full Issues\$SCIAM - 1970'S\Sciam - 1975";
			// string filename = "Sciam - 1975-12-December - X Rays From a Supernova Remnant - Copy.pdf";

			// string InFile = Path.Combine(dir, filename);
		}

//---------------------------------------------------------------------------------------

		private void ProcessYear(int FilenameYear) {
			string dir = Path.Combine(SciAmDecadeDir, $"Sciam - {FilenameYear}");
			var Months = Directory.GetFiles(dir, "*.pdf");
			foreach (var year in Months) {
				Check4Toc(year);
			}
		}

//---------------------------------------------------------------------------------------

		private void Check4Toc(string year) {
			bool bExists = File.Exists(year);
			if (!File.Exists(year)) return;
			bool bFoundToc = false;
			using (var rdr = new PdfReader(year)) {
				int ix = year.LastIndexOf('/');
				// string IssueTitle = year.Substring(ix + 1);
				string IssueTitle = Path.GetFileNameWithoutExtension(year);
				OutFile.WriteLine($"<hr><p/><issue>{IssueTitle}</issue><p/>");
				Console.WriteLine($"Processing {IssueTitle}");
				using (var pdoc    = new PdfDocument(rdr)) {
					var nPages     = pdoc.GetNumberOfPages();
					// long len    = rdr.GetFileLength();
					for (int n     = 1; n <= nPages; n++) {
						var page   = pdoc.GetPage(n);
						var text   = PdfTextExtractor.GetTextFromPage(page);
						var lines  = text.Split(new char[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
						if (IsTocPage(lines)) {
							bFoundToc = ProcessTOC(n, lines);
						}
					}
				}
			}
			if (bFoundToc) return;
			// MessageBox.Show($"TOC not found for {year}", "SciAmToc");
		}

//---------------------------------------------------------------------------------------

		private  bool ProcessTOC(int PageNo, string[] Lines) {
			bool bFoundToc;
			// Console.WriteLine($"\nPage = {PageNo}");
			bFoundToc = true;
			for (int i = 0; i < Lines.Count(); i++) {
				string line = Lines[i];
				// Console.WriteLine($"\t{i}: {line}");
				if (line == "ARTICLES ") {
					ProcessArticles(i + 1, Lines);
				}
			}

			return bFoundToc;
		}

//---------------------------------------------------------------------------------------

		private void ProcessArticles(int i, string[] lines) {
			for (int n = i; n < lines.Count(); n++) {
				string line = lines[n];
				string ArticleTitle = GetPutativeTitle(line);
				if (IsAllUpCase(ArticleTitle)) {
					var desc = GatherDescription(n + 1, lines);
					if (desc.Count() == 0) break;
					OutFile.WriteLine($"<article>{ArticleTitle}</article>");
					string desc2 = string.Join(" ", desc);
					OutFile.WriteLine($"<desc>{desc2}</desc><p/>");
					n += desc.Count();
				}
			}
		}

//---------------------------------------------------------------------------------------

		private static string GetPutativeTitle(string line) {
			int ixBy = line.IndexOf(" by ");
			int ixHy = line.IndexOf(" hy ");	// Common OCR error
			string title;
			if ((ixBy < 0) && (ixHy < 0)) {
				title = line;
			} else {
				title = line.Substring(0, Math.Max(ixBy, ixHy));
			}
			return title;
		}

//---------------------------------------------------------------------------------------

		private static List<string> GatherDescription(int n, string[] lines) {
			var desc = new List<string>();
			for (int i = n; i < lines.Count(); i++) {
				string line  = lines[i];
				string title = GetPutativeTitle(line);
				if (IsAllUpCase(title)) return desc;
				desc.Add(line);
			}
			return desc;
		}

//---------------------------------------------------------------------------------------

		private static bool IsAllUpCase(string title) {
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
			if (nUpper == 0) return false;		// Not a single upcase letter
			if (nLower == 0) return true;       // Since nUpper > 0
			if ((nUpper / (nUpper + nLower)) < 0.9f) return false;	// > 90%
			return true;
		}

//---------------------------------------------------------------------------------------

		private bool IsTocPage(string[] lines) {
			int nMatches = 0;
			for (int i = 0; i < lines.Count(); i++) {
				string line = lines[i];
				if (line == "ARTICLES ") ++nMatches;
				if (line.StartsWith("Established ")) ++nMatches;
			}
			return nMatches == 2 ? true : false;
		}

//---------------------------------------------------------------------------------------

		private void InitialSetup() {
			for (int decade = 1840; decade <= 2020; decade += 10) {
				CmbFirstDecade.Items.Add(decade);
				CmbLastDecade.Items.Add(decade);
			}
			int ixStart = (1950 - 1840) / 10;
			CmbFirstDecade.SelectedIndex = ixStart;
			CmbLastDecade.SelectedIndex = (2020 - 1840) / 10;

			OutFile = new StreamWriter(OutFileName);    // TODO: .PDF?
			OutFile.WriteLine(@"
<HTML>
<HEAD>
	<TITLE>Scientific American Table of Contents</TITLE>
	<Style>
		issue {
			color: blue;
			text-align: center;
			font-size: 40px;
		}

		article {
			font-weight: bold;
			background-color: oldlace;
			border: 2px solid green;
		}

		desc {
		}
	</Style>
</HEAD>
<BODY>
");
		}
	}
}
