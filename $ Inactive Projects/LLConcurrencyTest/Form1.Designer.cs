namespace LLConcurrencyTest {
	partial class LLConcurrencyTest {
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
			this.cmbDatabase = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtRCUID = new System.Windows.Forms.TextBox();
			this.txtRCPassword = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.chkTxtOut = new System.Windows.Forms.CheckBox();
			this.chkHtmlOut = new System.Windows.Forms.CheckBox();
			this.chkXmlOut = new System.Windows.Forms.CheckBox();
			this.btnGo = new System.Windows.Forms.Button();
			this.btnNewImporterInstance = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.txtOutputDirectory = new System.Windows.Forms.TextBox();
			this.btnSetOutputDir = new System.Windows.Forms.Button();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.label7 = new System.Windows.Forms.Label();
			this.txtMapCfgID = new System.Windows.Forms.TextBox();
			this.txtEventID = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Database";
			// 
			// cmbDatabase
			// 
			this.cmbDatabase.FormattingEnabled = true;
			this.cmbDatabase.Items.AddRange(new object[] {
            "DEVEL",
            "PROD"});
			this.cmbDatabase.Location = new System.Drawing.Point(90, 13);
			this.cmbDatabase.Name = "cmbDatabase";
			this.cmbDatabase.Size = new System.Drawing.Size(79, 21);
			this.cmbDatabase.TabIndex = 0;
			this.cmbDatabase.SelectedIndexChanged += new System.EventHandler(this.cmbDatabase_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(191, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(55, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "MapCfgID";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 51);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(44, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "RC UID";
			// 
			// txtRCUID
			// 
			this.txtRCUID.Location = new System.Drawing.Point(90, 48);
			this.txtRCUID.Name = "txtRCUID";
			this.txtRCUID.Size = new System.Drawing.Size(79, 20);
			this.txtRCUID.TabIndex = 4;
			// 
			// txtRCPassword
			// 
			this.txtRCPassword.Location = new System.Drawing.Point(269, 48);
			this.txtRCPassword.Name = "txtRCPassword";
			this.txtRCPassword.Size = new System.Drawing.Size(70, 20);
			this.txtRCPassword.TabIndex = 5;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(191, 51);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(71, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "RC Password";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 82);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(39, 13);
			this.label5.TabIndex = 8;
			this.label5.Text = "Output";
			// 
			// chkTxtOut
			// 
			this.chkTxtOut.AutoSize = true;
			this.chkTxtOut.Checked = true;
			this.chkTxtOut.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTxtOut.Location = new System.Drawing.Point(90, 81);
			this.chkTxtOut.Name = "chkTxtOut";
			this.chkTxtOut.Size = new System.Drawing.Size(40, 17);
			this.chkTxtOut.TabIndex = 6;
			this.chkTxtOut.Text = ".txt";
			this.chkTxtOut.UseVisualStyleBackColor = true;
			// 
			// chkHtmlOut
			// 
			this.chkHtmlOut.AutoSize = true;
			this.chkHtmlOut.Location = new System.Drawing.Point(161, 81);
			this.chkHtmlOut.Name = "chkHtmlOut";
			this.chkHtmlOut.Size = new System.Drawing.Size(48, 17);
			this.chkHtmlOut.TabIndex = 7;
			this.chkHtmlOut.Text = ".html";
			this.chkHtmlOut.UseVisualStyleBackColor = true;
			// 
			// chkXmlOut
			// 
			this.chkXmlOut.AutoSize = true;
			this.chkXmlOut.Location = new System.Drawing.Point(236, 81);
			this.chkXmlOut.Name = "chkXmlOut";
			this.chkXmlOut.Size = new System.Drawing.Size(44, 17);
			this.chkXmlOut.TabIndex = 8;
			this.chkXmlOut.Text = ".xml";
			this.chkXmlOut.UseVisualStyleBackColor = true;
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(455, 77);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 10;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// btnNewImporterInstance
			// 
			this.btnNewImporterInstance.Location = new System.Drawing.Point(314, 77);
			this.btnNewImporterInstance.Name = "btnNewImporterInstance";
			this.btnNewImporterInstance.Size = new System.Drawing.Size(109, 23);
			this.btnNewImporterInstance.TabIndex = 9;
			this.btnNewImporterInstance.Text = "New Importer";
			this.btnNewImporterInstance.UseVisualStyleBackColor = true;
			this.btnNewImporterInstance.Click += new System.EventHandler(this.btnNewImporterInstance_Click);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(369, 51);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(81, 13);
			this.label6.TabIndex = 14;
			this.label6.Text = "OutputDirectory";
			// 
			// txtOutputDirectory
			// 
			this.txtOutputDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtOutputDirectory.Location = new System.Drawing.Point(456, 48);
			this.txtOutputDirectory.Name = "txtOutputDirectory";
			this.txtOutputDirectory.Size = new System.Drawing.Size(328, 20);
			this.txtOutputDirectory.TabIndex = 2;
			// 
			// btnSetOutputDir
			// 
			this.btnSetOutputDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSetOutputDir.Location = new System.Drawing.Point(802, 46);
			this.btnSetOutputDir.Name = "btnSetOutputDir";
			this.btnSetOutputDir.Size = new System.Drawing.Size(75, 23);
			this.btnSetOutputDir.TabIndex = 3;
			this.btnSetOutputDir.Text = "Output Dir";
			this.btnSetOutputDir.UseVisualStyleBackColor = true;
			this.btnSetOutputDir.Click += new System.EventHandler(this.btnSetOutputDir_Click);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutPanel1.AutoScroll = true;
			this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(15, 119);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(862, 297);
			this.flowLayoutPanel1.TabIndex = 17;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(369, 16);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(49, 13);
			this.label7.TabIndex = 19;
			this.label7.Text = "Event ID";
			// 
			// txtMapCfgID
			// 
			this.txtMapCfgID.Location = new System.Drawing.Point(269, 13);
			this.txtMapCfgID.Name = "txtMapCfgID";
			this.txtMapCfgID.Size = new System.Drawing.Size(70, 20);
			this.txtMapCfgID.TabIndex = 21;
			// 
			// txtEventID
			// 
			this.txtEventID.Location = new System.Drawing.Point(456, 13);
			this.txtEventID.Name = "txtEventID";
			this.txtEventID.Size = new System.Drawing.Size(70, 20);
			this.txtEventID.TabIndex = 22;
			// 
			// LLConcurrencyTest
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(889, 428);
			this.Controls.Add(this.txtEventID);
			this.Controls.Add(this.txtMapCfgID);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.btnSetOutputDir);
			this.Controls.Add(this.txtOutputDirectory);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.btnNewImporterInstance);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.chkXmlOut);
			this.Controls.Add(this.chkHtmlOut);
			this.Controls.Add(this.chkTxtOut);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.txtRCPassword);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtRCUID);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cmbDatabase);
			this.Controls.Add(this.label1);
			this.Name = "LLConcurrencyTest";
			this.Text = "LeadsLightning Concurrency Tester";
			this.Load += new System.EventHandler(this.LLConcurrencyTest_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbDatabase;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtRCUID;
		private System.Windows.Forms.TextBox txtRCPassword;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox chkTxtOut;
		private System.Windows.Forms.CheckBox chkHtmlOut;
		private System.Windows.Forms.CheckBox chkXmlOut;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.Button btnNewImporterInstance;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtOutputDirectory;
		private System.Windows.Forms.Button btnSetOutputDir;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtMapCfgID;
		private System.Windows.Forms.TextBox txtEventID;
	}
}

