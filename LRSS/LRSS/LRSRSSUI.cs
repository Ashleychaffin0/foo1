// Copyright (c) 2005-2006 by Larry Smith

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Text;

using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace LRSS {

	/// <summary>
	/// Contains additional information for a node in the tree. Each TreeNode.Tag field
	/// contains a reference to an instance of this class.
	/// </summary>
	public class LRSRSSTreeNode {
		public RSSFeed				feed;
		public List<LRSRSSTreeNode>	children;
        public bool                 IsFeed;

        [XmlIgnore]
        public TreeNode             NodePtr;       // Back to Tree Node that points to us

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Constructor for the LRSRSSTreeNode class. 
		/// </summary>
		/// <param name="BackPtr">Points to the TreeNode that pointed to this instance.
		/// </param>
		/// <param name="URL">The URL for the node. Empty if this is a Category. A new
		/// feed instance will be created, based on this URL.</param>
		/// <param name="IsFeed">bool - Whether the node is a feed or not.</param>
        public LRSRSSTreeNode(TreeNode BackPtr, string URL, bool IsFeed) {
			feed		= new RSSFeed(URL);
			children	= new List<LRSRSSTreeNode>();
            this.IsFeed = IsFeed;
            NodePtr     = BackPtr;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Constructor for the LRSRSSTreeNode class.
		/// </summary>
		/// <param name="BackPtr">Points to the TreeNode that pointed to this instance.
		/// </param>
		/// <param name="feed">An already constructed feed.</param>
		/// <param name="IsFeed">bool - Whether the node is a feed or not.</param>
		public LRSRSSTreeNode(TreeNode BackPtr, RSSFeed feed, bool IsFeed) {
			this.feed   = feed;
			children    = new List<LRSRSSTreeNode>();
            this.IsFeed = IsFeed;
            NodePtr     = BackPtr;
		}

//---------------------------------------------------------------------------------------

        public LRSRSSTreeNode(TreeNode BackPtr)
            : this() {
            NodePtr = BackPtr;
        }

//---------------------------------------------------------------------------------------

		public LRSRSSTreeNode() {
			// Public ctor for serialization
			feed	 = null;
			children = new List<LRSRSSTreeNode>();
            IsFeed   = false;
            NodePtr  = null;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Retrieves all items for the given feed (unless the feed is a category). Adds
		/// all new items to the database.
		/// </summary>
		/// <param name="feed"></param>
		public void GetItemsFromInternet(RSSFeed feed) {
			if (! IsFeed)
				return;				// It's a category

			// Set feed.FeedID, adding the feed if necessary
			SetFeedID(feed);
			
			List<RSSItem> Items = feed.Load();				// Refresh the feed
			AddItemsToDatabase(Items);
		}

//---------------------------------------------------------------------------------------

		private void SetFeedID(RSSFeed feed) {
			if (feed.FeedID >= 0) 	// Must be second call. We've already found ourself.
				return;

			using (RssPersist DB = new RssPersist()) {
				try {
					DB.GetFeedInfo(feed);
				} catch (Exception ex) {
					string msg = "An unexpected error ({0}) occurred while processing the"
						+ " feed for\n\nURL:\t\t{1}\n\nTitle:\t\t{2}\n\nDescription:\t{3}";
					msg = string.Format(msg, ex.Message, feed.URL, feed.Title, feed.Description);
					MessageBox.Show(msg, "LRSS Error",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

//---------------------------------------------------------------------------------------

		internal void AddItemsToDatabase(List<RSSItem> Items) {
			using (RssPersist DB = new RssPersist()) {
				DB.AddItemsToDatabase(feed, Items);
			}
		}
	}



//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// Handles all interactions with the ListView pane.
	/// </summary>
	public class LRSSListView {
		ListView	lv;
		TreeView	tvRSS;
		LRSS		lrss;
		WebBrowser	web;

		// For an optimization in ShowItemsForNode
		int LastFeedID = -1;
		LRSRSSTreeNode LastRssNode = null;

		delegate void delSetFeedColor(LRSRSSTreeNode node, Color ForeColor);

//---------------------------------------------------------------------------------------

		public LRSSListView(ListView lv, TreeView tvRSS, WebBrowser web, LRSS lrss) {
			this.lv		= lv;
			this.tvRSS	= tvRSS;
			this.web	= web;
			this.lrss	= lrss;
		}

//---------------------------------------------------------------------------------------

		internal void SetFeedColor(LRSRSSTreeNode node, Color ForeColor) {
			// Log("Feed refreshed for {0}", node.feed.URL);
			if (tvRSS.InvokeRequired) {
				delSetFeedColor del = delegate(LRSRSSTreeNode RSSNode, Color FgColor) {
					SetFeedColor(RSSNode, FgColor);
				};
				tvRSS.BeginInvoke(del, node, ForeColor);
				return;
			} else {
				node.NodePtr.ForeColor = ForeColor;
			}
		}


//---------------------------------------------------------------------------------------

		internal void SetNodeTitle(LRSRSSTreeNode tn) {
			int Read, Total, Unread;
			string Title = tn.NodePtr.Text;

			Read = tn.feed.cntItemsRead;
			Total = tn.feed.cntItemsTotal;
			Unread = Total - Read;
			int n = Title.LastIndexOf('(');
			if (n >= 0) {
				Title = Title.Substring(0, n) + string.Format("({0},{1})", Unread, Total);
				tn.NodePtr.Text = Title;
			}
			Application.DoEvents();
		}

//---------------------------------------------------------------------------------------

		internal void SetNodeTitleCounts(LRSRSSTreeNode node, RssPersist DB, TreeNode tNode) {
			int cntTotalItems, cntReadItems;

			DB.GetFeedCounts(node.feed.FeedID, out cntTotalItems, out cntReadItems);
			node.feed.cntItemsTotal = cntTotalItems;
			node.feed.cntItemsRead = cntReadItems;
			int cntUnreadItems = cntTotalItems - cntReadItems;
			tNode.Text += string.Format(" ({0}/{1})", cntUnreadItems, cntTotalItems);
		}

//---------------------------------------------------------------------------------------

		internal void ShowItemsForNode(LRSRSSTreeNode tn) {
			LRSRSSTreeNode tn2;
			DataTable Results = new DataTable();
			using (RssPersist DB = new RssPersist()) {
				DB.GetLinksFromDatabase(tn, Results);
			}

			int count = Results.Rows.Count;
			string msg = string.Format("ShowItemsForNode for LRSRSSTreeNode {0}, OPQ length={1}, Count = {2}",
				tn.NodePtr.Text, lrss.OPQ.Count, count);
			lrss.Log(Color.Cyan, "Starting " + msg);
			// Note: If we've clicked on a node, and there's nothing to show, we
			//		 must erase the listbox anyway.
			lv.Items.Clear();
			if (count == 0) {		// May have no items within the horizon, so don't
				return;				//	waste time updating the listbox.
			}
			ListViewItem lvItem;
			foreach (DataRow row in Results.Rows) {
				int FeedID;
				string ItemTitle, ItemDescription, FeedTitle;
				DateTime PubDate, DownloadDate;
				string sPubDate, sDownloadDate;
				bool bRead;
				short Flag;

				FeedID = (int)RSSItem.GetNonDBNull(row["FeedID"], -1);
				ItemTitle = (string)RSSItem.GetNonDBNull(row["FeedItemTitle"], "");
				ItemDescription = (string)RSSItem.GetNonDBNull(row["FeedItemDescription"], "");
				FeedTitle = (string)row["FeedTitle"];
				PubDate = (DateTime)RSSItem.GetNonDBNull(row["FeedItemPubDate"], DateTime.Now);
				DownloadDate = (DateTime)RSSItem.GetNonDBNull(row["DownloadDate"], LRSS.GlobalDownloadDate);
				sPubDate = PubDate.ToString("MM/dd/yy HH:mm");
				sDownloadDate = DownloadDate.ToString("MM/dd/yy HH:mm");
				bRead = (bool)RSSItem.GetNonDBNull(row["bRead"], false);
				Flag = (short)RSSItem.GetNonDBNull(row["Flag"], 0);

				// TODO: Keep <item.SubItems[2]> in synch with below
				string[] gRow = new string[] { 
					ItemTitle, sPubDate, FeedTitle, sDownloadDate, 
					Flag < 0 ? "" : Flag.ToString()
				};

				lvItem = new ListViewItem(gRow);
				Pair<LRSRSSTreeNode, RSSItem> p;
				// Do a bit of caching. See if this FeedID is the same as the previous
				if (FeedID == LastFeedID) {
					tn2 = LastRssNode;
					Debug.Assert(tn != null, "tn == null at " + Environment.StackTrace);
				} else {
					tn2 = RSSNodeFromFeedID(lrss.RootNode, FeedID);
					LastFeedID = FeedID;
					LastRssNode = tn2;
					Debug.Assert(tn != null, "tn == null at " + Environment.StackTrace);
				}
				p = new Pair<LRSRSSTreeNode, RSSItem>(tn2, new RSSItem(row));
				lvItem.Tag = p;			// TODO: Isn't this too much overhead.?
				if (bRead) {
					lvItem.Font = lrss.fntLviRead;
				} else {
					lvItem.Font = lrss.fntLviUnread;
				}

				if (lrss.Favorites.ContainsKey((string)row["URL"])) {		// TODO: Optimize by ColumnNumber. Also check for dbNull
					lvItem.BackColor = lrss.FavoritesBGColor;
				}
				lv.Items.Add(lvItem);
			}
			lrss.Log(Color.Cyan, "Ending " + msg);
			Application.DoEvents();
		}

//---------------------------------------------------------------------------------------

		internal void ShowItemsForNode(TreeNode tn) {
			ShowItemsForNode((LRSRSSTreeNode)tn.Tag);
		}

//---------------------------------------------------------------------------------------

		internal LRSRSSTreeNode RSSNodeFromFeedID(TreeNode tn, int FeedID) {
			// Note: We'll return the first matching node we find, even if there are dups
			foreach (TreeNode child in tn.Nodes) {
				LRSRSSTreeNode node = (LRSRSSTreeNode)child.Tag;
				if (node.IsFeed) {
					if (node.feed.FeedID == FeedID) {
						return node;
					}
				} else {			// It's a category
					node = RSSNodeFromFeedID(child, FeedID);
					if (node != null) {
						return node;
					}
				}
			}
			return null;
		}

//---------------------------------------------------------------------------------------

		public void ShowArticle(ListViewItem item) {

			item.Font = lrss.fntLviRead;
			Pair<LRSRSSTreeNode, RSSItem> p = (Pair<LRSRSSTreeNode, RSSItem>)item.Tag;
			LRSRSSTreeNode tn = p.First;
			RSSItem rssItem = p.Second;

			RSSFeed feed = tn.feed;

			if (feed != null) {
				using (RssPersist DB = new RssPersist()) {
					DB.SetRssItemReadField(feed.FeedID, rssItem.FeedItemID, true);
				}
			}
			// Now update the text of the feed.
			// TODO: Make common routine with somewhere else
			if (!rssItem.bRead) {			// Don't count again if already read
				++feed.cntItemsRead;
				rssItem.bRead = true;
			}
			SetNodeTitle(tn);

			// web.Navigate("about:blank");
			// HtmlDocument	doc = web.Document;

			// doc.BackColor = Color.White;
			// doc.ForeColor = Color.Black;
			// web.Document.Window.Alert("Hi world");

			// web.DocumentText = "<HTML><HEAD><Title>Hi world</Title><BODY BackColor=\"Red\"></BODY></HTML>";

			StringBuilder HTML = new StringBuilder();
			// TODO: For BradA, for Feed:, add <a href="http://blogs.msdn.com/brada/default.aspx"
			HTML.AppendFormat("<TABLE bgcolor=\"#cccccc\" WIDTH=\"100%\" BORDER=\"1\">\n");
			HTML.Append("<TR ALIGN=\"LEFT\">\n");
			if (feed.ImageURL != null) {
				// if ((feed.Width > 0) && (feed.Height > 0)) {
				Uri baseUri = new Uri(feed.URL);
				string basename = baseUri.Scheme + "://" + baseUri.Host;
				HTML.AppendFormat("<TD><A HREF=\"{0}\">", basename);
				//					HTML.AppendFormat("<IMG SRC=\"{0}\" WIDTH=\"{1}\" HEIGHT=\"{2}\"></A>&nbsp;\n", feed.ImageURL, feed.Width, feed.Height);
				HTML.AppendFormat("<IMG SRC=\"{0}\"></A>&nbsp;\n", feed.ImageURL);
				// }
			}
			// TODO: Keep <item.SubItems[2]> in synch with above
			HTML.AppendFormat("<b>Feed:</b>&nbsp;{0}\n", item.SubItems[2].Text.Trim());	// TODO:
			HTML.AppendFormat("<BR><b>Title:</b>&nbsp;<A HREF=\"{0}\">{1}</A>\n", rssItem.Link, rssItem.Title);
			HTML.AppendFormat("</TD></TR></TABLE>\n");
			HTML.Append("\n<TABLE bgcolor=\"#ffffff\" WIDTH=\"100%\" cellpadding=\"2\" cellspacing=\"2\" border=\"0\"><TR><TD>");
			HTML.Append(rssItem.Description);
			HTML.Append("</TD></TR></TABLE>");
			// web.Refresh();
			string outFileName = @"C:\LRSSFoo.html";		// TODO:
			StreamWriter outFile = File.CreateText(outFileName);
			outFile.Write(HTML.ToString());
			outFile.Close();
			web.Navigate(outFileName);
			//doc.Body.InnerHtml = HTML.ToString();
			//web.Navigate(rssItem.Link);

#if false	// Sample SharpReader HTML
	<body>
		<table width="100%" bgcolor="#cccccc" cellpadding="0" cellspacing="0" border="0">
			<tr><td>
				&nbsp;<a href="http://blogs.msdn.com/brada/default.aspx" class="black"><b>Feed: </b>Brad Abrams </a><br/>
				&nbsp;<a href="http://blogs.msdn.com/brada/archive/2006/03/27/562600.aspx" class="black"><b>Title: </b>Build your own map Mashup: step by step example</a>
			</td><td align="right">
				<b>Author: </b>BradA&nbsp;<br/>
				<a href="http://blogs.msdn.com/brada/comments/562600.aspx" class="black">0 Comments</a>&nbsp;
			</td></tr>
			<tr bgcolor="#666666" height="1"><td colspan="2"></td></tr>
		</table>
		<table width="100%" cellpadding="2" cellspacing="2" border="0"><tr><td>

<P class=MsoNormal deactivatedstyle="MARGIN: 0in 0in 0pt"><FONT face=Arial color=#8f3b39 size=2>I 
#endif
		}
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	[Serializable]
	public class LRSSTreeNodes {
		public List<LRSRSSTreeNode> nodes;

//---------------------------------------------------------------------------------------

		public LRSSTreeNodes() {
			nodes = new List<LRSRSSTreeNode>();
		}
	}
}
