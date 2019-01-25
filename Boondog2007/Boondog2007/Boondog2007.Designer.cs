namespace Boondog2009 {
    partial class Boondog2009 {
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
            this.newGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblPgmUnfound = new System.Windows.Forms.Label();
            this.lblProgramStats = new System.Windows.Forms.Label();
            this.lbProgramWords = new System.Windows.Forms.ListBox();
            this.lblProgramDone = new System.Windows.Forms.Label();
            this.lbPlayerDone = new System.Windows.Forms.Label();
            this.lbPlayerWords = new System.Windows.Forms.ListBox();
            this.lblPlayerStats = new System.Windows.Forms.Label();
            this.lblPlayerUnfound = new System.Windows.Forms.Label();
            this.lblWordCounts = new System.Windows.Forms.Label();
            this.grpBogBox = new System.Windows.Forms.GroupBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.colorsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(582, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newGameToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.fileToolStripMenuItem.Text = "Game";
            // 
            // newGameToolStripMenuItem
            // 
            this.newGameToolStripMenuItem.Name = "newGameToolStripMenuItem";
            this.newGameToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.newGameToolStripMenuItem.Text = "&New Game";
            this.newGameToolStripMenuItem.Click += new System.EventHandler(this.newGameToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(129, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // colorsToolStripMenuItem
            // 
            this.colorsToolStripMenuItem.Name = "colorsToolStripMenuItem";
            this.colorsToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.colorsToolStripMenuItem.Text = "Colors";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(582, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 466);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip1.Size = new System.Drawing.Size(582, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblPgmUnfound
            // 
            this.lblPgmUnfound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPgmUnfound.BackColor = System.Drawing.Color.Blue;
            this.lblPgmUnfound.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPgmUnfound.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPgmUnfound.ForeColor = System.Drawing.Color.White;
            this.lblPgmUnfound.Location = new System.Drawing.Point(462, 52);
            this.lblPgmUnfound.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPgmUnfound.Name = "lblPgmUnfound";
            this.lblPgmUnfound.Size = new System.Drawing.Size(111, 20);
            this.lblPgmUnfound.TabIndex = 3;
            this.lblPgmUnfound.Text = "Unfound:";
            this.lblPgmUnfound.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProgramStats
            // 
            this.lblProgramStats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgramStats.BackColor = System.Drawing.Color.Blue;
            this.lblProgramStats.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblProgramStats.ForeColor = System.Drawing.Color.White;
            this.lblProgramStats.Location = new System.Drawing.Point(462, 79);
            this.lblProgramStats.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblProgramStats.Name = "lblProgramStats";
            this.lblProgramStats.Size = new System.Drawing.Size(111, 52);
            this.lblProgramStats.TabIndex = 4;
            this.lblProgramStats.Text = "Words:\n\rScore:\nAvg:";
            this.lblProgramStats.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbProgramWords
            // 
            this.lbProgramWords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbProgramWords.BackColor = System.Drawing.Color.Blue;
            this.lbProgramWords.ForeColor = System.Drawing.Color.White;
            this.lbProgramWords.FormattingEnabled = true;
            this.lbProgramWords.HorizontalScrollbar = true;
            this.lbProgramWords.Location = new System.Drawing.Point(462, 147);
            this.lbProgramWords.Margin = new System.Windows.Forms.Padding(2);
            this.lbProgramWords.Name = "lbProgramWords";
            this.lbProgramWords.Size = new System.Drawing.Size(112, 277);
            this.lbProgramWords.Sorted = true;
            this.lbProgramWords.TabIndex = 5;
            // 
            // lblProgramDone
            // 
            this.lblProgramDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgramDone.BackColor = System.Drawing.Color.Blue;
            this.lblProgramDone.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblProgramDone.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgramDone.ForeColor = System.Drawing.Color.White;
            this.lblProgramDone.Location = new System.Drawing.Point(462, 439);
            this.lblProgramDone.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblProgramDone.Name = "lblProgramDone";
            this.lblProgramDone.Size = new System.Drawing.Size(111, 20);
            this.lblProgramDone.TabIndex = 6;
            this.lblProgramDone.Text = "*Done*";
            this.lblProgramDone.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPlayerDone
            // 
            this.lbPlayerDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPlayerDone.BackColor = System.Drawing.Color.Blue;
            this.lbPlayerDone.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbPlayerDone.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPlayerDone.ForeColor = System.Drawing.Color.White;
            this.lbPlayerDone.Location = new System.Drawing.Point(327, 439);
            this.lbPlayerDone.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbPlayerDone.Name = "lbPlayerDone";
            this.lbPlayerDone.Size = new System.Drawing.Size(111, 20);
            this.lbPlayerDone.TabIndex = 10;
            this.lbPlayerDone.Text = "*Done*";
            this.lbPlayerDone.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPlayerWords
            // 
            this.lbPlayerWords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPlayerWords.BackColor = System.Drawing.Color.Blue;
            this.lbPlayerWords.ForeColor = System.Drawing.Color.White;
            this.lbPlayerWords.FormattingEnabled = true;
            this.lbPlayerWords.HorizontalScrollbar = true;
            this.lbPlayerWords.Location = new System.Drawing.Point(327, 147);
            this.lbPlayerWords.Margin = new System.Windows.Forms.Padding(2);
            this.lbPlayerWords.Name = "lbPlayerWords";
            this.lbPlayerWords.Size = new System.Drawing.Size(112, 277);
            this.lbPlayerWords.Sorted = true;
            this.lbPlayerWords.TabIndex = 9;
            // 
            // lblPlayerStats
            // 
            this.lblPlayerStats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPlayerStats.BackColor = System.Drawing.Color.Blue;
            this.lblPlayerStats.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPlayerStats.ForeColor = System.Drawing.Color.White;
            this.lblPlayerStats.Location = new System.Drawing.Point(327, 79);
            this.lblPlayerStats.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPlayerStats.Name = "lblPlayerStats";
            this.lblPlayerStats.Size = new System.Drawing.Size(111, 52);
            this.lblPlayerStats.TabIndex = 8;
            this.lblPlayerStats.Text = "Words:\n\rScore:\nAvg:";
            this.lblPlayerStats.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPlayerUnfound
            // 
            this.lblPlayerUnfound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPlayerUnfound.BackColor = System.Drawing.Color.Blue;
            this.lblPlayerUnfound.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPlayerUnfound.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlayerUnfound.ForeColor = System.Drawing.Color.White;
            this.lblPlayerUnfound.Location = new System.Drawing.Point(327, 52);
            this.lblPlayerUnfound.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPlayerUnfound.Name = "lblPlayerUnfound";
            this.lblPlayerUnfound.Size = new System.Drawing.Size(111, 20);
            this.lblPlayerUnfound.TabIndex = 7;
            this.lblPlayerUnfound.Text = "Unfound:";
            this.lblPlayerUnfound.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWordCounts
            // 
            this.lblWordCounts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWordCounts.BackColor = System.Drawing.Color.Blue;
            this.lblWordCounts.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblWordCounts.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWordCounts.ForeColor = System.Drawing.Color.White;
            this.lblWordCounts.Location = new System.Drawing.Point(9, 406);
            this.lblWordCounts.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWordCounts.Name = "lblWordCounts";
            this.lblWordCounts.Size = new System.Drawing.Size(303, 52);
            this.lblWordCounts.TabIndex = 11;
            this.lblWordCounts.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpBogBox
            // 
            this.grpBogBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBogBox.Location = new System.Drawing.Point(12, 52);
            this.grpBogBox.Margin = new System.Windows.Forms.Padding(2);
            this.grpBogBox.Name = "grpBogBox";
            this.grpBogBox.Padding = new System.Windows.Forms.Padding(2);
            this.grpBogBox.Size = new System.Drawing.Size(298, 340);
            this.grpBogBox.TabIndex = 12;
            this.grpBogBox.TabStop = false;
            this.grpBogBox.Text = "groupBox1";
            this.grpBogBox.Visible = false;
            // 
            // Boondog2009
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(582, 488);
            this.Controls.Add(this.grpBogBox);
            this.Controls.Add(this.lblWordCounts);
            this.Controls.Add(this.lbPlayerDone);
            this.Controls.Add(this.lbPlayerWords);
            this.Controls.Add(this.lblPlayerStats);
            this.Controls.Add(this.lblPlayerUnfound);
            this.Controls.Add(this.lblProgramDone);
            this.Controls.Add(this.lbProgramWords);
            this.Controls.Add(this.lblProgramStats);
            this.Controls.Add(this.lblPgmUnfound);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Boondog2009";
            this.Text = "Boondoggle 2007";
            this.Resize += new System.EventHandler(this.Boondog2009_Resize);
            this.Load += new System.EventHandler(this.Boondog2009_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.Label lblPgmUnfound;
        private System.Windows.Forms.Label lblProgramStats;
        private System.Windows.Forms.ListBox lbProgramWords;
        private System.Windows.Forms.Label lblProgramDone;
        private System.Windows.Forms.Label lbPlayerDone;
        private System.Windows.Forms.ListBox lbPlayerWords;
        private System.Windows.Forms.Label lblPlayerStats;
        private System.Windows.Forms.Label lblPlayerUnfound;
        private System.Windows.Forms.Label lblWordCounts;
        private System.Windows.Forms.GroupBox grpBogBox;
        private System.Windows.Forms.ToolStripMenuItem newGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    }
}

