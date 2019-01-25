namespace MonitorIAEM {
	partial class MonitorIAEM {
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
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.lblLastUpdate = new System.Windows.Forms.Label();
			this.lvSwipes = new System.Windows.Forms.ListView();
			this.colTime = new System.Windows.Forms.ColumnHeader();
			this.colCountDevel = new System.Windows.Forms.ColumnHeader();
			this.colDeltaDelta = new System.Windows.Forms.ColumnHeader();
			this.colProdCount = new System.Windows.Forms.ColumnHeader();
			this.colProdDelta = new System.Windows.Forms.ColumnHeader();
			this.colDevtemps = new System.Windows.Forms.ColumnHeader();
			this.colDevTempsDelta = new System.Windows.Forms.ColumnHeader();
			this.colProdDevTemps = new System.Windows.Forms.ColumnHeader();
			this.colProdTempsDelta = new System.Windows.Forms.ColumnHeader();
			this.lblSwipes = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtUpdateInterval = new System.Windows.Forms.TextBox();
			this.btnGo = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// timer1
			// 
			this.timer1.Interval = 300000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// lblLastUpdate
			// 
			this.lblLastUpdate.AutoSize = true;
			this.lblLastUpdate.Location = new System.Drawing.Point(9, 8);
			this.lblLastUpdate.Name = "lblLastUpdate";
			this.lblLastUpdate.Size = new System.Drawing.Size(105, 17);
			this.lblLastUpdate.TabIndex = 0;
			this.lblLastUpdate.Text = "Last Updated : ";
			// 
			// lvSwipes
			// 
			this.lvSwipes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.lvSwipes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTime,
            this.colCountDevel,
            this.colDeltaDelta,
            this.colProdCount,
            this.colProdDelta,
            this.colDevtemps,
            this.colDevTempsDelta,
            this.colProdDevTemps,
            this.colProdTempsDelta});
			this.lvSwipes.Location = new System.Drawing.Point(12, 91);
			this.lvSwipes.Name = "lvSwipes";
			this.lvSwipes.Size = new System.Drawing.Size(940, 351);
			this.lvSwipes.TabIndex = 1;
			this.lvSwipes.UseCompatibleStateImageBehavior = false;
			this.lvSwipes.View = System.Windows.Forms.View.Details;
			// 
			// colTime
			// 
			this.colTime.Text = "Time";
			this.colTime.Width = 84;
			// 
			// colCountDevel
			// 
			this.colCountDevel.Text = "Devel Count";
			this.colCountDevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.colCountDevel.Width = 100;
			// 
			// colDeltaDelta
			// 
			this.colDeltaDelta.Text = "Devel Delta";
			this.colDeltaDelta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.colDeltaDelta.Width = 84;
			// 
			// colProdCount
			// 
			this.colProdCount.Text = "Prod Count";
			this.colProdCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.colProdCount.Width = 100;
			// 
			// colProdDelta
			// 
			this.colProdDelta.Text = "Prod Delta";
			this.colProdDelta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.colProdDelta.Width = 100;
			// 
			// colDevtemps
			// 
			this.colDevtemps.Text = "Dev Temps";
			this.colDevtemps.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.colDevtemps.Width = 100;
			// 
			// colDevTempsDelta
			// 
			this.colDevTempsDelta.Text = "Dev Temps Delta";
			this.colDevTempsDelta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.colDevTempsDelta.Width = 120;
			// 
			// colProdDevTemps
			// 
			this.colProdDevTemps.Text = "Prod Temps";
			this.colProdDevTemps.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.colProdDevTemps.Width = 100;
			// 
			// colProdTempsDelta
			// 
			this.colProdTempsDelta.Text = "Prod Temps Delta";
			this.colProdTempsDelta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.colProdTempsDelta.Width = 130;
			// 
			// lblSwipes
			// 
			this.lblSwipes.Location = new System.Drawing.Point(12, 70);
			this.lblSwipes.Name = "lblSwipes";
			this.lblSwipes.Size = new System.Drawing.Size(940, 18);
			this.lblSwipes.TabIndex = 2;
			this.lblSwipes.Text = "Swipes and Temp Files";
			this.lblSwipes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 43);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(162, 17);
			this.label1.TabIndex = 3;
			this.label1.Text = "Update Period (Minutes)";
			// 
			// txtUpdateInterval
			// 
			this.txtUpdateInterval.Location = new System.Drawing.Point(204, 43);
			this.txtUpdateInterval.Name = "txtUpdateInterval";
			this.txtUpdateInterval.Size = new System.Drawing.Size(100, 22);
			this.txtUpdateInterval.TabIndex = 4;
			this.txtUpdateInterval.Text = "5";
			this.txtUpdateInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(353, 41);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 5;
			this.btnGo.Text = "Set";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// MonitorIAEM
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1036, 470);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.txtUpdateInterval);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblSwipes);
			this.Controls.Add(this.lvSwipes);
			this.Controls.Add(this.lblLastUpdate);
			this.Name = "MonitorIAEM";
			this.Text = "Monitor IAEM";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label lblLastUpdate;
		private System.Windows.Forms.ListView lvSwipes;
		private System.Windows.Forms.ColumnHeader colTime;
		private System.Windows.Forms.ColumnHeader colCountDevel;
		private System.Windows.Forms.ColumnHeader colDeltaDelta;
		private System.Windows.Forms.Label lblSwipes;
		private System.Windows.Forms.ColumnHeader colProdCount;
		private System.Windows.Forms.ColumnHeader colProdDelta;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtUpdateInterval;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.ColumnHeader colDevtemps;
		private System.Windows.Forms.ColumnHeader colDevTempsDelta;
		private System.Windows.Forms.ColumnHeader colProdDevTemps;
		private System.Windows.Forms.ColumnHeader colProdTempsDelta;
	}
}

