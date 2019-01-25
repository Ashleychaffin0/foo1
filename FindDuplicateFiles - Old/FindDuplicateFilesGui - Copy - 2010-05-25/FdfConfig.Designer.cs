namespace FindDuplicateFilesGui {
	partial class FdfOptions {
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
			this.components = new System.ComponentModel.Container();
			this.btnEditFileMasksToInclude_Add = new System.Windows.Forms.Button();
			this.cbFileMasksToInclude = new System.Windows.Forms.ComboBox();
			this.lblFileMasksInc = new System.Windows.Forms.Label();
			this.btnEditDirectoriesToExclude_Add = new System.Windows.Forms.Button();
			this.cbDirectoriesToExclude = new System.Windows.Forms.ComboBox();
			this.lblDirExc = new System.Windows.Forms.Label();
			this.btnEditDirectoriesToInclude_Add = new System.Windows.Forms.Button();
			this.cbDirectoriesToInclude = new System.Windows.Forms.ComboBox();
			this.lblDirInc = new System.Windows.Forms.Label();
			this.btnEditFileMasksToExclude_Add = new System.Windows.Forms.Button();
			this.cbFileMasksToExclude = new System.Windows.Forms.ComboBox();
			this.lblFileMasksEx = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radSizeInTB = new System.Windows.Forms.RadioButton();
			this.radSizeInGB = new System.Windows.Forms.RadioButton();
			this.radSizeInMB = new System.Windows.Forms.RadioButton();
			this.radSizeInK = new System.Windows.Forms.RadioButton();
			this.radSizeInBytes = new System.Windows.Forms.RadioButton();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.txtMinFileSize = new System.Windows.Forms.TextBox();
			this.txtMaxFileSize = new System.Windows.Forms.TextBox();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnEditFileMasksToInclude_Add
			// 
			this.btnEditFileMasksToInclude_Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnEditFileMasksToInclude_Add.Location = new System.Drawing.Point(750, 118);
			this.btnEditFileMasksToInclude_Add.Name = "btnEditFileMasksToInclude_Add";
			this.btnEditFileMasksToInclude_Add.Size = new System.Drawing.Size(75, 23);
			this.btnEditFileMasksToInclude_Add.TabIndex = 7;
			this.btnEditFileMasksToInclude_Add.Text = "Edit";
			this.btnEditFileMasksToInclude_Add.UseVisualStyleBackColor = true;
			this.btnEditFileMasksToInclude_Add.Click += new System.EventHandler(this.btnEditFileMasksToInclude_Click);
			// 
			// cbFileMasksToInclude
			// 
			this.cbFileMasksToInclude.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cbFileMasksToInclude.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbFileMasksToInclude.FormattingEnabled = true;
			this.cbFileMasksToInclude.Location = new System.Drawing.Point(226, 113);
			this.cbFileMasksToInclude.Name = "cbFileMasksToInclude";
			this.cbFileMasksToInclude.Size = new System.Drawing.Size(499, 28);
			this.cbFileMasksToInclude.TabIndex = 6;
			// 
			// lblFileMasksInc
			// 
			this.lblFileMasksInc.BackColor = System.Drawing.Color.Gold;
			this.lblFileMasksInc.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFileMasksInc.Location = new System.Drawing.Point(29, 113);
			this.lblFileMasksInc.Name = "lblFileMasksInc";
			this.lblFileMasksInc.Size = new System.Drawing.Size(181, 20);
			this.lblFileMasksInc.TabIndex = 20;
			this.lblFileMasksInc.Text = "File Masks to Include";
			// 
			// btnEditDirectoriesToExclude_Edit
			// 
			this.btnEditDirectoriesToExclude_Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnEditDirectoriesToExclude_Add.Location = new System.Drawing.Point(750, 72);
			this.btnEditDirectoriesToExclude_Add.Name = "btnEditDirectoriesToExclude_Add";
			this.btnEditDirectoriesToExclude_Add.Size = new System.Drawing.Size(75, 23);
			this.btnEditDirectoriesToExclude_Add.TabIndex = 4;
			this.btnEditDirectoriesToExclude_Add.Text = "Edit";
			this.btnEditDirectoriesToExclude_Add.UseVisualStyleBackColor = true;
			this.btnEditDirectoriesToExclude_Add.Click += new System.EventHandler(this.btnEditDirectoriesToExclude_Click);
			// 
			// cbDirectoriesToExclude
			// 
			this.cbDirectoriesToExclude.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cbDirectoriesToExclude.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbDirectoriesToExclude.FormattingEnabled = true;
			this.cbDirectoriesToExclude.Location = new System.Drawing.Point(226, 67);
			this.cbDirectoriesToExclude.Name = "cbDirectoriesToExclude";
			this.cbDirectoriesToExclude.Size = new System.Drawing.Size(499, 28);
			this.cbDirectoriesToExclude.TabIndex = 3;
			// 
			// lblDirExc
			// 
			this.lblDirExc.BackColor = System.Drawing.Color.Gold;
			this.lblDirExc.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblDirExc.Location = new System.Drawing.Point(29, 67);
			this.lblDirExc.Name = "lblDirExc";
			this.lblDirExc.Size = new System.Drawing.Size(181, 20);
			this.lblDirExc.TabIndex = 16;
			this.lblDirExc.Text = "Directories to Exclude";
			// 
			// btnEditDirectoriesToInclude_Edit
			// 
			this.btnEditDirectoriesToInclude_Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnEditDirectoriesToInclude_Add.Location = new System.Drawing.Point(750, 26);
			this.btnEditDirectoriesToInclude_Add.Name = "btnEditDirectoriesToInclude_Add";
			this.btnEditDirectoriesToInclude_Add.Size = new System.Drawing.Size(75, 23);
			this.btnEditDirectoriesToInclude_Add.TabIndex = 1;
			this.btnEditDirectoriesToInclude_Add.Text = "Edit";
			this.btnEditDirectoriesToInclude_Add.UseVisualStyleBackColor = true;
			this.btnEditDirectoriesToInclude_Add.Click += new System.EventHandler(this.btnEditDirectoriesToInclude_Click);
			// 
			// cbDirectoriesToInclude
			// 
			this.cbDirectoriesToInclude.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cbDirectoriesToInclude.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbDirectoriesToInclude.FormattingEnabled = true;
			this.cbDirectoriesToInclude.Location = new System.Drawing.Point(226, 21);
			this.cbDirectoriesToInclude.Name = "cbDirectoriesToInclude";
			this.cbDirectoriesToInclude.Size = new System.Drawing.Size(499, 28);
			this.cbDirectoriesToInclude.TabIndex = 0;
			// 
			// lblDirInc
			// 
			this.lblDirInc.BackColor = System.Drawing.Color.Gold;
			this.lblDirInc.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblDirInc.Location = new System.Drawing.Point(29, 21);
			this.lblDirInc.Name = "lblDirInc";
			this.lblDirInc.Size = new System.Drawing.Size(181, 20);
			this.lblDirInc.TabIndex = 12;
			this.lblDirInc.Text = "Directories to Include";
			// 
			// btnEditFileMasksToExclude_Add
			// 
			this.btnEditFileMasksToExclude_Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnEditFileMasksToExclude_Add.Location = new System.Drawing.Point(750, 164);
			this.btnEditFileMasksToExclude_Add.Name = "btnEditFileMasksToExclude_Add";
			this.btnEditFileMasksToExclude_Add.Size = new System.Drawing.Size(75, 23);
			this.btnEditFileMasksToExclude_Add.TabIndex = 10;
			this.btnEditFileMasksToExclude_Add.Text = "Edit";
			this.btnEditFileMasksToExclude_Add.UseVisualStyleBackColor = true;
			this.btnEditFileMasksToExclude_Add.Click += new System.EventHandler(this.btnFileMasksToExclude_Edit_Click);
			// 
			// cbFileMasksToExclude
			// 
			this.cbFileMasksToExclude.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cbFileMasksToExclude.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbFileMasksToExclude.FormattingEnabled = true;
			this.cbFileMasksToExclude.Location = new System.Drawing.Point(226, 159);
			this.cbFileMasksToExclude.Name = "cbFileMasksToExclude";
			this.cbFileMasksToExclude.Size = new System.Drawing.Size(499, 28);
			this.cbFileMasksToExclude.TabIndex = 9;
			// 
			// lblFileMasksEx
			// 
			this.lblFileMasksEx.BackColor = System.Drawing.Color.Gold;
			this.lblFileMasksEx.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFileMasksEx.Location = new System.Drawing.Point(29, 159);
			this.lblFileMasksEx.Name = "lblFileMasksEx";
			this.lblFileMasksEx.Size = new System.Drawing.Size(181, 20);
			this.lblFileMasksEx.TabIndex = 24;
			this.lblFileMasksEx.Text = "File Masks to Exclude";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(329, 302);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 41);
			this.btnOK.TabIndex = 25;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(459, 302);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 41);
			this.btnCancel.TabIndex = 26;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.Color.Chartreuse;
			this.groupBox1.Controls.Add(this.radSizeInTB);
			this.groupBox1.Controls.Add(this.radSizeInGB);
			this.groupBox1.Controls.Add(this.radSizeInMB);
			this.groupBox1.Controls.Add(this.radSizeInK);
			this.groupBox1.Controls.Add(this.radSizeInBytes);
			this.groupBox1.Location = new System.Drawing.Point(31, 220);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(403, 61);
			this.groupBox1.TabIndex = 27;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "File Size Units";
			// 
			// radSizeInTB
			// 
			this.radSizeInTB.AutoSize = true;
			this.radSizeInTB.Location = new System.Drawing.Point(333, 21);
			this.radSizeInTB.Name = "radSizeInTB";
			this.radSizeInTB.Size = new System.Drawing.Size(47, 21);
			this.radSizeInTB.TabIndex = 4;
			this.radSizeInTB.TabStop = true;
			this.radSizeInTB.Text = "TB";
			this.radSizeInTB.UseVisualStyleBackColor = true;
			// 
			// radSizeInGB
			// 
			this.radSizeInGB.AutoSize = true;
			this.radSizeInGB.Location = new System.Drawing.Point(258, 21);
			this.radSizeInGB.Name = "radSizeInGB";
			this.radSizeInGB.Size = new System.Drawing.Size(49, 21);
			this.radSizeInGB.TabIndex = 3;
			this.radSizeInGB.TabStop = true;
			this.radSizeInGB.Text = "GB";
			this.radSizeInGB.UseVisualStyleBackColor = true;
			// 
			// radSizeInMB
			// 
			this.radSizeInMB.AutoSize = true;
			this.radSizeInMB.Checked = true;
			this.radSizeInMB.Location = new System.Drawing.Point(183, 21);
			this.radSizeInMB.Name = "radSizeInMB";
			this.radSizeInMB.Size = new System.Drawing.Size(49, 21);
			this.radSizeInMB.TabIndex = 2;
			this.radSizeInMB.TabStop = true;
			this.radSizeInMB.Text = "MB";
			this.radSizeInMB.UseVisualStyleBackColor = true;
			// 
			// radSizeInK
			// 
			this.radSizeInK.AutoSize = true;
			this.radSizeInK.Location = new System.Drawing.Point(110, 21);
			this.radSizeInK.Name = "radSizeInK";
			this.radSizeInK.Size = new System.Drawing.Size(47, 21);
			this.radSizeInK.TabIndex = 1;
			this.radSizeInK.TabStop = true;
			this.radSizeInK.Text = "KB";
			this.radSizeInK.UseVisualStyleBackColor = true;
			// 
			// radSizeInBytes
			// 
			this.radSizeInBytes.AutoSize = true;
			this.radSizeInBytes.Location = new System.Drawing.Point(20, 21);
			this.radSizeInBytes.Name = "radSizeInBytes";
			this.radSizeInBytes.Size = new System.Drawing.Size(64, 21);
			this.radSizeInBytes.TabIndex = 0;
			this.radSizeInBytes.TabStop = true;
			this.radSizeInBytes.Text = "Bytes";
			this.radSizeInBytes.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.Gold;
			this.label5.Location = new System.Drawing.Point(476, 220);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(140, 17);
			this.label5.TabIndex = 28;
			this.label5.Text = "Minimum Size";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label6
			// 
			this.label6.BackColor = System.Drawing.Color.Gold;
			this.label6.Location = new System.Drawing.Point(685, 222);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(140, 17);
			this.label6.TabIndex = 29;
			this.label6.Text = "Maximum Size";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtMinFileSize
			// 
			this.txtMinFileSize.Location = new System.Drawing.Point(476, 248);
			this.txtMinFileSize.Name = "txtMinFileSize";
			this.txtMinFileSize.Size = new System.Drawing.Size(140, 22);
			this.txtMinFileSize.TabIndex = 30;
			// 
			// txtMaxFileSize
			// 
			this.txtMaxFileSize.Location = new System.Drawing.Point(685, 248);
			this.txtMaxFileSize.Name = "txtMaxFileSize";
			this.txtMaxFileSize.Size = new System.Drawing.Size(140, 22);
			this.txtMaxFileSize.TabIndex = 31;
			// 
			// toolTip1
			// 
			this.toolTip1.ToolTipTitle = "Hello LRS";
			// 
			// FdfOptions
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(838, 355);
			this.Controls.Add(this.txtMaxFileSize);
			this.Controls.Add(this.txtMinFileSize);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnEditFileMasksToExclude_Add);
			this.Controls.Add(this.cbFileMasksToExclude);
			this.Controls.Add(this.lblFileMasksEx);
			this.Controls.Add(this.btnEditFileMasksToInclude_Add);
			this.Controls.Add(this.cbFileMasksToInclude);
			this.Controls.Add(this.lblFileMasksInc);
			this.Controls.Add(this.btnEditDirectoriesToExclude_Add);
			this.Controls.Add(this.cbDirectoriesToExclude);
			this.Controls.Add(this.lblDirExc);
			this.Controls.Add(this.btnEditDirectoriesToInclude_Add);
			this.Controls.Add(this.cbDirectoriesToInclude);
			this.Controls.Add(this.lblDirInc);
			this.Name = "FdfOptions";
			this.Text = "Configuration Values";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FdfOptions_FormClosing);
			this.Load += new System.EventHandler(this.FdfOptions_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.FdfOptions_Paint);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnEditFileMasksToInclude_Add;
		private System.Windows.Forms.ComboBox cbFileMasksToInclude;
		private System.Windows.Forms.Label lblFileMasksInc;
		private System.Windows.Forms.Button btnEditDirectoriesToExclude_Add;
		private System.Windows.Forms.ComboBox cbDirectoriesToExclude;
		private System.Windows.Forms.Label lblDirExc;
		private System.Windows.Forms.Button btnEditDirectoriesToInclude_Add;
		private System.Windows.Forms.ComboBox cbDirectoriesToInclude;
		private System.Windows.Forms.Label lblDirInc;
		private System.Windows.Forms.Button btnEditFileMasksToExclude_Add;
		private System.Windows.Forms.ComboBox cbFileMasksToExclude;
		private System.Windows.Forms.Label lblFileMasksEx;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radSizeInGB;
		private System.Windows.Forms.RadioButton radSizeInMB;
		private System.Windows.Forms.RadioButton radSizeInK;
		private System.Windows.Forms.RadioButton radSizeInBytes;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtMinFileSize;
		private System.Windows.Forms.TextBox txtMaxFileSize;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.RadioButton radSizeInTB;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}