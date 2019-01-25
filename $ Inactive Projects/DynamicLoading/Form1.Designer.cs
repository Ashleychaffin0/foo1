namespace DynamicLoading {
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
			this.btnShow = new System.Windows.Forms.Button();
			this.lbMsgs = new System.Windows.Forms.ListBox();
			this.btnClear = new System.Windows.Forms.Button();
			this.btnLoadDLL = new System.Windows.Forms.Button();
			this.txtDLLName = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.ofd = new System.Windows.Forms.OpenFileDialog();
			this.btnCall = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnShow
			// 
			this.btnShow.Location = new System.Drawing.Point(173, 52);
			this.btnShow.Name = "btnShow";
			this.btnShow.Size = new System.Drawing.Size(75, 23);
			this.btnShow.TabIndex = 0;
			this.btnShow.Text = "Show";
			this.btnShow.UseVisualStyleBackColor = true;
			this.btnShow.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// lbMsgs
			// 
			this.lbMsgs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lbMsgs.FormattingEnabled = true;
			this.lbMsgs.Location = new System.Drawing.Point(13, 81);
			this.lbMsgs.Name = "lbMsgs";
			this.lbMsgs.Size = new System.Drawing.Size(443, 173);
			this.lbMsgs.TabIndex = 1;
			// 
			// btnClear
			// 
			this.btnClear.Location = new System.Drawing.Point(268, 52);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(96, 23);
			this.btnClear.TabIndex = 2;
			this.btnClear.Text = "Clear messages\r\n";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// btnLoadDLL
			// 
			this.btnLoadDLL.Location = new System.Drawing.Point(12, 12);
			this.btnLoadDLL.Name = "btnLoadDLL";
			this.btnLoadDLL.Size = new System.Drawing.Size(75, 23);
			this.btnLoadDLL.TabIndex = 3;
			this.btnLoadDLL.Text = "Load DLL";
			this.btnLoadDLL.UseVisualStyleBackColor = true;
			this.btnLoadDLL.Click += new System.EventHandler(this.btnLoadDLL_Click);
			// 
			// txtDLLName
			// 
			this.txtDLLName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtDLLName.Location = new System.Drawing.Point(115, 12);
			this.txtDLLName.Name = "txtDLLName";
			this.txtDLLName.Size = new System.Drawing.Size(249, 20);
			this.txtDLLName.TabIndex = 4;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Location = new System.Drawing.Point(381, 12);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(75, 23);
			this.btnBrowse.TabIndex = 5;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// ofd
			// 
			this.ofd.FileName = "LLWS_Importer.dll";
			this.ofd.InitialDirectory = "C:\\LRS\\$LeadsLightning\\LLWS_Importer\\LLWS_Importer\\LLWS_Importer\\bin";
			this.ofd.RestoreDirectory = true;
			// 
			// btnCall
			// 
			this.btnCall.Location = new System.Drawing.Point(13, 52);
			this.btnCall.Name = "btnCall";
			this.btnCall.Size = new System.Drawing.Size(75, 23);
			this.btnCall.TabIndex = 6;
			this.btnCall.Text = "Call";
			this.btnCall.UseVisualStyleBackColor = true;
			this.btnCall.Click += new System.EventHandler(this.btnCall_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(468, 264);
			this.Controls.Add(this.btnCall);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.txtDLLName);
			this.Controls.Add(this.btnLoadDLL);
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.lbMsgs);
			this.Controls.Add(this.btnShow);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnShow;
		private System.Windows.Forms.ListBox lbMsgs;
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.Button btnLoadDLL;
		private System.Windows.Forms.TextBox txtDLLName;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.OpenFileDialog ofd;
		private System.Windows.Forms.Button btnCall;
	}
}

