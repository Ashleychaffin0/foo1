namespace DBTableSizes {
	partial class DBTablesSizes {
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
			this.btnReadDatabase = new System.Windows.Forms.Button();
			this.btnSortByName = new System.Windows.Forms.Button();
			this.grdTableSizes = new System.Windows.Forms.DataGridView();
			this.btnSortBySize = new System.Windows.Forms.Button();
			this.btnSortByIndexSize = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.cmbDBs = new System.Windows.Forms.ComboBox();
			this.btnSortByRows = new System.Windows.Forms.Button();
			this.btnExport = new System.Windows.Forms.Button();
			this.btnSortByReserved = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.grdTableSizes)).BeginInit();
			this.SuspendLayout();
			// 
			// btnReadDatabase
			// 
			this.btnReadDatabase.Location = new System.Drawing.Point(231, 12);
			this.btnReadDatabase.Name = "btnReadDatabase";
			this.btnReadDatabase.Size = new System.Drawing.Size(105, 23);
			this.btnReadDatabase.TabIndex = 0;
			this.btnReadDatabase.Text = "Read Database";
			this.btnReadDatabase.UseVisualStyleBackColor = true;
			this.btnReadDatabase.Click += new System.EventHandler(this.btnReadDatabase_Click);
			// 
			// btnSortByName
			// 
			this.btnSortByName.Location = new System.Drawing.Point(373, 12);
			this.btnSortByName.Name = "btnSortByName";
			this.btnSortByName.Size = new System.Drawing.Size(98, 23);
			this.btnSortByName.TabIndex = 4;
			this.btnSortByName.Text = "Sort by Name";
			this.btnSortByName.UseVisualStyleBackColor = true;
			this.btnSortByName.Click += new System.EventHandler(this.btnSortByName_Click);
			// 
			// grdTableSizes
			// 
			this.grdTableSizes.AllowUserToOrderColumns = true;
			this.grdTableSizes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grdTableSizes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grdTableSizes.Location = new System.Drawing.Point(13, 42);
			this.grdTableSizes.Name = "grdTableSizes";
			this.grdTableSizes.ReadOnly = true;
			this.grdTableSizes.Size = new System.Drawing.Size(1059, 372);
			this.grdTableSizes.TabIndex = 5;
			this.grdTableSizes.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdTableSizes_ColumnHeaderMouseClick);
			// 
			// btnSortBySize
			// 
			this.btnSortBySize.Location = new System.Drawing.Point(490, 13);
			this.btnSortBySize.Name = "btnSortBySize";
			this.btnSortBySize.Size = new System.Drawing.Size(98, 23);
			this.btnSortBySize.TabIndex = 6;
			this.btnSortBySize.Text = "Sort by Size";
			this.btnSortBySize.UseVisualStyleBackColor = true;
			this.btnSortBySize.Click += new System.EventHandler(this.btnSortBySize_Click);
			// 
			// btnSortByIndexSize
			// 
			this.btnSortByIndexSize.Location = new System.Drawing.Point(607, 12);
			this.btnSortByIndexSize.Name = "btnSortByIndexSize";
			this.btnSortByIndexSize.Size = new System.Drawing.Size(113, 23);
			this.btnSortByIndexSize.TabIndex = 7;
			this.btnSortByIndexSize.Text = "Sort by Index Size";
			this.btnSortByIndexSize.UseVisualStyleBackColor = true;
			this.btnSortByIndexSize.Click += new System.EventHandler(this.btnSortByIndexSize_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 13);
			this.label1.TabIndex = 8;
			this.label1.Text = "Database";
			// 
			// cmbDBs
			// 
			this.cmbDBs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbDBs.FormattingEnabled = true;
			this.cmbDBs.Location = new System.Drawing.Point(91, 14);
			this.cmbDBs.Name = "cmbDBs";
			this.cmbDBs.Size = new System.Drawing.Size(121, 21);
			this.cmbDBs.TabIndex = 9;
			// 
			// btnSortByRows
			// 
			this.btnSortByRows.Location = new System.Drawing.Point(856, 12);
			this.btnSortByRows.Name = "btnSortByRows";
			this.btnSortByRows.Size = new System.Drawing.Size(98, 23);
			this.btnSortByRows.TabIndex = 10;
			this.btnSortByRows.Text = "Sort by Rows";
			this.btnSortByRows.UseVisualStyleBackColor = true;
			this.btnSortByRows.Click += new System.EventHandler(this.btnSortByRows_Click);
			// 
			// btnExport
			// 
			this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExport.Location = new System.Drawing.Point(974, 12);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(98, 23);
			this.btnExport.TabIndex = 11;
			this.btnExport.Text = "Export\r\n";
			this.btnExport.UseVisualStyleBackColor = true;
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			// 
			// btnSortByReserved
			// 
			this.btnSortByReserved.Location = new System.Drawing.Point(739, 12);
			this.btnSortByReserved.Name = "btnSortByReserved";
			this.btnSortByReserved.Size = new System.Drawing.Size(98, 23);
			this.btnSortByReserved.TabIndex = 12;
			this.btnSortByReserved.Text = "Sort by Reserved";
			this.btnSortByReserved.UseVisualStyleBackColor = true;
			this.btnSortByReserved.Click += new System.EventHandler(this.btnSortByReserved_Click);
			// 
			// DBTablesSizes
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1084, 420);
			this.Controls.Add(this.btnSortByReserved);
			this.Controls.Add(this.btnExport);
			this.Controls.Add(this.btnSortByRows);
			this.Controls.Add(this.cmbDBs);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnSortByIndexSize);
			this.Controls.Add(this.btnSortBySize);
			this.Controls.Add(this.grdTableSizes);
			this.Controls.Add(this.btnSortByName);
			this.Controls.Add(this.btnReadDatabase);
			this.Name = "DBTablesSizes";
			this.Text = "Database Table Sizes";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.grdTableSizes)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnReadDatabase;
		private System.Windows.Forms.Button btnSortByName;
		private System.Windows.Forms.DataGridView grdTableSizes;
		private System.Windows.Forms.Button btnSortBySize;
		private System.Windows.Forms.Button btnSortByIndexSize;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbDBs;
		private System.Windows.Forms.Button btnSortByRows;
		private System.Windows.Forms.Button btnExport;
		private System.Windows.Forms.Button btnSortByReserved;
	}
}

