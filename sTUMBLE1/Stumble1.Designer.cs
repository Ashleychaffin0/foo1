namespace Stumble1
{
	partial class Stumble1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.web1 = new System.Windows.Forms.WebBrowser();
			this.BtnGo = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// web1
			// 
			this.web1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.web1.Location = new System.Drawing.Point(12, 35);
			this.web1.MinimumSize = new System.Drawing.Size(20, 20);
			this.web1.Name = "web1";
			this.web1.Size = new System.Drawing.Size(538, 294);
			this.web1.TabIndex = 0;
			// 
			// BtnGo
			// 
			this.BtnGo.Location = new System.Drawing.Point(12, 6);
			this.BtnGo.Name = "BtnGo";
			this.BtnGo.Size = new System.Drawing.Size(75, 23);
			this.BtnGo.TabIndex = 1;
			this.BtnGo.Text = "Go";
			this.BtnGo.UseVisualStyleBackColor = true;
			this.BtnGo.Click += new System.EventHandler(this.BtnGo_Click);
			// 
			// Stumble1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(562, 341);
			this.Controls.Add(this.BtnGo);
			this.Controls.Add(this.web1);
			this.Name = "Stumble1";
			this.Text = "Stumble Upon 1";
			this.Load += new System.EventHandler(this.Stumble1_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.WebBrowser web1;
		private System.Windows.Forms.Button BtnGo;
	}
}

