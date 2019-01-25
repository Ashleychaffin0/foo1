// Copyright (c) 2005-2006 by Larry Smith

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

// Just a way to get some of the initialization (and termination) code out of the main
// module...

namespace LRSS {
	public partial class LRSS {

//---------------------------------------------------------------------------------------

		private void SetupLogging() {
			// For now, we'll use the system Trace facility. And we'll ignore multi-
			// threading considerations. Later we can implement locking, provide our
			// own tracing class, etc.

			// Some users (e.g. non-Power Users) may not be able to write to the
			// LRSS installation directory. Put this file into My Documents.
			string MyDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			TraceFileName = Path.Combine(MyDocs, TraceFileName);

			File.Delete(TraceFileName);
			Trace.Listeners.Add(new TextWriterTraceListener(TraceFileName));
			Trace.AutoFlush = true;
			Trace.WriteLine("<HTML>\r\n<BODY>\r\n");
		}

//---------------------------------------------------------------------------------------

		private string GetInitFiles() {
			string filename = FindFileUpInHierarchy(LRSSDatabaseFilename);
			if (filename == null) {
				MessageBox.Show("Program must terminate. Unable to find database file, " + LRSSDatabaseFilename,
					"LRSS Initialization", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				Application.Exit();
			}
			LRSSDatabaseFilename = filename;

			filename = FindFileUpInHierarchy(ParmsFileName);
			if (filename == null) {
				MessageBox.Show("Program must terminate. Unable to find parms file, " + ParmsFileName,
					"LRSS Initialization", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				Application.Exit();
			}

			Parms = GenericSerialization<LRSSParms>.Load(filename);
			// I'd like to do the following. But no. Still, you'd think it could/should work.
			// Parms = GenericSerialization<typeof(Parms)>.Load(ParmsFileName);
			if (Parms == null)
				Parms = new LRSSParms();

			filename = FindFileUpInHierarchy(Parms.TreeFileName);
			if (filename == null) {
				MessageBox.Show("Program must terminate. Unable to find tree file, " + Parms.TreeFileName,
					"LRSS Initialization", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				Application.Exit();
			}
			return filename;
		}

//---------------------------------------------------------------------------------------

		private void LoadTree(string FileName) {
			// Question: Why is this still coming in from XML instead of from Access?
			// Answer:   Because the Access table doesn't have tree hierarchy info in it.
			TreeNodes = GenericSerialization<LRSSTreeNodes>.Load(FileName);
			RootNode = new TreeNode("Subscribed Feeds");
			// Pretend we've clicked the root node. This way, there's always a value for
			// CurrentClickedNode. This will simplify processing later.
			CurrentClickedNode = RootNode;
			tvRSS.BeginUpdate();
			tvRSS.Nodes.Add(RootNode);
			// Make sure the root node has an LRSRSSTreeNode tag too
			LRSRSSTreeNode RootTag = new LRSRSSTreeNode(RootNode);
			RootTag.IsFeed = false;
			// int cntTotal = 0, cntRead = 0;
			using (RssPersist DB = new RssPersist()) {
				foreach (LRSRSSTreeNode node in TreeNodes.nodes) {
#if true
					// Bypass certain trouble-making (e.g. throws exceptions that slow down
					// debugging) feeds.
					if (node.IsFeed && node.feed.Title == "Mano's Blog")
						continue;
#endif
					AddTreeNode(RootNode, node, DB);
					if (node.IsFeed && node.feed.Favorite) {
						Favorites[node.feed.URL] = 1;
						node.NodePtr.BackColor = FavoritesBGColor;
					}
					RootTag.children.Add(node);
				}
			}
			RootNode.Tag = RootTag;
			RootNode.ExpandAll();
			tvRSS.EndUpdate();
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

		private void LRSS_Load(object sender, EventArgs e) {
			string filename = GetInitFiles();

			ThreadPool.SetMaxThreads(MaxThreads, MaxIOCompPorts);

			// Make sure we have *some* kind of document (albeit essentially empty) for
			// when we click on the first Item and have to modify the browser's document.
			// Without this line, the web.Document property would be null.
			web.Navigate("about:blank");

			LoadTree(filename);

			this.Visible = true;
			Application.DoEvents();				// Let tree draw itself first
			lv.ShowItemsForNode(RootNode);		// Only then will we show the items

			statusStrip1.Items.Add(tsl);
			tsl.Name = "label";

#if false
			// As we add each node to the tree, check to see if it's on the
			// database. Now this should always be the case, and the following
			// routine should return immediately. But application/system crashes may
			// get things out of synch, so we'll add a probably redundant check here.
			// Note that this will add only feeds (and their items) that we haven't
			// seen/fully processed before. The main processing of existing feeds
			// to look for new items will be done later, during the background
			// processing phase.
			node.AddFeedToDatabase();
#endif

			UIMon = new UIMonitor();

			// Start a timer with a short initial interval, to do our background
			// processing
			timer1.Start();

			// TODO: This doesn't go here. It goes in the event handler when something
			//		 is put onto the output queue.
			UIMon.CallWhenUIBecomesIdle(new UIIdleRoutine(IdleRoutine));
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

#if false
			MessageBox.Show("Save of LRSTree.xml temporarily disabled");
#else
            GenericSerialization<LRSSTreeNodes>.Save(filename, nodes);
#endif
			Trace.WriteLine("</BODY>\r\n</HTML>\r\n");
			if (File.Exists(TraceFileName))
				Process.Start(TraceFileName);
		}
	}
}
