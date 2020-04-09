using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

/* Logic (such as it is) of this routine:
 Note: The following comments are for Vintage 1989 09 onwards. Similar, but different
	   comments apply to 1948-05 to 1989--08.
 1) OnlineOCR.net seems to do a nice job of presenting the scanned data into what are
	essentially columns, with various sections offset at a common, well, offset from the
	beginning of each line.
 2) With that said, if the TOC is multi-page, each page is separated by a line containing
	a \f (formfeed), and the columns may be at different offsets
 3) The start of an article looks like "  39    Article Title ". We can find this via
	a Regex, and it's a single line.
 4) This is followed by a single line of Author info: "C. Kumar N. Patel" at the same
	offset.
 5) Then comes the multi-line comment (again at the same offset). The end of the comment
	is signaled by either
		1) A line that start with \f
		2) A line with "DEPARTMENTS" in it
		3) A new Article Title
*/

namespace MonitorClipboardChange {
	internal class Vintage_1948_05 : VintageBase {

		private ArticleInfo Article;
		private int Offset;
		private Regex reNewArticle;

//---------------------------------------------------------------------------------------

		public Vintage_1948_05(List<string> lines)
			: base(lines) {
		}

//---------------------------------------------------------------------------------------

		// Note: Most (all?) of the methods in this class should be virtual

		internal override void ParseToc() {
			// var art = new ArticleInfo();	// Will fill in as we find stuff
			var pat = @"^\s*\d+\s+";        // e.g. "  38    "
			reNewArticle = new Regex(pat, RegexOptions.Compiled);
			var MarginSizes = GetMarginSizes(reNewArticle);
			int ixMargins = 0;
			Offset = MarginSizes[ixMargins];
			for (int i = 0; i < Lines.Count; i++) {
				string line = Lines[i];
				// Step 5.1
				if (IsFormFeed(line)) {
					if (ixMargins++ >= MarginSizes.Count) continue; // Ignore trailing \f
					Offset = MarginSizes[ixMargins];
					continue;
				}
				// Step 5.2
				if (line.Contains("DEPARTMENTS")) {
					DoDepartments(i + 1);
					return;
					// TODO:
				}
				// Step 5.3
				if (IsNewArticle(line)) {
					GetAuthorsAndDescription(ref i);
					continue;
					// TODO:
				}
			}
		}

//---------------------------------------------------------------------------------------

		private bool IsFormFeed(string line) => line[0] == '\f';

//---------------------------------------------------------------------------------------

		private bool IsNewArticle(string line) {
			var m = reNewArticle.Match(line);
			if (m.Success) {    // Start of article
								// TODO: The title has the author(s). Scan it off
				string ArticleAuthors = "(No authors found)";
				string ArticleTitle = line.Substring(Offset).Trim();
				int ix = ArticleTitle.IndexOf(", by ");
				if (ix >= 0) {
					ArticleAuthors = ArticleTitle.Substring(ix + 5);
					ArticleTitle = ArticleTitle.Substring(0, ix);
				}
				Article = new ArticleInfo {
					Title = ArticleTitle,
					Authors = ArticleAuthors,
					PageNum = int.Parse(m.Value)
				};
				Articles.Add(Article);
				return true;
			}
			return false;
		}

//---------------------------------------------------------------------------------------

		private void GetAuthorsAndDescription(ref int i) {
			// Article.Authors = Lines[++i].Substring(Offset);
			// Now get Description
			// TODO: The only reason for end-of-description is either (a) DEPARTMENTS,
			//		 (b) EOF or (c) new article
			for (int j = ++i; j < Lines.Count; i++, j++) {
				string line = Lines[j];
				if (!Is_NOT_DescriptionLine(line)) {
					Article.Description.Add(line.Substring(Offset));
				} else {        // It's not a description line. Stop appending and return
					--i;        // Reprocess current line
					return;
				}
			}
		}

//---------------------------------------------------------------------------------------

		private bool Is_NOT_DescriptionLine(string line) {
			if (line.Length < Offset) return true;
			if (line.Contains("DEPARTMENTS")) return true;
			if (line[Offset] == ' ') return true;
			if (reNewArticle.Match(line).Success) return true;
			return false;
		}

//---------------------------------------------------------------------------------------

		private void DoDepartments(int LineNo) {
			/*
[62]: "   DEPARTMENTS"
[63]: " 8 Letters                   108 The Amateur Scientist"
 8 Letters                   108 The Amateur Scientist
[64]: " 12         50 and 100       112         Computer "
[65]: "            Years Ago                    Recreations"
[66]: "            September, 1887: The Thistle Social distance and the way "
[67]: "            has arrived in New York to   to the refreshment table: "
[68]: "            go after the America's Cup.  PARTY PLANNER figures it out."
[69]: " 16 The Authors              116 Books"
[70]: " 18 Science and the Citizen  120 Bibliography"
[71]: "                        © 1987 SCIENTIFIC AMERICAN, INC"
[72]: "\f"
*/
			// string pat = @"\s*\d+[^0-9]*";
			string pat = @"\d+[^0-9]*";
			var re = new Regex(pat);
			// (int ixFirstCol, int ixSecondCol) = DoFirstLine(Lines[LineNo], re);
			// for (int i = LineNo + 1; i < Lines.Count; i++) {
			for (int i = LineNo; i < Lines.Count; i++) {
				string line = Lines[i];
#if true
				int ix = 0;
				if ((ix = line.IndexOf("50 and 100")) >= 0) {   // Assume it's 1st in line
					string line2 = line.Trim();
					ix = line2.IndexOf(' ');
					int pNum = -1;
					if (ix >= 0) {
						int.TryParse(line2, out pNum);
					}
					Departments.Add(new ArticleInfo {
						PageNum = pNum,
						Title = "50 and 100 Years Ago"
					});
					line = line.Substring(ix + 11);
				}
				var m = re.Matches(line);
				if (m.Count == 0) continue;
				foreach (Match item in m) {
					ix = item.Value.IndexOf(' ');
					if (ix < 0) continue;
					if (!int.TryParse(item.Value.Substring(0, ix), out int PageNum)) continue;
					if (PageNum > 250) continue;
					// Console.WriteLine($"DEPTS: '{item.Value}', Line[{i}]='{line}'");
					var pieces = item.Value.Split(new string[] { " " }, 2, StringSplitOptions.RemoveEmptyEntries);
					if (pieces.Length != 2) continue;
					if (pieces[1] == "Computer ") pieces[1] = "Computer Recreations";
					var art = new ArticleInfo {
						PageNum = Convert.ToInt32(pieces[0]),
						Title = pieces[1]
					};
					Departments.Add(art);
				}
#endif
			}
		}

//---------------------------------------------------------------------------------------

		private (int ixFirstCol, int ixSecondCol) DoFirstLine(string line, Regex re) {
			var m = re.Matches(line);
			int ix1 = m.Count > 1 ? m[1].Index : -1;
			return (m[0].Index, ix1);
		}

//---------------------------------------------------------------------------------------

		internal override string GetData() {
			var sb = new StringBuilder();
			foreach (var art in Articles) {
				sb.AppendLine($"A {art.Title}");
			}
			sb.AppendLine("O");
			sb.AppendLine("ARTICLES");
			foreach (var art in Articles) {
				var auth = art.Authors == "" ? "" : $", by {art.Authors}";
				sb.AppendLine($"{art.PageNum} {art.Title.ToUpper()}{auth}");
				sb.AppendLine("\t" + art.Desc);
			}
			Departments = (List<ArticleInfo>)Departments.OrderBy(x => x.PageNum).ToList<ArticleInfo>();
			foreach (var dept in Departments) {
				sb.AppendLine($"{dept.PageNum:D2} {dept.Title.ToUpper()}");
				if (dept.Title.StartsWith("50")) sb.AppendLine("");
				if (dept.Title == "Computer Recreations") sb.AppendLine("");
			}
			sb.AppendLine("$");
			return sb.ToString();
		}


#if false
		//---------------------------------------------------------------------------------------

		internal void Old_Parse() {
			Articles = new List<ArticleInfo>();
			var pat = @"^\s*\d+\s+";        // e.g. "  38    "
			var re = new Regex(pat, RegexOptions.Compiled);
			var MarginSizes = GetMarginSizes(re);
			int ixMargins = 0;
			int ofs = MarginSizes[ixMargins];
			for (int i = 0; i < Lines.Count; i++) {
				string line = Lines[i];
				if (line.Contains("DEPARTMENTS")) {	// TODO: Do inside ProcessArticles
					DoDepartments(i + 1);
					return;
				}
				if ((line.Length > 0) && (line[0] == '\f') && (ixMargins < MarginSizes.Count)) {
					
				}
				line = line.Trim();         // Get rid of trailing \r's
				if (line.Length == 0) continue;
				var m = re.Match(line);
				if (m.Success) {    // Start of article
					int.TryParse(m.Value, out int PageNum);
					i = Old_ProcessArticle(i, ofs, re, out ArticleInfo art);
					art.PageNum = PageNum;	// TODO: Put this inside ProcessArticle
					Articles.Add(art);  // TODO: Add to Articles inside ProcessArticle
					// TODO: Rename to ProcessArticle***s***
					// TODO: Rename to ProcessTOC
					// TODO: Also handle DEPARTMENTS in there
				}
			}
		}
#endif

#if false
		//---------------------------------------------------------------------------------------

		enum ArticleStates {
			Title,
			Author,
			Description
		};

//---------------------------------------------------------------------------------------

		private int Old_ProcessArticle(int i, int ofs, Regex re, out ArticleInfo art) {
			string Title  = "";
			string Author = "";
			string Desc   = "";
			var  Description = new List<string>();
			ArticleStates CurState = ArticleStates.Title;
			bool bScanning = true;
			while (bScanning) {
				// string line = Lines[i++].TrimEnd(); // Drop trailing \r
				string line = Lines[i++];
				if (re.Match(line).Success) {
					if (Title.Length > 0) {			// New article found
						Desc = string.Join(" ", Description);
						art  = new ArticleInfo(Title, Author, Desc);
						return i - 2;
					} else {
						CurState = ArticleStates.Title;
					}
				}
				string line2 = "";
				if (line.Length > ofs) line2 = line.Substring(ofs);
				switch (CurState) {
					case ArticleStates.Title:
						Title = line2;
						CurState = ArticleStates.Author;
						break;
					case ArticleStates.Author:
						Author = line2;
						CurState = ArticleStates.Description;
						break;
					case ArticleStates.Description:
						// if (line2.Length == 0) continue;	// TODO: s/b State=Ignore?
						if (line.Contains("DEPARTMENTS")) {
							bScanning = false;
						} else if ((line2.Length > 0) && (line2[0] != ' ')) {
							Description.Add(line2);
						}
						break;
					default:
						break;
				}
			}
			Desc = string.Join(" ", Description);
			art  = new ArticleInfo(Title, Author, Desc);
			return i - 2;
		}
#endif
	}
}