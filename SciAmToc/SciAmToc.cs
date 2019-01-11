// See https://www.codeproject.com/articles/686994/create-read-advance-pdf-report-using-itextsharp-in
// See https://www.codeproject.com/articles/12445/converting-pdf-to-text-in-c
// See https://www.codeproject.com/KB/cpp/ExtractPDFText/ExtractPDFText_src.zip

// To extract images, see https://stackoverflow.com/questions/802269/extract-images-using-itextsharp

// What seems to be iText doc'n -- https://developers.itextpdf.com/content/itext-7-jump-start-tutorial/chapter-6-reusing-existing-pdf-documents

// See http://hintdesk.com/c-itextsharp-pdf-file-insertextract-imagetext-and-auto-fillin/comment-page-1/

// TODO: Handle DEPARTMENTS. In the 1950's, we sometimes (but not always) have an
//		 empty entry for this, for example October 1950

// TODO: Massive update to Guidance file -- article titles first, please
// TODO: Use the parsing routine as the basis for everything, including HTML generation
// TODO: Do something with pre-1948-05 TOCs???
// TODO: Parse Authors into List<string>
// TODO: When the dust settles, put this info into a database.
// TODO: Detect when IssueInfo.txt is changed, and do automatic refresh

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;


namespace SciAmToc {
	public partial class SciAmToc : Form {
		public const string SciAmBaseDir = @"G:\LRS-8500\$SciAm-Full Issues";    // \$SCIAM - 1970'S";
		public string       SciAmDecadeDir;

		StreamWriter    OutFile;
		const string    OutFileName = @"G:\LRS\SciAmToc.html";

		string GuideanceFilename = "IssueInfo.txt";

		public static string[] Months = new string [] { "January", "February", "March",
			 "April", "May","June", "July", "August", "September", "October",
				"November", "December" };


		public List<SciAmIssue> Issues;

		public static SciAmToc  Main;

		FileSystemWatcher fsw;

//---------------------------------------------------------------------------------------

		public SciAmToc() {
			InitializeComponent();

			Main = this;

			InitialSetup();

			SetupFilewatcher();
		}

//---------------------------------------------------------------------------------------

		private void SetupFilewatcher() {
			var path = Path.GetDirectoryName(Application.ExecutablePath);
			if (path.EndsWith("\\Debug")) path += "\\..\\..";
			fsw = new FileSystemWatcher(path, GuideanceFilename);
			fsw.IncludeSubdirectories = false;
			fsw.Renamed += Fsw_Renamed; // See comments in this routine
			fsw.NotifyFilter = NotifyFilters.FileName;
			fsw.EnableRaisingEvents = true;
		}

//---------------------------------------------------------------------------------------

		private void Fsw_Renamed(object sender, RenamedEventArgs e) {
			// 1) Rename from IssueInfo.txt to IssueInfo.txt~RF2509a813.TMP
			// 2) Renmae from l13vne1j.1va~ to IssueInfo.txt
			// Dunno why I'm not seeing other fileops. Test code (that I've since 
			// deleted) hasn't shown any.
			// And I don't know why this is being invoked, rather than the Changed event.
			// Maybe File History has something to do with it???
			if (e.Name == GuideanceFilename) {
				Thread.Sleep(200);      // Give Rename a chance to finish
				this.Invoke(new Action(DoGo));
			}
		}

//---------------------------------------------------------------------------------------

		private void BtnGo_Click(object sender, EventArgs e) {
			DoGo();
		}

//---------------------------------------------------------------------------------------

		private void DoGo() {
			// LRS_PdfUtils.Test();
			BtnReloadGuidance_Click(null, EventArgs.Empty);    // TODO: Delete this later?
			if (!ChkFirstIssueOnly.Checked) {
				SetupOutputFile();
			}

			Issues = new List<SciAmIssue>();

			int EndYear, EndMonthIndex;
			if (ChkFirstIssueOnly.Checked) {
				EndYear = (int)CmbFirstYear.SelectedItem;
				EndMonthIndex = CmbFirstMonth.SelectedIndex + 1;
			} else {
				EndYear = (int)CmbLastYear.SelectedItem;
				EndMonthIndex = CmbLastMonth.SelectedIndex + 1;
			}
			var dr = new DateRange(
				(int)CmbFirstYear.SelectedItem,
				CmbFirstMonth.SelectedIndex + 1,
				EndYear,
				EndMonthIndex);

#if false
			var sb = new StringBuilder();
			foreach (var item in dr.Dates()) {
				sb.Append($"\r\nI {item.Year} {item.Month}\r\nT 0\r\n");
			}
			Clipboard.SetText(sb.ToString());
#endif
			// CountTensors(dr);
			ProcessIssues(dr);
			if (!ChkFirstIssueOnly.Checked) {
				OutFile.WriteLine(@"
</BODY>
</HTML>");
				OutFile.Close();
				System.Diagnostics.Process.Start(OutFileName);
			}
		}

//---------------------------------------------------------------------------------------

		private void BtnNext_Click(object sender, EventArgs e) {
			// TODO: Handle Next Year and Next Decade
			int ix = CmbFirstMonth.SelectedIndex + 1;
			if (ix == 12) {
				ix = 1;
				if (CmbFirstYear.SelectedIndex == 9) {
					++CmbFirstDecade.SelectedIndex;
				} else {
					++CmbFirstYear.SelectedIndex;
				}
			} else {
				CmbFirstMonth.SelectedIndex = ix;
			}
			DoGo();
		}

//---------------------------------------------------------------------------------------

		private void ProcessIssues(DateRange Range) {
			foreach (var item in Range.Dates()) {
				// For some reason, August 1983 is missing. Sigh.
				if ((item.Year == 1983) && (item.Month == 8)) continue;
				int Decade = (item.Year / 10) * 10;
				SciAmDecadeDir = Path.Combine(SciAmBaseDir, $"$SCIAM - {Decade}'S");

				if (ChkFirstIssueOnly.Checked) {
					var ish = ProcessIssue_2(item.Year, item.Month);
					IssueToGrid(ish);
				} else {
					ProcessIssue(item.Year, item.Month);
				}
			}
		}

		public (int Year, int Month) GetCurYearMonth() {
			int Year = Convert.ToInt32(CmbFirstYear.Text);
			int Month = Convert.ToInt32(CmbFirstMonth.SelectedIndex + 1);
			return (Year, Month);
		}

//---------------------------------------------------------------------------------------

		private void IssueToGrid(SciAmIssue ish) {
			LvArticles.Items.Clear();
			foreach (var art in ish.Articles) {
				Color FgColor = Color.Black;
				Color BgColor = Color.White;
				if (art.IsInteresting) {
					FgColor = Color.Black;
					BgColor = Color.DarkOrange;
				}
				var lvItem = new ListViewItem(new string[] {art.Title }, -1, FgColor, BgColor, LblProcessing.Font);
				lvItem.Tag = art;
				lvItem.SubItems.Add(art.StartPageNumber.ToString());
				lvItem.SubItems.Add(art.Authors);
				lvItem.SubItems.Add(art.Description);
				lvItem.Tag = art;
				lvItem.ToolTipText = art.Description;
				LvArticles.Items.Add(lvItem);
			}

			Application.DoEvents();
			if (LvArticles.Items.Count > 0) {
				// TODO: This doesn't quite work right
				var Item0 = LvArticles.Items[0];
				Item0.Selected = true;
				Item0.Focused = true;
			}
		}

//---------------------------------------------------------------------------------------

		private SciAmIssue ProcessIssue_2(int IssueYear, int IssueMonth) {
			// TODO: These two routines should be coalesced
			var Issue = new SciAmIssue(IssueYear, IssueMonth, OutFile);
			if (Issue.FileNotFound) return null;
			LblProcessing.Text = Issue.Title;
			var toc = IssueToc.Factory(Issue, IssueYear, IssueMonth);
			// For now support only those issues with an explicit TOC page(s)
			// Same down to here



			IssueGuidance.IssueData.TryGetValue((IssueYear, IssueMonth), out IssueGuidance Guide);
			if (Guide == null) {
				return null;
			}
			Issue.Guide = Guide;
			var TocLines = toc.GetIssueTocPages(ref Guide);
			toc.ParseLines();


			// Same from here down
			Application.DoEvents();
			return Issue;
		}

//---------------------------------------------------------------------------------------

		private SciAmIssue ProcessIssue(int IssueYear, int IssueMonth) {
			// TODO: These two routines should be coalesced
			var Issue = new SciAmIssue(IssueYear, IssueMonth, OutFile);
			if (Issue.FileNotFound) return null;
			LblProcessing.Text = Issue.Title; ;
			var toc = IssueToc.Factory(Issue, IssueYear, IssueMonth);
			// Same down to here


			IssueGuidance Guide = null;
			toc.GetIssueTocPages(ref Guide);
			if (toc.Lines.Count == 0) return null;    // Error message already logged
			Issue.Guide = Guide;
			toc.ProcessToc(Guide);

			// Same from here down
			Application.DoEvents();
			return Issue;
		}

//---------------------------------------------------------------------------------------

		private void InitialSetup() {
			const int LastDecade = 2010;
			for (int decade = 1840; decade <= LastDecade; decade += 10) {
				CmbFirstDecade.Items.Add(decade);
				CmbLastDecade.Items.Add(decade);
			}
			int ixStart = (1950 - 1840) / 10;
			CmbFirstDecade.SelectedIndex = ixStart;
			CmbLastDecade.SelectedIndex = (LastDecade - 1840) / 10;

			// TODO: Uncomment this out later
			// IssueGuidance.ReadInfoFile("IssueInfo.txt");
		}

//---------------------------------------------------------------------------------------

		private void SetupOutputFile() {
			OutFile = new StreamWriter(OutFileName);    // TODO: .PDF?
			OutFile.WriteLine(@"
<HTML>
<HEAD>
	<TITLE>Scientific American Table of Contents</TITLE>
	<Style>
		.interesting {
			background-color: lime;
		}

		issue {
			color: blue;
			text-align: center;
			font-size: 24px;
			border: 2px solid blue;
		}

		article {
			font-weight: bold;
			font-size: 16px;
			display: inline;
		}

		author {
			background-color: yellow;
			/* display: inline; */
		}

		desc {
		}

		none {
		}

		disclaim {
			color: red;
		}

		error {
			color: red;
			font-weight: bold;
			font-size: 60px;
		}

		notused {
			/* border: 2px solid blue; */
			/* color: red; */
			/* background-color: oldlace; */
		}
	</Style>
</HEAD>
<BODY>
<disclaim>
DISCLAIMER: As far as I can tell, when Scientific American created the PDFs of older
(i.e. before issues were created on some sort of word processor) issues, they scanned
the pages and ran them through an Optical Character Recognition (OCR) program. The good
news is that OCR has (with the help of Machine Learning) improved significantly over the
last few years. The bad news is that the issues were scanned before all the improvements
and there are many, many, many misrecognitions, not only in an issue, but in individual
lines. For example, from September 1962, a line that displays as &quot;60 THE ANTARCTIC. by
A. P. Crary&quot; is, internally, &quot;TilE A,\T.\BCTIC. b� A. P. CI'aI'Y&quot (with the
page number of the article (60) on the next line!).
<br/><br/>
I admit I don't understand the internals of the PDF format well enough to say why
it displays so well, but internally it can be an absolute mess. But the mess is what this
program has to deal with, and I've invested far more time than I expected trying to clean
up the text. So I'm hoping I've got most of the Table of Contents pages (mostly) right.
Yeah, the Descriptions of the articles may well be off. I've spent enough time as it is.
But hopefully you get the idea.
</disclaim>
");
		}

//---------------------------------------------------------------------------------------

		private void BtnReloadGuidance_Click(object sender, EventArgs e) {
			if (IssueGuidance.IssueData != null) {
				IssueGuidance.IssueData = null;
			}
			// TODO: Add check for application.executablepath
			File.Copy(@"..\..\IssueInfo.txt", "IssueInfo.txt", true);
			IssueGuidance.ReadInfoFile(GuideanceFilename);
		}

//---------------------------------------------------------------------------------------

		private void LvArticles_DoubleClick(object sender, EventArgs e) {
			var ixs = LvArticles.SelectedIndices;
			if (ixs.Count == 0) return;     // Display clicked before Go
			int ix = ixs[0];
			var art = LvArticles.Items[ix].Tag as SciAmArticle;
			var ish = art.Issue;
			var fn = Path.GetTempPath();
			string path = Path.GetDirectoryName(ish.FullPath);
			fn = Path.Combine(fn, path);
			fn = Path.Combine(fn, "Articles");
			Directory.CreateDirectory(fn);
			fn = Path.Combine(fn, $"{art.Title}.pdf");

			// TODO: OK, we've just jumped through a few hoops to get the file name of
			//		 the article in a nice subdirectory. But until we've got this
			//		 program more debugged, we're going to always put the file into the
			//		 temp directory. We'll generate a new filename each time, just in
			//		 case we want to look at more than one article at once. These will
			//		 be deleted when we empty the TEMP directory
			fn = Path.GetTempFileName() + ".pdf";
			// TODO: But for now (just before going to Philly), we want to put selected
			//		 articles out for safekeeping
#if true
			if (art.IsInteresting) {
				fn = $@"G:\LRS\SciAmInteresting\{ish.Title} -- {art.Title}.pdf";
			}
#endif

			int ofs = 2;                        // Adjust for Front and Inside cover
			int FirstPage = art.StartPageNumber + ofs;
			int LastPage = FindLastPage(ish, art.StartPageNumber) + ofs - 1;
			// Because of <ofs>, this may be beyond the last page. Adjust for that
			LastPage = Math.Min(LastPage, ish.NumberOfPages);
			// TODO: Add option for cover page
			LRS_PdfUtils.CreateArticle(ish.FullPath, fn, FirstPage, LastPage, false);
		}

//---------------------------------------------------------------------------------------

		// Find the smallest page number greater than FirstPage
		private int FindLastPage(SciAmIssue ish, int FirstPage) {
			int LastPage = ish.NumberOfPages;
			foreach (ListViewItem item in LvArticles.Items) {
				var art = item.Tag as SciAmArticle;
				if (art.StartPageNumber <= FirstPage) continue;
				LastPage = Math.Min(LastPage, art.StartPageNumber);
			}
			return LastPage;
		}

//---------------------------------------------------------------------------------------

		private void CmbFirstYear_SelectedIndexChanged(object sender, EventArgs e) {
			if (CmbFirstMonth.Items.Count > 0) {
				CmbFirstMonth.SelectedIndex = 0;
			}
		}

//---------------------------------------------------------------------------------------

		private void BtnMakeLastFirst_Click(object sender, EventArgs e) {
			CmbLastDecade.SelectedIndex = CmbFirstDecade.SelectedIndex;
			CmbLastYear.SelectedIndex = CmbFirstYear.SelectedIndex;
			CmbLastMonth.SelectedIndex = CmbFirstMonth.SelectedIndex;
		}

//---------------------------------------------------------------------------------------

		private void BtnShowToc_Click(object sender, EventArgs e) {
			int Year = Convert.ToInt32(CmbFirstYear.SelectedItem);
			int Month = Convert.ToInt32(CmbFirstMonth.SelectedIndex + 1);
			string fname = GetFullIssuePath(Year, Month);
			Process.Start(fname);
		}

//---------------------------------------------------------------------------------------

		private string GetFullIssuePath(int Year, int Month) {
			int Decade = Year / 10 * 10;
			var dir    = Path.Combine(SciAmBaseDir, $"$SCIAM - {Decade}'S");
			dir = Path.Combine(dir, $"Sciam - {Year}");
			if (!Directory.Exists(dir)) return null;

			var pattern = $"Sciam - {Year}-{Month:d2}*.pdf";
			var fn = Directory.GetFiles(dir, pattern).FirstOrDefault();
			return fn;
		}

//---------------------------------------------------------------------------------------

		private void TxtDescription_KeyPress(object sender, KeyPressEventArgs e) {
			e.Handled = true;
		}

//---------------------------------------------------------------------------------------

		private void LvArticles_Click(object sender, EventArgs e) {
			ShowDescription();
		}

//---------------------------------------------------------------------------------------

		private void LvArticles_SelectedIndexChanged(object sender, EventArgs e) {
			ShowDescription();
		}

//---------------------------------------------------------------------------------------

		private void ShowDescription() {
			var items = LvArticles.SelectedItems;
			if (items.Count == 0) return;
			SciAmArticle art = (items[0].Tag) as SciAmArticle;
			TxtDescription.Text = art.Description;
		}

//---------------------------------------------------------------------------------------

		private void BtnExtractToc_Click(object sender, EventArgs e) {
			(int Year, int Month) = GetCurYearMonth();
			var Infile = GetFullIssuePath(Year, Month); // TODO:
			var path   = Path.GetTempPath();
			var fn     = Path.GetTempFileName() + ".pdf";
			fn = Path.Combine(path, fn);
			// TODO: Must check that IssueData isn't null. If so, tell user to click Go first
			IssueGuidance.IssueData.TryGetValue((Year, Month), out IssueGuidance Guide);
			LRS_PdfUtils.CreateArticle(Infile, fn, Guide.TocPageStartNumber, Guide.TocPageEndNumber);
			Clipboard.SetText(fn);
		}

//---------------------------------------------------------------------------------------

		private void LvArticles_KeyPress(object sender, KeyPressEventArgs e) {
			if (e.KeyChar != '\r') return;
			LvArticles_DoubleClick(sender, e);
		}

//---------------------------------------------------------------------------------------

		private void BtnTest_Click(object sender, EventArgs e) {
#if false
			int EndYear, EndMonthIndex;
			if (ChkFirstIssueOnly.Checked) {
				EndYear = (int)CmbFirstYear.SelectedItem;
				EndMonthIndex = CmbFirstMonth.SelectedIndex + 1;
			} else {
				EndYear = (int)CmbLastYear.SelectedItem;
				EndMonthIndex = CmbLastMonth.SelectedIndex + 1;
			}
			var dr = new DateRange(
				(int)CmbFirstYear.SelectedItem,
				CmbFirstMonth.SelectedIndex + 1,
				EndYear,
				EndMonthIndex);
			foreach (var item in dr.Dates()) {
				// For some reason, August 1983 is missing. Sigh.
				if ((item.Year == 1983) && (item.Month == 8)) continue;
				int Decade = (item.Year / 10) * 10;
				SciAmDecadeDir = Path.Combine(SciAmBaseDir, $"$SCIAM - {Decade}'S");
			}
#endif
		}
	}
}
