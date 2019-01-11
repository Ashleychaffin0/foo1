namespace ZuneFixWmaTags {
	partial class ZuneFixWmaTags {
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
			this.btnBrowsePath = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbArtist = new System.Windows.Forms.ComboBox();
			this.cmbAlbums = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtPath = new System.Windows.Forms.TextBox();
			this.cmbTrack = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(14, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(37, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Path";
			// 
			// btnBrowsePath
			// 
			this.btnBrowsePath.Location = new System.Drawing.Point(226, 14);
			this.btnBrowsePath.Name = "btnBrowsePath";
			this.btnBrowsePath.Size = new System.Drawing.Size(75, 23);
			this.btnBrowsePath.TabIndex = 2;
			this.btnBrowsePath.Text = "Browse";
			this.btnBrowsePath.UseVisualStyleBackColor = true;
			this.btnBrowsePath.Click += new System.EventHandler(this.btnBrowsePath_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 52);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 17);
			this.label2.TabIndex = 3;
			this.label2.Text = "Artist";
			// 
			// cmbArtist
			// 
			this.cmbArtist.FormattingEnabled = true;
			this.cmbArtist.Location = new System.Drawing.Point(57, 52);
			this.cmbArtist.Name = "cmbArtist";
			this.cmbArtist.Size = new System.Drawing.Size(226, 24);
			this.cmbArtist.TabIndex = 4;
			this.cmbArtist.SelectedIndexChanged += new System.EventHandler(this.cmbArtist_SelectedIndexChanged);
			// 
			// cmbAlbums
			// 
			this.cmbAlbums.FormattingEnabled = true;
			this.cmbAlbums.Location = new System.Drawing.Point(349, 52);
			this.cmbAlbums.Name = "cmbAlbums";
			this.cmbAlbums.Size = new System.Drawing.Size(326, 24);
			this.cmbAlbums.TabIndex = 6;
			this.cmbAlbums.SelectedIndexChanged += new System.EventHandler(this.cmbAlbums_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(289, 52);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(47, 17);
			this.label3.TabIndex = 5;
			this.label3.Text = "Album";
			// 
			// txtPath
			// 
			this.txtPath.Location = new System.Drawing.Point(57, 14);
			this.txtPath.Name = "txtPath";
			this.txtPath.Size = new System.Drawing.Size(161, 22);
			this.txtPath.TabIndex = 1;
			// 
			// cmbTrack
			// 
			this.cmbTrack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cmbTrack.FormattingEnabled = true;
			this.cmbTrack.Location = new System.Drawing.Point(746, 52);
			this.cmbTrack.Name = "cmbTrack";
			this.cmbTrack.Size = new System.Drawing.Size(264, 24);
			this.cmbTrack.TabIndex = 8;
			this.cmbTrack.SelectedIndexChanged += new System.EventHandler(this.cmbTrack_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(686, 52);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(44, 17);
			this.label4.TabIndex = 7;
			this.label4.Text = "Track";
			// 
			// dataGridView1
			// 
			this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Location = new System.Drawing.Point(17, 82);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowTemplate.Height = 24;
			this.dataGridView1.Size = new System.Drawing.Size(993, 343);
			this.dataGridView1.TabIndex = 9;
			this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
			// 
			// ZuneFixWmaTags
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.PaleTurquoise;
			this.ClientSize = new System.Drawing.Size(1032, 437);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.cmbTrack);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.cmbAlbums);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cmbArtist);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnBrowsePath);
			this.Controls.Add(this.txtPath);
			this.Controls.Add(this.label1);
			this.Name = "ZuneFixWmaTags";
			this.Text = "Fix Zune Wma Tags";
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnBrowsePath;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cmbArtist;
		private System.Windows.Forms.ComboBox cmbAlbums;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtPath;
		private System.Windows.Forms.ComboBox cmbTrack;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.DataGridView dataGridView1;
	}
}

