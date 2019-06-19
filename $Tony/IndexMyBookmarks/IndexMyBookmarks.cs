using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ChromeBookmarksSchema;

using HAP = HtmlAgilityPack;

/* Major TODOs
	* Get Progress bar working correctly
		* Add elapsed time, estimated time remaining
	* Split into classes. Perhaps tblWords et al, class DoSearch, etc 
	* Comment this sucker better, including .docx file
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

namespace IndexMyBookmarks {
	public partial class IndexMyBookmarks : Form {
		internal string DatabaseName = "IndexMyBookmarks.sqlite";
		internal string TagTitle     = "//head/title";
		internal string TagBody      = "//body";
		// TODO: //body/#text ?

		internal int  dbgMaxUrlsToProcess = 50;	// If > 0, only this many URLs processed
		internal bool bStop = false;			// Stop button pushed

		internal PPFTD	 db;
		internal IdCache WordCache;
		internal int	 nUrlsSkipped = 0;

		public static IndexMyBookmarks Main;

//---------------------------------------------------------------------------------------

		public IndexMyBookmarks() {
			InitializeComponent();
			Main = this;
		}

//---------------------------------------------------------------------------------------

		internal void DoEvents() {
			Application.DoEvents();
		}

//---------------------------------------------------------------------------------------

		internal void Msg(string msg = "") {
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
		private void ProcessHtmlTagContents(ChromeBookmark bm, HAP.HtmlDocument doc, params string[] tags) {
			var AllWords = new List<string>();
			string Title = "N/A";

			foreach (var tag in tags) {
				var TagNode = doc.DocumentNode.SelectSingleNode($"{tag}");

				string TitleText = GetInnerTextWords(TagNode);

				if (tag == TagTitle) { Title = TitleText; }
				AllWords.AddRange(parser.Words);
			}

			long UrlID = AddUrlToDatabase(db, bm, Title);
			AddTextToDatabase(db, UrlID, AllWords.Distinct());
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
		private string GetInnerTextWords(HAP.HtmlNode MainNode) {
			// TODO: This is wrong. We don't know that this is a Title tag
			string Title = "";
			foreach (var node in MainNode.Descendants()) {
				if (IsIgnoredNode(node)) { continue; }  // <script> or <style>
				if (node.Name == "#text") {
					parser.ParseInnerText(node.InnerText);
					if (Title.Length == 0) { Title = node.InnerText.Trim(); }
				}
			}
			return Title;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// We don't really want to index text inside a <script>. Or a <style>. See
		/// if this node is a child of one of these. If so, ignore it. 
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		private bool IsIgnoredNode(HAP.HtmlNode node) {
			string xpath = node.XPath;
			return xpath.Contains("/script[") || xpath.Contains("/style[");
		}

//---------------------------------------------------------------------------------------

		internal long AddUrlToDatabase(PPFTD db, ChromeBookmark bm, string Title) {
			// See comments about duplicates at the front of AddTextToDatabase(). But
			// since we're only doing this once per clicking the Go button, I'm not going
			// to worry about caching.
			long UrlID;
			string sql;
			Title = Title.Replace("'", "''");   // Beware of titles with possesive's
			try {
sql = $@"
INSERT INTO tblUrls (URL, Title, WhenCreated, Folder)
	VALUES('{bm.url}', '{Title}', '{bm.date_added_dt}', '{lblFolder.Text}');
SELECT last_insert_rowid() FROM tblUrls;";
				UrlID = (long)db.ExecuteScalar(sql);
			} catch (Exception ex) {
sql = $"SELECT UrlID from tblUrls WHERE URL='{bm.url}';";
				UrlID = (long)db.ExecuteScalar(sql);
			}
			Application.DoEvents();
			return UrlID;
		}

//---------------------------------------------------------------------------------------

		internal void AddTextToDatabase(PPFTD db, long urlID, IEnumerable<string> cleanedText) {
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

		private string GetSearchQuery(string word) {
			string sql = $"SELECT Url, Title FROM AllRefs WHERE Word='{word}'";
			return sql;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// A very simple test to see if this is likely to be a real URL
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		private static bool IsUrl(string text) {
			text = text.ToLower();
			return text.StartsWith("http://") || text.StartsWith("https://");
		}

//---------------------------------------------------------------------------------------

		private long? GetUrlID(string url) {
			string sql = $"SELECT UrlID FROM tblUrls WHERE url='{url}'";
			var UrlID = db.ExecuteScalar(sql);
			return (long?)UrlID;
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
