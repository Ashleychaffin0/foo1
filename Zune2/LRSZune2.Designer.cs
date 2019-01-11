namespace Zune2 {
	partial class LRSZune2 {
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
			this.btnShowArtists = new System.Windows.Forms.Button();
			this.btnShowAlbums = new System.Windows.Forms.Button();
			this.btnGetKnownFolders = new System.Windows.Forms.Button();
			this.btnTryAllQueryTypes = new System.Windows.Forms.Button();
			this.btnQueryTypeAndSchemae = new System.Windows.Forms.Button();
			this.btnSaveQueriesAndSchemae = new System.Windows.Forms.Button();
			this.btnLoadQueriesAndSchemae = new System.Windows.Forms.Button();
			this.btnGetAlbumsViaClasses = new System.Windows.Forms.Button();
			this.btnFoo = new System.Windows.Forms.Button();
			this.btnGetArtistsViaClasses = new System.Windows.Forms.Button();
			this.btnGetTracksViaClasses = new System.Windows.Forms.Button();
			this.btnDumpAllZune = new System.Windows.Forms.Button();
			this.btnGetVideosViaClasses = new System.Windows.Forms.Button();
			this.btnVideosToXml = new System.Windows.Forms.Button();
			this.btnAlbumsToXml = new System.Windows.Forms.Button();
			this.btnTracksToXml = new System.Windows.Forms.Button();
			this.btnTotalPlayingTime = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnShowArtists
			// 
			this.btnShowArtists.Location = new System.Drawing.Point(12, 12);
			this.btnShowArtists.Name = "btnShowArtists";
			this.btnShowArtists.Size = new System.Drawing.Size(75, 23);
			this.btnShowArtists.TabIndex = 0;
			this.btnShowArtists.Text = "Show Artists";
			this.btnShowArtists.UseVisualStyleBackColor = true;
			this.btnShowArtists.Click += new System.EventHandler(this.btnShowArtists_Click);
			// 
			// btnShowAlbums
			// 
			this.btnShowAlbums.Location = new System.Drawing.Point(109, 12);
			this.btnShowAlbums.Name = "btnShowAlbums";
			this.btnShowAlbums.Size = new System.Drawing.Size(86, 23);
			this.btnShowAlbums.TabIndex = 1;
			this.btnShowAlbums.Text = "Show Albums";
			this.btnShowAlbums.UseVisualStyleBackColor = true;
			this.btnShowAlbums.Click += new System.EventHandler(this.btnShowAlbums_Click);
			// 
			// btnGetKnownFolders
			// 
			this.btnGetKnownFolders.Location = new System.Drawing.Point(212, 12);
			this.btnGetKnownFolders.Name = "btnGetKnownFolders";
			this.btnGetKnownFolders.Size = new System.Drawing.Size(117, 23);
			this.btnGetKnownFolders.TabIndex = 2;
			this.btnGetKnownFolders.Text = "Get Known Folders";
			this.btnGetKnownFolders.UseVisualStyleBackColor = true;
			this.btnGetKnownFolders.Click += new System.EventHandler(this.btnGetKnownFolders_Click);
			// 
			// btnTryAllQueryTypes
			// 
			this.btnTryAllQueryTypes.Location = new System.Drawing.Point(12, 58);
			this.btnTryAllQueryTypes.Name = "btnTryAllQueryTypes";
			this.btnTryAllQueryTypes.Size = new System.Drawing.Size(122, 23);
			this.btnTryAllQueryTypes.TabIndex = 3;
			this.btnTryAllQueryTypes.Text = "Try All Query Types";
			this.btnTryAllQueryTypes.UseVisualStyleBackColor = true;
			this.btnTryAllQueryTypes.Click += new System.EventHandler(this.btnTryAllQueryTypes_Click);
			// 
			// btnQueryTypeAndSchemae
			// 
			this.btnQueryTypeAndSchemae.Location = new System.Drawing.Point(162, 58);
			this.btnQueryTypeAndSchemae.Name = "btnQueryTypeAndSchemae";
			this.btnQueryTypeAndSchemae.Size = new System.Drawing.Size(151, 23);
			this.btnQueryTypeAndSchemae.TabIndex = 4;
			this.btnQueryTypeAndSchemae.Text = "Query Types and Schemae";
			this.btnQueryTypeAndSchemae.UseVisualStyleBackColor = true;
			this.btnQueryTypeAndSchemae.Click += new System.EventHandler(this.btnQueryTypeAndSchemae_Click);
			// 
			// btnSaveQueriesAndSchemae
			// 
			this.btnSaveQueriesAndSchemae.Location = new System.Drawing.Point(12, 108);
			this.btnSaveQueriesAndSchemae.Name = "btnSaveQueriesAndSchemae";
			this.btnSaveQueriesAndSchemae.Size = new System.Drawing.Size(153, 23);
			this.btnSaveQueriesAndSchemae.TabIndex = 6;
			this.btnSaveQueriesAndSchemae.Text = "Save Queries and Schemae";
			this.btnSaveQueriesAndSchemae.UseVisualStyleBackColor = true;
			this.btnSaveQueriesAndSchemae.Click += new System.EventHandler(this.btnSaveQueriesAndSchemae_Click);
			// 
			// btnLoadQueriesAndSchemae
			// 
			this.btnLoadQueriesAndSchemae.Location = new System.Drawing.Point(183, 108);
			this.btnLoadQueriesAndSchemae.Name = "btnLoadQueriesAndSchemae";
			this.btnLoadQueriesAndSchemae.Size = new System.Drawing.Size(153, 23);
			this.btnLoadQueriesAndSchemae.TabIndex = 7;
			this.btnLoadQueriesAndSchemae.Text = "Generate Classes";
			this.btnLoadQueriesAndSchemae.UseVisualStyleBackColor = true;
			this.btnLoadQueriesAndSchemae.Click += new System.EventHandler(this.btnGenerateClasses_Click);
			// 
			// btnGetAlbumsViaClasses
			// 
			this.btnGetAlbumsViaClasses.Location = new System.Drawing.Point(183, 151);
			this.btnGetAlbumsViaClasses.Name = "btnGetAlbumsViaClasses";
			this.btnGetAlbumsViaClasses.Size = new System.Drawing.Size(206, 23);
			this.btnGetAlbumsViaClasses.TabIndex = 8;
			this.btnGetAlbumsViaClasses.Text = "Get Albums Via Classes";
			this.btnGetAlbumsViaClasses.UseVisualStyleBackColor = true;
			this.btnGetAlbumsViaClasses.Click += new System.EventHandler(this.btnGetAlbumsViaClasses_Click);
			// 
			// btnFoo
			// 
			this.btnFoo.Location = new System.Drawing.Point(12, 151);
			this.btnFoo.Name = "btnFoo";
			this.btnFoo.Size = new System.Drawing.Size(122, 23);
			this.btnFoo.TabIndex = 9;
			this.btnFoo.Text = "foo";
			this.btnFoo.UseVisualStyleBackColor = true;
			this.btnFoo.Click += new System.EventHandler(this.btnFoo_Click);
			// 
			// btnGetArtistsViaClasses
			// 
			this.btnGetArtistsViaClasses.Location = new System.Drawing.Point(183, 180);
			this.btnGetArtistsViaClasses.Name = "btnGetArtistsViaClasses";
			this.btnGetArtistsViaClasses.Size = new System.Drawing.Size(206, 23);
			this.btnGetArtistsViaClasses.TabIndex = 10;
			this.btnGetArtistsViaClasses.Text = "Get Artists Via Classes";
			this.btnGetArtistsViaClasses.UseVisualStyleBackColor = true;
			this.btnGetArtistsViaClasses.Click += new System.EventHandler(this.btnGetArtistsViaClasses_Click);
			// 
			// btnGetTracksViaClasses
			// 
			this.btnGetTracksViaClasses.Location = new System.Drawing.Point(183, 209);
			this.btnGetTracksViaClasses.Name = "btnGetTracksViaClasses";
			this.btnGetTracksViaClasses.Size = new System.Drawing.Size(206, 23);
			this.btnGetTracksViaClasses.TabIndex = 11;
			this.btnGetTracksViaClasses.Text = "Get Tracks Via Classes";
			this.btnGetTracksViaClasses.UseVisualStyleBackColor = true;
			this.btnGetTracksViaClasses.Click += new System.EventHandler(this.btnGetTracksViaClasses_Click);
			// 
			// btnDumpAllZune
			// 
			this.btnDumpAllZune.Location = new System.Drawing.Point(267, 296);
			this.btnDumpAllZune.Name = "btnDumpAllZune";
			this.btnDumpAllZune.Size = new System.Drawing.Size(122, 23);
			this.btnDumpAllZune.TabIndex = 12;
			this.btnDumpAllZune.Text = "Dump All Zune";
			this.btnDumpAllZune.UseVisualStyleBackColor = true;
			this.btnDumpAllZune.Click += new System.EventHandler(this.DumpAllZune_Click);
			// 
			// btnGetVideosViaClasses
			// 
			this.btnGetVideosViaClasses.Location = new System.Drawing.Point(183, 238);
			this.btnGetVideosViaClasses.Name = "btnGetVideosViaClasses";
			this.btnGetVideosViaClasses.Size = new System.Drawing.Size(206, 23);
			this.btnGetVideosViaClasses.TabIndex = 13;
			this.btnGetVideosViaClasses.Text = "Get Videos Via Classes";
			this.btnGetVideosViaClasses.UseVisualStyleBackColor = true;
			this.btnGetVideosViaClasses.Click += new System.EventHandler(this.btnGetVideosViaClasses_Click);
			// 
			// btnVideosToXml
			// 
			this.btnVideosToXml.Location = new System.Drawing.Point(12, 296);
			this.btnVideosToXml.Name = "btnVideosToXml";
			this.btnVideosToXml.Size = new System.Drawing.Size(206, 23);
			this.btnVideosToXml.TabIndex = 14;
			this.btnVideosToXml.Text = "Videos to XML";
			this.btnVideosToXml.UseVisualStyleBackColor = true;
			this.btnVideosToXml.Click += new System.EventHandler(this.btnVideosToXml_Click);
			// 
			// btnAlbumsToXml
			// 
			this.btnAlbumsToXml.Location = new System.Drawing.Point(12, 325);
			this.btnAlbumsToXml.Name = "btnAlbumsToXml";
			this.btnAlbumsToXml.Size = new System.Drawing.Size(206, 23);
			this.btnAlbumsToXml.TabIndex = 15;
			this.btnAlbumsToXml.Text = "Albums to XML";
			this.btnAlbumsToXml.UseVisualStyleBackColor = true;
			this.btnAlbumsToXml.Click += new System.EventHandler(this.btnAlbumsToXml_Click);
			// 
			// btnTracksToXml
			// 
			this.btnTracksToXml.Location = new System.Drawing.Point(12, 354);
			this.btnTracksToXml.Name = "btnTracksToXml";
			this.btnTracksToXml.Size = new System.Drawing.Size(206, 23);
			this.btnTracksToXml.TabIndex = 16;
			this.btnTracksToXml.Text = "Tracks to XML";
			this.btnTracksToXml.UseVisualStyleBackColor = true;
			this.btnTracksToXml.Click += new System.EventHandler(this.btnTracksToXml_Click);
			// 
			// btnTotalPlayingTime
			// 
			this.btnTotalPlayingTime.Location = new System.Drawing.Point(267, 325);
			this.btnTotalPlayingTime.Name = "btnTotalPlayingTime";
			this.btnTotalPlayingTime.Size = new System.Drawing.Size(122, 23);
			this.btnTotalPlayingTime.TabIndex = 17;
			this.btnTotalPlayingTime.Text = "Total Playing Time";
			this.btnTotalPlayingTime.UseVisualStyleBackColor = true;
			this.btnTotalPlayingTime.Click += new System.EventHandler(this.btnTotalPlayingTime_Click);
			// 
			// LRSZune2
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(459, 393);
			this.Controls.Add(this.btnTotalPlayingTime);
			this.Controls.Add(this.btnTracksToXml);
			this.Controls.Add(this.btnAlbumsToXml);
			this.Controls.Add(this.btnVideosToXml);
			this.Controls.Add(this.btnGetVideosViaClasses);
			this.Controls.Add(this.btnDumpAllZune);
			this.Controls.Add(this.btnGetTracksViaClasses);
			this.Controls.Add(this.btnGetArtistsViaClasses);
			this.Controls.Add(this.btnFoo);
			this.Controls.Add(this.btnGetAlbumsViaClasses);
			this.Controls.Add(this.btnLoadQueriesAndSchemae);
			this.Controls.Add(this.btnSaveQueriesAndSchemae);
			this.Controls.Add(this.btnQueryTypeAndSchemae);
			this.Controls.Add(this.btnTryAllQueryTypes);
			this.Controls.Add(this.btnGetKnownFolders);
			this.Controls.Add(this.btnShowAlbums);
			this.Controls.Add(this.btnShowArtists);
			this.Name = "LRSZune2";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnShowArtists;
		private System.Windows.Forms.Button btnShowAlbums;
		private System.Windows.Forms.Button btnGetKnownFolders;
		private System.Windows.Forms.Button btnTryAllQueryTypes;
		private System.Windows.Forms.Button btnQueryTypeAndSchemae;
		private System.Windows.Forms.Button btnSaveQueriesAndSchemae;
		private System.Windows.Forms.Button btnLoadQueriesAndSchemae;
		private System.Windows.Forms.Button btnGetAlbumsViaClasses;
		private System.Windows.Forms.Button btnFoo;
		private System.Windows.Forms.Button btnGetArtistsViaClasses;
		private System.Windows.Forms.Button btnGetTracksViaClasses;
		private System.Windows.Forms.Button btnDumpAllZune;
		private System.Windows.Forms.Button btnGetVideosViaClasses;
		private System.Windows.Forms.Button btnVideosToXml;
		private System.Windows.Forms.Button btnAlbumsToXml;
		private System.Windows.Forms.Button btnTracksToXml;
		private System.Windows.Forms.Button btnTotalPlayingTime;
	}
}

