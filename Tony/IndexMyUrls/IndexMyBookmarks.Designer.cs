namespace IndexMyBookmarks {
	partial class IndexMyBookmarks {
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
			this.TxtUrl = new System.Windows.Forms.TextBox();
			this.BtnFromClipboard = new System.Windows.Forms.Button();
			this.BtnIndexUrl = new System.Windows.Forms.Button();
			this.TxtSearchBox = new System.Windows.Forms.TextBox();
			this.BtnSearch = new System.Windows.Forms.Button();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.DoTheIndexingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.DoTheIndexingToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.useTestDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.copyMessagesToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.Statbar = new System.Windows.Forms.ToolStripStatusLabel();
			this.LbMsgs = new System.Windows.Forms.ListBox();
			this.ChkShowSkippingMsgs = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lblFolder = new System.Windows.Forms.Label();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.TabMessages = new System.Windows.Forms.TabPage();
			this.TabSearchResults = new System.Windows.Forms.TabPage();
			this.LbSearchResults = new System.Windows.Forms.ListBox();
			this.TabBrowser = new System.Windows.Forms.TabPage();
			this.BtnNextMatch = new System.Windows.Forms.Button();
			this.BtnPreviousMatch = new System.Windows.Forms.Button();
			this.Web = new System.Windows.Forms.WebBrowser();
			this.BtnStop = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.pbProgress = new System.Windows.Forms.ProgressBar();
			this.menuStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.TabMessages.SuspendLayout();
			this.TabSearchResults.SuspendLayout();
			this.TabBrowser.SuspendLayout();
			this.SuspendLayout();
			// 
			// TxtUrl
			// 
			this.TxtUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TxtUrl.Location = new System.Drawing.Point(112, 55);
			this.TxtUrl.Name = "TxtUrl";
			this.TxtUrl.Size = new System.Drawing.Size(504, 22);
			this.TxtUrl.TabIndex = 1;
			// 
			// BtnFromClipboard
			// 
			this.BtnFromClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnFromClipboard.Location = new System.Drawing.Point(641, 55);
			this.BtnFromClipboard.Name = "BtnFromClipboard";
			this.BtnFromClipboard.Size = new System.Drawing.Size(147, 23);
			this.BtnFromClipboard.TabIndex = 2;
			this.BtnFromClipboard.Text = "From Clipboard";
			this.BtnFromClipboard.UseVisualStyleBackColor = true;
			this.BtnFromClipboard.Click += new System.EventHandler(this.BtnUrlFromClipboard_Click);
			// 
			// BtnIndexUrl
			// 
			this.BtnIndexUrl.Location = new System.Drawing.Point(31, 56);
			this.BtnIndexUrl.Name = "BtnIndexUrl";
			this.BtnIndexUrl.Size = new System.Drawing.Size(75, 23);
			this.BtnIndexUrl.TabIndex = 4;
			this.BtnIndexUrl.Text = "Index Url";
			this.BtnIndexUrl.UseVisualStyleBackColor = true;
			this.BtnIndexUrl.Click += new System.EventHandler(this.BtnIndexUrl_Click);
			// 
			// TxtSearchBox
			// 
			this.TxtSearchBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TxtSearchBox.Location = new System.Drawing.Point(112, 102);
			this.TxtSearchBox.Name = "TxtSearchBox";
			this.TxtSearchBox.Size = new System.Drawing.Size(676, 22);
			this.TxtSearchBox.TabIndex = 7;
			// 
			// BtnSearch
			// 
			this.BtnSearch.Location = new System.Drawing.Point(31, 101);
			this.BtnSearch.Name = "BtnSearch";
			this.BtnSearch.Size = new System.Drawing.Size(75, 23);
			this.BtnSearch.TabIndex = 8;
			this.BtnSearch.Text = "Search";
			this.BtnSearch.UseVisualStyleBackColor = true;
			this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
			// 
			// menuStrip1
			// 
			this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DoTheIndexingToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(800, 28);
			this.menuStrip1.TabIndex = 9;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// DoTheIndexingToolStripMenuItem
			// 
			this.DoTheIndexingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DoTheIndexingToolStripMenuItem1,
            this.useTestDataToolStripMenuItem,
            this.toolStripMenuItem1,
            this.copyMessagesToClipboardToolStripMenuItem,
            this.toolStripMenuItem3,
            this.exitToolStripMenuItem});
			this.DoTheIndexingToolStripMenuItem.Name = "DoTheIndexingToolStripMenuItem";
			this.DoTheIndexingToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
			this.DoTheIndexingToolStripMenuItem.Text = "File";
			// 
			// DoTheIndexingToolStripMenuItem1
			// 
			this.DoTheIndexingToolStripMenuItem1.Name = "DoTheIndexingToolStripMenuItem1";
			this.DoTheIndexingToolStripMenuItem1.Size = new System.Drawing.Size(274, 26);
			this.DoTheIndexingToolStripMenuItem1.Text = "Empty the Database";
			this.DoTheIndexingToolStripMenuItem1.Click += new System.EventHandler(this.EmptyTheDatabaseToolStripMenuItem1_Click);
			// 
			// useTestDataToolStripMenuItem
			// 
			this.useTestDataToolStripMenuItem.Name = "useTestDataToolStripMenuItem";
			this.useTestDataToolStripMenuItem.Size = new System.Drawing.Size(274, 26);
			this.useTestDataToolStripMenuItem.Text = "Index Bookmarks";
			this.useTestDataToolStripMenuItem.Click += new System.EventHandler(this.DoTheIndexing_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(271, 6);
			// 
			// copyMessagesToClipboardToolStripMenuItem
			// 
			this.copyMessagesToClipboardToolStripMenuItem.Name = "copyMessagesToClipboardToolStripMenuItem";
			this.copyMessagesToClipboardToolStripMenuItem.Size = new System.Drawing.Size(274, 26);
			this.copyMessagesToClipboardToolStripMenuItem.Text = "Copy Messages to Clipboard";
			this.copyMessagesToClipboardToolStripMenuItem.Click += new System.EventHandler(this.CopyMessagesToClipboardToolStripMenuItem_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(271, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(274, 26);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Statbar});
			this.statusStrip1.Location = new System.Drawing.Point(0, 501);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(800, 22);
			this.statusStrip1.TabIndex = 10;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// Statbar
			// 
			this.Statbar.Name = "Statbar";
			this.Statbar.Size = new System.Drawing.Size(0, 17);
			// 
			// LbMsgs
			// 
			this.LbMsgs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LbMsgs.FormattingEnabled = true;
			this.LbMsgs.ItemHeight = 16;
			this.LbMsgs.Location = new System.Drawing.Point(3, 6);
			this.LbMsgs.Name = "LbMsgs";
			this.LbMsgs.Size = new System.Drawing.Size(757, 212);
			this.LbMsgs.TabIndex = 11;
			// 
			// ChkShowSkippingMsgs
			// 
			this.ChkShowSkippingMsgs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ChkShowSkippingMsgs.AutoSize = true;
			this.ChkShowSkippingMsgs.Location = new System.Drawing.Point(568, 28);
			this.ChkShowSkippingMsgs.Name = "ChkShowSkippingMsgs";
			this.ChkShowSkippingMsgs.Size = new System.Drawing.Size(220, 21);
			this.ChkShowSkippingMsgs.TabIndex = 12;
			this.ChkShowSkippingMsgs.Text = "Show skipping URL messages";
			this.ChkShowSkippingMsgs.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(31, 146);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 17);
			this.label1.TabIndex = 13;
			this.label1.Text = "Folder";
			// 
			// lblFolder
			// 
			this.lblFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblFolder.Location = new System.Drawing.Point(109, 146);
			this.lblFolder.Name = "lblFolder";
			this.lblFolder.Size = new System.Drawing.Size(679, 17);
			this.lblFolder.TabIndex = 14;
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.TabMessages);
			this.tabControl1.Controls.Add(this.TabSearchResults);
			this.tabControl1.Controls.Add(this.TabBrowser);
			this.tabControl1.Location = new System.Drawing.Point(34, 228);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(754, 270);
			this.tabControl1.TabIndex = 15;
			// 
			// TabMessages
			// 
			this.TabMessages.Controls.Add(this.LbMsgs);
			this.TabMessages.Location = new System.Drawing.Point(4, 25);
			this.TabMessages.Name = "TabMessages";
			this.TabMessages.Padding = new System.Windows.Forms.Padding(3);
			this.TabMessages.Size = new System.Drawing.Size(746, 241);
			this.TabMessages.TabIndex = 0;
			this.TabMessages.Text = "Messages";
			this.TabMessages.UseVisualStyleBackColor = true;
			// 
			// TabSearchResults
			// 
			this.TabSearchResults.Controls.Add(this.LbSearchResults);
			this.TabSearchResults.Location = new System.Drawing.Point(4, 25);
			this.TabSearchResults.Name = "TabSearchResults";
			this.TabSearchResults.Padding = new System.Windows.Forms.Padding(3);
			this.TabSearchResults.Size = new System.Drawing.Size(746, 213);
			this.TabSearchResults.TabIndex = 1;
			this.TabSearchResults.Text = "Search Results";
			this.TabSearchResults.UseVisualStyleBackColor = true;
			// 
			// LbSearchResults
			// 
			this.LbSearchResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LbSearchResults.FormattingEnabled = true;
			this.LbSearchResults.ItemHeight = 16;
			this.LbSearchResults.Location = new System.Drawing.Point(-5, 8);
			this.LbSearchResults.Name = "LbSearchResults";
			this.LbSearchResults.Size = new System.Drawing.Size(757, 196);
			this.LbSearchResults.TabIndex = 12;
			this.LbSearchResults.Click += new System.EventHandler(this.LbSearchResults_Click);
			this.LbSearchResults.SelectedIndexChanged += new System.EventHandler(this.LbSearchResults_SelectedIndexChanged);
			// 
			// TabBrowser
			// 
			this.TabBrowser.Controls.Add(this.BtnNextMatch);
			this.TabBrowser.Controls.Add(this.BtnPreviousMatch);
			this.TabBrowser.Controls.Add(this.Web);
			this.TabBrowser.Location = new System.Drawing.Point(4, 25);
			this.TabBrowser.Name = "TabBrowser";
			this.TabBrowser.Size = new System.Drawing.Size(746, 213);
			this.TabBrowser.TabIndex = 2;
			this.TabBrowser.Text = "Browser";
			this.TabBrowser.UseVisualStyleBackColor = true;
			// 
			// BtnNextMatch
			// 
			this.BtnNextMatch.Location = new System.Drawing.Point(84, 3);
			this.BtnNextMatch.Name = "BtnNextMatch";
			this.BtnNextMatch.Size = new System.Drawing.Size(75, 23);
			this.BtnNextMatch.TabIndex = 2;
			this.BtnNextMatch.Text = ">>";
			this.BtnNextMatch.UseVisualStyleBackColor = true;
			this.BtnNextMatch.Click += new System.EventHandler(this.BtnNextMatch_Click);
			// 
			// BtnPreviousMatch
			// 
			this.BtnPreviousMatch.Location = new System.Drawing.Point(3, 3);
			this.BtnPreviousMatch.Name = "BtnPreviousMatch";
			this.BtnPreviousMatch.Size = new System.Drawing.Size(75, 23);
			this.BtnPreviousMatch.TabIndex = 1;
			this.BtnPreviousMatch.Text = "<<";
			this.BtnPreviousMatch.UseVisualStyleBackColor = true;
			this.BtnPreviousMatch.Click += new System.EventHandler(this.BtnPreviousMatch_Click);
			// 
			// Web
			// 
			this.Web.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Web.Location = new System.Drawing.Point(3, 32);
			this.Web.MinimumSize = new System.Drawing.Size(20, 20);
			this.Web.Name = "Web";
			this.Web.ScriptErrorsSuppressed = true;
			this.Web.Size = new System.Drawing.Size(743, 185);
			this.Web.TabIndex = 0;
			// 
			// BtnStop
			// 
			this.BtnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnStop.Location = new System.Drawing.Point(471, 26);
			this.BtnStop.Name = "BtnStop";
			this.BtnStop.Size = new System.Drawing.Size(75, 23);
			this.BtnStop.TabIndex = 16;
			this.BtnStop.Text = "Stop";
			this.BtnStop.UseVisualStyleBackColor = true;
			this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(35, 185);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(65, 17);
			this.label2.TabIndex = 17;
			this.label2.Text = "Progress";
			// 
			// pbProgress
			// 
			this.pbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pbProgress.Location = new System.Drawing.Point(112, 179);
			this.pbProgress.Name = "pbProgress";
			this.pbProgress.Size = new System.Drawing.Size(676, 23);
			this.pbProgress.TabIndex = 18;
			// 
			// IndexMyBookmarks
			// 
			this.AcceptButton = this.BtnSearch;
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 523);
			this.Controls.Add(this.pbProgress);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.BtnStop);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.lblFolder);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.ChkShowSkippingMsgs);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.BtnSearch);
			this.Controls.Add(this.TxtSearchBox);
			this.Controls.Add(this.BtnIndexUrl);
			this.Controls.Add(this.BtnFromClipboard);
			this.Controls.Add(this.TxtUrl);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "IndexMyBookmarks";
			this.Text = "Index My Urls";
			this.Load += new System.EventHandler(this.IndexMyUrls_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.TabMessages.ResumeLayout(false);
			this.TabSearchResults.ResumeLayout(false);
			this.TabBrowser.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TextBox TxtUrl;
		private System.Windows.Forms.Button BtnFromClipboard;
		private System.Windows.Forms.Button BtnIndexUrl;
		private System.Windows.Forms.TextBox TxtSearchBox;
		private System.Windows.Forms.Button BtnSearch;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem DoTheIndexingToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem DoTheIndexingToolStripMenuItem1;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem useTestDataToolStripMenuItem;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel Statbar;
		private System.Windows.Forms.ListBox LbMsgs;
		private System.Windows.Forms.CheckBox ChkShowSkippingMsgs;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblFolder;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage TabMessages;
		private System.Windows.Forms.TabPage TabSearchResults;
		private System.Windows.Forms.ListBox LbSearchResults;
		private System.Windows.Forms.TabPage TabBrowser;
		private System.Windows.Forms.Button BtnNextMatch;
		private System.Windows.Forms.Button BtnPreviousMatch;
		private System.Windows.Forms.WebBrowser Web;
		private System.Windows.Forms.Button BtnStop;
		private System.Windows.Forms.ToolStripMenuItem copyMessagesToClipboardToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ProgressBar pbProgress;
	}
}

