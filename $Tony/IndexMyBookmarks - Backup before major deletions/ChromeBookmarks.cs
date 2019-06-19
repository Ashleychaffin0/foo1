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
			root = JsonConvert.DeserializeObject<ChromeBookmarksRoot>(bmJson);
		}

//---------------------------------------------------------------------------------------

		public string BookmarksFilename {
			get {
				var appdata	 = Environment.SpecialFolder.LocalApplicationData;
				var filename = Environment.GetFolderPath(appdata);
				filename	 = Path.Combine(filename, @"Google\Chrome\User Data\Default\Bookmarks");
				return filename;
			}
		}

//---------------------------------------------------------------------------------------

		public IEnumerable<ChromeBookmark> Roots() {
			yield return root.roots.bookmark_bar;
			yield return root.roots.other;
			yield return root.roots.synced;
		}

//---------------------------------------------------------------------------------------

		public IEnumerable<ChromeBookmark> BookmarkUrls() {
			foreach (var node in Roots()) {
				foreach (var bookmark in ProcessNode(node, node.name)) {
					yield return bookmark;
				}
			}
		}

//---------------------------------------------------------------------------------------

		private IEnumerable<ChromeBookmark> ProcessNode(ChromeBookmark node, string folderPath) {
			if (node.type == "folder") {
				foreach (var child in node.children) {
					if (child.type == "url") {
						child.folder_path = folderPath;
						yield return child;
					}
					foreach (var kidInfo in ProcessNode(child, folderPath + "/" + child.name)) {
						yield return kidInfo;
					}
				}
			}
		}

//---------------------------------------------------------------------------------------

		public int CountUrls() {
			int n = 0;
			foreach (var url in BookmarkUrls()) {
				++n;
			}
			return n;
		}
	}
}
