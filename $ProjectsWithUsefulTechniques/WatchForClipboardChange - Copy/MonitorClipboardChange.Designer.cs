namespace MonitorClipboardChange {
	partial class MonitorClipboardChange {
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
			this.BtnEnableDisable = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// BtnEnableDisable
			// 
			this.BtnEnableDisable.Location = new System.Drawing.Point(12, 12);
			this.BtnEnableDisable.Name = "BtnEnableDisable";
			this.BtnEnableDisable.Size = new System.Drawing.Size(91, 23);
			this.BtnEnableDisable.TabIndex = 0;
			this.BtnEnableDisable.Text = "Disable";
			this.BtnEnableDisable.UseVisualStyleBackColor = true;
			this.BtnEnableDisable.Click += new System.EventHandler(this.BtnEnableDisable_Click);
			// 
			// MonitorClipboardChange
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(270, 54);
			this.Controls.Add(this.BtnEnableDisable);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MonitorClipboardChange";
			this.Text = "Monitor Clipboard Change";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button BtnEnableDisable;
	}
}

