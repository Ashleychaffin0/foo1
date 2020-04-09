namespace TestLLImporter {
	partial class TestLLImporter {
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
			this.txtMapCfg = new System.Windows.Forms.TextBox();
			this.btnBrowseMapCfg = new System.Windows.Forms.Button();
			this.btnGo = new System.Windows.Forms.Button();
			this.btnBrowseVisitorTxt = new System.Windows.Forms.Button();
			this.txtVisitorTxt = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.lbMsgs = new System.Windows.Forms.ListBox();
			this.chkUseWebService = new System.Windows.Forms.CheckBox();
			this.btnClearListbox = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radKeyedBasicDataImport = new System.Windows.Forms.RadioButton();
			this.radUploadMapCfg = new System.Windows.Forms.RadioButton();
			this.radGetSetupFile = new System.Windows.Forms.RadioButton();
			this.radGetSetupInfo = new System.Windows.Forms.RadioButton();
			this.radImport = new System.Windows.Forms.RadioButton();
			this.lblStatus = new System.Windows.Forms.Label();
			this.chkAppendToOutputFile = new System.Windows.Forms.CheckBox();
			this.lvBadRecs = new System.Windows.Forms.ListView();
			this.txtDebugRecord = new System.Windows.Forms.TextBox();
			this.btnEditVisitor = new System.Windows.Forms.Button();
			this.btnEditMap = new System.Windows.Forms.Button();
			this.radCcLeads = new System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 31);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(47, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Map.Cfg";
			// 
			// txtMapCfg
			// 
			this.txtMapCfg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtMapCfg.Location = new System.Drawing.Point(82, 31);
			this.txtMapCfg.Margin = new System.Windows.Forms.Padding(2);
			this.txtMapCfg.Name = "txtMapCfg";
			this.txtMapCfg.Size = new System.Drawing.Size(299, 20);
			this.txtMapCfg.TabIndex = 1;
			this.txtMapCfg.Text = "C:\\LRS\\$Importer Data\\Disk 4\\Map.cfg";
			// 
			// btnBrowseMapCfg
			// 
			this.btnBrowseMapCfg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseMapCfg.Location = new System.Drawing.Point(396, 31);
			this.btnBrowseMapCfg.Margin = new System.Windows.Forms.Padding(2);
			this.btnBrowseMapCfg.Name = "btnBrowseMapCfg";
			this.btnBrowseMapCfg.Size = new System.Drawing.Size(56, 19);
			this.btnBrowseMapCfg.TabIndex = 2;
			this.btnBrowseMapCfg.Text = "Browse";
			this.btnBrowseMapCfg.UseVisualStyleBackColor = true;
			this.btnBrowseMapCfg.Click += new System.EventHandler(this.btnBrowseMapCfg_Click);
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(224, 111);
			this.btnGo.Margin = new System.Windows.Forms.Padding(2);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(56, 19);
			this.btnGo.TabIndex = 3;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// btnBrowseVisitorTxt
			// 
			this.btnBrowseVisitorTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseVisitorTxt.Location = new System.Drawing.Point(396, 65);
			this.btnBrowseVisitorTxt.Margin = new System.Windows.Forms.Padding(2);
			this.btnBrowseVisitorTxt.Name = "btnBrowseVisitorTxt";
			this.btnBrowseVisitorTxt.Size = new System.Drawing.Size(56, 19);
			this.btnBrowseVisitorTxt.TabIndex = 6;
			this.btnBrowseVisitorTxt.Text = "Browse";
			this.btnBrowseVisitorTxt.UseVisualStyleBackColor = true;
			this.btnBrowseVisitorTxt.Click += new System.EventHandler(this.btnBrowseVisitorTxt_Click);
			// 
			// txtVisitorTxt
			// 
			this.txtVisitorTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtVisitorTxt.Location = new System.Drawing.Point(82, 65);
			this.txtVisitorTxt.Margin = new System.Windows.Forms.Padding(2);
			this.txtVisitorTxt.Name = "txtVisitorTxt";
			this.txtVisitorTxt.Size = new System.Drawing.Size(299, 20);
			this.txtVisitorTxt.TabIndex = 5;
			this.txtVisitorTxt.Text = "C:\\LRS\\$Importer Data\\Disk 4\\Visitor.txt";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 65);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(49, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Visitor.txt";
			// 
			// lbMsgs
			// 
			this.lbMsgs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lbMsgs.FormattingEnabled = true;
			this.lbMsgs.HorizontalScrollbar = true;
			this.lbMsgs.Location = new System.Drawing.Point(19, 250);
			this.lbMsgs.Margin = new System.Windows.Forms.Padding(2);
			this.lbMsgs.Name = "lbMsgs";
			this.lbMsgs.Size = new System.Drawing.Size(500, 212);
			this.lbMsgs.TabIndex = 7;
			// 
			// chkUseWebService
			// 
			this.chkUseWebService.AutoSize = true;
			this.chkUseWebService.Checked = true;
			this.chkUseWebService.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkUseWebService.Location = new System.Drawing.Point(356, 113);
			this.chkUseWebService.Margin = new System.Windows.Forms.Padding(2);
			this.chkUseWebService.Name = "chkUseWebService";
			this.chkUseWebService.Size = new System.Drawing.Size(110, 17);
			this.chkUseWebService.TabIndex = 8;
			this.chkUseWebService.Text = "Use Web Service";
			this.chkUseWebService.UseVisualStyleBackColor = true;
			// 
			// btnClearListbox
			// 
			this.btnClearListbox.Location = new System.Drawing.Point(224, 195);
			this.btnClearListbox.Margin = new System.Windows.Forms.Padding(2);
			this.btnClearListbox.Name = "btnClearListbox";
			this.btnClearListbox.Size = new System.Drawing.Size(110, 19);
			this.btnClearListbox.TabIndex = 9;
			this.btnClearListbox.Text = "Clear Listbox";
			this.btnClearListbox.UseVisualStyleBackColor = true;
			this.btnClearListbox.Click += new System.EventHandler(this.btnClearListbox_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radCcLeads);
			this.groupBox1.Controls.Add(this.radKeyedBasicDataImport);
			this.groupBox1.Controls.Add(this.radUploadMapCfg);
			this.groupBox1.Controls.Add(this.radGetSetupFile);
			this.groupBox1.Controls.Add(this.radGetSetupInfo);
			this.groupBox1.Controls.Add(this.radImport);
			this.groupBox1.Location = new System.Drawing.Point(19, 93);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox1.Size = new System.Drawing.Size(188, 154);
			this.groupBox1.TabIndex = 10;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Operation";
			// 
			// radKeyedBasicDataImport
			// 
			this.radKeyedBasicDataImport.AutoSize = true;
			this.radKeyedBasicDataImport.Checked = true;
			this.radKeyedBasicDataImport.Location = new System.Drawing.Point(16, 104);
			this.radKeyedBasicDataImport.Margin = new System.Windows.Forms.Padding(2);
			this.radKeyedBasicDataImport.Name = "radKeyedBasicDataImport";
			this.radKeyedBasicDataImport.Size = new System.Drawing.Size(142, 17);
			this.radKeyedBasicDataImport.TabIndex = 4;
			this.radKeyedBasicDataImport.TabStop = true;
			this.radKeyedBasicDataImport.Text = "Keyed Basic Data Import";
			this.radKeyedBasicDataImport.UseVisualStyleBackColor = true;
			// 
			// radUploadMapCfg
			// 
			this.radUploadMapCfg.AutoSize = true;
			this.radUploadMapCfg.Location = new System.Drawing.Point(16, 83);
			this.radUploadMapCfg.Margin = new System.Windows.Forms.Padding(2);
			this.radUploadMapCfg.Name = "radUploadMapCfg";
			this.radUploadMapCfg.Size = new System.Drawing.Size(102, 17);
			this.radUploadMapCfg.TabIndex = 3;
			this.radUploadMapCfg.Text = "Upload Map.Cfg";
			this.radUploadMapCfg.UseVisualStyleBackColor = true;
			// 
			// radGetSetupFile
			// 
			this.radGetSetupFile.AutoSize = true;
			this.radGetSetupFile.Location = new System.Drawing.Point(16, 62);
			this.radGetSetupFile.Margin = new System.Windows.Forms.Padding(2);
			this.radGetSetupFile.Name = "radGetSetupFile";
			this.radGetSetupFile.Size = new System.Drawing.Size(92, 17);
			this.radGetSetupFile.TabIndex = 2;
			this.radGetSetupFile.Text = "Get Setup File";
			this.radGetSetupFile.UseVisualStyleBackColor = true;
			// 
			// radGetSetupInfo
			// 
			this.radGetSetupInfo.AutoSize = true;
			this.radGetSetupInfo.Location = new System.Drawing.Point(16, 40);
			this.radGetSetupInfo.Margin = new System.Windows.Forms.Padding(2);
			this.radGetSetupInfo.Name = "radGetSetupInfo";
			this.radGetSetupInfo.Size = new System.Drawing.Size(94, 17);
			this.radGetSetupInfo.TabIndex = 1;
			this.radGetSetupInfo.Text = "Get Setup Info";
			this.radGetSetupInfo.UseVisualStyleBackColor = true;
			// 
			// radImport
			// 
			this.radImport.AutoSize = true;
			this.radImport.Checked = true;
			this.radImport.Location = new System.Drawing.Point(16, 18);
			this.radImport.Margin = new System.Windows.Forms.Padding(2);
			this.radImport.Name = "radImport";
			this.radImport.Size = new System.Drawing.Size(54, 17);
			this.radImport.TabIndex = 0;
			this.radImport.TabStop = true;
			this.radImport.Text = "Import";
			this.radImport.UseVisualStyleBackColor = true;
			// 
			// lblStatus
			// 
			this.lblStatus.AutoSize = true;
			this.lblStatus.Location = new System.Drawing.Point(221, 154);
			this.lblStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(0, 13);
			this.lblStatus.TabIndex = 11;
			// 
			// chkAppendToOutputFile
			// 
			this.chkAppendToOutputFile.AutoSize = true;
			this.chkAppendToOutputFile.Location = new System.Drawing.Point(356, 195);
			this.chkAppendToOutputFile.Margin = new System.Windows.Forms.Padding(2);
			this.chkAppendToOutputFile.Name = "chkAppendToOutputFile";
			this.chkAppendToOutputFile.Size = new System.Drawing.Size(124, 17);
			this.chkAppendToOutputFile.TabIndex = 12;
			this.chkAppendToOutputFile.Text = "Append to output file";
			this.chkAppendToOutputFile.UseVisualStyleBackColor = true;
			// 
			// lvBadRecs
			// 
			this.lvBadRecs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvBadRecs.Location = new System.Drawing.Point(19, 471);
			this.lvBadRecs.Margin = new System.Windows.Forms.Padding(2);
			this.lvBadRecs.Name = "lvBadRecs";
			this.lvBadRecs.Size = new System.Drawing.Size(132, 186);
			this.lvBadRecs.TabIndex = 13;
			this.lvBadRecs.UseCompatibleStateImageBehavior = false;
			this.lvBadRecs.SelectedIndexChanged += new System.EventHandler(this.lvBadRecs_SelectedIndexChanged);
			// 
			// txtDebugRecord
			// 
			this.txtDebugRecord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.txtDebugRecord.Location = new System.Drawing.Point(167, 471);
			this.txtDebugRecord.Margin = new System.Windows.Forms.Padding(2);
			this.txtDebugRecord.Multiline = true;
			this.txtDebugRecord.Name = "txtDebugRecord";
			this.txtDebugRecord.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtDebugRecord.Size = new System.Drawing.Size(352, 186);
			this.txtDebugRecord.TabIndex = 14;
			// 
			// btnEditVisitor
			// 
			this.btnEditVisitor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnEditVisitor.Location = new System.Drawing.Point(462, 65);
			this.btnEditVisitor.Margin = new System.Windows.Forms.Padding(2);
			this.btnEditVisitor.Name = "btnEditVisitor";
			this.btnEditVisitor.Size = new System.Drawing.Size(56, 19);
			this.btnEditVisitor.TabIndex = 16;
			this.btnEditVisitor.Text = "Edit";
			this.btnEditVisitor.UseVisualStyleBackColor = true;
			this.btnEditVisitor.Click += new System.EventHandler(this.btnEditVisitor_Click);
			// 
			// btnEditMap
			// 
			this.btnEditMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnEditMap.Location = new System.Drawing.Point(462, 31);
			this.btnEditMap.Margin = new System.Windows.Forms.Padding(2);
			this.btnEditMap.Name = "btnEditMap";
			this.btnEditMap.Size = new System.Drawing.Size(56, 19);
			this.btnEditMap.TabIndex = 15;
			this.btnEditMap.Text = "Edit";
			this.btnEditMap.UseVisualStyleBackColor = true;
			this.btnEditMap.Click += new System.EventHandler(this.btnEditMap_Click);
			// 
			// radCcLeads
			// 
			this.radCcLeads.AutoSize = true;
			this.radCcLeads.Checked = true;
			this.radCcLeads.Location = new System.Drawing.Point(16, 125);
			this.radCcLeads.Margin = new System.Windows.Forms.Padding(2);
			this.radCcLeads.Name = "radCcLeads";
			this.radCcLeads.Size = new System.Drawing.Size(67, 17);
			this.radCcLeads.TabIndex = 5;
			this.radCcLeads.TabStop = true;
			this.radCcLeads.Text = "CcLeads";
			this.radCcLeads.UseVisualStyleBackColor = true;
			// 
			// TestLLImporter
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(540, 666);
			this.Controls.Add(this.btnEditVisitor);
			this.Controls.Add(this.btnEditMap);
			this.Controls.Add(this.txtDebugRecord);
			this.Controls.Add(this.lvBadRecs);
			this.Controls.Add(this.chkAppendToOutputFile);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnClearListbox);
			this.Controls.Add(this.chkUseWebService);
			this.Controls.Add(this.lbMsgs);
			this.Controls.Add(this.btnBrowseVisitorTxt);
			this.Controls.Add(this.txtVisitorTxt);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.btnBrowseMapCfg);
			this.Controls.Add(this.txtMapCfg);
			this.Controls.Add(this.label1);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "TestLLImporter";
			this.Text = "Test LeadsLightning";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtMapCfg;
		private System.Windows.Forms.Button btnBrowseMapCfg;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.Button btnBrowseVisitorTxt;
		private System.Windows.Forms.TextBox txtVisitorTxt;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListBox lbMsgs;
		private System.Windows.Forms.CheckBox chkUseWebService;
		private System.Windows.Forms.Button btnClearListbox;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radImport;
		private System.Windows.Forms.RadioButton radGetSetupInfo;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.RadioButton radGetSetupFile;
		private System.Windows.Forms.RadioButton radUploadMapCfg;
		private System.Windows.Forms.CheckBox chkAppendToOutputFile;
		private System.Windows.Forms.ListView lvBadRecs;
		private System.Windows.Forms.TextBox txtDebugRecord;
		private System.Windows.Forms.Button btnEditVisitor;
		private System.Windows.Forms.Button btnEditMap;
		private System.Windows.Forms.RadioButton radKeyedBasicDataImport;
		private System.Windows.Forms.RadioButton radCcLeads;
	}
}

