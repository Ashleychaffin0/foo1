namespace TestReportViewer2 {
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
			this.components = new System.ComponentModel.Container();
			this.btnGo = new System.Windows.Forms.Button();
			this.lldevelDataSet = new TestReportViewer2.lldevelDataSet();
			this.tblAccountsBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.tblAccountsTableAdapter = new TestReportViewer2.lldevelDataSetTableAdapters.tblAccountsTableAdapter();
			this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
			((System.ComponentModel.ISupportInitialize)(this.lldevelDataSet)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tblAccountsBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(12, 27);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 0;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			// 
			// lldevelDataSet
			// 
			this.lldevelDataSet.DataSetName = "lldevelDataSet";
			this.lldevelDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// tblAccountsBindingSource
			// 
			this.tblAccountsBindingSource.DataMember = "tblAccounts";
			this.tblAccountsBindingSource.DataSource = this.lldevelDataSet;
			// 
			// tblAccountsTableAdapter
			// 
			this.tblAccountsTableAdapter.ClearBeforeFill = true;
			// 
			// reportViewer1
			// 
			this.reportViewer1.Location = new System.Drawing.Point(13, 78);
			this.reportViewer1.Name = "reportViewer1";
			this.reportViewer1.Size = new System.Drawing.Size(400, 250);
			this.reportViewer1.TabIndex = 1;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(656, 409);
			this.Controls.Add(this.reportViewer1);
			this.Controls.Add(this.btnGo);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.lldevelDataSet)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tblAccountsBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnGo;
		private lldevelDataSet lldevelDataSet;
		private System.Windows.Forms.BindingSource tblAccountsBindingSource;
		private TestReportViewer2.lldevelDataSetTableAdapters.tblAccountsTableAdapter tblAccountsTableAdapter;
		private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
	}
}

