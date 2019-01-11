namespace TestBarcodes {
	partial class TestBarcodes {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestBarcodes));
			this.label1 = new System.Windows.Forms.Label();
			this.txtOriginal = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnFillWithRandomData = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.txtRandomDataSize = new System.Windows.Forms.TextBox();
			this.txtScanned = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.btnFillFromFile = new System.Windows.Forms.Button();
			this.btnFilenameBrowse = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.txtFilename = new System.Windows.Forms.TextBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.btnPrintPreview = new System.Windows.Forms.Button();
			this.btnPrint = new System.Windows.Forms.Button();
			this.txtShrinkFactor = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.txtBarcodeHeight = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txtBarcodeWidth = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.lblOriginalMatchesScanned = new System.Windows.Forms.Label();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.btnClearOriginalText = new System.Windows.Forms.Button();
			this.btnClearScannedText = new System.Windows.Forms.Button();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.txtScannedHeight = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.txtScannedWidth = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.txtComments = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.btnOpenDatabase = new System.Windows.Forms.Button();
			this.cmbScanningEase = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.btnLogToDatabase = new System.Windows.Forms.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.cmbPrinters = new System.Windows.Forms.ComboBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.lblPrinterCustomHeight = new System.Windows.Forms.Label();
			this.txtPrinterCustomHeight = new System.Windows.Forms.TextBox();
			this.lblPrinterCustomWidth = new System.Windows.Forms.Label();
			this.txtPrinterCustomWidth = new System.Windows.Forms.TextBox();
			this.cmbPrinterResolution = new System.Windows.Forms.ComboBox();
			this.label11 = new System.Windows.Forms.Label();
			this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(230, 180);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(418, 17);
			this.label1.TabIndex = 3;
			this.label1.Text = "Original Text";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtOriginal
			// 
			this.txtOriginal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtOriginal.Location = new System.Drawing.Point(230, 200);
			this.txtOriginal.Multiline = true;
			this.txtOriginal.Name = "txtOriginal";
			this.txtOriginal.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtOriginal.Size = new System.Drawing.Size(418, 136);
			this.txtOriginal.TabIndex = 4;
			this.txtOriginal.TextChanged += new System.EventHandler(this.txtOriginal_TextChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnFillWithRandomData);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtRandomDataSize);
			this.groupBox1.Location = new System.Drawing.Point(13, 192);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(201, 100);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Fill with random data";
			// 
			// btnFillWithRandomData
			// 
			this.btnFillWithRandomData.Location = new System.Drawing.Point(46, 71);
			this.btnFillWithRandomData.Name = "btnFillWithRandomData";
			this.btnFillWithRandomData.Size = new System.Drawing.Size(75, 23);
			this.btnFillWithRandomData.TabIndex = 1;
			this.btnFillWithRandomData.Text = "Go";
			this.btnFillWithRandomData.UseVisualStyleBackColor = true;
			this.btnFillWithRandomData.Click += new System.EventHandler(this.btnFillWithRandomData_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(17, 37);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(35, 17);
			this.label3.TabIndex = 1;
			this.label3.Text = "Size";
			// 
			// txtRandomDataSize
			// 
			this.txtRandomDataSize.Location = new System.Drawing.Point(86, 34);
			this.txtRandomDataSize.Name = "txtRandomDataSize";
			this.txtRandomDataSize.Size = new System.Drawing.Size(54, 22);
			this.txtRandomDataSize.TabIndex = 0;
			this.txtRandomDataSize.Text = "123";
			this.toolTip1.SetToolTip(this.txtRandomDataSize, "Number of characters to generate (may have problems if more than 1024)");
			this.txtRandomDataSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRandomDataSize_KeyPress);
			// 
			// txtScanned
			// 
			this.txtScanned.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtScanned.Location = new System.Drawing.Point(230, 370);
			this.txtScanned.Multiline = true;
			this.txtScanned.Name = "txtScanned";
			this.txtScanned.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtScanned.Size = new System.Drawing.Size(418, 136);
			this.txtScanned.TabIndex = 6;
			this.txtScanned.TextChanged += new System.EventHandler(this.txtScanned_TextChanged);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Location = new System.Drawing.Point(230, 350);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(418, 17);
			this.label2.TabIndex = 6;
			this.label2.Text = "Scanned Text";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.btnFillFromFile);
			this.groupBox2.Controls.Add(this.btnFilenameBrowse);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.txtFilename);
			this.groupBox2.Location = new System.Drawing.Point(13, 620);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(629, 98);
			this.groupBox2.TabIndex = 8;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Fill Original Text from file";
			// 
			// btnFillFromFile
			// 
			this.btnFillFromFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFillFromFile.Location = new System.Drawing.Point(452, 22);
			this.btnFillFromFile.Name = "btnFillFromFile";
			this.btnFillFromFile.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.btnFillFromFile.Size = new System.Drawing.Size(75, 23);
			this.btnFillFromFile.TabIndex = 0;
			this.btnFillFromFile.Text = "Reload";
			this.toolTip1.SetToolTip(this.btnFillFromFile, "Reloads the specified file, and copies it to the Original Text box");
			this.btnFillFromFile.UseVisualStyleBackColor = true;
			this.btnFillFromFile.Click += new System.EventHandler(this.btnFillFromFile_Click);
			// 
			// btnFilenameBrowse
			// 
			this.btnFilenameBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFilenameBrowse.Location = new System.Drawing.Point(548, 22);
			this.btnFilenameBrowse.Name = "btnFilenameBrowse";
			this.btnFilenameBrowse.Size = new System.Drawing.Size(75, 23);
			this.btnFilenameBrowse.TabIndex = 1;
			this.btnFilenameBrowse.Text = "Browse";
			this.toolTip1.SetToolTip(this.btnFilenameBrowse, "Prompts for a filename, and copies it to the Original Text box");
			this.btnFilenameBrowse.UseVisualStyleBackColor = true;
			this.btnFilenameBrowse.Click += new System.EventHandler(this.btnFilenameBrowse_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(17, 25);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(65, 17);
			this.label4.TabIndex = 1;
			this.label4.Text = "Filename";
			// 
			// txtFilename
			// 
			this.txtFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFilename.Location = new System.Drawing.Point(20, 58);
			this.txtFilename.Name = "txtFilename";
			this.txtFilename.Size = new System.Drawing.Size(603, 22);
			this.txtFilename.TabIndex = 2;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.btnPrintPreview);
			this.groupBox3.Controls.Add(this.btnPrint);
			this.groupBox3.Controls.Add(this.txtShrinkFactor);
			this.groupBox3.Controls.Add(this.label13);
			this.groupBox3.Controls.Add(this.txtBarcodeHeight);
			this.groupBox3.Controls.Add(this.label6);
			this.groupBox3.Controls.Add(this.txtBarcodeWidth);
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.Location = new System.Drawing.Point(13, 12);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(200, 156);
			this.groupBox3.TabIndex = 0;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Barcode Area";
			// 
			// btnPrintPreview
			// 
			this.btnPrintPreview.Location = new System.Drawing.Point(111, 127);
			this.btnPrintPreview.Name = "btnPrintPreview";
			this.btnPrintPreview.Size = new System.Drawing.Size(75, 23);
			this.btnPrintPreview.TabIndex = 7;
			this.btnPrintPreview.Text = "Preview";
			this.btnPrintPreview.UseVisualStyleBackColor = true;
			this.btnPrintPreview.Click += new System.EventHandler(this.btnPrintPreview_Click);
			// 
			// btnPrint
			// 
			this.btnPrint.Location = new System.Drawing.Point(20, 127);
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(75, 23);
			this.btnPrint.TabIndex = 2;
			this.btnPrint.Text = "Print";
			this.btnPrint.UseVisualStyleBackColor = true;
			this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
			// 
			// txtShrinkFactor
			// 
			this.txtShrinkFactor.Location = new System.Drawing.Point(111, 88);
			this.txtShrinkFactor.Name = "txtShrinkFactor";
			this.txtShrinkFactor.Size = new System.Drawing.Size(54, 22);
			this.txtShrinkFactor.TabIndex = 5;
			this.txtShrinkFactor.Text = "10.0";
			this.toolTip1.SetToolTip(this.txtShrinkFactor, "Percentage to shrink height. So 10.0 here is 10% and will make heights 90%, .81%," +
					" etc.");
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(19, 93);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(88, 17);
			this.label13.TabIndex = 6;
			this.label13.Text = "Shrink factor";
			// 
			// txtBarcodeHeight
			// 
			this.txtBarcodeHeight.Location = new System.Drawing.Point(111, 57);
			this.txtBarcodeHeight.Name = "txtBarcodeHeight";
			this.txtBarcodeHeight.Size = new System.Drawing.Size(54, 22);
			this.txtBarcodeHeight.TabIndex = 1;
			this.txtBarcodeHeight.Text = "1";
			this.toolTip1.SetToolTip(this.txtBarcodeHeight, "Height in inches. May include a decimal point (e.g. 3.2)");
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(19, 62);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(49, 17);
			this.label6.TabIndex = 4;
			this.label6.Text = "Height";
			// 
			// txtBarcodeWidth
			// 
			this.txtBarcodeWidth.Location = new System.Drawing.Point(111, 29);
			this.txtBarcodeWidth.Name = "txtBarcodeWidth";
			this.txtBarcodeWidth.Size = new System.Drawing.Size(54, 22);
			this.txtBarcodeWidth.TabIndex = 0;
			this.txtBarcodeWidth.Text = "3";
			this.toolTip1.SetToolTip(this.txtBarcodeWidth, "Width in inches. May include a decimal point (e.g. 1.5)");
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(19, 34);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(44, 17);
			this.label5.TabIndex = 0;
			this.label5.Text = "Width";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(35, 370);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(178, 17);
			this.label7.TabIndex = 7;
			this.label7.Text = "Original matches Scanned:";
			// 
			// lblOriginalMatchesScanned
			// 
			this.lblOriginalMatchesScanned.Location = new System.Drawing.Point(35, 397);
			this.lblOriginalMatchesScanned.Name = "lblOriginalMatchesScanned";
			this.lblOriginalMatchesScanned.Size = new System.Drawing.Size(179, 20);
			this.lblOriginalMatchesScanned.TabIndex = 8;
			this.lblOriginalMatchesScanned.Text = "False";
			this.lblOriginalMatchesScanned.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnClearOriginalText
			// 
			this.btnClearOriginalText.Location = new System.Drawing.Point(139, 313);
			this.btnClearOriginalText.Name = "btnClearOriginalText";
			this.btnClearOriginalText.Size = new System.Drawing.Size(75, 23);
			this.btnClearOriginalText.TabIndex = 3;
			this.btnClearOriginalText.Text = "Clear";
			this.toolTip1.SetToolTip(this.btnClearOriginalText, "Clear the contents of the Original Text box");
			this.btnClearOriginalText.UseVisualStyleBackColor = true;
			this.btnClearOriginalText.Click += new System.EventHandler(this.btnClearOriginalText_Click);
			// 
			// btnClearScannedText
			// 
			this.btnClearScannedText.Location = new System.Drawing.Point(149, 483);
			this.btnClearScannedText.Name = "btnClearScannedText";
			this.btnClearScannedText.Size = new System.Drawing.Size(75, 23);
			this.btnClearScannedText.TabIndex = 5;
			this.btnClearScannedText.Text = "Clear";
			this.toolTip1.SetToolTip(this.btnClearScannedText, "Clear the contents of the Scanned Text box");
			this.btnClearScannedText.UseVisualStyleBackColor = true;
			this.btnClearScannedText.Click += new System.EventHandler(this.btnClearScannedText_Click);
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.txtScannedHeight);
			this.groupBox4.Controls.Add(this.label14);
			this.groupBox4.Controls.Add(this.txtScannedWidth);
			this.groupBox4.Controls.Add(this.label12);
			this.groupBox4.Controls.Add(this.txtComments);
			this.groupBox4.Controls.Add(this.label9);
			this.groupBox4.Controls.Add(this.btnOpenDatabase);
			this.groupBox4.Controls.Add(this.cmbScanningEase);
			this.groupBox4.Controls.Add(this.label8);
			this.groupBox4.Controls.Add(this.btnLogToDatabase);
			this.groupBox4.Location = new System.Drawing.Point(228, 12);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(420, 156);
			this.groupBox4.TabIndex = 2;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Logging";
			// 
			// txtScannedHeight
			// 
			this.txtScannedHeight.Location = new System.Drawing.Point(220, 49);
			this.txtScannedHeight.Name = "txtScannedHeight";
			this.txtScannedHeight.Size = new System.Drawing.Size(54, 22);
			this.txtScannedHeight.TabIndex = 10;
			this.txtScannedHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.toolTip1.SetToolTip(this.txtScannedHeight, "Height in inches of the scanned barcode. May include a decimal point (e.g. 1.5).");
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(217, 29);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(57, 17);
			this.label14.TabIndex = 9;
			this.label14.Text = "Height";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtScannedWidth
			// 
			this.txtScannedWidth.Location = new System.Drawing.Point(146, 49);
			this.txtScannedWidth.Name = "txtScannedWidth";
			this.txtScannedWidth.Size = new System.Drawing.Size(54, 22);
			this.txtScannedWidth.TabIndex = 8;
			this.txtScannedWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.toolTip1.SetToolTip(this.txtScannedWidth, "Width in inches of the scanned barcode. May include a decimal point (e.g. 1.5).\r\n" +
					"");
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(143, 29);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(57, 17);
			this.label12.TabIndex = 6;
			this.label12.Text = "Width";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtComments
			// 
			this.txtComments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtComments.Location = new System.Drawing.Point(86, 93);
			this.txtComments.Multiline = true;
			this.txtComments.Name = "txtComments";
			this.txtComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtComments.Size = new System.Drawing.Size(318, 57);
			this.txtComments.TabIndex = 1;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(6, 93);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(74, 17);
			this.label9.TabIndex = 5;
			this.label9.Text = "Comments";
			// 
			// btnOpenDatabase
			// 
			this.btnOpenDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOpenDatabase.Location = new System.Drawing.Point(309, 20);
			this.btnOpenDatabase.Name = "btnOpenDatabase";
			this.btnOpenDatabase.Size = new System.Drawing.Size(95, 26);
			this.btnOpenDatabase.TabIndex = 3;
			this.btnOpenDatabase.Text = "Open DB";
			this.btnOpenDatabase.UseVisualStyleBackColor = true;
			this.btnOpenDatabase.Click += new System.EventHandler(this.btnOpenDatabase_Click);
			// 
			// cmbScanningEase
			// 
			this.cmbScanningEase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbScanningEase.FormattingEnabled = true;
			this.cmbScanningEase.Items.AddRange(new object[] {
            "Wouldn\'t scan",
            "Very easy",
            "Easy",
            "Eventually"});
			this.cmbScanningEase.Location = new System.Drawing.Point(6, 49);
			this.cmbScanningEase.Name = "cmbScanningEase";
			this.cmbScanningEase.Size = new System.Drawing.Size(121, 24);
			this.cmbScanningEase.TabIndex = 0;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(6, 29);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(121, 17);
			this.label8.TabIndex = 3;
			this.label8.Text = "Scanning ease";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnLogToDatabase
			// 
			this.btnLogToDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnLogToDatabase.Location = new System.Drawing.Point(307, 57);
			this.btnLogToDatabase.Name = "btnLogToDatabase";
			this.btnLogToDatabase.Size = new System.Drawing.Size(95, 26);
			this.btnLogToDatabase.TabIndex = 2;
			this.btnLogToDatabase.Text = "Log to DB";
			this.btnLogToDatabase.UseVisualStyleBackColor = true;
			this.btnLogToDatabase.Click += new System.EventHandler(this.btnLogToDatabase_Click);
			// 
			// label10
			// 
			this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label10.Location = new System.Drawing.Point(21, 18);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(345, 17);
			this.label10.TabIndex = 3;
			this.label10.Text = "Printer";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cmbPrinters
			// 
			this.cmbPrinters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cmbPrinters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbPrinters.FormattingEnabled = true;
			this.cmbPrinters.Location = new System.Drawing.Point(20, 43);
			this.cmbPrinters.Name = "cmbPrinters";
			this.cmbPrinters.Size = new System.Drawing.Size(346, 24);
			this.cmbPrinters.TabIndex = 0;
			this.cmbPrinters.SelectedIndexChanged += new System.EventHandler(this.cmbPrinters_SelectedIndexChanged);
			// 
			// groupBox5
			// 
			this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox5.Controls.Add(this.lblPrinterCustomHeight);
			this.groupBox5.Controls.Add(this.txtPrinterCustomHeight);
			this.groupBox5.Controls.Add(this.lblPrinterCustomWidth);
			this.groupBox5.Controls.Add(this.txtPrinterCustomWidth);
			this.groupBox5.Controls.Add(this.cmbPrinterResolution);
			this.groupBox5.Controls.Add(this.label11);
			this.groupBox5.Controls.Add(this.cmbPrinters);
			this.groupBox5.Controls.Add(this.label10);
			this.groupBox5.Location = new System.Drawing.Point(13, 524);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(629, 79);
			this.groupBox5.TabIndex = 7;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Printer Area";
			// 
			// lblPrinterCustomHeight
			// 
			this.lblPrinterCustomHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblPrinterCustomHeight.Location = new System.Drawing.Point(572, 18);
			this.lblPrinterCustomHeight.Name = "lblPrinterCustomHeight";
			this.lblPrinterCustomHeight.Size = new System.Drawing.Size(46, 23);
			this.lblPrinterCustomHeight.TabIndex = 14;
			this.lblPrinterCustomHeight.Text = "Y";
			this.lblPrinterCustomHeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtPrinterCustomHeight
			// 
			this.txtPrinterCustomHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txtPrinterCustomHeight.Location = new System.Drawing.Point(572, 43);
			this.txtPrinterCustomHeight.Name = "txtPrinterCustomHeight";
			this.txtPrinterCustomHeight.Size = new System.Drawing.Size(46, 22);
			this.txtPrinterCustomHeight.TabIndex = 3;
			// 
			// lblPrinterCustomWidth
			// 
			this.lblPrinterCustomWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblPrinterCustomWidth.Location = new System.Drawing.Point(508, 18);
			this.lblPrinterCustomWidth.Name = "lblPrinterCustomWidth";
			this.lblPrinterCustomWidth.Size = new System.Drawing.Size(46, 23);
			this.lblPrinterCustomWidth.TabIndex = 12;
			this.lblPrinterCustomWidth.Text = "X";
			this.lblPrinterCustomWidth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtPrinterCustomWidth
			// 
			this.txtPrinterCustomWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txtPrinterCustomWidth.Location = new System.Drawing.Point(508, 43);
			this.txtPrinterCustomWidth.Name = "txtPrinterCustomWidth";
			this.txtPrinterCustomWidth.Size = new System.Drawing.Size(46, 22);
			this.txtPrinterCustomWidth.TabIndex = 2;
			// 
			// cmbPrinterResolution
			// 
			this.cmbPrinterResolution.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbPrinterResolution.FormattingEnabled = true;
			this.cmbPrinterResolution.Location = new System.Drawing.Point(388, 43);
			this.cmbPrinterResolution.Name = "cmbPrinterResolution";
			this.cmbPrinterResolution.Size = new System.Drawing.Size(80, 24);
			this.cmbPrinterResolution.TabIndex = 1;
			this.cmbPrinterResolution.SelectedIndexChanged += new System.EventHandler(this.cmbPrinterResolution_SelectedIndexChanged);
			// 
			// label11
			// 
			this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label11.Location = new System.Drawing.Point(388, 18);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(80, 17);
			this.label11.TabIndex = 8;
			this.label11.Text = "Resolution";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
			// TestBarcodes
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(660, 730);
			this.Controls.Add(this.groupBox5);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.btnClearScannedText);
			this.Controls.Add(this.btnClearOriginalText);
			this.Controls.Add(this.lblOriginalMatchesScanned);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.txtScanned);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.txtOriginal);
			this.Controls.Add(this.label1);
			this.Name = "TestBarcodes";
			this.Text = "Test Barcodes";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtOriginal;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtScanned;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnFillWithRandomData;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtRandomDataSize;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button btnFilenameBrowse;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtFilename;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button btnPrint;
		private System.Windows.Forms.TextBox txtBarcodeHeight;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtBarcodeWidth;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label lblOriginalMatchesScanned;
		private System.Windows.Forms.Button btnFillFromFile;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Button btnClearOriginalText;
		private System.Windows.Forms.Button btnClearScannedText;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Button btnLogToDatabase;
		private System.Windows.Forms.ComboBox cmbScanningEase;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button btnOpenDatabase;
		private System.Windows.Forms.TextBox txtComments;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.ComboBox cmbPrinters;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.ComboBox cmbPrinterResolution;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label lblPrinterCustomHeight;
		private System.Windows.Forms.TextBox txtPrinterCustomHeight;
		private System.Windows.Forms.Label lblPrinterCustomWidth;
		private System.Windows.Forms.TextBox txtPrinterCustomWidth;
		private System.Windows.Forms.TextBox txtShrinkFactor;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Button btnPrintPreview;
		private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
		private System.Windows.Forms.TextBox txtScannedHeight;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TextBox txtScannedWidth;
		private System.Windows.Forms.Label label12;
	}
}

