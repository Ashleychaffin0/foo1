using System;

namespace MovieCollection1 {
	partial class SearchOnline {
		//UserControl overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode]
		protected override void Dispose(bool disposing) {
			if (disposing && components != null) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		//Required by the Windows Form Designer
		private System.ComponentModel.IContainer components;

		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			this.dvdDataGridView = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn20 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn21 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn22 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn23 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn24 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn25 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn26 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn27 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn28 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dvdDataConnector = new System.Windows.Forms.BindingSource(this.components);
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.searchButton1 = new System.Windows.Forms.Button();
			this.searchTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.detailsLabel = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.descriptionLabel = new System.Windows.Forms.Label();
			this.releasedDateLabel = new System.Windows.Forms.Label();
			this.releasedDateLabel1 = new System.Windows.Forms.Label();
			this.titleLabel = new System.Windows.Forms.Label();
			this.titleTextBox = new System.Windows.Forms.Label();
			this.descriptionTextBox = new System.Windows.Forms.TextBox();
			this.addToCollectionButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dvdDataGridView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvdDataConnector)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// dvdDataGridView
			// 
			this.dvdDataGridView.AllowUserToAddRows = false;
			this.dvdDataGridView.AllowUserToDeleteRows = false;
			this.dvdDataGridView.AllowUserToResizeRows = false;
			this.dvdDataGridView.AutoGenerateColumns = false;
			this.dvdDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
			this.dvdDataGridView.Columns.Add(this.dataGridViewTextBoxColumn19);
			this.dvdDataGridView.Columns.Add(this.dataGridViewTextBoxColumn20);
			this.dvdDataGridView.Columns.Add(this.dataGridViewTextBoxColumn21);
			this.dvdDataGridView.Columns.Add(this.dataGridViewTextBoxColumn22);
			this.dvdDataGridView.Columns.Add(this.dataGridViewTextBoxColumn23);
			this.dvdDataGridView.Columns.Add(this.dataGridViewTextBoxColumn24);
			this.dvdDataGridView.Columns.Add(this.dataGridViewTextBoxColumn25);
			this.dvdDataGridView.Columns.Add(this.dataGridViewTextBoxColumn26);
			this.dvdDataGridView.Columns.Add(this.dataGridViewTextBoxColumn27);
			this.dvdDataGridView.Columns.Add(this.dataGridViewTextBoxColumn28);
			this.dvdDataGridView.DataSource = this.dvdDataConnector;
			this.dvdDataGridView.Location = new System.Drawing.Point(13, 158);
			this.dvdDataGridView.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
			this.dvdDataGridView.MultiSelect = false;
			this.dvdDataGridView.Name = "dvdDataGridView";
			this.dvdDataGridView.ReadOnly = true;
			this.dvdDataGridView.RowHeadersVisible = false;
			this.dvdDataGridView.EnableHeadersVisualStyles = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSkyBlue;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dvdDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dvdDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle3;
			this.dvdDataGridView.ImeMode = System.Windows.Forms.ImeMode.Disable;
			this.dvdDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dvdDataGridView.Size = new System.Drawing.Size(277, 243);
			this.dvdDataGridView.StandardTab = true;
			this.dvdDataGridView.TabIndex = 3;
			this.dvdDataGridView.DoubleClick += new System.EventHandler(this.dvdDataGridView_DoubleClick);
			// 
			// dataGridViewTextBoxColumn19
			// 
			this.dataGridViewTextBoxColumn19.DataPropertyName = "Title";
			this.dataGridViewTextBoxColumn19.HeaderText = "Title";
			this.dataGridViewTextBoxColumn19.Name = "Title";
			this.dataGridViewTextBoxColumn19.ReadOnly = true;
			this.dataGridViewTextBoxColumn19.Width = 224;
			this.dataGridViewTextBoxColumn19.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// dataGridViewTextBoxColumn20
			// 
			this.dataGridViewTextBoxColumn20.DataPropertyName = "ReleasedDate";
			this.dataGridViewTextBoxColumn20.HeaderText = "Year";
			this.dataGridViewTextBoxColumn20.Name = "ReleasedDate";
			dataGridViewCellStyle2.Format = "yyyy";
			this.dataGridViewTextBoxColumn20.DefaultCellStyle = dataGridViewCellStyle2;
			this.dataGridViewTextBoxColumn20.ReadOnly = true;
			this.dataGridViewTextBoxColumn20.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// dataGridViewTextBoxColumn21
			// 
			this.dataGridViewTextBoxColumn21.DataPropertyName = "Directors";
			this.dataGridViewTextBoxColumn21.HeaderText = "Directors";
			this.dataGridViewTextBoxColumn21.Name = "Directors";
			this.dataGridViewTextBoxColumn21.ReadOnly = true;
			this.dataGridViewTextBoxColumn21.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// dataGridViewTextBoxColumn22
			// 
			this.dataGridViewTextBoxColumn22.DataPropertyName = "Rating";
			this.dataGridViewTextBoxColumn22.HeaderText = "Rating";
			this.dataGridViewTextBoxColumn22.Name = "Rating";
			this.dataGridViewTextBoxColumn22.ReadOnly = true;
			this.dataGridViewTextBoxColumn22.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// dataGridViewTextBoxColumn23
			// 
			this.dataGridViewTextBoxColumn23.DataPropertyName = "Actors";
			this.dataGridViewTextBoxColumn23.HeaderText = "Actors";
			this.dataGridViewTextBoxColumn23.Name = "Actors";
			this.dataGridViewTextBoxColumn23.ReadOnly = true;
			this.dataGridViewTextBoxColumn23.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// dataGridViewTextBoxColumn24
			// 
			this.dataGridViewTextBoxColumn24.DataPropertyName = "RunningTime";
			this.dataGridViewTextBoxColumn24.HeaderText = "RunningTime";
			this.dataGridViewTextBoxColumn24.Name = "RunningTime";
			this.dataGridViewTextBoxColumn24.ReadOnly = true;
			this.dataGridViewTextBoxColumn24.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// dataGridViewTextBoxColumn25
			// 
			this.dataGridViewTextBoxColumn25.DataPropertyName = "Description";
			this.dataGridViewTextBoxColumn25.HeaderText = "Description";
			this.dataGridViewTextBoxColumn25.Name = "Description";
			this.dataGridViewTextBoxColumn25.ReadOnly = true;
			this.dataGridViewTextBoxColumn25.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// dataGridViewTextBoxColumn26
			// 
			this.dataGridViewTextBoxColumn26.DataPropertyName = "UPC";
			this.dataGridViewTextBoxColumn26.HeaderText = "UPC";
			this.dataGridViewTextBoxColumn26.Name = "UPC";
			this.dataGridViewTextBoxColumn26.ReadOnly = true;
			this.dataGridViewTextBoxColumn26.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// dataGridViewTextBoxColumn27
			// 
			this.dataGridViewTextBoxColumn27.DataPropertyName = "ImageUrl";
			this.dataGridViewTextBoxColumn27.HeaderText = "ImageUrl";
			this.dataGridViewTextBoxColumn27.Name = "ImageUrl";
			this.dataGridViewTextBoxColumn27.ReadOnly = true;
			this.dataGridViewTextBoxColumn27.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridViewTextBoxColumn27.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// dataGridViewTextBoxColumn28
			// 
			this.dataGridViewTextBoxColumn28.DataPropertyName = "WebPageUrl";
			this.dataGridViewTextBoxColumn28.HeaderText = "WebPageUrl";
			this.dataGridViewTextBoxColumn28.Name = "WebPageUrl";
			this.dataGridViewTextBoxColumn28.ReadOnly = true;
			this.dataGridViewTextBoxColumn28.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridViewTextBoxColumn28.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// dvdDataConnector
			// 
			this.dvdDataConnector.DataSource = typeof(MovieCollection1.Controls.DVD);
			// 
			// pictureBox2
			// 
			this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox2.BackgroundImage = global::MovieCollection1.Properties.Resources.icon_magnifyingGlass;
			this.pictureBox2.Location = new System.Drawing.Point(13, 21);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(51, 88);
			this.pictureBox2.TabIndex = 52;
			this.pictureBox2.TabStop = false;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(71, 23);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(181, 36);
			this.label1.TabIndex = 51;
			this.label1.Tag = "";
			this.label1.Text = "Please type in a DVD keyword and press Search:";
			// 
			// searchButton1
			// 
			this.searchButton1.BackColor = System.Drawing.Color.Transparent;
			this.searchButton1.BackgroundImage = global::MovieCollection1.Properties.Resources.button_smallBlue_search;
			this.searchButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.searchButton1.FlatAppearance.BorderSize = 0;
			this.searchButton1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.searchButton1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.searchButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.searchButton1.ForeColor = System.Drawing.Color.Black;
			this.searchButton1.Location = new System.Drawing.Point(218, 59);
			this.searchButton1.Name = "searchButton1";
			this.searchButton1.Size = new System.Drawing.Size(70, 32);
			this.searchButton1.TabIndex = 1;
			this.searchButton1.Text = "Search";
			this.searchButton1.UseVisualStyleBackColor = false;
			this.searchButton1.Click += new System.EventHandler(this.SearchButton_Click);
			// 
			// searchTextBox
			// 
			this.searchTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.searchTextBox.Location = new System.Drawing.Point(71, 66);
			this.searchTextBox.Name = "searchTextBox";
			this.searchTextBox.Size = new System.Drawing.Size(140, 21);
			this.searchTextBox.TabIndex = 0;
			this.searchTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SearchTextBox_KeyUp);
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.ForeColor = System.Drawing.Color.GhostWhite;
			this.label3.Location = new System.Drawing.Point(11, 120);
			this.label3.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(264, 32);
			this.label3.TabIndex = 47;
			this.label3.Tag = "";
			this.label3.Text = "Select a Title and optionally press Add to collection:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.GhostWhite;
			this.label2.Location = new System.Drawing.Point(13, 411);
			this.label2.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(97, 15);
			this.label2.TabIndex = 48;
			this.label2.Text = "0 results found.";
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.Transparent;
			this.panel2.BackgroundImage = global::MovieCollection1.Properties.Resources.panel_dvdInfo_noTxt;
			this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.panel2.Controls.Add(this.detailsLabel);
			this.panel2.Controls.Add(this.pictureBox1);
			this.panel2.Controls.Add(this.descriptionLabel);
			this.panel2.Controls.Add(this.releasedDateLabel);
			this.panel2.Controls.Add(this.releasedDateLabel1);
			this.panel2.Controls.Add(this.titleLabel);
			this.panel2.Controls.Add(this.titleTextBox);
			this.panel2.Controls.Add(this.descriptionTextBox);
			this.panel2.Location = new System.Drawing.Point(320, 64);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(577, 414);
			this.panel2.TabIndex = 3;
			// 
			// detailsLabel
			// 
			this.detailsLabel.AutoSize = true;
			this.detailsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.detailsLabel.ForeColor = System.Drawing.Color.DarkGray;
			this.detailsLabel.Location = new System.Drawing.Point(16, 27);
			this.detailsLabel.Name = "detailsLabel";
			this.detailsLabel.Size = new System.Drawing.Size(109, 24);
			this.detailsLabel.TabIndex = 55;
			this.detailsLabel.Text = "dvd details";
			// 
			// pictureBox1
			// 
			this.pictureBox1.DataBindings.Add(new System.Windows.Forms.Binding("ImageLocation", this.dvdDataConnector, "ImageUrl", true));
			this.pictureBox1.Location = new System.Drawing.Point(323, 90);
			this.pictureBox1.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(230, 275);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 42;
			this.pictureBox1.TabStop = false;
			// 
			// descriptionLabel
			// 
			this.descriptionLabel.AutoSize = true;
			this.descriptionLabel.BackColor = System.Drawing.Color.Transparent;
			this.descriptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.descriptionLabel.ForeColor = System.Drawing.Color.MediumTurquoise;
			this.descriptionLabel.Location = new System.Drawing.Point(22, 193);
			this.descriptionLabel.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
			this.descriptionLabel.Name = "descriptionLabel";
			this.descriptionLabel.Size = new System.Drawing.Size(77, 15);
			this.descriptionLabel.TabIndex = 40;
			this.descriptionLabel.Text = "Description:";
			// 
			// releasedDateLabel
			// 
			this.releasedDateLabel.AutoSize = true;
			this.releasedDateLabel.BackColor = System.Drawing.Color.Transparent;
			this.releasedDateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.releasedDateLabel.ForeColor = System.Drawing.Color.MediumTurquoise;
			this.releasedDateLabel.Location = new System.Drawing.Point(23, 136);
			this.releasedDateLabel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
			this.releasedDateLabel.Name = "releasedDateLabel";
			this.releasedDateLabel.Size = new System.Drawing.Size(35, 15);
			this.releasedDateLabel.TabIndex = 38;
			this.releasedDateLabel.Text = "Year:";
			// 
			// releasedDateLabel1
			// 
			this.releasedDateLabel1.BackColor = System.Drawing.Color.Transparent;
			this.releasedDateLabel1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dvdDataConnector, "ReleasedDate", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, "", "yyyy"));
			this.releasedDateLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.releasedDateLabel1.ForeColor = System.Drawing.Color.White;
			this.releasedDateLabel1.Location = new System.Drawing.Point(74, 161);
			this.releasedDateLabel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 1);
			this.releasedDateLabel1.Name = "releasedDateLabel1";
			this.releasedDateLabel1.Size = new System.Drawing.Size(180, 29);
			this.releasedDateLabel1.TabIndex = 39;
			// 
			// titleLabel
			// 
			this.titleLabel.AutoSize = true;
			this.titleLabel.BackColor = System.Drawing.Color.Transparent;
			this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.titleLabel.ForeColor = System.Drawing.Color.MediumTurquoise;
			this.titleLabel.Location = new System.Drawing.Point(24, 62);
			this.titleLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
			this.titleLabel.Name = "titleLabel";
			this.titleLabel.Size = new System.Drawing.Size(34, 15);
			this.titleLabel.TabIndex = 36;
			this.titleLabel.Text = "Title:";
			// 
			// titleTextBox
			// 
			this.titleTextBox.BackColor = System.Drawing.Color.Transparent;
			this.titleTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dvdDataConnector, "Title", true));
			this.titleTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.titleTextBox.ForeColor = System.Drawing.Color.White;
			this.titleTextBox.Location = new System.Drawing.Point(74, 75);
			this.titleTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 1, 2);
			this.titleTextBox.Name = "titleTextBox";
			this.titleTextBox.Size = new System.Drawing.Size(216, 59);
			this.titleTextBox.TabIndex = 37;
			// 
			// descriptionTextBox
			// 
			this.descriptionTextBox.AutoSize = false;
			this.descriptionTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dvdDataConnector, "Description", true));
			this.descriptionTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.descriptionTextBox.Location = new System.Drawing.Point(23, 218);
			this.descriptionTextBox.Multiline = true;
			this.descriptionTextBox.Name = "descriptionTextBox";
			this.descriptionTextBox.ReadOnly = true;
			this.descriptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.descriptionTextBox.Size = new System.Drawing.Size(266, 169);
			this.descriptionTextBox.TabIndex = 41;
			this.descriptionTextBox.TabStop = false;
			// 
			// addToCollectionButton
			// 
			this.addToCollectionButton.BackColor = System.Drawing.Color.Transparent;
			this.addToCollectionButton.BackgroundImage = global::MovieCollection1.Properties.Resources.button_addCollection;
			this.addToCollectionButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.addToCollectionButton.FlatAppearance.BorderSize = 0;
			this.addToCollectionButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.addToCollectionButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.addToCollectionButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.addToCollectionButton.ForeColor = System.Drawing.Color.Black;
			this.addToCollectionButton.Location = new System.Drawing.Point(13, 436);
			this.addToCollectionButton.Name = "addToCollectionButton";
			this.addToCollectionButton.Size = new System.Drawing.Size(149, 36);
			this.addToCollectionButton.TabIndex = 4;
			this.addToCollectionButton.Text = "Add to collection";
			this.addToCollectionButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.addToCollectionButton.UseVisualStyleBackColor = false;
			this.addToCollectionButton.Click += new System.EventHandler(this.AddToCollectionButton_Click);
			// 
			// SearchOnline
			// 
			this.BackColor = System.Drawing.SystemColors.Control;
			this.BackgroundImage = global::MovieCollection1.Properties.Resources.PanelBG_Tile;
			this.Controls.Add(this.addToCollectionButton);
			this.Controls.Add(this.pictureBox2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.searchButton1);
			this.Controls.Add(this.searchTextBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.dvdDataGridView);
			this.Controls.Add(this.panel2);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "SearchOnline";
			this.Size = new System.Drawing.Size(900, 500);
			((System.ComponentModel.ISupportInitialize)(this.dvdDataGridView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvdDataConnector)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button searchButton1;
		private System.Windows.Forms.TextBox searchTextBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.DataGridView dvdDataGridView;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label descriptionLabel;
		private System.Windows.Forms.Label releasedDateLabel;
		private System.Windows.Forms.Label releasedDateLabel1;
		private System.Windows.Forms.Label titleLabel;
		private System.Windows.Forms.Label titleTextBox;
		private System.Windows.Forms.TextBox descriptionTextBox;
		private System.Windows.Forms.Button addToCollectionButton;
		private System.Windows.Forms.BindingSource dvdDataConnector;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn20;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn21;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn22;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn23;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn24;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn25;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn26;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn27;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn28;
		private System.Windows.Forms.Label detailsLabel;
	}
}
