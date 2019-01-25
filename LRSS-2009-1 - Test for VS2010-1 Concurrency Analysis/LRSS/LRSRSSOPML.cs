// Copyright (c) 2005-2006 by Larry Smith

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

#if false	// Sample OPML data:
    <outline 
			type="rss" title="Scientific American - Official RSS Feed" 
			description="Science news and technology updates from Scientific American" 
			xmlUrl="http://www.sciam.com/xml/sciam.xml" 
			htmlUrl="http://www.sciam.com/" />
    <outline title="Microsoft Speech Team">
      <outline 
			type="rss" 
			title="All the cool developers use Speech APIs" 
			description="You want to be cool, don't you?  ;-)" 
			xmlUrl="http://blogs.msdn.com/robertbrown/rss.aspx" 
			htmlUrl="http://blogs.msdn.com/robertbrown/default.aspx" />
#endif

namespace LRSS {

	/// <summary>
	/// Processes OPML files. TODO: Really shouldn't update the tree directly.
	/// </summary>
	public class OPML {
		XmlDocument _xdoc = null;
		Exception _LastException = null;

//---------------------------------------------------------------------------------------

		public XmlDocument Xdoc {
			get { return _xdoc; }
			private set { _xdoc = value; }
		}

//---------------------------------------------------------------------------------------

		public Exception LastException {
			get { return _LastException; }
			private set { _LastException = value; }
		}

//---------------------------------------------------------------------------------------

		public OPML() {
		}

//---------------------------------------------------------------------------------------

		public bool Import(TreeNode CurrentClickedNode) {
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.InitialDirectory = Application.StartupPath;	// TODO: My Docs???
			ofd.Title = "Select OPML 1.0 file to import";
			ofd.Filter = "OPML files (*.opml)|*.opml|All files (*.*)|*.*";
			ofd.RestoreDirectory = true;
			ofd.CheckFileExists = true;

			if (ofd.ShowDialog() != DialogResult.OK) {
				return false;
			}

			Load(ofd.FileName);		// Get the data
			using (RssPersist DB = new RssPersist()) {
				AddToTree(CurrentClickedNode, DB);
				// Application.DoEvents();
				// TODO: The interface seems to freeze when we're importing. You'd
				//		 think that Application.DoEvents() would help, but apparently
				//		 not here (may have something to do with tvRSS.BeginUpdate()).
				//		 CurrentClickedNode.ExpandAll() seems to help, but with a big
				//		 import, it can cause the tree/screen to flicker like crazy.
				//		 But for now...
				CurrentClickedNode.ExpandAll();
			}
			return true;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Loads an OPML file into an XmlDocument.
		/// </summary>
		/// <param name="filename">The name of the file to load.</param>
		/// <returns>bool - Whether it was a valid XML file or not.</returns>
		bool Load(string filename) {
			try {
				Xdoc = new XmlDocument();
				_xdoc.Load(filename);
				return true;
			} catch (Exception ex) {
				_LastException = ex;
				return false;
			}
		}

//---------------------------------------------------------------------------------------

		private void AddOPMLNodes(TreeNode node, XmlNodeList xnl, RssPersist DB) {
			// Note: We're guaranteed that <node> is a category node, not a feed node
			string			title, description, url;
			TreeNode		newNode;
			LRSRSSTreeNode	rssNode;
			Dictionary<string, string> dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			LRSRSSTreeNode	baseRssNode = (LRSRSSTreeNode)node.Tag;
			int				cntTotalItems, cntReadItems;

			foreach (XmlNode xnode in xnl) {
				dict.Clear();
				GetAttrs(xnode, dict);
				// Get some common values. Note that this processing might be wasted if,
				// say, this is a category (and may have no Description), or if the type
				// of feed node isn't "rss" (so the whole thing is ignored. But hey, this
				// is extrememly low overhead, and if it simplifies the code, it's well
				// worth it.
				// Reset the values each time through
				title = description = url = null;
				dict.TryGetValue("title", out title);
				if ((title == null) || (title.Trim() == ""))
					title = "";
				dict.TryGetValue("description", out description);
				if ((description == null) || (description.Trim() == ""))
					description = "";
				dict.TryGetValue("xmlurl", out url);

				newNode = new TreeNode(title);
				// See if this is a category or a feed. A category is defined as one
				// without a URL.
				if (url == null) {
					rssNode = new LRSRSSTreeNode(newNode);		// Category
				} else {
					RSSFeed feed = new RSSFeed(url);
					feed.Title = title;
					feed.Description = description;
					rssNode = new LRSRSSTreeNode(newNode, feed, true);
					DB.GetFeedInfo(feed);
					DB.GetFeedCounts(rssNode.feed.FeedID, out cntTotalItems, out cntReadItems);
					int		cntUnreadItems = cntTotalItems - cntReadItems;
					newNode.Text += string.Format(" ({0}/{1})", cntUnreadItems, cntTotalItems);
				}
				newNode.Tag = rssNode;
				node.Nodes.Add(newNode);
				baseRssNode.children.Add(rssNode);

				if (url == null) {
					// It's a category. Recurse.
					AddOPMLNodes(newNode, xnode.ChildNodes, DB);
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void GetAttrs(XmlNode xnode, Dictionary<string, string> dict) {
			foreach (XmlAttribute attr in xnode.Attributes) {
				// TODO: Remove .ToUpper() after we see if we've got case
				//		 insensitivity on the dict working OK.
				dict[attr.Name] = attr.Value as string;
			}
		}

//---------------------------------------------------------------------------------------

		private TreeNode CategoryNodeFromNode(TreeNode node) {
			LRSRSSTreeNode rsNode = (LRSRSSTreeNode)node.Tag;
			if (rsNode.IsFeed)
				return node.Parent;
			else
				return node;
		}

//---------------------------------------------------------------------------------------

		internal void AddToTree(TreeNode CurrentClickedNode, RssPersist DB) {
			// So what we need to do here is
			//	*	Prompt the user to see if we should import this as a child
			//		of the current node (or its parent, if it isn't itself a category).
			//		TODO: For now, make it a child. Maybe later we'll offer a "Promote
			//			  all entries at this level to children of the next higher
			//			  level" option.

			TreeNode catNode = CategoryNodeFromNode(CurrentClickedNode);

			XmlNodeList xnl;
			xnl = Xdoc.GetElementsByTagName("body");
			AddOPMLNodes(catNode, xnl[0].ChildNodes, DB);
		}
	}
}
