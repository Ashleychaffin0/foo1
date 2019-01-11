namespace RevEngEConcat {
	partial class RevEngEConcat {
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
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuOpenProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSaveConsolidatedcFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(897, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOpenProjectToolStripMenuItem,
            this.mnuSaveConsolidatedcFileToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// mnuOpenProjectToolStripMenuItem
			// 
			this.mnuOpenProjectToolStripMenuItem.Name = "mnuOpenProjectToolStripMenuItem";
			this.mnuOpenProjectToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl-O";
			this.mnuOpenProjectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.mnuOpenProjectToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
			this.mnuOpenProjectToolStripMenuItem.Text = "&Open Project Folder";
			this.mnuOpenProjectToolStripMenuItem.ToolTipText = "The folder with the .c files";
			this.mnuOpenProjectToolStripMenuItem.Click += new System.EventHandler(this.mnuOpenProjectToolStripMenuItem_Click);
			// 
			// mnuSaveConsolidatedcFileToolStripMenuItem
			// 
			this.mnuSaveConsolidatedcFileToolStripMenuItem.Name = "mnuSaveConsolidatedcFileToolStripMenuItem";
			this.mnuSaveConsolidatedcFileToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl-S";
			this.mnuSaveConsolidatedcFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.mnuSaveConsolidatedcFileToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
			this.mnuSaveConsolidatedcFileToolStripMenuItem.Text = "&Save Consolidated .c File";
			this.mnuSaveConsolidatedcFileToolStripMenuItem.Click += new System.EventHandler(this.mnuSaveConsolidatedcFileToolStripMenuItem_Click);
			// 
			// RevEngEConcat
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(897, 417);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "RevEngEConcat";
			this.Text = "RevEngE .c Concatenator";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuOpenProjectToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuSaveConsolidatedcFileToolStripMenuItem;
	}
}

