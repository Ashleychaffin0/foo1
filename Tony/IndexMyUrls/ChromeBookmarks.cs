using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChromeBookmarksSchema {
	class ChromeBookmarks {
		public ChromeBookmarksRoot root;

//---------------------------------------------------------------------------------------

		public ChromeBookmarks(string filename = null) {
			if (filename is null) { filename = BookmarksFilename; }

			string bmJson = File.ReadAllText(filename);
			root          = JsonConvert.DeserializeObject<ChromeBookmarksRoot>(bmJson);
		}

//---------------------------------------------------------------------------------------

		public string BookmarksFilename {
			get {
				var appdata  = Environment.SpecialFolder.LocalApplicationData;
				var filename = Environment.GetFolderPath(appdata);
				filename     = Path.Combine(filename, @"Google\Chrome\User Data\Default\Bookmarks");
				return filename;
			}
		}

//---------------------------------------------------------------------------------------

		public IEnumerable<ChromeBookmark> Roots() {
			yield return root.roots.bookmark_bar;
			yield return root.roots.other;
			yield return root.roots.synced;
		}
	}
}
