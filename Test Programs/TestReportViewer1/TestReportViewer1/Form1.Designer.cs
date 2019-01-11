namespace TestReportViewer1 {
	partial class Form1 {
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
			this.BtnGo = new System.Windows.Forms.Button();
			this.rpt = new Microsoft.Reporting.WinForms.ReportViewer();
			this.dataSet1 = new System.Data.DataSet();
			this.lldevelDataSet1 = new TestReportViewer1.lldevelDataSet();
			this.tblAccountsTableAdapter1 = new TestReportViewer1.lldevelDataSetTableAdapters.tblAccountsTableAdapter();
			((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.lldevelDataSet1)).BeginInit();
			this.SuspendLayout();
			// 
			// BtnGo
			// 
			this.BtnGo.Location = new System.Drawing.Point(12, 22);
			this.BtnGo.Name = "BtnGo";
			this.BtnGo.Size = new System.Drawing.Size(75, 23);
			this.BtnGo.TabIndex = 0;
			this.BtnGo.Text = "Go";
			this.BtnGo.UseVisualStyleBackColor = true;
			// 
			// rpt
			// 
			this.rpt.LocalReport.ReportPath = "C:\\LRS\\C#\\TestReportViewer1\\TestReportViewer1\\Report1.rdlc";
			this.rpt.Location = new System.Drawing.Point(12, 78);
			this.rpt.Name = "rpt";
			this.rpt.Size = new System.Drawing.Size(603, 381);
			this.rpt.TabIndex = 1;
			// 
			// dataSet1
			// 
			this.dataSet1.DataSetName = "NewDataSet";
			// 
			// lldevelDataSet1
			// 
			this.lldevelDataSet1.DataSetName = "lldevelDataSet";
			this.lldevelDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// tblAccountsTableAdapter1
			// 
			this.tblAccountsTableAdapter1.ClearBeforeFill = true;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(627, 471);
			this.Controls.Add(this.rpt);
			this.Controls.Add(this.BtnGo);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.lldevelDataSet1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button BtnGo;
		private Microsoft.Reporting.WinForms.ReportViewer rpt;
		private System.Data.DataSet dataSet1;
		private lldevelDataSet lldevelDataSet1;
		private TestReportViewer1.lldevelDataSetTableAdapters.tblAccountsTableAdapter tblAccountsTableAdapter1;
	}
}

