namespace AnalyzeNovel {
	partial class AnalyzeNovel {
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
			System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
			this.label1 = new System.Windows.Forms.Label();
			this.LvStats = new System.Windows.Forms.ListView();
			this.nWord = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.xWordText = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.xWordCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.xRunningCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.xRunningPercent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.xWordLength = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.xLengthCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.TxtFindWord = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.TxtFolder = new System.Windows.Forms.TextBox();
			this.BtnBrowse = new System.Windows.Forms.Button();
			this.BsWordsLengths = new System.Windows.Forms.BindingSource(this.components);
			this.LvWordsCounts = new System.Windows.Forms.ListView();
			this.Word = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Count = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.PercentDecrease = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.TvToc = new System.Windows.Forms.TreeView();
			((System.ComponentModel.ISupportInitialize)(this.BsWordsLengths)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(26, 69);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(44, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Novel";
			// 
			// LvStats
			// 
			this.LvStats.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LvStats.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nWord,
            this.xWordText,
            this.xWordCount,
            this.xRunningCount,
            this.xRunningPercent,
            this.xWordLength,
            this.xLengthCount});
			this.LvStats.FullRowSelect = true;
			this.LvStats.HideSelection = false;
			this.LvStats.Location = new System.Drawing.Point(384, 69);
			this.LvStats.Name = "LvStats";
			this.LvStats.Size = new System.Drawing.Size(495, 212);
			this.LvStats.TabIndex = 4;
			this.LvStats.UseCompatibleStateImageBehavior = false;
			this.LvStats.View = System.Windows.Forms.View.Details;
			// 
			// nWord
			// 
			this.nWord.Text = "";
			// 
			// xWordText
			// 
			this.xWordText.Text = "Word";
			this.xWordText.Width = 100;
			// 
			// xWordCount
			// 
			this.xWordCount.Text = "Count";
			this.xWordCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.xWordCount.Width = 76;
			// 
			// xRunningCount
			// 
			this.xRunningCount.Text = "Running Count";
			this.xRunningCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.xRunningCount.Width = 107;
			// 
			// xRunningPercent
			// 
			this.xRunningPercent.Text = "Running %";
			this.xRunningPercent.Width = 85;
			// 
			// xWordLength
			// 
			this.xWordLength.Text = "Word Length";
			this.xWordLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.xWordLength.Width = 100;
			// 
			// xLengthCount
			// 
			this.xLengthCount.Text = "Length Count";
			this.xLengthCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.xLengthCount.Width = 110;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(26, 305);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 17);
			this.label2.TabIndex = 5;
			this.label2.Text = "Stats";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(26, 262);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(35, 17);
			this.label3.TabIndex = 6;
			this.label3.Text = "Find";
			// 
			// TxtFindWord
			// 
			this.TxtFindWord.Location = new System.Drawing.Point(90, 259);
			this.TxtFindWord.Name = "TxtFindWord";
			this.TxtFindWord.Size = new System.Drawing.Size(248, 22);
			this.TxtFindWord.TabIndex = 7;
			this.TxtFindWord.TextChanged += new System.EventHandler(this.TxtFindWord_TextChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(22, 27);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 17);
			this.label4.TabIndex = 8;
			this.label4.Text = "Folder";
			// 
			// TxtFolder
			// 
			this.TxtFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TxtFolder.Location = new System.Drawing.Point(90, 24);
			this.TxtFolder.Name = "TxtFolder";
			this.TxtFolder.Size = new System.Drawing.Size(697, 22);
			this.TxtFolder.TabIndex = 9;
			// 
			// BtnBrowse
			// 
			this.BtnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnBrowse.Location = new System.Drawing.Point(804, 24);
			this.BtnBrowse.Name = "BtnBrowse";
			this.BtnBrowse.Size = new System.Drawing.Size(75, 23);
			this.BtnBrowse.TabIndex = 10;
			this.BtnBrowse.Text = "Browse";
			this.BtnBrowse.UseVisualStyleBackColor = true;
			this.BtnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
			// 
			// LvWordsCounts
			// 
			this.LvWordsCounts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.LvWordsCounts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Word,
            this.Count,
            this.PercentDecrease});
			this.LvWordsCounts.Location = new System.Drawing.Point(90, 305);
			this.LvWordsCounts.Name = "LvWordsCounts";
			this.LvWordsCounts.Size = new System.Drawing.Size(265, 346);
			this.LvWordsCounts.TabIndex = 11;
			this.LvWordsCounts.UseCompatibleStateImageBehavior = false;
			this.LvWordsCounts.View = System.Windows.Forms.View.Details;
			// 
			// Word
			// 
			this.Word.Text = "Word";
			this.Word.Width = 64;
			// 
			// Count
			// 
			this.Count.Text = "Count";
			this.Count.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.Count.Width = 76;
			// 
			// PercentDecrease
			// 
			this.PercentDecrease.Text = "% Decrease";
			this.PercentDecrease.Width = 100;
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
			this.chart1.Location = new System.Drawing.Point(384, 305);
			this.chart1.Name = "chart1";
			series1.ChartArea = "ChartArea1";
			series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
			series1.Color = System.Drawing.Color.Red;
			series1.LabelFormat = "N0";
			series1.Legend = "Legend1";
			series1.Name = "Zipf";
			series2.BorderWidth = 3;
			series2.ChartArea = "ChartArea1";
			series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
			series2.Color = System.Drawing.Color.Blue;
			series2.Legend = "Legend1";
			series2.Name = "RegressionLine";
			this.chart1.Series.Add(series1);
			this.chart1.Series.Add(series2);
			this.chart1.Size = new System.Drawing.Size(495, 355);
			this.chart1.TabIndex = 13;
			this.chart1.Text = "Zipf\'s Law";
			title1.Name = "Slope = 123";
			this.chart1.Titles.Add(title1);
			// 
			// TvToc
			// 
			this.TvToc.Location = new System.Drawing.Point(90, 69);
			this.TvToc.Name = "TvToc";
			this.TvToc.Size = new System.Drawing.Size(265, 184);
			this.TvToc.TabIndex = 14;
			this.TvToc.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TvToc_AfterSelect);
			// 
			// AnalyzeNovel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(891, 663);
			this.Controls.Add(this.TvToc);
			this.Controls.Add(this.chart1);
			this.Controls.Add(this.LvWordsCounts);
			this.Controls.Add(this.BtnBrowse);
			this.Controls.Add(this.TxtFolder);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.TxtFindWord);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.LvStats);
			this.Controls.Add(this.label1);
			this.Name = "AnalyzeNovel";
			this.Text = "Analyze Novel";
			((System.ComponentModel.ISupportInitialize)(this.BsWordsLengths)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListView LvStats;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ColumnHeader xWordText;
		private System.Windows.Forms.ColumnHeader xWordCount;
		private System.Windows.Forms.ColumnHeader xWordLength;
		private System.Windows.Forms.ColumnHeader xLengthCount;
		private System.Windows.Forms.ColumnHeader xRunningCount;
		private System.Windows.Forms.ColumnHeader xRunningPercent;
		private System.Windows.Forms.ColumnHeader nWord;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox TxtFindWord;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox TxtFolder;
		private System.Windows.Forms.Button BtnBrowse;
		private System.Windows.Forms.BindingSource BsWordsLengths;
		private System.Windows.Forms.ListView LvWordsCounts;
		private System.Windows.Forms.ColumnHeader Word;
		private System.Windows.Forms.ColumnHeader Count;
		private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ColumnHeader PercentDecrease;
		private System.Windows.Forms.TreeView TvToc;
	}
}

