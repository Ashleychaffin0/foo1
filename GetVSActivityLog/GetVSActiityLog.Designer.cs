namespace GetVSActivityLog {
	partial class GetVSActiityLog {
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
			this.CmbVersion = new System.Windows.Forms.ComboBox();
			this.ChkErrors = new System.Windows.Forms.CheckBox();
			this.ChkInformation = new System.Windows.Forms.CheckBox();
			this.ChkOther = new System.Windows.Forms.CheckBox();
			this.BtnGo = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(28, 28);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(106, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Visual Studio Version";
			// 
			// CmbVersion
			// 
			this.CmbVersion.FormattingEnabled = true;
			this.CmbVersion.Location = new System.Drawing.Point(154, 25);
			this.CmbVersion.Name = "CmbVersion";
			this.CmbVersion.Size = new System.Drawing.Size(121, 21);
			this.CmbVersion.TabIndex = 1;
			this.CmbVersion.SelectedIndexChanged += new System.EventHandler(this.CmbVersion_SelectedIndexChanged);
			// 
			// ChkErrors
			// 
			this.ChkErrors.AutoSize = true;
			this.ChkErrors.Location = new System.Drawing.Point(31, 67);
			this.ChkErrors.Name = "ChkErrors";
			this.ChkErrors.Size = new System.Drawing.Size(53, 17);
			this.ChkErrors.TabIndex = 2;
			this.ChkErrors.Text = "Errors";
			this.ChkErrors.UseVisualStyleBackColor = true;
			// 
			// ChkInformation
			// 
			this.ChkInformation.AutoSize = true;
			this.ChkInformation.Location = new System.Drawing.Point(120, 67);
			this.ChkInformation.Name = "ChkInformation";
			this.ChkInformation.Size = new System.Drawing.Size(78, 17);
			this.ChkInformation.TabIndex = 3;
			this.ChkInformation.Text = "Information";
			this.ChkInformation.UseVisualStyleBackColor = true;
			// 
			// ChkOther
			// 
			this.ChkOther.AutoSize = true;
			this.ChkOther.Location = new System.Drawing.Point(222, 67);
			this.ChkOther.Name = "ChkOther";
			this.ChkOther.Size = new System.Drawing.Size(52, 17);
			this.ChkOther.TabIndex = 4;
			this.ChkOther.Text = "Other";
			this.ChkOther.UseVisualStyleBackColor = true;
			// 
			// BtnGo
			// 
			this.BtnGo.Location = new System.Drawing.Point(350, 67);
			this.BtnGo.Name = "BtnGo";
			this.BtnGo.Size = new System.Drawing.Size(75, 23);
			this.BtnGo.TabIndex = 5;
			this.BtnGo.Text = "Go";
			this.BtnGo.UseVisualStyleBackColor = true;
			// 
			// GetVSActiityLog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(550, 465);
			this.Controls.Add(this.BtnGo);
			this.Controls.Add(this.ChkOther);
			this.Controls.Add(this.ChkInformation);
			this.Controls.Add(this.ChkErrors);
			this.Controls.Add(this.CmbVersion);
			this.Controls.Add(this.label1);
			this.Name = "GetVSActiityLog";
			this.Text = "Get VS Activity Log";
			this.Load += new System.EventHandler(this.GetVSActiityLog_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox CmbVersion;
		private System.Windows.Forms.CheckBox ChkErrors;
		private System.Windows.Forms.CheckBox ChkInformation;
		private System.Windows.Forms.CheckBox ChkOther;
		private System.Windows.Forms.Button BtnGo;
	}
}

