namespace Pretext_NET_1 {
	partial class Pretext_NET {
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
			this.btnBrowseInput = new System.Windows.Forms.Button();
			this.btnBrowseOutput = new System.Windows.Forms.Button();
			this.grpInput = new System.Windows.Forms.GroupBox();
			this.txtInput = new System.Windows.Forms.TextBox();
			this.txtOutput = new System.Windows.Forms.TextBox();
			this.grpOutput = new System.Windows.Forms.GroupBox();
			this.grpProgress = new System.Windows.Forms.GroupBox();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.btnQuit = new System.Windows.Forms.Button();
			this.btnAboutPretext = new System.Windows.Forms.Button();
			this.btnProcessFile = new System.Windows.Forms.Button();
			this.btnAuto = new System.Windows.Forms.Button();
			this.grpInput.SuspendLayout();
			this.grpOutput.SuspendLayout();
			this.grpProgress.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnBrowseInput
			// 
			this.btnBrowseInput.Location = new System.Drawing.Point(99, 111);
			this.btnBrowseInput.Name = "btnBrowseInput";
			this.btnBrowseInput.Size = new System.Drawing.Size(223, 37);
			this.btnBrowseInput.TabIndex = 0;
			this.btnBrowseInput.Text = "Browse for Input JPEG/PNM";
			this.btnBrowseInput.UseVisualStyleBackColor = true;
			// 
			// btnBrowseOutput
			// 
			this.btnBrowseOutput.Location = new System.Drawing.Point(402, 111);
			this.btnBrowseOutput.Name = "btnBrowseOutput";
			this.btnBrowseOutput.Size = new System.Drawing.Size(223, 37);
			this.btnBrowseOutput.TabIndex = 1;
			this.btnBrowseOutput.Text = "Browse for Output Folder";
			this.btnBrowseOutput.UseVisualStyleBackColor = true;
			// 
			// grpInput
			// 
			this.grpInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpInput.Controls.Add(this.txtInput);
			this.grpInput.Location = new System.Drawing.Point(17, 169);
			this.grpInput.Name = "grpInput";
			this.grpInput.Size = new System.Drawing.Size(731, 61);
			this.grpInput.TabIndex = 2;
			this.grpInput.TabStop = false;
			this.grpInput.Text = "Input JPG/JPEG/PNM";
			// 
			// txtInput
			// 
			this.txtInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtInput.Location = new System.Drawing.Point(19, 24);
			this.txtInput.Name = "txtInput";
			this.txtInput.Size = new System.Drawing.Size(695, 22);
			this.txtInput.TabIndex = 0;
			// 
			// txtOutput
			// 
			this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtOutput.Location = new System.Drawing.Point(19, 24);
			this.txtOutput.Name = "txtOutput";
			this.txtOutput.Size = new System.Drawing.Size(695, 22);
			this.txtOutput.TabIndex = 0;
			// 
			// grpOutput
			// 
			this.grpOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpOutput.Controls.Add(this.txtOutput);
			this.grpOutput.Location = new System.Drawing.Point(17, 246);
			this.grpOutput.Name = "grpOutput";
			this.grpOutput.Size = new System.Drawing.Size(731, 61);
			this.grpOutput.TabIndex = 3;
			this.grpOutput.TabStop = false;
			this.grpOutput.Text = "Output Folder";
			// 
			// grpProgress
			// 
			this.grpProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpProgress.Controls.Add(this.progressBar1);
			this.grpProgress.Location = new System.Drawing.Point(17, 321);
			this.grpProgress.Name = "grpProgress";
			this.grpProgress.Size = new System.Drawing.Size(731, 61);
			this.grpProgress.TabIndex = 4;
			this.grpProgress.TabStop = false;
			this.grpProgress.Text = "Progress";
			// 
			// progressBar1
			// 
			this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar1.Location = new System.Drawing.Point(19, 22);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(695, 23);
			this.progressBar1.TabIndex = 0;
			// 
			// btnQuit
			// 
			this.btnQuit.Location = new System.Drawing.Point(525, 406);
			this.btnQuit.Name = "btnQuit";
			this.btnQuit.Size = new System.Drawing.Size(158, 37);
			this.btnQuit.TabIndex = 5;
			this.btnQuit.Text = "Quit";
			this.btnQuit.UseVisualStyleBackColor = true;
			// 
			// btnAboutPretext
			// 
			this.btnAboutPretext.Location = new System.Drawing.Point(267, 406);
			this.btnAboutPretext.Name = "btnAboutPretext";
			this.btnAboutPretext.Size = new System.Drawing.Size(158, 37);
			this.btnAboutPretext.TabIndex = 6;
			this.btnAboutPretext.Text = "About Pretext";
			this.btnAboutPretext.UseVisualStyleBackColor = true;
			// 
			// btnProcessFile
			// 
			this.btnProcessFile.Location = new System.Drawing.Point(17, 406);
			this.btnProcessFile.Name = "btnProcessFile";
			this.btnProcessFile.Size = new System.Drawing.Size(158, 37);
			this.btnProcessFile.TabIndex = 7;
			this.btnProcessFile.Text = "Process File";
			this.btnProcessFile.UseVisualStyleBackColor = true;
			// 
			// btnAuto
			// 
			this.btnAuto.BackColor = System.Drawing.Color.LightGreen;
			this.btnAuto.Location = new System.Drawing.Point(17, 12);
			this.btnAuto.Name = "btnAuto";
			this.btnAuto.Size = new System.Drawing.Size(53, 37);
			this.btnAuto.TabIndex = 8;
			this.btnAuto.Text = "Auto";
			this.btnAuto.UseVisualStyleBackColor = false;
			this.btnAuto.Click += new System.EventHandler(this.btnAuto_Click);
			// 
			// Pretext_NET
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(771, 463);
			this.Controls.Add(this.btnAuto);
			this.Controls.Add(this.btnProcessFile);
			this.Controls.Add(this.btnAboutPretext);
			this.Controls.Add(this.btnQuit);
			this.Controls.Add(this.grpProgress);
			this.Controls.Add(this.grpOutput);
			this.Controls.Add(this.grpInput);
			this.Controls.Add(this.btnBrowseOutput);
			this.Controls.Add(this.btnBrowseInput);
			this.Name = "Pretext_NET";
			this.Text = "Pretext.NET";
			this.SizeChanged += new System.EventHandler(this.Pretext_NET_SizeChanged);
			this.grpInput.ResumeLayout(false);
			this.grpInput.PerformLayout();
			this.grpOutput.ResumeLayout(false);
			this.grpOutput.PerformLayout();
			this.grpProgress.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnBrowseInput;
		private System.Windows.Forms.Button btnBrowseOutput;
		private System.Windows.Forms.GroupBox grpInput;
		private System.Windows.Forms.TextBox txtInput;
		private System.Windows.Forms.TextBox txtOutput;
		private System.Windows.Forms.GroupBox grpOutput;
		private System.Windows.Forms.GroupBox grpProgress;
		private System.Windows.Forms.Button btnQuit;
		private System.Windows.Forms.Button btnAboutPretext;
		private System.Windows.Forms.Button btnProcessFile;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Button btnAuto;
	}
}

