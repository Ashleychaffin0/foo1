namespace ReadCDandDVD {
	partial class ReadCDandDVD {
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
			this.cmbDrives = new System.Windows.Forms.ComboBox();
			this.btnGo = new System.Windows.Forms.Button();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(37, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Drives";
			// 
			// cmbDrives
			// 
			this.cmbDrives.FormattingEnabled = true;
			this.cmbDrives.Location = new System.Drawing.Point(59, 24);
			this.cmbDrives.Name = "cmbDrives";
			this.cmbDrives.Size = new System.Drawing.Size(121, 21);
			this.cmbDrives.TabIndex = 1;
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(239, 24);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 2;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// listBox1
			// 
			this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Location = new System.Drawing.Point(9, 65);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(548, 303);
			this.listBox1.TabIndex = 3;
			// 
			// ReadCDandDVD
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(569, 379);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.cmbDrives);
			this.Controls.Add(this.label1);
			this.Name = "ReadCDandDVD";
			this.Text = "ReadCDandDVD";
			this.Load += new System.EventHandler(this.ReadCDandDVD_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbDrives;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.ListBox listBox1;
	}
}

