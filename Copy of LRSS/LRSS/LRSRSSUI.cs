using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace LRSS {
	public class LRSRSSTreeNode {
		public RSSFeed				feed;
		public List<LRSRSSTreeNode>	children;

//---------------------------------------------------------------------------------------

		public LRSRSSTreeNode(string URL) {
			feed		= new RSSFeed(URL);
			children	= new List<LRSRSSTreeNode>();
		}

//---------------------------------------------------------------------------------------

		public LRSRSSTreeNode() {
			// Public ctor for serialization
			feed	 = null;
			children = new List<LRSRSSTreeNode>();
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
