// Copyright (c) 2005-2006 by Larry Smith

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;


namespace LRSS {
	/// <summary>
	/// A class meant to handle reading and writing RSS-related data to disk. In 
	/// principle, it could be to a flat file, to a series of flat files, to one or
	/// many XML files, and so on. In this case, we persist to/from an Access database.
	/// <para>
	/// Note that one of the guiding principles here is that if we ever change our minds,
	/// and want to use, say, a set of XML files, the caller should (except for a one-time
	/// conversion) be unaware that the internals of this class have changed.
	/// </para>
	/// <para>
	/// Oh all right. Maybe we have to pass in a config-file based database name. But 
	/// other than matters like that, this class is meant to be quite, quite opaque.
	/// </para>
	/// <para>
	/// Note: Yes, in at least one case, we return a DataTable. But that's OK. There's
	/// nothing that says we can't persist to, say, XML, but present the data to the user
	/// as a DataTable.
	/// </para>
	/// </summary>
	class RssPersist : IDisposable {
		OleDbConnection	conn;

//---------------------------------------------------------------------------------------

		public RssPersist() {
			Open();					// We seem to always want this done
		}

//---------------------------------------------------------------------------------------
	
		/// <summary>
		/// Opens a connection on a database. Note something subtle but important. 
		/// Because ADO.NET connection pooling is currently based on an exact match on
		/// the connection string, by having the string built here and nowhere else,
		/// with no options, guarantees us proper connection pooling. And with all the
		/// concurrency going on, this can easily be a significant performance booster.
		/// </summary>
		public void Open() {
			// TODO: Should we check to see if it's already open, and if so just return?
			OleDbConnectionStringBuilder bld = new OleDbConnectionStringBuilder();
			bld.Provider = "Microsoft.Jet.OLEDB.4.0";
			// Note: We pick up the database name automagically.
			bld.DataSource = LRSS.LRSSDatabaseFilename;
			conn = new OleDbConnection(bld.ConnectionString);
			conn.Open();
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Given a feed instance, locate it on the database via its URL, and set several
		/// feed properties. However, if the feed isn't found on the database, add it.
		/// The Autonum will be its ID, and the other properties will be set to their
		/// default values. Note: If the URL is empty, the feed instance represents a
		/// category, not an actual feed.
		/// </summary>
		/// <param name="feed"></param>
		public void GetFeedInfo(RSSFeed feed) {
			if (feed.URL == "")			// Quick test for Category
				return;

			string SQL = "SELECT FeedID, cntRead, cntTotal, Favorite FROM tblFeeds WHERE URL = @URL";
			OleDbCommand cmd = new OleDbCommand(SQL, conn);
			cmd.Parameters.Add(new OleDbParameter("URL", feed.URL));
			OleDbDataReader	rdr = cmd.ExecuteReader();
			if (rdr.HasRows) {
				rdr.Read();					// Uh, actually read the data!
				feed.FeedID			= (int)rdr["FeedID"];
				feed.cntItemsRead	= (int)rdr["cntRead"];
				feed.cntItemsTotal	= (int)rdr["cntTotal"];
				// TODO: Should this be on the database, or on the LRSTree.xml file?
				//		 For now, put it on the XML
				// feed.Favorite		= (bool)rdr["Favorite"];
				rdr.Close();
				return;
			}

			// Feed not found. Let's add that puppy.
			rdr.Close();
			DataTable dt = new DataTable();
			cmd = new OleDbCommand("tblFeeds", conn);
			cmd.CommandType = CommandType.TableDirect;
			OleDbDataAdapter adapt = new OleDbDataAdapter(cmd);
			adapt.Fill(dt);
			adapt.RowUpdated += new OleDbRowUpdatedEventHandler(adapt_FeedRowUpdated);

			OleDbCommandBuilder cmdblder = new OleDbCommandBuilder(adapt);
			cmdblder.QuotePrefix = "[";
			cmdblder.QuoteSuffix = "]";
			// adapt.FillSchema(dt, SchemaType.Source);
			DataRow row				= dt.NewRow();
			row["URL"]				= feed.URL;
			row["FeedTitle"]		= feed.Title;
			row["FeedDescription"]	= feed.Description;
			row["FeedPubDate"]		= feed.PubDate;
			row["cntRead"]			= 0;
			row["cntTotal"]			= 0;
			dt.Rows.Add(row);
			adapt.Update(dt);
			feed.FeedID = (int)row["FeedID"];
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Given a feed and a list of items, add those items to the database. Any items
		/// already on the database will be left unchanged. The feed properties 
		/// cntItemsToal and cntItemsRead will be refreshed/updated.
		/// </summary>
		/// <param name="feed"></param>
		/// <param name="Items"></param>
		public void AddItemsToDatabase(RSSFeed feed, List<RSSItem> Items) {
			string SQL = "SELECT * FROM tblFeedItems WHERE FeedID = " + feed.FeedID.ToString();
			// Note: Could add, if needed: + " ORDER BY PubDate DESC";
			DataTable dt = new DataTable();
			OleDbDataAdapter adapt = new OleDbDataAdapter(SQL, conn);
			adapt.Fill(dt);
			adapt.RowUpdated += new OleDbRowUpdatedEventHandler(adapt_ItemRowUpdated);

			// OK. We now have two arrays, feed.Items and the row(s) from the database.
			// We need to essentially do a merge on them, to find out what items have
			// been added (the most likely case), modified (the PubDate field has
			// changed) or deleted.
			// Note: We *could* sort both the database items and the items list (say
			//		 by PubDate). But the expected number of items is small enough
			//		 that I'll live with the n^2 algorithm, rather than two n*log(n)
			//		 sorts. And hey, we can always change our algorithm here, 
			//		 if necessary.

#if false	// Sort the feed items (not needed, but interesting code)
			// We don't really need to sort the feed items, but I thought I'd do so
			// just to find out how to use Comparison<T> with an anonymous delegate.
			feed.Items.Sort(new Comparison<RSSItem>(delegate(RSSItem x, RSSItem y) {
				// PubDates are usually on the feed as most recent first. So we want to
				// do a descending sort on the dates. Hence the return values are
				// "backwards" to a standard ascending sort.
				if (x.PubDate < y.PubDate)
					return 1;
				if (x.PubDate > y.PubDate)
					return -1;
				return 0;
			}));
#endif

			DataRow dr;
			// We modify <item> in the subroutines we call, so we can't use foreach
			// foreach (RSSItem item in feed.Items) {
			RSSItem item;
			feed.cntItemsTotal = Items.Count;
			feed.cntItemsRead  = 0;
			for (int i = 0; i < Items.Count; i++) {
				item = Items[i];
				if (item.bRead) {
					++feed.cntItemsRead;
				}
				dr = FindMatchingFeedRow(item, dt);
				if (dr == null) {
					AddItemToDatabase(feed, item, adapt, dt);
				}
			}
#if false
			// Datarow accesses are more expensive than List<T> accesses, so run
			// through the datarows first.
			// TODO: Uh, except that what makes more sense 
			RSSItem		FoundItem;
			foreach (DataRow dr in dt.Rows) {
				FoundItem = FindMatchingFeedItem(dr);
				if (FoundItem == null) {
					// Didn't find it. Must add.
					// TODO:
				}
			}
#endif
			conn.Close();
		}

//---------------------------------------------------------------------------------------

		private void AddItemToDatabase(RSSFeed feed, RSSItem item, OleDbDataAdapter adapt, DataTable dt) {
			DataRow	row;
			try {
				// Not on the database. Let's add that puppy.
				OleDbCommandBuilder cmdblder = new OleDbCommandBuilder(adapt);
				cmdblder.QuotePrefix = "[";
				cmdblder.QuoteSuffix = "]";
				// adapt.FillSchema(dt, SchemaType.Source);
				row = dt.NewRow();
				row["FeedID"]				= feed.FeedID;
				row["DownloadDate"]			= LRSS.GlobalDownloadDate;
				row["FeedItemTitle"]		= item.Title ?? "*No Title*";
				row["FeedItemDescription"]	= item.Description ?? "*No Description*";
				row["FeedItemPubDate"]		= item.PubDate;
				row["Link"]					= item.Link;
				row["bRead"]				= item.bRead;
				row["Flag"]					= (short)-1;
				row["FeedItemLen"]			= item.ItemBody.Length;
				// I've put in the feature to capture the entire feed item (all the
				// HTML, etc) (e.g. so you can capture a blog, even if the author (for
				// some reason) later deletes it. But it absolutely balloons the size
				// of the database. So we'll comment it out. If anyone wants it, maybe
				// we can add it in later as a user option.
				// row["FeedItem"] = item.ItemBody;
				dt.Rows.Add(row);
				adapt.Update(dt);
				item.FeedItemID				= (int)row["FeedItemID"];
				// We've written the ItemBody out to the database. It can be big
				// (10's of K's). Free up the space.
				item.ItemBody = null;
				// Also, we don't need this row in the DataTable any more. Get rid
				// of it.
				dt.Rows.Clear();
			} catch (OleDbException ex) {
				// TODO: This code is the same as in the Exception handler below.
				//		 We want to figure out what's going on with all our 
				//		 concurrency errors.
				// Y'know, at this point, I don't really care that much why the add 
				// failed. Ignore any errors.
				string	msg = string.Format("Exception '{0}' in AddItemToDatabase. Stack Trace:\n{1}", ex.Message, ex.StackTrace);
				Trace.WriteLine(msg);
				dt.Rows.Clear();		// Else we'd keep getting errors on this row
			} catch (Exception ex) {
				// Y'know, at this point, I don't really care that much why the add 
				// failed. Ignore any errors.
				string	msg = string.Format("Exception '{0}' in AddItemToDatabase. Stack Trace:\n{1}", ex.Message, ex.StackTrace);
				Trace.WriteLine(msg);
				dt.Rows.Clear();		// Else we'd keep getting errors on this row
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Updates the bRead property for an item, in the database.
		/// </summary>
		/// <param name="FeedID"></param>
		/// <param name="FeedItemID"></param>
		/// <param name="bRead"></param>
		public void SetRssItemReadField(int FeedID, int FeedItemID, bool bRead) {
			string	SQL = string.Format("UPDATE tblFeedItems SET bRead = {0}" 
				+ " WHERE FeedID = {1} AND FeedItemID = {2}", bRead, FeedID, FeedItemID);
			OleDbCommand	cmd = new OleDbCommand(SQL, conn);
			cmd.ExecuteNonQuery();		// Returns # of items updated, if you care.
		}

//---------------------------------------------------------------------------------------

		private DataRow FindMatchingFeedRow(RSSItem item, DataTable dt) {
			foreach (DataRow dr in dt.Rows) {
				if ((item.PubDate == (DateTime)dr["FeedItemPubDate"]) &&	// Should be
					(item.Title == (string)dr["FeedItemTitle"])			// unique enough
				   ) {
					// Note: We could implement a <bFoundOnDatabase> field in each item,
					//		 set here when a match is found. This would let us do a
					//		 subsequent scan to find deleted records.
					return dr;
				}
			}
			return null;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Given an LRSRSSTreeNode, return a DataTable filled with all relevant records
		/// for that node. Note that if the node is a Category, return data for all 
		/// child nodes (including grandchildren, etc).
		/// </summary>
		/// <param name="node"></param>
		/// <param name="dt"></param>
		public void GetLinksFromDatabase(LRSRSSTreeNode node, DataTable dt) {
			try {
				string SQL;
				// Note: Special processing for root.
				// Note: Rather than Select'ing * below, keep memory usage down by 
				//		 Select'ing just the fields we need.
				string Selects, From, Where, OrderBy;
				Selects = "SELECT FeedItemTitle, FeedItemPubDate, DownloadDate, Link, tblFeedItems.FeedID, FeedItemID, FeedTitle, FeedItemDescription, bRead, Flag, tblFeeds.URL";
				From = "FROM tblFeeds INNER JOIN tblFeedItems ON tblFeeds.FeedID = tblFeedItems.FeedID";
				OrderBy = "ORDER BY DownloadDate DESC, FeedItemPubDate DESC, tblFeedItems.FeedID ASC";
				if (!node.IsFeed) {
					if (node.NodePtr.Parent == null) {
						// This is the root, and it's easy. Get everything.
						Where = "";
					} else {
						// This is a category, but not the root. We've got a bit more
						// work to do here.
						List<int> KidIds = new List<int>();
						GetChildNodeIDs(node.NodePtr, KidIds);	// TODO: Bad Method name
						StringBuilder sb = new StringBuilder("WHERE tblFeeds.FeedID IN (");
						// string.Join() ?
						bool bCommaNeeded = false;
						foreach (int id in KidIds) {
							if (bCommaNeeded) {
								sb.Append(",");		// Small note: Keep SQL length down
								//   by having ",", not ", "
							}
							bCommaNeeded = true;
							sb.Append(id.ToString());
						}
						sb.Append(")");
						Where = sb.ToString();
					}
				} else if (node.feed.FeedID >= 0) {
					Where = "WHERE (tblFeeds.FeedID = " + node.feed.FeedID.ToString() + ")";
				} else {
					// TODO: MsgBox or something?
					MessageBox.Show("Feed not yet initialized. Try later.", "LRSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}

				// TODO: Show just relatively recent stuff. Make a parm

				string		WhereSuffix;
				int			CutoffDateInterval = LRSS.CutoffDateInterval;
				DateTime CutoffDate = DateTime.Now.AddDays(-CutoffDateInterval);	
				WhereSuffix = string.Format(" (FeedItemPubDate >= #{0:d}#)", CutoffDate);
				if (Where == "") {
					Where = "WHERE " + WhereSuffix;
				} else {
					Where += " AND " + WhereSuffix;
				}

				SQL = string.Format("{0}\n{1}\n{2}\n{3}\n", Selects, From, Where, OrderBy);
				OleDbDataAdapter adapt = new OleDbDataAdapter(SQL, conn);

#if false
				Clipboard.SetData(DataFormats.Text, SQL);
				OleDbCommand cmd = new OleDbCommand(SQL, conn);
				OleDbDataReader rdr = cmd.ExecuteReader();
				Console.WriteLine();
				while (rdr.Read()) {
					Console.WriteLine("Title={0}, DLDate={1}, PubDate={2}", rdr["FeedItemTitle"], rdr["DownloadDate"], rdr["FeedItemPubDate"]);
				}
#endif

				dt.Rows.Clear();
				adapt.Fill(dt);

			} catch (Exception ex) {
				MessageBox.Show("Exception in GetLinksFromDatabase - " + ex.Message); // TODO:
			}
		}

//---------------------------------------------------------------------------------------

		private void GetChildNodeIDs(TreeNode node, List<int> IDs) {
			TreeNode tnBase = node;
			LRSRSSTreeNode rssNode;
			foreach (TreeNode tn in tnBase.Nodes) {
				rssNode = (LRSRSSTreeNode)tn.Tag;
				if (rssNode.IsFeed) {
					IDs.Add(rssNode.feed.FeedID);
				} else {
					GetChildNodeIDs(tn, IDs);
				}
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// For the given Feed ID, return the total number of items, and the number read.
		/// </summary>
		/// <param name="FeedID"></param>
		/// <param name="cntTotal"></param>
		/// <param name="cntRead"></param>
		public void GetFeedCounts(int FeedID, out int cntTotal, out int cntRead) {
			string		SQL;

			SQL = "SELECT COUNT(*) FROM tblFeedItems WHERE FeedID = " + FeedID.ToString();
			OleDbCommand	cmd = new OleDbCommand(SQL, conn);
			cntTotal = (int)cmd.ExecuteScalar();	

			SQL += " AND bRead = true";
			cmd = new OleDbCommand(SQL, conn);
			cntRead = (int)cmd.ExecuteScalar();
	
			SQL = "UPDATE tblFeeds\n"
				+ " SET cntRead = @cntRead,\n"
				+ "     cntTotal = @cntTotal\n"
				+ " WHERE FeedID = @FeedID";
			cmd = new OleDbCommand(SQL, conn);
			cmd.Parameters.AddWithValue("@cntTotal", cntTotal);
			cmd.Parameters.AddWithValue("@cntRead", cntRead);
			cmd.Parameters.AddWithValue("@FeedID", FeedID);
			int nRows = cmd.ExecuteNonQuery();
			// TODO: Check for nRows == -1
		}

//---------------------------------------------------------------------------------------

		public void Close() {
			if ((conn != null) && (conn.State == ConnectionState.Open))
				conn.Close();
			conn = null;
		}

//---------------------------------------------------------------------------------------

        private void adapt_FeedRowUpdated(object sender, OleDbRowUpdatedEventArgs e) {
			int?	id = GetAutonum(sender, e);
			if (id.HasValue) {
				e.Row["FeedID"] = id;
			}
        }

//---------------------------------------------------------------------------------------

		private void adapt_ItemRowUpdated(object sender, OleDbRowUpdatedEventArgs e) {
			int? id = GetAutonum(sender, e);
			if (id.HasValue) {
				e.Row["FeedItemID"] = id;
			}
		}

//---------------------------------------------------------------------------------------

		private int? GetAutonum(object sender, OleDbRowUpdatedEventArgs e) {
			int? id = null;
			if (e.StatementType == StatementType.Insert) {
				// Only after a row was inserted successfully into a table do we
				// retrieve back the new generated auto number field value.
				OleDbDataAdapter da = (OleDbDataAdapter)sender;
				OleDbCommand cmd = new OleDbCommand("SELECT @@IDENTITY", e.Command.Connection);
				object ret = cmd.ExecuteScalar();
				if (ret != DBNull.Value) {
					id = Convert.ToInt32(ret);
				}
			}
			return id;
		}

		#region IDisposable Members

//---------------------------------------------------------------------------------------

		public void Dispose() {
			Close();
		}

		#endregion

//---------------------------------------------------------------------------------------

		internal void SetFavorite(RSSFeed Feed) {
			string	SQL = "UPDATE tblFeeds\n"
						+ "   SET Favorite=@Favorite\n"
						+ " WHERE FeedID=@FeedID";
			OleDbCommand	cmd = new OleDbCommand(SQL, conn);
			cmd.Parameters.AddWithValue("@Favorite", Feed.Favorite);
			cmd.Parameters.AddWithValue("@FeedID", Feed.FeedID);
			int	nRows = cmd.ExecuteNonQuery();
			// TODO: Something if nRows == -1
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// We've had concurrency errors updating the database. I've searched, and its not
	/// clear if it's a bug in the Access engine or not. So we're going to try to get
	/// around it by queueing all database writes, and handling them from a dedicated
	/// thread. This class represents the write request.
	/// TODO: I think we'll just lock() on a field and use that instead.
	/// </summary>
	internal class DbWriteRequest {

		RowType		Rowtype;
		DataRow		dr;

//---------------------------------------------------------------------------------------
		
		/// <summary>
		/// We need to know if this is a write to tblFeeds or tblFeedItems
		/// </summary>
		internal enum RowType {
			Feed,						// RSSFeeds
			Item						// RSSFeedItems
		}

//---------------------------------------------------------------------------------------

		internal DbWriteRequest() {
			// TODO:
		}
	}
}
