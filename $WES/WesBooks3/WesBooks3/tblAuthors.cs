using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LRSUtils.Database;

namespace WesBooks3 {
	class tblAuthors {

		public int AuthorID;
		public string AuthorFirstName;			// I hope you have no books by,
												// say, Saki
		public string AuthorLastName;

		LRSAccessDatabase db;

//---------------------------------------------------------------------------------------

		public tblAuthors(LRSAccessDatabase db) {
			this.db = db;
		}

//---------------------------------------------------------------------------------------

		public tblAuthors(OleDbDataReader rdr) {
			AuthorID = (int)rdr["AuthorID"];
			AuthorFirstName = (string)rdr["AuthorFirstName"];
			AuthorLastName = (string)rdr["AuthorLasttName"];
		}

//---------------------------------------------------------------------------------------

		public List<tblAuthors> Read(string AuthorPrefix) {
			var results = new List<tblAuthors>();
			string SQL = @"
				SELECT * FROM tblAuthors 
					WHERE AuthorLasttName LIKE '" + AuthorPrefix + @"%'
					ORDER BY AuthorLasttName, AuthorFirstName
";
			// TODO: Code stolen from tblBooks.ReturnBooks
			var qry = new AccessQuery(db);
			var rdr = qry.OpenQuery(SQL);
			var books = new List<tblAuthors>();
			while (rdr.Read()) {
				books.Add(new tblAuthors(rdr));
			}
			return books;
		}

//---------------------------------------------------------------------------------------

		public override string ToString() {
			return AuthorFirstName.Trim() + " " + AuthorLastName.Trim();
		}

//---------------------------------------------------------------------------------------

		public static void CreateTable(string filename) {
			var db = new LRSAccessDatabase(filename);
			var qry = new AccessQuery(db);
			var SQL = "DROP TABLE tblAuthors";
			var OK = qry.ExecuteNonQuery(SQL);
			SQL = @"
				CREATE TABLE tblAuthors( 
					AuthorID			IDENTITY PRIMARY KEY,
					AuthorFirstName		CHAR(20) NOT NULL,
					AuthorLasttName		CHAR(20) NOT NULL
				)
";
			OK = qry.ExecuteNonQuery(SQL);
		}
	}
}

