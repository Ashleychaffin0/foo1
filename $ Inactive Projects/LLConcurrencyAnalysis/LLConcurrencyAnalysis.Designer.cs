namespace LLConcurrencyAnalysis {
	partial class LLConcurrencyAnalysis {
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
			this.btnDeviceID = new System.Windows.Forms.Button();
			this.lbMsgs = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// btnDeviceID
			// 
			this.btnDeviceID.Location = new System.Drawing.Point(12, 12);
			this.btnDeviceID.Name = "btnDeviceID";
			this.btnDeviceID.Size = new System.Drawing.Size(75, 23);
			this.btnDeviceID.TabIndex = 0;
			this.btnDeviceID.Text = "Device ID";
			this.btnDeviceID.UseVisualStyleBackColor = true;
			this.btnDeviceID.Click += new System.EventHandler(this.btnDeviceID_Click);
			// 
			// lbMsgs
			// 
			this.lbMsgs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lbMsgs.FormattingEnabled = true;
			this.lbMsgs.Location = new System.Drawing.Point(12, 41);
			this.lbMsgs.Name = "lbMsgs";
			this.lbMsgs.Size = new System.Drawing.Size(260, 212);
			this.lbMsgs.TabIndex = 1;
			// 
			// LLConcurrencyAnalysis
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 264);
			this.Controls.Add(this.lbMsgs);
			this.Controls.Add(this.btnDeviceID);
			this.Name = "LLConcurrencyAnalysis";
			this.Text = "LL Concurrency Analysis";
			this.Load += new System.EventHandler(this.LLConcurrencyAnalysis_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnDeviceID;
		private System.Windows.Forms.ListBox lbMsgs;
	}
}

