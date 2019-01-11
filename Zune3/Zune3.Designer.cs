namespace Zune3 {
	partial class LRSZune3 {
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
			this.btnArtists = new System.Windows.Forms.Button();
			this.dbgTryAllTypes = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnArtists
			// 
			this.btnArtists.Location = new System.Drawing.Point(39, 31);
			this.btnArtists.Name = "btnArtists";
			this.btnArtists.Size = new System.Drawing.Size(75, 23);
			this.btnArtists.TabIndex = 0;
			this.btnArtists.Text = "Artists";
			this.btnArtists.UseVisualStyleBackColor = true;
			this.btnArtists.Click += new System.EventHandler(this.btnArtists_Click);
			// 
			// dbgTryAllTypes
			// 
			this.dbgTryAllTypes.Location = new System.Drawing.Point(39, 69);
			this.dbgTryAllTypes.Name = "dbgTryAllTypes";
			this.dbgTryAllTypes.Size = new System.Drawing.Size(140, 23);
			this.dbgTryAllTypes.TabIndex = 1;
			this.dbgTryAllTypes.Text = "Dbg - Try All Types";
			this.dbgTryAllTypes.UseVisualStyleBackColor = true;
			this.dbgTryAllTypes.Click += new System.EventHandler(this.dbgTryAllTypes_Click);
			// 
			// LRSZune3
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(282, 257);
			this.Controls.Add(this.dbgTryAllTypes);
			this.Controls.Add(this.btnArtists);
			this.Name = "LRSZune3";
			this.Text = "Zune 3";
			this.Load += new System.EventHandler(this.LRSZune3_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnArtists;
		private System.Windows.Forms.Button dbgTryAllTypes;
	}
}

