namespace LLConcurrencyTest {
	partial class ImporterDefinition {
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
			this.btnBrowseFilename = new System.Windows.Forms.Button();
			this.txtFilename = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.txtTerminalID = new System.Windows.Forms.TextBox();
			this.txtUserID = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.chkDataIsExpanded = new System.Windows.Forms.CheckBox();
			this.chkIgnoreFirstRecord = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtMinRecsPerImport = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.txtMaxRecsPerImport = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txtMaxSecsBetweenImports = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.txtMinSecsBetweenImports = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnBrowseFilename
			// 
			this.btnBrowseFilename.Location = new System.Drawing.Point(13, 13);
			this.btnBrowseFilename.Name = "btnBrowseFilename";
			this.btnBrowseFilename.Size = new System.Drawing.Size(120, 23);
			this.btnBrowseFilename.TabIndex = 0;
			this.btnBrowseFilename.Text = "Browse Filename";
			this.btnBrowseFilename.UseVisualStyleBackColor = true;
			this.btnBrowseFilename.Click += new System.EventHandler(this.btnBrowseFilename_Click);
			// 
			// txtFilename
			// 
			this.txtFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFilename.Location = new System.Drawing.Point(162, 15);
			this.txtFilename.Name = "txtFilename";
			this.txtFilename.Size = new System.Drawing.Size(481, 20);
			this.txtFilename.TabIndex = 0;
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(13, 201);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 9;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(139, 201);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 10;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(10, 85);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 23);
			this.label1.TabIndex = 4;
			this.label1.Text = "Terminal ID";
			// 
			// txtTerminalID
			// 
			this.txtTerminalID.Location = new System.Drawing.Point(162, 88);
			this.txtTerminalID.Name = "txtTerminalID";
			this.txtTerminalID.Size = new System.Drawing.Size(99, 20);
			this.txtTerminalID.TabIndex = 2;
			// 
			// txtUserID
			// 
			this.txtUserID.Location = new System.Drawing.Point(162, 51);
			this.txtUserID.Name = "txtUserID";
			this.txtUserID.Size = new System.Drawing.Size(99, 20);
			this.txtUserID.TabIndex = 1;
			this.txtUserID.Text = "Con17";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(10, 54);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(120, 23);
			this.label2.TabIndex = 6;
			this.label2.Text = "User ID";
			// 
			// chkDataIsExpanded
			// 
			this.chkDataIsExpanded.AutoSize = true;
			this.chkDataIsExpanded.Checked = true;
			this.chkDataIsExpanded.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkDataIsExpanded.Location = new System.Drawing.Point(13, 121);
			this.chkDataIsExpanded.Name = "chkDataIsExpanded";
			this.chkDataIsExpanded.Size = new System.Drawing.Size(111, 17);
			this.chkDataIsExpanded.TabIndex = 3;
			this.chkDataIsExpanded.Text = "Data Is Expanded";
			this.chkDataIsExpanded.UseVisualStyleBackColor = true;
			// 
			// chkIgnoreFirstRecord
			// 
			this.chkIgnoreFirstRecord.AutoSize = true;
			this.chkIgnoreFirstRecord.Checked = true;
			this.chkIgnoreFirstRecord.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkIgnoreFirstRecord.Location = new System.Drawing.Point(13, 154);
			this.chkIgnoreFirstRecord.Name = "chkIgnoreFirstRecord";
			this.chkIgnoreFirstRecord.Size = new System.Drawing.Size(116, 17);
			this.chkIgnoreFirstRecord.TabIndex = 4;
			this.chkIgnoreFirstRecord.Text = "Ignore First Record";
			this.chkIgnoreFirstRecord.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(330, 54);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(120, 23);
			this.label3.TabIndex = 9;
			this.label3.Text = "Records per Import";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtMinRecsPerImport
			// 
			this.txtMinRecsPerImport.Location = new System.Drawing.Point(333, 103);
			this.txtMinRecsPerImport.Name = "txtMinRecsPerImport";
			this.txtMinRecsPerImport.Size = new System.Drawing.Size(43, 20);
			this.txtMinRecsPerImport.TabIndex = 5;
			this.txtMinRecsPerImport.Text = "1";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(330, 77);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(46, 23);
			this.label4.TabIndex = 11;
			this.label4.Text = "Min";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(404, 77);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(46, 23);
			this.label5.TabIndex = 13;
			this.label5.Text = "Max";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtMaxRecsPerImport
			// 
			this.txtMaxRecsPerImport.Location = new System.Drawing.Point(407, 103);
			this.txtMaxRecsPerImport.Name = "txtMaxRecsPerImport";
			this.txtMaxRecsPerImport.Size = new System.Drawing.Size(43, 20);
			this.txtMaxRecsPerImport.TabIndex = 6;
			this.txtMaxRecsPerImport.Text = "1";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(404, 177);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(46, 23);
			this.label6.TabIndex = 18;
			this.label6.Text = "Max";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtMaxSecsBetweenImports
			// 
			this.txtMaxSecsBetweenImports.Location = new System.Drawing.Point(407, 203);
			this.txtMaxSecsBetweenImports.Name = "txtMaxSecsBetweenImports";
			this.txtMaxSecsBetweenImports.Size = new System.Drawing.Size(43, 20);
			this.txtMaxSecsBetweenImports.TabIndex = 8;
			this.txtMaxSecsBetweenImports.Text = "5000";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(330, 177);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(46, 23);
			this.label7.TabIndex = 16;
			this.label7.Text = "Min";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtMinSecsBetweenImports
			// 
			this.txtMinSecsBetweenImports.Location = new System.Drawing.Point(333, 203);
			this.txtMinSecsBetweenImports.Name = "txtMinSecsBetweenImports";
			this.txtMinSecsBetweenImports.Size = new System.Drawing.Size(43, 20);
			this.txtMinSecsBetweenImports.TabIndex = 7;
			this.txtMinSecsBetweenImports.Text = "1000";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(330, 141);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(120, 36);
			this.label8.TabIndex = 14;
			this.label8.Text = "Milliseconds between Imports";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ImporterDefinition
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(655, 246);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtMaxSecsBetweenImports);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.txtMinSecsBetweenImports);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.txtMaxRecsPerImport);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtMinRecsPerImport);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.chkIgnoreFirstRecord);
			this.Controls.Add(this.chkDataIsExpanded);
			this.Controls.Add(this.txtUserID);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtTerminalID);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.txtFilename);
			this.Controls.Add(this.btnBrowseFilename);
			this.Name = "ImporterDefinition";
			this.Text = "Define Importer Instance";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImporterDefinition_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnBrowseFilename;
		private System.Windows.Forms.TextBox txtFilename;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtTerminalID;
		private System.Windows.Forms.TextBox txtUserID;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox chkDataIsExpanded;
		private System.Windows.Forms.CheckBox chkIgnoreFirstRecord;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtMinRecsPerImport;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtMaxRecsPerImport;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtMaxSecsBetweenImports;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtMinSecsBetweenImports;
		private System.Windows.Forms.Label label8;
	}
}