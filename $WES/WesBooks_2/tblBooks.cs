using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LRSUtils.Database;

namespace WesBooks_2 {
	class tblBooks {
		public int          BookID;
		public string		Title;
		public int			AuthorID;
		public int			GenreID;
		public int			PublisherID;
		public bool         bRead;
		public DateTime		dtDateRead;
		public bool         bAnthology;

		LRSAccessDatabase db;

		const int UnknownID = -1;

//---------------------------------------------------------------------------------------

		public tblBooks(LRSAccessDatabase db) {
			this.db = db;
			Clear();
		}

//---------------------------------------------------------------------------------------

		public tblBooks(
				LRSAccessDatabase db,
				int			BookID,
				string		Title,
				int			AuthorID,
				int			GenreID,
				int			PublisherID,
				bool		bRead,
				DateTime	dtDateRead,
		bool bAnthology
			) {

		}

//---------------------------------------------------------------------------------------

		public tblBooks(OleDbDataReader rdr) {
			BookID = (int)  rdr["BookID"];
			Title = (string)rdr["Title"];
			// TODO: The rest
		}

//---------------------------------------------------------------------------------------

		public void Clear() {
			BookID      = UnknownID;
			Title       = null;
			AuthorID    = UnknownID;
			GenreID     = UnknownID;
			PublisherID = UnknownID;
			bRead       = false;
			dtDateRead  = DateTime.MinValue;
			bAnthology  = false;
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
			var SQL = "SELECT * FROM tblBooks WHERE Title " + op + " \"" + BookTitle + "\"";
			return ReturnBooks(SQL);
		}

//---------------------------------------------------------------------------------------

		private List<tblBooks> ReturnBooks(string SQL) {
			var qry = new AccessQuery(db);
			var rdr = qry.OpenQuery(SQL);
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
					(Title, AuthorID, GenreID, PublisherID, bRead, dtDateRead, bAnthology)
					VALUES(@Title, @AuthorID, @GenreID, @PublisherID, @bRead, @dtDateRead, @bAnthology)
";
			OleDbCommand oCmd = new OleDbCommand(SQL, db.Conn);
			oCmd.Parameters.AddWithValue("@Title", Title);
			oCmd.Parameters.AddWithValue("@AuthorID", AuthorID);
			oCmd.Parameters.AddWithValue("@GenreID", GenreID);
			oCmd.Parameters.AddWithValue("@PublisherID", PublisherID);
			oCmd.Parameters.AddWithValue("@bRead", bRead);
			oCmd.Parameters.AddWithValue("@dtDateRead", dtDateRead);
			oCmd.Parameters.AddWithValue("@bAnthology", bAnthology);

#if false			
			// TODO: Named parameters would be much nicer. Define a class Pair
			// TODO: Use KeyValuePair<string, int> instead of Pair
			foreach (Pair p in parms) {
				oCmd.Parameters.AddWithValue((string)p.a, p.b);
			}
			reader = oCmd.ExecuteReader();
			return reader;


#endif
			var qry = new AccessQuery(db);
			var rc = qry.ExecuteNonQuery(SQL, oCmd);

			int Identity;
			oCmd.CommandText = "SELECT @@IDENTITY";
			Identity = (int)oCmd.ExecuteScalar();

			return Identity;
		}

//---------------------------------------------------------------------------------------

		public static void CreateTable(string filename) {
			// Access data types -- http://msdn.microsoft.com/en-us/library/bb208866%28v=office.12%29.aspx
			// Create table, with links to other DDL statements -- http://msdn.microsoft.com/en-us/library/bb177893(v=office.12).aspx
			// http://office.microsoft.com/en-us/access-help/introduction-to-access-sql-HA010341468.aspx
			// Create Access database -- http://www.excelguru.ca/content.php?122-Creating-an-Access-Database-(on-the-fly)-Using-VBA-and-SQL

			var db  = new LRSAccessDatabase(filename);
			var qry = new AccessQuery(db);
			var SQL = "DROP TABLE tblBooks";
			var OK  = qry.ExecuteNonQuery(SQL);

			SQL = @"
				CREATE TABLE tblBooks( 
					BookID		IDENTITY PRIMARY KEY,
					Title		CHAR(60) NOT NULL,
					AuthorID	INTEGER NOT NULL,
					GenreID		INTEGER NOT NULL,
					bRead		BIT NOT NULL,
					dtDateRead	DATETIME,
					bAnthology	BIT NOT NULL
				)
";
			OK = qry.ExecuteNonQuery(SQL);
		}
	}
}
