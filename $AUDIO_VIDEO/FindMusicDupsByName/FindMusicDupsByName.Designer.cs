namespace FindMusicDupsByName {
	partial class FindMusicDupsByName {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindMusicDupsByName));
			this.label1 = new System.Windows.Forms.Label();
			this.txtDirName = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.btnGo = new System.Windows.Forms.Button();
			this.txtCurDir = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnPlay = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.lblSpaceSaved = new System.Windows.Forms.Label();
			this.tvTracks = new System.Windows.Forms.TreeView();
			this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
			this.btnStop = new System.Windows.Forms.Button();
			this.lblDuration = new System.Windows.Forms.Label();
			this.lblFileSize = new System.Windows.Forms.Label();
			this.lblArtist = new System.Windows.Forms.Label();
			this.lblNumDupSongs = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(32, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(98, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Directory to Search";
			// 
			// txtDirName
			// 
			this.txtDirName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtDirName.Location = new System.Drawing.Point(149, 19);
			this.txtDirName.Name = "txtDirName";
			this.txtDirName.Size = new System.Drawing.Size(560, 20);
			this.txtDirName.TabIndex = 1;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Location = new System.Drawing.Point(737, 17);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(75, 23);
			this.btnBrowse.TabIndex = 2;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(35, 50);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 3;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// txtCurDir
			// 
			this.txtCurDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtCurDir.Location = new System.Drawing.Point(149, 89);
			this.txtCurDir.Name = "txtCurDir";
			this.txtCurDir.Size = new System.Drawing.Size(560, 20);
			this.txtCurDir.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(32, 92);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(104, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Processing Directory";
			// 
			// btnPlay
			// 
			this.btnPlay.Location = new System.Drawing.Point(35, 123);
			this.btnPlay.Name = "btnPlay";
			this.btnPlay.Size = new System.Drawing.Size(75, 23);
			this.btnPlay.TabIndex = 6;
			this.btnPlay.Text = "Play";
			this.btnPlay.UseVisualStyleBackColor = true;
			this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Location = new System.Drawing.Point(303, 123);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(75, 23);
			this.btnDelete.TabIndex = 7;
			this.btnDelete.Text = "Delete";
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// lblSpaceSaved
			// 
			this.lblSpaceSaved.AutoSize = true;
			this.lblSpaceSaved.Location = new System.Drawing.Point(411, 128);
			this.lblSpaceSaved.Name = "lblSpaceSaved";
			this.lblSpaceSaved.Size = new System.Drawing.Size(112, 13);
			this.lblSpaceSaved.TabIndex = 8;
			this.lblSpaceSaved.Text = "Space Saved: 0 bytes";
			// 
			// tvTracks
			// 
			this.tvTracks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tvTracks.Location = new System.Drawing.Point(35, 204);
			this.tvTracks.Name = "tvTracks";
			this.tvTracks.Size = new System.Drawing.Size(777, 438);
			this.tvTracks.TabIndex = 11;
			this.tvTracks.Click += new System.EventHandler(this.tvTracks_Click);
			this.tvTracks.DoubleClick += new System.EventHandler(this.tvTracks_DoubleClick);
			// 
			// axWindowsMediaPlayer1
			// 
			this.axWindowsMediaPlayer1.Enabled = true;
			this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(737, 85);
			this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
			this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
			this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(75, 23);
			this.axWindowsMediaPlayer1.TabIndex = 12;
			this.axWindowsMediaPlayer1.Visible = false;
			// 
			// btnStop
			// 
			this.btnStop.Location = new System.Drawing.Point(149, 123);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(75, 23);
			this.btnStop.TabIndex = 13;
			this.btnStop.Text = "Stop";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// lblDuration
			// 
			this.lblDuration.AutoSize = true;
			this.lblDuration.Location = new System.Drawing.Point(32, 167);
			this.lblDuration.Name = "lblDuration";
			this.lblDuration.Size = new System.Drawing.Size(47, 13);
			this.lblDuration.TabIndex = 14;
			this.lblDuration.Text = "Duration";
			// 
			// lblFileSize
			// 
			this.lblFileSize.AutoSize = true;
			this.lblFileSize.Location = new System.Drawing.Point(146, 167);
			this.lblFileSize.Name = "lblFileSize";
			this.lblFileSize.Size = new System.Drawing.Size(46, 13);
			this.lblFileSize.TabIndex = 15;
			this.lblFileSize.Text = "File Size";
			// 
			// lblArtist
			// 
			this.lblArtist.AutoSize = true;
			this.lblArtist.Location = new System.Drawing.Point(300, 167);
			this.lblArtist.Name = "lblArtist";
			this.lblArtist.Size = new System.Drawing.Size(30, 13);
			this.lblArtist.TabIndex = 16;
			this.lblArtist.Text = "Artist";
			// 
			// lblNumDupSongs
			// 
			this.lblNumDupSongs.AutoSize = true;
			this.lblNumDupSongs.Location = new System.Drawing.Point(156, 59);
			this.lblNumDupSongs.Name = "lblNumDupSongs";
			this.lblNumDupSongs.Size = new System.Drawing.Size(10, 13);
			this.lblNumDupSongs.TabIndex = 17;
			this.lblNumDupSongs.Text = " ";
			// 
			// FindMusicDupsByName
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(824, 654);
			this.Controls.Add(this.lblNumDupSongs);
			this.Controls.Add(this.lblArtist);
			this.Controls.Add(this.lblFileSize);
			this.Controls.Add(this.lblDuration);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.axWindowsMediaPlayer1);
			this.Controls.Add(this.tvTracks);
			this.Controls.Add(this.lblSpaceSaved);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnPlay);
			this.Controls.Add(this.txtCurDir);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.txtDirName);
			this.Controls.Add(this.label1);
			this.Name = "FindMusicDupsByName";
			this.Text = "Find Music Dups By Name";
			this.Load += new System.EventHandler(this.FindMusicDupsByName_Load);
			((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtDirName;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.TextBox txtCurDir;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnPlay;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Label lblSpaceSaved;
		private System.Windows.Forms.TreeView tvTracks;
		private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.Label lblDuration;
		private System.Windows.Forms.Label lblFileSize;
		private System.Windows.Forms.Label lblArtist;
		private System.Windows.Forms.Label lblNumDupSongs;
	}
}

