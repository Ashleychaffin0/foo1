namespace LRSLibraryBooks {
	partial class PromptForString {
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
			this.lblName = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnNewGenreCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblName
			// 
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(13, 13);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(17, 13);
			this.lblName.TabIndex = 0;
			this.lblName.Text = "lbl";
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(115, 13);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(157, 20);
			this.txtName.TabIndex = 1;
			this.txtName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtName_KeyPress);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(115, 59);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnNewGenreCancel
			// 
			this.btnNewGenreCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNewGenreCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnNewGenreCancel.Location = new System.Drawing.Point(196, 59);
			this.btnNewGenreCancel.Name = "btnNewGenreCancel";
			this.btnNewGenreCancel.Size = new System.Drawing.Size(75, 23);
			this.btnNewGenreCancel.TabIndex = 3;
			this.btnNewGenreCancel.Text = "Cancel";
			this.btnNewGenreCancel.UseVisualStyleBackColor = true;
			// 
			// PromptForString
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 99);
			this.Controls.Add(this.btnNewGenreCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.lblName);
			this.Name = "PromptForString";
			this.Text = "Specify string";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnNewGenreCancel;
	}
}