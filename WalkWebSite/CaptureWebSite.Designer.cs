namespace WalkWebSite {
	partial class CaptureWebSite {
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
			this.txtURL = new System.Windows.Forms.TextBox();
			this.lbMsgs = new System.Windows.Forms.ListBox();
			this.btnGo = new System.Windows.Forms.Button();
			this.txtOutputDir = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.web = new System.Windows.Forms.WebBrowser();
			this.btnSwap = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(27, 74);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(36, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "URL";
			// 
			// txtURL
			// 
			this.txtURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtURL.Location = new System.Drawing.Point(149, 74);
			this.txtURL.Name = "txtURL";
			this.txtURL.Size = new System.Drawing.Size(519, 22);
			this.txtURL.TabIndex = 1;
			// 
			// lbMsgs
			// 
			this.lbMsgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.lbMsgs.FormattingEnabled = true;
			this.lbMsgs.HorizontalScrollbar = true;
			this.lbMsgs.ItemHeight = 16;
			this.lbMsgs.Location = new System.Drawing.Point(12, 145);
			this.lbMsgs.Name = "lbMsgs";
			this.lbMsgs.Size = new System.Drawing.Size(127, 404);
			this.lbMsgs.TabIndex = 2;
			// 
			// btnGo
			// 
			this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGo.Location = new System.Drawing.Point(686, 71);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 3;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// txtOutputDir
			// 
			this.txtOutputDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtOutputDir.Location = new System.Drawing.Point(149, 27);
			this.txtOutputDir.Name = "txtOutputDir";
			this.txtOutputDir.Size = new System.Drawing.Size(519, 22);
			this.txtOutputDir.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(27, 27);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(112, 17);
			this.label2.TabIndex = 4;
			this.label2.Text = "Output Directory";
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Location = new System.Drawing.Point(686, 27);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(75, 23);
			this.btnBrowse.TabIndex = 6;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// web
			// 
			this.web.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.web.Location = new System.Drawing.Point(149, 145);
			this.web.MinimumSize = new System.Drawing.Size(20, 20);
			this.web.Name = "web";
			this.web.Size = new System.Drawing.Size(612, 407);
			this.web.TabIndex = 7;
			this.web.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.web_DocumentCompleted);
			// 
			// btnSwap
			// 
			this.btnSwap.Location = new System.Drawing.Point(12, 113);
			this.btnSwap.Name = "btnSwap";
			this.btnSwap.Size = new System.Drawing.Size(75, 23);
			this.btnSwap.TabIndex = 8;
			this.btnSwap.Text = "Swap";
			this.btnSwap.UseVisualStyleBackColor = true;
			this.btnSwap.Click += new System.EventHandler(this.btnSwap_Click);
			// 
			// CaptureWebSite
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(782, 557);
			this.Controls.Add(this.btnSwap);
			this.Controls.Add(this.web);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.txtOutputDir);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.lbMsgs);
			this.Controls.Add(this.txtURL);
			this.Controls.Add(this.label1);
			this.Name = "CaptureWebSite";
			this.Text = "Capture Web Site";
			this.Load += new System.EventHandler(this.CaptureWebSite_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtURL;
		private System.Windows.Forms.ListBox lbMsgs;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.TextBox txtOutputDir;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.WebBrowser web;
		private System.Windows.Forms.Button btnSwap;
	}
}

