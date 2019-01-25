using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

using HAP = HtmlAgilityPack;

using ChromeBookmarksSchema;

/* Major TODOs
	* Get Progress bar working correctly
		* Add elapsed time, estimated time remaining
	* Do some kind of profiling to improve performance -- It's mostly downloading URLs
	* Split into classes. Perhaps tblWords et al, class DoSearch, etc 
	* Comment this sucker better, including .docx file
	* Profile it from an empty database
	* Run SLOC on this
	* Add the option to delete all traces of this ("Foreign Keys" / "Cascade Delete") in
		the database and continue
	* What to do about TxtUrl? Probably delete it (and event handler). Just add
		it as a new bookmark???
	* Add Options dialog for ShowSkipUrl, copy msgs (including Search?)
		to clipboard/file, whatever
	* Add other fields to database 
		* Timestamp when bookmark was created
		* Folder where this URL was found -- note that the same URL can be in more than
			one folder. We currently don't really support this
*/

/* Minor TODOs
	* On try/catch errors, give folder, url -- done for url, need Folder support.
		Add FolderName as global. Also add it to tblUrls? Or its own table?
	* Problem with PDF file. Is this really a PDF, or a bad link to one?
	* Rename BaseWord to StemWord?
	* Blank out/disable fields/buttons/menu items when necessary. Re-enable when done
	* Need Reload button on Browser tab
*/

namespace IndexMyBookmarks {
	public partial class IndexMyBookmarks : Form {
		const string DatabaseName = "IndexMyUrls.sqlite";
		const string TagTitle     = "//head/title";
		const string TagBody      = "//body";

		const int	 dbgMaxUrlsToProcess = 0;	// If > 0, only this many URLs processed
		bool bQuit = false;

		PoorPersonsFullTextDatabase db;
		IdCache WordCache;

		int nFolders     = 0;
		int nUrls        = 0;
		int nUrlsSkipped = 0;

		bool bIndexUrls;				// If false, just count them, for progress bar

//---------------------------------------------------------------------------------------

		public IndexMyBookmarks() {
			InitializeComponent();

			bIndexUrls = false;
		}

//---------------------------------------------------------------------------------------

		// HACK: Parse
		private bool ProcessUrl(string url, string title) {
			if (!IsUrl(url)) {
				Msg($"Prepending http:// to {url}");
				url = "http://" + url;		// OK, try this
			}

			var UrlID = GetUrlID(url);
			if (UrlID.HasValue) {
				++nUrlsSkipped;
				if (ChkShowSkippingMsgs.Checked) {
					Msg($"        Skipping {url}");
				}
				return false;
			}

			db.Stats.Reset();
			int SaveCacheHits   = WordCache.nWordsFoundInCache;
			int SaveCacheMisses = WordCache.nWordsNotInCache;

			Msg();                // Add spacer line to the listbox
			Msg($"Processing URL[{nUrls:N0}]: {url}");
			Msg($"Title:  {title}");
			Msg($"Folder: {lblFolder.Text}");

			using (var wc = new WebClient()) {
				try {
					string RawHtml = wc.DownloadString(url);
					var doc = new HAP.HtmlDocument();
					doc.LoadHtml(RawHtml);

					var sw = Timing.TimeIt(() => 
							ProcessHtmlTagContents(url, doc, TagTitle, TagBody));

					ShowUrlProcessingStats(url, SaveCacheHits, SaveCacheMisses, sw);
				} catch (Exception ex) {
					Msg($"*** Error '{ex.Message}'");
					Msg($"*** Error processing URL={url}");	// TODO: Needed?
				}
			}
			return true;
		}

//---------------------------------------------------------------------------------------

		private void ShowUrlProcessingStats(string url, int SaveCacheHits, int SaveCacheMisses, Stopwatch sw) {
			Msg($@"{sw.Elapsed:mm\:ss} to process {url}");
			int nHits    = WordCache.nWordsFoundInCache - SaveCacheHits;
			int nMisses  = WordCache.nWordsNotInCache - SaveCacheMisses;
			var HitRatio = nHits / (float)(nHits + nMisses);
			Msg($"Word cache hits: {nHits}, misses: {nMisses}, hit ratio: {HitRatio:P}");
		}

//---------------------------------------------------------------------------------------

		private void Msg() {
			Msg("");
		}

//---------------------------------------------------------------------------------------

		private void Msg(string msg) {

			if (msg.Length > 0) {
				msg = DateTime.Now.ToString("HH:mm:ss") + "  -  " + msg;
			}
			Statbar.Text = msg;
			LbMsgs.Items.Insert(0, msg);    // Most recent msg at the top
			Application.DoEvents();
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Common routine for processing the contents of the specified Html tag
		/// </summary>
		/// <param name="url"></param>
		/// <param name="doc"></param>
		/// <param name="tag"></param>
		// HACK: Parse
		private void ProcessHtmlTagContents(string url, HAP.HtmlDocument doc, params string[] tags) {
		var AllWords = new List<string>();
			string Title = "N/A";

			foreach (var tag in tags) {
				var TagNode = doc.DocumentNode.SelectSingleNode($"{tag}");

				var (CleanedText, TitleText) = GetInnerTextWords(TagNode);

				foreach (var item in CleanedText) {
					if (!WordMunging.IsValidateWord(item)) {
						Debugger.Break();
					}
				}

				if (tag == TagTitle) { Title = TitleText.Trim(); }
				AllWords.AddRange(CleanedText);
			}

			long UrlID = AddUrlToDatabase(db, url, Title);
			AddTextToDatabase(db, UrlID, AllWords.Distinct());	// TODO: Add folder here?
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Sigh. When I first wrote this routine, I thought that all I'd have to
		/// do was to take the InnerText of the HAP doc variable. But text like
		/// <a href='#'>Contact</a><button class='menu-toggle'>More</button> comes out
		/// as "ContactMore". Sigh again. Looks like we've got to traverse the whole
		/// body and pick them out individually. Triple sigh...
		/// 
		/// However, the good news is that the HTML Agility Pack has a method that visits
		/// every node in the HTML. Which makes things much, much easier.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		// HACK: Parse
		private (List<string> DistinctWords, string Title) GetInnerTextWords(HAP.HtmlNode MainNode) {
		var results = new List<string>();		// TODO: Should be HashSet, not List
			string Title = "";
			foreach (var node in MainNode.Descendants()) {
				if (IsIgnoredNode(node)) { continue; }	// <script> or <style>
				if (node.Name == "#text") {
					var text = WordMunging.ConvertToUtf8(node.InnerText);
					if (Title.Length == 0) { Title = text; }
					var words = WordMunging.ParseAndCleanString(text);
#if DEBUG && false
					if (text.Trim().Length > 0) {   // TODO: 
						Console.WriteLine("* " + text);
						Console.WriteLine("  => " + string.Join(", ", words.Distinct()));
					}
#endif
					results.AddRange(words);
				}
			}
			return (results.Distinct().ToList(), Title);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// We don't really want to index text inside a <script>. Or a <style>. See
		/// if this node is a child of one of these. If so, ignore it. 
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		// HACK: IX
		private bool IsIgnoredNode(HAP.HtmlNode node) {
			string xpath = node.XPath;
			return xpath.Contains("/script[") || xpath.Contains("/style[");
		}

//---------------------------------------------------------------------------------------

		// HACK: DB
		private long? GetUrlID(string url) {
string sql = $"SELECT UrlID FROM tblUrls WHERE url='{url}'";
			var UrlID = db.ExecuteScalar(sql);
			return (long?)UrlID;
		}

//---------------------------------------------------------------------------------------

		// Hack: DB
		private long AddUrlToDatabase(PoorPersonsFullTextDatabase db, string url, string Title) {
			// See comments about duplicates at the front of AddTextToDatabase(). But
			// since we're only doing this once per clicking the Go button, I'm not going
			// to worry about caching.
			long UrlID;
			string sql;
			Title = Title.Replace("'", "''");   // Beware of titles with possesive's
			try {
sql = $@"
INSERT INTO tblUrls (URL, Title, WhenRetrieved)
	VALUES('{url}', '{Title}', '{DateTime.Now}');
SELECT last_insert_rowid() FROM tblUrls;";
				UrlID = (long)db.ExecuteScalar(sql);
			} catch (Exception ex) {
sql = $"SELECT UrlID from tblUrls WHERE URL='{url}';";
				UrlID = (long)db.ExecuteScalar(sql);
			}
			Application.DoEvents();
			return UrlID;
		}

//---------------------------------------------------------------------------------------

			// Hack: DB
		private void AddTextToDatabase(PoorPersonsFullTextDatabase db, long urlID, IEnumerable<string> cleanedText) {
			// OK, here's our problem. We want a single unique instance of a word in
			// tblWords, and get a unique WordID. But we can't just do a simple INSERT
			// since the word may have been seen before (either in this URL, or in a
			// previous URL.
			//
			// Since queries to the database are fairly expensive, we'll maintain a
			// Dictionary mapping the word to its WordID. Before we try to INSERT a word
			// into tblWords, we'll check the dictionary first. In a sense we're caching
			// this aspect of the table.

			// On a different topic, Sqlite supports (among many, many other things) an
			// extended INSERT statement of the form
			//		INSERT INTO tblfoo (field1, field2)
			//			(1, 2),
			//			(3, 4);
			// So we'll collect the information for all the references and then pass
			// everything to AddWordsToTblRefs().
			var Values = new List<(long, long)>();

			foreach (var word in cleanedText) {
				var WordID = WordCache.GetIDForWord(word);
				Values.Add((WordID, urlID));
			}
			AddRefsToTblRefs(Values);
		}

//---------------------------------------------------------------------------------------

			// Hack: DB
		private void AddRefsToTblRefs(List<(long wordID, long urlID)> Refs) {
string sql = $@"
	INSERT INTO tblRefs (WordID, UrlID)
		VALUES";
			var sb = new StringBuilder(sql);
			foreach (var item in Refs) {
				sb.Append(Environment.NewLine + $"({item.wordID}, {item.urlID}),");
			}
			--sb.Length;        // Drop trailing comma
			sb.Append(";");
			db.ExecuteNonQuery(sb.ToString());
			Application.DoEvents();
		}

//---------------------------------------------------------------------------------------

			/// <summary>
			/// A very simple test to see if this is likely to be a real URL
			/// </summary>
			/// <param name="text"></param>
			/// <returns></returns>
		private bool IsUrl(string text) {
			text = text.ToLower();
			return text.StartsWith("http://") || text.StartsWith("https://");
		}

//---------------------------------------------------------------------------------------

		// HACK: DB
		private string GetSearchQuery(string word) {
			string sql = $"SELECT Url, Title FROM AllRefs WHERE Word='{word}'";
			return sql;
		}

//---------------------------------------------------------------------------------------

		// HACK: IX
		private void ProcessNode(ChromeBookmark child, string folderName) {
			foreach (var kid in child.children) {
				ProcessChild(kid, folderName, 0);
				if (bQuit) { return; }
			}
		}

//---------------------------------------------------------------------------------------

		// HACK: DB
		private bool ProcessChild(ChromeBookmark kid, string folderName, int level) {
			if (bIndexUrls) {
				++pbProgress.Value;
				Application.DoEvents();
			}
			if (kid.type == "folder") {
				++nFolders;
				folderName += "/" + kid.name;
				foreach (var grandkid in kid.children) {
					lblFolder.Text = folderName;
					Application.DoEvents();
					bQuit = ProcessChild(grandkid, folderName, level + 1);
					if (bQuit) { return true; }
				}
			}  else if (kid.type == "url" && !kid.url.StartsWith("chrome://")) {
				++nUrls;
				if (bIndexUrls) {
					bool bValid = ProcessUrl(kid.url, kid.name);
					if (bValid && (dbgMaxUrlsToProcess > 0)) {
						if (nUrls >= dbgMaxUrlsToProcess) { bQuit = true; } // TODO: > or >=?
					}
				}
			}
			return bQuit;
		}

//---------------------------------------------------------------------------------------

		private void CopyMessagesToClipboardToolStripMenuItem_Click(object sender, EventArgs e) {
			var sb = new StringBuilder();
			foreach (var item in LbMsgs.Items) {
				sb.AppendLine(item.ToString());
			}
			Clipboard.SetText(sb.ToString());
		}
	}
}
