namespace FindDuplicateFilesGui {
	partial class FdfConfig2 {
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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabIncludeDirs = new System.Windows.Forms.TabPage();
			this.tabExcludeDirs = new System.Windows.Forms.TabPage();
			this.tabIncludeFiles = new System.Windows.Forms.TabPage();
			this.tabExcludeFiles = new System.Windows.Forms.TabPage();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.tabControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabIncludeDirs);
			this.tabControl1.Controls.Add(this.tabExcludeDirs);
			this.tabControl1.Controls.Add(this.tabIncludeFiles);
			this.tabControl1.Controls.Add(this.tabExcludeFiles);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(701, 473);
			this.tabControl1.TabIndex = 0;
			// 
			// tabIncludeDirs
			// 
			this.tabIncludeDirs.Location = new System.Drawing.Point(4, 22);
			this.tabIncludeDirs.Name = "tabIncludeDirs";
			this.tabIncludeDirs.Padding = new System.Windows.Forms.Padding(3);
			this.tabIncludeDirs.Size = new System.Drawing.Size(693, 447);
			this.tabIncludeDirs.TabIndex = 0;
			this.tabIncludeDirs.Text = "Include Dirs";
			this.tabIncludeDirs.UseVisualStyleBackColor = true;
			// 
			// tabExcludeDirs
			// 
			this.tabExcludeDirs.Location = new System.Drawing.Point(4, 22);
			this.tabExcludeDirs.Name = "tabExcludeDirs";
			this.tabExcludeDirs.Padding = new System.Windows.Forms.Padding(3);
			this.tabExcludeDirs.Size = new System.Drawing.Size(693, 447);
			this.tabExcludeDirs.TabIndex = 1;
			this.tabExcludeDirs.Text = "Exclude Dirs";
			this.tabExcludeDirs.UseVisualStyleBackColor = true;
			// 
			// tabIncludeFiles
			// 
			this.tabIncludeFiles.Location = new System.Drawing.Point(4, 22);
			this.tabIncludeFiles.Name = "tabIncludeFiles";
			this.tabIncludeFiles.Padding = new System.Windows.Forms.Padding(3);
			this.tabIncludeFiles.Size = new System.Drawing.Size(693, 447);
			this.tabIncludeFiles.TabIndex = 2;
			this.tabIncludeFiles.Text = "Include Files";
			this.tabIncludeFiles.UseVisualStyleBackColor = true;
			// 
			// tabExcludeFiles
			// 
			this.tabExcludeFiles.Location = new System.Drawing.Point(4, 22);
			this.tabExcludeFiles.Name = "tabExcludeFiles";
			this.tabExcludeFiles.Padding = new System.Windows.Forms.Padding(3);
			this.tabExcludeFiles.Size = new System.Drawing.Size(359, 175);
			this.tabExcludeFiles.TabIndex = 3;
			this.tabExcludeFiles.Text = "Exclude Files";
			this.tabExcludeFiles.UseVisualStyleBackColor = true;
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// FdfConfig2
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(701, 473);
			this.Controls.Add(this.tabControl1);
			this.Name = "FdfConfig2";
			this.Text = "Form1";
			this.tabControl1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabIncludeDirs;
		private System.Windows.Forms.TabPage tabExcludeDirs;
		private System.Windows.Forms.TabPage tabIncludeFiles;
		private System.Windows.Forms.TabPage tabExcludeFiles;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
	}
}