using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LRSUtils.Database;

namespace WesBooks3 {
	class tblBooks {
		private int _BookID;
		public int BookID {
			get { return _BookID; }
			set { _BookID = value; IsDirty = true; }
		}

		private string _Title;
		public string Title {
			get { return _Title;	}

			set { _Title = value; IsDirty = true; }
		}

		// The rest would be done the same as the above

		private string title;
		public string ISBN;
		public string CoverLocation;
		public int? AuthorID;
		public int? GenreID;
		public int? PublisherID;
		public int? SeriesID;
		public bool bRead;
		public bool bSeries;
		public bool bAnthology;
		public DateTime? dtDateRead;

		public bool IsDirty;

		LRSAccessDatabase db;

		const int UnknownID = -1;

//---------------------------------------------------------------------------------------

		public tblBooks(LRSAccessDatabase db) {
			this.db = db;
			Clear();
		}

//---------------------------------------------------------------------------------------

		public tblBooks(OleDbDataReader rdr) {
			BookID        = (int)rdr["BookID"];
			Title         = (string)rdr["Title"];
			ISBN          = (string)rdr["ISBN"];
			CoverLocation = (string)rdr["CoverLocation"];
			AuthorID      = (int)rdr["AuthorID"];
			PublisherID   = (int)rdr["PublisherID"];
			SeriesID      = (int)rdr["SeriesID"];
			dtDateRead    = (DateTime)rdr["dtDateTime"];
			bRead         = (bool)rdr["bRead"];
			bAnthology    = (bool)rdr["bAnthology"];
			bSeries       = (bool)rdr["bSeries"];

			IsDirty = true;
		}

//---------------------------------------------------------------------------------------

		public void Clear() {
			if (IsDirty) {
				throw new Exception("Data aasn't been written yet");
			}
			BookID        = UnknownID;
			Title         = null;
			ISBN          = null;
			CoverLocation = null;
			AuthorID      = UnknownID;
			GenreID       = UnknownID;
			PublisherID   = UnknownID;
			SeriesID      = UnknownID;
			bRead         = false;
			bAnthology    = false;
			bSeries       = false;
			dtDateRead    = DateTime.MinValue;

			IsDirty = false;
		}

//---------------------------------------------------------------------------------------

		public void NewBook(string Title, string ISBN, string CoverLocation /* And the other parms */) {
			if (IsDirty) {
				throw new Exception("Data aasn't been written yet");
			}
			this.BookID        = UnknownID;
			this.Title         = Title;
			this.ISBN          = ISBN;
			this.CoverLocation = CoverLocation;
			this.AuthorID      = UnknownID;
			this.GenreID       = UnknownID;
			PublisherID        = UnknownID;
			SeriesID           = UnknownID;
			bRead              = false;
			bAnthology         = false;
			bSeries            = false;
			dtDateRead         = DateTime.MinValue;
		}

//---------------------------------------------------------------------------------------

		public List<tblBooks> ReadByBookID(int ID) {
			var SQL = "SELECT * FROM tblBooks WHERE BookID = " + /* Book */ID;
			return ReturnBooks(SQL);
		}

//---------------------------------------------------------------------------------------

		public List<tblBooks> ReadByTitle(string BookTitle) {
			BookTitle = BookTitle.Replace('*', '%');	// Replace normal wildcard (if
			// any) with Access wildcard
			string op = BookTitle.Contains('%') ? "LIKE" : "=";
			var SQL   = "SELECT * FROM tblBooks WHERE Title " + op + " \"" + BookTitle + "\"";
			return ReturnBooks(SQL);
		}

//---------------------------------------------------------------------------------------

		private List<tblBooks> ReturnBooks(string SQL) {
			var qry   = new AccessQuery(db);
			var rdr   = qry.OpenQuery(SQL);
			var books = new List<tblBooks>();
			while (rdr.Read()) {
				books.Add(new tblBooks(rdr));
			}
			return books;
		}


//---------------------------------------------------------------------------------------

		public int Write() {
			// TODO: What if this is a replace?
			var SQL = @"
				INSERT INTO tblBooks 
					(Title, AuthorID, GenreID, PublisherID, SeriesID, bRead, dtDateRead, bAnthology, bSeries)
					VALUES(@Title, @AuthorID, @GenreID, @PublisherID, @SeriesID, @bRead, @dtDateRead, @bAnthology, @bSeries)
";
			OleDbCommand oCmd = new OleDbCommand(SQL, db.Conn);
			oCmd.Parameters.AddWithValue("@Title", Title);
			// TODO: Add ISBN here and maybe other places
			oCmd.Parameters.AddWithValue("@AuthorID", AuthorID);
			oCmd.Parameters.AddWithValue("@GenreID", GenreID);
			oCmd.Parameters.AddWithValue("@PublisherID", PublisherID);
			oCmd.Parameters.AddWithValue("@SeriesID", SeriesID);
			oCmd.Parameters.AddWithValue("@bRead", bRead);
			oCmd.Parameters.AddWithValue("@dtDateRead", dtDateRead);
			oCmd.Parameters.AddWithValue("@bAnthology", bAnthology);
			oCmd.Parameters.AddWithValue("@bSeries", bSeries);
			var qry = new AccessQuery(db);
			try {
				var rc = qry.ExecuteNonQuery(SQL, oCmd);
			} catch (Exception ex) {
				System.Windows.Forms.MessageBox.Show(ex.Message + "1234");
			}

			int Identity;
			oCmd.CommandText = "SELECT @@IDENTITY";
			Identity = (int)oCmd.ExecuteScalar();

			IsDirty = false;

			return Identity;
		}

//---------------------------------------------------------------------------------------

		public static void CreateTable(string filename) {
			// Access data types -- http://msdn.microsoft.com/en-us/library/bb208866%28v=office.12%29.aspx
			// Create table, with links to other DDL statements -- http://msdn.microsoft.com/en-us/library/bb177893(v=office.12).aspx
			// http://office.microsoft.com/en-us/access-help/introduction-to-access-sql-HA010341468.aspx
			// Create Access database -- http://www.excelguru.ca/content.php?122-Creating-an-Access-Database-(on-the-fly)-Using-VBA-and-SQL

			var db = new LRSAccessDatabase(filename);
			var qry = new AccessQuery(db);
			var SQL = "DROP TABLE tblBooks";
			var OK = qry.ExecuteNonQuery(SQL);

			SQL = @"
				CREATE TABLE tblBooks( 
					BookID		  IDENTITY PRIMARY KEY,
					Title		  CHAR(60) NOT NULL,
					ISBN		  CHAR(20) NOT NULL,
					CoverLocation CHAR(60) NOT NULL,
					AuthorID	  INTEGER NOT NULL,
					GenreID		  INTEGER NOT NULL,
					SeriesID      INTEGER NOT NULL,  
					bRead		  BIT NOT NULL,
					dtDateRead	  DATETIME,
					bAnthology	  BIT NOT NULL,
					bSeries       BIT NOT NULL 
				)
";
			OK = qry.ExecuteNonQuery(SQL);
		}
	}
}
