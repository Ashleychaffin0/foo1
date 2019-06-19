using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

using ChromeBookmarksSchema;
using HAP = HtmlAgilityPack;

namespace IndexMyBookmarks {
	public partial class IndexMyBookmarks : Form {
		ParseHtmlWords parser;

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
			db                      = new PPFTD(FullDatabaseName);

			// Set up our words cache
			WordCache  = new IdCache(db, "Word");
			int nWords = WordCache.CacheAllWordIds();
			Msg($"Filled cache with {nWords:N0} word(s)");
		}

#if false   // Routine no longer used, but does show how to reference the Clipboard
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
#endif

//---------------------------------------------------------------------------------------

		private void BtnSearch_Click(object sender, EventArgs e) {
			DoSearch();
		}

//---------------------------------------------------------------------------------------

		private void DoSearch() {
			var words   = WordMunging.ParseAndCleanString(TxtSearchBox.Text);
			// var xxx = ParseHtmlWords.ConvertToPlainText("#MLNET â€“ New version 0.4, news Improvements in Text analysis using Word Embedding &#8211; El Bruno");
			string sql  = GetSearchSql(words);
			var entries = new List<SearchResult>();
			// Get the results of the query and put them for the moment in <entries>
			var rdr = db.ExecuteReader(sql);
			if (rdr.HasRows) {
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

		private string GetSearchSql(List<string> words) {
			var sb = new StringBuilder();
			// Loop through all words, creating a query that looks like
			//		SELECT Url, Title FROM AllRefs WHERE Word='MICROSOFT'
			//			INTERSECT
			//		SELECT Url, Title FROM AllRefs WHERE Word='ACCESS'
			for (int i = 0; i < words.Count; i++) {
				if (i > 0) {
					sb.Append(Environment.NewLine + "INTERSECT" + Environment.NewLine);
				}
				string qry = GetSearchQuery(words[i]);
				sb.Append(qry);
			}

			string sql = sb.ToString();
			return sql;
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
			MessageBox.Show("Database emptied.", "Index My Urls",
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

//---------------------------------------------------------------------------------------

			/// <summary>
			/// Respond to clicking the Index Bookmarks menu item
			/// </summary>
			/// <param name="sender"></param>
			/// <param name="e"></param>
		private void DoTheIndexing_Click(object sender, EventArgs e) {
			// Reset a few things
			LbMsgs.Items.Clear();
			LbSearchResults.Items.Clear();
			tabControl1.SelectTab("tabMessages");
			lblFolder.Text  = "";
			bStop			= false;

			Stopwatch sw = Timing.TimeIt(DoTheIndexing);

			DoneWithBookmarksStats(sw);
		}

//---------------------------------------------------------------------------------------

		private void DoTheIndexing() {
			try {
				var cbms = new ChromeBookmarks();   // Get info on all bookmarks

				parser = new ParseHtmlWords();		// We'll need this later

				pbProgress.Maximum = cbms.CountUrls();
				pbProgress.Value = 0;

				nUrlsSkipped = 0;					// OK, reset the value

				// Here is where we do the actual indexing
				BeginTransaction();
				int nUrl = 0;
				foreach (var bm in cbms.BookmarkUrls()) {	// bm == bookmark
					++nUrl;
					if (bStop || (dbgMaxUrlsToProcess > 0) && (nUrl > dbgMaxUrlsToProcess)) {
						break;
					}
					Msg();                // Add spacer line to the listbox
					Msg($"Processing URL[{nUrl:N0}]: {bm.url}");
					Msg($"Title:  {bm.name}");
					Msg($"Folder: {bm.folder_path}");
					ProcessUrl(bm);
					++pbProgress.Value;
				}
				EndTransaction();
			} catch (Exception ex) {
				Debugger.Break();				// TODO:
			}
		}

//---------------------------------------------------------------------------------------

		// HACK: Parse
		private void ProcessUrl(ChromeBookmark bm) {
			if (!IsUrl(bm.url)) {
				Msg($"Prepending http:// to {bm.url}");
				bm.url = "http://" + bm.url;        // OK, try this
			}

			// I've seen at least one url that points to a .pdf file. Don't try to index
			if (bm.url.EndsWith(".pdf")) { return; }

			var UrlID = GetUrlID(bm.url);
			if (UrlID.HasValue) {
				++nUrlsSkipped;
				if (ChkShowSkippingMsgs.Checked) {
					Msg($"        Skipping {bm.url}");
				}
				return;
			}

			db.Stats.Reset();
			int SaveCacheHits   = WordCache.nWordsFoundInCache;
			int SaveCacheMisses = WordCache.nWordsNotInCache;

			using (var wc = new WebClient()) {
				try {
					string RawHtml  = wc.DownloadString(bm.url);
					var doc			= new HAP.HtmlDocument();
					doc.LoadHtml(RawHtml);

					var sw = Timing.TimeIt(() =>
							ProcessHtmlTagContents(bm, doc, TagTitle, TagBody));

					ShowUrlProcessingStats(bm.url, SaveCacheHits, SaveCacheMisses, sw);
				} catch (Exception ex) {
					Msg($"*** Error '{ex.Message}' in {bm.url}");
					// if (ex.Message.Contains("UNIQUE")) Debugger.Break();	// TODO:
				}
			}
			return;
		}

//---------------------------------------------------------------------------------------

		private void ShowUrlProcessingStats(string url, int SaveCacheHits, int SaveCacheMisses, Stopwatch sw) {
			Msg($@"{sw.Elapsed:mm\:ss} to process {url}");
			int nHits	 = WordCache.nWordsFoundInCache - SaveCacheHits;
			int nMisses	 = WordCache.nWordsNotInCache - SaveCacheMisses;
			var HitRatio = nHits / (float)(nHits + nMisses);
			Msg($"Word cache hits: {nHits}, misses: {nMisses}, hit ratio: {HitRatio:P}");
		}

#if false
//---------------------------------------------------------------------------------------

		// TODO: This goes into ChromeBookmarkProcessing
		// TODO: Returns tuple?
		private void CountBookmarks(ChromeBookmarks cbms) {
			bIndexUrls = false;
			foreach (var node in cbms.Roots()) {
				zzzProcessNode(node, node.name);
			}
			bIndexUrls = true;
		}
#endif

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
			Msg($"{nUrlsSkipped:N0} url(s) skipped");
			Msg($"Total words found/not found in cache: {WordCache.nWordsFoundInCache:N0} / {WordCache.nWordsNotInCache:N0}");
			Msg($"Cache hit ratio = {WordCache.nWordsFoundInCache / CacheCalls:P}");
			Msg($@"Done indexing in {sw.Elapsed:mm\:ss}.");
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
			var lb     = sender as ListBox;
			string url = (lb.SelectedItem as SearchResult).Url;
			Web.Navigate(url);
			tabControl1.SelectTab("tabBrowser");
		}

//---------------------------------------------------------------------------------------

		private void BtnStop_Click(object sender, EventArgs e) {
			bStop = true;
			// TODO: Other things, like re-enabling controls (and disable them when
			//		 searching or indexing)
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
