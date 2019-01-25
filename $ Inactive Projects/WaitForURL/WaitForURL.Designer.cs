namespace WaitForURL {
	partial class WaitForURL {
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
			this.lblURL = new System.Windows.Forms.Label();
			this.txtURL = new System.Windows.Forms.TextBox();
			this.btnGo = new System.Windows.Forms.Button();
			this.web = new System.Windows.Forms.WebBrowser();
			this.statMsg = new System.Windows.Forms.StatusStrip();
			this.SuspendLayout();
			// 
			// lblURL
			// 
			this.lblURL.AutoSize = true;
			this.lblURL.Location = new System.Drawing.Point(12, 31);
			this.lblURL.Name = "lblURL";
			this.lblURL.Size = new System.Drawing.Size(29, 13);
			this.lblURL.TabIndex = 0;
			this.lblURL.Text = "URL";
			// 
			// txtURL
			// 
			this.txtURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtURL.Location = new System.Drawing.Point(47, 28);
			this.txtURL.Name = "txtURL";
			this.txtURL.Size = new System.Drawing.Size(703, 20);
			this.txtURL.TabIndex = 1;
			this.txtURL.Text = "http://www.perimeterinstitute.ca/members";
			// 
			// btnGo
			// 
			this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGo.Location = new System.Drawing.Point(756, 26);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 2;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// web
			// 
			this.web.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.web.Location = new System.Drawing.Point(13, 75);
			this.web.MinimumSize = new System.Drawing.Size(20, 20);
			this.web.Name = "web";
			this.web.Size = new System.Drawing.Size(811, 384);
			this.web.TabIndex = 3;
			this.web.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.web_Navigated);
			this.web.ProgressChanged += new System.Windows.Forms.WebBrowserProgressChangedEventHandler(this.web_ProgressChanged);
			// 
			// statMsg
			// 
			this.statMsg.Location = new System.Drawing.Point(0, 463);
			this.statMsg.Name = "statMsg";
			this.statMsg.Size = new System.Drawing.Size(836, 22);
			this.statMsg.TabIndex = 4;
			this.statMsg.Text = "statusStrip1";
			// 
			// WaitForURL
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(836, 485);
			this.Controls.Add(this.statMsg);
			this.Controls.Add(this.web);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.txtURL);
			this.Controls.Add(this.lblURL);
			this.Name = "WaitForURL";
			this.Text = "Wait for URL";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblURL;
		private System.Windows.Forms.TextBox txtURL;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.WebBrowser web;
		private System.Windows.Forms.StatusStrip statMsg;
	}
}

