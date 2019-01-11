namespace TestSecurity1 {
	partial class TestSecurity1 {
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
			this.lbMsgs = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// lbMsgs
			// 
			this.lbMsgs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lbMsgs.FormattingEnabled = true;
			this.lbMsgs.ItemHeight = 16;
			this.lbMsgs.Location = new System.Drawing.Point(12, 6);
			this.lbMsgs.Name = "lbMsgs";
			this.lbMsgs.Size = new System.Drawing.Size(662, 244);
			this.lbMsgs.TabIndex = 0;
			// 
			// TestSecurity1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(686, 262);
			this.Controls.Add(this.lbMsgs);
			this.Name = "TestSecurity1";
			this.Text = "Test Security";
			this.Load += new System.EventHandler(this.TestSecurity1_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox lbMsgs;
	}
}

