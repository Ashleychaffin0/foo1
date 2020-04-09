namespace FindDuplicateFilesGui {
	partial class FindDuplicateFilesGui {
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
			this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.configureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.StatBar = new System.Windows.Forms.StatusStrip();
			this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.Grid = new System.Windows.Forms.DataGridView();
			this.DeleteBox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.FileSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DirName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnDeleteSelectedFiles = new System.Windows.Forms.Button();
			this.menuStrip1.SuspendLayout();
			this.StatBar.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(866, 28);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// runToolStripMenuItem
			// 
			this.runToolStripMenuItem.Name = "runToolStripMenuItem";
			this.runToolStripMenuItem.Size = new System.Drawing.Size(103, 24);
			this.runToolStripMenuItem.Text = "&Run";
			this.runToolStripMenuItem.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 24);
			this.exitToolStripMenuItem.Text = "E&xit";
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configureToolStripMenuItem});
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(57, 24);
			this.optionsToolStripMenuItem.Text = "&Tools";
			// 
			// configureToolStripMenuItem
			// 
			this.configureToolStripMenuItem.Name = "configureToolStripMenuItem";
			this.configureToolStripMenuItem.Size = new System.Drawing.Size(130, 24);
			this.configureToolStripMenuItem.Text = "&Options";
			this.configureToolStripMenuItem.Click += new System.EventHandler(this.configureToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
			this.helpToolStripMenuItem.Text = "&Help";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(119, 24);
			this.aboutToolStripMenuItem.Text = "&About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// StatBar
			// 
			this.StatBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
			this.StatBar.Location = new System.Drawing.Point(0, 523);
			this.StatBar.Name = "StatBar";
			this.StatBar.Size = new System.Drawing.Size(866, 25);
			this.StatBar.TabIndex = 1;
			this.StatBar.Text = "statusStrip1";
			// 
			// toolStripProgressBar1
			// 
			this.toolStripProgressBar1.Name = "toolStripProgressBar1";
			this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 19);
			this.toolStripProgressBar1.Visible = false;
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
			// 
			// Grid
			// 
			this.Grid.AllowUserToAddRows = false;
			this.Grid.AllowUserToDeleteRows = false;
			this.Grid.AllowUserToOrderColumns = true;
			this.Grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.Grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DeleteBox,
            this.FileSize,
            this.FileName,
            this.DirName});
			this.Grid.Location = new System.Drawing.Point(23, 68);
			this.Grid.Name = "Grid";
			this.Grid.RowTemplate.Height = 24;
			this.Grid.Size = new System.Drawing.Size(799, 417);
			this.Grid.TabIndex = 2;
			// 
			// DeleteBox
			// 
			this.DeleteBox.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			this.DeleteBox.HeaderText = "Delete";
			this.DeleteBox.Name = "DeleteBox";
			this.DeleteBox.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.DeleteBox.Width = 55;
			// 
			// FileSize
			// 
			this.FileSize.HeaderText = "File Size";
			this.FileSize.Name = "FileSize";
			this.FileSize.ReadOnly = true;
			// 
			// FileName
			// 
			this.FileName.HeaderText = "File Name";
			this.FileName.Name = "FileName";
			this.FileName.ReadOnly = true;
			this.FileName.Width = 300;
			// 
			// DirName
			// 
			this.DirName.HeaderText = "Directory";
			this.DirName.Name = "DirName";
			this.DirName.ReadOnly = true;
			this.DirName.Width = 300;
			// 
			// btnDeleteSelectedFiles
			// 
			this.btnDeleteSelectedFiles.Location = new System.Drawing.Point(23, 31);
			this.btnDeleteSelectedFiles.Name = "btnDeleteSelectedFiles";
			this.btnDeleteSelectedFiles.Size = new System.Drawing.Size(168, 31);
			this.btnDeleteSelectedFiles.TabIndex = 3;
			this.btnDeleteSelectedFiles.Text = "Delete selected files";
			this.btnDeleteSelectedFiles.UseVisualStyleBackColor = true;
			this.btnDeleteSelectedFiles.Click += new System.EventHandler(this.btnDeleteSelectedFiles_Click);
			// 
			// FindDuplicateFilesGui
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(866, 548);
			this.Controls.Add(this.btnDeleteSelectedFiles);
			this.Controls.Add(this.Grid);
			this.Controls.Add(this.StatBar);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "FindDuplicateFilesGui";
			this.Text = "Find Duplicate Files";
			this.Load += new System.EventHandler(this.FindDuplicateFilesGui_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.StatBar.ResumeLayout(false);
			this.StatBar.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem configureToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.StatusStrip StatBar;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
		private System.Windows.Forms.DataGridView Grid;
		private System.Windows.Forms.DataGridViewCheckBoxColumn DeleteBox;
		private System.Windows.Forms.DataGridViewTextBoxColumn FileSize;
		private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
		private System.Windows.Forms.DataGridViewTextBoxColumn DirName;
		private System.Windows.Forms.Button btnDeleteSelectedFiles;

	}
}

