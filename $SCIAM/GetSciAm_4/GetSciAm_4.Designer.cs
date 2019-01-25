namespace GetSciAm_4 {
	partial class GetSciAm_4 {
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
			this.lblDownloaded = new System.Windows.Forms.Label();
			this.lblDownloadInMB = new System.Windows.Forms.Label();
			this.chkWeekly = new System.Windows.Forms.CheckBox();
			this.WeekPicker = new System.Windows.Forms.DateTimePicker();
			this.btnNext = new System.Windows.Forms.Button();
			this.lbTrace = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(948, 74);
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
			this.btnGoToPDF.Text = "Get PDF";
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
			this.cmbYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmbYear.FormattingEnabled = true;
			this.cmbYear.Location = new System.Drawing.Point(172, 39);
			this.cmbYear.Name = "cmbYear";
			this.cmbYear.Size = new System.Drawing.Size(121, 28);
			this.cmbYear.TabIndex = 5;
			this.cmbYear.SelectedIndexChanged += new System.EventHandler(this.cmbYear_SelectedIndexChanged);
			// 
			// cmbDecade
			// 
			this.cmbDecade.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmbDecade.FormattingEnabled = true;
			this.cmbDecade.Location = new System.Drawing.Point(32, 39);
			this.cmbDecade.Name = "cmbDecade";
			this.cmbDecade.Size = new System.Drawing.Size(121, 28);
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
			this.cmbMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmbMonth.FormattingEnabled = true;
			this.cmbMonth.Location = new System.Drawing.Point(315, 39);
			this.cmbMonth.Name = "cmbMonth";
			this.cmbMonth.Size = new System.Drawing.Size(121, 28);
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
			// lblDownloaded
			// 
			this.lblDownloaded.Location = new System.Drawing.Point(529, 74);
			this.lblDownloaded.Name = "lblDownloaded";
			this.lblDownloaded.Size = new System.Drawing.Size(73, 13);
			this.lblDownloaded.TabIndex = 12;
			this.lblDownloaded.Text = "Downloaded --   ";
			this.lblDownloaded.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblDownloadInMB
			// 
			this.lblDownloadInMB.Location = new System.Drawing.Point(608, 74);
			this.lblDownloadInMB.Name = "lblDownloadInMB";
			this.lblDownloadInMB.Size = new System.Drawing.Size(73, 13);
			this.lblDownloadInMB.TabIndex = 13;
			this.lblDownloadInMB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// chkWeekly
			// 
			this.chkWeekly.AutoSize = true;
			this.chkWeekly.Location = new System.Drawing.Point(868, 37);
			this.chkWeekly.Name = "chkWeekly";
			this.chkWeekly.Size = new System.Drawing.Size(62, 17);
			this.chkWeekly.TabIndex = 14;
			this.chkWeekly.Text = "Weekly";
			this.chkWeekly.UseVisualStyleBackColor = true;
			this.chkWeekly.CheckedChanged += new System.EventHandler(this.chkWeekly_CheckedChanged);
			// 
			// WeekPicker
			// 
			this.WeekPicker.Location = new System.Drawing.Point(1038, 12);
			this.WeekPicker.Name = "WeekPicker";
			this.WeekPicker.Size = new System.Drawing.Size(214, 20);
			this.WeekPicker.TabIndex = 15;
			this.WeekPicker.Value = new System.DateTime(1846, 1, 1, 0, 0, 0, 0);
			// 
			// btnNext
			// 
			this.btnNext.Location = new System.Drawing.Point(948, 37);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(75, 23);
			this.btnNext.TabIndex = 16;
			this.btnNext.Text = "Next";
			this.btnNext.UseVisualStyleBackColor = true;
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// lbTrace
			// 
			this.lbTrace.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbTrace.FormattingEnabled = true;
			this.lbTrace.Location = new System.Drawing.Point(1038, 37);
			this.lbTrace.Name = "lbTrace";
			this.lbTrace.Size = new System.Drawing.Size(214, 160);
			this.lbTrace.TabIndex = 17;
			// 
			// GetSciAm_4
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1264, 203);
			this.Controls.Add(this.lbTrace);
			this.Controls.Add(this.btnNext);
			this.Controls.Add(this.WeekPicker);
			this.Controls.Add(this.chkWeekly);
			this.Controls.Add(this.lblDownloadInMB);
			this.Controls.Add(this.lblDownloaded);
			this.Controls.Add(this.cmbMonth);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cmbDecade);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cmbYear);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnGoToPDF);
			this.Controls.Add(this.btnSaveAs);
			this.Controls.Add(this.btnGo);
			this.Name = "GetSciAm_4";
			this.Text = "Get Sciam 4";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GetSciAm_4_FormClosed);
			this.Load += new System.EventHandler(this.GetSciAm_4_Load);
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
		private System.Windows.Forms.Label lblDownloaded;
		private System.Windows.Forms.Label lblDownloadInMB;
		private System.Windows.Forms.CheckBox chkWeekly;
		private System.Windows.Forms.DateTimePicker WeekPicker;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.ListBox lbTrace;
	}
}

