namespace LLTerminalMaintenance {
	partial class LLTerminalMaintenance {
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
			this.btnBrowse = new System.Windows.Forms.Button();
			this.txtTermFile = new System.Windows.Forms.TextBox();
			this.btnGo = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbOwners = new System.Windows.Forms.ComboBox();
			this.cmbHosts = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.chkIgnoreFirstLine = new System.Windows.Forms.CheckBox();
			this.lbMsgs = new System.Windows.Forms.ListBox();
			this.chkIsOwnerBartizan = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 105);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(65, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Filename";
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Location = new System.Drawing.Point(668, 105);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(75, 23);
			this.btnBrowse.TabIndex = 1;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// txtTermFile
			// 
			this.txtTermFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtTermFile.Location = new System.Drawing.Point(95, 105);
			this.txtTermFile.Name = "txtTermFile";
			this.txtTermFile.Size = new System.Drawing.Size(552, 22);
			this.txtTermFile.TabIndex = 2;
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(12, 205);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 3;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 154);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(49, 17);
			this.label2.TabIndex = 4;
			this.label2.Text = "Owner";
			// 
			// cmbOwners
			// 
			this.cmbOwners.FormattingEnabled = true;
			this.cmbOwners.Location = new System.Drawing.Point(95, 154);
			this.cmbOwners.Name = "cmbOwners";
			this.cmbOwners.Size = new System.Drawing.Size(228, 24);
			this.cmbOwners.TabIndex = 5;
			this.cmbOwners.SelectedIndexChanged += new System.EventHandler(this.cmbOwners_SelectedIndexChanged);
			// 
			// cmbHosts
			// 
			this.cmbHosts.FormattingEnabled = true;
			this.cmbHosts.Location = new System.Drawing.Point(95, 27);
			this.cmbHosts.Name = "cmbHosts";
			this.cmbHosts.Size = new System.Drawing.Size(228, 24);
			this.cmbHosts.TabIndex = 7;
			this.cmbHosts.SelectedIndexChanged += new System.EventHandler(this.cmbHosts_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(9, 27);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(44, 17);
			this.label3.TabIndex = 6;
			this.label3.Text = "Hosts";
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// chkIgnoreFirstLine
			// 
			this.chkIgnoreFirstLine.AutoSize = true;
			this.chkIgnoreFirstLine.Checked = true;
			this.chkIgnoreFirstLine.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkIgnoreFirstLine.Location = new System.Drawing.Point(546, 157);
			this.chkIgnoreFirstLine.Name = "chkIgnoreFirstLine";
			this.chkIgnoreFirstLine.Size = new System.Drawing.Size(132, 21);
			this.chkIgnoreFirstLine.TabIndex = 8;
			this.chkIgnoreFirstLine.Text = "Ignore First Line";
			this.chkIgnoreFirstLine.UseVisualStyleBackColor = true;
			// 
			// lbMsgs
			// 
			this.lbMsgs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lbMsgs.FormattingEnabled = true;
			this.lbMsgs.ItemHeight = 16;
			this.lbMsgs.Location = new System.Drawing.Point(95, 205);
			this.lbMsgs.Name = "lbMsgs";
			this.lbMsgs.Size = new System.Drawing.Size(648, 228);
			this.lbMsgs.TabIndex = 9;
			// 
			// chkIsOwnerBartizan
			// 
			this.chkIsOwnerBartizan.AutoSize = true;
			this.chkIsOwnerBartizan.Checked = true;
			this.chkIsOwnerBartizan.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkIsOwnerBartizan.Location = new System.Drawing.Point(371, 156);
			this.chkIsOwnerBartizan.Name = "chkIsOwnerBartizan";
			this.chkIsOwnerBartizan.Size = new System.Drawing.Size(141, 21);
			this.chkIsOwnerBartizan.TabIndex = 10;
			this.chkIsOwnerBartizan.Text = "Is Owner Bartizan";
			this.chkIsOwnerBartizan.UseVisualStyleBackColor = true;
			// 
			// LLTerminalMaintenance
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(756, 443);
			this.Controls.Add(this.chkIsOwnerBartizan);
			this.Controls.Add(this.lbMsgs);
			this.Controls.Add(this.chkIgnoreFirstLine);
			this.Controls.Add(this.cmbHosts);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cmbOwners);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.txtTermFile);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.label1);
			this.Name = "LLTerminalMaintenance";
			this.Text = "LeadsLightning Terminal Maintenance";
			this.Load += new System.EventHandler(this.LLTerminalMaintenance_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.TextBox txtTermFile;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cmbOwners;
		private System.Windows.Forms.ComboBox cmbHosts;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.CheckBox chkIgnoreFirstLine;
		private System.Windows.Forms.ListBox lbMsgs;
		private System.Windows.Forms.CheckBox chkIsOwnerBartizan;
	}
}

