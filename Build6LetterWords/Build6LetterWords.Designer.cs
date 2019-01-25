namespace Build6LetterWords {
	partial class Build6LetterWords {
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
			this.TxtInputFile = new System.Windows.Forms.TextBox();
			this.BtnBrowseInput = new System.Windows.Forms.Button();
			this.BtnGo = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(34, 23);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(54, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "File Name";
			// 
			// TxtInputFile
			// 
			this.TxtInputFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TxtInputFile.Location = new System.Drawing.Point(94, 20);
			this.TxtInputFile.Name = "TxtInputFile";
			this.TxtInputFile.Size = new System.Drawing.Size(421, 20);
			this.TxtInputFile.TabIndex = 1;
			// 
			// BtnBrowseInput
			// 
			this.BtnBrowseInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnBrowseInput.Location = new System.Drawing.Point(541, 18);
			this.BtnBrowseInput.Name = "BtnBrowseInput";
			this.BtnBrowseInput.Size = new System.Drawing.Size(75, 23);
			this.BtnBrowseInput.TabIndex = 2;
			this.BtnBrowseInput.Text = "Browse";
			this.BtnBrowseInput.UseVisualStyleBackColor = true;
			// 
			// BtnGo
			// 
			this.BtnGo.Location = new System.Drawing.Point(37, 66);
			this.BtnGo.Name = "BtnGo";
			this.BtnGo.Size = new System.Drawing.Size(75, 23);
			this.BtnGo.TabIndex = 3;
			this.BtnGo.Text = "Go";
			this.BtnGo.UseVisualStyleBackColor = true;
			this.BtnGo.Click += new System.EventHandler(this.BtnGo_Click);
			// 
			// Build6LetterWords
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(650, 388);
			this.Controls.Add(this.BtnGo);
			this.Controls.Add(this.BtnBrowseInput);
			this.Controls.Add(this.TxtInputFile);
			this.Controls.Add(this.label1);
			this.Name = "Build6LetterWords";
			this.Text = "Build 6 Letter Words";
			this.Load += new System.EventHandler(this.Build6LetterWords_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox TxtInputFile;
		private System.Windows.Forms.Button BtnBrowseInput;
		private System.Windows.Forms.Button BtnGo;
	}
}

