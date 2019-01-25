namespace LLImportStats {
	partial class LLImportStats {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LLImportStats));
			this.cbSystem = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnRefreshImportStats = new System.Windows.Forms.Button();
			this.dgvStats = new System.Windows.Forms.DataGridView();
			this.btnLegend = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.nudRefreshInterval = new System.Windows.Forms.NumericUpDown();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.btnFilter = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabEventSummary = new System.Windows.Forms.TabPage();
			this.dgvSummary = new System.Windows.Forms.DataGridView();
			this.btnRefreshEventSummary = new System.Windows.Forms.Button();
			this.tabEventDetails = new System.Windows.Forms.TabPage();
			this.btnRefreshEventDetails = new System.Windows.Forms.Button();
			this.dgvDetails = new System.Windows.Forms.DataGridView();
			this.tabImportStats = new System.Windows.Forms.TabPage();
			this.chkOnlyActivated = new System.Windows.Forms.CheckBox();
			this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
			this.btnPrint = new System.Windows.Forms.Button();
			this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
			this.btnExport = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.dgvStats)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudRefreshInterval)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tabEventSummary.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvSummary)).BeginInit();
			this.tabEventDetails.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).BeginInit();
			this.tabImportStats.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
			this.SuspendLayout();
			// 
			// cbSystem
			// 
			this.cbSystem.FormattingEnabled = true;
			this.cbSystem.Items.AddRange(new object[] {
			"Prod",
            "DBMart",
            "CrystalTech",
            "Ahmed-Local"});
			this.cbSystem.Location = new System.Drawing.Point(77, 24);
			this.cbSystem.Name = "cbSystem";
			this.cbSystem.Size = new System.Drawing.Size(183, 24);
			this.cbSystem.TabIndex = 0;
			this.cbSystem.SelectedIndexChanged += new System.EventHandler(this.cbSystem_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 27);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(54, 17);
			this.label1.TabIndex = 1;
			this.label1.Text = "System";
			// 
			// btnRefreshImportStats
			// 
			this.btnRefreshImportStats.Location = new System.Drawing.Point(15, 6);
			this.btnRefreshImportStats.Name = "btnRefreshImportStats";
			this.btnRefreshImportStats.Size = new System.Drawing.Size(75, 23);
			this.btnRefreshImportStats.TabIndex = 2;
			this.btnRefreshImportStats.Text = "Refresh";
			this.btnRefreshImportStats.UseVisualStyleBackColor = true;
			this.btnRefreshImportStats.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// dgvStats
			// 
			this.dgvStats.AllowUserToOrderColumns = true;
			this.dgvStats.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dgvStats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvStats.Location = new System.Drawing.Point(0, 37);
			this.dgvStats.Name = "dgvStats";
			this.dgvStats.RowTemplate.Height = 24;
			this.dgvStats.Size = new System.Drawing.Size(672, 333);
			this.dgvStats.TabIndex = 3;
			// 
			// btnLegend
			// 
			this.btnLegend.Location = new System.Drawing.Point(102, 6);
			this.btnLegend.Name = "btnLegend";
			this.btnLegend.Size = new System.Drawing.Size(75, 23);
			this.btnLegend.TabIndex = 9;
			this.btnLegend.Text = "Legend";
			this.btnLegend.UseVisualStyleBackColor = true;
			this.btnLegend.Click += new System.EventHandler(this.btnLegend_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(289, 27);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(101, 17);
			this.label4.TabIndex = 10;
			this.label4.Text = "Refresh (mins)";
			// 
			// nudRefreshInterval
			// 
			this.nudRefreshInterval.Location = new System.Drawing.Point(405, 26);
			this.nudRefreshInterval.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.nudRefreshInterval.Name = "nudRefreshInterval";
			this.nudRefreshInterval.Size = new System.Drawing.Size(67, 22);
			this.nudRefreshInterval.TabIndex = 1;
			this.nudRefreshInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.nudRefreshInterval.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.nudRefreshInterval.ValueChanged += new System.EventHandler(this.nudRefreshInterval_ValueChanged);
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// btnFilter
			// 
			this.btnFilter.Location = new System.Drawing.Point(496, 26);
			this.btnFilter.Name = "btnFilter";
			this.btnFilter.Size = new System.Drawing.Size(75, 23);
			this.btnFilter.TabIndex = 2;
			this.btnFilter.Text = "Filter";
			this.btnFilter.UseVisualStyleBackColor = true;
			this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabEventSummary);
			this.tabControl1.Controls.Add(this.tabEventDetails);
			this.tabControl1.Controls.Add(this.tabImportStats);
			this.tabControl1.Location = new System.Drawing.Point(16, 54);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(683, 405);
			this.tabControl1.TabIndex = 13;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tabEventSummary
			// 
			this.tabEventSummary.Controls.Add(this.dgvSummary);
			this.tabEventSummary.Controls.Add(this.btnRefreshEventSummary);
			this.tabEventSummary.Location = new System.Drawing.Point(4, 25);
			this.tabEventSummary.Name = "tabEventSummary";
			this.tabEventSummary.Padding = new System.Windows.Forms.Padding(3);
			this.tabEventSummary.Size = new System.Drawing.Size(675, 376);
			this.tabEventSummary.TabIndex = 1;
			this.tabEventSummary.Text = "Event Summary";
			this.tabEventSummary.UseVisualStyleBackColor = true;
			// 
			// dgvSummary
			// 
			this.dgvSummary.AllowUserToOrderColumns = true;
			this.dgvSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dgvSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvSummary.Location = new System.Drawing.Point(0, 37);
			this.dgvSummary.Name = "dgvSummary";
			this.dgvSummary.RowTemplate.Height = 24;
			this.dgvSummary.Size = new System.Drawing.Size(672, 333);
			this.dgvSummary.TabIndex = 5;
			this.dgvSummary.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSummary_CellDoubleClick);
			// 
			// btnRefreshEventSummary
			// 
			this.btnRefreshEventSummary.Location = new System.Drawing.Point(15, 6);
			this.btnRefreshEventSummary.Name = "btnRefreshEventSummary";
			this.btnRefreshEventSummary.Size = new System.Drawing.Size(75, 23);
			this.btnRefreshEventSummary.TabIndex = 3;
			this.btnRefreshEventSummary.Text = "Refresh";
			this.btnRefreshEventSummary.UseVisualStyleBackColor = true;
			this.btnRefreshEventSummary.Click += new System.EventHandler(this.btnRefreshSummary_Click);
			// 
			// tabEventDetails
			// 
			this.tabEventDetails.Controls.Add(this.btnRefreshEventDetails);
			this.tabEventDetails.Controls.Add(this.dgvDetails);
			this.tabEventDetails.Location = new System.Drawing.Point(4, 25);
			this.tabEventDetails.Name = "tabEventDetails";
			this.tabEventDetails.Size = new System.Drawing.Size(675, 376);
			this.tabEventDetails.TabIndex = 2;
			this.tabEventDetails.Text = "Event Details";
			this.tabEventDetails.UseVisualStyleBackColor = true;
			// 
			// btnRefreshEventDetails
			// 
			this.btnRefreshEventDetails.Location = new System.Drawing.Point(15, 6);
			this.btnRefreshEventDetails.Name = "btnRefreshEventDetails";
			this.btnRefreshEventDetails.Size = new System.Drawing.Size(75, 23);
			this.btnRefreshEventDetails.TabIndex = 5;
			this.btnRefreshEventDetails.Text = "Refresh";
			this.btnRefreshEventDetails.UseVisualStyleBackColor = true;
			this.btnRefreshEventDetails.Click += new System.EventHandler(this.btnRefreshEventDetails_Click);
			// 
			// dgvDetails
			// 
			this.dgvDetails.AllowUserToOrderColumns = true;
			this.dgvDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dgvDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvDetails.Location = new System.Drawing.Point(0, 37);
			this.dgvDetails.Name = "dgvDetails";
			this.dgvDetails.RowTemplate.Height = 24;
			this.dgvDetails.Size = new System.Drawing.Size(672, 333);
			this.dgvDetails.TabIndex = 4;
			// 
			// tabImportStats
			// 
			this.tabImportStats.Controls.Add(this.chkOnlyActivated);
			this.tabImportStats.Controls.Add(this.btnRefreshImportStats);
			this.tabImportStats.Controls.Add(this.dgvStats);
			this.tabImportStats.Controls.Add(this.btnLegend);
			this.tabImportStats.Location = new System.Drawing.Point(4, 25);
			this.tabImportStats.Name = "tabImportStats";
			this.tabImportStats.Padding = new System.Windows.Forms.Padding(3);
			this.tabImportStats.Size = new System.Drawing.Size(675, 376);
			this.tabImportStats.TabIndex = 0;
			this.tabImportStats.Text = "Import Stats";
			this.tabImportStats.UseVisualStyleBackColor = true;
			// 
			// chkOnlyActivated
			// 
			this.chkOnlyActivated.AutoSize = true;
			this.chkOnlyActivated.Location = new System.Drawing.Point(203, 8);
			this.chkOnlyActivated.Name = "chkOnlyActivated";
			this.chkOnlyActivated.Size = new System.Drawing.Size(121, 21);
			this.chkOnlyActivated.TabIndex = 10;
			this.chkOnlyActivated.Text = "Only Activated";
			this.chkOnlyActivated.UseVisualStyleBackColor = true;
			this.chkOnlyActivated.CheckedChanged += new System.EventHandler(this.chkOnlyActivated_CheckedChanged);
			// 
			// btnPrint
			// 
			this.btnPrint.Location = new System.Drawing.Point(496, -3);
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(75, 23);
			this.btnPrint.TabIndex = 14;
			this.btnPrint.Text = "Print";
			this.btnPrint.UseVisualStyleBackColor = true;
			this.btnPrint.Visible = false;
			this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
			// 
			// printPreviewDialog1
			// 
			this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
			this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
			this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
			this.printPreviewDialog1.Enabled = true;
			this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
			this.printPreviewDialog1.Name = "printPreviewDialog1";
			this.printPreviewDialog1.Visible = false;
			// 
			// btnExport
			// 
			this.btnExport.Location = new System.Drawing.Point(590, 27);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(75, 23);
			this.btnExport.TabIndex = 15;
			this.btnExport.Text = "Export";
			this.btnExport.UseVisualStyleBackColor = true;
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// LLImportStats
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(709, 471);
			this.Controls.Add(this.btnExport);
			this.Controls.Add(this.btnPrint);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.btnFilter);
			this.Controls.Add(this.nudRefreshInterval);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cbSystem);
			this.Name = "LLImportStats";
			this.Text = "LeadsLightning Import Statistics";
			this.Load += new System.EventHandler(this.LLImportStats_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvStats)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudRefreshInterval)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tabEventSummary.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvSummary)).EndInit();
			this.tabEventDetails.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).EndInit();
			this.tabImportStats.ResumeLayout(false);
			this.tabImportStats.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox cbSystem;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnRefreshImportStats;
		private System.Windows.Forms.DataGridView dgvStats;
		private System.Windows.Forms.Button btnLegend;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown nudRefreshInterval;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Button btnFilter;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabImportStats;
		private System.Windows.Forms.TabPage tabEventSummary;
		private System.Windows.Forms.TabPage tabEventDetails;
		private System.Windows.Forms.Button btnRefreshEventSummary;
		private System.Windows.Forms.DataGridView dgvDetails;
		private System.Windows.Forms.DataGridView dgvSummary;
		private System.Windows.Forms.Button btnRefreshEventDetails;
		private System.Windows.Forms.BindingSource bindingSource1;
		private System.Windows.Forms.Button btnPrint;
		private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
		private System.Windows.Forms.Button btnExport;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.CheckBox chkOnlyActivated;
	}
}

