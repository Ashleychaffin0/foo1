namespace nsTrackFoldingAtHome {
	partial class TrackF_H {
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
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
			this.label1 = new System.Windows.Forms.Label();
			this.lblRank = new System.Windows.Forms.Label();
			this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbChartTypes = new System.Windows.Forms.ComboBox();
			this.btnGetData = new System.Windows.Forms.Button();
			this.chk3D = new System.Windows.Forms.CheckBox();
			this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
			this.btnTalkToMe = new System.Windows.Forms.Button();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.TrackF_HBindingSource = new System.Windows.Forms.BindingSource(this.components);
			((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.TrackF_HBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(28, 29);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Rank";
			// 
			// lblRank
			// 
			this.lblRank.AutoSize = true;
			this.lblRank.Location = new System.Drawing.Point(91, 28);
			this.lblRank.Name = "lblRank";
			this.lblRank.Size = new System.Drawing.Size(0, 17);
			this.lblRank.TabIndex = 1;
			// 
			// chart1
			// 
			this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			chartArea1.Name = "ChartArea1";
			this.chart1.ChartAreas.Add(chartArea1);
			legend1.Name = "Legend1";
			this.chart1.Legends.Add(legend1);
			this.chart1.Location = new System.Drawing.Point(17, 3);
			this.chart1.Name = "chart1";
			series1.ChartArea = "ChartArea1";
			series1.Legend = "Legend1";
			series1.Name = "Series1";
			this.chart1.Series.Add(series1);
			this.chart1.Size = new System.Drawing.Size(804, 353);
			this.chart1.TabIndex = 2;
			this.chart1.Text = "chart1";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(458, 29);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(78, 17);
			this.label2.TabIndex = 3;
			this.label2.Text = "Chart Type";
			// 
			// cmbChartTypes
			// 
			this.cmbChartTypes.FormattingEnabled = true;
			this.cmbChartTypes.Location = new System.Drawing.Point(573, 26);
			this.cmbChartTypes.Name = "cmbChartTypes";
			this.cmbChartTypes.Size = new System.Drawing.Size(180, 24);
			this.cmbChartTypes.TabIndex = 4;
			this.cmbChartTypes.SelectedIndexChanged += new System.EventHandler(this.cmbChartTypes_SelectedIndexChanged);
			// 
			// btnGetData
			// 
			this.btnGetData.Location = new System.Drawing.Point(31, 64);
			this.btnGetData.Name = "btnGetData";
			this.btnGetData.Size = new System.Drawing.Size(75, 23);
			this.btnGetData.TabIndex = 5;
			this.btnGetData.Text = "Get Data";
			this.btnGetData.UseVisualStyleBackColor = true;
			this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
			// 
			// chk3D
			// 
			this.chk3D.AutoSize = true;
			this.chk3D.Location = new System.Drawing.Point(790, 28);
			this.chk3D.Name = "chk3D";
			this.chk3D.Size = new System.Drawing.Size(48, 21);
			this.chk3D.TabIndex = 8;
			this.chk3D.Text = "3D";
			this.chk3D.UseVisualStyleBackColor = true;
			this.chk3D.CheckedChanged += new System.EventHandler(this.chk3D_CheckedChanged);
			// 
			// reportViewer1
			// 
			this.reportViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			reportDataSource1.Name = "DataSet1";
			reportDataSource1.Value = this.TrackF_HBindingSource;
			this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
			this.reportViewer1.LocalReport.ReportEmbeddedResource = "nsTrackFoldingAtHome.Report1.rdlc";
			this.reportViewer1.Location = new System.Drawing.Point(17, 17);
			this.reportViewer1.Name = "reportViewer1";
			this.reportViewer1.Size = new System.Drawing.Size(804, 310);
			this.reportViewer1.TabIndex = 9;
			// 
			// btnTalkToMe
			// 
			this.btnTalkToMe.Location = new System.Drawing.Point(150, 64);
			this.btnTalkToMe.Name = "btnTalkToMe";
			this.btnTalkToMe.Size = new System.Drawing.Size(100, 23);
			this.btnTalkToMe.TabIndex = 10;
			this.btnTalkToMe.Text = "Talk to Me";
			this.btnTalkToMe.UseVisualStyleBackColor = true;
			this.btnTalkToMe.Click += new System.EventHandler(this.btnTalkToMe_Click);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitContainer1.Location = new System.Drawing.Point(12, 106);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.chart1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.reportViewer1);
			this.splitContainer1.Size = new System.Drawing.Size(828, 676);
			this.splitContainer1.SplitterDistance = 338;
			this.splitContainer1.TabIndex = 11;
			// 
			// TrackF_HBindingSource
			// 
			this.TrackF_HBindingSource.DataMember = "StatsData";
			this.TrackF_HBindingSource.DataSource = typeof(nsTrackFoldingAtHome.TrackF_H);
			this.TrackF_HBindingSource.CurrentChanged += new System.EventHandler(this.TrackF_HBindingSource_CurrentChanged);
			// 
			// TrackF_H
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(852, 794);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.btnTalkToMe);
			this.Controls.Add(this.chk3D);
			this.Controls.Add(this.btnGetData);
			this.Controls.Add(this.cmbChartTypes);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lblRank);
			this.Controls.Add(this.label1);
			this.Name = "TrackF_H";
			this.Text = "LRS Folding@Home Statistics";
			this.Load += new System.EventHandler(this.TrackF_H_Load);
			((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.TrackF_HBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblRank;
		private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cmbChartTypes;
		private System.Windows.Forms.Button btnGetData;
		private System.Windows.Forms.CheckBox chk3D;
		private System.Windows.Forms.BindingSource TrackF_HBindingSource;
		private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
		private System.Windows.Forms.Button btnTalkToMe;
		private System.Windows.Forms.SplitContainer splitContainer1;
	}
}

