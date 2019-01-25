using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChromeBookmarksSchema;
using Newtonsoft.Json;

namespace IndexMyBookmarks {
	public partial class IndexMyBookmarks : Form {

		// TODO: Sort method names

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Initialization code when the program (actually, the form/window) starts.
		/// Gets the database and cache ready.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void IndexMyUrls_Load(object sender, EventArgs e) {
			Statbar.Text = "";
			LbMsgs.Items.Clear();

			// Find our Documents folder where our database is/will be
			var docs                = Environment.SpecialFolder.MyDocuments;
			string FullDatabaseName = Environment.GetFolderPath(docs);
			FullDatabaseName        = Path.Combine(FullDatabaseName, DatabaseName);
			db                      = new PoorPersonsFullTextDatabase(FullDatabaseName);

			// Set up our words cache
			WordCache  = new IdCache(db, "Word");
			int nWords = WordCache.CacheAllWordIds();
			Msg($"Filled cache with {nWords:N0} word(s)");
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		///  Simply gets the contents of the clipboard in text format (if it exists;
		///  it may be, for example, a graphic) and puts it into the URL text box
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnUrlFromClipboard_Click(object sender, EventArgs e) {
			string Url = Clipboard.GetText(TextDataFormat.UnicodeText).ToLower();
			if (Url.Length > 0) { TxtUrl.Text = Url; }
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Process the URL to extract the text and update the database
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnIndexUrl_Click(object sender, EventArgs e) {
			ProcessUrl(TxtUrl.Text, "");
		}

//---------------------------------------------------------------------------------------

		private void BtnSearch_Click(object sender, EventArgs e) {
			DoSearch();
		}

//---------------------------------------------------------------------------------------

		private void DoSearch() {
			// TODO: Bug
/*
I've got a bug where the following query works:
	SELECT * FROM AllRefs WHERE Word='MICROSOFT'
	INTERSECT
	SELECT * FROM AllRefs WHERE Word='ACCESS'
But
	SELECT * FROM AllRefs WHERE Wordid=277530
	INTERSECT
	SELECT * FROM AllRefs WHERE Wordid=278106
doesn't. And yes, those are the right WordIDs.
*/
			var words = WordMunging.ParseAndCleanString(TxtSearchBox.Text);
			var sb = new StringBuilder();
			for (int i = 0; i < words.Count; i++) {
				if (i > 0) {
					sb.Append(Environment.NewLine + "INTERSECT" + Environment.NewLine);
				}
				string qry = GetSearchQuery(words[i]);
				sb.Append(qry);
			}

			string sql = sb.ToString();
			var entries = new List<SearchResult>();
			var rdr = db.ExecuteReader(sql);
			if (rdr.HasRows) {			// Read all results into <entries>
				while (rdr.Read()) {
					string Title = (string)rdr["Title"] ?? "N/A";
					string Url   = (string)rdr["Url"];
					entries.Add(new SearchResult(Title, Url));
				}
				// Sort the entries by Title and update the Results tab
				var SortedEntries = entries.OrderBy(entry => entry.Title).ToArray();
				LbSearchResults.Items.Clear();
				LbSearchResults.Items.AddRange(SortedEntries);
				tabControl1.SelectTab("TabSearchResults");
				Msg($"Query '{TxtSearchBox.Text}' yielded {entries.Count} hit(s)");
			} else {
				Msg("Query produced no hits");
			}
		}

//---------------------------------------------------------------------------------------

		private void EmptyTheDatabaseToolStripMenuItem1_Click(object sender, EventArgs e) {
			var response = MessageBox.Show("Empty the database?", "RED ALERT!!!",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (response != DialogResult.Yes) {
				MessageBox.Show("OK, cancel Red Alert. Database unchanged.",
					"Index My Urls", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			db.EmptyTheDatabase();
			Application.DoEvents();
			MessageBox.Show("Database emptied.", "Index My Urls",
				MessageBoxButtons.OK, MessageBoxIcon.Information);

		}

//---------------------------------------------------------------------------------------

		private void DoTheIndexing_Click(object sender, EventArgs e) {
			LbMsgs.Items.Clear();
			LbSearchResults.Items.Clear();
			lblFolder.Text = "";
			bQuit = false;

			Stopwatch sw = Timing.TimeIt(DoTheIndexing);

			DoneWithBookmarksStats(sw);
		}

//---------------------------------------------------------------------------------------

		private void DoTheIndexing() {
			var cbms = new ChromeBookmarks();	// Get info on all bookmarks

			CountBookmarks(cbms);				// How many, for progress bar
			int nNodes         = nFolders + nUrls;
			pbProgress.Maximum = nNodes * 1000;	// TODO: Get # right
			pbProgress.Value   = 0;

			ResetFoldersUrlStats();				// OK, reset the values

			// Here is where we do the actual indexing
			BeginTransaction();
			foreach (var node in cbms.Roots()) {
				ProcessNode(node, node.name);
			}
			EndTransaction();
		}

//---------------------------------------------------------------------------------------

		private void ResetFoldersUrlStats() {
			nFolders     = 0;
			nUrls        = 0;
			nUrlsSkipped = 0;
		}

//---------------------------------------------------------------------------------------

		private void CountBookmarks(ChromeBookmarks cbms) {
			bIndexUrls = false;
			foreach (var node in cbms.Roots()) {
				ProcessNode(node, node.name);
			}
			bIndexUrls = true;
		}

//---------------------------------------------------------------------------------------

		private void BeginTransaction() {
			string sql = "BEGIN TRANSACTION;";
			db.ExecuteNonQuery(sql);
		}

//---------------------------------------------------------------------------------------

		private void EndTransaction() {
			string sql = "COMMIT TRANSACTION;";
			db.ExecuteNonQuery(sql);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Once we've finished indexing bookmarks, show some overall stats
		/// </summary>
		/// <param name="sw"></param>
		/// <param name="CommitTime"></param>
		private void DoneWithBookmarksStats(Stopwatch sw) {
			Msg();
			float CacheCalls = WordCache.TotalCacheCalls;
			Msg($"{nFolders:N0} folder(s), {nUrls:N0} url(s), {nUrlsSkipped:N0} url(s) skipped");
			Msg($"Total words found/not found in cache: {WordCache.nWordsFoundInCache:N0} / {WordCache.nWordsNotInCache:N0}");
			Msg($"Cache hit ratio = {WordCache.nWordsFoundInCache / CacheCalls:P}");
			Msg($@"Done with test data in {sw.Elapsed:mm\:ss}.");
		}

//---------------------------------------------------------------------------------------

		private void ExitToolStripMenuItem_Click(object sender, EventArgs e) {
			Application.Exit();				// Buh-bye
		}

//---------------------------------------------------------------------------------------

		private void LbSearchResults_Click(object sender, EventArgs e) {
			ShowUrlInBrowser(sender);
		}

//---------------------------------------------------------------------------------------

		private void ShowUrlInBrowser(object sender) {
			var lb = sender as ListBox;
			string url = (lb.SelectedItem as SearchResult).Url;
			Web.Navigate(url);
			tabControl1.SelectTab("tabBrowser");
		}

//---------------------------------------------------------------------------------------

		private void BtnStop_Click(object sender, EventArgs e) {
			bQuit = true;
			// TODO: Other things, like re-enabling controls (and disable them when
			//		 searching or indexing
		}

//---------------------------------------------------------------------------------------

		private void BtnPreviousMatch_Click(object sender, EventArgs e) {
			var lb = LbSearchResults;
			int ix = lb.SelectedIndex;
			if (ix > 0) { --lb.SelectedIndex; }
		}

//---------------------------------------------------------------------------------------

		private void BtnNextMatch_Click(object sender, EventArgs e) {
			var lb = LbSearchResults;
			int ix = lb.SelectedIndex;
			if (ix != lb.Items.Count - 1) { ++lb.SelectedIndex; }
		}

//---------------------------------------------------------------------------------------

		private void LbSearchResults_SelectedIndexChanged(object sender, EventArgs e) {
			ShowUrlInBrowser(sender);
		}
	}
}
