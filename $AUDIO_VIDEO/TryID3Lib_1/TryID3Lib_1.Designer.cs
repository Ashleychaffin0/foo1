namespace TryID3Lib_1 {
	partial class TryID3Lib_1 {
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
			this.btn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btn
			// 
			this.btn.Location = new System.Drawing.Point(32, 60);
			this.btn.Name = "btn";
			this.btn.Size = new System.Drawing.Size(386, 108);
			this.btn.TabIndex = 0;
			this.btn.Text = "Hello";
			this.btn.UseVisualStyleBackColor = true;
			// 
			// TryID3Lib_1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(871, 512);
			this.Controls.Add(this.btn);
			this.Name = "TryID3Lib_1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.TryID3Lib_1_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btn;
	}
}

