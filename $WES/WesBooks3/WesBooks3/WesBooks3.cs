using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using LRSUtils.Database;

namespace WesBooks3 {
	public partial class WesBooks3 : Form {
		string dbName;
		LRSAccessDatabase db;

		tblBooks CurBook;
		tblAuthors Authors;

//---------------------------------------------------------------------------------------

		public WesBooks3() {

			InitializeComponent();

			var isbn = new IsbnLookup();
			var x = isbn.Read("0-449-21815-5");
			x = isbn.Read("0-449-21815-5");

			// TODO: What if run on different machine?
			if (Environment.MachineName == "LRS8500-PC") {
				dbName = @"D:\LRS\Devel\C#-2013\WesBooks3\WesBooks3\WesBooks3.mdb";
			} else {
				dbName = @"C:\Code\WesBooks3\WesBooks3.mdb";
			}

			db = new LRSAccessDatabase(dbName);
			// var d = tblGenres.ReadAll(db);

			SetupComboBoxes();

			Authors = new tblAuthors(db);

			// var s = new SpeechSynthesizer();
			// s.Speak("Welcome to book inventory");

		}

//---------------------------------------------------------------------------------------

		void NewBook() {
			txtTitle.Text           = "";
			txtAuthorFirstName.Text = "";
			txtAuthorLastName.Text  = "";
			txtISBN.Text            = "";
			dtLastRead.Value        = DateTime.Now;
		}

//---------------------------------------------------------------------------------------

		private void SetupComboBoxes() {
			var Genres = tblGenres.ReadAll(db);
			cmbGenre.DataSource = Genres;
			// foreach (var genre in Genres) {
			// 	cmbGenre.Items.Add(genre);
			// }

			var Publishers = tblPublishers.ReadAll(db);
			cmbPublisher.DataSource = Publishers;
		}

//---------------------------------------------------------------------------------------

		private void txtAuthorLastName_TextChanged(object sender, EventArgs e) {
			string txt = txtAuthorLastName.Text.Trim();	// Ignore leading blanks
			var writers = Authors.Read(txt);
			if (writers.Count > 0) {
				// HighlightAuthorName(txt, writers);
				PopupAuthorName(txt, writers);
			}
		}

//---------------------------------------------------------------------------------------

		private void PopupAuthorName(string txt, List<tblAuthors> writers) {
			pupAuthors.Items.Clear();
			foreach (var writer in writers) {
				ToolStripItem item = pupAuthors.Items.Add(writer.ToString());
				item.Tag = writer;
			}
			pupAuthors.Show(txtAuthorLastName, txtAuthorLastName.Location);
		}

//---------------------------------------------------------------------------------------

		private void HighlightAuthorName(string txt, List<tblAuthors> writers) {
			// Note: This code doesn't quite work correctly. It recurses when we set
			//		 txtAuthLastName to the value read from the database. In
			//		 particular, backspacing doesn't work the way you'd expect.
			var writer                        = writers[0];
			txtAuthorLastName.Text            = writer.AuthorLastName.Trim();
			txtAuthorLastName.SelectionStart  = txt.Length;
			txtAuthorLastName.SelectionLength = writer.AuthorLastName.Length - txt.Length;
			txtAuthorFirstName.Text           = writer.AuthorFirstName;
		}

//---------------------------------------------------------------------------------------

		private void pupAuthors_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
			var Author              = (tblAuthors)e.ClickedItem.Tag;
			txtAuthorFirstName.Text = Author.AuthorFirstName.Trim();
			txtAuthorLastName.Text  = Author.AuthorLastName.Trim();
		}

//---------------------------------------------------------------------------------------

		private void btnAdd_Click(object sender, EventArgs e) {
			var CurBook         = new tblBooks(db);
			// var Author       = (tblAuthors)e.ClickedItem.Tag;
			CurBook.AuthorID    = 1; // Testing Author.AuthorID; How to pass?
			CurBook.GenreID     = ((tblGenres)cmbGenre.SelectedItem).GenreID;
			CurBook.PublisherID = ((tblPublishers)cmbPublisher.SelectedItem).PublisherID;
			CurBook.SeriesID    = 1; //Testing
			
			CurBook.Title         = txtTitle.Text;
			CurBook.ISBN          = txtISBN.Text;
			CurBook.SeriesID      = 1;  //Testing
			CurBook.CoverLocation = @"C:\WES\WORK\FRED.JPG"; //Testing
			CurBook.bAnthology    = chkAnthology.Checked;
			CurBook.bSeries       = true;  // Testing for now
			CurBook.bRead         = false;
			// bool bSeries       = chkSeries.Checked;

			// var book = new tblBooks(db, txtTitle.Text, txtISBN.Text, AuthorID, SeriesID, , ....);
			int BookID = CurBook.Write();
			System.Windows.Forms.MessageBox.Show("Book ID=" + BookID); 
		}

//---------------------------------------------------------------------------------------

		private void btnNewGenre_Click(object sender, EventArgs e) {
			var dlg = new dlgEditGenres(db); 
			var result = dlg.ShowDialog();
			if (result == DialogResult.OK) {
				SetupComboBoxes();			// Arguably, it would be slightly better to
											// just refresh the Genre combo box, not all
											// of them. But the overhead is trivial, so
											// we'll just do it this way.
				Console.WriteLine("Number of times Apply was clicked: {0}", dlg.NumberOfApplies);

				var fname = FindFileUpInHierarchy("volcano_stefnisson_960.jpg");
				var img = Image.FromFile(fname);
				// picCover.Image = img;
			}
		}

		
//---------------------------------------------------------------------------------------

		string FindFileUpInHierarchy(string filename) {
			// TODO: Need overload - with and without default starting directory
			string	path = Application.StartupPath + @"\";
			bool	bFound = false;

			// TODO: Not 6. Should stop when it gets to merely a drive letter
			for (int i=0; i<6; ++i) {				// Six levels should be enough!
				if (File.Exists(path + filename)) {
					bFound = true;
					break;
				} else {
					filename = @"..\" + filename;
				}
			}
			if (! bFound) {
				return null;
			}
			// FileInfo	fi = new FileInfo(filename);
			// string FullName = Path.Combine(fi.DirectoryName, filename);
			// return Path.GetFullPath(FullName);
			return filename;
		}

	}
}
