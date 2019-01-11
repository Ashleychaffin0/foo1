namespace MyTVGuide {
	partial class MyTVGuide {
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
			this.web1 = new System.Windows.Forms.WebBrowser();
			this.btnGo = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// web1
			// 
			this.web1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.web1.Location = new System.Drawing.Point(12, 52);
			this.web1.MinimumSize = new System.Drawing.Size(20, 20);
			this.web1.Name = "web1";
			this.web1.Size = new System.Drawing.Size(859, 486);
			this.web1.TabIndex = 0;
			this.web1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.web1_DocumentCompleted);
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(361, 13);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 1;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// MyTVGuide
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(883, 550);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.web1);
			this.Name = "MyTVGuide";
			this.Text = "My TV Guide";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.WebBrowser web1;
		private System.Windows.Forms.Button btnGo;
	}
}

