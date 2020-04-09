namespace GetSciAm {
	partial class GetSciAm {
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
            this.label4 = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.btnGetEntireIssue = new System.Windows.Forms.Button();
            this.lbArticles = new System.Windows.Forms.ListBox();
            this.btnGetNextArticle = new System.Windows.Forms.Button();
            this.btnCleanSciAmTempDirectory = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnRenameFiles = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Year";
            // 
            // cmbYear
            // 
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.Enabled = false;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(112, 12);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(121, 24);
            this.cmbYear.TabIndex = 7;
            this.cmbYear.SelectedIndexChanged += new System.EventHandler(this.cmbYear_SelectedIndexChanged);
            this.cmbYear.SelectedValueChanged += new System.EventHandler(this.cmbYear_SelectedValueChanged);
            // 
            // cmbMonth
            // 
            this.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMonth.Enabled = false;
            this.cmbMonth.FormattingEnabled = true;
            this.cmbMonth.Items.AddRange(new object[] {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"});
            this.cmbMonth.Location = new System.Drawing.Point(355, 12);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(121, 24);
            this.cmbMonth.TabIndex = 9;
            this.cmbMonth.SelectedValueChanged += new System.EventHandler(this.cmbMonth_SelectedValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(265, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "Month";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(25, 57);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 11;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(24, 315);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(821, 367);
            this.webBrowser1.TabIndex = 12;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            this.webBrowser1.FileDownload += new System.EventHandler(this.webBrowser1_FileDownload);
            this.webBrowser1.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser1_Navigating);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 712);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(863, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // btnGetEntireIssue
            // 
            this.btnGetEntireIssue.Location = new System.Drawing.Point(154, 57);
            this.btnGetEntireIssue.Name = "btnGetEntireIssue";
            this.btnGetEntireIssue.Size = new System.Drawing.Size(147, 23);
            this.btnGetEntireIssue.TabIndex = 14;
            this.btnGetEntireIssue.Text = "Get Entire Issue";
            this.btnGetEntireIssue.UseVisualStyleBackColor = true;
            this.btnGetEntireIssue.Click += new System.EventHandler(this.btnGetEntireIssue_Click);
            // 
            // lbArticles
            // 
            this.lbArticles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbArticles.FormattingEnabled = true;
            this.lbArticles.HorizontalScrollbar = true;
            this.lbArticles.ItemHeight = 16;
            this.lbArticles.Location = new System.Drawing.Point(25, 106);
            this.lbArticles.Name = "lbArticles";
            this.lbArticles.Size = new System.Drawing.Size(802, 180);
            this.lbArticles.TabIndex = 16;
            this.lbArticles.DoubleClick += new System.EventHandler(this.lbArticles_DoubleClick);
            // 
            // btnGetNextArticle
            // 
            this.btnGetNextArticle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetNextArticle.Location = new System.Drawing.Point(765, 15);
            this.btnGetNextArticle.Name = "btnGetNextArticle";
            this.btnGetNextArticle.Size = new System.Drawing.Size(62, 65);
            this.btnGetNextArticle.TabIndex = 17;
            this.btnGetNextArticle.Text = "Get All Articles";
            this.btnGetNextArticle.UseVisualStyleBackColor = true;
            this.btnGetNextArticle.Visible = false;
            this.btnGetNextArticle.Click += new System.EventHandler(this.btnGetAllArticles_Click);
            // 
            // btnCleanSciAmTempDirectory
            // 
            this.btnCleanSciAmTempDirectory.Location = new System.Drawing.Point(355, 57);
            this.btnCleanSciAmTempDirectory.Name = "btnCleanSciAmTempDirectory";
            this.btnCleanSciAmTempDirectory.Size = new System.Drawing.Size(236, 23);
            this.btnCleanSciAmTempDirectory.TabIndex = 18;
            this.btnCleanSciAmTempDirectory.Text = "Clean SciAm Temp Directory";
            this.btnCleanSciAmTempDirectory.UseVisualStyleBackColor = true;
            this.btnCleanSciAmTempDirectory.Click += new System.EventHandler(this.btnCleanSciAmTempDirectory_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(516, 9);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 19;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnRenameFiles
            // 
            this.btnRenameFiles.Location = new System.Drawing.Point(624, 57);
            this.btnRenameFiles.Name = "btnRenameFiles";
            this.btnRenameFiles.Size = new System.Drawing.Size(112, 23);
            this.btnRenameFiles.TabIndex = 20;
            this.btnRenameFiles.Text = "Rename Files";
            this.btnRenameFiles.UseVisualStyleBackColor = true;
            this.btnRenameFiles.Click += new System.EventHandler(this.btnRenameFiles_Click);
            // 
            // GetSciAm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 734);
            this.Controls.Add(this.btnRenameFiles);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnCleanSciAmTempDirectory);
            this.Controls.Add(this.btnGetNextArticle);
            this.Controls.Add(this.lbArticles);
            this.Controls.Add(this.btnGetEntireIssue);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.cmbMonth);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbYear);
            this.Controls.Add(this.label4);
            this.Name = "GetSciAm";
            this.Text = "Get Scientific American";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GetSciAm_FormClosed);
            this.Load += new System.EventHandler(this.GetSciAm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cmbYear;
		private System.Windows.Forms.ComboBox cmbMonth;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button btnLogin;
		private System.Windows.Forms.WebBrowser webBrowser1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.Button btnGetEntireIssue;
		private System.Windows.Forms.ListBox lbArticles;
		private System.Windows.Forms.Button btnGetNextArticle;
		private System.Windows.Forms.Button btnCleanSciAmTempDirectory;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnRenameFiles;
	}
}

