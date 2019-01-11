namespace ReadOPML {
	partial class Form1 {
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
			this.txtOPMLFilename = new System.Windows.Forms.TextBox();
			this.btnBrowseOPML = new System.Windows.Forms.Button();
			this.btnGo = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "OPML filename";
			// 
			// txtOPMLFilename
			// 
			this.txtOPMLFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtOPMLFilename.Location = new System.Drawing.Point(123, 24);
			this.txtOPMLFilename.Name = "txtOPMLFilename";
			this.txtOPMLFilename.Size = new System.Drawing.Size(363, 22);
			this.txtOPMLFilename.TabIndex = 1;
			this.txtOPMLFilename.Text = "C:\\Documents and Settings\\larrys\\My Documents\\RSS Bandit - 2005-12-16.opml";
			// 
			// btnBrowseOPML
			// 
			this.btnBrowseOPML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseOPML.Location = new System.Drawing.Point(507, 24);
			this.btnBrowseOPML.Name = "btnBrowseOPML";
			this.btnBrowseOPML.Size = new System.Drawing.Size(75, 23);
			this.btnBrowseOPML.TabIndex = 2;
			this.btnBrowseOPML.Text = "Browse";
			this.btnBrowseOPML.UseVisualStyleBackColor = true;
			this.btnBrowseOPML.Click += new System.EventHandler(this.btnBrowseOPML_Click);
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(16, 64);
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
			// listBox1
			// 
			this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.listBox1.FormattingEnabled = true;
			this.listBox1.ItemHeight = 16;
			this.listBox1.Location = new System.Drawing.Point(16, 109);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(565, 132);
			this.listBox1.TabIndex = 4;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(594, 260);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.btnBrowseOPML);
			this.Controls.Add(this.txtOPMLFilename);
			this.Controls.Add(this.label1);
			this.Name = "Form1";
			this.Text = "LRS OPML Testing";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtOPMLFilename;
		private System.Windows.Forms.Button btnBrowseOPML;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.ListBox listBox1;
	}
}

