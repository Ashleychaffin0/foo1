namespace XWord {
	partial class frmXWord {
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
			this.XWordPanel = new System.Windows.Forms.Panel();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newPuzzleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadPuzzleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.savePuzzleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.savePuzzleAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutXWordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// XWordPanel
			// 
			this.XWordPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.XWordPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.XWordPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.XWordPanel.Location = new System.Drawing.Point(23, 41);
			this.XWordPanel.Name = "XWordPanel";
			this.XWordPanel.Size = new System.Drawing.Size(556, 544);
			this.XWordPanel.TabIndex = 0;
			this.XWordPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.XWordPanel_Paint);
			this.XWordPanel.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.XWordPanel_PreviewKeyDown);
			this.XWordPanel.Resize += new System.EventHandler(this.XWordPanel_Resize);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(880, 28);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newPuzzleToolStripMenuItem,
            this.loadPuzzleToolStripMenuItem,
            this.savePuzzleToolStripMenuItem,
            this.savePuzzleAsToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// newPuzzleToolStripMenuItem
			// 
			this.newPuzzleToolStripMenuItem.Name = "newPuzzleToolStripMenuItem";
			this.newPuzzleToolStripMenuItem.Size = new System.Drawing.Size(184, 24);
			this.newPuzzleToolStripMenuItem.Text = "&New Puzzle";
			this.newPuzzleToolStripMenuItem.Click += new System.EventHandler(this.newPuzzleToolStripMenuItem_Click);
			// 
			// loadPuzzleToolStripMenuItem
			// 
			this.loadPuzzleToolStripMenuItem.Name = "loadPuzzleToolStripMenuItem";
			this.loadPuzzleToolStripMenuItem.Size = new System.Drawing.Size(184, 24);
			this.loadPuzzleToolStripMenuItem.Text = "&Load Puzzle...";
			// 
			// savePuzzleToolStripMenuItem
			// 
			this.savePuzzleToolStripMenuItem.Name = "savePuzzleToolStripMenuItem";
			this.savePuzzleToolStripMenuItem.Size = new System.Drawing.Size(184, 24);
			this.savePuzzleToolStripMenuItem.Text = "&Save Puzzle";
			// 
			// savePuzzleAsToolStripMenuItem
			// 
			this.savePuzzleAsToolStripMenuItem.Name = "savePuzzleAsToolStripMenuItem";
			this.savePuzzleAsToolStripMenuItem.Size = new System.Drawing.Size(184, 24);
			this.savePuzzleAsToolStripMenuItem.Text = "Save Puzzle &As...";
			this.savePuzzleAsToolStripMenuItem.Click += new System.EventHandler(this.savePuzzleAsToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(184, 24);
			this.exitToolStripMenuItem.Text = "E&xit";
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutXWordToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
			this.helpToolStripMenuItem.Text = "Help";
			// 
			// aboutXWordToolStripMenuItem
			// 
			this.aboutXWordToolStripMenuItem.Name = "aboutXWordToolStripMenuItem";
			this.aboutXWordToolStripMenuItem.Size = new System.Drawing.Size(169, 24);
			this.aboutXWordToolStripMenuItem.Text = "About XWord";
			// 
			// textBox1
			// 
			this.textBox1.Enabled = false;
			this.textBox1.Location = new System.Drawing.Point(640, 167);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(100, 22);
			this.textBox1.TabIndex = 2;
			// 
			// frmXWord
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(880, 614);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.XWordPanel);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "frmXWord";
			this.Text = "LRS Crossword";
			this.Load += new System.EventHandler(this.frmXWord_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel XWordPanel;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newPuzzleToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutXWordToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadPuzzleToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem savePuzzleToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem savePuzzleAsToolStripMenuItem;
		private System.Windows.Forms.TextBox textBox1;
	}
}

