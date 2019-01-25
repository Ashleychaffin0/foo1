namespace Lou1 {
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
			this.label1 = new System.Windows.Forms.Label();
			this.txtX = new System.Windows.Forms.TextBox();
			this.Answer = new System.Windows.Forms.Label();
			this.txtAnswer = new System.Windows.Forms.TextBox();
			this.btnGo = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(25, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(55, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Enter X";
			// 
			// txtX
			// 
			this.txtX.Location = new System.Drawing.Point(98, 40);
			this.txtX.Name = "txtX";
			this.txtX.Size = new System.Drawing.Size(178, 22);
			this.txtX.TabIndex = 1;
			// 
			// Answer
			// 
			this.Answer.AutoSize = true;
			this.Answer.Location = new System.Drawing.Point(28, 89);
			this.Answer.Name = "Answer";
			this.Answer.Size = new System.Drawing.Size(54, 17);
			this.Answer.TabIndex = 2;
			this.Answer.Text = "Answer";
			// 
			// txtAnswer
			// 
			this.txtAnswer.Location = new System.Drawing.Point(98, 89);
			this.txtAnswer.Name = "txtAnswer";
			this.txtAnswer.Size = new System.Drawing.Size(178, 22);
			this.txtAnswer.TabIndex = 3;
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(372, 38);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 4;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(481, 378);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.txtAnswer);
			this.Controls.Add(this.Answer);
			this.Controls.Add(this.txtX);
			this.Controls.Add(this.label1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtX;
		private System.Windows.Forms.Label Answer;
		private System.Windows.Forms.TextBox txtAnswer;
		private System.Windows.Forms.Button btnGo;
	}
}

