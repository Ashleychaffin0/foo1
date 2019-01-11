namespace ZuneGenerateClasses {
	partial class ZuneGenerateClasses {
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
			this.btnBackupZuneDatabase = new System.Windows.Forms.Button();
			this.btnRestoreZuneDatabase = new System.Windows.Forms.Button();
			this.btnGenerateClasses = new System.Windows.Forms.Button();
			this.btnTest1 = new System.Windows.Forms.Button();
			this.btnTest2 = new System.Windows.Forms.Button();
			this.btnSendAllInfoToFile = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnBackupZuneDatabase
			// 
			this.btnBackupZuneDatabase.Location = new System.Drawing.Point(12, 12);
			this.btnBackupZuneDatabase.Name = "btnBackupZuneDatabase";
			this.btnBackupZuneDatabase.Size = new System.Drawing.Size(146, 23);
			this.btnBackupZuneDatabase.TabIndex = 0;
			this.btnBackupZuneDatabase.Text = "Backup Zune Database";
			this.btnBackupZuneDatabase.UseVisualStyleBackColor = true;
			this.btnBackupZuneDatabase.Click += new System.EventHandler(this.btnBackupZuneDatabase_Click);
			// 
			// btnRestoreZuneDatabase
			// 
			this.btnRestoreZuneDatabase.Location = new System.Drawing.Point(185, 12);
			this.btnRestoreZuneDatabase.Name = "btnRestoreZuneDatabase";
			this.btnRestoreZuneDatabase.Size = new System.Drawing.Size(143, 23);
			this.btnRestoreZuneDatabase.TabIndex = 1;
			this.btnRestoreZuneDatabase.Text = "Restore Zune Database";
			this.btnRestoreZuneDatabase.UseVisualStyleBackColor = true;
			this.btnRestoreZuneDatabase.Click += new System.EventHandler(this.btnRestoreZuneDatabase_Click);
			// 
			// btnGenerateClasses
			// 
			this.btnGenerateClasses.Location = new System.Drawing.Point(15, 58);
			this.btnGenerateClasses.Name = "btnGenerateClasses";
			this.btnGenerateClasses.Size = new System.Drawing.Size(143, 23);
			this.btnGenerateClasses.TabIndex = 2;
			this.btnGenerateClasses.Text = "Generate Classes";
			this.btnGenerateClasses.UseVisualStyleBackColor = true;
			this.btnGenerateClasses.Click += new System.EventHandler(this.btnGenerateClasses_Click);
			// 
			// btnTest1
			// 
			this.btnTest1.Location = new System.Drawing.Point(223, 58);
			this.btnTest1.Name = "btnTest1";
			this.btnTest1.Size = new System.Drawing.Size(61, 23);
			this.btnTest1.TabIndex = 3;
			this.btnTest1.Text = "Test 1";
			this.btnTest1.UseVisualStyleBackColor = true;
			this.btnTest1.Click += new System.EventHandler(this.btnTest_Click);
			// 
			// btnTest2
			// 
			this.btnTest2.Location = new System.Drawing.Point(223, 98);
			this.btnTest2.Name = "btnTest2";
			this.btnTest2.Size = new System.Drawing.Size(61, 23);
			this.btnTest2.TabIndex = 4;
			this.btnTest2.Text = "Test 2";
			this.btnTest2.UseVisualStyleBackColor = true;
			this.btnTest2.Click += new System.EventHandler(this.btnTest2_Click);
			// 
			// btnSendAllInfoToFile
			// 
			this.btnSendAllInfoToFile.Location = new System.Drawing.Point(15, 98);
			this.btnSendAllInfoToFile.Name = "btnSendAllInfoToFile";
			this.btnSendAllInfoToFile.Size = new System.Drawing.Size(143, 23);
			this.btnSendAllInfoToFile.TabIndex = 5;
			this.btnSendAllInfoToFile.Text = "Send all info to File";
			this.btnSendAllInfoToFile.UseVisualStyleBackColor = true;
			this.btnSendAllInfoToFile.Click += new System.EventHandler(this.btnSendAllInfoToFile_Click);
			// 
			// ZuneGenerateClasses
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(372, 207);
			this.Controls.Add(this.btnSendAllInfoToFile);
			this.Controls.Add(this.btnTest2);
			this.Controls.Add(this.btnTest1);
			this.Controls.Add(this.btnGenerateClasses);
			this.Controls.Add(this.btnRestoreZuneDatabase);
			this.Controls.Add(this.btnBackupZuneDatabase);
			this.Name = "ZuneGenerateClasses";
			this.Text = "Zune - Generate Classes";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnBackupZuneDatabase;
		private System.Windows.Forms.Button btnRestoreZuneDatabase;
		private System.Windows.Forms.Button btnGenerateClasses;
		private System.Windows.Forms.Button btnTest1;
		private System.Windows.Forms.Button btnTest2;
		private System.Windows.Forms.Button btnSendAllInfoToFile;
	}
}

