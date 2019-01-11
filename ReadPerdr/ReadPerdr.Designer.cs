namespace ReadPerdr {
	partial class ReadPerdr {
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
			this.txtFilename = new System.Windows.Forms.TextBox();
			this.btnBrowseFilename = new System.Windows.Forms.Button();
			this.btnGo = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(22, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(49, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Filename";
			// 
			// txtFilename
			// 
			this.txtFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtFilename.Location = new System.Drawing.Point(98, 19);
			this.txtFilename.Name = "txtFilename";
			this.txtFilename.Size = new System.Drawing.Size(552, 20);
			this.txtFilename.TabIndex = 1;
			// 
			// btnBrowseFilename
			// 
			this.btnBrowseFilename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseFilename.Location = new System.Drawing.Point(670, 17);
			this.btnBrowseFilename.Name = "btnBrowseFilename";
			this.btnBrowseFilename.Size = new System.Drawing.Size(75, 23);
			this.btnBrowseFilename.TabIndex = 2;
			this.btnBrowseFilename.Text = "Browse...";
			this.btnBrowseFilename.UseVisualStyleBackColor = true;
			this.btnBrowseFilename.Click += new System.EventHandler(this.btnBrowseFilename_Click);
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(25, 58);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 3;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// ReadPerdr
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(771, 448);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.btnBrowseFilename);
			this.Controls.Add(this.txtFilename);
			this.Controls.Add(this.label1);
			this.Name = "ReadPerdr";
			this.Text = "Read Perdr";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtFilename;
		private System.Windows.Forms.Button btnBrowseFilename;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
	}
}

