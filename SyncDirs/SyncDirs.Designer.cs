namespace SyncDirs {
	partial class SyncDirs {
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
			this.TxtMasterFolder = new System.Windows.Forms.TextBox();
			this.BtnBrowse1 = new System.Windows.Forms.Button();
			this.BtnBrowse2 = new System.Windows.Forms.Button();
			this.TxtBackupFolder = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.BtnGo = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(34, 36);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(95, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Master Folder";
			// 
			// TxtMasterFolder
			// 
			this.TxtMasterFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TxtMasterFolder.Location = new System.Drawing.Point(135, 33);
			this.TxtMasterFolder.Name = "TxtMasterFolder";
			this.TxtMasterFolder.Size = new System.Drawing.Size(539, 22);
			this.TxtMasterFolder.TabIndex = 1;
			// 
			// BtnBrowse1
			// 
			this.BtnBrowse1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnBrowse1.Location = new System.Drawing.Point(699, 30);
			this.BtnBrowse1.Name = "BtnBrowse1";
			this.BtnBrowse1.Size = new System.Drawing.Size(75, 23);
			this.BtnBrowse1.TabIndex = 2;
			this.BtnBrowse1.Text = "Browse";
			this.BtnBrowse1.UseVisualStyleBackColor = true;
			// 
			// BtnBrowse2
			// 
			this.BtnBrowse2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnBrowse2.Location = new System.Drawing.Point(699, 72);
			this.BtnBrowse2.Name = "BtnBrowse2";
			this.BtnBrowse2.Size = new System.Drawing.Size(75, 23);
			this.BtnBrowse2.TabIndex = 5;
			this.BtnBrowse2.Text = "Browse";
			this.BtnBrowse2.UseVisualStyleBackColor = true;
			// 
			// TxtBackupFolder
			// 
			this.TxtBackupFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TxtBackupFolder.Location = new System.Drawing.Point(135, 75);
			this.TxtBackupFolder.Name = "TxtBackupFolder";
			this.TxtBackupFolder.Size = new System.Drawing.Size(539, 22);
			this.TxtBackupFolder.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(34, 78);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(99, 17);
			this.label2.TabIndex = 3;
			this.label2.Text = "Backup Folder";
			// 
			// BtnGo
			// 
			this.BtnGo.Location = new System.Drawing.Point(37, 121);
			this.BtnGo.Name = "BtnGo";
			this.BtnGo.Size = new System.Drawing.Size(75, 23);
			this.BtnGo.TabIndex = 6;
			this.BtnGo.Text = "Go";
			this.BtnGo.UseVisualStyleBackColor = true;
			this.BtnGo.Click += new System.EventHandler(this.BtnGo_Click);
			// 
			// SyncDirs
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.BtnGo);
			this.Controls.Add(this.BtnBrowse2);
			this.Controls.Add(this.TxtBackupFolder);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.BtnBrowse1);
			this.Controls.Add(this.TxtMasterFolder);
			this.Controls.Add(this.label1);
			this.Name = "SyncDirs";
			this.Text = "Sync Folders";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox TxtMasterFolder;
		private System.Windows.Forms.Button BtnBrowse1;
		private System.Windows.Forms.Button BtnBrowse2;
		private System.Windows.Forms.TextBox TxtBackupFolder;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button BtnGo;
	}
}

