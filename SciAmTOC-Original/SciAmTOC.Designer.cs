namespace SciAmTOC {
	partial class SciAmTOC {
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
			this.CmbFirstDecade = new System.Windows.Forms.ComboBox();
			this.CmbLastDecade = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.BtnGo = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(35, 27);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "First Decade";
			// 
			// CmbFirstDecade
			// 
			this.CmbFirstDecade.FormattingEnabled = true;
			this.CmbFirstDecade.Location = new System.Drawing.Point(132, 24);
			this.CmbFirstDecade.Name = "CmbFirstDecade";
			this.CmbFirstDecade.Size = new System.Drawing.Size(121, 24);
			this.CmbFirstDecade.TabIndex = 1;
			// 
			// CmbLastDecade
			// 
			this.CmbLastDecade.FormattingEnabled = true;
			this.CmbLastDecade.Location = new System.Drawing.Point(378, 24);
			this.CmbLastDecade.Name = "CmbLastDecade";
			this.CmbLastDecade.Size = new System.Drawing.Size(121, 24);
			this.CmbLastDecade.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(271, 27);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 17);
			this.label2.TabIndex = 2;
			this.label2.Text = "Last Decade";
			// 
			// BtnGo
			// 
			this.BtnGo.Location = new System.Drawing.Point(545, 24);
			this.BtnGo.Name = "BtnGo";
			this.BtnGo.Size = new System.Drawing.Size(75, 23);
			this.BtnGo.TabIndex = 4;
			this.BtnGo.Text = "Go";
			this.BtnGo.UseVisualStyleBackColor = true;
			this.BtnGo.Click += new System.EventHandler(this.BtnGo_Click);
			// 
			// SciAmTOC
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(861, 432);
			this.Controls.Add(this.BtnGo);
			this.Controls.Add(this.CmbLastDecade);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.CmbFirstDecade);
			this.Controls.Add(this.label1);
			this.Name = "SciAmTOC";
			this.Text = "Scientific American TOC";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox CmbFirstDecade;
		private System.Windows.Forms.ComboBox CmbLastDecade;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button BtnGo;
	}
}

