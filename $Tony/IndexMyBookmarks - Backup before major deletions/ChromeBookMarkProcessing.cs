using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using ChromeBookmarksSchema;
using HAP = HtmlAgilityPack;

namespace IndexMyBookmarks {
	partial class IndexMyBookmarks {    // TODO: Not partial class. Move these methods somewhere

#if false
//---------------------------------------------------------------------------------------

		public void zzzProcessNode(ChromeBookmark node, string nodeName) {
			foreach (var kid in node.children) {
				zzzProcessChild(kid, nodeName, 0);
				if (bStop) { return; }
			}
		}
#endif

#if false
//---------------------------------------------------------------------------------------

		// HACK: DB
		private bool zzzProcessChild(ChromeBookmark kid, string folderName, int level) {
			if (kid.type == "folder") {
				++nFolders;
				folderName += "/" + kid.name;
				foreach (var grandkid in kid.children) {
					lblFolder.Text = folderName;
					DoEvents();
					bStop = zzzProcessChild(grandkid, folderName, level + 1);
					if (bStop) { return true; }
				}
			} else if (kid.type == "url" && !kid.url.StartsWith("chrome://")) {
				if (bIndexUrls) {
					++pbProgress.Value;
					DoEvents();
				}
				++nUrls;
				if (bIndexUrls) {
					bool bValid = zzzProcessUrl(kid, folderName);
					if (bValid && (dbgMaxUrlsToProcess > 0)) {
						if (nUrls >= dbgMaxUrlsToProcess) { bStop = true; } // TODO: > or >=?
					}
				}
			}
			return bStop;
		}
#endif

#if false
		//---------------------------------------------------------------------------------------

		// HACK: Parse
		private bool zzzProcessUrl(ChromeBookmark bm, string folder) {
			if (!IsUrl(bm.url)) {
				Msg($"Prepending http:// to {bm.url}");
				bm.url = "http://" + bm.url;        // OK, try this
			}

			var UrlID = GetUrlID(bm.url);
			if (UrlID.HasValue) {
				++nUrlsSkipped;
				if (ChkShowSkippingMsgs.Checked) {
					Msg($"        Skipping {bm.url}");
				}
				return false;
			}

			db.Stats.Reset();
			int SaveCacheHits   = WordCache.nWordsFoundInCache;
			int SaveCacheMisses = WordCache.nWordsNotInCache;

			Msg();                // Add spacer line to the listbox
			Msg($"Processing URL[{nUrls:N0}]: {bm.url}");
			Msg($"Title:  {bm.name}");
			Msg($"Folder: {lblFolder.Text}");

			using (var wc = new WebClient()) {
				try {
					string RawHtml = wc.DownloadString(bm.url);
					var doc = new HAP.HtmlDocument();
					doc.LoadHtml(RawHtml);

					var sw = Timing.TimeIt(() =>
							ProcessHtmlTagContents(bm, doc, TagTitle, TagBody));

					ShowUrlProcessingStats(bm.url, SaveCacheHits, SaveCacheMisses, sw);
				} catch (Exception ex) {
					Msg($"*** Error '{ex.Message}'");
				}
			}
			return true;
		}
#endif

#if false
//---------------------------------------------------------------------------------------

		private void zzzShowUrlProcessingStats(string url, int SaveCacheHits, int SaveCacheMisses, Stopwatch sw) {
			Msg($@"{sw.Elapsed:mm\:ss} to process {url}");
			int nHits    = WordCache.nWordsFoundInCache - SaveCacheHits;
			int nMisses  = WordCache.nWordsNotInCache - SaveCacheMisses;
			var HitRatio = nHits / (float)(nHits + nMisses);
			Msg($"Word cache hits: {nHits}, misses: {nMisses}, hit ratio: {HitRatio:P}");
		}
#endif

#if false
//---------------------------------------------------------------------------------------

		/// <summary>
		/// Common routine for processing the contents of the specified Html tag
		/// </summary>
		/// <param name="url"></param>
		/// <param name="doc"></param>
		/// <param name="tag"></param>
		// HACK: Parse
		private void qqqProcessHtmlTagContents(ChromeBookmark bm, HAP.HtmlDocument doc, params string[] tags) {
			var AllWords = new List<string>();
			string Title = "N/A";

			foreach (var tag in tags) {
				var TagNode = doc.DocumentNode.SelectSingleNode($"{tag}");

				var (CleanedText, TitleText) = GetInnerTextWords(TagNode);

				if (tag == TagTitle) { Title = TitleText.Trim(); }
				AllWords.AddRange(CleanedText);
			}

			long UrlID = AddUrlToDatabase(db, bm, Title);
			AddTextToDatabase(db, UrlID, AllWords.Distinct());  // TODO: Add folder here?
		}
#endif

//---------------------------------------------------------------------------------------

			/// <summary>
			/// We don't really want to index text inside a <script>. Or a <style>. See
			/// if this node is a child of one of these. If so, ignore it. 
			/// </summary>
			/// <param name="node"></param>
			/// <returns></returns>
		// HACK: IX
		private static bool IsIgnoredNode(HAP.HtmlNode node) {
			string xpath = node.XPath;
			return xpath.Contains("/script[") || xpath.Contains("/style[");
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

		// HACK: DB
		private long? GetUrlID(string url) {
			string sql = $"SELECT UrlID FROM tblUrls WHERE url='{url}'";
			var UrlID  = db.ExecuteScalar(sql);
			return (long?)UrlID;
		}
	}
}