using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LRSUtils.Database;

namespace WesBooks_2 {
	class tblGenres {

		public int		GenreID;
		public string	Genre;

//---------------------------------------------------------------------------------------

		public tblGenres(OleDbDataReader rdr) {
			GenreID = (int)   rdr["GenreID"];
			Genre   = (string)rdr["Genre"];
		}

//---------------------------------------------------------------------------------------

		public override string ToString() {
			return Genre;
		}

//---------------------------------------------------------------------------------------

		public static List<tblGenres> ReadAll(LRSAccessDatabase db) {
			var SQL = @"
				SELECT * FROM tblGenres ORDER BY Genre
";
			var qry = new AccessQuery(db);
			var rdr = qry.OpenQuery(SQL);
			var Genres = new List<tblGenres>();
			while (rdr.Read()) {
				Genres.Add(new tblGenres(rdr));
			}
			return Genres;
		}

//---------------------------------------------------------------------------------------

		public static void CreateTable(string filename) {
			var db = new LRSAccessDatabase(filename);
			var qry = new AccessQuery(db);
			var SQL = "DROP TABLE tblGenres";
			var OK = qry.ExecuteNonQuery(SQL);
			SQL = @"
				CREATE TABLE tblGenres( 
					GenreID	IDENTITY PRIMARY KEY,
					Genre	CHAR(24) NOT NULL
				)
";
			OK = qry.ExecuteNonQuery(SQL);
		}
	}
}
