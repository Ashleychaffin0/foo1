namespace LRSLibraryBooks {
	partial class LRSLibraryBooks {
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
			this.components = new System.ComponentModel.Container();
			Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
			this.SelectAllItemsBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabAdd = new System.Windows.Forms.TabPage();
			this.chkNotDue = new System.Windows.Forms.CheckBox();
			this.btnEdit = new System.Windows.Forms.Button();
			this.btnNewItem = new System.Windows.Forms.Button();
			this.calDateDue = new System.Windows.Forms.MonthCalendar();
			this.label7 = new System.Windows.Forms.Label();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnNewOwner = new System.Windows.Forms.Button();
			this.cmbOwner = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.btnNewMediaType = new System.Windows.Forms.Button();
			this.cmbMediaType = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.btnNewGenre = new System.Windows.Forms.Button();
			this.cmbGenres = new System.Windows.Forms.ComboBox();
			this.txtBookTitle = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtAuthorLastName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtAuthorFirstName = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
			this.label8 = new System.Windows.Forms.Label();
			this.btnExport = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.SelectAllItemsBindingSource)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tabAdd.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabAdd);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(12, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(719, 450);
			this.tabControl1.TabIndex = 1;
			// 
			// tabAdd
			// 
			this.tabAdd.BackColor = System.Drawing.Color.PaleTurquoise;
			this.tabAdd.Controls.Add(this.btnExport);
			this.tabAdd.Controls.Add(this.chkNotDue);
			this.tabAdd.Controls.Add(this.btnEdit);
			this.tabAdd.Controls.Add(this.btnNewItem);
			this.tabAdd.Controls.Add(this.calDateDue);
			this.tabAdd.Controls.Add(this.label7);
			this.tabAdd.Controls.Add(this.btnAdd);
			this.tabAdd.Controls.Add(this.btnNewOwner);
			this.tabAdd.Controls.Add(this.cmbOwner);
			this.tabAdd.Controls.Add(this.label6);
			this.tabAdd.Controls.Add(this.btnNewMediaType);
			this.tabAdd.Controls.Add(this.cmbMediaType);
			this.tabAdd.Controls.Add(this.label5);
			this.tabAdd.Controls.Add(this.btnNewGenre);
			this.tabAdd.Controls.Add(this.cmbGenres);
			this.tabAdd.Controls.Add(this.txtBookTitle);
			this.tabAdd.Controls.Add(this.label3);
			this.tabAdd.Controls.Add(this.txtAuthorLastName);
			this.tabAdd.Controls.Add(this.label2);
			this.tabAdd.Controls.Add(this.txtAuthorFirstName);
			this.tabAdd.Controls.Add(this.label4);
			this.tabAdd.Controls.Add(this.label1);
			this.tabAdd.Location = new System.Drawing.Point(4, 22);
			this.tabAdd.Name = "tabAdd";
			this.tabAdd.Padding = new System.Windows.Forms.Padding(3);
			this.tabAdd.Size = new System.Drawing.Size(711, 424);
			this.tabAdd.TabIndex = 0;
			this.tabAdd.Text = "Add";
			// 
			// chkNotDue
			// 
			this.chkNotDue.AutoSize = true;
			this.chkNotDue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkNotDue.Location = new System.Drawing.Point(567, 77);
			this.chkNotDue.Name = "chkNotDue";
			this.chkNotDue.Size = new System.Drawing.Size(73, 17);
			this.chkNotDue.TabIndex = 19;
			this.chkNotDue.Text = "Not Due";
			this.chkNotDue.UseVisualStyleBackColor = true;
			// 
			// btnEdit
			// 
			this.btnEdit.Location = new System.Drawing.Point(654, 39);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.Size = new System.Drawing.Size(39, 23);
			this.btnEdit.TabIndex = 18;
			this.btnEdit.Text = "Edit";
			this.btnEdit.UseVisualStyleBackColor = true;
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// btnNewItem
			// 
			this.btnNewItem.Location = new System.Drawing.Point(6, 39);
			this.btnNewItem.Name = "btnNewItem";
			this.btnNewItem.Size = new System.Drawing.Size(39, 23);
			this.btnNewItem.TabIndex = 17;
			this.btnNewItem.Text = "New";
			this.btnNewItem.UseVisualStyleBackColor = true;
			this.btnNewItem.Click += new System.EventHandler(this.btnNewItem_Click);
			// 
			// calDateDue
			// 
			this.calDateDue.Location = new System.Drawing.Point(317, 75);
			this.calDateDue.Name = "calDateDue";
			this.calDateDue.TabIndex = 16;
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(163, 75);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(116, 19);
			this.label7.TabIndex = 15;
			this.label7.Text = "Date Due";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(32, 323);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(75, 23);
			this.btnAdd.TabIndex = 14;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnNewOwner
			// 
			this.btnNewOwner.Location = new System.Drawing.Point(593, 273);
			this.btnNewOwner.Name = "btnNewOwner";
			this.btnNewOwner.Size = new System.Drawing.Size(39, 23);
			this.btnNewOwner.TabIndex = 13;
			this.btnNewOwner.Text = "New";
			this.btnNewOwner.UseVisualStyleBackColor = true;
			this.btnNewOwner.Click += new System.EventHandler(this.btnNewOwner_Click);
			// 
			// cmbOwner
			// 
			this.cmbOwner.FormattingEnabled = true;
			this.cmbOwner.Location = new System.Drawing.Point(456, 273);
			this.cmbOwner.Name = "cmbOwner";
			this.cmbOwner.Size = new System.Drawing.Size(121, 21);
			this.cmbOwner.TabIndex = 12;
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(456, 249);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(121, 23);
			this.label6.TabIndex = 11;
			this.label6.Text = "Owner";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// btnNewMediaType
			// 
			this.btnNewMediaType.Location = new System.Drawing.Point(381, 273);
			this.btnNewMediaType.Name = "btnNewMediaType";
			this.btnNewMediaType.Size = new System.Drawing.Size(39, 23);
			this.btnNewMediaType.TabIndex = 10;
			this.btnNewMediaType.Text = "New";
			this.btnNewMediaType.UseVisualStyleBackColor = true;
			this.btnNewMediaType.Click += new System.EventHandler(this.btnNewMediaType_Click);
			// 
			// cmbMediaType
			// 
			this.cmbMediaType.FormattingEnabled = true;
			this.cmbMediaType.Location = new System.Drawing.Point(241, 275);
			this.cmbMediaType.Name = "cmbMediaType";
			this.cmbMediaType.Size = new System.Drawing.Size(121, 21);
			this.cmbMediaType.TabIndex = 9;
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(241, 249);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(121, 23);
			this.label5.TabIndex = 8;
			this.label5.Text = "Media Type";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// btnNewGenre
			// 
			this.btnNewGenre.Location = new System.Drawing.Point(172, 273);
			this.btnNewGenre.Name = "btnNewGenre";
			this.btnNewGenre.Size = new System.Drawing.Size(39, 23);
			this.btnNewGenre.TabIndex = 7;
			this.btnNewGenre.Text = "New";
			this.btnNewGenre.UseVisualStyleBackColor = true;
			this.btnNewGenre.Click += new System.EventHandler(this.btnNewGenre_Click);
			// 
			// cmbGenres
			// 
			this.cmbGenres.FormattingEnabled = true;
			this.cmbGenres.Location = new System.Drawing.Point(32, 275);
			this.cmbGenres.Name = "cmbGenres";
			this.cmbGenres.Size = new System.Drawing.Size(121, 21);
			this.cmbGenres.TabIndex = 6;
			// 
			// txtBookTitle
			// 
			this.txtBookTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtBookTitle.Location = new System.Drawing.Point(317, 42);
			this.txtBookTitle.Name = "txtBookTitle";
			this.txtBookTitle.Size = new System.Drawing.Size(315, 20);
			this.txtBookTitle.TabIndex = 5;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(317, 25);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(315, 23);
			this.label3.TabIndex = 4;
			this.label3.Text = "Book Title";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// txtAuthorLastName
			// 
			this.txtAuthorLastName.Location = new System.Drawing.Point(191, 42);
			this.txtAuthorLastName.Name = "txtAuthorLastName";
			this.txtAuthorLastName.Size = new System.Drawing.Size(116, 20);
			this.txtAuthorLastName.TabIndex = 3;
			this.txtAuthorLastName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAuthorLastName_KeyPress);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(188, 25);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(116, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "Author Last Name";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// txtAuthorFirstName
			// 
			this.txtAuthorFirstName.Location = new System.Drawing.Point(51, 42);
			this.txtAuthorFirstName.Name = "txtAuthorFirstName";
			this.txtAuthorFirstName.Size = new System.Drawing.Size(116, 20);
			this.txtAuthorFirstName.TabIndex = 1;
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(32, 249);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(121, 23);
			this.label4.TabIndex = 0;
			this.label4.Text = "Genre";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(51, 25);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(116, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Author First Name";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.reportViewer1);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(711, 424);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			this.tabPage2.Enter += new System.EventHandler(this.tabPage2_Enter);
			// 
			// reportViewer1
			// 
			reportDataSource1.Name = "DataSet1";
			reportDataSource1.Value = this.SelectAllItemsBindingSource;
			this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
			this.reportViewer1.LocalReport.ReportEmbeddedResource = "LRSLibraryBooks.Report3.rdlc";
			this.reportViewer1.Location = new System.Drawing.Point(39, 51);
			this.reportViewer1.Name = "reportViewer1";
			this.reportViewer1.Size = new System.Drawing.Size(396, 246);
			this.reportViewer1.TabIndex = 0;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.BackColor = System.Drawing.Color.MediumBlue;
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.ForeColor = System.Drawing.Color.Red;
			this.label8.Location = new System.Drawing.Point(147, 2);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(314, 31);
			this.label8.TabIndex = 2;
			this.label8.Text = " T E S T  V E R S I O N";
			// 
			// btnExport
			// 
			this.btnExport.Location = new System.Drawing.Point(166, 323);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(75, 23);
			this.btnExport.TabIndex = 20;
			this.btnExport.Text = "Export";
			this.btnExport.UseVisualStyleBackColor = true;
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			// 
			// LRSLibraryBooks
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.PaleTurquoise;
			this.ClientSize = new System.Drawing.Size(743, 474);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.tabControl1);
			this.Name = "LRSLibraryBooks";
			this.Text = "Library Books";
			this.Load += new System.EventHandler(this.LRSLibraryBooks_Load);
			((System.ComponentModel.ISupportInitialize)(this.SelectAllItemsBindingSource)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tabAdd.ResumeLayout(false);
			this.tabAdd.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabAdd;
		private System.Windows.Forms.TextBox txtBookTitle;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtAuthorLastName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtAuthorFirstName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.Button btnNewOwner;
		private System.Windows.Forms.ComboBox cmbOwner;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button btnNewMediaType;
		private System.Windows.Forms.ComboBox cmbMediaType;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button btnNewGenre;
		private System.Windows.Forms.ComboBox cmbGenres;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.MonthCalendar calDateDue;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button btnNewItem;
		private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
		private System.Windows.Forms.BindingSource SelectAllItemsBindingSource;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.CheckBox chkNotDue;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button btnExport;
	}
}

