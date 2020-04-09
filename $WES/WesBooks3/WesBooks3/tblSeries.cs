using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LRSUtils.Database;

namespace WesBooks3 {
	class tblSeries {

		public int SeriesID;
		public string Series;

//---------------------------------------------------------------------------------------

		public tblSeries(OleDbDataReader rdr) {
			SeriesID = (int)rdr["SeriesID"];
			Series   = (string)rdr["Series"];
		}

//---------------------------------------------------------------------------------------

		public override string ToString() {
			return Series;
		}

//---------------------------------------------------------------------------------------

		public static List<tblSeries> ReadAll(LRSAccessDatabase db) {
			var SQL = @"
				SELECT * FROM tblSeries ORDER BY Series
";
			var qry = new AccessQuery(db);
			var rdr = qry.OpenQuery(SQL);
			var Seriesof = new List<tblSeries>();
			while (rdr.Read()) {
				Seriesof.Add(new tblSeries(rdr));
			}
			return Seriesof;
		}

//---------------------------------------------------------------------------------------

		public static void CreateTable(string filename) {
			var db  = new LRSAccessDatabase(filename);
			var qry = new AccessQuery(db);
			var SQL = "DROP TABLE tblSeries";
			var OK  = qry.ExecuteNonQuery(SQL);
			SQL     = @"
				CREATE TABLE tblSeries( 
					 SeriesID	IDENTITY PRIMARY KEY,
					 Series	    CHAR(24) NOT NULL
				)
";
			OK = qry.ExecuteNonQuery(SQL);
		}
	}
}


