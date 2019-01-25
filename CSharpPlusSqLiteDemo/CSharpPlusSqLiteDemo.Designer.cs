namespace CSharpPlusSqLiteDemo {
	partial class CSharpPlusSqLiteDemo {
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
			this.txtSQL = new System.Windows.Forms.TextBox();
			this.btnGo = new System.Windows.Forms.Button();
			this.lbOutput = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(30, 29);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(74, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Enter SQL";
			// 
			// txtSQL
			// 
			this.txtSQL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSQL.Location = new System.Drawing.Point(127, 29);
			this.txtSQL.Multiline = true;
			this.txtSQL.Name = "txtSQL";
			this.txtSQL.Size = new System.Drawing.Size(534, 62);
			this.txtSQL.TabIndex = 1;
			this.txtSQL.Text = "SELECT * FROM mail1 limit(10);";
			// 
			// btnGo
			// 
			this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGo.Location = new System.Drawing.Point(690, 29);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 2;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// lbOutput
			// 
			this.lbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lbOutput.FormattingEnabled = true;
			this.lbOutput.ItemHeight = 16;
			this.lbOutput.Location = new System.Drawing.Point(127, 115);
			this.lbOutput.Name = "lbOutput";
			this.lbOutput.Size = new System.Drawing.Size(534, 244);
			this.lbOutput.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(30, 115);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(51, 17);
			this.label2.TabIndex = 4;
			this.label2.Text = "Output";
			// 
			// CSharpPlusSqLiteDemo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(781, 370);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lbOutput);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.txtSQL);
			this.Controls.Add(this.label1);
			this.Name = "CSharpPlusSqLiteDemo";
			this.Text = "C# SqLite Demo";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtSQL;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.ListBox lbOutput;
		private System.Windows.Forms.Label label2;
	}
}

