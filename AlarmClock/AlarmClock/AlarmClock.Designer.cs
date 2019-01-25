namespace AlarmClock {
	partial class AlarmClock {
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
			this.components = new System.ComponentModel.Container();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.label1 = new System.Windows.Forms.Label();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.txtFilename = new System.Windows.Forms.TextBox();
			this.btnBrowseFileToPlay = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.btnGo = new System.Windows.Forms.Button();
			this.txtHour = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtSecond = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtMinute = new System.Windows.Forms.TextBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radPM = new System.Windows.Forms.RadioButton();
			this.radAM = new System.Windows.Forms.RadioButton();
			this.lblCurrentTime = new System.Windows.Forms.Label();
			this.btnClearFileToPlay = new System.Windows.Forms.Button();
			this.btnClearFolder = new System.Windows.Forms.Button();
			this.btnBrowseForFolder = new System.Windows.Forms.Button();
			this.txtFolderName = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.Location = new System.Drawing.Point(140, 30);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(264, 22);
			this.dateTimePicker1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(35, 92);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(76, 17);
			this.label1.TabIndex = 1;
			this.label1.Text = "File to play";
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// txtFilename
			// 
			this.txtFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFilename.Location = new System.Drawing.Point(140, 92);
			this.txtFilename.Name = "txtFilename";
			this.txtFilename.Size = new System.Drawing.Size(373, 22);
			this.txtFilename.TabIndex = 3;
			// 
			// btnBrowseFileToPlay
			// 
			this.btnBrowseFileToPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseFileToPlay.Location = new System.Drawing.Point(532, 91);
			this.btnBrowseFileToPlay.Name = "btnBrowseFileToPlay";
			this.btnBrowseFileToPlay.Size = new System.Drawing.Size(75, 23);
			this.btnBrowseFileToPlay.TabIndex = 4;
			this.btnBrowseFileToPlay.Text = "Browse";
			this.btnBrowseFileToPlay.UseVisualStyleBackColor = true;
			this.btnBrowseFileToPlay.Click += new System.EventHandler(this.btnBrowseFileToPlay_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(35, 35);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(45, 17);
			this.label2.TabIndex = 4;
			this.label2.Text = "When";
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(38, 170);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 5;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// txtHour
			// 
			this.txtHour.Location = new System.Drawing.Point(444, 32);
			this.txtHour.Name = "txtHour";
			this.txtHour.Size = new System.Drawing.Size(41, 22);
			this.txtHour.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(441, 9);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(45, 17);
			this.label3.TabIndex = 7;
			this.label3.Text = "Hour";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(554, 9);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(39, 17);
			this.label4.TabIndex = 9;
			this.label4.Text = "Sec";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// txtSecond
			// 
			this.txtSecond.Location = new System.Drawing.Point(557, 32);
			this.txtSecond.Name = "txtSecond";
			this.txtSecond.Size = new System.Drawing.Size(41, 22);
			this.txtSecond.TabIndex = 8;
			this.txtSecond.Text = "0";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(498, 9);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(41, 17);
			this.label5.TabIndex = 11;
			this.label5.Text = "Min";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// txtMinute
			// 
			this.txtMinute.Location = new System.Drawing.Point(501, 32);
			this.txtMinute.Name = "txtMinute";
			this.txtMinute.Size = new System.Drawing.Size(41, 22);
			this.txtMinute.TabIndex = 2;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radPM);
			this.groupBox1.Controls.Add(this.radAM);
			this.groupBox1.Location = new System.Drawing.Point(604, 9);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(81, 72);
			this.groupBox1.TabIndex = 12;
			this.groupBox1.TabStop = false;
			// 
			// radPM
			// 
			this.radPM.AutoSize = true;
			this.radPM.Location = new System.Drawing.Point(6, 40);
			this.radPM.Name = "radPM";
			this.radPM.Size = new System.Drawing.Size(49, 21);
			this.radPM.TabIndex = 1;
			this.radPM.Text = "PM";
			this.radPM.UseVisualStyleBackColor = true;
			// 
			// radAM
			// 
			this.radAM.AutoSize = true;
			this.radAM.Checked = true;
			this.radAM.Location = new System.Drawing.Point(6, 13);
			this.radAM.Name = "radAM";
			this.radAM.Size = new System.Drawing.Size(49, 21);
			this.radAM.TabIndex = 0;
			this.radAM.TabStop = true;
			this.radAM.Text = "AM";
			this.radAM.UseVisualStyleBackColor = true;
			// 
			// lblCurrentTime
			// 
			this.lblCurrentTime.AutoSize = true;
			this.lblCurrentTime.Location = new System.Drawing.Point(197, 173);
			this.lblCurrentTime.Name = "lblCurrentTime";
			this.lblCurrentTime.Size = new System.Drawing.Size(0, 17);
			this.lblCurrentTime.TabIndex = 13;
			// 
			// btnClearFileToPlay
			// 
			this.btnClearFileToPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClearFileToPlay.Location = new System.Drawing.Point(624, 92);
			this.btnClearFileToPlay.Name = "btnClearFileToPlay";
			this.btnClearFileToPlay.Size = new System.Drawing.Size(75, 23);
			this.btnClearFileToPlay.TabIndex = 14;
			this.btnClearFileToPlay.Text = "Clear";
			this.btnClearFileToPlay.UseVisualStyleBackColor = true;
			this.btnClearFileToPlay.Click += new System.EventHandler(this.btnClearFileToPlay_Click);
			// 
			// btnClearFolder
			// 
			this.btnClearFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClearFolder.Location = new System.Drawing.Point(624, 130);
			this.btnClearFolder.Name = "btnClearFolder";
			this.btnClearFolder.Size = new System.Drawing.Size(75, 23);
			this.btnClearFolder.TabIndex = 18;
			this.btnClearFolder.Text = "Clear";
			this.btnClearFolder.UseVisualStyleBackColor = true;
			this.btnClearFolder.Click += new System.EventHandler(this.btnClearFolder_Click);
			// 
			// btnBrowseForFolder
			// 
			this.btnBrowseForFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseForFolder.Location = new System.Drawing.Point(532, 129);
			this.btnBrowseForFolder.Name = "btnBrowseForFolder";
			this.btnBrowseForFolder.Size = new System.Drawing.Size(75, 23);
			this.btnBrowseForFolder.TabIndex = 17;
			this.btnBrowseForFolder.Text = "Browse";
			this.btnBrowseForFolder.UseVisualStyleBackColor = true;
			this.btnBrowseForFolder.Click += new System.EventHandler(this.btnBrowseForFolder_Click);
			// 
			// txtFolderName
			// 
			this.txtFolderName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFolderName.Location = new System.Drawing.Point(140, 130);
			this.txtFolderName.Name = "txtFolderName";
			this.txtFolderName.Size = new System.Drawing.Size(373, 22);
			this.txtFolderName.TabIndex = 16;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(35, 130);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(94, 17);
			this.label6.TabIndex = 15;
			this.label6.Text = "Folder to play";
			// 
			// AlarmClock
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(717, 217);
			this.Controls.Add(this.btnClearFolder);
			this.Controls.Add(this.btnBrowseForFolder);
			this.Controls.Add(this.txtFolderName);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.btnClearFileToPlay);
			this.Controls.Add(this.lblCurrentTime);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.txtMinute);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtSecond);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtHour);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnBrowseFileToPlay);
			this.Controls.Add(this.txtFilename);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.dateTimePicker1);
			this.Name = "AlarmClock";
			this.Text = "LRS Alarm Clock";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.TextBox txtFilename;
		private System.Windows.Forms.Button btnBrowseFileToPlay;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.TextBox txtHour;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtSecond;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtMinute;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radPM;
		private System.Windows.Forms.RadioButton radAM;
		private System.Windows.Forms.Label lblCurrentTime;
		private System.Windows.Forms.Button btnClearFileToPlay;
		private System.Windows.Forms.Button btnClearFolder;
		private System.Windows.Forms.Button btnBrowseForFolder;
		private System.Windows.Forms.TextBox txtFolderName;
		private System.Windows.Forms.Label label6;
	}
}

