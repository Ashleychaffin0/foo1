using System;
using System.Collections.Generic;

namespace ChromeBookmarksSchema {
	public class ChromeBookmarksRoot {
		public string	checksum			{ get; set; }
		public ChromeBookmarksRoots roots	{ get; set; }
		public string	version				{ get; set; }
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class ChromeBookmarksRoots {
		public ChromeBookmark bookmark_bar		{ get; set; }
		public ChromeBookmark other				{ get; set; }
		public string sync_transaction_version	{ get; set; }
		public ChromeBookmark synced			{ get; set; }
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class ChromeBookmark {
		public string date_added				{ get; set; }
		public string date_modified				{ get; set; }
		public string id						{ get; set; }
		public string name						{ get; set; }
		public string sync_transaction_version	{ get; set; }
		public string type						{ get; set; }
		public string url						{ get; set; }
		public List<ChromeBookmark> children	{ get; set; }
		public ChromeBookmarkMetaInfo meta_info	{ get; set; }

//---------------------------------------------------------------------------------------

		public static DateTime ToDateTime(string sDate) {
			// The dates are microseconds since January 1, 1601 
			long lDate =Convert.ToInt64(sDate);
			int mics  = (int)(lDate % 1_000_000);
			long secs = lDate / 1_000_000;
			int days  = (int)(secs / 86_400);
			secs      = secs % 86_400;
			var dt     = new DateTime(1601, 1, 1) + new TimeSpan(days, 0, 0, (int)secs);
			return dt;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class ChromeBookmarkMetaInfo {
		public string last_visited			{ get; set; }
		public string last_visited_desktop  { get; set; }
	}
}
