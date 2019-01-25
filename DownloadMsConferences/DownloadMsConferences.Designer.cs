namespace DownloadMsConferences {
	partial class DownloadMsConferences {
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
			this.chkWmv = new System.Windows.Forms.CheckBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// btn_Go
			// 
			this.btn_Go.Location = new System.Drawing.Point(13, 24);
			this.btn_Go.Name = "btn_Go";
			this.btn_Go.Size = new System.Drawing.Size(96, 23);
			this.btn_Go.TabIndex = 0;
			this.btn_Go.Text = "Go";
			this.btn_Go.UseVisualStyleBackColor = true;
			this.btn_Go.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// lbMsgs
			// 
			this.lbMsgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbMsgs.FormattingEnabled = true;
			this.lbMsgs.HorizontalScrollbar = true;
			this.lbMsgs.Location = new System.Drawing.Point(13, 103);
			this.lbMsgs.Name = "lbMsgs";
			this.lbMsgs.Size = new System.Drawing.Size(1053, 147);
			this.lbMsgs.TabIndex = 1;
			// 
			// btnOpenBuildFolder
			// 
			this.btnOpenBuildFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOpenBuildFolder.Location = new System.Drawing.Point(937, 28);
			this.btnOpenBuildFolder.Name = "btnOpenBuildFolder";
			this.btnOpenBuildFolder.Size = new System.Drawing.Size(128, 23);
			this.btnOpenBuildFolder.TabIndex = 2;
			this.btnOpenBuildFolder.Text = "Open Download folder";
			this.btnOpenBuildFolder.UseVisualStyleBackColor = true;
			this.btnOpenBuildFolder.Click += new System.EventHandler(this.btnOpenBuildFolder_Click);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutPanel1.AutoScroll = true;
			this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(13, 262);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(1052, 341);
			this.flowLayoutPanel1.TabIndex = 5;
			// 
			// chkSlides
			// 
			this.chkSlides.AutoSize = true;
			this.chkSlides.Checked = true;
			this.chkSlides.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkSlides.Location = new System.Drawing.Point(145, 28);
			this.chkSlides.Name = "chkSlides";
			this.chkSlides.Size = new System.Drawing.Size(54, 17);
			this.chkSlides.TabIndex = 6;
			this.chkSlides.Text = "Slides";
			this.chkSlides.UseVisualStyleBackColor = true;
			// 
			// chkVideos
			// 
			this.chkVideos.AutoSize = true;
			this.chkVideos.Location = new System.Drawing.Point(217, 28);
			this.chkVideos.Name = "chkVideos";
			this.chkVideos.Size = new System.Drawing.Size(58, 17);
			this.chkVideos.TabIndex = 7;
			this.chkVideos.Text = "Videos";
			this.chkVideos.UseVisualStyleBackColor = true;
			// 
			// cmbConf
			// 
			this.cmbConf.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbConf.FormattingEnabled = true;
			this.cmbConf.Location = new System.Drawing.Point(490, 26);
			this.cmbConf.Name = "cmbConf";
			this.cmbConf.Size = new System.Drawing.Size(427, 21);
			this.cmbConf.TabIndex = 9;
			this.cmbConf.SelectedIndexChanged += new System.EventHandler(this.cmbConf_SelectedIndexChanged);
			// 
			// lblFilesToGo
			// 
			this.lblFilesToGo.Location = new System.Drawing.Point(12, 63);
			this.lblFilesToGo.Name = "lblFilesToGo";
			this.lblFilesToGo.Size = new System.Drawing.Size(125, 23);
			this.lblFilesToGo.TabIndex = 10;
			this.lblFilesToGo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pbFilesToGo
			// 
			this.pbFilesToGo.Location = new System.Drawing.Point(145, 63);
			this.pbFilesToGo.Name = "pbFilesToGo";
			this.pbFilesToGo.Size = new System.Drawing.Size(316, 20);
			this.pbFilesToGo.Step = 1;
			this.pbFilesToGo.TabIndex = 11;
			// 
			// lblElapsed
			// 
			this.lblElapsed.Location = new System.Drawing.Point(351, 24);
			this.lblElapsed.Name = "lblElapsed";
			this.lblElapsed.Size = new System.Drawing.Size(133, 23);
			this.lblElapsed.TabIndex = 12;
			this.lblElapsed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtTargetDir
			// 
			this.txtTargetDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtTargetDir.Location = new System.Drawing.Point(490, 63);
			this.txtTargetDir.Name = "txtTargetDir";
			this.txtTargetDir.Size = new System.Drawing.Size(427, 20);
			this.txtTargetDir.TabIndex = 13;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Location = new System.Drawing.Point(937, 63);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(128, 23);
			this.btnBrowse.TabIndex = 14;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// chkWmv
			// 
			this.chkWmv.AutoSize = true;
			this.chkWmv.Checked = true;
			this.chkWmv.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkWmv.Location = new System.Drawing.Point(292, 28);
			this.chkWmv.Name = "chkWmv";
			this.chkWmv.Size = new System.Drawing.Size(53, 17);
			this.chkWmv.TabIndex = 15;
			this.chkWmv.Text = "WMV";
			this.chkWmv.UseVisualStyleBackColor = true;
			// 
			// toolTip1
			// 
			this.toolTip1.AutomaticDelay = 0;
			// 
			// DownloadMsConferences
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(1077, 615);
			this.Controls.Add(this.chkWmv);
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
			this.Name = "DownloadMsConferences";
			this.Text = "Download Microsoft Conferences";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DownloadMsConferences_FormClosing);
			this.Load += new System.EventHandler(this.Download_Build_2015_via_HtmlAgilityPack_2_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btn_Go;
		private System.Windows.Forms.ListBox lbMsgs;
		private System.Windows.Forms.Button btnOpenBuildFolder;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.CheckBox chkSlides;
		private System.Windows.Forms.CheckBox chkVideos;
		private System.Windows.Forms.CheckBox chkWmv;
		private System.Windows.Forms.ComboBox cmbConf;
		private System.Windows.Forms.Label lblFilesToGo;
		private System.Windows.Forms.ProgressBar pbFilesToGo;
		private System.Windows.Forms.Label lblElapsed;
		private System.Windows.Forms.TextBox txtTargetDir;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}

