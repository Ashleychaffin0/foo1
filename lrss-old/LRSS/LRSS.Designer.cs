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
            this.deleteFeedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.popupDeleteCategory = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.popupProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.web = new System.Windows.Forms.WebBrowser();
            this.lvItems = new System.Windows.Forms.ListView();
            this.Title = new System.Windows.Forms.ColumnHeader();
            this.Description = new System.Windows.Forms.ColumnHeader();
            this.Date = new System.Windows.Forms.ColumnHeader();
            this.Author = new System.Windows.Forms.ColumnHeader();
            this.Subject = new System.Windows.Forms.ColumnHeader();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
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
            this.tvRSS.Location = new System.Drawing.Point(0, 55);
            this.tvRSS.Name = "tvRSS";
            this.tvRSS.Size = new System.Drawing.Size(243, 371);
            this.tvRSS.TabIndex = 0;
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
            this.deleteFeedToolStripMenuItem,
            this.popupDeleteCategory,
            this.toolStripSeparator3,
            this.popupProperties});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(197, 176);
            // 
            // popupAddFeed
            // 
            this.popupAddFeed.Name = "popupAddFeed";
            this.popupAddFeed.Size = new System.Drawing.Size(196, 22);
            this.popupAddFeed.Text = "Add Feed";
            this.popupAddFeed.Click += new System.EventHandler(this.popupAddFeed_Click);
            // 
            // popupAddCategory
            // 
            this.popupAddCategory.Name = "popupAddCategory";
            this.popupAddCategory.Size = new System.Drawing.Size(196, 22);
            this.popupAddCategory.Text = "Add Category";
            this.popupAddCategory.Click += new System.EventHandler(this.popupAddCategory_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(193, 6);
            // 
            // popupRefresh
            // 
            this.popupRefresh.Name = "popupRefresh";
            this.popupRefresh.Size = new System.Drawing.Size(196, 22);
            this.popupRefresh.Text = "Refresh";
            this.popupRefresh.Click += new System.EventHandler(this.popupRefreshFeed_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(193, 6);
            // 
            // deleteFeedToolStripMenuItem
            // 
            this.deleteFeedToolStripMenuItem.Name = "deleteFeedToolStripMenuItem";
            this.deleteFeedToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.deleteFeedToolStripMenuItem.Text = "Delete Feed";
            this.deleteFeedToolStripMenuItem.Click += new System.EventHandler(this.deleteFeedToolStripMenuItem_Click);
            // 
            // popupDeleteCategory
            // 
            this.popupDeleteCategory.Name = "popupDeleteCategory";
            this.popupDeleteCategory.Size = new System.Drawing.Size(196, 22);
            this.popupDeleteCategory.Text = "Delete Category";
            this.popupDeleteCategory.Click += new System.EventHandler(this.popupDeleteCategory_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(193, 6);
            // 
            // popupProperties
            // 
            this.popupProperties.Name = "popupProperties";
            this.popupProperties.Size = new System.Drawing.Size(196, 22);
            this.popupProperties.Text = "Properties";
            this.popupProperties.Click += new System.EventHandler(this.popupProperties_Click);
            // 
            // web
            // 
            this.web.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.web.Location = new System.Drawing.Point(0, -2);
            this.web.MinimumSize = new System.Drawing.Size(20, 20);
            this.web.Name = "web";
            this.web.Size = new System.Drawing.Size(487, 250);
            this.web.TabIndex = 1;
            // 
            // lvItems
            // 
            this.lvItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Title,
            this.Description,
            this.Date,
            this.Author,
            this.Subject});
            this.lvItems.Location = new System.Drawing.Point(0, 55);
            this.lvItems.Name = "lvItems";
            this.lvItems.Size = new System.Drawing.Size(487, 112);
            this.lvItems.TabIndex = 2;
            this.lvItems.UseCompatibleStateImageBehavior = false;
            this.lvItems.View = System.Windows.Forms.View.Details;
            this.lvItems.Click += new System.EventHandler(this.lvItems_Click);
            // 
            // Title
            // 
            this.Title.Text = "Title";
            this.Title.Width = 175;
            // 
            // Description
            // 
            this.Description.Text = "Description";
            this.Description.Width = 120;
            // 
            // Date
            // 
            this.Date.Text = "Date";
            this.Date.Width = 79;
            // 
            // Author
            // 
            this.Author.Text = "Author";
            this.Author.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Subject
            // 
            this.Subject.Text = "Subject";
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
            this.splitContainer1.Size = new System.Drawing.Size(740, 426);
            this.splitContainer1.SplitterDistance = 246;
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
            this.splitContainer2.Panel1.Controls.Add(this.lvItems);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.web);
            this.splitContainer2.Size = new System.Drawing.Size(490, 426);
            this.splitContainer2.SplitterDistance = 171;
            this.splitContainer2.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 429);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(740, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // LRSS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 451);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Name = "LRSS";
            this.Text = "LRSS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LRSS_FormClosing);
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
		private System.Windows.Forms.ListView lvItems;
		private System.Windows.Forms.ColumnHeader Title;
		private System.Windows.Forms.ColumnHeader Description;
		private System.Windows.Forms.ColumnHeader Date;
		private System.Windows.Forms.ColumnHeader Author;
		private System.Windows.Forms.ColumnHeader Subject;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem popupAddFeed;
        private System.Windows.Forms.ToolStripMenuItem popupRefresh;
		private System.Windows.Forms.ToolStripMenuItem deleteFeedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem popupAddCategory;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem popupDeleteCategory;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem popupProperties;
	}
}

