namespace MyDumbInterpreter {
	partial class MyDumbInterpreter {
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
			this.TxtProgram = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.LbOutput = new System.Windows.Forms.ListBox();
			this.BtnRun = new System.Windows.Forms.Button();
			this.BtnContinue = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(44, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(399, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Program";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// TxtProgram
			// 
			this.TxtProgram.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.TxtProgram.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TxtProgram.Location = new System.Drawing.Point(28, 53);
			this.TxtProgram.Multiline = true;
			this.TxtProgram.Name = "TxtProgram";
			this.TxtProgram.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.TxtProgram.Size = new System.Drawing.Size(428, 372);
			this.TxtProgram.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(604, 13);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(458, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "Output";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// LbOutput
			// 
			this.LbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LbOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.LbOutput.FormattingEnabled = true;
			this.LbOutput.HorizontalScrollbar = true;
			this.LbOutput.IntegralHeight = false;
			this.LbOutput.ItemHeight = 25;
			this.LbOutput.Location = new System.Drawing.Point(604, 53);
			this.LbOutput.Name = "LbOutput";
			this.LbOutput.Size = new System.Drawing.Size(458, 372);
			this.LbOutput.TabIndex = 3;
			// 
			// BtnRun
			// 
			this.BtnRun.Location = new System.Drawing.Point(479, 13);
			this.BtnRun.Name = "BtnRun";
			this.BtnRun.Size = new System.Drawing.Size(103, 23);
			this.BtnRun.TabIndex = 4;
			this.BtnRun.Text = "Run";
			this.BtnRun.UseVisualStyleBackColor = true;
			this.BtnRun.Click += new System.EventHandler(this.BtnRun_Click);
			// 
			// BtnContinue
			// 
			this.BtnContinue.Location = new System.Drawing.Point(479, 62);
			this.BtnContinue.Name = "BtnContinue";
			this.BtnContinue.Size = new System.Drawing.Size(103, 23);
			this.BtnContinue.TabIndex = 5;
			this.BtnContinue.Text = "Continue";
			this.BtnContinue.UseVisualStyleBackColor = true;
			this.BtnContinue.Visible = false;
			this.BtnContinue.Click += new System.EventHandler(this.BtnContinue_Click);
			// 
			// MyDumbInterpreter
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1092, 443);
			this.Controls.Add(this.BtnContinue);
			this.Controls.Add(this.BtnRun);
			this.Controls.Add(this.LbOutput);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.TxtProgram);
			this.Controls.Add(this.label1);
			this.Name = "MyDumbInterpreter";
			this.Text = "My Dumb Interpreter";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox TxtProgram;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button BtnRun;
		public System.Windows.Forms.ListBox LbOutput;
		private System.Windows.Forms.Button BtnContinue;
	}
}

