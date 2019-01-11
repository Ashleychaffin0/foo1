namespace SaveBrowserTabLinks {
	partial class SaveBrowserTabLinks {
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
			this.ChkChrome = new System.Windows.Forms.CheckBox();
			this.ChkEdge = new System.Windows.Forms.CheckBox();
			this.BtnSave = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// ChkChrome
			// 
			this.ChkChrome.AutoSize = true;
			this.ChkChrome.Location = new System.Drawing.Point(24, 28);
			this.ChkChrome.Name = "ChkChrome";
			this.ChkChrome.Size = new System.Drawing.Size(79, 21);
			this.ChkChrome.TabIndex = 0;
			this.ChkChrome.Text = "Chrome";
			this.ChkChrome.UseVisualStyleBackColor = true;
			// 
			// ChkEdge
			// 
			this.ChkEdge.AutoSize = true;
			this.ChkEdge.Location = new System.Drawing.Point(159, 28);
			this.ChkEdge.Name = "ChkEdge";
			this.ChkEdge.Size = new System.Drawing.Size(63, 21);
			this.ChkEdge.TabIndex = 1;
			this.ChkEdge.Text = "Edge";
			this.ChkEdge.UseVisualStyleBackColor = true;
			// 
			// BtnSave
			// 
			this.BtnSave.Location = new System.Drawing.Point(91, 71);
			this.BtnSave.Name = "BtnSave";
			this.BtnSave.Size = new System.Drawing.Size(75, 23);
			this.BtnSave.TabIndex = 2;
			this.BtnSave.Text = "Save";
			this.BtnSave.UseVisualStyleBackColor = true;
			this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
			// 
			// SaveBrowserTabLinks
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(254, 130);
			this.Controls.Add(this.BtnSave);
			this.Controls.Add(this.ChkEdge);
			this.Controls.Add(this.ChkChrome);
			this.Name = "SaveBrowserTabLinks";
			this.Text = "Save Browser Tab Links";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox ChkChrome;
		private System.Windows.Forms.CheckBox ChkEdge;
		private System.Windows.Forms.Button BtnSave;
	}
}

