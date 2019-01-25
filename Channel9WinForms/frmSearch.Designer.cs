namespace WinFormsChannel9 {
	partial class frmSearch {
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
			this.txtFilter = new System.Windows.Forms.TextBox();
			this.txtSearch = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.lbAllKeywords = new System.Windows.Forms.ListBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.lbKeywordsToSearch = new System.Windows.Forms.ListBox();
			this.web2 = new System.Windows.Forms.WebBrowser();
			this.label4 = new System.Windows.Forms.Label();
			this.cbDateSortDirection = new System.Windows.Forms.ComboBox();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.grdDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.grdTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.grdDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.grdKeywords = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnGo = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(26, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(29, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Filter";
			// 
			// txtFilter
			// 
			this.txtFilter.Location = new System.Drawing.Point(89, 10);
			this.txtFilter.Name = "txtFilter";
			this.txtFilter.Size = new System.Drawing.Size(177, 20);
			this.txtFilter.TabIndex = 1;
			// 
			// txtSearch
			// 
			this.txtSearch.Location = new System.Drawing.Point(408, 10);
			this.txtSearch.Name = "txtSearch";
			this.txtSearch.Size = new System.Drawing.Size(177, 20);
			this.txtSearch.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(345, 13);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(41, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Search";
			// 
			// lbAllKeywords
			// 
			this.lbAllKeywords.FormattingEnabled = true;
			this.lbAllKeywords.Location = new System.Drawing.Point(29, 50);
			this.lbAllKeywords.Name = "lbAllKeywords";
			this.lbAllKeywords.Size = new System.Drawing.Size(237, 160);
			this.lbAllKeywords.TabIndex = 4;
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(280, 64);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(62, 23);
			this.btnAdd.TabIndex = 5;
			this.btnAdd.Text = "->";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnRemove
			// 
			this.btnRemove.Location = new System.Drawing.Point(280, 93);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(62, 23);
			this.btnRemove.TabIndex = 6;
			this.btnRemove.Text = "<-";
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// lbKeywordsToSearch
			// 
			this.lbKeywordsToSearch.FormattingEnabled = true;
			this.lbKeywordsToSearch.Location = new System.Drawing.Point(348, 50);
			this.lbKeywordsToSearch.Name = "lbKeywordsToSearch";
			this.lbKeywordsToSearch.Size = new System.Drawing.Size(237, 160);
			this.lbKeywordsToSearch.TabIndex = 7;
			// 
			// web2
			// 
			this.web2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.web2.Location = new System.Drawing.Point(619, 50);
			this.web2.MinimumSize = new System.Drawing.Size(20, 20);
			this.web2.Name = "web2";
			this.web2.Size = new System.Drawing.Size(459, 450);
			this.web2.TabIndex = 9;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(277, 141);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(65, 13);
			this.label4.TabIndex = 10;
			this.label4.Text = "Date Sort";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// cbDateSortDirection
			// 
			this.cbDateSortDirection.FormattingEnabled = true;
			this.cbDateSortDirection.Items.AddRange(new object[] {
            "Asc",
            "Desc"});
			this.cbDateSortDirection.Location = new System.Drawing.Point(280, 157);
			this.cbDateSortDirection.Name = "cbDateSortDirection";
			this.cbDateSortDirection.Size = new System.Drawing.Size(62, 21);
			this.cbDateSortDirection.TabIndex = 11;
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.grdDate,
            this.grdTitle,
            this.grdDesc,
            this.grdKeywords});
			this.dataGridView1.Location = new System.Drawing.Point(29, 236);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.RowHeadersVisible = false;
			this.dataGridView1.Size = new System.Drawing.Size(556, 264);
			this.dataGridView1.TabIndex = 12;
			this.dataGridView1.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_RowEnter);
			// 
			// grdDate
			// 
			this.grdDate.HeaderText = "Date";
			this.grdDate.Name = "grdDate";
			this.grdDate.ReadOnly = true;
			this.grdDate.Width = 80;
			// 
			// grdTitle
			// 
			this.grdTitle.HeaderText = "Title";
			this.grdTitle.Name = "grdTitle";
			this.grdTitle.ReadOnly = true;
			// 
			// grdDesc
			// 
			this.grdDesc.HeaderText = "Description";
			this.grdDesc.Name = "grdDesc";
			this.grdDesc.ReadOnly = true;
			this.grdDesc.Width = 200;
			// 
			// grdKeywords
			// 
			this.grdKeywords.HeaderText = "Keywords";
			this.grdKeywords.Name = "grdKeywords";
			this.grdKeywords.ReadOnly = true;
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(280, 187);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(62, 23);
			this.btnGo.TabIndex = 13;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// frmSearch
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1090, 512);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.cbDateSortDirection);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.web2);
			this.Controls.Add(this.lbKeywordsToSearch);
			this.Controls.Add(this.btnRemove);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.lbAllKeywords);
			this.Controls.Add(this.txtSearch);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtFilter);
			this.Controls.Add(this.label1);
			this.Name = "frmSearch";
			this.Text = "Search Channel 9";
			this.Load += new System.EventHandler(this.frmSearch_Load);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtFilter;
		private System.Windows.Forms.TextBox txtSearch;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListBox lbAllKeywords;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.ListBox lbKeywordsToSearch;
		private System.Windows.Forms.WebBrowser web2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cbDateSortDirection;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.DataGridViewTextBoxColumn grdDate;
		private System.Windows.Forms.DataGridViewTextBoxColumn grdTitle;
		private System.Windows.Forms.DataGridViewTextBoxColumn grdDesc;
		private System.Windows.Forms.DataGridViewTextBoxColumn grdKeywords;
		private System.Windows.Forms.Button btnGo;
	}
}