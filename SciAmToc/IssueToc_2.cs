using System.Linq;
using System.Text.RegularExpressions;

namespace SciAmToc {
	public class IssueToc_2 : IssueToc {   // 1956-01 to 1996-03
		public IssueToc_2(
				SciAmIssue	Issue,
				int			IssueYear,
				int			IssueMonth)
			: base(Issue, IssueYear, IssueMonth) {
		}

//---------------------------------------------------------------------------------------

		internal override void ParseLines() {
			bool bContinue = true;
			bool bArticlesStarted = false;
			var blank = new char[] { ' ' };
			var re = new Regex(@" \d+ *\z", RegexOptions.Compiled); // TODO: Make static global
			for (int n = 0; n < Lines.Count; n++) {
				var line = Lines[n].Trim();
				if (line.Length == 0) continue;
				if (!bContinue) break;
				switch (line.Trim()) {
					case "ARTICLES":
						bArticlesStarted = true;
						break;
					case "DEPARTMENTS":
						bContinue = false;
						break;
					default:
						if (!bArticlesStarted) continue;
						var ArticleStart = line.Split(blank, 2);
						bool bOK = int.TryParse(ArticleStart[0], out int PageNo);
#if false
						if (! bOK) {
							System.Windows.Forms.MessageBox.Show($"Test: Line {n}: {line}");
							continue;
						}
						if (!bOK) {
							System.Windows.Forms.MessageBox.Show($"Test2: Line {n}: {line}");
							continue;
						}
#endif
						if (ArticleStart.Length <= 1) continue;		// User must update Guidance
						string ArticleTitle = SciAmToc.GetPutativeTitle(ArticleStart[1], out string Authors).Trim();
						if (SciAmToc.IsAllUpCase(ArticleTitle)) {
							var desc = SciAmToc.GatherDescription(n + 1, Issue.Toc.Lines);
							string desc2;
							if (desc.Count() == 0) {
								desc2 = "(No description found)";
							} else {
								desc2 = string.Join(" ", desc).Trim();
								n += desc.Count();
							}
							bool Interesting = Issue.Guide?.ArticleNamesOfInterest.Find(p => p == ArticleTitle.ToUpper()) != null;
							Issue.Articles.Add(new SciAmArticle(Issue, ArticleTitle, Authors, PageNo, desc2, Interesting));
						}
						break;
					}
				}
			}
		}
}
