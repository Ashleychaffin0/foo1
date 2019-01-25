namespace FindLargeFiles2 {
	partial class FindLargeFiles2 {
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
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.label1 = new System.Windows.Forms.Label();
			this.txtMinSize = new System.Windows.Forms.TextBox();
			this.cmbMinSize = new System.Windows.Forms.ComboBox();
			this.cmbMaxSize = new System.Windows.Forms.ComboBox();
			this.txtMaxSize = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnGo = new System.Windows.Forms.Button();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.listView1 = new System.Windows.Forms.ListView();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 33);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(47, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Min Size";
			// 
			// txtMinSize
			// 
			this.txtMinSize.Location = new System.Drawing.Point(77, 30);
			this.txtMinSize.Name = "txtMinSize";
			this.txtMinSize.Size = new System.Drawing.Size(57, 20);
			this.txtMinSize.TabIndex = 1;
			// 
			// cmbMinSize
			// 
			this.cmbMinSize.FormattingEnabled = true;
			this.cmbMinSize.Items.AddRange(new object[] {
            "KB",
            "MB",
            "GB"});
			this.cmbMinSize.Location = new System.Drawing.Point(150, 29);
			this.cmbMinSize.Name = "cmbMinSize";
			this.cmbMinSize.Size = new System.Drawing.Size(48, 21);
			this.cmbMinSize.TabIndex = 2;
			// 
			// cmbMaxSize
			// 
			this.cmbMaxSize.FormattingEnabled = true;
			this.cmbMaxSize.Items.AddRange(new object[] {
            "KB",
            "MB",
            "GB"});
			this.cmbMaxSize.Location = new System.Drawing.Point(396, 29);
			this.cmbMaxSize.Name = "cmbMaxSize";
			this.cmbMaxSize.Size = new System.Drawing.Size(48, 21);
			this.cmbMaxSize.TabIndex = 5;
			// 
			// txtMaxSize
			// 
			this.txtMaxSize.Location = new System.Drawing.Point(323, 30);
			this.txtMaxSize.Name = "txtMaxSize";
			this.txtMaxSize.Size = new System.Drawing.Size(57, 20);
			this.txtMaxSize.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(258, 33);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(50, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Max Size";
			// 
			// btnGo
			// 
			this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGo.Location = new System.Drawing.Point(764, 28);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 6;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(493, 27);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(75, 23);
			this.btnBrowse.TabIndex = 7;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			// 
			// listView1
			// 
			this.listView1.Location = new System.Drawing.Point(12, 71);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(827, 230);
			this.listView1.TabIndex = 8;
			this.listView1.UseCompatibleStateImageBehavior = false;
			// 
			// FindLargeFiles2
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(851, 304);
			this.Controls.Add(this.listView1);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.cmbMaxSize);
			this.Controls.Add(this.txtMaxSize);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cmbMinSize);
			this.Controls.Add(this.txtMinSize);
			this.Controls.Add(this.label1);
			this.Name = "FindLargeFiles2";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.FindLargeFiles2_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtMinSize;
		private System.Windows.Forms.ComboBox cmbMinSize;
		private System.Windows.Forms.ComboBox cmbMaxSize;
		private System.Windows.Forms.TextBox txtMaxSize;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.ListView listView1;
	}
}

