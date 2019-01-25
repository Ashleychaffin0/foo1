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
			this.tvRSS = new System.Windows.Forms.TreeView();
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
			this.tvRSS.Location = new System.Drawing.Point(0, 0);
			this.tvRSS.Name = "tvRSS";
			this.tvRSS.Size = new System.Drawing.Size(243, 426);
			this.tvRSS.TabIndex = 0;
			this.tvRSS.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvRSS_NodeMouseClick);
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
			this.lvItems.Location = new System.Drawing.Point(0, 0);
			this.lvItems.Name = "lvItems";
			this.lvItems.Size = new System.Drawing.Size(487, 167);
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
	}
}

