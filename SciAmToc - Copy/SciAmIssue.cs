using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;

namespace SciAmToc {
	public class SciAmIssue {
		public int						Year;
		public int						Month;
		public bool                     FileNotFound;
		public StreamWriter				OutFile;
		public string					Title;
		public string                   FullPath;
		public int                      NumberOfPages;
		public PdfReader				pRdr;
		public PdfDocument              pDoc;
		public IssueToc                 Toc;
		public List<SciAmArticle>		Articles;
		public HashSet<int>             Swaps;
		public HashSet<int>             Glues;
		public List<SciAmDepartment>	Departments;
		public List<string>             OcrResults;
		public IssueGuidance            Guide;

//---------------------------------------------------------------------------------------

		public SciAmIssue() {
			Articles    = new List<SciAmArticle>();
			Departments = new List<SciAmDepartment>();
			Swaps       = new HashSet<int>();
			Glues       = new HashSet<int>();

			FileNotFound = false;
		}

//---------------------------------------------------------------------------------------

		public SciAmIssue(int Year, int Month, StreamWriter OutFile) : this() {
			this.Year    = Year;
			this.Month   = Month;
			this.OutFile = OutFile;
			bool bOK = GetBasicIssueInfo();
			if (! bOK) {
				FileNotFound = true;
				return;
			}
			GetRdrDoc();
		}

//---------------------------------------------------------------------------------------

		private bool GetBasicIssueInfo() {
			string dir = Path.Combine(SciAmToc.Main.SciAmDecadeDir, $"Sciam - {Year}");
			if (!Directory.Exists(dir)) return false;
			SciAmToc.Main.Issues.Add(this);
			// The following will find (hopefully!) exactly one file that matches the
			// curent year and month. 
			string FNamePrefix = $"Sciam - {Year}-{Month:d2}";
			var FullPathList   = Directory.GetFiles(dir, $"{FNamePrefix}*.pdf");
			if (FullPathList.Length == 0) return false;   // Missing Issue

			FullPath = FullPathList[0];
			Title    = Path.GetFileNameWithoutExtension(FullPath);
			return true;
		}

//---------------------------------------------------------------------------------------

		public void FormatHtmlOutput() {
			bool bNoDesc = SciAmToc.Main.ChkSkipDescrptions.Checked;
			OutFile.WriteLine($"<p/><issue>{Title}</issue><p/>");

			foreach (var art in Articles) {
				if (art.IsInteresting) {
					OutFile.WriteLine($"\t<p/><article class=\"interesting\">{art.Title}</article> by <author>{art.Authors}</author><br/>");
				} else {
					OutFile.WriteLine($"\t<p/><article                    >{art.Title}</article> by <author>{art.Authors}</author><br/>");
					// 	OutFile.WriteLine($"\t<b                          >{ArticleTitle}</b      ><author>{Authors}</author><br/>");
				}
				if ((art.Description.Length == 0) && !bNoDesc) {
					OutFile.WriteLine("<desc>(No description found)</desc>");
					continue;
				}
				string desc2 = string.Join(" ", art.Description);
				if (!bNoDesc) {
					OutFile.WriteLine($"\t\t<desc>{desc2}</desc><br/>");
				}

			}
		}

//---------------------------------------------------------------------------------------

		private void GetRdrDoc() {
			pRdr = new PdfReader(FullPath);
			pDoc = new PdfDocument(pRdr);
			NumberOfPages = pDoc.GetNumberOfPages();
		}
	}

//---------------------------------------------------------------------------------------

	public class SciAmArticle {
		public SciAmIssue   Issue;
		public string		Title;
		public string		Authors;			// TODO: Make List<string>
		public int			StartPageNumber;
		public string		Description;
		public bool			IsInteresting;		// TODO: Make bool

//---------------------------------------------------------------------------------------

		public SciAmArticle(SciAmIssue Issue, string Title, string Authors, int StartPageNumber, string Description, bool IsInteresting) {
			this.Issue			 = Issue;
			this.Title           = Title;		// TODO: Must clean Title of non-filesystem chars
			this.Authors         = Authors;
			this.StartPageNumber = StartPageNumber;
			this.Description     = Description;
			this.IsInteresting   = IsInteresting;
		}
	}

//---------------------------------------------------------------------------------------

	public class SciAmDepartment {
		// TODO: Same as SciAmArticle, but with flag that says this is a Department?
		public string	Name;
		public int      StartPageNumber;
	}
}
