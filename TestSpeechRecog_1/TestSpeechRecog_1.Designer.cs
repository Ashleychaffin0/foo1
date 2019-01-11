namespace TestSpeechRecog_1 {
	partial class TestSpeechRecog_1 {
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
			this.btnTest = new System.Windows.Forms.Button();
			this.txtResult = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnTest
			// 
			this.btnTest.Location = new System.Drawing.Point(25, 26);
			this.btnTest.Name = "btnTest";
			this.btnTest.Size = new System.Drawing.Size(74, 23);
			this.btnTest.TabIndex = 0;
			this.btnTest.Text = "Test";
			this.btnTest.UseVisualStyleBackColor = true;
			this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
			// 
			// txtResult
			// 
			this.txtResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtResult.Location = new System.Drawing.Point(25, 79);
			this.txtResult.Name = "txtResult";
			this.txtResult.Size = new System.Drawing.Size(915, 20);
			this.txtResult.TabIndex = 1;
			// 
			// TestSpeechRecog_1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(952, 261);
			this.Controls.Add(this.txtResult);
			this.Controls.Add(this.btnTest);
			this.Name = "TestSpeechRecog_1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnTest;
		private System.Windows.Forms.TextBox txtResult;
	}
}

