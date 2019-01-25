namespace ReadThunderbird_1 {
	partial class ReadThunderbird_1 {
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
			this.txtInboxFilename = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.btnGo = new System.Windows.Forms.Button();
			this.lbMsgs = new System.Windows.Forms.ListBox();
			this.webBrowser1 = new System.Windows.Forms.WebBrowser();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 51);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(65, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Filename";
			// 
			// txtInboxFilename
			// 
			this.txtInboxFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtInboxFilename.Location = new System.Drawing.Point(94, 51);
			this.txtInboxFilename.Name = "txtInboxFilename";
			this.txtInboxFilename.Size = new System.Drawing.Size(764, 22);
			this.txtInboxFilename.TabIndex = 1;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Location = new System.Drawing.Point(875, 49);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(75, 23);
			this.btnBrowse.TabIndex = 2;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(387, 12);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 3;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// lbMsgs
			// 
			this.lbMsgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.lbMsgs.FormattingEnabled = true;
			this.lbMsgs.HorizontalScrollbar = true;
			this.lbMsgs.ItemHeight = 16;
			this.lbMsgs.Location = new System.Drawing.Point(12, 95);
			this.lbMsgs.Name = "lbMsgs";
			this.lbMsgs.Size = new System.Drawing.Size(337, 468);
			this.lbMsgs.TabIndex = 4;
			this.lbMsgs.Click += new System.EventHandler(this.lbMsgs_Click);
			// 
			// webBrowser1
			// 
			this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.webBrowser1.Location = new System.Drawing.Point(375, 95);
			this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
			this.webBrowser1.Name = "webBrowser1";
			this.webBrowser1.Size = new System.Drawing.Size(575, 468);
			this.webBrowser1.TabIndex = 5;
			// 
			// ReadThunderbird_1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(963, 592);
			this.Controls.Add(this.webBrowser1);
			this.Controls.Add(this.lbMsgs);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.txtInboxFilename);
			this.Controls.Add(this.label1);
			this.Name = "ReadThunderbird_1";
			this.Text = "Read Thunderbird";
			this.Load += new System.EventHandler(this.ReadThunderbird_1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtInboxFilename;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.ListBox lbMsgs;
		private System.Windows.Forms.WebBrowser webBrowser1;
	}
}

