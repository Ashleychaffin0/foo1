// Copyright (c) 2005-2006 by Larry Smith
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

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

//---------------------------------------------------------------------------------------

		public LRSS() {
			InitializeComponent();

			Parms = GenericSerialization<LRSSParms>.Load(ParmsFileName);
			// I'd like to do the following. But no. Still, you'd think it could/should work.
			// Parms = GenericSerialization<typeof(Parms)>.Load(ParmsFileName);
			if (Parms == null)
				Parms = new LRSSParms();
			LoadTree(Parms.TreeFileName);
		}

//---------------------------------------------------------------------------------------

		private void LoadTree(string FileName) {
			TreeNodes = GenericSerialization<LRSSTreeNodes>.Load(FileName);
			TreeNode	tNode;
			TreeNode	RootNode = new TreeNode("Subscribed Feeds");
			tvRSS.Nodes.Add(RootNode);
			foreach (LRSRSSTreeNode node in TreeNodes.nodes) {
				node.feed = new RSSFeed(node.feed.URL);		// TODO: ???
				tNode = new TreeNode(node.feed.Title);
				tNode.Tag = node;
				RootNode.Nodes.Add(tNode);
				// TODO: Add children
			}
		}

//---------------------------------------------------------------------------------------

		private void LRSS_FormClosing(object sender, FormClosingEventArgs e) {
			GenericSerialization<LRSSParms>.Save(ParmsFileName, Parms);
			GenericSerialization<LRSSTreeNodes>.Save(Parms.TreeFileName, TreeNodes);
		}

//---------------------------------------------------------------------------------------

		private void tvRSS_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
			TreeNode	node = e.Node;
			if (node.Tag == null)
				return;				// If no tag, then not a feed, just a parent node
			LRSRSSTreeNode	tn = (LRSRSSTreeNode)node.Tag;
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

		private void lvItems_Click(object sender, EventArgs e) {
			ListViewItem	item = lvItems.SelectedItems[0];
			RSSItem			rssItem = (RSSItem)item.Tag;
			web.Navigate(rssItem.Link);
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
}