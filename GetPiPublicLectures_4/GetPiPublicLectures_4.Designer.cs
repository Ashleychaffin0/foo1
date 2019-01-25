namespace GetPiPublicLectures_4 {
	partial class GetPiPublicLectures_4 {
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
			this.btnGo = new System.Windows.Forms.Button();
			this.lbMsg = new System.Windows.Forms.ListBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.txtTargetDir = new System.Windows.Forms.TextBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.copyMsgsToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ChkMp4 = new System.Windows.Forms.CheckBox();
			this.ChkLoResMp4 = new System.Windows.Forms.CheckBox();
			this.ChkMp3 = new System.Windows.Forms.CheckBox();
			this.ChkPdf = new System.Windows.Forms.CheckBox();
			this.BtnDownloadFolder = new System.Windows.Forms.Button();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.lblProgress = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.UdConcurrency = new System.Windows.Forms.NumericUpDown();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.ChkOnlySaveLinks = new System.Windows.Forms.CheckBox();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.UdConcurrency)).BeginInit();
			this.SuspendLayout();
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(30, 39);
			this.btnGo.Margin = new System.Windows.Forms.Padding(4);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(100, 28);
			this.btnGo.TabIndex = 3;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// lbMsg
			// 
			this.lbMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbMsg.FormattingEnabled = true;
			this.lbMsg.ItemHeight = 16;
			this.lbMsg.Location = new System.Drawing.Point(31, 162);
			this.lbMsg.Margin = new System.Windows.Forms.Padding(4);
			this.lbMsg.Name = "lbMsg";
			this.lbMsg.Size = new System.Drawing.Size(1186, 212);
			this.lbMsg.TabIndex = 0;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Location = new System.Drawing.Point(1117, 39);
			this.btnBrowse.Margin = new System.Windows.Forms.Padding(4);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(100, 28);
			this.btnBrowse.TabIndex = 3;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// txtTargetDir
			// 
			this.txtTargetDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtTargetDir.Location = new System.Drawing.Point(156, 42);
			this.txtTargetDir.Margin = new System.Windows.Forms.Padding(4);
			this.txtTargetDir.Name = "txtTargetDir";
			this.txtTargetDir.Size = new System.Drawing.Size(936, 22);
			this.txtTargetDir.TabIndex = 2;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutPanel1.AutoScroll = true;
			this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(31, 396);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(1186, 358);
			this.flowLayoutPanel1.TabIndex = 14;
			// 
			// menuStrip1
			// 
			this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyMsgsToClipboardToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1234, 28);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// copyMsgsToClipboardToolStripMenuItem
			// 
			this.copyMsgsToClipboardToolStripMenuItem.Name = "copyMsgsToClipboardToolStripMenuItem";
			this.copyMsgsToClipboardToolStripMenuItem.Size = new System.Drawing.Size(181, 24);
			this.copyMsgsToClipboardToolStripMenuItem.Text = "Copy msgs to Clipboard";
			this.copyMsgsToClipboardToolStripMenuItem.Click += new System.EventHandler(this.CopyMsgsToClipboard_Click);
			// 
			// ChkMp4
			// 
			this.ChkMp4.AutoSize = true;
			this.ChkMp4.Location = new System.Drawing.Point(34, 88);
			this.ChkMp4.Name = "ChkMp4";
			this.ChkMp4.Size = new System.Drawing.Size(61, 21);
			this.ChkMp4.TabIndex = 4;
			this.ChkMp4.Text = ".mp4";
			this.ChkMp4.UseVisualStyleBackColor = true;
			// 
			// ChkLoResMp4
			// 
			this.ChkLoResMp4.AutoSize = true;
			this.ChkLoResMp4.Location = new System.Drawing.Point(126, 89);
			this.ChkLoResMp4.Name = "ChkLoResMp4";
			this.ChkLoResMp4.Size = new System.Drawing.Size(102, 21);
			this.ChkLoResMp4.TabIndex = 5;
			this.ChkLoResMp4.Text = "LoRes.mp4";
			this.ChkLoResMp4.UseVisualStyleBackColor = true;
			// 
			// ChkMp3
			// 
			this.ChkMp3.AutoSize = true;
			this.ChkMp3.Location = new System.Drawing.Point(259, 89);
			this.ChkMp3.Name = "ChkMp3";
			this.ChkMp3.Size = new System.Drawing.Size(61, 21);
			this.ChkMp3.TabIndex = 6;
			this.ChkMp3.Text = ".mp3";
			this.ChkMp3.UseVisualStyleBackColor = true;
			// 
			// ChkPdf
			// 
			this.ChkPdf.AutoSize = true;
			this.ChkPdf.Location = new System.Drawing.Point(351, 89);
			this.ChkPdf.Name = "ChkPdf";
			this.ChkPdf.Size = new System.Drawing.Size(54, 21);
			this.ChkPdf.TabIndex = 7;
			this.ChkPdf.Text = ".pdf";
			this.ChkPdf.UseVisualStyleBackColor = true;
			// 
			// BtnDownloadFolder
			// 
			this.BtnDownloadFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnDownloadFolder.Location = new System.Drawing.Point(1029, 84);
			this.BtnDownloadFolder.Margin = new System.Windows.Forms.Padding(4);
			this.BtnDownloadFolder.Name = "BtnDownloadFolder";
			this.BtnDownloadFolder.Size = new System.Drawing.Size(188, 28);
			this.BtnDownloadFolder.TabIndex = 11;
			this.BtnDownloadFolder.Text = "Open Download Folder";
			this.BtnDownloadFolder.UseVisualStyleBackColor = true;
			this.BtnDownloadFolder.Click += new System.EventHandler(this.BtnDownloadFolder_Click);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(239, 129);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(341, 23);
			this.progressBar1.TabIndex = 13;
			// 
			// lblProgress
			// 
			this.lblProgress.AutoSize = true;
			this.lblProgress.Location = new System.Drawing.Point(31, 135);
			this.lblProgress.Name = "lblProgress";
			this.lblProgress.Size = new System.Drawing.Size(0, 17);
			this.lblProgress.TabIndex = 12;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(661, 90);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(166, 17);
			this.label1.TabIndex = 9;
			this.label1.Text = "Simultaneous Downloads";
			// 
			// UdConcurrency
			// 
			this.UdConcurrency.Location = new System.Drawing.Point(833, 88);
			this.UdConcurrency.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
			this.UdConcurrency.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.UdConcurrency.Name = "UdConcurrency";
			this.UdConcurrency.Size = new System.Drawing.Size(64, 22);
			this.UdConcurrency.TabIndex = 10;
			this.UdConcurrency.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.UdConcurrency.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
			// 
			// ChkOnlySaveLinks
			// 
			this.ChkOnlySaveLinks.AutoSize = true;
			this.ChkOnlySaveLinks.Location = new System.Drawing.Point(436, 88);
			this.ChkOnlySaveLinks.Name = "ChkOnlySaveLinks";
			this.ChkOnlySaveLinks.Size = new System.Drawing.Size(125, 21);
			this.ChkOnlySaveLinks.TabIndex = 8;
			this.ChkOnlySaveLinks.Text = "Only save links";
			this.ChkOnlySaveLinks.UseVisualStyleBackColor = true;
			// 
			// GetPiPublicLectures_4
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(1234, 767);
			this.Controls.Add(this.ChkOnlySaveLinks);
			this.Controls.Add(this.UdConcurrency);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblProgress);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.BtnDownloadFolder);
			this.Controls.Add(this.ChkPdf);
			this.Controls.Add(this.ChkMp3);
			this.Controls.Add(this.ChkLoResMp4);
			this.Controls.Add(this.ChkMp4);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.txtTargetDir);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.lbMsg);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "GetPiPublicLectures_4";
			this.Text = "Get PI Public Lectures";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GetPiPublicLectures_4_FormClosed);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.UdConcurrency)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.TextBox txtTargetDir;
		internal System.Windows.Forms.ListBox lbMsg;
		internal System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem copyMsgsToClipboardToolStripMenuItem;
		private System.Windows.Forms.Button BtnDownloadFolder;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label lblProgress;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown UdConcurrency;
		public System.Windows.Forms.Timer timer1;
		public System.Windows.Forms.CheckBox ChkMp4;
		public System.Windows.Forms.CheckBox ChkLoResMp4;
		public System.Windows.Forms.CheckBox ChkMp3;
		public System.Windows.Forms.CheckBox ChkPdf;
		public System.Windows.Forms.CheckBox ChkOnlySaveLinks;
	}
}

