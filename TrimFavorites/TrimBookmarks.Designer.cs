namespace TrimFavorites {
	partial class TrimBookmarks {
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
			this.BtnGo = new System.Windows.Forms.Button();
			this.TvBms = new System.Windows.Forms.TreeView();
			this.SuspendLayout();
			// 
			// BtnGo
			// 
			this.BtnGo.Location = new System.Drawing.Point(1397, 48);
			this.BtnGo.Name = "BtnGo";
			this.BtnGo.Size = new System.Drawing.Size(75, 23);
			this.BtnGo.TabIndex = 0;
			this.BtnGo.Text = "Go";
			this.BtnGo.UseVisualStyleBackColor = true;
			this.BtnGo.Click += new System.EventHandler(this.BtnGo_Click);
			// 
			// TvBms
			// 
			this.TvBms.Location = new System.Drawing.Point(32, 27);
			this.TvBms.Name = "TvBms";
			this.TvBms.Size = new System.Drawing.Size(627, 892);
			this.TvBms.TabIndex = 1;
			// 
			// TrimBookmarks
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1691, 969);
			this.Controls.Add(this.TvBms);
			this.Controls.Add(this.BtnGo);
			this.Name = "TrimBookmarks";
			this.Text = "Trim Bookmarks";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button BtnGo;
		private System.Windows.Forms.TreeView TvBms;
	}
}

