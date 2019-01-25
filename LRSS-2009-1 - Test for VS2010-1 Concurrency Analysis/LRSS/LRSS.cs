// Copyright (c) 2005-2006 by Larry Smith

// Note: Add feed http://blogs.msdn.com/ericlee/rss.aspx

#if	false	// From Q304103: How To Change the Font Size in a WebBrowser Control That Is Hosted Inside a Visual Basic Application. 
// http://support.microsoft.com/kb/304103
You can use ExecWB and pass the OLECMDID_ZOOM command as the cmdID parameter to retrieve the current value of the zoom level. Zoom level refers to the current font size of the text that Internet Explorer shows; it is equivalent to the value that is specified as Text Size on the View menu of Internet Explorer.

After you retrieve the current value of the font size, you can set it to a different value. The default value should be 2, which corresponds with Medium in the graphical user interface menu. Because the option allows both 2 levels greater and 2 levels smaller, the valid range for the zoom level is 0 through 4, in which 0 is smallest and 4 is largest. The OLECMDID_GETZOOMRANGE command returns the valid range for the font size, which should be from 0 to 4.

The following steps demonstrate how to add this functionality into your custom browser: 1. Open a new standard EXE project in Visual Basic 6.0. Form1 is created by default. 
2. Add Microsoft Internet Controls into your component lists. 
3. Add a WebBrowser control and three command buttons to Form1, and use the default names. 
4. Copy and paste the following code in Form1:Private Sub Form_Load()
    WebBrowser1.Navigate "http://www.microsoft.com"
    Command1.Caption = "Get current font size"
    Command2.Caption = "Decrease font size"
    Command3.Caption = "Increase font size"
End Sub


Private Sub Command1_Click()

Dim Z As Variant     'Z is the value to hold the zoom level.
WebBrowser1.ExecWB OLECMDID_ZOOM, OLECMDEXECOPT_DONTPROMPTUSER, Null, Z

MsgBox "The current font size is " & Z

End Sub

Private Sub Command2_Click()

Dim Z As Variant     'Z is the value to hold the zoom level.
WebBrowser1.ExecWB OLECMDID_ZOOM, OLECMDEXECOPT_DONTPROMPTUSER, Null, Z

If Z > 0 Then
    Z = Z - 1
Else
    Z = 0
End If

WebBrowser1.ExecWB OLECMDID_ZOOM, OLECMDEXECOPT_DONTPROMPTUSER, Z, Null
End Sub

Private Sub Command3_Click()

Dim Z As Variant     'Z is the value to hold the zoom level.
WebBrowser1.ExecWB OLECMDID_ZOOM, OLECMDEXECOPT_DONTPROMPTUSER, Null, Z

If Z < 4 Then
    Z = Z + 1
Else
    Z = 4
End If

WebBrowser1.ExecWB OLECMDID_ZOOM, OLECMDEXECOPT_DONTPROMPTUSER, Z, Null
End Sub
#endif

// The following define can be used to limit the thread pool to a single thread. This
// can help debugging, especially single-stepping, immensely.
// #define SINGLE_THREAD

// Check out http://thevalerios.net/matt/2008/07/controlinvokerequired-delegatemarshaler-and-anonymous-methods/
/*
public class DelegateMarshaler {
	private SynchronizationContext _synchronizationContext = null;

	public static DelegateMarshaler Create() {
		 if (SynchronizationContext.Current == null) {
			throw new InvalidOperationException("No SynchronizationContext exists for the current thread.");
		}
		return new DelegateMarshaler(SynchronizationContext.Current);
	}
	private DelegateMarshaler(SynchronizationContext synchronizationContext) {
		_synchronizationContext = synchronizationContext;
	}

	private bool IsMarshalRequired {
		get {
			return _synchronizationContext != SynchronizationContext.Current;
		}
	}

	public void Invoke(Action action) {
		if (!this.IsMarshalRequired) {
			action();
		} else {
			_synchronizationContext.Send(o => action(), null);
		}
	}
}
*/

// Usage of above
/*
private void btnStart_Click(object sender, EventArgs e) {
    DelegateMarshaler marshaler = DelegateMarshaler.Create();
    ThreadPoolHelper.QueueUserWorkItem(() => {
            marshaler.Invoke(() => {
                    txtOutput.AppendText("Starting long-running computation…");
                    txtOutput.AppendText(Environment.NewLine);
                });

            for (int i = 0; i < 100; i++) {
                marshaler.Invoke(() => {
						txtOutput.AppendText(String.Format("Percent: {0}", i));
						txtOutput.AppendText(Environment.NewLine);
						prgProgress.Value = i;
					}
				);
                Thread.Sleep(50);
            }
            marshaler.Invoke(() => {
                txtOutput.AppendText("Done!");
                txtOutput.AppendText(Environment.NewLine);
            });
        });
}
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using System.Xml.Serialization;

/*
 * Note from TestStronglyTypedDatasets:
 * 			// Note: See blog at http://blogs.msdn.com/smartclientdata/archive/2005/08/26/456886.aspx\
			//		 for how to change the |DataDirectory| macro in app.config.
			AppDomain.CurrentDomain.SetData("DataDirectory", @"G:\LRSDevel\C#\TestStronglyTypedDatasets\TestStronglyTypedDatasets\");
			adaptFeeds = new TestStronglyTypedDatasets.LRSSDataSetTableAdapters.tblFeedsTableAdapter();
			tblFeeds = new LRSSDataSet.tblFeedsDataTable();
*/

// Bugs
//	*	Feed www.dotnetindia.com/index.rss had a bad DateTime. Also
//		www.wired.com/news_drop/netcenter/netcenter.rdf
//	*	blogs.msdn.com/deeptandshuv/rss.aspx -- "Could not update; currently locked". Is
//		that feed on there twice?
//	*	Feed starts/stops don't log.

// Current main TODO:'s
//	*	Node counts need work.
//		*	Add counts to RssFeed, and use TreeView.Paint (or whatever) to colorize
//		*	Update tree when item status changes to/from Read/Unread
//		*	Update counts (if necessary) after refresh on node
//		*	Support counts in Category nodes
//		*	Maybe add 3rd column to counts to include items beyond the horizon?
//		*	Doesn't add counts to tree when importing from OPML
//	*	Update lvItems when event comes in (under control of UIMgr). Later, optimize
//		to not update if feed results wouldn't update the UI anyway (e.g. no new items,
//		or feed for node that isn't a child of what we're currently viewing)
//	*	To status line, add count of active (green) threads. Also max(green).
//	*	Add context menu to ListView items rows to give Mark as Read/Unread, maybe Print, etc.
//		Also, of course, for Flag 0-9. Flag category???
//	*	DownloadDate per Session. Uh, not quite. If we keep LRSS running constantly,
//		I don't want a DownLoadDate of last week. But for now...
//	*	Log XML errors (with full info, such as traceback), but don't MsgBox.
//	*	OPML export. 
//	*	Make sure all database tables have relevant indexes (e.g. URL, Feed#/Item#, Seg#)
//	*	private AutoResetEvent m_inputEvent
//	*	Node.ExpandAll/CollapseAll on popup (rename to ctxFeed) menu.
//	*	Figure out why this program runs *much* more slowly with UD Agent running. See
//		http://windowssdk.msdn.microsoft.com/en-us/library/ms681622.aspx and
//		http://windowssdk.msdn.microsoft.com/en-us/library/ms681418.aspx
//		for Vista Wait Chain Traveral (WCT) API info.
//	*	Counts: Actually need 4 of them; Total/Read, and Total/Read within horizon

// Things to put into Parms
//	*	Red/Green/Black colors for feed tree
//	*	Horizon (how many days back to display data)

// Current main TODO:'s done
//	*	Send msgs to the status bar from non-GUI threads. Include (at least)
//			Start/Stop msgs, 
//			Feed title/desc, 
//			<n> titles left (use InterlockedIncrement/Decrement).
//  *   Add Subscribe form and implement it.
//  *   Get rid of items from tree.xml (in Parms).
//	*	Add Memo segmentation support on database
//	*	Add added node to main list of nodes so it will be serialized out.
//  *   Implement adding a Category
//	*	OPML import.
//	*	DataGridView - Decided not to use it. Too memory intensive
//  *   Implement Refresh Feed popup

// Minor TODO:'s
//	*	Tree manipulation (rename node=feed/category + update tree.xml); drag-and-drop
//		nodes (and update tree.xml appropriately)
//	*	On a per-feed basis, keep track of the Last Date an Item was Read, so we can
//		perhaps prune the tree.

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

	/// <summary>
	/// The main class for this program.
	/// </summary>
	public partial class LRSS : Form {

		const string		ParmsFileName = "LRSSParms.xml";
		LRSSParms			Parms;
		LRSSTreeNodes		TreeNodes;
		internal TreeNode	CurrentClickedNode;
		internal TreeNode	RootNode;

		public static string LRSSDatabaseFilename = "LRSS.mdb";

		string				TraceFileName = "LRSSTrace.html";

		UIMonitor			UIMon;

		ToolStripLabel		tsl = new ToolStripLabel();	// TODO: Give name

		int					OutstandingRefreshes = 0;

		internal Queue<LRSRSSTreeNode>	OPQ = new Queue<LRSRSSTreeNode>();// OutPut Queue

		delegate void delFeedRefreshed(LRSRSSTreeNode node);

		event delFeedRefreshed				evtFeedRefreshed;	// Also, new elem on OPQ

		public static DateTime				GlobalDownloadDate;

		// Lvi = ListViewItem
		internal Font						fntLviRead, fntLviUnread;	

		internal Dictionary<string, int>	Favorites;
		internal Color						FavoritesBGColor;

		// TODO: Make parms
		public static int CutoffDateInterval = 20;	// In last 20 days
		int			MaxThreads;
		int			MaxIOCompPorts;					// We don't use this

		LRSSListView	lv;							// For handling this part of the UI

//---------------------------------------------------------------------------------------

		public LRSS() {
			InitializeComponent();

			SetupLogging();

			evtFeedRefreshed += new delFeedRefreshed(FeedRefreshed);

			GlobalDownloadDate = DateTime.Now;

			fntLviRead = lvRssItems.Font;
			fntLviUnread = new Font(lvRssItems.Font, FontStyle.Bold);

			Favorites = new Dictionary<string, int>();
			// TODO: Get color from some kind of config file
			FavoritesBGColor = Color.Gold;

			lv = new LRSSListView(lvRssItems, tvRSS, web, this);

#if SINGLE_THREAD
			MaxThreads			= 1;
#else
			MaxThreads			= 50;
#endif
			int MaxIOCompPorts	= 10;		// TODO: Use this, maybe?
		}

//---------------------------------------------------------------------------------------

		private void FeedRefreshed(LRSRSSTreeNode node) {
			if (tvRSS.InvokeRequired) {
				delFeedRefreshed del = new delFeedRefreshed(FeedRefreshed);
				tvRSS.BeginInvoke(del, node);
				return;
			}

			Log(Color.Indigo, "Entered FeedRefreshed - OPQ count = (approx) {0}", OPQ.Count);
			LRSRSSTreeNode tn;
			using (RssPersist DB = new RssPersist()) {
				while (true) {
					lock (OPQ) {
						if (OPQ.Count == 0) {
							break;
						}
						tn = OPQ.Dequeue();
						Log(Color.Indigo, "Just dequeued {0}", tn.NodePtr.Text);
						// TODO: Need GetFeedCounts? Or is it in tn.feed already???
						// DB.GetFeedCounts(tn.feed.FeedID, out cntTotal, 
						//		out cntRead);
						lv.SetNodeTitle(tn);
						// This feed (one that's now finished) might (or might not)
						// affect the current display. The cases are:
						//	1) This node (tn) == CurrentClickedNode. Refresh Items.
						//	2) This node is a child of CurrentNodeClicked. Refresh 
						//		Items.
						//	3) Otherwise it's a node that's not being displayed. 
						//		Ignore.
						// TODO: For now, always refresh current clicked node
					}
				}
			}
			// TODO: Later optimize. Don't refresh if it wouldn't affect the tree.
			//		 For example, if the elements are too old. Also, don't refresh
			//		 the treeuntil the UI Manager says we can. But for now...
			// TODO: Must remember what node was last Refresh'ed, so that we can
			//		 refresh just it. For now, steal code from tvRSS_NodeMouseClick
			//		 to show the entire tree (and should still use the UI Manager).
			// TODO: The concept of the "last node Refresh'ed" isn't quite correct
			//		 since we could re-refresh while a refresh was going on. Maybe
			//		 just always refresh the whole tree, and yet again under control
			//		 of the UI Manager.

			UIMon.CallWhenUIBecomesIdle(ShowItemsForCurrentNode);
			// ShowItemsForNode(CurrentClickedNode);
			// TODO: Uh, wrong. Don't redisplay CurrentClickedNode in the tree.
			//		 Redisplay the current node (not that we've defined it yet)
			//		 in the Items ListView.
#if false
			if (CurrentClickedNode != null) {
				CurrentClickedNode.EnsureVisible();
			}
#endif
			// Log(Color.Indigo, "Exited FeedRefreshed");
		}

//---------------------------------------------------------------------------------------

		delegate void delSetStripText(Color color, string fmt, object[] objs);

//---------------------------------------------------------------------------------------

		/// <summary>
		/// A simple cover function for the main Log routine, which supplies a default
		/// Color.Black parameter.
		/// </summary>
		/// <param name="fmt">See the main Log description.</param>
		/// <param name="objs">See the main Log description.</param>
		public void Log(string fmt, params object[] objs) {
			Log(Color.Black, fmt, objs);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Writes a record to the logging (class Trace) file. The output is expected to
		/// go to a .html file, so that color and other formatting can be applied. Each
		/// line is prefixed with a timestamp.
		/// </summary>
		/// <param name="color">A Color.xxx value, so that you can highlight/distinguish
		/// certain messages from others.</param>
		/// <param name="fmt">A string.Format formatting string. The parameter and/or the
		/// following objs parameter can contain HTML tags for formatting purposes.</param>
		/// <param name="objs">A params object [] set of values to be formatted.</param>
		public void Log(Color color, string fmt, params object[] objs) {
			if (statusStrip1.InvokeRequired) {
				delSetStripText del = new delSetStripText(Log);
				statusStrip1.BeginInvoke(del, color, fmt, objs);
				return;
			}

			int workerThreads, completionPortThreads;
			ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
			string msg = string.Format(fmt, objs);
			msg = string.Format("AT={0} of {1} - {2}", workerThreads, MaxThreads, msg);
			// TODO: I've seen some blank lines in the status bar. It seems that if
			//		 the line is too long, instead of truncating it, they blank out
			//		 the entire status strip item! Figure out what to do about this 
			//		 later.
			// TODO: Maybe it's wrapping, which is why it seems totally blank.
			tsl.Text = msg;
			Application.DoEvents();
			msg = string.Format("<p><FONT COLOR=\"{0}\">{1} {2}</FONT></p>", color.Name, 
				DateTime.Now, msg);
			Trace.WriteLine(msg);
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

			lv.ShowItemsForNode(CurrentClickedNode);
		}

//---------------------------------------------------------------------------------------

		private void ShowItemsForCurrentNode() {
			lv.ShowItemsForNode(CurrentClickedNode);
		}


//---------------------------------------------------------------------------------------

		private void RightClickOnNode(TreeView treeView, TreeNodeMouseClickEventArgs e) {
			contextMenuStrip1.Show();
		}

//---------------------------------------------------------------------------------------

		private void lvItems_Click(object sender, EventArgs e) {
			ListViewItem item = lvRssItems.SelectedItems[0];
			lv.ShowArticle(item);
		}

//---------------------------------------------------------------------------------------

		private void RefreshNode(TreeNode node, bool bSetGlobalDownloadDate) {
			if (bSetGlobalDownloadDate)
				GlobalDownloadDate = DateTime.Now;
			// We now want to optionally set a wait cursor, and disable updating the
			// tree (since we may no a *lot* of updating if there are a lot of nodes).
			// But since we recurse, we only want to do this once. So we'll use the
			// bSetGlobalDownloadDate flag to also indicate that this is the highest
			// level of recursion.
			if (bSetGlobalDownloadDate) {
				Cursor = Cursors.WaitCursor;
			}
			try {
				RefreshTreeNode(node);
			} finally {
				if (bSetGlobalDownloadDate) {
					tvRSS.EndUpdate();
					Cursor = Cursors.Default;
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void RefreshAllFeeds() {
			RefreshNode(RootNode, true);
		}

//---------------------------------------------------------------------------------------

		private void RefreshTreeNode(TreeNode node) {
			LRSRSSTreeNode tn = (LRSRSSTreeNode)node.Tag;
			// Log("Processing node {0}", node.FullPath);
			if (tn.IsFeed) {
				// Note: Increment before queueing, rather than in thread itself. This
				//		 way we get to see the number of feeds left to be processed,
				//		 in a way that is independent of the number of threads in the
				//		 thread pool.
				node.ForeColor = Color.Red;
				Interlocked.Increment(ref OutstandingRefreshes);
				Log(Color.Green, "Queueing Feed: Outstanding={0}, Node={1}, Title={2}, Desc={3}",
					OutstandingRefreshes, node.FullPath, tn.feed.Title, tn.feed.Description);
				ThreadPool.QueueUserWorkItem(ProcessBGFeed, node.Tag);
			}
			foreach (TreeNode child in node.Nodes) {
				RefreshTreeNode(child);
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// This is the main routine in each worker thread. It gets the feed data from
		/// the web, then puts it onto the output queue.
		/// </summary>
		/// <param name="o">The LRSRSSTreeNode to process.</param>
		private void ProcessBGFeed(object o) {
			LRSRSSTreeNode node = o as LRSRSSTreeNode;
			int TID = Thread.CurrentThread.ManagedThreadId;
			Thread.CurrentThread.Name = node.feed.URL;
			lv.SetFeedColor(node, Color.Blue);
			Stopwatch sw = new Stopwatch();
			sw.Start();

			Log(Color.Red, "BG mgThreadID={0}, Outstanding={1}, Start processing node {2}",
				TID, OutstandingRefreshes, node.NodePtr.Text);
			node.GetItemsFromInternet(node.feed);

			sw.Stop();
			lock (OPQ) {
				OPQ.Enqueue(node);
			}
			lv.SetFeedColor(node, Color.Black);
			Interlocked.Decrement(ref OutstandingRefreshes);
			TimeSpan Elapsed = new TimeSpan(sw.Elapsed.Hours, sw.Elapsed.Minutes, sw.Elapsed.Seconds);
			Log(Color.Blue, "BG mgThreadID={0},     Outstanding={1}, End processing node {2}. Elapsed={3:HH.mm.ss}",
				TID, OutstandingRefreshes, node.NodePtr.Text, Elapsed);
			evtFeedRefreshed(node);
		}

//---------------------------------------------------------------------------------------

		private void web_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
			WebBrowser web2 = sender as WebBrowser;
			// TODO: Not quite sure how to figure out if this is a true Document
			//		 Complete event (e.g. not just a Frame). Traditionally I'd code
#if false
			if (web2 == web.Application as SHDocVw.WebBrowser) {
				// Done
			}
#endif
			// Except that the new V2.0 WebBrowser control doesn't have an Application
			// property...
		}

//---------------------------------------------------------------------------------------

		private void timer1_Tick(object sender, EventArgs e) {
#if false
			string msg = string.Format("{0} Timer tick in LRSS. RefreshInterval = {1:N0}", DateTime.Now, Parms.RefreshInterval);
			Debug.Print(msg);
#endif
			// Log("Tick at {0}", DateTime.Now);
			System.Windows.Forms.Timer tmr = (System.Windows.Forms.Timer)sender;
			// The first time through, the Interval will be short (e.g. 1 second). Change
			// things so that the next scan is further away.
			// It looks like just setting the interval doesn't change things. So stop
			// and restart.
			tmr.Stop();
			// TODO: For now, wait for the RefreshInterval. But later, let every feed
			//		 specify how often it should be refreshed, and wait just until the
			//		 next feed needs to be checked. For example, if the user wants to
			//		 check CNN every 20 minutes, fine. Note that we'll always check
			//		 at least every RefreshInterval (e.g. 1 hour).
			tmr.Interval = Parms.RefreshInterval;
			tmr.Start();

			RefreshAllFeeds();
		}

//---------------------------------------------------------------------------------------

		private void IdleRoutine() {
			// TODO: Scan the output queue and update the UI?
#if false
			Log("{0} LRSS Idle routine called.", DateTime.Now);
			Debug.Print(msg);
#endif
		}

//---------------------------------------------------------------------------------------

		private void mnuFile_Refresh_Click(object sender, EventArgs e) {
			// TODO: We don't have any menus yet. Should this be named popup...?
			foreach (TreeNode node in tvRSS.Nodes) {
				// TODO: Shouldn't we check for Category or not?
				RefreshNode(node, true);
			}
		}

//---------------------------------------------------------------------------------------

		private void deleteFeedToolStripMenuItem_Click(object sender, EventArgs e) {
			MessageBox.Show("Delete Feed not yet implemented.");     // TODO:
		}

//---------------------------------------------------------------------------------------

		private void AddTreeNode(TreeNode CurNode, LRSRSSTreeNode node, RssPersist DB) {
			TreeNode tNode;

			// TODO: Don't know if this is correct.
			// TODO: Duplicates code below
			if (! node.IsFeed) {
				tNode = new TreeNode("***LRS***");
				tNode.Tag = node;
				node.NodePtr = tNode;
				CurNode.Nodes.Add(tNode);

				foreach (LRSRSSTreeNode kid in node.children) {
					AddTreeNode(tNode, kid, DB);
				}
				return;
			}

			// node.feed = new RSSFeed(node.feed.URL);		// TODO: ???
			tNode = new TreeNode(node.feed.Title);
			tNode.Tag = node;
			// If this instance came in via Serialization, the back pointer from the
			// LRSRSSTreeNode will be null. Fill it in, just in case.
			node.NodePtr = tNode;

			DB.GetFeedInfo(node.feed);
			lv.SetNodeTitleCounts(node, DB, tNode);
			CurNode.Nodes.Add(tNode);

			foreach (LRSRSSTreeNode kid in node.children) {
				AddTreeNode(tNode, kid, DB);
			}
		}

//---------------------------------------------------------------------------------------

		private void tvRSS_DrawNode(object sender, DrawTreeNodeEventArgs e) {
			// TODO: Turn on the TreeView DrawMode option to enable this
#if false	// TODO: <sender> is always Subscribed Feeds
			e.Graphics.DrawString(e.Node.Text, e.Node.TreeView.Font, Brushes.DodgerBlue, e.Node.Bounds);
			SizeF szf = e.Graphics.MeasureString(e.Node.Text, e.Node.TreeView.Font);
			e.Graphics.DrawString(" Hello", e.Node.TreeView.Font, Brushes.Salmon, (int)szf.Width, 0);
#endif
		}

//---------------------------------------------------------------------------------------

		private void contextMenuStrip1_Opening(object sender, CancelEventArgs e) {
			ContextMenuStrip	cms = (ContextMenuStrip)sender;
			ToolStripMenuItem	mi  = (ToolStripMenuItem)cms.Items["PopupFavorite"];
			LRSRSSTreeNode		tn  = (LRSRSSTreeNode)CurrentClickedNode.Tag;
			if ((tn.feed != null) && (tn.feed.Favorite)) {
				mi.Checked = true;
			} else {
				mi.Checked = false;
			}
		}
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// Holds the persisted parameters for the program.
	/// </summary>
	[Serializable]
	public class LRSSParms {
		public string		TreeFileName;
		public int			RefreshInterval;

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Sets the default values for the parameters.
		/// </summary>
		public LRSSParms() {
			TreeFileName = "LRSTree.xml";
			RefreshInterval = 60 * 60 * 1000;		// 1 hour in milliseconds
		}
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// A minor helper class, used to hold exactly two items. Basically the same as the
	/// Pair class in the C++ STL. Note: I could use the KeyValuePair class, but I hate
	/// that name as being too specific. So in the interests of being self documenting,
	/// we'll define our own. In this case, it's easy enough.
	/// </summary>
	/// <typeparam name="T1">The type of the first field.</typeparam>
	/// <typeparam name="T2">The type of the first field.</typeparam>
	[Serializable]
	public class Pair<T1, T2> {
		/// <summary>
		/// The first field.
		/// </summary>
		public T1	First;
		/// <summary>
		/// The second field.
		/// </summary>
		public T2	Second;

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Creates an instance of a Pair.
		/// </summary>
		/// <param name="First">The first field.</param>
		/// <param name="Second">The second field.</param>
		public Pair(T1 First, T2 Second) {
			this.First = First;
			this.Second = Second;
		}
	}
}