namespace CoreDownloadMsConferences {
	partial class CoreDownloadMsConferences {
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
			this.btn_Go = new System.Windows.Forms.Button();
			this.lbMsgs = new System.Windows.Forms.ListBox();
			this.btnOpenBuildFolder = new System.Windows.Forms.Button();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.chkSlides = new System.Windows.Forms.CheckBox();
			this.chkVideos = new System.Windows.Forms.CheckBox();
			this.cmbConf = new System.Windows.Forms.ComboBox();
			this.lblFilesToGo = new System.Windows.Forms.Label();
			this.pbFilesToGo = new System.Windows.Forms.ProgressBar();
			this.lblElapsed = new System.Windows.Forms.Label();
			this.txtTargetDir = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.NumSimulDownloads = new System.Windows.Forms.NumericUpDown();
			this.lblSimDownloads = new System.Windows.Forms.Label();
			this.btnOpenHomePage = new System.Windows.Forms.Button();
			this.chkSkeletonOnly = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.NumSimulDownloads)).BeginInit();
			this.SuspendLayout();
			// 
			// btn_Go
			// 
			this.btn_Go.Location = new System.Drawing.Point(16, 39);
			this.btn_Go.Margin = new System.Windows.Forms.Padding(4);
			this.btn_Go.Name = "btn_Go";
			this.btn_Go.Size = new System.Drawing.Size(128, 28);
			this.btn_Go.TabIndex = 0;
			this.btn_Go.Text = "Go";
			this.btn_Go.UseVisualStyleBackColor = true;
			this.btn_Go.Click += new System.EventHandler(this.BtnGo_Click);
			// 
			// lbMsgs
			// 
			this.lbMsgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbMsgs.FormattingEnabled = true;
			this.lbMsgs.HorizontalScrollbar = true;
			this.lbMsgs.ItemHeight = 16;
			this.lbMsgs.Location = new System.Drawing.Point(16, 134);
			this.lbMsgs.Margin = new System.Windows.Forms.Padding(4);
			this.lbMsgs.Name = "lbMsgs";
			this.lbMsgs.Size = new System.Drawing.Size(1403, 212);
			this.lbMsgs.TabIndex = 1;
			// 
			// btnOpenBuildFolder
			// 
			this.btnOpenBuildFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOpenBuildFolder.Location = new System.Drawing.Point(1244, 39);
			this.btnOpenBuildFolder.Margin = new System.Windows.Forms.Padding(4);
			this.btnOpenBuildFolder.Name = "btnOpenBuildFolder";
			this.btnOpenBuildFolder.Size = new System.Drawing.Size(171, 28);
			this.btnOpenBuildFolder.TabIndex = 2;
			this.btnOpenBuildFolder.Text = "Open Download folder";
			this.btnOpenBuildFolder.UseVisualStyleBackColor = true;
			this.btnOpenBuildFolder.Click += new System.EventHandler(this.BtnOpenBuildFolder_Click);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutPanel1.AutoScroll = true;
			this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(17, 354);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(1401, 387);
			this.flowLayoutPanel1.TabIndex = 5;
			// 
			// chkSlides
			// 
			this.chkSlides.AutoSize = true;
			this.chkSlides.Checked = true;
			this.chkSlides.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkSlides.Location = new System.Drawing.Point(196, 48);
			this.chkSlides.Margin = new System.Windows.Forms.Padding(4);
			this.chkSlides.Name = "chkSlides";
			this.chkSlides.Size = new System.Drawing.Size(68, 21);
			this.chkSlides.TabIndex = 6;
			this.chkSlides.Text = "Slides";
			this.chkSlides.UseVisualStyleBackColor = true;
			// 
			// chkVideos
			// 
			this.chkVideos.AutoSize = true;
			this.chkVideos.Checked = true;
			this.chkVideos.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkVideos.Location = new System.Drawing.Point(292, 48);
			this.chkVideos.Margin = new System.Windows.Forms.Padding(4);
			this.chkVideos.Name = "chkVideos";
			this.chkVideos.Size = new System.Drawing.Size(73, 21);
			this.chkVideos.TabIndex = 7;
			this.chkVideos.Text = "Videos";
			this.chkVideos.UseVisualStyleBackColor = true;
			// 
			// cmbConf
			// 
			this.cmbConf.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbConf.FormattingEnabled = true;
			this.cmbConf.Location = new System.Drawing.Point(635, 42);
			this.cmbConf.Margin = new System.Windows.Forms.Padding(4);
			this.cmbConf.Name = "cmbConf";
			this.cmbConf.Size = new System.Drawing.Size(387, 24);
			this.cmbConf.TabIndex = 9;
			this.cmbConf.SelectedIndexChanged += new System.EventHandler(this.CmbConf_SelectedIndexChanged);
			// 
			// lblFilesToGo
			// 
			this.lblFilesToGo.Location = new System.Drawing.Point(20, 92);
			this.lblFilesToGo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblFilesToGo.Name = "lblFilesToGo";
			this.lblFilesToGo.Size = new System.Drawing.Size(156, 28);
			this.lblFilesToGo.TabIndex = 10;
			this.lblFilesToGo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pbFilesToGo
			// 
			this.pbFilesToGo.Location = new System.Drawing.Point(184, 96);
			this.pbFilesToGo.Margin = new System.Windows.Forms.Padding(4);
			this.pbFilesToGo.Name = "pbFilesToGo";
			this.pbFilesToGo.Size = new System.Drawing.Size(419, 25);
			this.pbFilesToGo.Step = 1;
			this.pbFilesToGo.TabIndex = 11;
			// 
			// lblElapsed
			// 
			this.lblElapsed.Location = new System.Drawing.Point(20, 66);
			this.lblElapsed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblElapsed.Name = "lblElapsed";
			this.lblElapsed.Size = new System.Drawing.Size(156, 28);
			this.lblElapsed.TabIndex = 12;
			this.lblElapsed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtTargetDir
			// 
			this.txtTargetDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtTargetDir.Location = new System.Drawing.Point(635, 92);
			this.txtTargetDir.Margin = new System.Windows.Forms.Padding(4);
			this.txtTargetDir.Name = "txtTargetDir";
			this.txtTargetDir.Size = new System.Drawing.Size(581, 22);
			this.txtTargetDir.TabIndex = 13;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Location = new System.Drawing.Point(1244, 92);
			this.btnBrowse.Margin = new System.Windows.Forms.Padding(4);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(171, 28);
			this.btnBrowse.TabIndex = 14;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
			// 
			// toolTip1
			// 
			this.toolTip1.AutomaticDelay = 0;
			// 
			// NumSimulDownloads
			// 
			this.NumSimulDownloads.Location = new System.Drawing.Point(512, 43);
			this.NumSimulDownloads.Margin = new System.Windows.Forms.Padding(4);
			this.NumSimulDownloads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.NumSimulDownloads.Name = "NumSimulDownloads";
			this.NumSimulDownloads.Size = new System.Drawing.Size(91, 22);
			this.NumSimulDownloads.TabIndex = 15;
			this.NumSimulDownloads.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.NumSimulDownloads.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
			// 
			// lblSimDownloads
			// 
			this.lblSimDownloads.Location = new System.Drawing.Point(508, 4);
			this.lblSimDownloads.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblSimDownloads.Name = "lblSimDownloads";
			this.lblSimDownloads.Size = new System.Drawing.Size(109, 36);
			this.lblSimDownloads.TabIndex = 16;
			this.lblSimDownloads.Text = "Simultaneous Downloads";
			this.lblSimDownloads.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnOpenHomePage
			// 
			this.btnOpenHomePage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOpenHomePage.Location = new System.Drawing.Point(1047, 39);
			this.btnOpenHomePage.Margin = new System.Windows.Forms.Padding(4);
			this.btnOpenHomePage.Name = "btnOpenHomePage";
			this.btnOpenHomePage.Size = new System.Drawing.Size(171, 28);
			this.btnOpenHomePage.TabIndex = 17;
			this.btnOpenHomePage.Text = "Open Home Page";
			this.btnOpenHomePage.UseVisualStyleBackColor = true;
			this.btnOpenHomePage.Click += new System.EventHandler(this.BtnOpenHomePage_Click);
			// 
			// chkSkeletonOnly
			// 
			this.chkSkeletonOnly.AutoSize = true;
			this.chkSkeletonOnly.Location = new System.Drawing.Point(377, 48);
			this.chkSkeletonOnly.Margin = new System.Windows.Forms.Padding(4);
			this.chkSkeletonOnly.Name = "chkSkeletonOnly";
			this.chkSkeletonOnly.Size = new System.Drawing.Size(118, 21);
			this.chkSkeletonOnly.TabIndex = 19;
			this.chkSkeletonOnly.Text = "Skeleton Only";
			this.chkSkeletonOnly.UseVisualStyleBackColor = true;
			this.chkSkeletonOnly.CheckedChanged += new System.EventHandler(this.ChkSkeletonOnly_CheckedChanged);
			// 
			// DownloadMsConferences
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(1436, 757);
			this.Controls.Add(this.chkSkeletonOnly);
			this.Controls.Add(this.btnOpenHomePage);
			this.Controls.Add(this.lblSimDownloads);
			this.Controls.Add(this.NumSimulDownloads);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.txtTargetDir);
			this.Controls.Add(this.lblElapsed);
			this.Controls.Add(this.lbMsgs);
			this.Controls.Add(this.pbFilesToGo);
			this.Controls.Add(this.lblFilesToGo);
			this.Controls.Add(this.cmbConf);
			this.Controls.Add(this.chkVideos);
			this.Controls.Add(this.chkSlides);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.btnOpenBuildFolder);
			this.Controls.Add(this.btn_Go);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "DownloadMsConferences";
			this.Text = "Download Microsoft Conferences";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DownloadMsConferences_FormClosing);
			this.Load += new System.EventHandler(this.DownloadMsConferences_Load);
			((System.ComponentModel.ISupportInitialize)(this.NumSimulDownloads)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btn_Go;
		private System.Windows.Forms.Button btnOpenBuildFolder;
		private System.Windows.Forms.CheckBox chkSlides;
		private System.Windows.Forms.CheckBox chkVideos;
		private System.Windows.Forms.ComboBox cmbConf;
		private System.Windows.Forms.Label lblFilesToGo;
		private System.Windows.Forms.ProgressBar pbFilesToGo;
		private System.Windows.Forms.Label lblElapsed;
		private System.Windows.Forms.TextBox txtTargetDir;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.NumericUpDown NumSimulDownloads;
        private System.Windows.Forms.Label lblSimDownloads;
        private System.Windows.Forms.Button btnOpenHomePage;
        private System.Windows.Forms.CheckBox chkSkeletonOnly;
		public System.Windows.Forms.ListBox lbMsgs;
		internal System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
	}
}

