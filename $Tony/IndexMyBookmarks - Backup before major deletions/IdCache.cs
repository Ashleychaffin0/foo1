using System.Collections.Generic;

namespace IndexMyBookmarks {
	class IdCache {
		PPFTD					 db;
		Dictionary<string, long> Cache;

		string					 tblType;   // "Url", "Word", "Ref"
											// For better or worse, we make the convention that all the tables we want to
											// cache obey the following rules:
											//	* the table name is tbl{tblType}s -- e.g. tblUrls
											//	* the ID field is {tblType}ID -- e.g. WordID

		public int nWordsFoundInCache;
		public int nWordsNotInCache;

//---------------------------------------------------------------------------------------

		public IdCache(PPFTD db, string tblType) {
			this.db      = db;
			this.tblType = tblType;

			nWordsFoundInCache = 0;
			nWordsNotInCache   = 0;
		}

//---------------------------------------------------------------------------------------

		public int TotalCacheCalls {
			get => nWordsFoundInCache + nWordsNotInCache;
		}

//---------------------------------------------------------------------------------------

		public int CacheAllWordIds() {
			Cache      = new Dictionary<string, long>();
			string sql = $"SELECT {tblType}Id, {tblType} from tbl{tblType}s";
			var rdr    = db.ExecuteReader(sql);
			int count  = 0;
			while (rdr.Read()) {
				long WordId = (long)rdr[0];
				string Word = (string)rdr[1];
				Cache[Word.Trim()] = WordId;
				count++;
			}
			return count;
		}

//---------------------------------------------------------------------------------------

		public long GetIDForWord(string word) {
			// Sigh. Dumb Sqlite. It treats "0" and "00" as identical. So get around
			// that. And also check for floating point. At one point I found a GUID in
			// the Html that had "-41e1-" in it. Sigh again.
			bool bOK = int.TryParse(word, out int n);
			if (bOK) {
				word = n.ToString();
			} else {
				bOK = double.TryParse(word, out double x);
				if (bOK) {
					word = x.ToString();
				}
			}

			bOK = Cache.TryGetValue(word, out long ID);
			if (bOK) {
				++nWordsFoundInCache;
			} else {
				++nWordsNotInCache;
				string sql = $@"
INSERT INTO tbl{tblType}s(word) VALUES('{word}');
SELECT last_insert_rowid() FROM tbl{tblType}s; ";
				ID = (long)db.ExecuteScalar(sql);
				Cache[word] = ID;
			}
			return ID;
		}
	}
}
