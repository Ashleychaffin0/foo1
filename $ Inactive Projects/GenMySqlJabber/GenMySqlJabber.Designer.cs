namespace GenMySqlJabber {
	partial class GenMySqlJabber {
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
			this.txtInputFilename = new System.Windows.Forms.TextBox();
			this.btnBrowseInputFilename = new System.Windows.Forms.Button();
			this.btnBrowseOutputFilename = new System.Windows.Forms.Button();
			this.txtOutputFilename = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnGo = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(76, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Input file name";
			// 
			// txtInputFilename
			// 
			this.txtInputFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtInputFilename.Location = new System.Drawing.Point(108, 26);
			this.txtInputFilename.Name = "txtInputFilename";
			this.txtInputFilename.Size = new System.Drawing.Size(297, 20);
			this.txtInputFilename.TabIndex = 1;
			// 
			// btnBrowseInputFilename
			// 
			this.btnBrowseInputFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseInputFilename.Location = new System.Drawing.Point(430, 21);
			this.btnBrowseInputFilename.Name = "btnBrowseInputFilename";
			this.btnBrowseInputFilename.Size = new System.Drawing.Size(75, 23);
			this.btnBrowseInputFilename.TabIndex = 2;
			this.btnBrowseInputFilename.Text = "Browse";
			this.btnBrowseInputFilename.UseVisualStyleBackColor = true;
			this.btnBrowseInputFilename.Click += new System.EventHandler(this.btnBrowseInputFilename_Click);
			// 
			// btnBrowseOutputFilename
			// 
			this.btnBrowseOutputFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseOutputFilename.Location = new System.Drawing.Point(430, 47);
			this.btnBrowseOutputFilename.Name = "btnBrowseOutputFilename";
			this.btnBrowseOutputFilename.Size = new System.Drawing.Size(75, 23);
			this.btnBrowseOutputFilename.TabIndex = 5;
			this.btnBrowseOutputFilename.Text = "Browse";
			this.btnBrowseOutputFilename.UseVisualStyleBackColor = true;
			this.btnBrowseOutputFilename.Click += new System.EventHandler(this.btnBrowseOutputFilename_Click);
			// 
			// txtOutputFilename
			// 
			this.txtOutputFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtOutputFilename.Location = new System.Drawing.Point(108, 52);
			this.txtOutputFilename.Name = "txtOutputFilename";
			this.txtOutputFilename.Size = new System.Drawing.Size(297, 20);
			this.txtOutputFilename.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 52);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(84, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Output file name";
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(231, 94);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 6;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// GenMySqlJabber
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(532, 141);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.btnBrowseOutputFilename);
			this.Controls.Add(this.txtOutputFilename);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnBrowseInputFilename);
			this.Controls.Add(this.txtInputFilename);
			this.Controls.Add(this.label1);
			this.Name = "GenMySqlJabber";
			this.Text = "Generate MySql Jabber Statements";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtInputFilename;
		private System.Windows.Forms.Button btnBrowseInputFilename;
		private System.Windows.Forms.Button btnBrowseOutputFilename;
		private System.Windows.Forms.TextBox txtOutputFilename;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnGo;
	}
}

