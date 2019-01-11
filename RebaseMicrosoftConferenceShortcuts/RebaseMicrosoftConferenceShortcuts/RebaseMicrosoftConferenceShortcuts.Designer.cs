namespace RebaseMicrosoftConferenceShortcuts {
	partial class RebaseMicrosoftConferenceShortcuts {
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
			this.cmbFromDrive = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.cmbToDrive = new System.Windows.Forms.ComboBox();
			this.txtTargetDir = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.btnGo = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(32, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(54, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Target Dir";
			// 
			// cmbFromDrive
			// 
			this.cmbFromDrive.FormattingEnabled = true;
			this.cmbFromDrive.Location = new System.Drawing.Point(110, 55);
			this.cmbFromDrive.Name = "cmbFromDrive";
			this.cmbFromDrive.Size = new System.Drawing.Size(73, 21);
			this.cmbFromDrive.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(32, 58);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(58, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "From Drive";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(32, 90);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "To Drive";
			// 
			// cmbToDrive
			// 
			this.cmbToDrive.FormattingEnabled = true;
			this.cmbToDrive.Location = new System.Drawing.Point(110, 87);
			this.cmbToDrive.Name = "cmbToDrive";
			this.cmbToDrive.Size = new System.Drawing.Size(73, 21);
			this.cmbToDrive.TabIndex = 4;
			// 
			// txtTargetDir
			// 
			this.txtTargetDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtTargetDir.Location = new System.Drawing.Point(110, 19);
			this.txtTargetDir.Name = "txtTargetDir";
			this.txtTargetDir.Size = new System.Drawing.Size(737, 20);
			this.txtTargetDir.TabIndex = 5;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Location = new System.Drawing.Point(886, 17);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(75, 23);
			this.btnBrowse.TabIndex = 6;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(35, 121);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 7;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// RebaseMicrosoftConferenceShortcuts
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(988, 432);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.txtTargetDir);
			this.Controls.Add(this.cmbToDrive);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cmbFromDrive);
			this.Controls.Add(this.label1);
			this.Name = "RebaseMicrosoftConferenceShortcuts";
			this.Text = "Rebae Microsoft Conference Shortcuts";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbFromDrive;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cmbToDrive;
		private System.Windows.Forms.TextBox txtTargetDir;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Button btnGo;
	}
}

