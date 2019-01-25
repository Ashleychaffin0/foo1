namespace DumpPdb {
	partial class DumpPdb {
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
			this.TxtPdbFilename = new System.Windows.Forms.TextBox();
			this.BtnBrowsePdb = new System.Windows.Forms.Button();
			this.BtnBrowseOutput = new System.Windows.Forms.Button();
			this.TxtOutputFilename = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.LbMsgs = new System.Windows.Forms.ListBox();
			this.BtnGo = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(35, 28);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(93, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "PDB filename";
			// 
			// TxtPdbFilename
			// 
			this.TxtPdbFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TxtPdbFilename.Location = new System.Drawing.Point(159, 25);
			this.TxtPdbFilename.Name = "TxtPdbFilename";
			this.TxtPdbFilename.Size = new System.Drawing.Size(803, 22);
			this.TxtPdbFilename.TabIndex = 1;
			// 
			// BtnBrowsePdb
			// 
			this.BtnBrowsePdb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnBrowsePdb.Location = new System.Drawing.Point(994, 25);
			this.BtnBrowsePdb.Name = "BtnBrowsePdb";
			this.BtnBrowsePdb.Size = new System.Drawing.Size(75, 23);
			this.BtnBrowsePdb.TabIndex = 2;
			this.BtnBrowsePdb.Text = "Browse";
			this.BtnBrowsePdb.UseVisualStyleBackColor = true;
			this.BtnBrowsePdb.Click += new System.EventHandler(this.BtnBrowsePdb_Click);
			// 
			// BtnBrowseOutput
			// 
			this.BtnBrowseOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnBrowseOutput.Location = new System.Drawing.Point(994, 68);
			this.BtnBrowseOutput.Name = "BtnBrowseOutput";
			this.BtnBrowseOutput.Size = new System.Drawing.Size(75, 23);
			this.BtnBrowseOutput.TabIndex = 5;
			this.BtnBrowseOutput.Text = "Browse";
			this.BtnBrowseOutput.UseVisualStyleBackColor = true;
			this.BtnBrowseOutput.Click += new System.EventHandler(this.BtnBrowseOutput_Click);
			// 
			// TxtOutputFilename
			// 
			this.TxtOutputFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TxtOutputFilename.Location = new System.Drawing.Point(159, 68);
			this.TxtOutputFilename.Name = "TxtOutputFilename";
			this.TxtOutputFilename.Size = new System.Drawing.Size(803, 22);
			this.TxtOutputFilename.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(35, 71);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(108, 17);
			this.label2.TabIndex = 3;
			this.label2.Text = "Output filename";
			// 
			// LbMsgs
			// 
			this.LbMsgs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LbMsgs.FormattingEnabled = true;
			this.LbMsgs.ItemHeight = 16;
			this.LbMsgs.Location = new System.Drawing.Point(38, 150);
			this.LbMsgs.Name = "LbMsgs";
			this.LbMsgs.Size = new System.Drawing.Size(1031, 196);
			this.LbMsgs.TabIndex = 6;
			// 
			// BtnGo
			// 
			this.BtnGo.Location = new System.Drawing.Point(13, 105);
			this.BtnGo.Name = "BtnGo";
			this.BtnGo.Size = new System.Drawing.Size(75, 23);
			this.BtnGo.TabIndex = 7;
			this.BtnGo.Text = "Go";
			this.BtnGo.UseVisualStyleBackColor = true;
			this.BtnGo.Click += new System.EventHandler(this.BtnGo_Click);
			// 
			// DumpPdb
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1098, 364);
			this.Controls.Add(this.BtnGo);
			this.Controls.Add(this.LbMsgs);
			this.Controls.Add(this.BtnBrowseOutput);
			this.Controls.Add(this.TxtOutputFilename);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.BtnBrowsePdb);
			this.Controls.Add(this.TxtPdbFilename);
			this.Controls.Add(this.label1);
			this.Name = "DumpPdb";
			this.Text = "Dump PDB";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox TxtPdbFilename;
		private System.Windows.Forms.Button BtnBrowsePdb;
		private System.Windows.Forms.Button BtnBrowseOutput;
		private System.Windows.Forms.TextBox TxtOutputFilename;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListBox LbMsgs;
		private System.Windows.Forms.Button BtnGo;
	}
}

