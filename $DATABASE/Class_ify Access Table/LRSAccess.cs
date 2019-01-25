using System;
using System.Collections.Generic;
using System.Data.OleDb;

// See also http://www.codeproject.com/Tips/810596/Csharp-Create-read-and-write-MS-Access-mdb-accdb-M

namespace LRSUtils.Database {
	public class LRSAccessDatabase {
		string dbname;			// Keep it around, just in case
		OleDbConnection conn;
		bool isConnected;

		//---------------------------------------------------------------------------------------

		public LRSAccessDatabase() {
			dbname = "";
			isConnected = false;
		}

		//---------------------------------------------------------------------------------------

		public LRSAccessDatabase(string dbName) {
			isConnected = false;
			Open(dbName);
		}

		//---------------------------------------------------------------------------------------

		public OleDbConnection Conn {
			get {
				return conn;
			}
		}

		//---------------------------------------------------------------------------------------

		public void Open(string dbName) {
			// TODO: If database is already open, do we a) fail the request
			// (i.e. with an exception), or b) quietly close the current one
			// and open the new. I think we do the former, since we can have
			// users (i.e. Access Query instances) referring to us). But for now...
			if (isConnected)
				throw new System.Exception();
			string sConn;
			dbname = dbName;
			// TODO: This seems to be opening the database in exclusive mode.
			// Check into it.
			sConn = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + dbname;
			// sConn = "Provider=Microsoft.ACE.OLEDB.12.0;data source=" + dbname + ";Persist Security Info=False;";	
			conn = new OleDbConnection(sConn);
			conn.Open();
			// TODO: I'm not sure how to find out whether the connection
			// opened OK or not. Probably with try/catch and/or checking
			// conn.State. But for now....
			isConnected = true;
		}

		//---------------------------------------------------------------------------------------

		public void Close() {
			conn.Close();
			isConnected = false;
		}

#if false			// Until I figure out what the problem is

//---------------------------------------------------------------------------------------

		public static List<T> ReadTable<T>(LRSAccessDatabase db, string SQL) 
								where T : class {
			var qry = new AccessQuery(db);
			var rdr = qry.OpenQuery(SQL);
			var items = new List<T>();
			while (rdr.Read()) {
				items.Add(new T(rdr));
			}
			return items;
		}
#endif
	}

	/*********************************************************************************/
	/*********************************************************************************/
	/*********************************************************************************/
	/*********************************************************************************/
	/*********************************************************************************/

	public class AccessQuery {
		LRSAccessDatabase    db;
		string				 SQL;
		OleDbDataReader      reader;

		//---------------------------------------------------------------------------------------

		public AccessQuery(LRSAccessDatabase db) {
			this.db = db;
			SQL = "";
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
