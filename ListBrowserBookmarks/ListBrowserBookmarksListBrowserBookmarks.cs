using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using ChromeBookMark;

namespace ListBrowserBookmarks {
	public partial class ListBrowserBookmarksListBrowserBookmarks : Form {
		string WindowTitle = "List Browser Bookmarks";

//---------------------------------------------------------------------------------------

		public ListBrowserBookmarksListBrowserBookmarks() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void ListBrowserBookmarksListBrowserBookmarks_Load(object sender, EventArgs e) {
			CmbBrowser.SelectedIndex = 0;       // Chrome
		}

//---------------------------------------------------------------------------------------

		private void CmbBrowser_SelectedIndexChanged(object sender, EventArgs e) {
			switch (CmbBrowser.SelectedItem) {
			case "Chrome":
				DoChromium(@"Google\Chrome");
				break;
			case "Edge":
				DoChromium(@"Microsoft\Edge");
				break;
			case "IE":
				DoIE();
				break;
			default:
				MessageBox.Show("Internal Error - Unknown browser", WindowTitle,
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				break;
			}
		}

//---------------------------------------------------------------------------------------

		private void DoChromium(string whichBrowser) {
			var DocDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			var BmName = Path.Combine(DocDir, whichBrowser, @"User Data\Default\Bookmarks");
			this.Text = WindowTitle + " - " + BmName;
			string txt = File.ReadAllText(BmName);  // Get the contents of the bookmarks file
			TvBookmarks.Nodes.Clear();
			// Parse the bookmarks file
			var Bookmarks = JsonConvert.DeserializeObject<RootObject>(txt);
			var Node = TvBookmarks.Nodes.Add("Bookmark Bar");
			foreach (var bm in Bookmarks.roots.bookmark_bar.children) { // Folder or Url
				AddChromeNode(Node, bm);
				// Node.Nodes.Add(bm.name);
			}
			Node = TvBookmarks.Nodes.Add("Others");
			foreach (var item in Bookmarks.roots.other.children) {
				AddChromeNode(Node, item);
				// Node.Nodes.Add(item.name);
			}
#if false
			// var aaa = JsonConvert.DeserializeObject<ChromeBookMarkFile>(txt);
			var bbb = JObject.Parse(txt);
			var lst = bbb.Children().ToList();
			var ccc = bbb.GetValue("version");
			var ddd = bbb.GetValue("checksum");
			var roots = bbb.GetValue("roots");

			var json = JsonConvert.SerializeObject(new RootObject());
#endif
#if false
			// var yyy = (ChromeBookMarkFile)JsonConvert.DeserializeObject(txt);
			// var roots = yyy.ChildrenTokens;
			// JSONObject jsonObject = (JSONObject)new JSONParser().parse(txt);
#endif
		}

//---------------------------------------------------------------------------------------

		private void AddChromeNode(TreeNode Node, Bookmark_Folder_or_Url bm) {
			if (bm.type == "folder") {
				var NewNode = Node.Nodes.Add(bm.name);
				foreach (var item in bm.children) {
					AddChromeNode(NewNode, item);
				}
			} else {
				var TerminalNode = Node.Nodes.Add(bm.name);
				TerminalNode.Tag = bm;
			}
			Application.DoEvents();
		}

//---------------------------------------------------------------------------------------

		private void DoIE() {
			this.Text = WindowTitle;
			MessageBox.Show("Nonce error -- IE not currently supported", WindowTitle,
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			TvBookmarks.Nodes.Clear();
		}

//---------------------------------------------------------------------------------------

		private void TvBookmarks_AfterSelect(object sender, TreeViewEventArgs e) {
			var tv = sender as TreeView;
			if (tv.SelectedNode == null) {
				return;
			}
		   if (tv.SelectedNode.Tag != null) {
				var bm = (tv.SelectedNode.Tag as Bookmark_Folder_or_Url);
				string Url = bm.url;
				webBrowser1.Navigate(Url);
			}
		}
	}
}
