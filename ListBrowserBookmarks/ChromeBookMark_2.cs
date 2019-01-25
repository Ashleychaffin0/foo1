using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Schema courtesy of http://json2csharp.com

namespace ChromeBookMark {

	public static class ChromeBooksMarksUtils {
		private static DateTime GetTimestamp(this string s) {
			long microseconds = long.Parse(s);
			var millis = microseconds / 1000;
			var epoch = new DateTime(1601, 1, 1);
			return epoch.AddMilliseconds(millis);
		}
	}

	public class RootObject {
		public string checksum { get; set; }
		public Roots roots { get; set; }
		public int version { get; set; }
	}

	public class Roots {
		public BookmarkBar bookmark_bar { get; set; }
		public Other other { get; set; }
		public string sync_transaction_version { get; set; }
		public Synced synced { get; set; }
	}

	public class BookmarkBar {
		public List<Bookmark_Folder_or_Url> children { get; set; }
		public string date_added { get; set; }
		public string date_modified { get; set; }
		public string id { get; set; }
		public string name { get; set; }
		public string sync_transaction_version { get; set; }
		public string type { get; set; }
	}

	public class Bookmark_List {
		public List<Bookmark_Folder_or_Url> children { get; set; }
		public string date_added { get; set; }
		public string date_modified { get; set; }
		public string id { get; set; }
		public string name { get; set; }
		public string sync_transaction_version { get; set; }
		public string type { get; set; }
	}

	public class Bookmark_Folder_or_Url : IComparable {
		public List<Bookmark_Folder_or_Url> children { get; set; }
		public string date_added { get; set; }
		public string id { get; set; }
		public MetaInfo meta_info { get; set; }
		public string name { get; set; }
		public string sync_transaction_version { get; set; }
		public string type { get; set; }
		public string url { get; set; }

		public int CompareTo(object obj) {
			// TODO: Get this working: CompareTo
			if (!(obj is Bookmark_Folder_or_Url bm)) {
				return -1;
			}
			if (@type == bm.type) {
				return this.name.CompareTo(bm.name);
			}
			if (@type == "folder") {
				return -1;
			}
			return 1;
		}
	}

	public class Other {
		public List<Bookmark_Folder_or_Url> children { get; set; }
		public string date_added { get; set; }
		public string date_modified { get; set; }
		public string id { get; set; }
		public string name { get; set; }
		public string sync_transaction_version { get; set; }
		public string type { get; set; }
	}

	public class MetaInfo {
		public string last_visited_desktop { get; set; }
	}

	public class Synced {
		public List<Bookmark_Folder_or_Url> children { get; set; }
		public string date_added { get; set; }
		public string date_modified { get; set; }
		public string id { get; set; }
		public string name { get; set; }
		public string sync_transaction_version { get; set; }
		public string type { get; set; }
	}
}
