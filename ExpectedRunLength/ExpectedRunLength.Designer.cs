namespace ExpectedRunLength {
	partial class ExpectedRunLength {
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
			this.label2 = new System.Windows.Forms.Label();
			this.TxtIterations = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.LblElapsedTime = new System.Windows.Forms.Label();
			this.BtnGo = new System.Windows.Forms.Button();
			this.ChtStats = new System.Windows.Forms.DataVisualization.Charting.Chart();
			((System.ComponentModel.ISupportInitialize)(this.ChtStats)).BeginInit();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(39, 15);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(66, 17);
			this.label2.TabIndex = 2;
			this.label2.Text = "Iterations";
			// 
			// TxtIterations
			// 
			this.TxtIterations.Location = new System.Drawing.Point(191, 12);
			this.TxtIterations.Name = "TxtIterations";
			this.TxtIterations.Size = new System.Drawing.Size(120, 22);
			this.TxtIterations.TabIndex = 3;
			this.TxtIterations.Text = "10,000";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(39, 65);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(126, 17);
			this.label3.TabIndex = 4;
			this.label3.Text = "Elapsed Time (ms)";
			// 
			// LblElapsedTime
			// 
			this.LblElapsedTime.AutoSize = true;
			this.LblElapsedTime.Location = new System.Drawing.Point(188, 65);
			this.LblElapsedTime.Name = "LblElapsedTime";
			this.LblElapsedTime.Size = new System.Drawing.Size(0, 17);
			this.LblElapsedTime.TabIndex = 5;
			// 
			// BtnGo
			// 
			this.BtnGo.Location = new System.Drawing.Point(42, 115);
			this.BtnGo.Name = "BtnGo";
			this.BtnGo.Size = new System.Drawing.Size(75, 23);
			this.BtnGo.TabIndex = 6;
			this.BtnGo.Text = "Go";
			this.BtnGo.UseVisualStyleBackColor = true;
			this.BtnGo.Click += new System.EventHandler(this.BtnGo_Click);
			// 
			// ChtStats
			// 
			this.ChtStats.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			chartArea1.Name = "ChartArea1";
			this.ChtStats.ChartAreas.Add(chartArea1);
			legend1.Name = "Legend1";
			this.ChtStats.Legends.Add(legend1);
			this.ChtStats.Location = new System.Drawing.Point(42, 166);
			this.ChtStats.Name = "ChtStats";
			series1.ChartArea = "ChartArea1";
			series1.Legend = "Legend1";
			series1.Name = "Max Run Lengths";
			this.ChtStats.Series.Add(series1);
			this.ChtStats.Size = new System.Drawing.Size(727, 380);
			this.ChtStats.TabIndex = 7;
			this.ChtStats.Text = "chart1";
			// 
			// ExpectedRunLength
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 568);
			this.Controls.Add(this.ChtStats);
			this.Controls.Add(this.BtnGo);
			this.Controls.Add(this.LblElapsedTime);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.TxtIterations);
			this.Controls.Add(this.label2);
			this.Name = "ExpectedRunLength";
			this.Text = "Max Run";
			((System.ComponentModel.ISupportInitialize)(this.ChtStats)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox TxtIterations;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label LblElapsedTime;
		private System.Windows.Forms.Button BtnGo;
		private System.Windows.Forms.DataVisualization.Charting.Chart ChtStats;
	}
}

