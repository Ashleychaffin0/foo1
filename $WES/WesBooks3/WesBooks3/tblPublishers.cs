using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LRSUtils.Database;

namespace WesBooks3 {
	class tblPublishers {

		public int PublisherID;
		public string Publisher;

//---------------------------------------------------------------------------------------

		public tblPublishers(OleDbDataReader rdr) {
			PublisherID = (int)rdr["PublisherID"];
			Publisher = (string)rdr["Publisher"];
		}

//---------------------------------------------------------------------------------------

		public override string ToString() {
			return Publisher;
		}

//---------------------------------------------------------------------------------------

		public static List<tblPublishers> ReadAll(LRSAccessDatabase db) {
			var SQL = @"
				SELECT * FROM tblPublishers ORDER BY Publisher
";
			var qry = new AccessQuery(db);
			var rdr = qry.OpenQuery(SQL);
			var Publishers = new List<tblPublishers>();
			while (rdr.Read()) {
				Publishers.Add(new tblPublishers(rdr));
			}
			return Publishers;
		}

//---------------------------------------------------------------------------------------

		public static void CreateTable(string filename) {
			var db = new LRSAccessDatabase(filename);
			var qry = new AccessQuery(db);
			var SQL = "DROP TABLE tblPublishers";
			var OK = qry.ExecuteNonQuery(SQL);
			SQL = @"
				CREATE TABLE tblPublishers( 
					 PublisherID	IDENTITY PRIMARY KEY,
					 Publisher	    CHAR(24) NOT NULL
				)
";
			OK = qry.ExecuteNonQuery(SQL);
		}
	}
}

