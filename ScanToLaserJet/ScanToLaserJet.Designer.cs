namespace ScanToLaserJet {
	partial class ScanToLaserJet {
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
			this.cmbPrinter = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnPrint = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.PicBox = new System.Windows.Forms.PictureBox();
			this.BtnScanNewPage = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PicBox)).BeginInit();
			this.SuspendLayout();
			// 
			// cmbPrinter
			// 
			this.cmbPrinter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbPrinter.FormattingEnabled = true;
			this.cmbPrinter.Location = new System.Drawing.Point(96, 15);
			this.cmbPrinter.Name = "cmbPrinter";
			this.cmbPrinter.Size = new System.Drawing.Size(582, 24);
			this.cmbPrinter.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(18, 18);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(50, 17);
			this.label2.TabIndex = 2;
			this.label2.Text = "Printer";
			// 
			// btnPrint
			// 
			this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPrint.Location = new System.Drawing.Point(713, 12);
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(75, 23);
			this.btnPrint.TabIndex = 6;
			this.btnPrint.Text = "Print";
			this.btnPrint.UseVisualStyleBackColor = true;
			this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.PicBox);
			this.panel1.Location = new System.Drawing.Point(21, 88);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(767, 350);
			this.panel1.TabIndex = 7;
			// 
			// PicBox
			// 
			this.PicBox.Location = new System.Drawing.Point(3, 3);
			this.PicBox.Name = "PicBox";
			this.PicBox.Size = new System.Drawing.Size(749, 330);
			this.PicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.PicBox.TabIndex = 0;
			this.PicBox.TabStop = false;
			// 
			// BtnScanNewPage
			// 
			this.BtnScanNewPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnScanNewPage.Location = new System.Drawing.Point(24, 45);
			this.BtnScanNewPage.Name = "BtnScanNewPage";
			this.BtnScanNewPage.Size = new System.Drawing.Size(169, 37);
			this.BtnScanNewPage.TabIndex = 8;
			this.BtnScanNewPage.Text = "Scan new page";
			this.BtnScanNewPage.UseVisualStyleBackColor = true;
			this.BtnScanNewPage.Click += new System.EventHandler(this.BtnScanNewPage_Click);
			// 
			// ScanToLaserJet
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.BtnScanNewPage);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.btnPrint);
			this.Controls.Add(this.cmbPrinter);
			this.Controls.Add(this.label2);
			this.Name = "ScanToLaserJet";
			this.Text = "Scan to LaserJet";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.PicBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.ComboBox cmbPrinter;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnPrint;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.PictureBox PicBox;
		private System.Windows.Forms.Button BtnScanNewPage;
	}
}

