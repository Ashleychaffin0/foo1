namespace FindDuplicateFiles {
	partial class FindDuplicateFiles {
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
            this.chkFullFileCompare = new System.Windows.Forms.CheckBox();
            this.txtOutputFilename = new System.Windows.Forms.Label();
            this.btnOutputFile = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radTB = new System.Windows.Forms.RadioButton();
            this.radGB = new System.Windows.Forms.RadioButton();
            this.radMB = new System.Windows.Forms.RadioButton();
            this.radKB = new System.Windows.Forms.RadioButton();
            this.radBytes = new System.Windows.Forms.RadioButton();
            this.txtMaxSize = new System.Windows.Forms.TextBox();
            this.txtMinSize = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblDir = new System.Windows.Forms.Label();
            this.btnFolderBrowse = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkFullFileCompare
            // 
            this.chkFullFileCompare.AutoSize = true;
            this.chkFullFileCompare.Location = new System.Drawing.Point(478, 125);
            this.chkFullFileCompare.Name = "chkFullFileCompare";
            this.chkFullFileCompare.Size = new System.Drawing.Size(168, 21);
            this.chkFullFileCompare.TabIndex = 22;
            this.chkFullFileCompare.Text = "Use Full File Compare";
            this.chkFullFileCompare.UseVisualStyleBackColor = true;
            // 
            // txtOutputFilename
            // 
            this.txtOutputFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputFilename.Location = new System.Drawing.Point(150, 59);
            this.txtOutputFilename.Name = "txtOutputFilename";
            this.txtOutputFilename.Size = new System.Drawing.Size(536, 32);
            this.txtOutputFilename.TabIndex = 21;
            this.txtOutputFilename.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOutputFile
            // 
            this.btnOutputFile.Location = new System.Drawing.Point(22, 59);
            this.btnOutputFile.Name = "btnOutputFile";
            this.btnOutputFile.Size = new System.Drawing.Size(112, 32);
            this.btnOutputFile.TabIndex = 20;
            this.btnOutputFile.Text = "Output File";
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(590, 179);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(88, 32);
            this.btnStop.TabIndex = 19;
            this.btnStop.Text = "Stop";
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(478, 179);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(88, 32);
            this.btnGo.TabIndex = 18;
            this.btnGo.Text = "Go";
            this.btnGo.Click += new System.EventHandler(this.BtnGo_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radTB);
            this.groupBox1.Controls.Add(this.radGB);
            this.groupBox1.Controls.Add(this.radMB);
            this.groupBox1.Controls.Add(this.radKB);
            this.groupBox1.Controls.Add(this.radBytes);
            this.groupBox1.Location = new System.Drawing.Point(22, 163);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(432, 64);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Units";
            // 
            // radTB
            // 
            this.radTB.Location = new System.Drawing.Point(344, 32);
            this.radTB.Name = "radTB";
            this.radTB.Size = new System.Drawing.Size(72, 24);
            this.radTB.TabIndex = 4;
            this.radTB.Text = "TB";
            // 
            // radGB
            // 
            this.radGB.Location = new System.Drawing.Point(264, 32);
            this.radGB.Name = "radGB";
            this.radGB.Size = new System.Drawing.Size(72, 24);
            this.radGB.TabIndex = 3;
            this.radGB.Text = "GB";
            // 
            // radMB
            // 
            this.radMB.Checked = true;
            this.radMB.Location = new System.Drawing.Point(184, 32);
            this.radMB.Name = "radMB";
            this.radMB.Size = new System.Drawing.Size(72, 24);
            this.radMB.TabIndex = 2;
            this.radMB.TabStop = true;
            this.radMB.Text = "MB";
            // 
            // radKB
            // 
            this.radKB.Location = new System.Drawing.Point(104, 32);
            this.radKB.Name = "radKB";
            this.radKB.Size = new System.Drawing.Size(72, 24);
            this.radKB.TabIndex = 1;
            this.radKB.Text = "KB";
            // 
            // radBytes
            // 
            this.radBytes.Location = new System.Drawing.Point(24, 32);
            this.radBytes.Name = "radBytes";
            this.radBytes.Size = new System.Drawing.Size(72, 24);
            this.radBytes.TabIndex = 0;
            this.radBytes.Text = "Bytes";
            // 
            // txtMaxSize
            // 
            this.txtMaxSize.Location = new System.Drawing.Point(294, 123);
            this.txtMaxSize.Name = "txtMaxSize";
            this.txtMaxSize.Size = new System.Drawing.Size(80, 22);
            this.txtMaxSize.TabIndex = 14;
            // 
            // txtMinSize
            // 
            this.txtMinSize.Location = new System.Drawing.Point(94, 123);
            this.txtMinSize.Name = "txtMinSize";
            this.txtMinSize.Size = new System.Drawing.Size(80, 22);
            this.txtMinSize.TabIndex = 12;
            this.txtMinSize.Text = "1";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(206, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 23);
            this.label3.TabIndex = 17;
            this.label3.Text = "Max Size";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(22, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 23);
            this.label2.TabIndex = 15;
            this.label2.Text = "Min Size";
            // 
            // lblDir
            // 
            this.lblDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDir.Location = new System.Drawing.Point(150, 21);
            this.lblDir.Name = "lblDir";
            this.lblDir.Size = new System.Drawing.Size(536, 32);
            this.lblDir.TabIndex = 13;
            this.lblDir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnFolderBrowse
            // 
            this.btnFolderBrowse.Location = new System.Drawing.Point(22, 21);
            this.btnFolderBrowse.Name = "btnFolderBrowse";
            this.btnFolderBrowse.Size = new System.Drawing.Size(112, 32);
            this.btnFolderBrowse.TabIndex = 11;
            this.btnFolderBrowse.Text = "Folder Browse";
            this.btnFolderBrowse.Click += new System.EventHandler(this.btnFolderBrowse_Click);
            // 
            // FindDuplicateFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.chkFullFileCompare);
            this.Controls.Add(this.txtOutputFilename);
            this.Controls.Add(this.btnOutputFile);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtMaxSize);
            this.Controls.Add(this.txtMinSize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblDir);
            this.Controls.Add(this.btnFolderBrowse);
            this.Name = "FindDuplicateFiles";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox chkFullFileCompare;
		private System.Windows.Forms.Label txtOutputFilename;
		private System.Windows.Forms.Button btnOutputFile;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radTB;
		private System.Windows.Forms.RadioButton radGB;
		private System.Windows.Forms.RadioButton radMB;
		private System.Windows.Forms.RadioButton radKB;
		private System.Windows.Forms.RadioButton radBytes;
		private System.Windows.Forms.TextBox txtMaxSize;
		private System.Windows.Forms.TextBox txtMinSize;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblDir;
		private System.Windows.Forms.Button btnFolderBrowse;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}

