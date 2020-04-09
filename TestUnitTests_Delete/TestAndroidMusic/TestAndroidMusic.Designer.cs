namespace TestAndroidMusic {
	partial class TestAndroidMusic {
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
            this.BtnTestPDM = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnTestPDM
            // 
            this.BtnTestPDM.Location = new System.Drawing.Point(34, 28);
            this.BtnTestPDM.Name = "BtnTestPDM";
            this.BtnTestPDM.Size = new System.Drawing.Size(143, 23);
            this.BtnTestPDM.TabIndex = 29;
            this.BtnTestPDM.Text = "Test PDM";
            this.BtnTestPDM.UseVisualStyleBackColor = true;
            this.BtnTestPDM.Click += new System.EventHandler(this.BtnTestPDM_Click);
            // 
            // TestAndroidMusic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BtnTestPDM);
            this.Name = "TestAndroidMusic";
            this.Text = "Form1";
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button BtnTestPDM;
	}
}

