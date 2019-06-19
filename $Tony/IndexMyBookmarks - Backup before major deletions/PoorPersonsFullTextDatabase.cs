using System;
using System.Data.SQLite;
using System.IO;

namespace IndexMyBookmarks {
	class PPFTD {                   // Poor Persons Full Text Database
		string				 DatabaseName;
		SQLiteConnection	 dbconn;
		public SessionCounts Stats;

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

		public class SessionCounts {
			public int Selects;
			public int Inserts;

//---------------------------------------------------------------------------------------

			public void Reset() {
				Selects = 0;
				Inserts = 0;
			}

//---------------------------------------------------------------------------------------

			public (int Selects, int Inserts) Data {
				get => (Selects, Inserts);
			}
		}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

		public PPFTD(string dbName) {
			Stats = new SessionCounts();
			DatabaseName = dbName;
			if (!File.Exists(DatabaseName)) {
				CreateDatabase();
			} else {
				ConnectToDatabase();
			}
		}

//---------------------------------------------------------------------------------------

		private void ConnectToDatabase() {
			dbconn = new SQLiteConnection($"Data Source={DatabaseName};Version=3;");
			dbconn.Open();
		}

//---------------------------------------------------------------------------------------

		public void EmptyTheDatabase() {
			string sql = @"
				DELETE FROM tblUrls;
				DELETE FROM tblWords;
				DELETE FROM tblRefs;
				VACUUM;
";
			ExecuteNonQuery(sql);
		}

//---------------------------------------------------------------------------------------

		public void ExecuteNonQuery(string sql) {
			++Stats.Inserts;
			using (var cmd = new SQLiteCommand(sql, dbconn)) {
				cmd.ExecuteNonQuery();
			}
		}

//---------------------------------------------------------------------------------------

		public object ExecuteScalar(string sql) {
			++Stats.Selects;    // Assume it's a SELECT
			using (var cmd = new SQLiteCommand(sql, dbconn)) {
				object UrlID = cmd.ExecuteScalar();
				return UrlID;
			}
		}

//---------------------------------------------------------------------------------------

		public SQLiteDataReader ExecuteReader(string sql) {
			++Stats.Selects;
			using (var cmd = new SQLiteCommand(sql, dbconn)) {
				var rdr    = cmd.ExecuteReader();
				return rdr;
			}
		}

//---------------------------------------------------------------------------------------

		private void CreateDatabase() {
			SQLiteConnection.CreateFile(DatabaseName);
			ConnectToDatabase();
			CreateTablesEtAl();
		}

//---------------------------------------------------------------------------------------

		private void CreateTablesEtAl() {
			CreateTableUrls();
			CreateTableWords();
			CreateTableRefs();

			// We've commented the next line out. I think that constantly updating the
			// index is what's making our performance suck. We'll depend on caching.
			// We'll also remove the UNIQUE attribute on tblWords. Again, we'll take care
			// of that ourselves.
			// CreateTableIndexes();

			CreateViewAllRefs();
		}

//---------------------------------------------------------------------------------------

		private void CreateTableUrls() {
			string sql = @"
CREATE TABLE tblUrls
	(UrlID			INTEGER PRIMARY KEY AUTOINCREMENT,
	 Url			STRING UNIQUE,
	 Title			STRING,
	 Folder			STRING,
	 WhenCreated	DATETIME
	);";
			ExecuteNonQuery(sql);
		}

//---------------------------------------------------------------------------------------

		private void CreateTableWords() {
			string sql = @"
CREATE TABLE tblWords
	(WordID			INTEGER PRIMARY KEY AUTOINCREMENT,
	 Word			STRING UNIQUE
	);";
			ExecuteNonQuery(sql);
		}

//---------------------------------------------------------------------------------------

		private void CreateTableRefs() {
			// TODO: Add Foreign keys. See
			//		 http://www.sqlitetutorial.net/sqlite-foreign-key/
			string sql = @"
CREATE TABLE tblRefs
	(
		RefID			INTEGER PRIMARY KEY AUTOINCREMENT,
		WordID			INTEGER REFERENCES tblWords(WordID),
		UrlID			INTEGER REFERENCES tblUrls(UrlID)
	);";
			ExecuteNonQuery(sql);
		}

//---------------------------------------------------------------------------------------

		private void CreateTableIndexes() {
			string sql = "CREATE UNIQUE INDEX ixWords on tblWords(word);";
			ExecuteNonQuery(sql);
			sql = "CREATE UNIQUE INDEX ixRefs on tblRefs(WordID);";
			ExecuteNonQuery(sql);
		}

//---------------------------------------------------------------------------------------

		private void CreateViewAllRefs() {
			string sql = @"
CREATE VIEW AllRefs AS
	SELECT * 
	FROM tblRefs 
		INNER JOIN tblWords on tblRefs.WordID = tblWords.WordID
		INNER JOIN tblUrls  on tblRefs.UrlID  = tblUrls.UrlID;
";
			ExecuteNonQuery(sql);
		}
	}
}
