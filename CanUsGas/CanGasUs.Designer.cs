namespace CanUsGas {
	partial class CanGasUs {
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
			this.label1 = new System.Windows.Forms.Label();
			this.txtUSD = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtCAD = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtUsPerGallon = new System.Windows.Forms.TextBox();
			this.txtCanPerLitre = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(366, 30);
			this.label1.TabIndex = 0;
			this.label1.Text = "Canada / US Gas Price Conversions";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtUSD
			// 
			this.txtUSD.Location = new System.Drawing.Point(48, 67);
			this.txtUSD.Name = "txtUSD";
			this.txtUSD.Size = new System.Drawing.Size(100, 20);
			this.txtUSD.TabIndex = 1;
			this.txtUSD.Click += new System.EventHandler(this.txtUSD_Click);
			this.txtUSD.TextChanged += new System.EventHandler(this.txtUSD_TextChanged);
			this.txtUSD.Enter += new System.EventHandler(this.txtUSD_Enter);
			this.txtUSD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUSD_KeyPress);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(154, 70);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "$US = ";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(300, 70);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(32, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "$Can";
			// 
			// txtCAD
			// 
			this.txtCAD.Location = new System.Drawing.Point(194, 67);
			this.txtCAD.Name = "txtCAD";
			this.txtCAD.Size = new System.Drawing.Size(100, 20);
			this.txtCAD.TabIndex = 3;
			this.txtCAD.Click += new System.EventHandler(this.txtCAD_Click);
			this.txtCAD.Enter += new System.EventHandler(this.txtCAD_Enter);
			this.txtCAD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCAD_KeyPress);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(48, 114);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(47, 13);
			this.label4.TabIndex = 5;
			this.label4.Text = "$US/gal";
			// 
			// txtUsPerGallon
			// 
			this.txtUsPerGallon.Location = new System.Drawing.Point(141, 111);
			this.txtUsPerGallon.Name = "txtUsPerGallon";
			this.txtUsPerGallon.Size = new System.Drawing.Size(100, 20);
			this.txtUsPerGallon.TabIndex = 6;
			this.txtUsPerGallon.TextChanged += new System.EventHandler(this.txtUsPerGallon_TextChanged);
			// 
			// txtCanPerLitre
			// 
			this.txtCanPerLitre.Location = new System.Drawing.Point(141, 150);
			this.txtCanPerLitre.Name = "txtCanPerLitre";
			this.txtCanPerLitre.Size = new System.Drawing.Size(100, 20);
			this.txtCanPerLitre.TabIndex = 8;
			this.txtCanPerLitre.TextChanged += new System.EventHandler(this.txtCanPerLitre_TextChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(48, 153);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(56, 13);
			this.label5.TabIndex = 7;
			this.label5.Text = "$CAN/litre";
			// 
			// CanGasUs
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(390, 199);
			this.Controls.Add(this.txtCanPerLitre);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.txtUsPerGallon);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtCAD);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtUSD);
			this.Controls.Add(this.label1);
			this.Name = "CanGasUs";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtUSD;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtCAD;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtUsPerGallon;
		private System.Windows.Forms.TextBox txtCanPerLitre;
		private System.Windows.Forms.Label label5;
	}
}

