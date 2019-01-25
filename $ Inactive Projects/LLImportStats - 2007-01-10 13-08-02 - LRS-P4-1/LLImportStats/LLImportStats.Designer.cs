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
			this.cbSystem = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnGo = new System.Windows.Forms.Button();
			this.dgvStats = new System.Windows.Forms.DataGridView();
			this.btnLegend = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.nudRefreshInterval = new System.Windows.Forms.NumericUpDown();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.btnFilter = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgvStats)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudRefreshInterval)).BeginInit();
			this.SuspendLayout();
			// 
			// cbSystem
			// 
			this.cbSystem.FormattingEnabled = true;
			this.cbSystem.Items.AddRange(new object[] {
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
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(16, 65);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 2;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// dgvStats
			// 
			this.dgvStats.AllowUserToOrderColumns = true;
			this.dgvStats.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dgvStats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvStats.Location = new System.Drawing.Point(16, 103);
			this.dgvStats.Name = "dgvStats";
			this.dgvStats.RowTemplate.Height = 24;
			this.dgvStats.Size = new System.Drawing.Size(683, 356);
			this.dgvStats.TabIndex = 3;
			// 
			// btnLegend
			// 
			this.btnLegend.Location = new System.Drawing.Point(103, 65);
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
			this.nudRefreshInterval.TabIndex = 11;
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
			this.btnFilter.Location = new System.Drawing.Point(546, 25);
			this.btnFilter.Name = "btnFilter";
			this.btnFilter.Size = new System.Drawing.Size(75, 23);
			this.btnFilter.TabIndex = 12;
			this.btnFilter.Text = "Filter";
			this.btnFilter.UseVisualStyleBackColor = true;
			this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
			// 
			// LLImportStats
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(711, 471);
			this.Controls.Add(this.btnFilter);
			this.Controls.Add(this.nudRefreshInterval);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.btnLegend);
			this.Controls.Add(this.dgvStats);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cbSystem);
			this.Name = "LLImportStats";
			this.Text = "LeadsLightning Import Statistics";
			this.Load += new System.EventHandler(this.LLImportStats_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvStats)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudRefreshInterval)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox cbSystem;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.DataGridView dgvStats;
		private System.Windows.Forms.Button btnLegend;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown nudRefreshInterval;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Button btnFilter;
	}
}

