using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Speech.Synthesis;
using System.Speech.Recognition;

using LRSUtils.Database;

namespace WesBooks_2 {
	public partial class WesBooks2 : Form {

		string				dbName;
		LRSAccessDatabase	db;

		tblBooks	CurBook;
		tblAuthors	Authors;

//---------------------------------------------------------------------------------------

		public WesBooks2() {
			InitializeComponent();

			// TODO: What if run on different machine?
			if (Environment.MachineName == "LRS8500-PC") {
				dbName = @"D:\LRS\WesBooks2.mdb";
			} else {
				dbName = @"C:\WRR\WesBooks2.mdb";
			}

			// DropTables(dbName);
			// CreateTables(dbName);

			db = new LRSAccessDatabase(dbName);
			
			CurBook = new tblBooks(db);
			Authors = new tblAuthors(db);

			SetupComboBoxes();

#if false
			var s = new SpeechSynthesizer();
			s.Speak("Hello Wes");

			var sre = new SpeechRecognitionEngine();
			sre.SpeechRecognized += Sre_SpeechRecognized;
			sre.Recognize();


#endif

		}

//---------------------------------------------------------------------------------------

		private void button1_Click_1(object sender, EventArgs e) {
			// http://stackoverflow.com/questions/5467827/good-speech-recognition-api
			SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();
			Grammar dictationGrammar = new DictationGrammar();
			recognizer.LoadGrammar(dictationGrammar);
			try {
				button1.Text = "Speak Now";
				recognizer.SetInputToDefaultAudioDevice();
				RecognitionResult result = recognizer.Recognize();
				button1.Text = result.Text;
			} catch (InvalidOperationException exception) {
				button1.Text = String.Format("Could not recognize input from default aduio device. Is a microphone or sound card available?\r\n{0} - {1}.", exception.Source, exception.Message);
			} finally {
				recognizer.UnloadAllGrammars();
			}
		}

		//---------------------------------------------------------------------------------------

		private void SetupComboBoxes() {
			var Genres = tblGenres.ReadAll(db);
			cmbGenre.DataSource = Genres;

			var Publishers = tblPublishers.ReadAll(db);
			cmbPublisher.DataSource = Publishers;
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

		private void txtAuthorLastName_TextChanged(object sender, EventArgs e) {
			string txt  = txtAuthorLastName.Text.Trim();	// Ignore leading blanks
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

		private void pupAuthors_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
			var Author = (tblAuthors)e.ClickedItem.Tag;
			txtAuthorFirstName.Text = Author.AuthorFirstName.Trim();
			txtAuthorLastName.Text  = Author.AuthorLastName.Trim();
		}

//---------------------------------------------------------------------------------------

		private void HighlightAuthorName(string txt, List<tblAuthors> writers) {
			// Note: This code doesn't quite work correctly. It recurses when we set
			//		 txtAuthLastName to the value read from the database. In
			//		 particular, backspacing doesn't work the way you'd expect.
			var writer = writers[0];
			txtAuthorLastName.Text = writer.AuthorLastName.Trim();
			txtAuthorLastName.SelectionStart = txt.Length;
			txtAuthorLastName.SelectionLength = writer.AuthorLastName.Length - txt.Length;
			txtAuthorFirstName.Text = writer.AuthorFirstName;
		}

//---------------------------------------------------------------------------------------

		private void btnAdd_Click(object sender, EventArgs e) {
			CurBook.Write();
#if false
			var book = new tblBooks(
				txtTitle.Text,
				txtAuthor.Text,
				cmbGenre.SelectedItem.ToString(),
				txtISBN.Text,
				cmbPublisher.SelectedItem.ToString(),
				dtLastRead.Value,
				"TODO: Cover image filename");
			int BookID = book.Write();

			dbWes.Add(book);

			GenericSerialization<List<WesBook>>.Save(BooksDatabaseFilename, dbWes);

			MessageBox.Show("Book Added", "Wes Books");
#endif
		}

//---------------------------------------------------------------------------------------

		private void btnBrowse_Click(object sender, EventArgs e) {
#if false
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
#endif
		}

//---------------------------------------------------------------------------------------

		private void tabSearchGo_Click(object sender, EventArgs e) {
 #if false
		   ixBook = 0;
		ShowBook();
 #endif
	   }

//---------------------------------------------------------------------------------------

		private void tabSearchBtnNext_Click(object sender, EventArgs e) {
  #if false
		  ShowBook();
#endif
		}

//---------------------------------------------------------------------------------------

		private void ShowBook() {
#if false
			if (ixBook >= dbWes.Count) {
				ixBook = 0;
			}
			var wb = dbWes[ixBook++];
			tabSearchTxtTitle.Text = wb.Title;
			tabSearchTxtAuthor.Text = wb.Author;
			tabSearchTextISBN.Text = wb.ISBN;

			var img = wb.GetImage();
			tabSearchPicCover.Image = img;

#endif
		}

//---------------------------------------------------------------------------------------

		private void btnNewGenre_Click(object sender, EventArgs e) {
			// TODO:
		}

//---------------------------------------------------------------------------------------

		private void btnNewPublisher_Click(object sender, EventArgs e) {
			// TODO:
		}

//---------------------------------------------------------------------------------------

		private void DropTables(string dbName) {
			var db = new LRSAccessDatabase(dbName);
			var qry = new AccessQuery(db);
			var SQL = "DROP TABLE Tracks";
			var OK = qry.ExecuteNonQuery(SQL);
		}

//---------------------------------------------------------------------------------------

		private void CreateTables(string dbName) {
			// tblBooks.CreateTable(dbName);
			// tblAuthors.CreateTable(dbName);
			// tblGenres.CreateTable(dbName);
			// tblPublishers.CreateTable(dbName);
			// tblCollections.CreateTable(dbName);
		}
	}
}
