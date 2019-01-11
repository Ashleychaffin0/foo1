using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;

// SOURCE: http://www.arstdesign.com/articles/treeviewms.html

// TODO: Make sure we can't multi-select from different parent nodes.

namespace TestTrees {
	public partial class TestTrees : Form {

		// class members
		protected List<TreeNode>	m_coll = new List<TreeNode>();
		protected TreeNode			m_lastNode, m_firstNode;

//---------------------------------------------------------------------------------------

		/* API method */ 
		public List<TreeNode> SelectedNodes {
			get { return m_coll; }
			set {
				removePaintFromNodes();
				m_coll.Clear();
				m_coll = value;
				paintSelectedNodes();
			}
		}

//---------------------------------------------------------------------------------------

		public TestTrees() {
			InitializeComponent();

			TestCache();
		}

//---------------------------------------------------------------------------------------

		private void TestTrees_Load(object sender, EventArgs e) {
			foreach (string fn in Directory.GetFiles(@"C:\")) {
				treeView1.Nodes.Add(fn);
			}
		}

//---------------------------------------------------------------------------------------

		private void textBox1_DragDrop(object sender, DragEventArgs e) {
			object o = sender;
		}

//---------------------------------------------------------------------------------------

		protected void paintSelectedNodes() {
			foreach (TreeNode n in m_coll) {
				n.BackColor = SystemColors.Highlight;
				n.ForeColor = SystemColors.HighlightText;
			}
		}

//---------------------------------------------------------------------------------------

		protected void removePaintFromNodes() {
			if (m_coll.Count == 0)
				return;

			TreeNode n0 = m_coll[0];
			Color back = n0.TreeView.BackColor;
			Color fore = n0.TreeView.ForeColor;

			foreach (TreeNode n in m_coll) {
				n.BackColor = back;
				n.ForeColor = fore;
			}
		}

//---------------------------------------------------------------------------------------
		
		protected bool isParent(TreeNode parentNode, TreeNode childNode) {
			if (parentNode == childNode)
				return true;

			TreeNode n = childNode;
			bool bFound = false;
			while (!bFound && n != null) {
				n = n.Parent;
				bFound = (n == parentNode);
			}
			return bFound;
		}

//---------------------------------------------------------------------------------------

		private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
			TreeView	tv = (TreeView)sender;
			bool bControl = (ModifierKeys == Keys.Control);
		}

//---------------------------------------------------------------------------------------

		private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e) {

			bool bControl = (ModifierKeys == Keys.Control);
			bool bShift = (ModifierKeys == Keys.Shift);

			// selecting twice the node while pressing CTRL ?
			if (bControl && m_coll.Contains(e.Node)) {
				// unselect it (let framework know we don't want selection this time)
				e.Cancel = true;

				// update nodes
				removePaintFromNodes();
				m_coll.Remove(e.Node);
				paintSelectedNodes();
				return;
			}

			m_lastNode = e.Node;
			if (!bShift)
				m_firstNode = e.Node; // store begin of shift sequence
		}


//---------------------------------------------------------------------------------------

			private void treeView1_AfterSelect(object sender, TreeViewEventArgs e) {
			// e.Node is the current node exposed by the base TreeView control

			bool bControl = (ModifierKeys == Keys.Control);
			bool bShift = (ModifierKeys == Keys.Shift);

			if (bControl) {
				if (!m_coll.Contains(e.Node)) { // new node ?
					m_coll.Add(e.Node);
				} else {	// not new, remove it from the collection
					removePaintFromNodes();
					m_coll.Remove(e.Node);
				}
				paintSelectedNodes();
			} else {
				if (bShift) {
					Queue<TreeNode> myQueue = new Queue<TreeNode>();

					TreeNode uppernode = m_firstNode;
					TreeNode bottomnode = e.Node;

					// case 1 : begin and end nodes are parent
					bool bParent = isParent(m_firstNode, e.Node); // is m_firstNode parent (direct or not) of e.Node
					if (!bParent) {
						bParent = isParent(bottomnode, uppernode);
						if (bParent) {		// swap nodes
							TreeNode t = uppernode;
							uppernode = bottomnode;
							bottomnode = t;
						}
					}
					if (bParent) {
						TreeNode n = bottomnode;
						while (n != uppernode.Parent) {
							if (!m_coll.Contains(n)) // new node ?
								myQueue.Enqueue(n);
							n = n.Parent;
						}
					}
						// case 2 : nor the begin nor the end node are descendant one another
				  else {
						if ((uppernode.Parent == null && bottomnode.Parent == null) || (uppernode.Parent != null && uppernode.Parent.Nodes.Contains(bottomnode))) {	// are they siblings ?
							int nIndexUpper = uppernode.Index;
							int nIndexBottom = bottomnode.Index;
							if (nIndexBottom < nIndexUpper) {	// reversed?
								TreeNode t = uppernode;
								uppernode = bottomnode;
								bottomnode = t;
								nIndexUpper = uppernode.Index;
								nIndexBottom = bottomnode.Index;
							}

							TreeNode n = uppernode;
							while (nIndexUpper <= nIndexBottom) {
								if (!m_coll.Contains(n)) // new node ?
									myQueue.Enqueue(n);

								n = n.NextNode;

								nIndexUpper++;
							} // end while

						} else {
							if (!m_coll.Contains(uppernode))
								myQueue.Enqueue(uppernode);
							if (!m_coll.Contains(bottomnode))
								myQueue.Enqueue(bottomnode);
						}

					}

					m_coll.AddRange(myQueue);

					paintSelectedNodes();
					m_firstNode = e.Node; // let us chain several SHIFTs if we like it

				} // end if m_bShift
				else {
					// in the case of a simple click, just add this item
					if (m_coll != null && m_coll.Count > 0) {
						removePaintFromNodes();
						m_coll.Clear();
					}
					m_coll.Add(e.Node);
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void treeView1_ItemDrag(object sender, ItemDragEventArgs e) {
			DoDragDrop( m_coll, DragDropEffects.Copy);
		}

//---------------------------------------------------------------------------------------

		private void textBox1_DragEnter(object sender, DragEventArgs e) {
			e.Effect = DragDropEffects.Copy;
		}

//---------------------------------------------------------------------------------------

			public class foo : IBartGetSize {
				string		s;

				public foo(string s) {
					this.s = s;
				}

				public int GetSize() {
					return s.Length;
				}

				public override string ToString() {
					return s;
				}
			}

//---------------------------------------------------------------------------------------

		public void TestCache() {
			SizeCache<foo> cache = new SizeCache<foo>(10);
			foo asd = new foo("asd");
			foo def = new foo("def");
			foo ghi = new foo("ghi");
			foo xyz = new foo("xyz");

			cache.Add(asd);
			cache.Add(def);
			cache.Add(ghi);
			cache.Add(def);
			cache.Add(xyz);

			foo		res;
			bool	found;
			found = cache.Get("asd", out res);
			found = cache.Get("def", out res);
			found = cache.Get("ghi", out res);
			found = cache.Get("xyz", out res);
			found = cache.Get("def", out res);
		}
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public interface IBartGetSize {
		int		GetSize();
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// This class holds elements that, in total size, never exceed a maximum value. This
	/// determines the pruning algorithm.
	/// <p>
	/// This is essentially a toy class, but could be useful. A more sophisticated class
	/// would probably be designed as an abstract base class that could be inherited
	/// from, with virtual methods for most things, especially the Add and Prune methods
	/// (which really are the heart of any caching algorithm).
	/// </p>
	/// <p>
	/// Note that we assume that every entry can be uniquely identified by a string, 
	/// which in turn is matched by an entry's ToString() method. This is hardly an
	/// adequate general-purpose approach, but we'll go with it for now. One possible
	/// way to generalize this is to make this class dependent on two generic parameters,
	/// the second of which is the type of parameter passed to the Get() method.
	/// </p>
	/// <p>
	/// Another reason this is a toy is that by using a LinkedList, we're implicitly 
	/// assuming that the number of entries in the cache is fairly small, since we do
	/// a linear search to see if it's there. If the number of entries were significantly
	/// larger, we'd need a more sophisticated data structure.
	/// </p>
	/// <p>
	/// A note on concurrency. This class is *not* thread safe. But if you think about
	/// it, that's OK. Consider the worst-case scenario in which, due to race conditions,
	/// the list is split in the middle, with one or more nodes at the end of the list
	/// being orphaned, not being pointed to by another node in the list. Also, if two
	/// entries are being added simultaneously, then one could easily be lost. Sounds 
	/// horrible, doesn't it? But with garbage collection, those lost nodes will be
	/// reclaimed. (And since an entry can be dropped from the cache at any time, the
	/// entry better not hold any resource that can't wait until garbage collection,
	/// anyway.) What bothers me a bit more is the CurrentCacheSize property. It can
	/// get confused. But even if that happens, the only effect is that either the cache
	/// will grow a bit too big, or we'll think that we don't have space to hold new
	/// entries, when really we do. Still, as more and more Add() operations are done,
	/// we'll delete LRU entries, and things will probably get back to normal. This is
	/// especially true if we ever try to Prune the collection, and wind up deleting
	/// all nodes. In that case the CurrentCacheSize parameter will be reset to 0, and
	/// we'll be totally back on track. In other words, this class is (more or less)
	/// self-healing after any concurrency issues. Pretty neat, huh?
	/// </p>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class SizeCache<T> where T: IBartGetSize {
		int					MaxSize;
		LinkedList<T>		Entries;

		int					CurrentCacheSize;

//---------------------------------------------------------------------------------------

		public SizeCache(int MaxSize) {
			this.MaxSize = MaxSize;
			Entries = new LinkedList<T>();
			CurrentCacheSize = 0;
		}

//---------------------------------------------------------------------------------------

		public void SetMaxSize(int NewMaxSize) {
			MaxSize = NewMaxSize;
		}

//---------------------------------------------------------------------------------------

		public void Add(T entry) {
			// Note: It's possible that we try to add an entry that's larger than the
			//		 cache, in which case the Prune() routine will empty out the entire
			//		 cache. To avoid this, we'll refuse to cache any entries that are
			//		 too large. This way we'll at least keep around the smaller entries.
			if (entry.GetSize() > MaxSize)
				return;

			if (Entries.Contains(entry)) {
				MoveToFront(entry);
			} else {
				Entries.AddFirst(entry);
				CurrentCacheSize += entry.GetSize();
			}
			if (CurrentCacheSize > MaxSize)
				Prune();
			dbgDumpCache();
			return;
		}

//---------------------------------------------------------------------------------------

		private void MoveToFront(T entry) {
			// Remove, so it can be added to the front
			Entries.Remove(entry);
			Entries.AddFirst(entry);
		}

//---------------------------------------------------------------------------------------

		public bool Get(string ID, out T value) {
			// Assumes all entries have a unique string ID. This is hardly general
			// purpose, but will do for now.
			foreach (T entry in Entries) {
				if (entry.ToString() == ID) {
					value = entry;
					MoveToFront(entry);
					return true;
				}
			}
			value = default(T);
			return false;
		}

//---------------------------------------------------------------------------------------

		private void Prune() {
			// We've been called from Add because we've overflowed the amount of data
			// that we'll let ourselves keep in the cache. Go through the Entries in
			// reverse order and delete entries until we're back in spec.

			LinkedListNode<T>		CurEntry = Entries.Last;	// Can't be null. 
																// Something's just been added
			LinkedListNode<T>		PrevEntry;

			while (CurrentCacheSize > MaxSize) {
				CurrentCacheSize -= CurEntry.Value.GetSize();
				PrevEntry = CurEntry.Previous;
				Entries.RemoveLast();
				CurEntry = PrevEntry;
				// The above code works, but isn't quite as robust as I'd like it. If
				// somehow we've screwed up our housekeeping (or if the user's GetSize()
				// method has a bug and doesn't always report the same value), then we
				// could run off the end of our linked list and blow trying to process
				// a null linked list node. So I'm going to put in an extra check here,
				// just in case.
				if (CurEntry == null) {
					CurrentCacheSize = 0;
					return;
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void dbgDumpCache() {
			foreach (T entry in Entries) {
				Console.WriteLine("Entry '{0}', size = {1}", entry.ToString(), entry.GetSize());
			}
			Console.WriteLine("--------------");
		}
	}
}
