namespace SciAm8 {
	partial class GetSciAm8 {
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
			this.cmbYear = new System.Windows.Forms.ComboBox();
			this.cmbMonth = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnLogin = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.btnCleanSciAmTempDirectory = new System.Windows.Forms.Button();
			this.btnTest = new System.Windows.Forms.Button();
			this.btnRenameFiles = new System.Windows.Forms.Button();
			this.btnGetNextArticle = new System.Windows.Forms.Button();
			this.lbArticles = new System.Windows.Forms.ListBox();
			this.cEXWB1 = new csExWB.cEXWB();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Year";
			// 
			// cmbYear
			// 
			this.cmbYear.FormattingEnabled = true;
			this.cmbYear.Location = new System.Drawing.Point(88, 15);
			this.cmbYear.Name = "cmbYear";
			this.cmbYear.Size = new System.Drawing.Size(121, 24);
			this.cmbYear.TabIndex = 1;
			// 
			// cmbMonth
			// 
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
			this.cmbMonth.Location = new System.Drawing.Point(337, 15);
			this.cmbMonth.Name = "cmbMonth";
			this.cmbMonth.Size = new System.Drawing.Size(121, 24);
			this.cmbMonth.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(256, 18);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(47, 17);
			this.label2.TabIndex = 2;
			this.label2.Text = "Month";
			// 
			// btnLogin
			// 
			this.btnLogin.Location = new System.Drawing.Point(12, 67);
			this.btnLogin.Name = "btnLogin";
			this.btnLogin.Size = new System.Drawing.Size(75, 23);
			this.btnLogin.TabIndex = 4;
			this.btnLogin.Text = "Login";
			this.btnLogin.UseVisualStyleBackColor = true;
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(134, 67);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(137, 23);
			this.button1.TabIndex = 5;
			this.button1.Text = "Get Entire Issue";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// btnCleanSciAmTempDirectory
			// 
			this.btnCleanSciAmTempDirectory.Location = new System.Drawing.Point(337, 67);
			this.btnCleanSciAmTempDirectory.Name = "btnCleanSciAmTempDirectory";
			this.btnCleanSciAmTempDirectory.Size = new System.Drawing.Size(236, 23);
			this.btnCleanSciAmTempDirectory.TabIndex = 19;
			this.btnCleanSciAmTempDirectory.Text = "Clean SciAm Temp Directory";
			this.btnCleanSciAmTempDirectory.UseVisualStyleBackColor = true;
			// 
			// btnTest
			// 
			this.btnTest.Location = new System.Drawing.Point(498, 15);
			this.btnTest.Name = "btnTest";
			this.btnTest.Size = new System.Drawing.Size(75, 23);
			this.btnTest.TabIndex = 20;
			this.btnTest.Text = "Test";
			this.btnTest.UseVisualStyleBackColor = true;
			// 
			// btnRenameFiles
			// 
			this.btnRenameFiles.Location = new System.Drawing.Point(621, 67);
			this.btnRenameFiles.Name = "btnRenameFiles";
			this.btnRenameFiles.Size = new System.Drawing.Size(112, 23);
			this.btnRenameFiles.TabIndex = 21;
			this.btnRenameFiles.Text = "Rename Files";
			this.btnRenameFiles.UseVisualStyleBackColor = true;
			// 
			// btnGetNextArticle
			// 
			this.btnGetNextArticle.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.btnGetNextArticle.Location = new System.Drawing.Point(621, 15);
			this.btnGetNextArticle.Name = "btnGetNextArticle";
			this.btnGetNextArticle.Size = new System.Drawing.Size(112, 44);
			this.btnGetNextArticle.TabIndex = 22;
			this.btnGetNextArticle.Text = "Get All Articles";
			this.btnGetNextArticle.UseVisualStyleBackColor = true;
			this.btnGetNextArticle.Visible = false;
			// 
			// lbArticles
			// 
			this.lbArticles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lbArticles.FormattingEnabled = true;
			this.lbArticles.HorizontalScrollbar = true;
			this.lbArticles.ItemHeight = 16;
			this.lbArticles.Location = new System.Drawing.Point(12, 105);
			this.lbArticles.Name = "lbArticles";
			this.lbArticles.Size = new System.Drawing.Size(872, 180);
			this.lbArticles.TabIndex = 23;
			// 
			// cEXWB1
			// 
			this.cEXWB1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cEXWB1.Border3DEnabled = false;
			this.cEXWB1.DocumentSource = "";
			this.cEXWB1.DocumentTitle = "";
			this.cEXWB1.DownloadActiveX = true;
			this.cEXWB1.DownloadFrames = true;
			this.cEXWB1.DownloadImages = true;
			this.cEXWB1.DownloadJava = true;
			this.cEXWB1.DownloadScripts = true;
			this.cEXWB1.DownloadSounds = true;
			this.cEXWB1.DownloadVideo = true;
			this.cEXWB1.FileDownloadDirectory = "C:\\Users\\LRS9450\\Documents\\";
			this.cEXWB1.Location = new System.Drawing.Point(12, 317);
			this.cEXWB1.LocationUrl = "about:blank";
			this.cEXWB1.Name = "cEXWB1";
			this.cEXWB1.ObjectForScripting = null;
			this.cEXWB1.OffLine = false;
			this.cEXWB1.RegisterAsBrowser = false;
			this.cEXWB1.RegisterAsDropTarget = false;
			this.cEXWB1.RegisterForInternalDragDrop = true;
			this.cEXWB1.ScrollBarsEnabled = true;
			this.cEXWB1.SendSourceOnDocumentCompleteWBEx = false;
			this.cEXWB1.Silent = false;
			this.cEXWB1.Size = new System.Drawing.Size(872, 389);
			this.cEXWB1.TabIndex = 24;
			this.cEXWB1.Text = "cEXWB1";
			this.cEXWB1.TextSize = IfacesEnumsStructsClasses.TextSizeWB.Medium;
			this.cEXWB1.UseInternalDownloadManager = true;
			this.cEXWB1.WBDOCDOWNLOADCTLFLAG = 112;
			this.cEXWB1.WBDOCHOSTUIDBLCLK = IfacesEnumsStructsClasses.DOCHOSTUIDBLCLK.DEFAULT;
			this.cEXWB1.WBDOCHOSTUIFLAG = 262276;
			this.cEXWB1.FileDownloadExStart += new csExWB.FileDownloadExEventHandler(this.cEXWB1_FileDownloadExStart);
			// 
			// GetSciAm8
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(896, 741);
			this.Controls.Add(this.cEXWB1);
			this.Controls.Add(this.lbArticles);
			this.Controls.Add(this.btnGetNextArticle);
			this.Controls.Add(this.btnRenameFiles);
			this.Controls.Add(this.btnTest);
			this.Controls.Add(this.btnCleanSciAmTempDirectory);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.btnLogin);
			this.Controls.Add(this.cmbMonth);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cmbYear);
			this.Controls.Add(this.label1);
			this.Name = "GetSciAm8";
			this.Text = "Get Scientific American";
			this.Load += new System.EventHandler(this.GetSciAm8_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbYear;
		private System.Windows.Forms.ComboBox cmbMonth;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnLogin;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button btnCleanSciAmTempDirectory;
		private System.Windows.Forms.Button btnTest;
		private System.Windows.Forms.Button btnRenameFiles;
		private System.Windows.Forms.Button btnGetNextArticle;
		private System.Windows.Forms.ListBox lbArticles;
		private csExWB.cEXWB cEXWB1;
	}
}

