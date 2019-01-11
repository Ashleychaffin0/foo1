using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinFormsChannel9 {
	static class Keywords {

		public static List<string> KeywordsList;

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
