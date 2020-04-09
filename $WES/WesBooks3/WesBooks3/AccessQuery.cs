using System.Collections.Generic;
using System.Data.OleDb;

// See also http://www.codeproject.com/Tips/810596/Csharp-Create-read-and-write-MS-Access-mdb-accdb-M

namespace LRSUtils.Database {
	public class AccessQuery {
		LRSAccessDatabase db;
		string SQL;
		OleDbDataReader reader;

//---------------------------------------------------------------------------------------

		public AccessQuery(LRSAccessDatabase db) {
			this.db = db;
			SQL     = "";
		}

//---------------------------------------------------------------------------------------

		public OleDbDataReader OpenQuery(string SQL, params KeyValuePair<string, string>[] parms) {
			this.SQL = SQL;
			OleDbCommand oCmd = new OleDbCommand(SQL, db.Conn);
			foreach (var p in parms) {
				oCmd.Parameters.AddWithValue(p.Key, p.Value);
			}
			reader = oCmd.ExecuteReader();
			return reader;
		}

//---------------------------------------------------------------------------------------

		public int ExecuteNonQuery(string SQL) {
			var ocmd = new OleDbCommand(SQL, db.Conn);
			int bOK = ocmd.ExecuteNonQuery();
			return bOK;
		}

//---------------------------------------------------------------------------------------

		public int ExecuteNonQuery(string SQL, OleDbCommand ocmd) {
			int bOK = ocmd.ExecuteNonQuery();
			return bOK;
		}

//---------------------------------------------------------------------------------------

		public void Close() {
			reader.Close();
		}
	}
}
