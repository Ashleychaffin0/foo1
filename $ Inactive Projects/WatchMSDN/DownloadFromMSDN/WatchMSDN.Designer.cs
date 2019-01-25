namespace DownloadFromMSDN {
	partial class WatchMSDN {
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
			this.btnGo = new System.Windows.Forms.Button();
			this.webMSDN = new System.Windows.Forms.WebBrowser();
			this.txtURL = new System.Windows.Forms.TextBox();
			this.btnStop = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(41, 102);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 0;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// webMSDN
			// 
			this.webMSDN.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.webMSDN.Location = new System.Drawing.Point(-1, 150);
			this.webMSDN.MinimumSize = new System.Drawing.Size(20, 20);
			this.webMSDN.Name = "webMSDN";
			this.webMSDN.Size = new System.Drawing.Size(738, 405);
			this.webMSDN.TabIndex = 1;
			this.webMSDN.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webMSDN_DocumentCompleted);
			// 
			// txtURL
			// 
			this.txtURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtURL.Location = new System.Drawing.Point(41, 13);
			this.txtURL.Multiline = true;
			this.txtURL.Name = "txtURL";
			this.txtURL.Size = new System.Drawing.Size(684, 83);
			this.txtURL.TabIndex = 2;
			// 
			// btnStop
			// 
			this.btnStop.Location = new System.Drawing.Point(195, 102);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(75, 23);
			this.btnStop.TabIndex = 3;
			this.btnStop.Text = "Stop";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// WatchMSDN
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(749, 556);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.txtURL);
			this.Controls.Add(this.webMSDN);
			this.Controls.Add(this.btnGo);
			this.Name = "WatchMSDN";
			this.Text = "Watch MSDN";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.WebBrowser webMSDN;
		private System.Windows.Forms.TextBox txtURL;
		private System.Windows.Forms.Button btnStop;
	}
}

