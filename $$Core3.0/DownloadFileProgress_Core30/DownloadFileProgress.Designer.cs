namespace LRS.Utils {
	partial class DownloadFileProgress {
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.lblTotalSize = new System.Windows.Forms.Label();
			this.lblPercentDone = new System.Windows.Forms.Label();
			this.ProgressBar1 = new System.Windows.Forms.ProgressBar();
			this.lblAmountDownloaded = new System.Windows.Forms.Label();
			this.LinkLabel1 = new System.Windows.Forms.LinkLabel();
			this.lblDownloadSpeed = new System.Windows.Forms.Label();
			this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// lblTotalSize
			// 
			this.lblTotalSize.ForeColor = System.Drawing.Color.Red;
			this.lblTotalSize.Location = new System.Drawing.Point(0, 0);
			this.lblTotalSize.Name = "lblTotalSize";
			this.lblTotalSize.Size = new System.Drawing.Size(74, 24);
			this.lblTotalSize.TabIndex = 0;
			this.lblTotalSize.Text = "TotBytes";
			this.lblTotalSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblPercentDone
			// 
			this.lblPercentDone.ForeColor = System.Drawing.Color.Red;
			this.lblPercentDone.Location = new System.Drawing.Point(230, 0);
			this.lblPercentDone.Name = "lblPercentDone";
			this.lblPercentDone.Size = new System.Drawing.Size(36, 24);
			this.lblPercentDone.TabIndex = 1;
			this.lblPercentDone.Text = "Pct Done";
			this.lblPercentDone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// progressBar1
			// 
			this.ProgressBar1.Location = new System.Drawing.Point(272, 0);
			this.ProgressBar1.Name = "progressBar1";
			this.ProgressBar1.Size = new System.Drawing.Size(209, 20);
			this.ProgressBar1.TabIndex = 3;
			// 
			// lblAmountDownloaded
			// 
			this.lblAmountDownloaded.ForeColor = System.Drawing.Color.Red;
			this.lblAmountDownloaded.Location = new System.Drawing.Point(79, 0);
			this.lblAmountDownloaded.Name = "lblAmountDownloaded";
			this.lblAmountDownloaded.Size = new System.Drawing.Size(74, 24);
			this.lblAmountDownloaded.TabIndex = 5;
			this.lblAmountDownloaded.Text = "Bytes to Date";
			this.lblAmountDownloaded.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// linkLabel1
			// 
			this.LinkLabel1.Location = new System.Drawing.Point(505, 6);
			this.LinkLabel1.Name = "linkLabel1";
			this.LinkLabel1.Size = new System.Drawing.Size(562, 23);
			this.LinkLabel1.TabIndex = 6;
			this.LinkLabel1.TabStop = true;
			this.LinkLabel1.Text = "linkLabel1";
			// 
			// lblDownloadSpeed
			// 
			this.lblDownloadSpeed.ForeColor = System.Drawing.Color.Red;
			this.lblDownloadSpeed.Location = new System.Drawing.Point(159, 0);
			this.lblDownloadSpeed.Name = "lblDownloadSpeed";
			this.lblDownloadSpeed.Size = new System.Drawing.Size(74, 24);
			this.lblDownloadSpeed.TabIndex = 7;
			this.lblDownloadSpeed.Text = "DL Speed";
			this.lblDownloadSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// toolTip1
			// 
			this.ToolTip1.AutoPopDelay = 30000;
			this.ToolTip1.InitialDelay = 500;
			this.ToolTip1.IsBalloon = true;
			this.ToolTip1.ReshowDelay = 100;
			this.ToolTip1.ShowAlways = true;
			// 
			// DownloadProgress
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblDownloadSpeed);
			this.Controls.Add(this.LinkLabel1);
			this.Controls.Add(this.lblAmountDownloaded);
			this.Controls.Add(this.ProgressBar1);
			this.Controls.Add(this.lblPercentDone);
			this.Controls.Add(this.lblTotalSize);
			this.Name = "DownloadProgress";
			this.Size = new System.Drawing.Size(1017, 23);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.ToolTip ToolTip1;
		public System.Windows.Forms.Label lblTotalSize;
		public System.Windows.Forms.Label lblPercentDone;
		public System.Windows.Forms.Label lblAmountDownloaded;
		public System.Windows.Forms.Label lblDownloadSpeed;
		public System.Windows.Forms.LinkLabel LinkLabel1;
		public System.Windows.Forms.ProgressBar ProgressBar1;
	}
}
