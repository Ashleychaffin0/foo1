using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SciAmToc {
	public class IssueToc_1 : IssueToc {    // // 1948-05 to 1959-12
		public IssueToc_1(SciAmIssue Issue,
				int IssueYear,
				int IssueMonth)
			: base(Issue, IssueYear, IssueMonth) {
		}

//---------------------------------------------------------------------------------------

		internal override void ParseLines() {
			bool bContinue = true;
			bool bArticlesStarted = false;
			var re = new Regex(@" \d+ *\z", RegexOptions.Compiled); // TODO: Make static global
			int n = -1;     // TODO: <for>, not <foreach>
			foreach (string line in Issue.Toc.Lines) {
				if (!bContinue) break;
				++n;
				switch (line.Trim()) {
					case "ARTICLES":
						bArticlesStarted = true;
						break;
					case "DEPARTMENTS":
						bContinue = false;
						break;
					default:
						if (!bArticlesStarted) continue;
						string ArticleTitle = SciAmToc.GetPutativeTitle(line, out string Authors).Trim();
						if (SciAmToc.IsAllUpCase(ArticleTitle)) {
							var desc = SciAmToc.GatherDescription(n + 1, Issue.Toc.Lines);
							string desc2;
							if (desc.Count() == 0) {
								desc2 = "(No description found)";
							} else {
								desc2 = string.Join(" ", desc);
							}
							var match = re.Match(desc2).Value;
							int PageNo;
							if (match.Length > 0) {
								PageNo = Convert.ToInt32(match);
								desc2 = desc2.Substring(0, desc2.Length - match.Length).Trim();
							} else {
								PageNo = -1;
							}
							bool Interesting = Issue.Guide?.ArticleNamesOfInterest.Find(p => p == ArticleTitle) != null;
							Issue.Articles.Add(new SciAmArticle(Issue, ArticleTitle, Authors, PageNo, desc2, Interesting));
						}
						break;
				}
			}
		}
	}
}
