namespace NetRWStatsW {
	partial class NetRWStatsW {
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblTotalReceived = new System.Windows.Forms.Label();
			this.lblTotalSent = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lblDeltaReceived = new System.Windows.Forms.Label();
			this.lblDeltaSent = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cmbUnits = new System.Windows.Forms.ComboBox();
			this.BtnStopGo = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// timer1
			// 
			this.timer1.Interval = 5000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(59, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(108, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Total Received";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(183, 9);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(111, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Total Sent";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblTotalReceived
			// 
			this.lblTotalReceived.Location = new System.Drawing.Point(56, 31);
			this.lblTotalReceived.Name = "lblTotalReceived";
			this.lblTotalReceived.Size = new System.Drawing.Size(111, 13);
			this.lblTotalReceived.TabIndex = 3;
			this.lblTotalReceived.Text = "Total Received";
			this.lblTotalReceived.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblTotalSent
			// 
			this.lblTotalSent.Location = new System.Drawing.Point(183, 31);
			this.lblTotalSent.Name = "lblTotalSent";
			this.lblTotalSent.Size = new System.Drawing.Size(111, 13);
			this.lblTotalSent.TabIndex = 4;
			this.lblTotalSent.Text = "Total Sent";
			this.lblTotalSent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(9, 54);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Change";
			// 
			// lblDeltaReceived
			// 
			this.lblDeltaReceived.Location = new System.Drawing.Point(56, 54);
			this.lblDeltaReceived.Name = "lblDeltaReceived";
			this.lblDeltaReceived.Size = new System.Drawing.Size(111, 13);
			this.lblDeltaReceived.TabIndex = 6;
			this.lblDeltaReceived.Text = "Delta Rcvd";
			this.lblDeltaReceived.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblDeltaSent
			// 
			this.lblDeltaSent.Location = new System.Drawing.Point(183, 54);
			this.lblDeltaSent.Name = "lblDeltaSent";
			this.lblDeltaSent.Size = new System.Drawing.Size(111, 13);
			this.lblDeltaSent.TabIndex = 7;
			this.lblDeltaSent.Text = "Delta Sent";
			this.lblDeltaSent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(354, 9);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(45, 13);
			this.label4.TabIndex = 8;
			this.label4.Text = "Units";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// cmbUnits
			// 
			this.cmbUnits.FormattingEnabled = true;
			this.cmbUnits.Items.AddRange(new object[] {
            "Bytes",
            "MB",
            "GB"});
			this.cmbUnits.Location = new System.Drawing.Point(408, 6);
			this.cmbUnits.Name = "cmbUnits";
			this.cmbUnits.Size = new System.Drawing.Size(72, 21);
			this.cmbUnits.TabIndex = 9;
			this.cmbUnits.SelectedIndexChanged += new System.EventHandler(this.cmbUnits_SelectedIndexChanged);
			// 
			// BtnStopGo
			// 
			this.BtnStopGo.Location = new System.Drawing.Point(324, 31);
			this.BtnStopGo.Name = "BtnStopGo";
			this.BtnStopGo.Size = new System.Drawing.Size(75, 23);
			this.BtnStopGo.TabIndex = 10;
			this.BtnStopGo.Text = "Pause";
			this.BtnStopGo.UseVisualStyleBackColor = true;
			this.BtnStopGo.Click += new System.EventHandler(this.BtnStopGo_Click);
			// 
			// NeyRWStatsW
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(507, 80);
			this.Controls.Add(this.BtnStopGo);
			this.Controls.Add(this.cmbUnits);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.lblDeltaSent);
			this.Controls.Add(this.lblDeltaReceived);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lblTotalSent);
			this.Controls.Add(this.lblTotalReceived);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "NeyRWStatsW";
			this.Text = "Network I/O Stats";
			this.Load += new System.EventHandler(this.NeyRWStatsW_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblTotalReceived;
		private System.Windows.Forms.Label lblTotalSent;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblDeltaReceived;
		private System.Windows.Forms.Label lblDeltaSent;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cmbUnits;
		private System.Windows.Forms.Button BtnStopGo;
	}
}

