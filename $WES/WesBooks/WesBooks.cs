using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

// Note: Started at 2:10 PM

namespace nsWesBooks {

    public partial class WesBooks : Form {

        const string BooksDatabaseFilename = @"D:\LRS\WesBooks\BooksPseudoDatabase.xml";

        List<WesBook>   dbWes;
        int ixBook;

//---------------------------------------------------------------------------------------

        public WesBooks() {
            InitializeComponent();

            dbWes = new List<WesBook>();

            PopulateGenres();
            PopulatePublishers();

            picCover.SizeMode = PictureBoxSizeMode.StretchImage;
            tabSearchPicCover.SizeMode = PictureBoxSizeMode.StretchImage;

            LoadDatabase();
        }

//---------------------------------------------------------------------------------------

        private void LoadDatabase() {
            if (!File.Exists(BooksDatabaseFilename)) {
                File.Create(BooksDatabaseFilename);
            }
            dbWes = GenericSerialization<List<WesBook>>.Load(BooksDatabaseFilename);
        }

//---------------------------------------------------------------------------------------

        private void PopulateGenres() {
            cmbGenre.Items.AddRange(new string[] { "SciFi", "Mystery", "Romance", "Other" });
            cmbGenre.SelectedIndex = 0;
        }

//---------------------------------------------------------------------------------------

        private void PopulatePublishers() {
            cmbPublisher.Items.AddRange(new string[] { "Tor", "Houghton Mifflin", "Other" });
            cmbPublisher.SelectedIndex = 0;
        }

//---------------------------------------------------------------------------------------

        private void btnBrowse_Click(object sender, EventArgs e) {
            var dlg = new OpenFileDialog();
            dlg.InitialDirectory = @"c:\LRS\WebBooks";
            dlg.Filter = "Jpeg (*.jpg)|*.jpg|All files (*.*)|*.*";
            var res = dlg.ShowDialog();
            if (res != DialogResult.OK) {
                return;
            }
            // Note: Could do try/catch for bad format file
            var img = Image.FromFile(dlg.FileName);
            // picCover.SizeMode = PictureBoxSizeMode.StretchImage;

            picCover.Image = img;
        }

//---------------------------------------------------------------------------------------

        private void btnAdd_Click(object sender, EventArgs e) {
            var book = new WesBook(txtTitle.Text,
                txtAuthor.Text,
                cmbGenre.SelectedItem.ToString(),
                txtISBN.Text,
                cmbPublisher.SelectedItem.ToString(),
                dateTimePicker1.Value,
                ImageToBase64(picCover.Image, ImageFormat.Jpeg));

            dbWes.Add(book);

            GenericSerialization<List<WesBook>>.Save(BooksDatabaseFilename, dbWes);

            MessageBox.Show("Book Added", "Wes Books");
        }

//---------------------------------------------------------------------------------------

        // Convert image to string
        public string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format) {
            using (MemoryStream ms = new MemoryStream()) {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

//---------------------------------------------------------------------------------------

        private void tabSearchGo_Click(object sender, EventArgs e) {
            ixBook = 0;
            ShowBook();
        }

//---------------------------------------------------------------------------------------

        private void tabSearchBtnNext_Click(object sender, EventArgs e) {
            ShowBook();
        }

//---------------------------------------------------------------------------------------

        private void ShowBook() {
            if (ixBook >= dbWes.Count) {
                ixBook = 0;
	        }
            var wb = dbWes[ixBook++];
            tabSearchTxtTitle.Text = wb.Title;
            tabSearchTxtAuthor.Text = wb.Author;
            tabSearchTextISBN.Text = wb.ISBN;

            var img = wb.GetImage();
            tabSearchPicCover.Image = img;
        }
	}
}