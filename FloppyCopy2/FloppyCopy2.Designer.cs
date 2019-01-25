namespace FloppyCopy2 {
	partial class FloppyCopy2 {
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
			this.btnDirA = new System.Windows.Forms.Button();
			this.btnCopy = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnDirA
			// 
			this.btnDirA.Location = new System.Drawing.Point(36, 33);
			this.btnDirA.Name = "btnDirA";
			this.btnDirA.Size = new System.Drawing.Size(75, 23);
			this.btnDirA.TabIndex = 0;
			this.btnDirA.Text = "Dir A:";
			this.btnDirA.UseVisualStyleBackColor = true;
			// 
			// btnCopy
			// 
			this.btnCopy.Location = new System.Drawing.Point(137, 32);
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.Size = new System.Drawing.Size(163, 109);
			this.btnCopy.TabIndex = 1;
			this.btnCopy.Text = "Copy";
			this.btnCopy.UseVisualStyleBackColor = true;
			this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
			// 
			// FloppyCopy2
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(684, 479);
			this.Controls.Add(this.btnCopy);
			this.Controls.Add(this.btnDirA);
			this.Name = "FloppyCopy2";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.FloppyCopy2_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnDirA;
		private System.Windows.Forms.Button btnCopy;
	}
}

