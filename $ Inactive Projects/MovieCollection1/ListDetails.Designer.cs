using System;

namespace MovieCollection1 {
	partial class ListDetails {
		[System.Diagnostics.DebuggerNonUserCode]
		public ListDetails()
			: base() {

			//This call is required by the Windows Form Designer.
			InitializeComponent();

		}

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
		[System.Diagnostics.DebuggerStepThrough]
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			this.dvdDetailsPanel = new System.Windows.Forms.Panel();
			this.importButton = new System.Windows.Forms.Button();
			this.detailsLabel = new System.Windows.Forms.Label();
			this.boxArtPictureBox = new System.Windows.Forms.PictureBox();
			this.dvdsDataConnector = new System.Windows.Forms.BindingSource(this.components);
			this.dvdCollectionDataSet = new MovieCollection1.DVDCollectionDataSet();
			this.minutesLabel = new System.Windows.Forms.Label();
			this.yearDisplay = new System.Windows.Forms.Label();
			this.titleDisplay = new System.Windows.Forms.Label();
			this.myRatingLabel = new System.Windows.Forms.Label();
			this.upcLabel = new System.Windows.Forms.Label();
			this.upcTextBox = new System.Windows.Forms.TextBox();
			this.ratedLabel = new System.Windows.Forms.Label();
			this.lengthLabel = new System.Windows.Forms.Label();
			this.directorLabel = new System.Windows.Forms.Label();
			this.directorTextBox = new System.Windows.Forms.TextBox();
			this.commentsLabel = new System.Windows.Forms.Label();
			this.commentsTextBox = new System.Windows.Forms.TextBox();
			this.descriptionLabel = new System.Windows.Forms.Label();
			this.descriptionTextBox = new System.Windows.Forms.TextBox();
			this.titleLabel = new System.Windows.Forms.Label();
			this.titleTextBox = new System.Windows.Forms.TextBox();
			this.yearReleasedDateTimePicker = new System.Windows.Forms.TextBox();
			this.ratedComboBox = new System.Windows.Forms.ComboBox();
			this.lengthNumericUpDown = new System.Windows.Forms.TextBox();
			this.yearReleasedTextbox = new System.Windows.Forms.TextBox();
			this.yearReleasedLabel1 = new System.Windows.Forms.Label();
			this.viewOnlineButton = new System.Windows.Forms.Button();
			this.refreshOnlineButton = new System.Windows.Forms.Button();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.title = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.yearReleased = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.director = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.myRating = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.rated = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.actors = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.length = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.comments = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.upc = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.imageLink = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.webPageLink = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.genre = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.removeButton = new System.Windows.Forms.Button();
			this.addTitleButton = new System.Windows.Forms.Button();
			this.lookLabel = new System.Windows.Forms.Label();
			this.filterTextBox = new System.Windows.Forms.TextBox();
			this.showAllImage = new System.Windows.Forms.Button();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.filterButton = new System.Windows.Forms.Button();
			this.showAllButton = new System.Windows.Forms.Button();
			this.dvdsTableAdapter = new MovieCollection1.DVDCollectionDataSetTableAdapters.DVDsTableAdapter();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.dvdDetailsPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.boxArtPictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvdsDataConnector)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvdCollectionDataSet)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			this.SuspendLayout();
			// 
			// dvdDetailsPanel
			// 
			this.dvdDetailsPanel.BackColor = System.Drawing.Color.Transparent;
			this.dvdDetailsPanel.BackgroundImage = global::MovieCollection1.Properties.Resources.panel_dvdInfo_noTxt;
			this.dvdDetailsPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.dvdDetailsPanel.Controls.Add(this.importButton);
			this.dvdDetailsPanel.Controls.Add(this.detailsLabel);
			this.dvdDetailsPanel.Controls.Add(this.yearReleasedLabel1);
			this.dvdDetailsPanel.Controls.Add(this.boxArtPictureBox);
			this.dvdDetailsPanel.Controls.Add(this.minutesLabel);
			this.dvdDetailsPanel.Controls.Add(this.yearDisplay);
			this.dvdDetailsPanel.Controls.Add(this.titleDisplay);
			this.dvdDetailsPanel.Controls.Add(this.myRatingLabel);
			this.dvdDetailsPanel.Controls.Add(this.upcLabel);
			this.dvdDetailsPanel.Controls.Add(this.upcTextBox);
			this.dvdDetailsPanel.Controls.Add(this.ratedLabel);
			this.dvdDetailsPanel.Controls.Add(this.lengthLabel);
			this.dvdDetailsPanel.Controls.Add(this.directorLabel);
			this.dvdDetailsPanel.Controls.Add(this.directorTextBox);
			this.dvdDetailsPanel.Controls.Add(this.commentsLabel);
			this.dvdDetailsPanel.Controls.Add(this.commentsTextBox);
			this.dvdDetailsPanel.Controls.Add(this.descriptionLabel);
			this.dvdDetailsPanel.Controls.Add(this.descriptionTextBox);
			this.dvdDetailsPanel.Controls.Add(this.titleLabel);
			this.dvdDetailsPanel.Controls.Add(this.titleTextBox);
			this.dvdDetailsPanel.Controls.Add(this.yearReleasedDateTimePicker);
			this.dvdDetailsPanel.Controls.Add(this.ratedComboBox);
			this.dvdDetailsPanel.Controls.Add(this.lengthNumericUpDown);
			this.dvdDetailsPanel.Controls.Add(this.yearReleasedTextbox);
			this.dvdDetailsPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dvdDetailsPanel.ForeColor = System.Drawing.Color.White;
			this.dvdDetailsPanel.Location = new System.Drawing.Point(320, 64);
			this.dvdDetailsPanel.Name = "dvdDetailsPanel";
			this.dvdDetailsPanel.Size = new System.Drawing.Size(577, 414);
			this.dvdDetailsPanel.TabIndex = 4;
			// 
			// importButton
			// 
			this.importButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.importButton.BackColor = System.Drawing.Color.Transparent;
			this.importButton.BackgroundImage = global::MovieCollection1.Properties.Resources.button_smallBlue_search;
			this.importButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.importButton.FlatAppearance.BorderSize = 0;
			this.importButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.importButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.importButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.importButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.importButton.ForeColor = System.Drawing.Color.Black;
			this.importButton.Location = new System.Drawing.Point(45, 335);
			this.importButton.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
			this.importButton.Name = "importButton";
			this.importButton.Size = new System.Drawing.Size(150, 23);
			this.importButton.TabIndex = 8;
			this.importButton.Text = "Import image";
			this.importButton.UseVisualStyleBackColor = false;
			this.importButton.Click += new System.EventHandler(this.ImportButton_Click);
			// 
			// detailsLabel
			// 
			this.detailsLabel.AutoSize = true;
			this.detailsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.detailsLabel.ForeColor = System.Drawing.Color.DarkGray;
			this.detailsLabel.Location = new System.Drawing.Point(16, 27);
			this.detailsLabel.Name = "detailsLabel";
			this.detailsLabel.Size = new System.Drawing.Size(110, 24);
			this.detailsLabel.TabIndex = 80;
			this.detailsLabel.Text = "dvd details";
			// 
			// boxArtPictureBox
			// 
			this.boxArtPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.boxArtPictureBox.DataBindings.Add(new System.Windows.Forms.Binding("Image", this.dvdsDataConnector, "ImageBinary", true, System.Windows.Forms.DataSourceUpdateMode.Never));
			this.boxArtPictureBox.Location = new System.Drawing.Point(19, 123);
			this.boxArtPictureBox.Margin = new System.Windows.Forms.Padding(3, 1, 3, 2);
			this.boxArtPictureBox.Name = "boxArtPictureBox";
			this.boxArtPictureBox.Size = new System.Drawing.Size(196, 208);
			this.boxArtPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.boxArtPictureBox.TabIndex = 37;
			this.boxArtPictureBox.TabStop = false;
			// 
			// dvdsDataConnector
			// 
			this.dvdsDataConnector.DataMember = "DVDs";
			this.dvdsDataConnector.DataSource = this.dvdCollectionDataSet;
			// 
			// dvdCollectionDataSet
			// 
			this.dvdCollectionDataSet.DataSetName = "DVDCollectionDataSet";
			this.dvdCollectionDataSet.Locale = new System.Globalization.CultureInfo("en-US");
			// 
			// minutesLabel
			// 
			this.minutesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.minutesLabel.AutoSize = true;
			this.minutesLabel.BackColor = System.Drawing.Color.DimGray;
			this.minutesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.minutesLabel.ForeColor = System.Drawing.Color.White;
			this.minutesLabel.Location = new System.Drawing.Point(486, 208);
			this.minutesLabel.Name = "minutesLabel";
			this.minutesLabel.Size = new System.Drawing.Size(38, 15);
			this.minutesLabel.TabIndex = 46;
			this.minutesLabel.Text = "mins";
			// 
			// yearDisplay
			// 
			this.yearDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.yearDisplay.AutoSize = true;
			this.yearDisplay.BackColor = System.Drawing.Color.DimGray;
			this.yearDisplay.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dvdsDataConnector, "YearReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, "", "yyyy"));
			this.yearDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.yearDisplay.ForeColor = System.Drawing.Color.White;
			this.yearDisplay.Location = new System.Drawing.Point(19, 95);
			this.yearDisplay.Name = "yearDisplay";
			this.yearDisplay.Size = new System.Drawing.Size(57, 20);
			this.yearDisplay.TabIndex = 38;
			this.yearDisplay.Text = "[Year]";
			// 
			// titleDisplay
			// 
			this.titleDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.titleDisplay.AutoSize = true;
			this.titleDisplay.BackColor = System.Drawing.Color.DimGray;
			this.titleDisplay.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dvdsDataConnector, "Title", true));
			this.titleDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.titleDisplay.ForeColor = System.Drawing.Color.White;
			this.titleDisplay.Location = new System.Drawing.Point(19, 64);
			this.titleDisplay.Name = "titleDisplay";
			this.titleDisplay.Size = new System.Drawing.Size(55, 24);
			this.titleDisplay.TabIndex = 36;
			this.titleDisplay.Text = "[Title]";
			// 
			// myRatingLabel
			// 
			this.myRatingLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.myRatingLabel.AutoSize = true;
			this.myRatingLabel.BackColor = System.Drawing.Color.DimGray;
			this.myRatingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.myRatingLabel.ForeColor = System.Drawing.Color.White;
			this.myRatingLabel.Location = new System.Drawing.Point(19, 388);
			this.myRatingLabel.Name = "myRatingLabel";
			this.myRatingLabel.Size = new System.Drawing.Size(75, 15);
			this.myRatingLabel.TabIndex = 48;
			this.myRatingLabel.Text = "My Rating:";
			// 
			// upcLabel
			// 
			this.upcLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.upcLabel.AutoSize = true;
			this.upcLabel.BackColor = System.Drawing.Color.DimGray;
			this.upcLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.upcLabel.ForeColor = System.Drawing.Color.White;
			this.upcLabel.Location = new System.Drawing.Point(19, 365);
			this.upcLabel.Name = "upcLabel";
			this.upcLabel.Size = new System.Drawing.Size(39, 15);
			this.upcLabel.TabIndex = 45;
			this.upcLabel.Text = "UPC:";
			// 
			// upcTextBox
			// 
			this.upcTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.upcTextBox.BackColor = System.Drawing.Color.White;
			this.upcTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dvdsDataConnector, "UPC", true));
			this.upcTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.upcTextBox.Location = new System.Drawing.Point(128, 363);
			this.upcTextBox.Name = "upcTextBox";
			this.upcTextBox.Size = new System.Drawing.Size(87, 21);
			this.upcTextBox.TabIndex = 7;
			// 
			// ratedLabel
			// 
			this.ratedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ratedLabel.AutoSize = true;
			this.ratedLabel.BackColor = System.Drawing.Color.DimGray;
			this.ratedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ratedLabel.ForeColor = System.Drawing.Color.White;
			this.ratedLabel.Location = new System.Drawing.Point(440, 133);
			this.ratedLabel.Name = "ratedLabel";
			this.ratedLabel.Size = new System.Drawing.Size(49, 15);
			this.ratedLabel.TabIndex = 42;
			this.ratedLabel.Text = "Rated:";
			// 
			// lengthLabel
			// 
			this.lengthLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lengthLabel.AutoSize = true;
			this.lengthLabel.BackColor = System.Drawing.Color.DimGray;
			this.lengthLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lengthLabel.ForeColor = System.Drawing.Color.White;
			this.lengthLabel.Location = new System.Drawing.Point(440, 181);
			this.lengthLabel.Name = "lengthLabel";
			this.lengthLabel.Size = new System.Drawing.Size(55, 15);
			this.lengthLabel.TabIndex = 44;
			this.lengthLabel.Text = "Length:";
			// 
			// directorLabel
			// 
			this.directorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.directorLabel.AutoSize = true;
			this.directorLabel.BackColor = System.Drawing.Color.DimGray;
			this.directorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.directorLabel.ForeColor = System.Drawing.Color.White;
			this.directorLabel.Location = new System.Drawing.Point(284, 133);
			this.directorLabel.Name = "directorLabel";
			this.directorLabel.Size = new System.Drawing.Size(62, 15);
			this.directorLabel.TabIndex = 41;
			this.directorLabel.Text = "Director:";
			// 
			// directorTextBox
			// 
			this.directorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.directorTextBox.BackColor = System.Drawing.Color.White;
			this.directorTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dvdsDataConnector, "Director", true));
			this.directorTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.directorTextBox.Location = new System.Drawing.Point(284, 154);
			this.directorTextBox.Name = "directorTextBox";
			this.directorTextBox.Size = new System.Drawing.Size(132, 21);
			this.directorTextBox.TabIndex = 1;
			// 
			// commentsLabel
			// 
			this.commentsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.commentsLabel.AutoSize = true;
			this.commentsLabel.BackColor = System.Drawing.Color.DimGray;
			this.commentsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.commentsLabel.ForeColor = System.Drawing.Color.White;
			this.commentsLabel.Location = new System.Drawing.Point(284, 181);
			this.commentsLabel.Name = "commentsLabel";
			this.commentsLabel.Size = new System.Drawing.Size(68, 15);
			this.commentsLabel.TabIndex = 43;
			this.commentsLabel.Text = "Comments";
			// 
			// commentsTextBox
			// 
			this.commentsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.commentsTextBox.BackColor = System.Drawing.Color.White;
			this.commentsTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dvdsDataConnector, "Comments", true));
			this.commentsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.commentsTextBox.Location = new System.Drawing.Point(284, 202);
			this.commentsTextBox.Multiline = true;
			this.commentsTextBox.Name = "commentsTextBox";
			this.commentsTextBox.Size = new System.Drawing.Size(132, 67);
			this.commentsTextBox.TabIndex = 3;
			// 
			// descriptionLabel
			// 
			this.descriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.descriptionLabel.AutoSize = true;
			this.descriptionLabel.BackColor = System.Drawing.Color.DimGray;
			this.descriptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.descriptionLabel.ForeColor = System.Drawing.Color.White;
			this.descriptionLabel.Location = new System.Drawing.Point(284, 278);
			this.descriptionLabel.Name = "descriptionLabel";
			this.descriptionLabel.Size = new System.Drawing.Size(72, 15);
			this.descriptionLabel.TabIndex = 49;
			this.descriptionLabel.Text = "Description";
			// 
			// descriptionTextBox
			// 
			this.descriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.descriptionTextBox.BackColor = System.Drawing.Color.White;
			this.descriptionTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dvdsDataConnector, "Description", true));
			this.descriptionTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.descriptionTextBox.Location = new System.Drawing.Point(284, 299);
			this.descriptionTextBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.descriptionTextBox.Multiline = true;
			this.descriptionTextBox.Name = "descriptionTextBox";
			this.descriptionTextBox.Size = new System.Drawing.Size(208, 89);
			this.descriptionTextBox.TabIndex = 6;
			// 
			// titleLabel
			// 
			this.titleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.titleLabel.AutoSize = true;
			this.titleLabel.BackColor = System.Drawing.Color.DimGray;
			this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.titleLabel.ForeColor = System.Drawing.Color.White;
			this.titleLabel.Location = new System.Drawing.Point(284, 86);
			this.titleLabel.Name = "titleLabel";
			this.titleLabel.Size = new System.Drawing.Size(39, 15);
			this.titleLabel.TabIndex = 40;
			this.titleLabel.Text = "Title:";
			// 
			// titleTextBox
			// 
			this.titleTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.titleTextBox.BackColor = System.Drawing.Color.White;
			this.titleTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dvdsDataConnector, "Title", true));
			this.titleTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.titleTextBox.Location = new System.Drawing.Point(284, 107);
			this.titleTextBox.Name = "titleTextBox";
			this.titleTextBox.Size = new System.Drawing.Size(207, 21);
			this.titleTextBox.TabIndex = 0;
			this.titleTextBox.Click += new System.EventHandler(this.TitleTextBox_Click);
			// 
			// yearReleasedDateTimePicker
			// 
			this.yearReleasedDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.yearReleasedDateTimePicker.Location = new System.Drawing.Point(284, 154);
			this.yearReleasedDateTimePicker.Name = "yearReleasedDateTimePicker";
			this.yearReleasedDateTimePicker.Size = new System.Drawing.Size(50, 21);
			this.yearReleasedDateTimePicker.TabIndex = 5;
			// 
			// ratedComboBox
			// 
			this.ratedComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ratedComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dvdsDataConnector, "Rated", true));
			this.ratedComboBox.DropDownHeight = 120;
			this.ratedComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ratedComboBox.FormattingEnabled = true;
			this.ratedComboBox.IntegralHeight = false;
			this.ratedComboBox.Items.AddRange(new object[] {
            "G",
            "PG",
            "PG-13",
            "R",
            "NC-17",
            "NR"});
			this.ratedComboBox.Location = new System.Drawing.Point(440, 154);
			this.ratedComboBox.Name = "ratedComboBox";
			this.ratedComboBox.Size = new System.Drawing.Size(69, 23);
			this.ratedComboBox.TabIndex = 2;
			// 
			// lengthNumericUpDown
			// 
			this.lengthNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lengthNumericUpDown.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dvdsDataConnector, "Length", true));
			this.lengthNumericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lengthNumericUpDown.Location = new System.Drawing.Point(440, 202);
			this.lengthNumericUpDown.Name = "lengthNumericUpDown";
			this.lengthNumericUpDown.Size = new System.Drawing.Size(36, 21);
			this.lengthNumericUpDown.TabIndex = 4;
			// 
			// yearReleasedTextbox
			// 
			this.yearReleasedTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.yearReleasedTextbox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dvdsDataConnector, "YearReleased", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, "", "yyyy"));
			this.yearReleasedTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.yearReleasedTextbox.Location = new System.Drawing.Point(440, 250);
			this.yearReleasedTextbox.Name = "yearReleasedTextbox";
			this.yearReleasedTextbox.Size = new System.Drawing.Size(69, 21);
			this.yearReleasedTextbox.TabIndex = 5;
			// 
			// yearReleasedLabel1
			// 
			this.yearReleasedLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.yearReleasedLabel1.AutoSize = true;
			this.yearReleasedLabel1.BackColor = System.Drawing.Color.DimGray;
			this.yearReleasedLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.yearReleasedLabel1.ForeColor = System.Drawing.Color.White;
			this.yearReleasedLabel1.Location = new System.Drawing.Point(440, 229);
			this.yearReleasedLabel1.Name = "yearReleasedLabel1";
			this.yearReleasedLabel1.Size = new System.Drawing.Size(105, 15);
			this.yearReleasedLabel1.TabIndex = 47;
			this.yearReleasedLabel1.Text = "Year Released:";
			// 
			// viewOnlineButton
			// 
			this.viewOnlineButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.viewOnlineButton.BackColor = System.Drawing.Color.Transparent;
			this.viewOnlineButton.BackgroundImage = global::MovieCollection1.Properties.Resources.button_smallBlue_search;
			this.viewOnlineButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.viewOnlineButton.FlatAppearance.BorderSize = 0;
			this.viewOnlineButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.viewOnlineButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.viewOnlineButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.viewOnlineButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.viewOnlineButton.ForeColor = System.Drawing.Color.Black;
			this.viewOnlineButton.Location = new System.Drawing.Point(119, 446);
			this.viewOnlineButton.Name = "viewOnlineButton";
			this.viewOnlineButton.Size = new System.Drawing.Size(107, 32);
			this.viewOnlineButton.TabIndex = 8;
			this.viewOnlineButton.Text = "View Online";
			this.viewOnlineButton.UseVisualStyleBackColor = false;
			this.viewOnlineButton.Click += new System.EventHandler(this.ViewOnlineButton_Click);
			// 
			// refreshOnlineButton
			// 
			this.refreshOnlineButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.refreshOnlineButton.BackColor = System.Drawing.Color.Transparent;
			this.refreshOnlineButton.BackgroundImage = global::MovieCollection1.Properties.Resources.button_smallBlue_search;
			this.refreshOnlineButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.refreshOnlineButton.FlatAppearance.BorderSize = 0;
			this.refreshOnlineButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.refreshOnlineButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.refreshOnlineButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.refreshOnlineButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.refreshOnlineButton.ForeColor = System.Drawing.Color.Black;
			this.refreshOnlineButton.Location = new System.Drawing.Point(13, 446);
			this.refreshOnlineButton.Name = "refreshOnlineButton";
			this.refreshOnlineButton.Size = new System.Drawing.Size(99, 32);
			this.refreshOnlineButton.TabIndex = 7;
			this.refreshOnlineButton.Text = "Refresh";
			this.refreshOnlineButton.UseVisualStyleBackColor = false;
			this.refreshOnlineButton.Click += new System.EventHandler(this.RefreshOnlineButton_Click);
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToResizeRows = false;
			this.dataGridView1.AutoGenerateColumns = false;
			this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
			this.dataGridView1.EnableHeadersVisualStyles = false;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSkyBlue;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dataGridView1.Columns.Add(this.title);
			this.dataGridView1.Columns.Add(this.yearReleased);
			this.dataGridView1.Columns.Add(this.director);
			this.dataGridView1.Columns.Add(this.myRating);
			this.dataGridView1.Columns.Add(this.rated);
			this.dataGridView1.Columns.Add(this.actors);
			this.dataGridView1.Columns.Add(this.length);
			this.dataGridView1.Columns.Add(this.description);
			this.dataGridView1.Columns.Add(this.comments);
			this.dataGridView1.Columns.Add(this.upc);
			this.dataGridView1.Columns.Add(this.imageLink);
			this.dataGridView1.Columns.Add(this.webPageLink);
			this.dataGridView1.Columns.Add(this.id);
			this.dataGridView1.Columns.Add(this.genre);
			this.dataGridView1.DataSource = this.dvdsDataConnector;
			this.dataGridView1.Location = new System.Drawing.Point(11, 151);
			this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
			this.dataGridView1.MultiSelect = false;
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.RowHeadersVisible = false;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle2;
			this.dataGridView1.ImeMode = System.Windows.Forms.ImeMode.Disable;
			this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridView1.Size = new System.Drawing.Size(277, 243);
			this.dataGridView1.StandardTab = true;
			this.dataGridView1.TabIndex = 3;
			// 
			// title
			// 
			this.title.DataPropertyName = "Title";
			this.title.HeaderText = "Title";
			this.title.Name = "Title";
			this.title.ReadOnly = true;
			this.title.Width = 224;
			this.title.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// yearReleased
			// 
			this.yearReleased.DataPropertyName = "YearReleased";
			this.yearReleased.HeaderText = "Year";
			this.yearReleased.Name = "YearReleased";
			dataGridViewCellStyle3.Format = "yyyy";
			this.yearReleased.DefaultCellStyle = dataGridViewCellStyle3;
			this.yearReleased.ReadOnly = true;
			this.yearReleased.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// director
			// 
			this.director.DataPropertyName = "Director";
			this.director.HeaderText = "Director";
			this.director.Name = "Director";
			this.director.ReadOnly = true;
			this.director.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// myRating
			// 
			this.myRating.DataPropertyName = "MyRating";
			this.myRating.HeaderText = "MyRating";
			this.myRating.Name = "MyRating";
			this.myRating.ReadOnly = true;
			this.myRating.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// rated
			// 
			this.rated.DataPropertyName = "Rated";
			this.rated.HeaderText = "Rated";
			this.rated.Name = "Rated";
			this.rated.ReadOnly = true;
			this.rated.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// actors
			// 
			this.actors.DataPropertyName = "Actors";
			this.actors.HeaderText = "Actors";
			this.actors.Name = "Actors";
			this.actors.ReadOnly = true;
			this.actors.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// length
			// 
			this.length.DataPropertyName = "Length";
			this.length.HeaderText = "Length";
			this.length.Name = "Length";
			this.length.ReadOnly = true;
			this.length.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// description
			// 
			this.description.DataPropertyName = "Description";
			this.description.HeaderText = "Description";
			this.description.Name = "Description";
			this.description.ReadOnly = true;
			this.description.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// comments
			// 
			this.comments.DataPropertyName = "Comments";
			this.comments.HeaderText = "Comments";
			this.comments.Name = "Comments";
			this.comments.ReadOnly = true;
			this.comments.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// upc
			// 
			this.upc.DataPropertyName = "UPC";
			this.upc.HeaderText = "UPC";
			this.upc.Name = "UPC";
			this.upc.ReadOnly = true;
			this.upc.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// imageLink
			// 
			this.imageLink.DataPropertyName = "ImageLink";
			this.imageLink.HeaderText = "ImageLink";
			this.imageLink.Name = "ImageLink";
			this.imageLink.ReadOnly = true;
			this.imageLink.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// webPageLink
			// 
			this.webPageLink.DataPropertyName = "WebPageLink";
			this.webPageLink.HeaderText = "WebPageLink";
			this.webPageLink.Name = "WebPageLink";
			this.webPageLink.ReadOnly = true;
			this.webPageLink.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// id
			// 
			this.id.DataPropertyName = "ID";
			this.id.HeaderText = "ID";
			this.id.Name = "ID";
			this.id.ReadOnly = true;
			this.id.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// genre
			// 
			this.genre.DataPropertyName = "Genre";
			this.genre.HeaderText = "Genre";
			this.genre.Name = "Genre";
			this.genre.ReadOnly = true;
			this.genre.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// removeButton
			// 
			this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.removeButton.BackColor = System.Drawing.Color.Transparent;
			this.removeButton.BackgroundImage = global::MovieCollection1.Properties.Resources.button_smallBlue_search;
			this.removeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.removeButton.FlatAppearance.BorderSize = 0;
			this.removeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.removeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.removeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.removeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.removeButton.ForeColor = System.Drawing.Color.Black;
			this.removeButton.Location = new System.Drawing.Point(119, 407);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size(107, 32);
			this.removeButton.TabIndex = 6;
			this.removeButton.Text = "Remove";
			this.removeButton.UseVisualStyleBackColor = false;
			this.removeButton.Click += new System.EventHandler(this.RemoveButton_Click);
			// 
			// addTitleButton
			// 
			this.addTitleButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.addTitleButton.BackColor = System.Drawing.Color.Transparent;
			this.addTitleButton.BackgroundImage = global::MovieCollection1.Properties.Resources.button_smallBlue_search;
			this.addTitleButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.addTitleButton.FlatAppearance.BorderSize = 0;
			this.addTitleButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.addTitleButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.addTitleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.addTitleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.addTitleButton.ForeColor = System.Drawing.Color.Black;
			this.addTitleButton.Location = new System.Drawing.Point(13, 407);
			this.addTitleButton.Name = "addTitleButton";
			this.addTitleButton.Size = new System.Drawing.Size(99, 32);
			this.addTitleButton.TabIndex = 5;
			this.addTitleButton.Text = "Add Title";
			this.addTitleButton.UseVisualStyleBackColor = false;
			this.addTitleButton.Click += new System.EventHandler(this.AddTitleButton_Click);
			// 
			// lookLabel
			// 
			this.lookLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lookLabel.AutoSize = true;
			this.lookLabel.BackColor = System.Drawing.Color.Transparent;
			this.lookLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lookLabel.ForeColor = System.Drawing.Color.White;
			this.lookLabel.Location = new System.Drawing.Point(71, 39);
			this.lookLabel.Name = "lookLabel";
			this.lookLabel.Size = new System.Drawing.Size(155, 15);
			this.lookLabel.TabIndex = 30;
			this.lookLabel.Text = "Look in my collection for:";
			// 
			// filterTextBox
			// 
			this.filterTextBox.Location = new System.Drawing.Point(71, 66);
			this.filterTextBox.Name = "filterTextBox";
			this.filterTextBox.Size = new System.Drawing.Size(140, 20);
			this.filterTextBox.TabIndex = 0;
			this.filterTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FilterTextBox_KeyUp);
			// 
			// showAllImage
			// 
			this.showAllImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.showAllImage.BackColor = System.Drawing.Color.Transparent;
			this.showAllImage.BackgroundImage = global::MovieCollection1.Properties.Resources.button_showDVD_L;
			this.showAllImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.showAllImage.FlatAppearance.BorderSize = 0;
			this.showAllImage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.showAllImage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.showAllImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.showAllImage.Location = new System.Drawing.Point(323, 15);
			this.showAllImage.Name = "showAllImage";
			this.showAllImage.Size = new System.Drawing.Size(62, 45);
			this.showAllImage.TabIndex = 31;
			this.showAllImage.TabStop = false;
			this.showAllImage.UseVisualStyleBackColor = false;
			// 
			// pictureBox2
			// 
			this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox2.BackgroundImage = global::MovieCollection1.Properties.Resources.icon_magnifyingGlass;
			this.pictureBox2.Location = new System.Drawing.Point(13, 21);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(51, 88);
			this.pictureBox2.TabIndex = 53;
			this.pictureBox2.TabStop = false;
			// 
			// filterButton
			// 
			this.filterButton.BackColor = System.Drawing.Color.Transparent;
			this.filterButton.BackgroundImage = global::MovieCollection1.Properties.Resources.button_smallBlue_search;
			this.filterButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.filterButton.FlatAppearance.BorderSize = 0;
			this.filterButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.filterButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.filterButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.filterButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.filterButton.ForeColor = System.Drawing.Color.Black;
			this.filterButton.Location = new System.Drawing.Point(218, 59);
			this.filterButton.Name = "filterButton";
			this.filterButton.Size = new System.Drawing.Size(70, 32);
			this.filterButton.TabIndex = 1;
			this.filterButton.Text = "Filter";
			this.filterButton.UseVisualStyleBackColor = false;
			this.filterButton.Click += new System.EventHandler(this.FilterButton_Clicked);
			// 
			// showAllButton
			// 
			this.showAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.showAllButton.BackColor = System.Drawing.Color.Transparent;
			this.showAllButton.BackgroundImage = global::MovieCollection1.Properties.Resources.button_showDvd_r;
			this.showAllButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.showAllButton.FlatAppearance.BorderSize = 0;
			this.showAllButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.showAllButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.showAllButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.showAllButton.ForeColor = System.Drawing.Color.Black;
			this.showAllButton.Location = new System.Drawing.Point(385, 15);
			this.showAllButton.Name = "showAllButton";
			this.showAllButton.Size = new System.Drawing.Size(120, 45);
			this.showAllButton.TabIndex = 2;
			this.showAllButton.Text = "Show all dvds";
			this.showAllButton.UseVisualStyleBackColor = false;
			this.showAllButton.Click += new System.EventHandler(this.ShowAllButton_Click);
			// 
			// dvdsTableAdapter
			// 
			this.dvdsTableAdapter.ClearBeforeFill = true;
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.Filter = "Common image files (*.jpg;*.gif;*.bmp;*.png)|*.jpg;*.gif;*.bmp;*.png|Jpeg files (" +
				"*.jpg)|*.jpg|GIF files (*.gif)|*.gif|Bitmap files (*.bmp)|*.bmp|PNG files (*.png" +
				")|*.png";
			this.openFileDialog1.Title = "Select the image file to import and press Open: ";
			this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenFileDialog1_FileOk);
			// 
			// ListDetails
			// 
			this.BackColor = System.Drawing.SystemColors.Control;
			this.BackgroundImage = global::MovieCollection1.Properties.Resources.PanelBG_Tile;
			this.Controls.Add(this.showAllButton);
			this.Controls.Add(this.filterButton);
			this.Controls.Add(this.pictureBox2);
			this.Controls.Add(this.showAllImage);
			this.Controls.Add(this.lookLabel);
			this.Controls.Add(this.filterTextBox);
			this.Controls.Add(this.addTitleButton);
			this.Controls.Add(this.removeButton);
			this.Controls.Add(this.viewOnlineButton);
			this.Controls.Add(this.refreshOnlineButton);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.dvdDetailsPanel);
			this.Name = "ListDetails";
			this.Size = new System.Drawing.Size(900, 500);
			this.Load += new System.EventHandler(this.ListDetails_Load);
			this.ParentChanged += new System.EventHandler(this.ListDetails_ParentChanged);
			this.dvdDetailsPanel.ResumeLayout(false);
			this.dvdDetailsPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.boxArtPictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvdsDataConnector)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvdCollectionDataSet)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		private System.Windows.Forms.Panel dvdDetailsPanel;
		private System.Windows.Forms.Label yearReleasedLabel1;
		private System.Windows.Forms.PictureBox boxArtPictureBox;
		private System.Windows.Forms.Label minutesLabel;
		private System.Windows.Forms.Label yearDisplay;
		private System.Windows.Forms.Label titleDisplay;
		private System.Windows.Forms.Label myRatingLabel;
		private System.Windows.Forms.Label upcLabel;
		private System.Windows.Forms.TextBox upcTextBox;
		private System.Windows.Forms.Label ratedLabel;
		private System.Windows.Forms.Label lengthLabel;
		private System.Windows.Forms.Label directorLabel;
		private System.Windows.Forms.TextBox directorTextBox;
		private System.Windows.Forms.Label commentsLabel;
		private System.Windows.Forms.TextBox commentsTextBox;
		private System.Windows.Forms.Label descriptionLabel;
		private System.Windows.Forms.TextBox descriptionTextBox;
		private System.Windows.Forms.Label titleLabel;
		private System.Windows.Forms.TextBox titleTextBox;
		private System.Windows.Forms.TextBox yearReleasedDateTimePicker;
		private System.Windows.Forms.ComboBox ratedComboBox;
		private System.Windows.Forms.TextBox lengthNumericUpDown;
		private System.Windows.Forms.TextBox yearReleasedTextbox;
		private System.Windows.Forms.Button viewOnlineButton;
		private System.Windows.Forms.Button refreshOnlineButton;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.Button addTitleButton;
		private System.Windows.Forms.Label lookLabel;
		private System.Windows.Forms.TextBox filterTextBox;
		private System.Windows.Forms.Button showAllImage;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.DataGridViewTextBoxColumn title;
		private System.Windows.Forms.DataGridViewTextBoxColumn yearReleased;
		private System.Windows.Forms.DataGridViewTextBoxColumn director;
		private System.Windows.Forms.DataGridViewTextBoxColumn myRating;
		private System.Windows.Forms.DataGridViewTextBoxColumn rated;
		private System.Windows.Forms.DataGridViewTextBoxColumn actors;
		private System.Windows.Forms.DataGridViewTextBoxColumn length;
		private System.Windows.Forms.DataGridViewTextBoxColumn description;
		private System.Windows.Forms.DataGridViewTextBoxColumn comments;
		private System.Windows.Forms.DataGridViewTextBoxColumn upc;
		private System.Windows.Forms.DataGridViewTextBoxColumn imageLink;
		private System.Windows.Forms.DataGridViewTextBoxColumn webPageLink;
		private System.Windows.Forms.DataGridViewTextBoxColumn id;
		private System.Windows.Forms.DataGridViewTextBoxColumn genre;
		private System.Windows.Forms.Button filterButton;
		private System.Windows.Forms.Button showAllButton;
		private System.Windows.Forms.Label detailsLabel;
		private MovieCollection1.DVDCollectionDataSetTableAdapters.DVDsTableAdapter dvdsTableAdapter;
		private System.Windows.Forms.Button importButton;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		internal MovieCollection1.DVDCollectionDataSet dvdCollectionDataSet;
		internal System.Windows.Forms.BindingSource dvdsDataConnector;
	}
}


