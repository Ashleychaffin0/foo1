namespace FindInZip {
	partial class FindInZip {
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
			this.label1 = new System.Windows.Forms.Label();
			this.LbFIlenames = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// BtnGo
			// 
			this.BtnGo.Location = new System.Drawing.Point(28, 48);
			this.BtnGo.Name = "BtnGo";
			this.BtnGo.Size = new System.Drawing.Size(75, 23);
			this.BtnGo.TabIndex = 0;
			this.BtnGo.Text = "Go";
			this.BtnGo.UseVisualStyleBackColor = true;
			this.BtnGo.Click += new System.EventHandler(this.BtnGo_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(25, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(165, 17);
			this.label1.TabIndex = 1;
			this.label1.Text = "Add Filename:, StartInDir";
			// 
			// LbFIlenames
			// 
			this.LbFIlenames.FormattingEnabled = true;
			this.LbFIlenames.ItemHeight = 16;
			this.LbFIlenames.Location = new System.Drawing.Point(28, 92);
			this.LbFIlenames.Name = "LbFIlenames";
			this.LbFIlenames.Size = new System.Drawing.Size(760, 340);
			this.LbFIlenames.TabIndex = 2;
			// 
			// FindInZip
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.LbFIlenames);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.BtnGo);
			this.Name = "FindInZip";
			this.Text = "Find In Zip";
			this.Load += new System.EventHandler(this.FindInZip_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button BtnGo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox LbFIlenames;
	}
}

