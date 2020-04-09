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
			this.label1 = new System.Windows.Forms.Label();
			this.txtURL = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtUserID = new System.Windows.Forms.TextBox();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cmbYear = new System.Windows.Forms.ComboBox();
			this.cmbMonth = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radEverything = new System.Windows.Forms.RadioButton();
			this.radAllArticles = new System.Windows.Forms.RadioButton();
			this.radEntireIssue = new System.Windows.Forms.RadioButton();
			this.btnGo = new System.Windows.Forms.Button();
			this.webBrowser1 = new System.Windows.Forms.WebBrowser();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(25, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(36, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "URL";
			// 
			// txtURL
			// 
			this.txtURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtURL.Location = new System.Drawing.Point(115, 22);
			this.txtURL.Name = "txtURL";
			this.txtURL.Size = new System.Drawing.Size(730, 22);
			this.txtURL.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(25, 65);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(51, 17);
			this.label2.TabIndex = 2;
			this.label2.Text = "UserID";
			// 
			// txtUserID
			// 
			this.txtUserID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtUserID.Location = new System.Drawing.Point(115, 65);
			this.txtUserID.Name = "txtUserID";
			this.txtUserID.Size = new System.Drawing.Size(84, 22);
			this.txtUserID.TabIndex = 3;
			// 
			// txtPassword
			// 
			this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtPassword.Location = new System.Drawing.Point(358, 65);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(166, 22);
			this.txtPassword.TabIndex = 5;
			this.txtPassword.UseSystemPasswordChar = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(268, 65);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(69, 17);
			this.label3.TabIndex = 4;
			this.label3.Text = "Password";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(25, 114);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(38, 17);
			this.label4.TabIndex = 6;
			this.label4.Text = "Year";
			// 
			// cmbYear
			// 
			this.cmbYear.FormattingEnabled = true;
			this.cmbYear.Location = new System.Drawing.Point(115, 114);
			this.cmbYear.Name = "cmbYear";
			this.cmbYear.Size = new System.Drawing.Size(121, 24);
			this.cmbYear.TabIndex = 7;
			// 
			// cmbMonth
			// 
			this.cmbMonth.FormattingEnabled = true;
			this.cmbMonth.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12"});
			this.cmbMonth.Location = new System.Drawing.Point(358, 111);
			this.cmbMonth.Name = "cmbMonth";
			this.cmbMonth.Size = new System.Drawing.Size(121, 24);
			this.cmbMonth.TabIndex = 9;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(268, 114);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(47, 17);
			this.label5.TabIndex = 8;
			this.label5.Text = "Month";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radEverything);
			this.groupBox1.Controls.Add(this.radAllArticles);
			this.groupBox1.Controls.Add(this.radEntireIssue);
			this.groupBox1.Location = new System.Drawing.Point(631, 65);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(223, 112);
			this.groupBox1.TabIndex = 10;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Download";
			// 
			// radEverything
			// 
			this.radEverything.AutoSize = true;
			this.radEverything.Location = new System.Drawing.Point(33, 75);
			this.radEverything.Name = "radEverything";
			this.radEverything.Size = new System.Drawing.Size(165, 21);
			this.radEverything.TabIndex = 2;
			this.radEverything.Text = "Entire issue + Articles";
			this.radEverything.UseVisualStyleBackColor = true;
			// 
			// radAllArticles
			// 
			this.radAllArticles.AutoSize = true;
			this.radAllArticles.Location = new System.Drawing.Point(33, 48);
			this.radAllArticles.Name = "radAllArticles";
			this.radAllArticles.Size = new System.Drawing.Size(94, 21);
			this.radAllArticles.TabIndex = 1;
			this.radAllArticles.Text = "All Articles";
			this.radAllArticles.UseVisualStyleBackColor = true;
			// 
			// radEntireIssue
			// 
			this.radEntireIssue.AutoSize = true;
			this.radEntireIssue.Checked = true;
			this.radEntireIssue.Location = new System.Drawing.Point(33, 21);
			this.radEntireIssue.Name = "radEntireIssue";
			this.radEntireIssue.Size = new System.Drawing.Size(103, 21);
			this.radEntireIssue.TabIndex = 0;
			this.radEntireIssue.TabStop = true;
			this.radEntireIssue.Text = "Entire issue";
			this.radEntireIssue.UseVisualStyleBackColor = true;
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(28, 164);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 11;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// webBrowser1
			// 
			this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.webBrowser1.Location = new System.Drawing.Point(24, 207);
			this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
			this.webBrowser1.Name = "webBrowser1";
			this.webBrowser1.Size = new System.Drawing.Size(821, 417);
			this.webBrowser1.TabIndex = 12;
			this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Location = new System.Drawing.Point(0, 654);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(863, 22);
			this.statusStrip1.TabIndex = 13;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// GetSciAm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(863, 676);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.webBrowser1);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.cmbMonth);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.cmbYear);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtUserID);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtURL);
			this.Controls.Add(this.label1);
			this.Name = "GetSciAm";
			this.Text = "Get Scientific American";
			this.Load += new System.EventHandler(this.GetSciAm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtURL;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtUserID;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cmbYear;
		private System.Windows.Forms.ComboBox cmbMonth;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radAllArticles;
		private System.Windows.Forms.RadioButton radEntireIssue;
		private System.Windows.Forms.RadioButton radEverything;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.WebBrowser webBrowser1;
		private System.Windows.Forms.StatusStrip statusStrip1;
	}
}

