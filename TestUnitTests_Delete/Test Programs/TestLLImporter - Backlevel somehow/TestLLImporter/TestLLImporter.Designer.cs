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
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(22, 38);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(60, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Map.Cfg";
			// 
			// txtMapCfg
			// 
			this.txtMapCfg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtMapCfg.Location = new System.Drawing.Point(109, 38);
			this.txtMapCfg.Name = "txtMapCfg";
			this.txtMapCfg.Size = new System.Drawing.Size(397, 22);
			this.txtMapCfg.TabIndex = 1;
			this.txtMapCfg.Text = "C:\\LRS\\$Importer Data\\Disk 4\\Map.cfg";
			// 
			// btnBrowseMapCfg
			// 
			this.btnBrowseMapCfg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseMapCfg.Location = new System.Drawing.Point(528, 38);
			this.btnBrowseMapCfg.Name = "btnBrowseMapCfg";
			this.btnBrowseMapCfg.Size = new System.Drawing.Size(75, 23);
			this.btnBrowseMapCfg.TabIndex = 2;
			this.btnBrowseMapCfg.Text = "Browse";
			this.btnBrowseMapCfg.UseVisualStyleBackColor = true;
			this.btnBrowseMapCfg.Click += new System.EventHandler(this.btnBrowseMapCfg_Click);
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(298, 137);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 3;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// btnBrowseVisitorTxt
			// 
			this.btnBrowseVisitorTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseVisitorTxt.Location = new System.Drawing.Point(528, 80);
			this.btnBrowseVisitorTxt.Name = "btnBrowseVisitorTxt";
			this.btnBrowseVisitorTxt.Size = new System.Drawing.Size(75, 23);
			this.btnBrowseVisitorTxt.TabIndex = 6;
			this.btnBrowseVisitorTxt.Text = "Browse";
			this.btnBrowseVisitorTxt.UseVisualStyleBackColor = true;
			this.btnBrowseVisitorTxt.Click += new System.EventHandler(this.btnBrowseVisitorTxt_Click);
			// 
			// txtVisitorTxt
			// 
			this.txtVisitorTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtVisitorTxt.Location = new System.Drawing.Point(109, 80);
			this.txtVisitorTxt.Name = "txtVisitorTxt";
			this.txtVisitorTxt.Size = new System.Drawing.Size(397, 22);
			this.txtVisitorTxt.TabIndex = 5;
			this.txtVisitorTxt.Text = "C:\\LRS\\$Importer Data\\Disk 4\\Visitor.txt";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(22, 80);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(65, 17);
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
			this.lbMsgs.ItemHeight = 16;
			this.lbMsgs.Location = new System.Drawing.Point(25, 276);
			this.lbMsgs.Name = "lbMsgs";
			this.lbMsgs.Size = new System.Drawing.Size(666, 148);
			this.lbMsgs.TabIndex = 7;
			// 
			// chkUseWebService
			// 
			this.chkUseWebService.AutoSize = true;
			this.chkUseWebService.Checked = true;
			this.chkUseWebService.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkUseWebService.Location = new System.Drawing.Point(475, 139);
			this.chkUseWebService.Name = "chkUseWebService";
			this.chkUseWebService.Size = new System.Drawing.Size(139, 21);
			this.chkUseWebService.TabIndex = 8;
			this.chkUseWebService.Text = "Use Web Service";
			this.chkUseWebService.UseVisualStyleBackColor = true;
			// 
			// btnClearListbox
			// 
			this.btnClearListbox.Location = new System.Drawing.Point(298, 221);
			this.btnClearListbox.Name = "btnClearListbox";
			this.btnClearListbox.Size = new System.Drawing.Size(146, 23);
			this.btnClearListbox.TabIndex = 9;
			this.btnClearListbox.Text = "Clear Listbox";
			this.btnClearListbox.UseVisualStyleBackColor = true;
			this.btnClearListbox.Click += new System.EventHandler(this.btnClearListbox_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radUploadMapCfg);
			this.groupBox1.Controls.Add(this.radGetSetupFile);
			this.groupBox1.Controls.Add(this.radGetSetupInfo);
			this.groupBox1.Controls.Add(this.radImport);
			this.groupBox1.Location = new System.Drawing.Point(25, 115);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(251, 129);
			this.groupBox1.TabIndex = 10;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Operation";
			// 
			// radUploadMapCfg
			// 
			this.radUploadMapCfg.AutoSize = true;
			this.radUploadMapCfg.Location = new System.Drawing.Point(21, 102);
			this.radUploadMapCfg.Name = "radUploadMapCfg";
			this.radUploadMapCfg.Size = new System.Drawing.Size(130, 21);
			this.radUploadMapCfg.TabIndex = 3;
			this.radUploadMapCfg.Text = "Upload Map.Cfg";
			this.radUploadMapCfg.UseVisualStyleBackColor = true;
			// 
			// radGetSetupFile
			// 
			this.radGetSetupFile.AutoSize = true;
			this.radGetSetupFile.Location = new System.Drawing.Point(21, 76);
			this.radGetSetupFile.Name = "radGetSetupFile";
			this.radGetSetupFile.Size = new System.Drawing.Size(119, 21);
			this.radGetSetupFile.TabIndex = 2;
			this.radGetSetupFile.Text = "Get Setup File";
			this.radGetSetupFile.UseVisualStyleBackColor = true;
			// 
			// radGetSetupInfo
			// 
			this.radGetSetupInfo.AutoSize = true;
			this.radGetSetupInfo.Location = new System.Drawing.Point(21, 49);
			this.radGetSetupInfo.Name = "radGetSetupInfo";
			this.radGetSetupInfo.Size = new System.Drawing.Size(120, 21);
			this.radGetSetupInfo.TabIndex = 1;
			this.radGetSetupInfo.Text = "Get Setup Info";
			this.radGetSetupInfo.UseVisualStyleBackColor = true;
			// 
			// radImport
			// 
			this.radImport.AutoSize = true;
			this.radImport.Checked = true;
			this.radImport.Location = new System.Drawing.Point(21, 22);
			this.radImport.Name = "radImport";
			this.radImport.Size = new System.Drawing.Size(68, 21);
			this.radImport.TabIndex = 0;
			this.radImport.TabStop = true;
			this.radImport.Text = "Import";
			this.radImport.UseVisualStyleBackColor = true;
			// 
			// lblStatus
			// 
			this.lblStatus.AutoSize = true;
			this.lblStatus.Location = new System.Drawing.Point(295, 189);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(0, 17);
			this.lblStatus.TabIndex = 11;
			// 
			// chkAppendToOutputFile
			// 
			this.chkAppendToOutputFile.AutoSize = true;
			this.chkAppendToOutputFile.Location = new System.Drawing.Point(475, 221);
			this.chkAppendToOutputFile.Name = "chkAppendToOutputFile";
			this.chkAppendToOutputFile.Size = new System.Drawing.Size(161, 21);
			this.chkAppendToOutputFile.TabIndex = 12;
			this.chkAppendToOutputFile.Text = "Append to output file";
			this.chkAppendToOutputFile.UseVisualStyleBackColor = true;
			// 
			// lvBadRecs
			// 
			this.lvBadRecs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvBadRecs.Location = new System.Drawing.Point(25, 450);
			this.lvBadRecs.Name = "lvBadRecs";
			this.lvBadRecs.Size = new System.Drawing.Size(175, 228);
			this.lvBadRecs.TabIndex = 13;
			this.lvBadRecs.UseCompatibleStateImageBehavior = false;
			this.lvBadRecs.SelectedIndexChanged += new System.EventHandler(this.lvBadRecs_SelectedIndexChanged);
			// 
			// txtDebugRecord
			// 
			this.txtDebugRecord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.txtDebugRecord.Location = new System.Drawing.Point(223, 450);
			this.txtDebugRecord.Multiline = true;
			this.txtDebugRecord.Name = "txtDebugRecord";
			this.txtDebugRecord.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtDebugRecord.Size = new System.Drawing.Size(468, 228);
			this.txtDebugRecord.TabIndex = 14;
			// 
			// btnEditVisitor
			// 
			this.btnEditVisitor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnEditVisitor.Location = new System.Drawing.Point(616, 80);
			this.btnEditVisitor.Name = "btnEditVisitor";
			this.btnEditVisitor.Size = new System.Drawing.Size(75, 23);
			this.btnEditVisitor.TabIndex = 16;
			this.btnEditVisitor.Text = "Edit";
			this.btnEditVisitor.UseVisualStyleBackColor = true;
			this.btnEditVisitor.Click += new System.EventHandler(this.btnEditVisitor_Click);
			// 
			// btnEditMap
			// 
			this.btnEditMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnEditMap.Location = new System.Drawing.Point(616, 38);
			this.btnEditMap.Name = "btnEditMap";
			this.btnEditMap.Size = new System.Drawing.Size(75, 23);
			this.btnEditMap.TabIndex = 15;
			this.btnEditMap.Text = "Edit";
			this.btnEditMap.UseVisualStyleBackColor = true;
			this.btnEditMap.Click += new System.EventHandler(this.btnEditMap_Click);
			// 
			// TestLLImporter
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(720, 690);
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
	}
}

