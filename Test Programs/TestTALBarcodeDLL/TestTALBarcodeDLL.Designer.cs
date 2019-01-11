namespace TestTALBarcodeDLL {
	partial class TestTALBarcodeDLL {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestTALBarcodeDLL));
			this.txtBarcodeText = new System.Windows.Forms.TextBox();
			this.btnGo = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.btnPrint = new System.Windows.Forms.Button();
			this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
			this.pd = new System.Drawing.Printing.PrintDocument();
			this.btnPrintWindow = new System.Windows.Forms.Button();
			this.lbDecodeBMP = new System.Windows.Forms.ListBox();
			this.lbMsgs = new System.Windows.Forms.ListBox();
			this.panel1 = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtBarcodeText
			// 
			this.txtBarcodeText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtBarcodeText.Location = new System.Drawing.Point(307, 26);
			this.txtBarcodeText.Name = "txtBarcodeText";
			this.txtBarcodeText.Size = new System.Drawing.Size(312, 20);
			this.txtBarcodeText.TabIndex = 0;
			this.txtBarcodeText.Text = "PDF417";
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(12, 23);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 1;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox1.BackColor = System.Drawing.Color.Turquoise;
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pictureBox1.Location = new System.Drawing.Point(3, 10);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(791, 121);
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			// 
			// btnPrint
			// 
			this.btnPrint.Location = new System.Drawing.Point(103, 23);
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(75, 23);
			this.btnPrint.TabIndex = 3;
			this.btnPrint.Text = "Print";
			this.btnPrint.UseVisualStyleBackColor = true;
			this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
			// 
			// printPreviewDialog1
			// 
			this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
			this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
			this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
			this.printPreviewDialog1.Enabled = true;
			this.printPreviewDialog1.Name = "printPreviewDialog1";
			this.printPreviewDialog1.Visible = false;
			// 
			// btnPrintWindow
			// 
			this.btnPrintWindow.Location = new System.Drawing.Point(193, 23);
			this.btnPrintWindow.Name = "btnPrintWindow";
			this.btnPrintWindow.Size = new System.Drawing.Size(96, 23);
			this.btnPrintWindow.TabIndex = 4;
			this.btnPrintWindow.Text = "Print Window";
			this.btnPrintWindow.UseVisualStyleBackColor = true;
			this.btnPrintWindow.Click += new System.EventHandler(this.btnPrintWindow_Click);
			// 
			// lbDecodeBMP
			// 
			this.lbDecodeBMP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lbDecodeBMP.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbDecodeBMP.FormattingEnabled = true;
			this.lbDecodeBMP.HorizontalScrollbar = true;
			this.lbDecodeBMP.ItemHeight = 14;
			this.lbDecodeBMP.Location = new System.Drawing.Point(13, 151);
			this.lbDecodeBMP.Name = "lbDecodeBMP";
			this.lbDecodeBMP.Size = new System.Drawing.Size(606, 60);
			this.lbDecodeBMP.TabIndex = 5;
			// 
			// lbMsgs
			// 
			this.lbMsgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lbMsgs.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbMsgs.FormattingEnabled = true;
			this.lbMsgs.HorizontalScrollbar = true;
			this.lbMsgs.ItemHeight = 14;
			this.lbMsgs.Location = new System.Drawing.Point(13, 61);
			this.lbMsgs.Name = "lbMsgs";
			this.lbMsgs.Size = new System.Drawing.Size(606, 74);
			this.lbMsgs.TabIndex = 6;
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.AutoSize = true;
			this.panel1.BackColor = System.Drawing.Color.Red;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.pictureBox1);
			this.panel1.Location = new System.Drawing.Point(26, 217);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(799, 578);
			this.panel1.TabIndex = 7;
			this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
			// 
			// TestTALBarcodeDLL
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(837, 807);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.lbMsgs);
			this.Controls.Add(this.lbDecodeBMP);
			this.Controls.Add(this.btnPrintWindow);
			this.Controls.Add(this.btnPrint);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.txtBarcodeText);
			this.Name = "TestTALBarcodeDLL";
			this.Text = "TestTALBarcodeDLL";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtBarcodeText;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button btnPrint;
		private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
		private System.Drawing.Printing.PrintDocument pd;
		private System.Windows.Forms.Button btnPrintWindow;
		private System.Windows.Forms.ListBox lbDecodeBMP;
		private System.Windows.Forms.ListBox lbMsgs;
		private System.Windows.Forms.Panel panel1;
	}
}

