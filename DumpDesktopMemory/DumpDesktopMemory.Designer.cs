namespace DumpDesktopMemory {
	partial class DumpDesktopMemory {
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
			this.btnDump = new System.Windows.Forms.Button();
			this.lbOutput = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtAddress = new System.Windows.Forms.TextBox();
			this.txtLength = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.cmbProcesses = new System.Windows.Forms.ComboBox();
			this.btnRefreshProcesses = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.cmbModules = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// btnDump
			// 
			this.btnDump.Location = new System.Drawing.Point(37, 33);
			this.btnDump.Name = "btnDump";
			this.btnDump.Size = new System.Drawing.Size(75, 23);
			this.btnDump.TabIndex = 0;
			this.btnDump.Text = "Dump";
			this.btnDump.UseVisualStyleBackColor = true;
			this.btnDump.Click += new System.EventHandler(this.btnDump_Click);
			// 
			// lbOutput
			// 
			this.lbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbOutput.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbOutput.FormattingEnabled = true;
			this.lbOutput.HorizontalScrollbar = true;
			this.lbOutput.ItemHeight = 19;
			this.lbOutput.Location = new System.Drawing.Point(37, 124);
			this.lbOutput.Name = "lbOutput";
			this.lbOutput.Size = new System.Drawing.Size(1075, 327);
			this.lbOutput.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(127, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 19);
			this.label1.TabIndex = 2;
			this.label1.Text = "Address";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtAddress
			// 
			this.txtAddress.Location = new System.Drawing.Point(127, 35);
			this.txtAddress.Name = "txtAddress";
			this.txtAddress.Size = new System.Drawing.Size(100, 20);
			this.txtAddress.TabIndex = 3;
			// 
			// txtLength
			// 
			this.txtLength.Location = new System.Drawing.Point(247, 35);
			this.txtLength.Name = "txtLength";
			this.txtLength.Size = new System.Drawing.Size(100, 20);
			this.txtLength.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(247, 14);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 19);
			this.label2.TabIndex = 4;
			this.label2.Text = "Length";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(388, 14);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(724, 19);
			this.label3.TabIndex = 6;
			this.label3.Text = "Process";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cmbProcesses
			// 
			this.cmbProcesses.FormattingEnabled = true;
			this.cmbProcesses.Location = new System.Drawing.Point(391, 35);
			this.cmbProcesses.Name = "cmbProcesses";
			this.cmbProcesses.Size = new System.Drawing.Size(721, 21);
			this.cmbProcesses.TabIndex = 7;
			this.cmbProcesses.SelectedIndexChanged += new System.EventHandler(this.cmbProcesses_SelectedIndexChanged);
			// 
			// btnRefreshProcesses
			// 
			this.btnRefreshProcesses.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRefreshProcesses.Location = new System.Drawing.Point(1037, 12);
			this.btnRefreshProcesses.Name = "btnRefreshProcesses";
			this.btnRefreshProcesses.Size = new System.Drawing.Size(75, 23);
			this.btnRefreshProcesses.TabIndex = 8;
			this.btnRefreshProcesses.Text = "Refresh";
			this.btnRefreshProcesses.UseVisualStyleBackColor = true;
			this.btnRefreshProcesses.Click += new System.EventHandler(this.btnRefreshProcesses_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(247, 75);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 19);
			this.label4.TabIndex = 9;
			this.label4.Text = "Modules";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cmbModules
			// 
			this.cmbModules.FormattingEnabled = true;
			this.cmbModules.Location = new System.Drawing.Point(391, 75);
			this.cmbModules.Name = "cmbModules";
			this.cmbModules.Size = new System.Drawing.Size(721, 21);
			this.cmbModules.TabIndex = 10;
			this.cmbModules.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cmbModules_MouseClick);
			// 
			// DumpDesktopMemory
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1136, 479);
			this.Controls.Add(this.cmbModules);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.btnRefreshProcesses);
			this.Controls.Add(this.cmbProcesses);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtLength);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtAddress);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lbOutput);
			this.Controls.Add(this.btnDump);
			this.Name = "DumpDesktopMemory";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

        #endregion

        private System.Windows.Forms.Button btnDump;
        private System.Windows.Forms.ListBox lbOutput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.TextBox txtLength;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbProcesses;
        private System.Windows.Forms.Button btnRefreshProcesses;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cmbModules;
	}
}

