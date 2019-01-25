namespace WinFormsChannel9 {
	partial class WinFormsChannel9 {
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
			this.PageNoStart = new System.Windows.Forms.NumericUpDown();
			this.btnGo = new System.Windows.Forms.Button();
			this.web1 = new System.Windows.Forms.WebBrowser();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.grid1 = new System.Windows.Forms.DataGridView();
			this.btnEditKeywords = new System.Windows.Forms.Button();
			this.PageNoEnd = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.btnStop = new System.Windows.Forms.Button();
			this.btnEmptyDatabase = new System.Windows.Forms.Button();
			this.lblElapsed = new System.Windows.Forms.Label();
			this.btnSearch = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.PageNoStart)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PageNoEnd)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(57, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Start Page";
			// 
			// PageNoStart
			// 
			this.PageNoStart.Location = new System.Drawing.Point(79, 20);
			this.PageNoStart.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.PageNoStart.Name = "PageNoStart";
			this.PageNoStart.Size = new System.Drawing.Size(70, 20);
			this.PageNoStart.TabIndex = 0;
			this.PageNoStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.PageNoStart.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(353, 17);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 2;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// web1
			// 
			this.web1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.web1.Location = new System.Drawing.Point(16, 63);
			this.web1.MinimumSize = new System.Drawing.Size(20, 20);
			this.web1.Name = "web1";
			this.web1.Size = new System.Drawing.Size(1071, 485);
			this.web1.TabIndex = 3;
			this.web1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.web1_DocumentCompleted);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Location = new System.Drawing.Point(0, 551);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(1099, 22);
			this.statusStrip1.TabIndex = 4;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// grid1
			// 
			this.grid1.AllowUserToAddRows = false;
			this.grid1.AllowUserToDeleteRows = false;
			this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grid1.Location = new System.Drawing.Point(487, 161);
			this.grid1.Name = "grid1";
			this.grid1.ReadOnly = true;
			this.grid1.Size = new System.Drawing.Size(382, 150);
			this.grid1.TabIndex = 2;
			this.grid1.Visible = false;
			// 
			// btnEditKeywords
			// 
			this.btnEditKeywords.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnEditKeywords.Location = new System.Drawing.Point(827, 17);
			this.btnEditKeywords.Name = "btnEditKeywords";
			this.btnEditKeywords.Size = new System.Drawing.Size(111, 23);
			this.btnEditKeywords.TabIndex = 4;
			this.btnEditKeywords.Text = "Edit Keywords";
			this.btnEditKeywords.UseVisualStyleBackColor = true;
			this.btnEditKeywords.Click += new System.EventHandler(this.btnEditKeywords_Click);
			// 
			// PageNoEnd
			// 
			this.PageNoEnd.Location = new System.Drawing.Point(241, 20);
			this.PageNoEnd.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.PageNoEnd.Name = "PageNoEnd";
			this.PageNoEnd.Size = new System.Drawing.Size(70, 20);
			this.PageNoEnd.TabIndex = 1;
			this.PageNoEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.PageNoEnd.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(175, 22);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(54, 13);
			this.label2.TabIndex = 7;
			this.label2.Text = "End Page";
			// 
			// btnStop
			// 
			this.btnStop.Location = new System.Drawing.Point(456, 17);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(75, 23);
			this.btnStop.TabIndex = 3;
			this.btnStop.Text = "Stop";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// btnEmptyDatabase
			// 
			this.btnEmptyDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnEmptyDatabase.Location = new System.Drawing.Point(961, 17);
			this.btnEmptyDatabase.Name = "btnEmptyDatabase";
			this.btnEmptyDatabase.Size = new System.Drawing.Size(111, 23);
			this.btnEmptyDatabase.TabIndex = 8;
			this.btnEmptyDatabase.Text = "Empty Database";
			this.btnEmptyDatabase.UseVisualStyleBackColor = true;
			this.btnEmptyDatabase.Click += new System.EventHandler(this.btnEmptyDatabase_Click);
			// 
			// lblElapsed
			// 
			this.lblElapsed.AutoSize = true;
			this.lblElapsed.Location = new System.Drawing.Point(563, 22);
			this.lblElapsed.Name = "lblElapsed";
			this.lblElapsed.Size = new System.Drawing.Size(0, 13);
			this.lblElapsed.TabIndex = 9;
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Location = new System.Drawing.Point(701, 17);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(111, 23);
			this.btnSearch.TabIndex = 10;
			this.btnSearch.Text = "Search";
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// WinFormsChannel9
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1099, 573);
			this.Controls.Add(this.btnSearch);
			this.Controls.Add(this.lblElapsed);
			this.Controls.Add(this.btnEmptyDatabase);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.PageNoEnd);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnEditKeywords);
			this.Controls.Add(this.grid1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.web1);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.PageNoStart);
			this.Controls.Add(this.label1);
			this.Name = "WinFormsChannel9";
			this.Text = "WinForms Channel 9";
			this.Load += new System.EventHandler(this.WinFormsChannel9_Load);
			((System.ComponentModel.ISupportInitialize)(this.PageNoStart)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PageNoEnd)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown PageNoStart;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.WebBrowser web1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.DataGridView grid1;
		private System.Windows.Forms.Button btnEditKeywords;
		private System.Windows.Forms.NumericUpDown PageNoEnd;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.Button btnEmptyDatabase;
		private System.Windows.Forms.Label lblElapsed;
		private System.Windows.Forms.Button btnSearch;
	}
}

