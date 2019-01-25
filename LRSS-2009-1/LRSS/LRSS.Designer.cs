namespace LRSS {
	partial class LRSS {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.tvRSS = new System.Windows.Forms.TreeView();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.popupAddFeed = new System.Windows.Forms.ToolStripMenuItem();
			this.popupAddCategory = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.popupRefresh = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.popupDeleteFeed = new System.Windows.Forms.ToolStripMenuItem();
			this.popupDeleteCategory = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.popupImportFromOPML = new System.Windows.Forms.ToolStripMenuItem();
			this.popupExportToOPML = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.popupProperties = new System.Windows.Forms.ToolStripMenuItem();
			this.PopupFavorite = new System.Windows.Forms.ToolStripMenuItem();
			this.web = new System.Windows.Forms.WebBrowser();
			this.lvRssItems = new System.Windows.Forms.ListView();
			this.Title = new System.Windows.Forms.ColumnHeader();
			this.Date = new System.Windows.Forms.ColumnHeader();
			this.Feed = new System.Windows.Forms.ColumnHeader();
			this.DownloadDate = new System.Windows.Forms.ColumnHeader();
			this.Flag = new System.Windows.Forms.ColumnHeader();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.contextMenuStrip1.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tvRSS
			// 
			this.tvRSS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tvRSS.ContextMenuStrip = this.contextMenuStrip1;
			this.tvRSS.Location = new System.Drawing.Point(0, 3);
			this.tvRSS.Name = "tvRSS";
			this.tvRSS.Size = new System.Drawing.Size(327, 592);
			this.tvRSS.TabIndex = 0;
			this.tvRSS.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.tvRSS_DrawNode);
			this.tvRSS.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvRSS_NodeMouseClick);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.popupAddFeed,
            this.popupAddCategory,
            this.toolStripSeparator1,
            this.popupRefresh,
            this.toolStripSeparator2,
            this.popupDeleteFeed,
            this.popupDeleteCategory,
            this.toolStripMenuItem1,
            this.popupImportFromOPML,
            this.popupExportToOPML,
            this.toolStripSeparator3,
            this.popupProperties,
            this.PopupFavorite});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(216, 248);
			this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
			// 
			// popupAddFeed
			// 
			this.popupAddFeed.Name = "popupAddFeed";
			this.popupAddFeed.Size = new System.Drawing.Size(215, 22);
			this.popupAddFeed.Text = "Add Feed";
			this.popupAddFeed.Click += new System.EventHandler(this.popupAddFeed_Click);
			// 
			// popupAddCategory
			// 
			this.popupAddCategory.Name = "popupAddCategory";
			this.popupAddCategory.Size = new System.Drawing.Size(215, 22);
			this.popupAddCategory.Text = "Add Category";
			this.popupAddCategory.Click += new System.EventHandler(this.popupAddCategory_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(212, 6);
			// 
			// popupRefresh
			// 
			this.popupRefresh.Name = "popupRefresh";
			this.popupRefresh.Size = new System.Drawing.Size(215, 22);
			this.popupRefresh.Text = "Refresh";
			this.popupRefresh.Click += new System.EventHandler(this.popupRefreshFeed_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(212, 6);
			// 
			// popupDeleteFeed
			// 
			this.popupDeleteFeed.Name = "popupDeleteFeed";
			this.popupDeleteFeed.Size = new System.Drawing.Size(215, 22);
			this.popupDeleteFeed.Text = "Delete Feed";
			this.popupDeleteFeed.Click += new System.EventHandler(this.deleteFeedToolStripMenuItem_Click);
			// 
			// popupDeleteCategory
			// 
			this.popupDeleteCategory.Name = "popupDeleteCategory";
			this.popupDeleteCategory.Size = new System.Drawing.Size(215, 22);
			this.popupDeleteCategory.Text = "Delete Category";
			this.popupDeleteCategory.Click += new System.EventHandler(this.popupDeleteCategory_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(212, 6);
			// 
			// popupImportFromOPML
			// 
			this.popupImportFromOPML.Name = "popupImportFromOPML";
			this.popupImportFromOPML.Size = new System.Drawing.Size(215, 22);
			this.popupImportFromOPML.Text = "Import from OPML";
			this.popupImportFromOPML.Click += new System.EventHandler(this.popupImportFromOPML_Click);
			// 
			// popupExportToOPML
			// 
			this.popupExportToOPML.Name = "popupExportToOPML";
			this.popupExportToOPML.Size = new System.Drawing.Size(215, 22);
			this.popupExportToOPML.Text = "Export to OPML";
			this.popupExportToOPML.Click += new System.EventHandler(this.popupExportToOPML_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(212, 6);
			// 
			// popupProperties
			// 
			this.popupProperties.Name = "popupProperties";
			this.popupProperties.Size = new System.Drawing.Size(215, 22);
			this.popupProperties.Text = "Properties";
			this.popupProperties.Click += new System.EventHandler(this.popupProperties_Click);
			// 
			// PopupFavorite
			// 
			this.PopupFavorite.Name = "PopupFavorite";
			this.PopupFavorite.Size = new System.Drawing.Size(215, 22);
			this.PopupFavorite.Text = "Favorite";
			this.PopupFavorite.Click += new System.EventHandler(this.PopupFavorite_Click);
			// 
			// web
			// 
			this.web.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.web.Location = new System.Drawing.Point(0, 3);
			this.web.MinimumSize = new System.Drawing.Size(20, 20);
			this.web.Name = "web";
			this.web.Size = new System.Drawing.Size(660, 217);
			this.web.TabIndex = 1;
			this.web.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.web_DocumentCompleted);
			// 
			// lvRssItems
			// 
			this.lvRssItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvRssItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Title,
            this.Date,
            this.Feed,
            this.DownloadDate,
            this.Flag});
			this.lvRssItems.FullRowSelect = true;
			this.lvRssItems.Location = new System.Drawing.Point(0, 3);
			this.lvRssItems.Name = "lvRssItems";
			this.lvRssItems.ShowItemToolTips = true;
			this.lvRssItems.Size = new System.Drawing.Size(660, 362);
			this.lvRssItems.TabIndex = 2;
			this.lvRssItems.UseCompatibleStateImageBehavior = false;
			this.lvRssItems.View = System.Windows.Forms.View.Details;
			this.lvRssItems.Click += new System.EventHandler(this.lvItems_Click);
			// 
			// Title
			// 
			this.Title.Text = "Title";
			this.Title.Width = 221;
			// 
			// Date
			// 
			this.Date.Text = "Date";
			this.Date.Width = 130;
			// 
			// Feed
			// 
			this.Feed.Text = "Feed";
			this.Feed.Width = 132;
			// 
			// DownloadDate
			// 
			this.DownloadDate.Text = "Download Date";
			this.DownloadDate.Width = 112;
			// 
			// Flag
			// 
			this.Flag.Text = "Flag";
			this.Flag.Width = 46;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.BackColor = System.Drawing.Color.Red;
			this.splitContainer1.Panel1.Controls.Add(this.tvRSS);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new System.Drawing.Size(997, 595);
			this.splitContainer1.SplitterDistance = 330;
			this.splitContainer1.TabIndex = 3;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.BackColor = System.Drawing.Color.LawnGreen;
			this.splitContainer2.Panel1.Controls.Add(this.lvRssItems);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.web);
			this.splitContainer2.Size = new System.Drawing.Size(663, 595);
			this.splitContainer2.SplitterDistance = 368;
			this.splitContainer2.TabIndex = 0;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Location = new System.Drawing.Point(0, 598);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(997, 22);
			this.statusStrip1.TabIndex = 4;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// timer1
			// 
			this.timer1.Interval = 1000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// LRSS
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(997, 620);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.splitContainer1);
			this.Name = "LRSS";
			this.Text = "LRSS";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LRSS_FormClosing);
			this.Load += new System.EventHandler(this.LRSS_Load);
			this.contextMenuStrip1.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TreeView tvRSS;
		private System.Windows.Forms.WebBrowser web;
		private System.Windows.Forms.ListView lvRssItems;
		private System.Windows.Forms.ColumnHeader Title;
		private System.Windows.Forms.ColumnHeader Date;
		private System.Windows.Forms.ColumnHeader Feed;
		private System.Windows.Forms.ColumnHeader DownloadDate;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem popupAddFeed;
        private System.Windows.Forms.ToolStripMenuItem popupRefresh;
		private System.Windows.Forms.ToolStripMenuItem popupDeleteFeed;
        private System.Windows.Forms.ToolStripMenuItem popupAddCategory;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem popupDeleteCategory;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem popupProperties;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem popupImportFromOPML;
		private System.Windows.Forms.ToolStripMenuItem popupExportToOPML;
		private System.Windows.Forms.ColumnHeader Flag;
		private System.Windows.Forms.ToolStripMenuItem PopupFavorite;
	}
}

