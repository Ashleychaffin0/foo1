namespace BartBlockForms {
	partial class frmAdminSwitchboard {
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
			this.btnChangeAdminPassword = new System.Windows.Forms.Button();
			this.btnEditAllowedSitesList = new System.Windows.Forms.Button();
			this.btnFinish = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(36, 33);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(414, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Please click one of the following buttons. Click Finish when done.";
			// 
			// btnChangeAdminPassword
			// 
			this.btnChangeAdminPassword.Location = new System.Drawing.Point(39, 80);
			this.btnChangeAdminPassword.Name = "btnChangeAdminPassword";
			this.btnChangeAdminPassword.Size = new System.Drawing.Size(212, 23);
			this.btnChangeAdminPassword.TabIndex = 1;
			this.btnChangeAdminPassword.Text = "Change Admin Password";
			this.btnChangeAdminPassword.UseVisualStyleBackColor = true;
			this.btnChangeAdminPassword.Click += new System.EventHandler(this.btnChangeAdminPassword_Click);
			// 
			// btnEditAllowedSitesList
			// 
			this.btnEditAllowedSitesList.Location = new System.Drawing.Point(39, 133);
			this.btnEditAllowedSitesList.Name = "btnEditAllowedSitesList";
			this.btnEditAllowedSitesList.Size = new System.Drawing.Size(212, 23);
			this.btnEditAllowedSitesList.TabIndex = 2;
			this.btnEditAllowedSitesList.Text = "Edit Allowed Sites List";
			this.btnEditAllowedSitesList.UseVisualStyleBackColor = true;
			this.btnEditAllowedSitesList.Click += new System.EventHandler(this.btnEditAllowedSitesList_Click);
			// 
			// btnFinish
			// 
			this.btnFinish.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnFinish.Location = new System.Drawing.Point(195, 199);
			this.btnFinish.Name = "btnFinish";
			this.btnFinish.Size = new System.Drawing.Size(75, 23);
			this.btnFinish.TabIndex = 3;
			this.btnFinish.Text = "Finish";
			this.btnFinish.UseVisualStyleBackColor = true;
			// 
			// frmAdminSwitchboard
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(520, 253);
			this.Controls.Add(this.btnFinish);
			this.Controls.Add(this.btnEditAllowedSitesList);
			this.Controls.Add(this.btnChangeAdminPassword);
			this.Controls.Add(this.label1);
			this.Name = "frmAdminSwitchboard";
			this.Text = "BartBlock Admin Switchboard";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnChangeAdminPassword;
		private System.Windows.Forms.Button btnEditAllowedSitesList;
		private System.Windows.Forms.Button btnFinish;
	}
}