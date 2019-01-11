// Copyright (c) 2005-2006 by Larry Smith

// Note: Add feed http://blogs.msdn.com/ericlee/rss.aspx

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

// Current main TODO:'s
//  *   Implement Refresh Feed popup
//  *   Clicking a node gets items (synchronously for now) for that node. Make
//      sure we distinguish between ones with a URL and ones without. The ones
//      with are simple. The ones without should show all their children.
//  *   Note: The above rule doesn't work awfully well if we default to show all
//      Subscribed nodes. But that's OK for now.
//  *   Make note of what nodes have already been loaded.
//	*	Make sure all database tables have relevant indexes (e.g. URL, Feed#/Item#, Seg#)

// Current main TODO:'s done
//  *   Add Subscribe form and implement it.
//  *   Get rid of items from tree.xml (in Parms).
//	*	Add Memo segmentation support on database
//	*	Add added node to main list of nodes so it will be serialized out.
//  *   Implement adding a Category

#if false
Wish List:

1 WISH: THE GUI SHOULD/MUST BE AS RESPONSIVE AS POSSIBLE!!!
2 Wish: There's a world of difference between when the entry was published (which
		 might contain a date many time zones away), and when we downloaded it. This
		 latter is *much* more relevant to viewing. So, sure, we can display the
		 publication date, but most/all of our usage will be based on the download
		 date.

TREE-RELATED:
* Import / Export OPML
* Allow sub-trees
* Adding feeds (to the appropriate part of the tree)
* Allow the same feed to be in different sub-trees (e.g. BradA, CLR, Favorites).
  Presumably this can be done if the same RSSFeed reference is used as the tag in both
  cases.

FEED-RELATED:
* Multiple threads to actually do the web-based part.
* Threads must have editable properties, such as how often to check the blog for
  updates, how long to keep the data, etc.
* SharpReader seems to check publication dates and shows the title in italics if the 
  blog entry has been updated since we first saw it. So we must hold both the original
  date, and the last updated date. Only if we view the entry should we update the 
  original date with Now().
* Copy the feeds to a database.
* Support comments?
* When the input queue has been fully processed (all worker threads (at least, those
  being used for IPQ processing) are idle), force a garbage collection, to try to
  cut down our working set.

GUI-RELATED:
* It's annoying in SharpReader when the GUI freezes while it does something (dunno 
  exactly what, but maybe updating the tree?). We probably want to monitor the mouse
  and keyboard, and only update the expensive (whatever they turn out to be) aspects
  of the GUI when the user has been idle for, say, 3 seconds (configurable, of course).
* Have an option to show only the feeds that were new (i.e. downloaded) in the past day,
  week, etc. Weekends are special.
* Use DataGridVew instead of ListView. Also use Data Binding

MULTI-USER:
* Need some kind of synchronization facility (email-based???) to let me read blogs at
  home and work.
#endif

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

#if false
Pseudocode / Design 

Function Initialize() {
	//	Set input queue (IPQ) and output queue (OPQ) to empty.
}

Function GoGetEm() {		// Called to check/get new RSS feed items
	//	Maybe set flag to say we're busy. Not really sure how this will work and if we
		really need it.
	//	Get contents of tree. Add RSS-related contents (oh hell, why not just the
		reference to the node itself) to the IPQ.
	//	Once all entries have been added to the queue, start a thread to start further
		threads. Notes: 1) We could start the threads as we add each element to the IPQ,
		but it should be quick enough (and simple) just to get them all, then start
		working. 2) I don't know what will happen if we do ThreadPool.QueueUserWorkItem
		and there are no more threads. Hopefully it will just add it to some internal
		queue. But maybe it'll block. Check this out. ThreadPool.QueueUserWorkItem();
	//	Note:
			ThreadPool.QueueUserWorkItem(new WaitCallback(BGLoad [, object State]));
			static void BGLoad(object stateInfo) { ... }
	//	Note: There may already be entries on the IPQ, left over from last time, if the
		user has configured too short a requery time. So always add to the queue, never
		reinitialize it.
	//	Note: Of course, whenever any routine references any queue, we must lock it. The
		C# lock statement will be acceptable.
	//	Note: If we do a subscribe in the middle (or hey, any time), we must add it to 
		the IPQ. But must think through exactly how we make sure some background thread
		knows how to process it. Again, as above, I'm concerned about issuing a call to
		QueueUserWorkItem and having it block. 
}

Function WorkerThread() {
	//	Assume each thread will do database work. So during thread initialization, open
		a connection to the database.
	//	Loop while the IPQ isn't empty, doing the following
	//	If IPQ is empty, close the database connection and exit. Note: I'm not going to
		try to optimize (yet) by reusing a single per-thread WebRequest.
	//	Process an entry.
		//	Get entire RSS feed from the web. If error (e.g. timeout), throw(???).
		//	Parse it into entries. If error (e.g. not valid XML), throw(???).
		//	For each entry, generate a primary key. Look it up in the database.
			//	If found and timestamp matches, do nothing and return.
			//	If found and timestamp doesn't match, set "Updated Feed" flag in entry.
				Update the database with the flag and the current date/time. Add to OPQ.
			//	If not found, set current date/time and add to database and OPQ.
	//	Remove item from IPQ.
}

Function UIMonitor() {
	//	Haven't fully thought this through yet, but the basic idea is that we don't want
		to update the GUI with an expensive operation unless the user has been idle for
		a certain (configurable) period (e.g. 5 seconds).
	//	When the application has been idle (no keyboard, no mouse clicks) for
		<n> (configurable) seconds, call a user-supplied delegate.
	//	ctor(int nMilliseconds, object user_supplied_parm, delegate ...)
	//	Note: Hopefully we can use Application.AddFilter to get all messages for all windows
		and check them for mouse/keyboard messages.
	//	Note: Originally I thought to have this class check for "idle and work-to-do", but
		that latter concept clearly is the application's responsibility.
	//	Also need some way to disable/enable the timer. We don't really want to call the
		user delegate every (say) 5 seconds if the application is totally quiescent. For
		example, if the user notices that both the IPQ and OPQ are empty, and no background
		threads are active, then it can disable this monitor until it decides (e.g. via its
		own 1 hour timer) that things are active again.
}
#endif

namespace LRSS {
	public partial class LRSS : Form {

		const string	ParmsFileName = "LRSSParms.xml";
		LRSSParms		Parms;
		LRSSTreeNodes	TreeNodes;
		TreeNode		CurrentClickedNode;
        TreeNode        RootNode;

        List<LRSSTreeNodes> IPQ;                // Input queue
        // TODO: Output queue

		// TODO: Move to Parms??? Nah.
        public static string	LRSSDatabaseFilename = "LRSS.mdb";

        public bool bInitOK = true;

//---------------------------------------------------------------------------------------

		public LRSS() {
			InitializeComponent();

			string filename = FindFileUpInHierarchy(LRSSDatabaseFilename);
			if (filename == null) {
				MessageBox.Show("Program must terminate. Unable to find database file, " + LRSSDatabaseFilename,
					"LRSS Initialization", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				bInitOK = false;
				// TODO: <return> doesn't work. Must set flag and check in mainline
				//		 Or move to Form_Loading, and then do Application.Exit()?
				return;
			}
			LRSSDatabaseFilename = filename;

            filename = FindFileUpInHierarchy(ParmsFileName);
            if (filename == null) {
                MessageBox.Show("Program must terminate. Unable to find parms file, " + ParmsFileName, 
                    "LRSS Initialization", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                bInitOK = false;
				// TODO: <return> doesn't work. Must set flag and check in mainline
				//		 Or move to Form_Loading, and then do Application.Exit()?
                return;
            }

            Parms = GenericSerialization<LRSSParms>.Load(filename);
			// I'd like to do the following. But no. Still, you'd think it could/should work.
			// Parms = GenericSerialization<typeof(Parms)>.Load(ParmsFileName);
			if (Parms == null)
				Parms = new LRSSParms();

            filename = FindFileUpInHierarchy(Parms.TreeFileName);
            if (filename == null) {
            }

            LoadTree(filename);
		}

//---------------------------------------------------------------------------------------

		private void LoadTree(string FileName) {
			TreeNodes = GenericSerialization<LRSSTreeNodes>.Load(FileName);
			RootNode = new TreeNode("Subscribed Feeds");
			tvRSS.Nodes.Add(RootNode);
			// Make sure the root node has an LRSRSSTreeNode tag too
            LRSRSSTreeNode RootTag = new LRSRSSTreeNode(RootNode);
            RootTag.IsFeed = false;
			foreach (LRSRSSTreeNode node in TreeNodes.nodes) {
                AddTreeNode(RootNode, node);
				RootTag.children.Add(node);
			}
			RootNode.Tag = RootTag;
            RootNode.ExpandAll();
		}

//---------------------------------------------------------------------------------------


        private void AddTreeNode(TreeNode CurNode, LRSRSSTreeNode node) {
			TreeNode	tNode;

			// node.feed = new RSSFeed(node.feed.URL);		// TODO: ???
			tNode = new TreeNode(node.feed.Title);
			tNode.Tag = node;

			CurNode.Nodes.Add(tNode);

            foreach (LRSRSSTreeNode kid in node.children) {
                AddTreeNode(tNode, kid);
            }
        }

//---------------------------------------------------------------------------------------

		private void LRSS_FormClosing(object sender, FormClosingEventArgs e) {
            string filename;
            // TODO: In principle (I suppose), these files may have moved, or been 
            //       created, in different places while this program was running.
            //       So you should (arguably) save the FindFileUp... filenames during
            //       initialization, and reuse them here. 
            filename = FindFileUpInHierarchy(ParmsFileName);
            GenericSerialization<LRSSParms>.Save(filename, Parms);
            filename = FindFileUpInHierarchy(Parms.TreeFileName);

            // GenericSerialization<LRSSTreeNodes>.Save(filename, TreeNodes);
            // TODO: There's *gotta* be a cleaner/easier way to do this...
            LRSSTreeNodes nodes = new LRSSTreeNodes();
            foreach (TreeNode node in RootNode.Nodes) {
                nodes.nodes.Add((LRSRSSTreeNode)node.Tag);
            }
            GenericSerialization<LRSSTreeNodes>.Save(filename, nodes);
		}

//---------------------------------------------------------------------------------------

		private void tvRSS_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
			CurrentClickedNode = e.Node;
            if (e.Button == MouseButtons.Right) {
                RightClickOnNode((TreeView)sender, e);
                return;
            }

			if (CurrentClickedNode.Tag == null)
				return;				// If no tag, then not a feed, just a parent node
			LRSRSSTreeNode	tn = (LRSRSSTreeNode)CurrentClickedNode.Tag;
            if (! tn.IsFeed)
                return;
            tn.feed.Load();
			lvItems.Items.Clear();
			// lvItems.Items.Add(new ListViewItem(tn.feed.Description));
			ListViewItem	lvItem;
			foreach (RSSItem item in tn.feed.Items) {
				lvItem = new ListViewItem(new string[] { item.Title, item.Description, item.pubDate.ToString("MM/dd/yy HH:mm") } );
				lvItem.Tag = item;
				lvItems.Items.Add(lvItem);
				
				// lvItems.Items.Add(item.Description);
			}
		}

//---------------------------------------------------------------------------------------

        private void RightClickOnNode(TreeView treeView, TreeNodeMouseClickEventArgs e) {
            contextMenuStrip1.Show();
        }

//---------------------------------------------------------------------------------------

		private void lvItems_Click(object sender, EventArgs e) {
			ListViewItem	item = lvItems.SelectedItems[0];
			RSSItem			rssItem = (RSSItem)item.Tag;
			web.Navigate(rssItem.Link);
		}

//---------------------------------------------------------------------------------------

        private void mnuFile_Refresh_Click(object sender, EventArgs e) {
            foreach (TreeNode node in tvRSS.Nodes) {
                ProcessNode(node);
            }
        }

//---------------------------------------------------------------------------------------

        private void ProcessNode(TreeNode node) {
            Console.WriteLine(node.Text);   // TODO: Process it
            foreach (TreeNode ChildNode in node.Nodes) {
                ProcessNode(ChildNode);
            }
        }

//---------------------------------------------------------------------------------------

        private void popupAddFeed_Click(object sender, EventArgs e) {
            frmAddRSSFeed dlgAddFeed = new frmAddRSSFeed();
            DialogResult res = dlgAddFeed.ShowDialog();
            if (res != DialogResult.OK)
                return;

            // We're trying to add this feed
            // TODO: Finish this (partial implementation follows)
			// TODO: Check if feed already exists at the same level.
			TreeNode	tNode = new TreeNode(dlgAddFeed.feed.Title);
            LRSRSSTreeNode node = new LRSRSSTreeNode(tNode, dlgAddFeed.feed, true);
			tNode.Tag = node;
			CurrentClickedNode.Nodes.Add(tNode);
			// Add to the main <TreeNodes> collection (so it will be written out)
			// TODO: We currently don't make the distinction between a category node
			//		 and a feed node. We need to do so. For now, assume the user (moi)
			//		 hasn't screwed things up, and is adding to a category node.
			LRSRSSTreeNode	CurRSSNode = (LRSRSSTreeNode)CurrentClickedNode.Tag;
			CurRSSNode.children.Add(node);

        }

//---------------------------------------------------------------------------------------

        private void popupAddCategory_Click(object sender, EventArgs e) {
            frmAddRSSCategory dlgAddCat = new frmAddRSSCategory();
            DialogResult res = dlgAddCat.ShowDialog();
            if (res != DialogResult.OK)
                return;

            // We can only add a category as a child of another category. If we've 
            // clicked on a feed node, go up a level to get to its category.
            TreeNode CurCatNode = CurrentClickedNode;
            LRSRSSTreeNode CurRSSNode = (LRSRSSTreeNode)CurCatNode.Tag;
            if (CurRSSNode.IsFeed) {
                CurRSSNode = (LRSRSSTreeNode)CurCatNode.Parent.Tag;
            }
            // CurRSSNode now refers to a category node

            // TODO: Make sure the Title isn't the same as any other at this level

            RSSFeed feed = new RSSFeed();
            feed.Title = dlgAddCat.txtTitle.Text;
            feed.Description = dlgAddCat.txtDescription.Text;

            // Add a new node to the tree, with its Tag
            TreeNode newNode = new TreeNode(feed.Title);
            LRSRSSTreeNode newRSSNode = new LRSRSSTreeNode(newNode, feed, false);
            newNode.Tag = newRSSNode;
            CurCatNode.Nodes.Add(newNode);

            // Now hook the category in with the main serialized class
            LRSRSSTreeNode xxx = (LRSRSSTreeNode)CurCatNode.Tag;
            xxx.children.Add(newRSSNode);
        }

//---------------------------------------------------------------------------------------

        string FindFileUpInHierarchy(string filename) {
            // TODO: Need overload - with and without default starting directory
            string path = Application.StartupPath + @"\";
            bool bFound = false;

            // TODO: Not 6. Should stop when it gets to merely a drive letter
            for (int i = 0; i < 6; ++i) {				// Six levels should be enough!
                if (File.Exists(path + filename)) {
                    bFound = true;
                    break;
                } else {
                    filename = @"..\" + filename;
                }
            }
            if (!bFound) {
                return null;
            }
            FileInfo fi = new FileInfo(filename);
            return fi.FullName;
        }

//---------------------------------------------------------------------------------------

        private void popupRefreshFeed_Click(object sender, EventArgs e) {
            MessageBox.Show("Refresh Feed not yet implemented");     // TODO:
        }

//---------------------------------------------------------------------------------------

		private void deleteFeedToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("Delete Feed not yet implemented.");     // TODO:
		}

//---------------------------------------------------------------------------------------

        private void popupDeleteCategory_Click(object sender, EventArgs e) {
            MessageBox.Show("Delete Category not yet implemented.");     // TODO:
        }

//---------------------------------------------------------------------------------------

        private void popupProperties_Click(object sender, EventArgs e) {
            MessageBox.Show("Properties not yet implemented.");     // TODO:
        }
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	[Serializable]
	public class LRSSParms {
		public string				TreeFileName;

		public LRSSParms() {
			TreeFileName = "LRSTree.xml";
		}
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

}