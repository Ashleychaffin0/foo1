namespace GetSciAm_3 {
	partial class GetSciAm_3 {
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
			this.btnGo = new System.Windows.Forms.Button();
			this.btnSaveAs = new System.Windows.Forms.Button();
			this.btnGoToPDF = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.cmbYear = new System.Windows.Forms.ComboBox();
			this.cmbDecade = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbMonth = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtIssueTitle = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(498, 37);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 0;
			this.btnGo.Text = "Go to Issue";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// btnSaveAs
			// 
			this.btnSaveAs.Location = new System.Drawing.Point(724, 37);
			this.btnSaveAs.Name = "btnSaveAs";
			this.btnSaveAs.Size = new System.Drawing.Size(75, 23);
			this.btnSaveAs.TabIndex = 2;
			this.btnSaveAs.Text = "Save As";
			this.btnSaveAs.UseVisualStyleBackColor = true;
			this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
			// 
			// btnGoToPDF
			// 
			this.btnGoToPDF.Location = new System.Drawing.Point(611, 37);
			this.btnGoToPDF.Name = "btnGoToPDF";
			this.btnGoToPDF.Size = new System.Drawing.Size(75, 23);
			this.btnGoToPDF.TabIndex = 3;
			this.btnGoToPDF.Text = "Go to PDF";
			this.btnGoToPDF.UseVisualStyleBackColor = true;
			this.btnGoToPDF.Click += new System.EventHandler(this.btnGoToPDF_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(172, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(121, 23);
			this.label1.TabIndex = 4;
			this.label1.Text = "Year";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cmbYear
			// 
			this.cmbYear.FormattingEnabled = true;
			this.cmbYear.Location = new System.Drawing.Point(172, 39);
			this.cmbYear.Name = "cmbYear";
			this.cmbYear.Size = new System.Drawing.Size(121, 21);
			this.cmbYear.TabIndex = 5;
			this.cmbYear.SelectedIndexChanged += new System.EventHandler(this.cmbYear_SelectedIndexChanged);
			// 
			// cmbDecade
			// 
			this.cmbDecade.FormattingEnabled = true;
			this.cmbDecade.Location = new System.Drawing.Point(32, 39);
			this.cmbDecade.Name = "cmbDecade";
			this.cmbDecade.Size = new System.Drawing.Size(121, 21);
			this.cmbDecade.TabIndex = 7;
			this.cmbDecade.SelectedIndexChanged += new System.EventHandler(this.cmbDecade_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(32, 12);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(121, 23);
			this.label2.TabIndex = 6;
			this.label2.Text = "Decade";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cmbMonth
			// 
			this.cmbMonth.FormattingEnabled = true;
			this.cmbMonth.Location = new System.Drawing.Point(315, 39);
			this.cmbMonth.Name = "cmbMonth";
			this.cmbMonth.Size = new System.Drawing.Size(121, 21);
			this.cmbMonth.TabIndex = 9;
			this.cmbMonth.SelectedIndexChanged += new System.EventHandler(this.cmbMonth_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(315, 12);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(121, 23);
			this.label3.TabIndex = 8;
			this.label3.Text = "Month";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtIssueTitle
			// 
			this.txtIssueTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtIssueTitle.Location = new System.Drawing.Point(851, 39);
			this.txtIssueTitle.Name = "txtIssueTitle";
			this.txtIssueTitle.Size = new System.Drawing.Size(401, 20);
			this.txtIssueTitle.TabIndex = 10;
			// 
			// GetSciAm_3
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1264, 83);
			this.Controls.Add(this.txtIssueTitle);
			this.Controls.Add(this.cmbMonth);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cmbDecade);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cmbYear);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnGoToPDF);
			this.Controls.Add(this.btnSaveAs);
			this.Controls.Add(this.btnGo);
			this.Name = "GetSciAm_3";
			this.Text = "Get Sciam 3";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GetSciAm_3_FormClosed);
			this.Load += new System.EventHandler(this.GetSciAm_3_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.Button btnSaveAs;
		private System.Windows.Forms.Button btnGoToPDF;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbYear;
		private System.Windows.Forms.ComboBox cmbDecade;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cmbMonth;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtIssueTitle;
	}
}

