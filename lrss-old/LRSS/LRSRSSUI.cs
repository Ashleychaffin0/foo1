// Copyright (c) 2005-2006 by Larry Smith

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;

using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace LRSS {
	public class LRSRSSTreeNode {
		public RSSFeed				feed;
		public List<LRSRSSTreeNode>	children;
        public bool                 IsFeed;

        [XmlIgnore]
        public TreeNode             NodePtr;       // Back to Tree Node that points to us

//---------------------------------------------------------------------------------------

        public LRSRSSTreeNode(TreeNode BackPtr, string URL, bool IsFeed) {
			feed		= new RSSFeed(URL);
			children	= new List<LRSRSSTreeNode>();
            this.IsFeed = IsFeed;
            NodePtr     = BackPtr;
		}

//---------------------------------------------------------------------------------------

		public LRSRSSTreeNode(TreeNode BackPtr, RSSFeed feed, bool IsFeed) {
			this.feed	= feed;
			children	= new List<LRSRSSTreeNode>();
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
            IsFeed   = true;
            NodePtr  = null;
		}

//---------------------------------------------------------------------------------------

		public void AddFeedToDatabase() {
			if (! IsFeed)
				return;				// It's a category

			if (feed.FeedID > 0)
				return;				// Already on the database
			// TODO: Should we do an automatic Refresh Fields if it's on the db?

			OleDbConnectionStringBuilder	bld = new OleDbConnectionStringBuilder();
			bld.Provider = "Microsoft.Jet.OLEDB.4.0";
			bld.DataSource = LRSS.LRSSDatabaseFilename;
			OleDbConnection	conn = new OleDbConnection(bld.ConnectionString);

			// See if this feed is already on the database. This could happen if we
			// previously added this feed, but then the application (or the system)
			// crashed before we could write out the updated tree.xml file. So when
			// we try to add this feed again...
			// Note: You'd think that maybe if we couldn't write out the .xml file, that
			// there'd be no entry to try to add. But that might not be the case if we
			// were to, say, import a .opml file.
			// Note: Yes, we could use SELECT COUNT(*), but there are few enough fields
			// (and only, at most, one row) that I'm going to bring in all fields, in
			// case we want them for something. The main thing that comes to mind is
			// validating that the Title/Description fields in the RSSFeed instance we're
			// processing matches what's on the database.
			string	SQL = "SELECT * FROM tblFeeds WHERE FeedID = " + feed.FeedID.ToString();
			DataTable	dt = new DataTable();
			OleDbDataAdapter	adapt = new OleDbDataAdapter(SQL, conn);
			adapt.Fill(dt);

			string		msg;
			if (dt.Rows.Count == 1) {
				string		dbTitle, dbDesc;
				dbTitle	= (string)dt.Rows[0]["Title"];
				dbDesc	= (string)dt.Rows[0]["Description"];
				if ((dbTitle != feed.Title) || (dbDesc != feed.Description)) {
					msg = string.Format("Title and/or Description don't match for feed {0}", feed.FeedID);
					msg += string.Format("\n\nFeed Title = '{0}', Database Title = '{1}'", feed.Title, dbTitle);
					msg += string.Format("\n\nFeed Desc = '{0}', Database Desc = '{1}'", feed.Description, dbDesc);
					MessageBox.Show(msg, "LRSS - Internal Error", 
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					conn.Close();			// Don't need any more
					return;
				}
			}

			if (dt.Rows.Count > 1) {
				msg = string.Format("{0} rows found for feed {1} - Title = '{2}', Desc = '{3}'\n", 
					dt.Rows.Count, feed.FeedID, feed.Title, feed.Description);
				foreach (DataRow dr in dt.Rows) {
					msg += string.Format("\n\tFeed ID = {0}, Title = '{1}', Desc = '{2}'", 
						dr["FeedID"], dr["Title"], dr["Description"]);
				}
				MessageBox.Show(msg, "LRSS - Internal Error", 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				conn.Close();			// Don't need any more
				return;
			}

			// Not on the database. Let's add that puppy.
			OleDbCommandBuilder	cmdblder = new OleDbCommandBuilder(adapt);
			cmdblder.QuotePrefix = "[";
			cmdblder.QuoteSuffix = "]";
			// adapt.FillSchema(dt, SchemaType.Source);
			DataRow	row = dt.NewRow();
			// TODO: Must get autonum back via @@IDENTITY
			row["URL"] = feed.URL;
			row["Title"] = feed.Title;
			row["Description"] = feed.Description;
			row["PubDate"] = feed.PubDate;
			dt.Rows.Add(row);
			adapt.Update(dt);
			conn.Close();			// Don't need any more
			// TODO: Need try/catch. Also finally for conn.Close()
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
