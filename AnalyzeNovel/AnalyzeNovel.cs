using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

// TODO: Stats panel, with (e.g.) # of words up to 10%, 20%, etc
// TODO: Least squares Zipf slope maybe? See http://procbits.com/2011/05/02/linear-regression-in-c-sharp-least-squares
//		 Also http://web.archive.org/web/20091002230502/http://www.trentfguidry.net:80/post/2009/07/19/Linear-and-Multiple-Linear-Regression-in-C.aspx
//		 Also http://web.archive.org/web/20091128150208/http://www.trentfguidry.net:80/post/2009/07/11/Creating-a-Matrix-Class-in-C-Part-4-Parallel-Processing-with-Threading-Parallel-For.aspx
//		 Also http://mathworld.wolfram.com/LeastSquaresFitting.html

#if false // Froim https://en.wikipedia.org/wiki/Zipf%27s_law
	For example, Zipf's law states that given some corpus of natural language utterances,
	the frequency of any word is inversely proportional to its rank in the frequency table.
	Thus the most frequent word will occur approximately twice as often as the second most
	frequent word, three times as often as the third most frequent word, etc.: the
	rank-frequency distribution is an inverse relation. For example, in the Brown Corpus
	of American English text, the word "the" is the most frequently occurring word,
	and by itself accounts for nearly 7% of all word occurrences (69,971 out of slightly
	over 1 million). True to Zipf's Law, the second-place word "of" accounts for
	slightly over 3.5% of words (36,411 occurrences), followed by "and" (28,852).
	Only 135 vocabulary items are needed to account for half the Brown Corpus.
#endif

namespace AnalyzeNovel {
	public partial class AnalyzeNovel : Form {
		string		BaseDir = @"G:\lrs\Text Novels";	// Could put this in parms file
		NovelStats	CurStats;

//---------------------------------------------------------------------------------------

		public AnalyzeNovel() {
			InitializeComponent();

			TxtFolder.Text = BaseDir;
			// ShowFiles();
			PopulateTocTree(BaseDir);
		}

//---------------------------------------------------------------------------------------

		private void PopulateTocTree(string baseDir) {
			// TODO: Supports only one level deep. Should really be recursive. But be
			//		 careful if you want to use a routine like this in a different
			//		 context (e.g. to display the contents of an entire drive), then
			//		 you may well wind up with an awfully big tree that can take many
			//		 seconds to construct.
			// TODO: Alternatively, initially fill the tree one level deep. Then detect
			//		 when a node is opened are fill in the sub nodes on demand. But be
			//		 aware that if you collapse a node then later reopen it, you must
			//		 refresh all sub nodes since files/directories may be added or
			//		 deleted or renamed by other processes.
			foreach (var dir in Directory.GetDirectories(BaseDir)) {
				var CurNode = TvToc.Nodes.Add(Path.GetFileName(dir));
				foreach (var SubDir in Directory.GetDirectories(dir)) {
					CurNode.Nodes.Add(Path.GetDirectoryName(SubDir));
				}
				foreach (var file in Directory.GetFiles(dir, "*.txt")) {
					var node = CurNode.Nodes.Add(Path.GetFileNameWithoutExtension(file));
					node.Tag = Path.Combine(dir, file);
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void BtnBrowse_Click(Object sender, EventArgs e) {
			var fbd = new FolderBrowserDialog {
				SelectedPath = TxtFolder.Text
			};
			var res = fbd.ShowDialog();
			if (res != DialogResult.OK) {
				return;
			}
			TxtFolder.Text = fbd.SelectedPath;
			PopulateTocTree(TxtFolder.Text);
			// ShowFiles();
		}

//---------------------------------------------------------------------------------------

		private void ShowFolders(string PathName) {
			var dirs = Directory.GetDirectories(PathName);
			foreach (var dir in dirs) {
				TvToc.Nodes.Add(dir);
			}
		}

//---------------------------------------------------------------------------------------

		private void ShowStats(NovelStats stats) {
			if (stats == null) return;
			ShowStatsWordsLengths(stats);
			ShowChartWordLengths(stats);
		}

//---------------------------------------------------------------------------------------

		private void ShowChartWordLengths(NovelStats stats) {
			var Zipf = chart1.Series["Zipf"];
			Zipf.Points.Clear();

			int nWords = stats.SortedWords.Count;
			var Points = new PointF[nWords];
			for (int i = 1; i <= nWords; i++) {
				float x = (float)Math.Log(i);
				float y = (float)Math.Log(stats.SortedWords[i - 1].Value);
				Zipf.Points.AddXY(x, y);
				Points[i - 1] = new PointF(x, y);
			}

			// Show regression line
			var Regress = chart1.Series["RegressionLine"];
			Regress.Points.Clear();

			bool bOK = stats.linreg(Points, out float m, out float b, out float r);
			// Should check bOK, but unlikely to occur
			DataPoint LinePoint(double x, double y) => new DataPoint(x, m * x + b);
			Regress.Points.Add(LinePoint(0, Math.Log(stats.SortedWords[1].Value)));
			Regress.Points.Add(LinePoint(Math.Log(nWords), Math.Log(stats.SortedWords[nWords - 1].Value)));
			Regress.Label = $"Slope = {m:N2}";
			Regress.Font  = new Font("Arial", 12);

		}

//---------------------------------------------------------------------------------------

		private void ShowStatsWordsLengths(NovelStats stats, string Prefix = "") {
			LvWordsCounts.Items.Clear();
			LvWordsCounts.BeginUpdate();
			float PrevValue   = 0;
			float PctDecrease = 0;
			foreach (var item in stats.SortedWords) {
				if (Prefix.Length > 0) {
					// Unfortunately the .Net framework doesn't, uh, contain a
					// string.Contains method that allows for case-insensitive search.
					// With a bit of research I found that there are i18n concerns. It's
					// possible to do one with a culture.CompareInfo method, but let's keep
					// it simple. The following code works best with English.
					if (!item.Key.ToUpper().Contains(Prefix.ToUpper()))
						continue;
				}

				if (PrevValue != 0) {
					PctDecrease = 100 * (1 - (float)item.Value / PrevValue);
				}
				PrevValue = item.Value;
				var items = new string[] {
					item.Key,
					item.Value.ToString("N0"),
					PctDecrease.ToString("N2")
				};
				var it = new ListViewItem(items);
				LvWordsCounts.Items.Add(it);
			}
			LvWordsCounts.EndUpdate();
		}

//---------------------------------------------------------------------------------------

		private void XxxShowStats(NovelStats stats) {
			if (stats == null) return;
			LvStats.Items.Clear();

			int nWords       = stats.SortedWords.Count;
			int nLengths     = stats.SortedLengths.Count;
			var nTotalWords  = stats.SortedWords.Sum(p => p.Value);
			int RunningCount = 0;

			LvStats.BeginUpdate();
			for (int i = 0; i < nWords; i++) {
				var w = stats.SortedWords.ElementAt(i);
				string WordLength, LengthCount;
				if (i < nLengths) {
					WordLength  = stats.SortedLengths.ElementAt(i).Key.ToString("N0");
					LengthCount = stats.SortedLengths.ElementAt(i).Value.ToString("N0");
				} else {
					WordLength = LengthCount = "";
				}
				RunningCount += w.Value;
				var item = new ListViewItem(new string[] {
						(i + 1).ToString("N0"),
						w.Key,
						w.Value.ToString("N0"),
						RunningCount.ToString("N0"),
						((float)RunningCount / nTotalWords).ToString("p"),
						WordLength, LengthCount });
				LvStats.Items.Add(item);
			}
			LvStats.EndUpdate();
		}

//---------------------------------------------------------------------------------------

		private void TxtFindWord_TextChanged(object sender, EventArgs e) {
			LvStats.SelectedItems.Clear();
			string txt = (sender as TextBox).Text;      // Or just = txtFindWord.Text
			ShowStatsWordsLengths(CurStats, txt);
			TxtFindWord.Focus();
		}
//---------------------------------------------------------------------------------------

#if false
		private void ShowFiles() {
			LbNovelNames.Items.Clear();
			// SearchOption opt = ChkSubFolders.Checked ? SearchOption.AllDirectories : 0;
			// var names = Directory.EnumerateFiles(TxtFolder.Text, "*.txt", opt);
			var names = Directory.EnumerateFiles(TxtFolder.Text, "*.txt");
			bool bNovelFound = false;
			foreach (var name in names.OrderBy(p => p)) {   // Names in sorted order
				bNovelFound = true;
				LbNovelNames.Items.Add(new NovelName(name));
			}
			if (bNovelFound) {
				LbNovelNames.SelectedIndex = 0;         // Show highlight on first element
			}
		}
		}
#endif

//---------------------------------------------------------------------------------------

		private void TvToc_AfterSelect(object sender, TreeViewEventArgs e) {
			var node = e.Node as TreeNode;
			if (node.Nodes.Count > 0) return;   // Non-terminal node

			TxtFindWord.Text = "";
			// Do a bit of caching. If we've already processed this file,
			//	don't do it again.
			switch (node.Tag) {
			case string Filename:
				CurStats = new NovelStats(Filename);
				CurStats.ProcessFile();
				node.Tag = CurStats;
				break;
			default:
				CurStats = node.Tag as NovelStats;
				break;
			}
			ShowStats(CurStats);
		}
	}
}
