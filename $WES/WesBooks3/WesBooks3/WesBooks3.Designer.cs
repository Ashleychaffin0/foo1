namespace WesBooks3
{
    partial class WesBooks3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			this.Tabs = new System.Windows.Forms.TabControl();
			this.tabNewBook = new System.Windows.Forms.TabPage();
			this.chkAnthology = new System.Windows.Forms.CheckBox();
			this.btnNewPublisher = new System.Windows.Forms.Button();
			this.btnNewGenre = new System.Windows.Forms.Button();
			this.txtAuthorFirstName = new System.Windows.Forms.TextBox();
			this.label15 = new System.Windows.Forms.Label();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.picCover = new System.Windows.Forms.PictureBox();
			this.dtLastRead = new System.Windows.Forms.DateTimePicker();
			this.label6 = new System.Windows.Forms.Label();
			this.cmbPublisher = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtISBN = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.cmbGenre = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtAuthorLastName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtTitle = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tabSearch = new System.Windows.Forms.TabPage();
			this.tabSearchBtnNext = new System.Windows.Forms.Button();
			this.tabSearchDtPicker = new System.Windows.Forms.DateTimePicker();
			this.label9 = new System.Windows.Forms.Label();
			this.tabSearchCmdPublisher = new System.Windows.Forms.ComboBox();
			this.label10 = new System.Windows.Forms.Label();
			this.tabSearchTextISBN = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.tabSearchCmbGenre = new System.Windows.Forms.ComboBox();
			this.label12 = new System.Windows.Forms.Label();
			this.tabSearchTxtAuthor = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.tabSearchTxtTitle = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.tabSearchGo = new System.Windows.Forms.Button();
			this.tabSearchPicCover = new System.Windows.Forms.PictureBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.cmbFieldname = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.pupAuthors = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.Tabs.SuspendLayout();
			this.tabNewBook.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picCover)).BeginInit();
			this.tabSearch.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tabSearchPicCover)).BeginInit();
			this.SuspendLayout();
			// 
			// Tabs
			// 
			this.Tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Tabs.Controls.Add(this.tabNewBook);
			this.Tabs.Controls.Add(this.tabSearch);
			this.Tabs.Location = new System.Drawing.Point(27, 23);
			this.Tabs.Name = "Tabs";
			this.Tabs.SelectedIndex = 0;
			this.Tabs.Size = new System.Drawing.Size(730, 359);
			this.Tabs.TabIndex = 0;
			// 
			// tabNewBook
			// 
			this.tabNewBook.Controls.Add(this.chkAnthology);
			this.tabNewBook.Controls.Add(this.btnNewPublisher);
			this.tabNewBook.Controls.Add(this.btnNewGenre);
			this.tabNewBook.Controls.Add(this.txtAuthorFirstName);
			this.tabNewBook.Controls.Add(this.label15);
			this.tabNewBook.Controls.Add(this.btnAdd);
			this.tabNewBook.Controls.Add(this.btnBrowse);
			this.tabNewBook.Controls.Add(this.picCover);
			this.tabNewBook.Controls.Add(this.dtLastRead);
			this.tabNewBook.Controls.Add(this.label6);
			this.tabNewBook.Controls.Add(this.cmbPublisher);
			this.tabNewBook.Controls.Add(this.label5);
			this.tabNewBook.Controls.Add(this.txtISBN);
			this.tabNewBook.Controls.Add(this.label4);
			this.tabNewBook.Controls.Add(this.cmbGenre);
			this.tabNewBook.Controls.Add(this.label3);
			this.tabNewBook.Controls.Add(this.txtAuthorLastName);
			this.tabNewBook.Controls.Add(this.label2);
			this.tabNewBook.Controls.Add(this.txtTitle);
			this.tabNewBook.Controls.Add(this.label1);
			this.tabNewBook.Location = new System.Drawing.Point(4, 22);
			this.tabNewBook.Name = "tabNewBook";
			this.tabNewBook.Padding = new System.Windows.Forms.Padding(3);
			this.tabNewBook.Size = new System.Drawing.Size(722, 333);
			this.tabNewBook.TabIndex = 1;
			this.tabNewBook.Text = "New Book";
			this.tabNewBook.UseVisualStyleBackColor = true;
			// 
			// chkAnthology
			// 
			this.chkAnthology.AutoSize = true;
			this.chkAnthology.Location = new System.Drawing.Point(204, 287);
			this.chkAnthology.Name = "chkAnthology";
			this.chkAnthology.Size = new System.Drawing.Size(73, 17);
			this.chkAnthology.TabIndex = 19;
			this.chkAnthology.Text = "Anthology";
			this.chkAnthology.UseVisualStyleBackColor = true;
			// 
			// btnNewPublisher
			// 
			this.btnNewPublisher.Location = new System.Drawing.Point(400, 165);
			this.btnNewPublisher.Name = "btnNewPublisher";
			this.btnNewPublisher.Size = new System.Drawing.Size(75, 23);
			this.btnNewPublisher.TabIndex = 18;
			this.btnNewPublisher.Text = "Edit";
			this.btnNewPublisher.UseVisualStyleBackColor = true;
			// 
			// btnNewGenre
			// 
			this.btnNewGenre.Location = new System.Drawing.Point(400, 125);
			this.btnNewGenre.Name = "btnNewGenre";
			this.btnNewGenre.Size = new System.Drawing.Size(75, 23);
			this.btnNewGenre.TabIndex = 17;
			this.btnNewGenre.Text = "Edit";
			this.btnNewGenre.UseVisualStyleBackColor = true;
			this.btnNewGenre.Click += new System.EventHandler(this.btnNewGenre_Click);
			// 
			// txtAuthorFirstName
			// 
			this.txtAuthorFirstName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtAuthorFirstName.Location = new System.Drawing.Point(146, 92);
			this.txtAuthorFirstName.Name = "txtAuthorFirstName";
			this.txtAuthorFirstName.Size = new System.Drawing.Size(329, 20);
			this.txtAuthorFirstName.TabIndex = 16;
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(23, 95);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(91, 13);
			this.label15.TabIndex = 15;
			this.label15.Text = "Author First Name";
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(26, 282);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(75, 23);
			this.btnAdd.TabIndex = 14;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Location = new System.Drawing.Point(641, 24);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(75, 23);
			this.btnBrowse.TabIndex = 13;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			// 
			// picCover
			// 
			this.picCover.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.picCover.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picCover.Location = new System.Drawing.Point(495, 21);
			this.picCover.Name = "picCover";
			this.picCover.Size = new System.Drawing.Size(125, 202);
			this.picCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picCover.TabIndex = 12;
			this.picCover.TabStop = false;
			// 
			// dtLastRead
			// 
			this.dtLastRead.Location = new System.Drawing.Point(146, 242);
			this.dtLastRead.Name = "dtLastRead";
			this.dtLastRead.Size = new System.Drawing.Size(200, 20);
			this.dtLastRead.TabIndex = 11;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(23, 242);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(82, 13);
			this.label6.TabIndex = 10;
			this.label6.Text = "Date Last Read";
			// 
			// cmbPublisher
			// 
			this.cmbPublisher.FormattingEnabled = true;
			this.cmbPublisher.Location = new System.Drawing.Point(146, 167);
			this.cmbPublisher.Name = "cmbPublisher";
			this.cmbPublisher.Size = new System.Drawing.Size(224, 21);
			this.cmbPublisher.TabIndex = 9;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(23, 167);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(50, 13);
			this.label5.TabIndex = 8;
			this.label5.Text = "Publisher";
			// 
			// txtISBN
			// 
			this.txtISBN.Location = new System.Drawing.Point(146, 203);
			this.txtISBN.Name = "txtISBN";
			this.txtISBN.Size = new System.Drawing.Size(224, 20);
			this.txtISBN.TabIndex = 7;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(23, 206);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(32, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "ISBN";
			// 
			// cmbGenre
			// 
			this.cmbGenre.FormattingEnabled = true;
			this.cmbGenre.Location = new System.Drawing.Point(146, 127);
			this.cmbGenre.Name = "cmbGenre";
			this.cmbGenre.Size = new System.Drawing.Size(224, 21);
			this.cmbGenre.TabIndex = 5;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(23, 136);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(36, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Genre";
			// 
			// txtAuthorLastName
			// 
			this.txtAuthorLastName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtAuthorLastName.Location = new System.Drawing.Point(146, 58);
			this.txtAuthorLastName.Name = "txtAuthorLastName";
			this.txtAuthorLastName.Size = new System.Drawing.Size(329, 20);
			this.txtAuthorLastName.TabIndex = 3;
			this.txtAuthorLastName.TextChanged += new System.EventHandler(this.txtAuthorLastName_TextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(23, 61);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(92, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Author Last Name";
			// 
			// txtTitle
			// 
			this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtTitle.Location = new System.Drawing.Point(146, 21);
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new System.Drawing.Size(329, 20);
			this.txtTitle.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(23, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(27, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Title";
			// 
			// tabSearch
			// 
			this.tabSearch.Controls.Add(this.tabSearchBtnNext);
			this.tabSearch.Controls.Add(this.tabSearchDtPicker);
			this.tabSearch.Controls.Add(this.label9);
			this.tabSearch.Controls.Add(this.tabSearchCmdPublisher);
			this.tabSearch.Controls.Add(this.label10);
			this.tabSearch.Controls.Add(this.tabSearchTextISBN);
			this.tabSearch.Controls.Add(this.label11);
			this.tabSearch.Controls.Add(this.tabSearchCmbGenre);
			this.tabSearch.Controls.Add(this.label12);
			this.tabSearch.Controls.Add(this.tabSearchTxtAuthor);
			this.tabSearch.Controls.Add(this.label13);
			this.tabSearch.Controls.Add(this.tabSearchTxtTitle);
			this.tabSearch.Controls.Add(this.label14);
			this.tabSearch.Controls.Add(this.tabSearchGo);
			this.tabSearch.Controls.Add(this.tabSearchPicCover);
			this.tabSearch.Controls.Add(this.textBox1);
			this.tabSearch.Controls.Add(this.label8);
			this.tabSearch.Controls.Add(this.cmbFieldname);
			this.tabSearch.Controls.Add(this.label7);
			this.tabSearch.Location = new System.Drawing.Point(4, 22);
			this.tabSearch.Name = "tabSearch";
			this.tabSearch.Padding = new System.Windows.Forms.Padding(3);
			this.tabSearch.Size = new System.Drawing.Size(722, 333);
			this.tabSearch.TabIndex = 2;
			this.tabSearch.Text = "Search";
			this.tabSearch.UseVisualStyleBackColor = true;
			// 
			// tabSearchBtnNext
			// 
			this.tabSearchBtnNext.Location = new System.Drawing.Point(435, 14);
			this.tabSearchBtnNext.Name = "tabSearchBtnNext";
			this.tabSearchBtnNext.Size = new System.Drawing.Size(75, 23);
			this.tabSearchBtnNext.TabIndex = 24;
			this.tabSearchBtnNext.Text = "Next";
			this.tabSearchBtnNext.UseVisualStyleBackColor = true;
			// 
			// tabSearchDtPicker
			// 
			this.tabSearchDtPicker.Location = new System.Drawing.Point(181, 270);
			this.tabSearchDtPicker.Name = "tabSearchDtPicker";
			this.tabSearchDtPicker.Size = new System.Drawing.Size(200, 20);
			this.tabSearchDtPicker.TabIndex = 23;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(58, 270);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(82, 13);
			this.label9.TabIndex = 22;
			this.label9.Text = "Date Last Read";
			// 
			// tabSearchCmdPublisher
			// 
			this.tabSearchCmdPublisher.FormattingEnabled = true;
			this.tabSearchCmdPublisher.Location = new System.Drawing.Point(181, 231);
			this.tabSearchCmdPublisher.Name = "tabSearchCmdPublisher";
			this.tabSearchCmdPublisher.Size = new System.Drawing.Size(224, 21);
			this.tabSearchCmdPublisher.TabIndex = 21;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(58, 231);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(50, 13);
			this.label10.TabIndex = 20;
			this.label10.Text = "Publisher";
			// 
			// tabSearchTextISBN
			// 
			this.tabSearchTextISBN.Location = new System.Drawing.Point(181, 192);
			this.tabSearchTextISBN.Name = "tabSearchTextISBN";
			this.tabSearchTextISBN.Size = new System.Drawing.Size(224, 20);
			this.tabSearchTextISBN.TabIndex = 19;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(58, 195);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(32, 13);
			this.label11.TabIndex = 18;
			this.label11.Text = "ISBN";
			// 
			// tabSearchCmbGenre
			// 
			this.tabSearchCmbGenre.FormattingEnabled = true;
			this.tabSearchCmbGenre.Location = new System.Drawing.Point(181, 155);
			this.tabSearchCmbGenre.Name = "tabSearchCmbGenre";
			this.tabSearchCmbGenre.Size = new System.Drawing.Size(224, 21);
			this.tabSearchCmbGenre.TabIndex = 17;
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(58, 164);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(36, 13);
			this.label12.TabIndex = 16;
			this.label12.Text = "Genre";
			// 
			// tabSearchTxtAuthor
			// 
			this.tabSearchTxtAuthor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabSearchTxtAuthor.Location = new System.Drawing.Point(181, 122);
			this.tabSearchTxtAuthor.Name = "tabSearchTxtAuthor";
			this.tabSearchTxtAuthor.Size = new System.Drawing.Size(329, 20);
			this.tabSearchTxtAuthor.TabIndex = 15;
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(58, 125);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(38, 13);
			this.label13.TabIndex = 14;
			this.label13.Text = "Author";
			// 
			// tabSearchTxtTitle
			// 
			this.tabSearchTxtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabSearchTxtTitle.Location = new System.Drawing.Point(181, 85);
			this.tabSearchTxtTitle.Name = "tabSearchTxtTitle";
			this.tabSearchTxtTitle.Size = new System.Drawing.Size(329, 20);
			this.tabSearchTxtTitle.TabIndex = 13;
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(58, 88);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(27, 13);
			this.label14.TabIndex = 12;
			this.label14.Text = "Title";
			// 
			// tabSearchGo
			// 
			this.tabSearchGo.Location = new System.Drawing.Point(337, 14);
			this.tabSearchGo.Name = "tabSearchGo";
			this.tabSearchGo.Size = new System.Drawing.Size(75, 23);
			this.tabSearchGo.TabIndex = 10;
			this.tabSearchGo.Text = "Go";
			this.tabSearchGo.UseVisualStyleBackColor = true;
			// 
			// tabSearchPicCover
			// 
			this.tabSearchPicCover.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tabSearchPicCover.Location = new System.Drawing.Point(547, 16);
			this.tabSearchPicCover.Name = "tabSearchPicCover";
			this.tabSearchPicCover.Size = new System.Drawing.Size(141, 267);
			this.tabSearchPicCover.TabIndex = 9;
			this.tabSearchPicCover.TabStop = false;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(75, 46);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(435, 20);
			this.textBox1.TabIndex = 8;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.ForeColor = System.Drawing.Color.Red;
			this.label8.Location = new System.Drawing.Point(17, 54);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(39, 13);
			this.label8.TabIndex = 7;
			this.label8.Text = "Value";
			// 
			// cmbFieldname
			// 
			this.cmbFieldname.FormattingEnabled = true;
			this.cmbFieldname.Location = new System.Drawing.Point(75, 16);
			this.cmbFieldname.Name = "cmbFieldname";
			this.cmbFieldname.Size = new System.Drawing.Size(224, 21);
			this.cmbFieldname.TabIndex = 6;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.ForeColor = System.Drawing.Color.Red;
			this.label7.Location = new System.Drawing.Point(17, 19);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(34, 13);
			this.label7.TabIndex = 0;
			this.label7.Text = "Field";
			// 
			// pupAuthors
			// 
			this.pupAuthors.Name = "pupAuthors";
			this.pupAuthors.Size = new System.Drawing.Size(61, 4);
			this.pupAuthors.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.pupAuthors_ItemClicked);
			// 
			// WesBooks3
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(788, 394);
			this.Controls.Add(this.Tabs);
			this.Name = "WesBooks3";
			this.Text = "Wes Books";
			this.Tabs.ResumeLayout(false);
			this.tabNewBook.ResumeLayout(false);
			this.tabNewBook.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picCover)).EndInit();
			this.tabSearch.ResumeLayout(false);
			this.tabSearch.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.tabSearchPicCover)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage tabNewBook;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.PictureBox picCover;
        private System.Windows.Forms.DateTimePicker dtLastRead;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbPublisher;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtISBN;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbGenre;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAuthorLastName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabSearch;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button tabSearchGo;
        private System.Windows.Forms.PictureBox tabSearchPicCover;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbFieldname;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker tabSearchDtPicker;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox tabSearchCmdPublisher;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tabSearchTextISBN;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox tabSearchCmbGenre;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tabSearchTxtAuthor;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tabSearchTxtTitle;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button tabSearchBtnNext;
        private System.Windows.Forms.TextBox txtAuthorFirstName;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnNewPublisher;
        private System.Windows.Forms.Button btnNewGenre;
        private System.Windows.Forms.ContextMenuStrip pupAuthors;
		private System.Windows.Forms.CheckBox chkAnthology;
    }
}

