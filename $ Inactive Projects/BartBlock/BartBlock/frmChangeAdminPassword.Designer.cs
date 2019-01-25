namespace BartBlockForms {
	partial class frmChangeAdminPassword {
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
			this.label1 = new System.Windows.Forms.Label();
			this.txtNewPassword = new System.Windows.Forms.TextBox();
			this.txtNewPassword2 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(25, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(135, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Enter new password";
			// 
			// txtNewPassword
			// 
			this.txtNewPassword.Location = new System.Drawing.Point(203, 40);
			this.txtNewPassword.Name = "txtNewPassword";
			this.txtNewPassword.PasswordChar = '*';
			this.txtNewPassword.Size = new System.Drawing.Size(140, 22);
			this.txtNewPassword.TabIndex = 1;
			// 
			// txtNewPassword2
			// 
			this.txtNewPassword2.Location = new System.Drawing.Point(203, 92);
			this.txtNewPassword2.Name = "txtNewPassword2";
			this.txtNewPassword2.PasswordChar = '*';
			this.txtNewPassword2.Size = new System.Drawing.Size(140, 22);
			this.txtNewPassword2.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(25, 92);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(128, 17);
			this.label2.TabIndex = 2;
			this.label2.Text = "Re-enter password";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(89, 138);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(276, 17);
			this.label3.TabIndex = 4;
			this.label3.Text = "Reminder: Passwords are CaSe sENsiTivE";
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(107, 186);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 5;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(269, 186);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// frmChangeAdminPassword
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(487, 260);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtNewPassword2);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtNewPassword);
			this.Controls.Add(this.label1);
			this.Name = "frmChangeAdminPassword";
			this.Text = "Change Admin Password";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		public System.Windows.Forms.TextBox txtNewPassword;
		public System.Windows.Forms.TextBox txtNewPassword2;
	}
}