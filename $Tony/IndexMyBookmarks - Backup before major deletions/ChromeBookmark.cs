using System;

/* Major TODOs
	* Add BEGIN / COMMIT to INSERT (https://sqlite.org/lang_transaction.html)
		* Remove Transactions on INSERT and see if it makes a difference
	* Put BEGIN at very beginning and COMMIT at end
	* Do some kind of profiling to improve performance
	* Allow ENTER in Search box to initiate the search
	* Add UI to show either search results or messages
	* Comment this sucker better, including .docx file
	* Profile it from an empty database
	* Read in JSON of Chrome bookmarks? How many years would that take???
	* Run SLOC on this
	* Delete <script> tags and their contents
	* Reinstate Msg(mm:ss -- Finished: <url>)
	* Add the option to delete all traces of this ("Cascade Delete") in the database
		and continue
*/

namespace IndexMyUrls {
	public class ChromeBookmark {
//		public string	UnescapedUrl { get; set; }
		public string	url			{ get; set; }
		public string	title		{ get; set; }
		public string	browser		{ get; set; }
		public DateTime added		{ get; set; }
		public string	fullPath	{ get; set; }
		public object	currentPath { get; set; }

//---------------------------------------------------------------------------------------

		public ChromeBookmark(string url, string title, string browser, DateTime added, string fullPath, object currentPath = null) {
			this.url         = url;
			this.title       = title;
			this.browser     = browser;
			this.added       = added;
			this.fullPath    = fullPath;
			this.currentPath = currentPath;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class BookMark2 {
		// TODO: Maybe properties?
		public string date_added				{ get; set; }
		public string id						{ get; set; }
		public string name						{ get; set; }
		public string sync_transaction_version	{ get; set; }
		public string type						{ get; set; }
		public string url						{ get; set; }

//---------------------------------------------------------------------------------------

		public BookMark2() {
			// Empty ctor -- needed?
		}
	}
}
