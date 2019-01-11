namespace RestoreFlattenedSciAm {
	partial class FlattenAndOrRestoreSciAm {
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
			this.label2 = new System.Windows.Forms.Label();
			this.txtSourceDir = new System.Windows.Forms.TextBox();
			this.btnBrowseSourceDir = new System.Windows.Forms.Button();
			this.btnBrowseTargetDir = new System.Windows.Forms.Button();
			this.txtTargetDir = new System.Windows.Forms.TextBox();
			this.radFlatten = new System.Windows.Forms.RadioButton();
			this.radRestore = new System.Windows.Forms.RadioButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnGo = new System.Windows.Forms.Button();
			this.lblOperation = new System.Windows.Forms.Label();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.progressBar2 = new System.Windows.Forms.ProgressBar();
			this.lblYear = new System.Windows.Forms.Label();
			this.lblMonth = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(28, 27);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(114, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Source Directory";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(28, 63);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(111, 17);
			this.label2.TabIndex = 1;
			this.label2.Text = "Target Directory";
			// 
			// txtSourceDir
			// 
			this.txtSourceDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSourceDir.Location = new System.Drawing.Point(181, 27);
			this.txtSourceDir.Name = "txtSourceDir";
			this.txtSourceDir.Size = new System.Drawing.Size(254, 22);
			this.txtSourceDir.TabIndex = 2;
			// 
			// btnBrowseSourceDir
			// 
			this.btnBrowseSourceDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseSourceDir.Location = new System.Drawing.Point(459, 24);
			this.btnBrowseSourceDir.Name = "btnBrowseSourceDir";
			this.btnBrowseSourceDir.Size = new System.Drawing.Size(75, 23);
			this.btnBrowseSourceDir.TabIndex = 3;
			this.btnBrowseSourceDir.Text = "Browse";
			this.btnBrowseSourceDir.UseVisualStyleBackColor = true;
			this.btnBrowseSourceDir.Click += new System.EventHandler(this.btnBrowseSourceDir_Click);
			// 
			// btnBrowseTargetDir
			// 
			this.btnBrowseTargetDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseTargetDir.Location = new System.Drawing.Point(459, 60);
			this.btnBrowseTargetDir.Name = "btnBrowseTargetDir";
			this.btnBrowseTargetDir.Size = new System.Drawing.Size(75, 23);
			this.btnBrowseTargetDir.TabIndex = 5;
			this.btnBrowseTargetDir.Text = "Browse";
			this.btnBrowseTargetDir.UseVisualStyleBackColor = true;
			this.btnBrowseTargetDir.Click += new System.EventHandler(this.btnBrowseTargetDir_Click);
			// 
			// txtTargetDir
			// 
			this.txtTargetDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtTargetDir.Location = new System.Drawing.Point(181, 63);
			this.txtTargetDir.Name = "txtTargetDir";
			this.txtTargetDir.Size = new System.Drawing.Size(254, 22);
			this.txtTargetDir.TabIndex = 4;
			// 
			// radFlatten
			// 
			this.radFlatten.AutoSize = true;
			this.radFlatten.Location = new System.Drawing.Point(16, 30);
			this.radFlatten.Name = "radFlatten";
			this.radFlatten.Size = new System.Drawing.Size(72, 21);
			this.radFlatten.TabIndex = 6;
			this.radFlatten.TabStop = true;
			this.radFlatten.Text = "Flatten";
			this.radFlatten.UseVisualStyleBackColor = true;
			this.radFlatten.CheckedChanged += new System.EventHandler(this.radFlatten_CheckedChanged);
			// 
			// radRestore
			// 
			this.radRestore.AutoSize = true;
			this.radRestore.Location = new System.Drawing.Point(146, 30);
			this.radRestore.Name = "radRestore";
			this.radRestore.Size = new System.Drawing.Size(79, 21);
			this.radRestore.TabIndex = 7;
			this.radRestore.TabStop = true;
			this.radRestore.Text = "Restore";
			this.radRestore.UseVisualStyleBackColor = true;
			this.radRestore.CheckedChanged += new System.EventHandler(this.radRestore_CheckedChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radRestore);
			this.groupBox1.Controls.Add(this.radFlatten);
			this.groupBox1.Location = new System.Drawing.Point(31, 102);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(243, 67);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Operation";
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(31, 184);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 9;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// lblOperation
			// 
			this.lblOperation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblOperation.Location = new System.Drawing.Point(313, 111);
			this.lblOperation.Name = "lblOperation";
			this.lblOperation.Size = new System.Drawing.Size(220, 95);
			this.lblOperation.TabIndex = 10;
			// 
			// progressBar1
			// 
			this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar1.Location = new System.Drawing.Point(134, 222);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(399, 23);
			this.progressBar1.TabIndex = 11;
			// 
			// progressBar2
			// 
			this.progressBar2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar2.Location = new System.Drawing.Point(134, 257);
			this.progressBar2.Name = "progressBar2";
			this.progressBar2.Size = new System.Drawing.Size(400, 23);
			this.progressBar2.TabIndex = 12;
			// 
			// lblYear
			// 
			this.lblYear.AutoSize = true;
			this.lblYear.Location = new System.Drawing.Point(28, 228);
			this.lblYear.Name = "lblYear";
			this.lblYear.Size = new System.Drawing.Size(38, 17);
			this.lblYear.TabIndex = 13;
			this.lblYear.Text = "Year";
			// 
			// lblMonth
			// 
			this.lblMonth.AutoSize = true;
			this.lblMonth.Location = new System.Drawing.Point(28, 257);
			this.lblMonth.Name = "lblMonth";
			this.lblMonth.Size = new System.Drawing.Size(47, 17);
			this.lblMonth.TabIndex = 14;
			this.lblMonth.Text = "Month";
			// 
			// FlattenAndOrRestoreSciAm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(546, 292);
			this.Controls.Add(this.lblMonth);
			this.Controls.Add(this.lblYear);
			this.Controls.Add(this.progressBar2);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.lblOperation);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnBrowseTargetDir);
			this.Controls.Add(this.txtTargetDir);
			this.Controls.Add(this.btnBrowseSourceDir);
			this.Controls.Add(this.txtSourceDir);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "FlattenAndOrRestoreSciAm";
			this.Text = "Flatten and/or Restore Scientific American Directories";
			this.Load += new System.EventHandler(this.FlattenAndOrRestoreSciAm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtSourceDir;
		private System.Windows.Forms.Button btnBrowseSourceDir;
		private System.Windows.Forms.Button btnBrowseTargetDir;
		private System.Windows.Forms.TextBox txtTargetDir;
		private System.Windows.Forms.RadioButton radFlatten;
		private System.Windows.Forms.RadioButton radRestore;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.Label lblOperation;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.ProgressBar progressBar2;
		private System.Windows.Forms.Label lblYear;
		private System.Windows.Forms.Label lblMonth;
	}
}

