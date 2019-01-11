namespace TrimSharpReaderCache {
	partial class TrimSharpReaderCache {
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
			this.txtCachePath = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.btnGo = new System.Windows.Forms.Button();
			this.lbMsgs = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cbMonthsToKeep = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// txtCachePath
			// 
			this.txtCachePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtCachePath.Location = new System.Drawing.Point(121, 31);
			this.txtCachePath.Name = "txtCachePath";
			this.txtCachePath.Size = new System.Drawing.Size(675, 22);
			this.txtCachePath.TabIndex = 0;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Location = new System.Drawing.Point(814, 31);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(75, 23);
			this.btnBrowse.TabIndex = 1;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(22, 31);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(81, 17);
			this.label1.TabIndex = 2;
			this.label1.Text = "Cache Path";
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(25, 76);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 3;
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
			this.lbMsgs.Location = new System.Drawing.Point(25, 124);
			this.lbMsgs.Name = "lbMsgs";
			this.lbMsgs.Size = new System.Drawing.Size(864, 276);
			this.lbMsgs.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(133, 79);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(143, 17);
			this.label2.TabIndex = 5;
			this.label2.Text = "Keep the most recent";
			// 
			// cbMonthsToKeep
			// 
			this.cbMonthsToKeep.DisplayMember = "12";
			this.cbMonthsToKeep.FormattingEnabled = true;
			this.cbMonthsToKeep.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "12",
            "18",
            "24"});
			this.cbMonthsToKeep.Location = new System.Drawing.Point(305, 76);
			this.cbMonthsToKeep.Name = "cbMonthsToKeep";
			this.cbMonthsToKeep.Size = new System.Drawing.Size(52, 24);
			this.cbMonthsToKeep.TabIndex = 6;
			this.cbMonthsToKeep.ValueMember = "12";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(392, 82);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(54, 17);
			this.label3.TabIndex = 7;
			this.label3.Text = "Months";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(920, 418);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cbMonthsToKeep);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lbMsgs);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.txtCachePath);
			this.Name = "Form1";
			this.Text = "Trim SharpReader Cache";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtCachePath;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.ListBox lbMsgs;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cbMonthsToKeep;
		private System.Windows.Forms.Label label3;
	}
}

