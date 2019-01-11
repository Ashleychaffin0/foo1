namespace TestCRC64 {
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
			this.txtRaw = new System.Windows.Forms.TextBox();
			this.lblCRC32 = new System.Windows.Forms.Label();
			this.btnGo = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtRaw
			// 
			this.txtRaw.Location = new System.Drawing.Point(93, 42);
			this.txtRaw.Name = "txtRaw";
			this.txtRaw.Size = new System.Drawing.Size(321, 22);
			this.txtRaw.TabIndex = 0;
			this.txtRaw.Text = "Hello";
			// 
			// lblCRC32
			// 
			this.lblCRC32.AutoSize = true;
			this.lblCRC32.Location = new System.Drawing.Point(90, 97);
			this.lblCRC32.Name = "lblCRC32";
			this.lblCRC32.Size = new System.Drawing.Size(0, 17);
			this.lblCRC32.TabIndex = 1;
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(201, 149);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 2;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(467, 260);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.lblCRC32);
			this.Controls.Add(this.txtRaw);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtRaw;
		private System.Windows.Forms.Label lblCRC32;
		private System.Windows.Forms.Button btnGo;
	}
}

