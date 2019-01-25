namespace WRR1 {
	partial class WRR1 {
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
			this.btnPushMe = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.btnHello = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnPushMe
			// 
			this.btnPushMe.Location = new System.Drawing.Point(56, 35);
			this.btnPushMe.Name = "btnPushMe";
			this.btnPushMe.Size = new System.Drawing.Size(75, 23);
			this.btnPushMe.TabIndex = 0;
			this.btnPushMe.Text = "Push Me";
			this.btnPushMe.UseVisualStyleBackColor = true;
			this.btnPushMe.Click += new System.EventHandler(this.btnPushMe_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(56, 85);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Name";
			// 
			// txtName
			// 
			this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtName.Location = new System.Drawing.Point(161, 82);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(399, 20);
			this.txtName.TabIndex = 2;
			// 
			// btnHello
			// 
			this.btnHello.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnHello.Location = new System.Drawing.Point(668, 80);
			this.btnHello.Name = "btnHello";
			this.btnHello.Size = new System.Drawing.Size(75, 23);
			this.btnHello.TabIndex = 3;
			this.btnHello.Text = "Hello";
			this.btnHello.UseVisualStyleBackColor = true;
			this.btnHello.Click += new System.EventHandler(this.btnHello_Click);
			// 
			// WRR1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(785, 575);
			this.Controls.Add(this.btnHello);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnPushMe);
			this.Name = "WRR1";
			this.Text = "My First C# Program";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnPushMe;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Button btnHello;
	}
}

