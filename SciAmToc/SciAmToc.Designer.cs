namespace SciAmToc {
	partial class SciAmToc {
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
			this.CmbFirstDecade = new System.Windows.Forms.ComboBox();
			this.CmbLastDecade = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.BtnGo = new System.Windows.Forms.Button();
			this.ChkRawLines = new System.Windows.Forms.CheckBox();
			this.CmbFirstYear = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.CmbLastYear = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.CmbFirstMonth = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.CmbLastMonth = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.LblProcessing = new System.Windows.Forms.Label();
			this.ChkSkipDepartments = new System.Windows.Forms.CheckBox();
			this.ChkSkipDescrptions = new System.Windows.Forms.CheckBox();
			this.button1 = new System.Windows.Forms.Button();
			this.ChkFirstIssueOnly = new System.Windows.Forms.CheckBox();
			this.LvArticles = new System.Windows.Forms.ListView();
			this.ArticleName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Page = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Authors = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.BtnNext = new System.Windows.Forms.Button();
			this.BtnMakeLastFirst = new System.Windows.Forms.Button();
			this.BtnShowIssue = new System.Windows.Forms.Button();
			this.TxtDescription = new System.Windows.Forms.TextBox();
			this.BtnExtractToc = new System.Windows.Forms.Button();
			this.BtnTest = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(122, 25);
			this.label1.TabIndex = 0;
			this.label1.Text = "First Decade";
			// 
			// CmbFirstDecade
			// 
			this.CmbFirstDecade.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CmbFirstDecade.FormattingEnabled = true;
			this.CmbFirstDecade.Location = new System.Drawing.Point(166, 17);
			this.CmbFirstDecade.Name = "CmbFirstDecade";
			this.CmbFirstDecade.Size = new System.Drawing.Size(121, 33);
			this.CmbFirstDecade.TabIndex = 1;
			this.CmbFirstDecade.SelectedIndexChanged += new System.EventHandler(this.CmbDecade_SelectedIndexChanged);
			// 
			// CmbLastDecade
			// 
			this.CmbLastDecade.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CmbLastDecade.FormattingEnabled = true;
			this.CmbLastDecade.Location = new System.Drawing.Point(430, 17);
			this.CmbLastDecade.Name = "CmbLastDecade";
			this.CmbLastDecade.Size = new System.Drawing.Size(121, 33);
			this.CmbLastDecade.TabIndex = 3;
			this.CmbLastDecade.SelectedIndexChanged += new System.EventHandler(this.CmbDecade_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(302, 19);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(122, 25);
			this.label2.TabIndex = 2;
			this.label2.Text = "Last Decade";
			// 
			// BtnGo
			// 
			this.BtnGo.Location = new System.Drawing.Point(589, 24);
			this.BtnGo.Name = "BtnGo";
			this.BtnGo.Size = new System.Drawing.Size(136, 27);
			this.BtnGo.TabIndex = 4;
			this.BtnGo.Text = "Go";
			this.BtnGo.UseVisualStyleBackColor = true;
			this.BtnGo.Click += new System.EventHandler(this.BtnGo_Click);
			// 
			// ChkRawLines
			// 
			this.ChkRawLines.AutoSize = true;
			this.ChkRawLines.Location = new System.Drawing.Point(746, 26);
			this.ChkRawLines.Name = "ChkRawLines";
			this.ChkRawLines.Size = new System.Drawing.Size(133, 21);
			this.ChkRawLines.TabIndex = 5;
			this.ChkRawLines.Text = "Show Raw Lines";
			this.ChkRawLines.UseVisualStyleBackColor = true;
			// 
			// CmbFirstYear
			// 
			this.CmbFirstYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CmbFirstYear.FormattingEnabled = true;
			this.CmbFirstYear.Location = new System.Drawing.Point(166, 64);
			this.CmbFirstYear.Name = "CmbFirstYear";
			this.CmbFirstYear.Size = new System.Drawing.Size(121, 33);
			this.CmbFirstYear.TabIndex = 7;
			this.CmbFirstYear.SelectedIndexChanged += new System.EventHandler(this.CmbFirstYear_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(16, 66);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(95, 25);
			this.label3.TabIndex = 6;
			this.label3.Text = "First Year";
			// 
			// CmbLastYear
			// 
			this.CmbLastYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CmbLastYear.FormattingEnabled = true;
			this.CmbLastYear.Location = new System.Drawing.Point(430, 64);
			this.CmbLastYear.Name = "CmbLastYear";
			this.CmbLastYear.Size = new System.Drawing.Size(121, 33);
			this.CmbLastYear.TabIndex = 9;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(302, 66);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(95, 25);
			this.label4.TabIndex = 8;
			this.label4.Text = "Last Year";
			// 
			// CmbFirstMonth
			// 
			this.CmbFirstMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CmbFirstMonth.FormattingEnabled = true;
			this.CmbFirstMonth.Location = new System.Drawing.Point(166, 111);
			this.CmbFirstMonth.Name = "CmbFirstMonth";
			this.CmbFirstMonth.Size = new System.Drawing.Size(121, 33);
			this.CmbFirstMonth.TabIndex = 11;
			this.CmbFirstMonth.SelectedIndexChanged += new System.EventHandler(this.CmbMonth_SelectedIndexChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(16, 113);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(109, 25);
			this.label5.TabIndex = 10;
			this.label5.Text = "First Month";
			// 
			// CmbLastMonth
			// 
			this.CmbLastMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CmbLastMonth.FormattingEnabled = true;
			this.CmbLastMonth.Location = new System.Drawing.Point(430, 111);
			this.CmbLastMonth.Name = "CmbLastMonth";
			this.CmbLastMonth.Size = new System.Drawing.Size(121, 33);
			this.CmbLastMonth.TabIndex = 13;
			this.CmbLastMonth.SelectedIndexChanged += new System.EventHandler(this.CmbMonth_SelectedIndexChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(302, 111);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(109, 25);
			this.label6.TabIndex = 12;
			this.label6.Text = "Last Month";
			// 
			// label8
			// 
			this.label8.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(19, 652);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(115, 25);
			this.label8.TabIndex = 22;
			this.label8.Text = "Processing:";
			// 
			// LblProcessing
			// 
			this.LblProcessing.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.LblProcessing.AutoSize = true;
			this.LblProcessing.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.LblProcessing.Location = new System.Drawing.Point(156, 652);
			this.LblProcessing.Name = "LblProcessing";
			this.LblProcessing.Size = new System.Drawing.Size(0, 25);
			this.LblProcessing.TabIndex = 23;
			// 
			// ChkSkipDepartments
			// 
			this.ChkSkipDepartments.AutoSize = true;
			this.ChkSkipDepartments.Checked = true;
			this.ChkSkipDepartments.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ChkSkipDepartments.Location = new System.Drawing.Point(746, 110);
			this.ChkSkipDepartments.Name = "ChkSkipDepartments";
			this.ChkSkipDepartments.Size = new System.Drawing.Size(142, 21);
			this.ChkSkipDepartments.TabIndex = 24;
			this.ChkSkipDepartments.Text = "Skip Departments";
			this.ChkSkipDepartments.UseVisualStyleBackColor = true;
			// 
			// ChkSkipDescrptions
			// 
			this.ChkSkipDescrptions.AutoSize = true;
			this.ChkSkipDescrptions.Location = new System.Drawing.Point(746, 68);
			this.ChkSkipDescrptions.Name = "ChkSkipDescrptions";
			this.ChkSkipDescrptions.Size = new System.Drawing.Size(136, 21);
			this.ChkSkipDescrptions.TabIndex = 25;
			this.ChkSkipDescrptions.Text = "Skip Descrptions";
			this.ChkSkipDescrptions.UseVisualStyleBackColor = true;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(589, 66);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(136, 27);
			this.button1.TabIndex = 26;
			this.button1.Text = "Reload Guidance";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.BtnReloadGuidance_Click);
			// 
			// ChkFirstIssueOnly
			// 
			this.ChkFirstIssueOnly.AutoSize = true;
			this.ChkFirstIssueOnly.Checked = true;
			this.ChkFirstIssueOnly.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ChkFirstIssueOnly.Location = new System.Drawing.Point(746, 152);
			this.ChkFirstIssueOnly.Name = "ChkFirstIssueOnly";
			this.ChkFirstIssueOnly.Size = new System.Drawing.Size(124, 21);
			this.ChkFirstIssueOnly.TabIndex = 28;
			this.ChkFirstIssueOnly.Text = "First Issue only";
			this.ChkFirstIssueOnly.UseVisualStyleBackColor = true;
			// 
			// LvArticles
			// 
			this.LvArticles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LvArticles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ArticleName,
            this.Page,
            this.Authors});
			this.LvArticles.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.LvArticles.FullRowSelect = true;
			this.LvArticles.HideSelection = false;
			this.LvArticles.Location = new System.Drawing.Point(21, 235);
			this.LvArticles.MultiSelect = false;
			this.LvArticles.Name = "LvArticles";
			this.LvArticles.ShowItemToolTips = true;
			this.LvArticles.Size = new System.Drawing.Size(872, 276);
			this.LvArticles.TabIndex = 30;
			this.LvArticles.UseCompatibleStateImageBehavior = false;
			this.LvArticles.View = System.Windows.Forms.View.Details;
			this.LvArticles.SelectedIndexChanged += new System.EventHandler(this.LvArticles_SelectedIndexChanged);
			this.LvArticles.Click += new System.EventHandler(this.LvArticles_Click);
			this.LvArticles.DoubleClick += new System.EventHandler(this.LvArticles_DoubleClick);
			this.LvArticles.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LvArticles_KeyPress);
			// 
			// ArticleName
			// 
			this.ArticleName.Text = "Article Name";
			this.ArticleName.Width = 400;
			// 
			// Page
			// 
			this.Page.Text = "Page #";
			this.Page.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.Page.Width = 68;
			// 
			// Authors
			// 
			this.Authors.Text = "Author(s)";
			this.Authors.Width = 400;
			// 
			// BtnNext
			// 
			this.BtnNext.Location = new System.Drawing.Point(589, 150);
			this.BtnNext.Name = "BtnNext";
			this.BtnNext.Size = new System.Drawing.Size(136, 27);
			this.BtnNext.TabIndex = 31;
			this.BtnNext.Text = "Next Issue";
			this.BtnNext.UseVisualStyleBackColor = true;
			this.BtnNext.Click += new System.EventHandler(this.BtnNext_Click);
			// 
			// BtnMakeLastFirst
			// 
			this.BtnMakeLastFirst.Location = new System.Drawing.Point(589, 108);
			this.BtnMakeLastFirst.Name = "BtnMakeLastFirst";
			this.BtnMakeLastFirst.Size = new System.Drawing.Size(136, 27);
			this.BtnMakeLastFirst.TabIndex = 32;
			this.BtnMakeLastFirst.Text = "Make Last = First";
			this.BtnMakeLastFirst.UseVisualStyleBackColor = true;
			this.BtnMakeLastFirst.Click += new System.EventHandler(this.BtnMakeLastFirst_Click);
			// 
			// BtnShowIssue
			// 
			this.BtnShowIssue.Location = new System.Drawing.Point(589, 192);
			this.BtnShowIssue.Name = "BtnShowIssue";
			this.BtnShowIssue.Size = new System.Drawing.Size(136, 27);
			this.BtnShowIssue.TabIndex = 33;
			this.BtnShowIssue.Text = "Show Issue";
			this.BtnShowIssue.UseVisualStyleBackColor = true;
			this.BtnShowIssue.Click += new System.EventHandler(this.BtnShowToc_Click);
			// 
			// TxtDescription
			// 
			this.TxtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TxtDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TxtDescription.Location = new System.Drawing.Point(21, 517);
			this.TxtDescription.Multiline = true;
			this.TxtDescription.Name = "TxtDescription";
			this.TxtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.TxtDescription.Size = new System.Drawing.Size(872, 132);
			this.TxtDescription.TabIndex = 34;
			this.TxtDescription.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtDescription_KeyPress);
			// 
			// BtnExtractToc
			// 
			this.BtnExtractToc.Location = new System.Drawing.Point(743, 192);
			this.BtnExtractToc.Name = "BtnExtractToc";
			this.BtnExtractToc.Size = new System.Drawing.Size(136, 27);
			this.BtnExtractToc.TabIndex = 35;
			this.BtnExtractToc.Text = "Extract TOC";
			this.BtnExtractToc.UseVisualStyleBackColor = true;
			this.BtnExtractToc.Click += new System.EventHandler(this.BtnExtractToc_Click);
			// 
			// BtnTest
			// 
			this.BtnTest.Location = new System.Drawing.Point(356, 164);
			this.BtnTest.Name = "BtnTest";
			this.BtnTest.Size = new System.Drawing.Size(136, 27);
			this.BtnTest.TabIndex = 36;
			this.BtnTest.Text = "Test";
			this.BtnTest.UseVisualStyleBackColor = true;
			this.BtnTest.Click += new System.EventHandler(this.BtnTest_Click);
			// 
			// SciAmToc
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.LightSkyBlue;
			this.ClientSize = new System.Drawing.Size(905, 695);
			this.Controls.Add(this.BtnTest);
			this.Controls.Add(this.BtnExtractToc);
			this.Controls.Add(this.TxtDescription);
			this.Controls.Add(this.BtnShowIssue);
			this.Controls.Add(this.BtnMakeLastFirst);
			this.Controls.Add(this.BtnNext);
			this.Controls.Add(this.LvArticles);
			this.Controls.Add(this.ChkFirstIssueOnly);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.ChkSkipDescrptions);
			this.Controls.Add(this.ChkSkipDepartments);
			this.Controls.Add(this.LblProcessing);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.CmbLastMonth);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.CmbFirstMonth);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.CmbLastYear);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.CmbFirstYear);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.ChkRawLines);
			this.Controls.Add(this.BtnGo);
			this.Controls.Add(this.CmbLastDecade);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.CmbFirstDecade);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "SciAmToc";
			this.Text = "Scientific American TOC";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox CmbFirstDecade;
		private System.Windows.Forms.ComboBox CmbLastDecade;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button BtnGo;
		private System.Windows.Forms.ComboBox CmbFirstYear;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox CmbLastYear;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox CmbFirstMonth;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox CmbLastMonth;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label LblProcessing;
		internal System.Windows.Forms.CheckBox ChkSkipDescrptions;
		internal System.Windows.Forms.CheckBox ChkSkipDepartments;
		private System.Windows.Forms.Button button1;
		internal System.Windows.Forms.CheckBox ChkFirstIssueOnly;
		private System.Windows.Forms.ListView LvArticles;
		private System.Windows.Forms.ColumnHeader ArticleName;
		private System.Windows.Forms.ColumnHeader Page;
		private System.Windows.Forms.ColumnHeader Authors;
		private System.Windows.Forms.Button BtnNext;
		private System.Windows.Forms.Button BtnMakeLastFirst;
		private System.Windows.Forms.Button BtnShowIssue;
		public System.Windows.Forms.CheckBox ChkRawLines;
		private System.Windows.Forms.TextBox TxtDescription;
		private System.Windows.Forms.Button BtnExtractToc;
		private System.Windows.Forms.Button BtnTest;
	}
}

