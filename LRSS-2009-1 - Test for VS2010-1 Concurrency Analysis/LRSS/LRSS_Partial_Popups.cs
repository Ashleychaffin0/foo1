// Copyright (c) 2005-2006 by Larry Smith

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

// Just a way to get those pesky popup handlers out of the main module...

namespace LRSS {
	public partial class LRSS {

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

		private void popupAddFeed_Click(object sender, EventArgs e) {
			frmAddRSSFeed dlgAddFeed = new frmAddRSSFeed();
			DialogResult res = dlgAddFeed.ShowDialog();
			if (res != DialogResult.OK)
				return;

			// We're trying to add this feed
			// TODO: Finish this (partial implementation follows)
			// TODO: Check if feed already exists at the same level.
			if (dlgAddFeed.feed == null) {
				string msg;
				msg = "Internal error - feed instance not created. Bypass: Click Get Feed.";
				MessageBox.Show(msg, "LRSS TODO:", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			TreeNode tNode = new TreeNode(dlgAddFeed.feed.Title);
			LRSRSSTreeNode node = new LRSRSSTreeNode(tNode, dlgAddFeed.feed, true);
			tNode.Tag = node;
			CurrentClickedNode.Nodes.Add(tNode);
			// Add to the main <TreeNodes> collection (so it will be written out)
			// TODO: We currently don't make the distinction between a category node
			//		 and a feed node. We need to do so. For now, assume the user (moi)
			//		 hasn't screwed things up, and is adding to a category node.
			LRSRSSTreeNode CurRSSNode = (LRSRSSTreeNode)CurrentClickedNode.Tag;
			CurRSSNode.children.Add(node);
		}

//---------------------------------------------------------------------------------------

		private void popupDeleteCategory_Click(object sender, EventArgs e) {
			MessageBox.Show("Delete Category not yet implemented.");     // TODO:
		}

//---------------------------------------------------------------------------------------

		private void popupProperties_Click(object sender, EventArgs e) {
			MessageBox.Show("Properties not yet implemented.");     // TODO:
		}

//---------------------------------------------------------------------------------------

		private void popupImportFromOPML_Click(object sender, EventArgs e) {
			OPML opml = new OPML();
			bool bOK;
			try {
				Cursor = Cursors.WaitCursor;
				tvRSS.BeginUpdate();
				bOK = opml.Import(CurrentClickedNode);
			} finally {
				tvRSS.EndUpdate();
				Cursor = Cursors.Default;
			}
			if (!bOK)
				return;

			Application.DoEvents();
			CurrentClickedNode.ExpandAll();
			RefreshNode(CurrentClickedNode, true);
		}

//---------------------------------------------------------------------------------------

		private void popupExportToOPML_Click(object sender, EventArgs e) {
			MessageBox.Show("Export to OPML not yet implemented");     // TODO:
		}

//---------------------------------------------------------------------------------------

		private void popupRefreshFeed_Click(object sender, EventArgs e) {
			// TODO: Shouldn't we check for category node or not?
			RefreshNode(CurrentClickedNode, true);
		}

//---------------------------------------------------------------------------------------

		private void PopupFavorite_Click(object sender, EventArgs e) {
			ToolStripMenuItem Favorite = (ToolStripMenuItem)sender;
			Favorite.Checked = !Favorite.Checked;
			// Set node background color
			LRSRSSTreeNode tn = (LRSRSSTreeNode)CurrentClickedNode.Tag;
			if (Favorite.Checked) {
				CurrentClickedNode.BackColor = FavoritesBGColor;
				Favorites[tn.feed.URL] = 1;
			} else {
				CurrentClickedNode.BackColor = tvRSS.BackColor;
				if (Favorites.ContainsKey(tn.feed.URL)) {
					Favorites.Remove(tn.feed.URL);
				}
			}

			tn.feed.Favorite = Favorite.Checked;
			using (RssPersist DB = new RssPersist()) {
				DB.SetFavorite(tn.feed);
			}

			// Reshow the items, in case some of them should have their Favorite
			// color either added or removed
			lv.ShowItemsForNode(CurrentClickedNode);
		}
	}
}
