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

		// Originally, when enumerting url's, I was returning a tuple with the url and
		// the folder path. Then I realized I might as well return the entire bookmark
		// instead of just the url, just in case I ever found a need for the other
		// fields. Finally, I could eliminate the need for the tuple (and thus simplify
		// the code) if I included the folder path in the bookmark itself.

		// So here it is. Yeah, there's the potential danger that someday a field by
		// this name will be used by Chrome. But that doesn't bother me. In particular,
		// this would only modify the in-memory copy of the bookmarks. It might be
		// and issue if I ever used this to write out a modified version of the file,
		// perhaps to fix broken links, to auto-sort the folders, whatever. But we'll
		// cross that bridge if/when we come to it. Anyway, either...
		//		a)	the field will contain info that I don't need/want and I'm free to
		//			overwrite it
		//		b)	it's got the same information (perhaps in a different format that I'd
		//			have to convert)
		//		c)	It's got information that I find useful, in which case I can just
		//			rename the field here and dance around the problem later.
		// In all cases, it won't be a big deal. So here's our field
		public string folder_path				{ get; set; }

		// See comments above for folder_path
		public DateTime date_added_dt	 => ToDateTime(date_added);
		public DateTime date_modified_dt => ToDateTime(date_modified);

//---------------------------------------------------------------------------------------

		private static DateTime ToDateTime(string sDate) {
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
