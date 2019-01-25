namespace WipeDrive {
	partial class WipeDrive {
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
			this.label1 = new System.Windows.Forms.Label();
			this.cmbDrives = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.lblDriveLabel = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.chkIncludeCDrive = new System.Windows.Forms.CheckBox();
			this.chkIncludeNetworkDrives = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.lblDriveType = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.btnGo = new System.Windows.Forms.Button();
			this.lbMsgs = new System.Windows.Forms.ListBox();
			this.prgWipe = new System.Windows.Forms.ProgressBar();
			this.lblFileSystem = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.lblDriveSize = new System.Windows.Forms.Label();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.udWipeCount = new System.Windows.Forms.NumericUpDown();
			this.lblDriveSpaceUsed = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.btnBrowseDrive = new System.Windows.Forms.Button();
			this.lblWipePass = new System.Windows.Forms.Label();
			this.lblPctDone = new System.Windows.Forms.Label();
			this.cmbFormat = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.udWipeCount)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 49);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(74, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Letter";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cmbDrives
			// 
			this.cmbDrives.FormattingEnabled = true;
			this.cmbDrives.Location = new System.Drawing.Point(27, 78);
			this.cmbDrives.Name = "cmbDrives";
			this.cmbDrives.Size = new System.Drawing.Size(71, 24);
			this.cmbDrives.TabIndex = 1;
			this.cmbDrives.SelectedIndexChanged += new System.EventHandler(this.cmbDrives_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(143, 49);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(117, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "Label";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDriveLabel
			// 
			this.lblDriveLabel.Location = new System.Drawing.Point(143, 79);
			this.lblDriveLabel.Name = "lblDriveLabel";
			this.lblDriveLabel.Size = new System.Drawing.Size(117, 23);
			this.lblDriveLabel.TabIndex = 3;
			this.lblDriveLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(431, 49);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(149, 23);
			this.label3.TabIndex = 4;
			this.label3.Text = "Drive Size (MB)";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// chkIncludeCDrive
			// 
			this.chkIncludeCDrive.AutoSize = true;
			this.chkIncludeCDrive.Location = new System.Drawing.Point(554, 131);
			this.chkIncludeCDrive.Name = "chkIncludeCDrive";
			this.chkIncludeCDrive.Size = new System.Drawing.Size(92, 21);
			this.chkIncludeCDrive.TabIndex = 6;
			this.chkIncludeCDrive.Text = "Include C:";
			this.chkIncludeCDrive.UseVisualStyleBackColor = true;
			this.chkIncludeCDrive.Visible = false;
			this.chkIncludeCDrive.Click += new System.EventHandler(this.chkIncludeCDrive_Click);
			// 
			// chkIncludeNetworkDrives
			// 
			this.chkIncludeNetworkDrives.AutoSize = true;
			this.chkIncludeNetworkDrives.Location = new System.Drawing.Point(683, 131);
			this.chkIncludeNetworkDrives.Name = "chkIncludeNetworkDrives";
			this.chkIncludeNetworkDrives.Size = new System.Drawing.Size(174, 21);
			this.chkIncludeNetworkDrives.TabIndex = 7;
			this.chkIncludeNetworkDrives.Text = "Include Network Drives";
			this.chkIncludeNetworkDrives.UseVisualStyleBackColor = true;
			this.chkIncludeNetworkDrives.Visible = false;
			this.chkIncludeNetworkDrives.Click += new System.EventHandler(this.chkIncludeNetworkDrives_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(24, 129);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(89, 23);
			this.label4.TabIndex = 8;
			this.label4.Text = "Wipe Count";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblDriveType
			// 
			this.lblDriveType.Location = new System.Drawing.Point(276, 79);
			this.lblDriveType.Name = "lblDriveType";
			this.lblDriveType.Size = new System.Drawing.Size(125, 23);
			this.lblDriveType.TabIndex = 10;
			this.lblDriveType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(276, 49);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(125, 23);
			this.label6.TabIndex = 9;
			this.label6.Text = "Type";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label5
			// 
			this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(24, 9);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(843, 23);
			this.label5.TabIndex = 11;
			this.label5.Text = "Drive Info";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(299, 171);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 13;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// lbMsgs
			// 
			this.lbMsgs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lbMsgs.FormattingEnabled = true;
			this.lbMsgs.ItemHeight = 16;
			this.lbMsgs.Location = new System.Drawing.Point(27, 261);
			this.lbMsgs.Name = "lbMsgs";
			this.lbMsgs.Size = new System.Drawing.Size(840, 260);
			this.lbMsgs.TabIndex = 14;
			// 
			// prgWipe
			// 
			this.prgWipe.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.prgWipe.Location = new System.Drawing.Point(146, 216);
			this.prgWipe.Name = "prgWipe";
			this.prgWipe.Size = new System.Drawing.Size(588, 23);
			this.prgWipe.TabIndex = 15;
			// 
			// lblFileSystem
			// 
			this.lblFileSystem.Location = new System.Drawing.Point(775, 79);
			this.lblFileSystem.Name = "lblFileSystem";
			this.lblFileSystem.Size = new System.Drawing.Size(92, 23);
			this.lblFileSystem.TabIndex = 17;
			this.lblFileSystem.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(775, 49);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(92, 23);
			this.label8.TabIndex = 16;
			this.label8.Text = "File System";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDriveSize
			// 
			this.lblDriveSize.Location = new System.Drawing.Point(431, 78);
			this.lblDriveSize.Name = "lblDriveSize";
			this.lblDriveSize.Size = new System.Drawing.Size(146, 23);
			this.lblDriveSize.TabIndex = 18;
			this.lblDriveSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnRefresh
			// 
			this.btnRefresh.Location = new System.Drawing.Point(185, 170);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(75, 23);
			this.btnRefresh.TabIndex = 19;
			this.btnRefresh.Text = "Refresh";
			this.btnRefresh.UseVisualStyleBackColor = true;
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// udWipeCount
			// 
			this.udWipeCount.Location = new System.Drawing.Point(128, 130);
			this.udWipeCount.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
			this.udWipeCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.udWipeCount.Name = "udWipeCount";
			this.udWipeCount.Size = new System.Drawing.Size(61, 22);
			this.udWipeCount.TabIndex = 20;
			this.udWipeCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.udWipeCount.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
			// 
			// lblDriveSpaceUsed
			// 
			this.lblDriveSpaceUsed.Location = new System.Drawing.Point(606, 78);
			this.lblDriveSpaceUsed.Name = "lblDriveSpaceUsed";
			this.lblDriveSpaceUsed.Size = new System.Drawing.Size(146, 23);
			this.lblDriveSpaceUsed.TabIndex = 22;
			this.lblDriveSpaceUsed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(606, 49);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(149, 23);
			this.label9.TabIndex = 21;
			this.label9.Text = "Space Used (MB)";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnBrowseDrive
			// 
			this.btnBrowseDrive.Location = new System.Drawing.Point(27, 171);
			this.btnBrowseDrive.Name = "btnBrowseDrive";
			this.btnBrowseDrive.Size = new System.Drawing.Size(119, 23);
			this.btnBrowseDrive.TabIndex = 23;
			this.btnBrowseDrive.Text = "Browse Drive";
			this.btnBrowseDrive.UseVisualStyleBackColor = true;
			this.btnBrowseDrive.Click += new System.EventHandler(this.btnBrowseDrive_Click);
			// 
			// lblWipePass
			// 
			this.lblWipePass.Location = new System.Drawing.Point(21, 216);
			this.lblWipePass.Name = "lblWipePass";
			this.lblWipePass.Size = new System.Drawing.Size(119, 23);
			this.lblWipePass.TabIndex = 24;
			this.lblWipePass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblPctDone
			// 
			this.lblPctDone.Location = new System.Drawing.Point(760, 216);
			this.lblPctDone.Name = "lblPctDone";
			this.lblPctDone.Size = new System.Drawing.Size(107, 23);
			this.lblPctDone.TabIndex = 25;
			this.lblPctDone.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cmbFormat
			// 
			this.cmbFormat.FormattingEnabled = true;
			this.cmbFormat.Location = new System.Drawing.Point(347, 131);
			this.cmbFormat.Name = "cmbFormat";
			this.cmbFormat.Size = new System.Drawing.Size(71, 24);
			this.cmbFormat.TabIndex = 26;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(219, 131);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(111, 23);
			this.label7.TabIndex = 27;
			this.label7.Text = "Format";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// WipeDrive
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(880, 552);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.cmbFormat);
			this.Controls.Add(this.lblPctDone);
			this.Controls.Add(this.lblWipePass);
			this.Controls.Add(this.btnBrowseDrive);
			this.Controls.Add(this.lblDriveSpaceUsed);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.udWipeCount);
			this.Controls.Add(this.btnRefresh);
			this.Controls.Add(this.lblDriveSize);
			this.Controls.Add(this.lblFileSystem);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.prgWipe);
			this.Controls.Add(this.lbMsgs);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.lblDriveType);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.chkIncludeNetworkDrives);
			this.Controls.Add(this.chkIncludeCDrive);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lblDriveLabel);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cmbDrives);
			this.Controls.Add(this.label1);
			this.Name = "WipeDrive";
			this.Text = "WipeDrive";
			this.Load += new System.EventHandler(this.WipeDrive_Load);
			((System.ComponentModel.ISupportInitialize)(this.udWipeCount)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbDrives;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblDriveLabel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox chkIncludeCDrive;
		private System.Windows.Forms.CheckBox chkIncludeNetworkDrives;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lblDriveType;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.ListBox lbMsgs;
		private System.Windows.Forms.ProgressBar prgWipe;
		private System.Windows.Forms.Label lblFileSystem;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label lblDriveSize;
		private System.Windows.Forms.Button btnRefresh;
		private System.Windows.Forms.NumericUpDown udWipeCount;
		private System.Windows.Forms.Label lblDriveSpaceUsed;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Button btnBrowseDrive;
		private System.Windows.Forms.Label lblWipePass;
		private System.Windows.Forms.Label lblPctDone;
		private System.Windows.Forms.ComboBox cmbFormat;
		private System.Windows.Forms.Label label7;
	}
}

