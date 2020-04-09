namespace CheckFor192Rip {
	partial class CheckFor192Rip {
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
            this.label1 = new System.Windows.Forms.Label();
            this.TxtStartingFolder = new System.Windows.Forms.TextBox();
            this.BtnBrowse = new System.Windows.Forms.Button();
            this.BtnGo = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.LblCount192 = new System.Windows.Forms.Label();
            this.LblCount128 = new System.Windows.Forms.Label();
            this.LblSize128 = new System.Windows.Forms.Label();
            this.LblSize192 = new System.Windows.Forms.Label();
            this.LblAvgSize128 = new System.Windows.Forms.Label();
            this.LblAvgSize192 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.LblDuration192 = new System.Windows.Forms.Label();
            this.LblDuration128 = new System.Windows.Forms.Label();
            this.LblAvgDuration128 = new System.Windows.Forms.Label();
            this.LblAvgDuration192 = new System.Windows.Forms.Label();
            this.BtnTestPDM = new System.Windows.Forms.Button();
            this.LblEstimatedSize = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.LblDone = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LblEarliest = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.LblElapsed = new System.Windows.Forms.Label();
            this.BtnAnalyze = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Starting Folder";
            // 
            // TxtStartingFolder
            // 
            this.TxtStartingFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtStartingFolder.Location = new System.Drawing.Point(140, 36);
            this.TxtStartingFolder.Name = "TxtStartingFolder";
            this.TxtStartingFolder.Size = new System.Drawing.Size(537, 22);
            this.TxtStartingFolder.TabIndex = 1;
            // 
            // BtnBrowse
            // 
            this.BtnBrowse.Location = new System.Drawing.Point(700, 36);
            this.BtnBrowse.Name = "BtnBrowse";
            this.BtnBrowse.Size = new System.Drawing.Size(75, 23);
            this.BtnBrowse.TabIndex = 2;
            this.BtnBrowse.Text = "Browse";
            this.BtnBrowse.UseVisualStyleBackColor = true;
            this.BtnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
            // 
            // BtnGo
            // 
            this.BtnGo.Location = new System.Drawing.Point(30, 71);
            this.BtnGo.Name = "BtnGo";
            this.BtnGo.Size = new System.Drawing.Size(75, 23);
            this.BtnGo.TabIndex = 3;
            this.BtnGo.Text = "Go";
            this.BtnGo.UseVisualStyleBackColor = true;
            this.BtnGo.Click += new System.EventHandler(this.BtnGo_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 224);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Rate 192";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 187);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Rate 128";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(137, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "Albums";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(235, 157);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 17);
            this.label6.TabIndex = 8;
            this.label6.Text = "Size";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(444, 157);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 17);
            this.label7.TabIndex = 9;
            this.label7.Text = "Duration";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(550, 157);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(119, 17);
            this.label9.TabIndex = 11;
            this.label9.Text = "Average Duration";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // LblCount192
            // 
            this.LblCount192.Location = new System.Drawing.Point(137, 224);
            this.LblCount192.Name = "LblCount192";
            this.LblCount192.Size = new System.Drawing.Size(54, 17);
            this.LblCount192.TabIndex = 12;
            this.LblCount192.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblCount128
            // 
            this.LblCount128.Location = new System.Drawing.Point(137, 187);
            this.LblCount128.Name = "LblCount128";
            this.LblCount128.Size = new System.Drawing.Size(54, 17);
            this.LblCount128.TabIndex = 13;
            this.LblCount128.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblSize128
            // 
            this.LblSize128.Location = new System.Drawing.Point(235, 187);
            this.LblSize128.Name = "LblSize128";
            this.LblSize128.Size = new System.Drawing.Size(68, 17);
            this.LblSize128.TabIndex = 16;
            this.LblSize128.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblSize192
            // 
            this.LblSize192.Location = new System.Drawing.Point(235, 224);
            this.LblSize192.Name = "LblSize192";
            this.LblSize192.Size = new System.Drawing.Size(68, 17);
            this.LblSize192.TabIndex = 15;
            this.LblSize192.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblAvgSize128
            // 
            this.LblAvgSize128.Location = new System.Drawing.Point(328, 187);
            this.LblAvgSize128.Name = "LblAvgSize128";
            this.LblAvgSize128.Size = new System.Drawing.Size(68, 17);
            this.LblAvgSize128.TabIndex = 20;
            this.LblAvgSize128.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblAvgSize192
            // 
            this.LblAvgSize192.Location = new System.Drawing.Point(328, 224);
            this.LblAvgSize192.Name = "LblAvgSize192";
            this.LblAvgSize192.Size = new System.Drawing.Size(68, 17);
            this.LblAvgSize192.TabIndex = 19;
            this.LblAvgSize192.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(328, 157);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(68, 17);
            this.label13.TabIndex = 18;
            this.label13.Text = "Avg. Size";
            this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // LblDuration192
            // 
            this.LblDuration192.Location = new System.Drawing.Point(444, 224);
            this.LblDuration192.Name = "LblDuration192";
            this.LblDuration192.Size = new System.Drawing.Size(100, 17);
            this.LblDuration192.TabIndex = 24;
            this.LblDuration192.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // LblDuration128
            // 
            this.LblDuration128.Location = new System.Drawing.Point(444, 187);
            this.LblDuration128.Name = "LblDuration128";
            this.LblDuration128.Size = new System.Drawing.Size(100, 17);
            this.LblDuration128.TabIndex = 25;
            this.LblDuration128.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // LblAvgDuration128
            // 
            this.LblAvgDuration128.Location = new System.Drawing.Point(550, 187);
            this.LblAvgDuration128.Name = "LblAvgDuration128";
            this.LblAvgDuration128.Size = new System.Drawing.Size(100, 17);
            this.LblAvgDuration128.TabIndex = 27;
            this.LblAvgDuration128.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // LblAvgDuration192
            // 
            this.LblAvgDuration192.Location = new System.Drawing.Point(550, 224);
            this.LblAvgDuration192.Name = "LblAvgDuration192";
            this.LblAvgDuration192.Size = new System.Drawing.Size(100, 17);
            this.LblAvgDuration192.TabIndex = 26;
            this.LblAvgDuration192.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // BtnTestPDM
            // 
            this.BtnTestPDM.Location = new System.Drawing.Point(30, 311);
            this.BtnTestPDM.Name = "BtnTestPDM";
            this.BtnTestPDM.Size = new System.Drawing.Size(143, 23);
            this.BtnTestPDM.TabIndex = 28;
            this.BtnTestPDM.Text = "Test PDM";
            this.BtnTestPDM.UseVisualStyleBackColor = true;
            this.BtnTestPDM.Click += new System.EventHandler(this.BtnTestPDM_Click);
            // 
            // LblEstimatedSize
            // 
            this.LblEstimatedSize.Location = new System.Drawing.Point(235, 270);
            this.LblEstimatedSize.Name = "LblEstimatedSize";
            this.LblEstimatedSize.Size = new System.Drawing.Size(68, 17);
            this.LblEstimatedSize.TabIndex = 29;
            this.LblEstimatedSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(83, 270);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(135, 17);
            this.label8.TabIndex = 30;
            this.label8.Text = "Estimated Final Size";
            // 
            // LblDone
            // 
            this.LblDone.AutoSize = true;
            this.LblDone.Location = new System.Drawing.Point(137, 74);
            this.LblDone.Name = "LblDone";
            this.LblDone.Size = new System.Drawing.Size(0, 17);
            this.LblDone.TabIndex = 31;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(328, 270);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 17);
            this.label3.TabIndex = 32;
            this.label3.Text = "Earliest 192 Album";
            // 
            // LblEarliest
            // 
            this.LblEarliest.Location = new System.Drawing.Point(460, 270);
            this.LblEarliest.Name = "LblEarliest";
            this.LblEarliest.Size = new System.Drawing.Size(84, 17);
            this.LblEarliest.TabIndex = 33;
            this.LblEarliest.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(27, 113);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 17);
            this.label10.TabIndex = 34;
            this.label10.Text = "Elapsed";
            // 
            // LblElapsed
            // 
            this.LblElapsed.Location = new System.Drawing.Point(137, 113);
            this.LblElapsed.Name = "LblElapsed";
            this.LblElapsed.Size = new System.Drawing.Size(100, 17);
            this.LblElapsed.TabIndex = 35;
            this.LblElapsed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BtnAnalyze
            // 
            this.BtnAnalyze.Location = new System.Drawing.Point(301, 107);
            this.BtnAnalyze.Name = "BtnAnalyze";
            this.BtnAnalyze.Size = new System.Drawing.Size(75, 23);
            this.BtnAnalyze.TabIndex = 36;
            this.BtnAnalyze.Text = "Analyze";
            this.BtnAnalyze.UseVisualStyleBackColor = true;
            this.BtnAnalyze.Click += new System.EventHandler(this.BtnAnalyze_Click);
            // 
            // CheckFor192Rip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BtnAnalyze);
            this.Controls.Add(this.LblElapsed);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.LblEarliest);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LblDone);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.LblEstimatedSize);
            this.Controls.Add(this.BtnTestPDM);
            this.Controls.Add(this.LblAvgDuration128);
            this.Controls.Add(this.LblAvgDuration192);
            this.Controls.Add(this.LblDuration128);
            this.Controls.Add(this.LblDuration192);
            this.Controls.Add(this.LblAvgSize128);
            this.Controls.Add(this.LblAvgSize192);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.LblSize128);
            this.Controls.Add(this.LblSize192);
            this.Controls.Add(this.LblCount128);
            this.Controls.Add(this.LblCount192);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BtnGo);
            this.Controls.Add(this.BtnBrowse);
            this.Controls.Add(this.TxtStartingFolder);
            this.Controls.Add(this.label1);
            this.Name = "CheckFor192Rip";
            this.Text = "Check for 192";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox TxtStartingFolder;
		private System.Windows.Forms.Button BtnBrowse;
		private System.Windows.Forms.Button BtnGo;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label LblCount192;
		private System.Windows.Forms.Label LblCount128;
		private System.Windows.Forms.Label LblSize128;
		private System.Windows.Forms.Label LblSize192;
		private System.Windows.Forms.Label LblAvgSize128;
		private System.Windows.Forms.Label LblAvgSize192;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label LblDuration192;
		private System.Windows.Forms.Label LblDuration128;
		private System.Windows.Forms.Label LblAvgDuration128;
		private System.Windows.Forms.Label LblAvgDuration192;
		private System.Windows.Forms.Button BtnTestPDM;
		private System.Windows.Forms.Label LblEstimatedSize;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label LblDone;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label LblEarliest;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label LblElapsed;
		private System.Windows.Forms.Button BtnAnalyze;
	}
}

