namespace WinFormsChannel9 {
	partial class EditKeywords {
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
			this.lbKeywords = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtKeyword = new System.Windows.Forms.TextBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lbKeywords
			// 
			this.lbKeywords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.lbKeywords.FormattingEnabled = true;
			this.lbKeywords.Location = new System.Drawing.Point(25, 44);
			this.lbKeywords.Name = "lbKeywords";
			this.lbKeywords.Size = new System.Drawing.Size(275, 407);
			this.lbKeywords.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(22, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Keywords";
			// 
			// txtKeyword
			// 
			this.txtKeyword.Location = new System.Drawing.Point(329, 47);
			this.txtKeyword.Name = "txtKeyword";
			this.txtKeyword.Size = new System.Drawing.Size(296, 20);
			this.txtKeyword.TabIndex = 2;
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(653, 47);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(75, 23);
			this.btnAdd.TabIndex = 3;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// EditKeywords
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(794, 468);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.txtKeyword);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lbKeywords);
			this.Name = "EditKeywords";
			this.Text = "Edit Keywords";
			this.Load += new System.EventHandler(this.EditKeywords_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox lbKeywords;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtKeyword;
		private System.Windows.Forms.Button btnAdd;
	}
}