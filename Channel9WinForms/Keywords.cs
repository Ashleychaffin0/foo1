using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlServerCe;

namespace WinFormsChannel9 {
	static class Keywords {

		public static List<string> KeywordsList;

//---------------------------------------------------------------------------------------

		public static Dictionary<string, int> GetKeywordsFromDatabase(SqlCeConnection conn) {
			var KeywordDict = new Dictionary<string, int>();
			string SQL = "SELECT Keyword, KeywordID FROM tblKeywords ORDER BY KEYWORD";
			var cmd = new SqlCeCommand(SQL, conn);
			using (var rdr = cmd.ExecuteReader()) {
				while (rdr.Read()) {
					string Keyword = (string) rdr["Keyword"];
					int KeywordID = Convert.ToInt32(rdr["KeywordID"]);
					KeywordDict[Keyword] = KeywordID;
				}
			}
			return KeywordDict;
		}

//---------------------------------------------------------------------------------------

		public static void SetKeywords(List<string> Keywords) {
			KeywordsList = Keywords;
		}

//---------------------------------------------------------------------------------------

		public static List<string> ScanForKeywords(string Text) {
			var Result = new List<string>();
			foreach (var Keyword in Keywords.KeywordsList) {
				//if (Keyword == "PowerShell") {
				//    System.Diagnostics.Debugger.Break();
				//}
				if (Text.IndexOf(Keyword, StringComparison.OrdinalIgnoreCase) >= 0) {
					Result.Add(Keyword);
				}
			}
			return Result;
		}
	}
}
