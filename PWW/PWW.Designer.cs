using System;

namespace PWW
{
	partial class PWW
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.cmbUsers = new System.Windows.Forms.ComboBox();
            this.CmbSites = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LblSiteUrl = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.LblLoginID = new System.Windows.Forms.Label();
            this.BtnCopyLoginID = new System.Windows.Forms.Button();
            this.BtnCopyPassword = new System.Windows.Forms.Button();
            this.LblPassword = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.LblComments = new System.Windows.Forms.Label();
            this.BtnCopySiteUrl = new System.Windows.Forms.Button();
            this.CmbCategories = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(33, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(358, 51);
            this.label1.TabIndex = 0;
            this.label1.Text = "User";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cmbUsers
            // 
            this.cmbUsers.FormattingEnabled = true;
            this.cmbUsers.Location = new System.Drawing.Point(33, 68);
            this.cmbUsers.Name = "cmbUsers";
            this.cmbUsers.Size = new System.Drawing.Size(358, 49);
            this.cmbUsers.TabIndex = 1;
            this.cmbUsers.SelectedIndexChanged += new System.EventHandler(this.CmbUsers_SelectedIndexChanged);
            // 
            // CmbSites
            // 
            this.CmbSites.FormattingEnabled = true;
            this.CmbSites.Location = new System.Drawing.Point(33, 170);
            this.CmbSites.Name = "CmbSites";
            this.CmbSites.Size = new System.Drawing.Size(392, 49);
            this.CmbSites.TabIndex = 4;
            this.CmbSites.SelectedIndexChanged += new System.EventHandler(this.CmbSites_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(33, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(392, 51);
            this.label2.TabIndex = 3;
            this.label2.Text = "Sites";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(483, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(423, 51);
            this.label3.TabIndex = 5;
            this.label3.Text = "Site Url";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // LblSiteUrl
            // 
            this.LblSiteUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblSiteUrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LblSiteUrl.Location = new System.Drawing.Point(483, 173);
            this.LblSiteUrl.Name = "LblSiteUrl";
            this.LblSiteUrl.Size = new System.Drawing.Size(423, 51);
            this.LblSiteUrl.TabIndex = 6;
            this.LblSiteUrl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(33, 241);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(321, 51);
            this.label4.TabIndex = 7;
            this.label4.Text = "LoginID";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // LblLoginID
            // 
            this.LblLoginID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LblLoginID.Location = new System.Drawing.Point(33, 281);
            this.LblLoginID.Name = "LblLoginID";
            this.LblLoginID.Size = new System.Drawing.Size(321, 51);
            this.LblLoginID.TabIndex = 8;
            this.LblLoginID.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // BtnCopyLoginID
            // 
            this.BtnCopyLoginID.Location = new System.Drawing.Point(397, 274);
            this.BtnCopyLoginID.Name = "BtnCopyLoginID";
            this.BtnCopyLoginID.Size = new System.Drawing.Size(139, 54);
            this.BtnCopyLoginID.TabIndex = 9;
            this.BtnCopyLoginID.Text = "Copy";
            this.BtnCopyLoginID.UseVisualStyleBackColor = true;
            this.BtnCopyLoginID.Click += new System.EventHandler(this.BtnCopyLoginID_Click);
            // 
            // BtnCopyPassword
            // 
            this.BtnCopyPassword.Location = new System.Drawing.Point(1143, 274);
            this.BtnCopyPassword.Name = "BtnCopyPassword";
            this.BtnCopyPassword.Size = new System.Drawing.Size(139, 54);
            this.BtnCopyPassword.TabIndex = 12;
            this.BtnCopyPassword.Text = "Copy";
            this.BtnCopyPassword.UseVisualStyleBackColor = true;
            this.BtnCopyPassword.Click += new System.EventHandler(this.BtnCopyPassword_Click);
            // 
            // LblPassword
            // 
            this.LblPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LblPassword.Location = new System.Drawing.Point(699, 280);
            this.LblPassword.Name = "LblPassword";
            this.LblPassword.Size = new System.Drawing.Size(423, 51);
            this.LblPassword.TabIndex = 11;
            this.LblPassword.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(699, 240);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(202, 51);
            this.label6.TabIndex = 10;
            this.label6.Text = "Password";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(33, 365);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(172, 51);
            this.label5.TabIndex = 13;
            this.label5.Text = "Comments";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // LblComments
            // 
            this.LblComments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblComments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LblComments.Location = new System.Drawing.Point(33, 416);
            this.LblComments.Name = "LblComments";
            this.LblComments.Size = new System.Drawing.Size(1287, 221);
            this.LblComments.TabIndex = 14;
            // 
            // BtnCopySiteUrl
            // 
            this.BtnCopySiteUrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCopySiteUrl.Location = new System.Drawing.Point(927, 166);
            this.BtnCopySiteUrl.Name = "BtnCopySiteUrl";
            this.BtnCopySiteUrl.Size = new System.Drawing.Size(139, 54);
            this.BtnCopySiteUrl.TabIndex = 12;
            this.BtnCopySiteUrl.Text = "Copy";
            this.BtnCopySiteUrl.UseVisualStyleBackColor = true;
            this.BtnCopySiteUrl.Click += new System.EventHandler(this.BtnCopySiteUrl_Click);
            // 
            // CmbCategories
            // 
            this.CmbCategories.FormattingEnabled = true;
            this.CmbCategories.Location = new System.Drawing.Point(483, 68);
            this.CmbCategories.Name = "CmbCategories";
            this.CmbCategories.Size = new System.Drawing.Size(423, 49);
            this.CmbCategories.TabIndex = 16;
            this.CmbCategories.SelectedIndexChanged += new System.EventHandler(this.CmbCategories_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(483, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(423, 51);
            this.label7.TabIndex = 15;
            this.label7.Text = "Categories";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // PWW
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 41F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(1346, 646);
            this.Controls.Add(this.CmbCategories);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.LblComments);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.BtnCopySiteUrl);
            this.Controls.Add(this.BtnCopyPassword);
            this.Controls.Add(this.LblPassword);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.BtnCopyLoginID);
            this.Controls.Add(this.LblLoginID);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.LblSiteUrl);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CmbSites);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbUsers);
            this.Controls.Add(this.label1);
            this.Name = "PWW";
            this.Text = "PWW";
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbUsers;
		private System.Windows.Forms.ComboBox CmbSites;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label LblSiteUrl;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label LblLoginID;
		private System.Windows.Forms.Button BtnCopyLoginID;
		private System.Windows.Forms.Button BtnCopyPassword;
		private System.Windows.Forms.Label LblPassword;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label LblComments;
		private System.Windows.Forms.Button BtnCopySiteUrl;
		private System.Windows.Forms.ComboBox CmbCategories;
		private System.Windows.Forms.Label label7;
	}
}

