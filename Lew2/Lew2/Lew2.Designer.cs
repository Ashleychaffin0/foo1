namespace Lew2 {
	partial class Lew2 {
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
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			this.cmbMonth = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbSalesRep = new System.Windows.Forms.ComboBox();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.lblTotalUnits = new System.Windows.Forms.Label();
			this.lblTotalRevenue = new System.Windows.Forms.Label();
			this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.maintenanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.createDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmbMonth
			// 
			this.cmbMonth.FormattingEnabled = true;
			this.cmbMonth.Location = new System.Drawing.Point(181, 103);
			this.cmbMonth.Name = "cmbMonth";
			this.cmbMonth.Size = new System.Drawing.Size(121, 24);
			this.cmbMonth.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(181, 80);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(117, 23);
			this.label1.TabIndex = 1;
			this.label1.Text = "Month";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(21, 80);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(117, 23);
			this.label2.TabIndex = 3;
			this.label2.Text = "Sales Rep";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cmbSalesRep
			// 
			this.cmbSalesRep.FormattingEnabled = true;
			this.cmbSalesRep.Location = new System.Drawing.Point(21, 103);
			this.cmbSalesRep.Name = "cmbSalesRep";
			this.cmbSalesRep.Size = new System.Drawing.Size(121, 24);
			this.cmbSalesRep.TabIndex = 2;
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Location = new System.Drawing.Point(24, 146);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.RowTemplate.Height = 24;
			this.dataGridView1.Size = new System.Drawing.Size(302, 272);
			this.dataGridView1.TabIndex = 4;
			// 
			// label3
			// 
			this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label3.Location = new System.Drawing.Point(387, 37);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(249, 23);
			this.label3.TabIndex = 5;
			this.label3.Text = "12 Month Forecast";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(519, 69);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(117, 23);
			this.label4.TabIndex = 6;
			this.label4.Text = "Revenue";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(387, 69);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(117, 23);
			this.label5.TabIndex = 7;
			this.label5.Text = "Units";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblTotalUnits
			// 
			this.lblTotalUnits.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblTotalUnits.Location = new System.Drawing.Point(387, 103);
			this.lblTotalUnits.Name = "lblTotalUnits";
			this.lblTotalUnits.Size = new System.Drawing.Size(117, 23);
			this.lblTotalUnits.TabIndex = 9;
			this.lblTotalUnits.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblTotalRevenue
			// 
			this.lblTotalRevenue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblTotalRevenue.Location = new System.Drawing.Point(519, 103);
			this.lblTotalRevenue.Name = "lblTotalRevenue";
			this.lblTotalRevenue.Size = new System.Drawing.Size(117, 23);
			this.lblTotalRevenue.TabIndex = 8;
			this.lblTotalRevenue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// chart1
			// 
			chartArea1.Name = "ChartArea1";
			this.chart1.ChartAreas.Add(chartArea1);
			legend1.Name = "Legend1";
			this.chart1.Legends.Add(legend1);
			this.chart1.Location = new System.Drawing.Point(387, 146);
			this.chart1.Name = "chart1";
			series1.ChartArea = "ChartArea1";
			series1.Legend = "Legend1";
			series1.Name = "Series1";
			this.chart1.Series.Add(series1);
			this.chart1.Size = new System.Drawing.Size(408, 272);
			this.chart1.TabIndex = 10;
			this.chart1.Text = "chart1";
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.maintenanceToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(807, 28);
			this.menuStrip1.TabIndex = 11;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// importToolStripMenuItem
			// 
			this.importToolStripMenuItem.Name = "importToolStripMenuItem";
			this.importToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
			this.importToolStripMenuItem.Text = "Import";
			this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
			// 
			// exportToolStripMenuItem
			// 
			this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
			this.exportToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
			this.exportToolStripMenuItem.Text = "Exit";
			// 
			// maintenanceToolStripMenuItem
			// 
			this.maintenanceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createDatabaseToolStripMenuItem});
			this.maintenanceToolStripMenuItem.Name = "maintenanceToolStripMenuItem";
			this.maintenanceToolStripMenuItem.Size = new System.Drawing.Size(106, 24);
			this.maintenanceToolStripMenuItem.Text = "Maintenance";
			// 
			// createDatabaseToolStripMenuItem
			// 
			this.createDatabaseToolStripMenuItem.Name = "createDatabaseToolStripMenuItem";
			this.createDatabaseToolStripMenuItem.Size = new System.Drawing.Size(188, 24);
			this.createDatabaseToolStripMenuItem.Text = "Create Database";
			this.createDatabaseToolStripMenuItem.Click += new System.EventHandler(this.createDatabaseToolStripMenuItem_Click);
			// 
			// Lew2
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(807, 434);
			this.Controls.Add(this.chart1);
			this.Controls.Add(this.lblTotalUnits);
			this.Controls.Add(this.lblTotalRevenue);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cmbSalesRep);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmbMonth);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Lew2";
			this.Text = "Bartizan Forecasting";
			this.Load += new System.EventHandler(this.Lew2_Load);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox cmbMonth;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cmbSalesRep;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblTotalUnits;
		private System.Windows.Forms.Label lblTotalRevenue;
		private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem maintenanceToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem createDatabaseToolStripMenuItem;
	}
}

