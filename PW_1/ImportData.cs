
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace PW_1 {
	class ImportData {
		internal Dictionary<string /* UserName */, List<ImportUserInfo>> UserValues;

		ImportUserInfo UserInfo;

		// I'll initialize things here. I don't expect the user to import data more
		// than once in a given session.

		int lineno = 0;

		List<ImportUserInfo> CurValues = new List<ImportUserInfo>();

		string   CurUserName = "";
		string	 CurCategory = "";
		string	 CurSiteName = "";
		string	 CurSiteUrl  = "";
		string	 CurLoginID  = "";
		string	 CurPassword = "";

		string	PrevUserName = "";

//---------------------------------------------------------------------------------------

		public ImportData() {
			UserValues = new Dictionary<string, List<ImportUserInfo>>();
			UserInfo   = new ImportUserInfo();
		}

//---------------------------------------------------------------------------------------

		internal bool Import(string fn) {
			bool rc = true;
			foreach (string src in File.ReadLines(fn)) {
				++lineno;
				string line = src.Replace('\t', ' ').Trim();
				Console.WriteLine($"[{lineno}]: ".PadRight(7) + line);
				if (line.Length == 0) { continue; } // Skip blank lines
				string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				string verb = tokens[0].ToLower();
				switch (verb) {
				case "user:":
					if (! HandleUser(tokens)) { rc = false; continue; }
					break;
				case "category:":
					if (! HandleCategory(tokens)) { rc = false; continue; }
					break;
				case "sitename:":
					if (! HandleSiteName(tokens)) { rc = false; continue; }
					break;
				case "siteurl:":
					if (! HandleSiteUrl(tokens)) { rc = false; continue; }
					break;
				case "logonid:":
					if (! HandleLogonID(tokens)) { rc = false; continue; }
					break;
				case "password:":
					if (! HandlePassword(tokens)) { rc = false; continue; }
					break;
				case "comment:":
					if (! HandleComment(tokens)) { rc = false; continue; }
					break;
				default:
					Console.WriteLine($"Error: '{tokens[0]}' on line {lineno} not recognized");
					rc = false;
					break;
				}
			}

			// Finish off last User entry
			rc &= AddInfo(UserInfo, CurUserName);

			return rc;
		}

//---------------------------------------------------------------------------------------

		private bool AddInfo(ImportUserInfo info, string userName) {
			if (! info.AreAllFieldsPresent()) {
				Console.WriteLine($"Line {lineno}: Not all fields are present for user {userName}");
				return false; 
			}
			UserValues[userName].Add(info);
			ResetValues();
			return true;
		}

//---------------------------------------------------------------------------------------

		private void CreateNewUser(string userName) {
			CurValues            = new List<ImportUserInfo>();
			UserValues[userName] = CurValues;
			ResetValues();
		}

//---------------------------------------------------------------------------------------

		void ResetValues() {
			CurCategory	= "";
			CurSiteName = "";
			CurSiteUrl  = "";
			CurLoginID  = "";
			CurPassword = "";
			UserInfo     = new ImportUserInfo();
		}

//---------------------------------------------------------------------------------------

		private bool HandleUser(string[] tokens) {
			CurUserName = tokens[1];
			bool rc = true;
			if (tokens.Length != 2) {
				Console.WriteLine($"Line {lineno}: Error: The USER: verb must have exactly 1 argument");
				return false;
			}
			if (PrevUserName.Length == 0) {
				// This is the first User: verb. No need to finish off
				// the previous one (which doesn't exist)
				CreateNewUser(CurUserName);
			} else {
				// Finish off previous User
				Console.WriteLine($"About to finish off user: {PrevUserName}");
				UserInfo.Dump();
				rc &= AddInfo(UserInfo, PrevUserName);
				if (! UserValues.ContainsKey(CurUserName)) { 
					// Haven't seen this User before. Start new one.
					Console.WriteLine($"About to start new user: {CurUserName}");
					CreateNewUser(CurUserName);
				}
			}
			PrevUserName = CurUserName;
			ResetValues();
			return rc;
		}

//---------------------------------------------------------------------------------------

		private bool HandleCategory(string[] tokens) {
			bool bGotUser = CheckForUser(tokens[0]);
			if (!bGotUser) { return false; }
			if (CurCategory.Length > 0) {
				Console.WriteLine($"Error: More than one Category: for user {CurUserName}");
				return false;
			}
			CurCategory = string.Join(' ', tokens[1..]);
			UserInfo.Category = CurCategory;
			return true;
		}

//---------------------------------------------------------------------------------------

		private bool HandleSiteName(string[] tokens) {
			bool bGotUser = CheckForUser(tokens[0]);
			if (! bGotUser) { return false; }
			if (CurSiteName.Length > 0) {
				Console.WriteLine($"Line {lineno}: Error: More than one SiteName: for user {CurUserName}");
				return false;
			}
			CurSiteName = string.Join(' ', tokens[1..]);
			UserInfo.SiteName = CurSiteName;
			return true;
		}

//---------------------------------------------------------------------------------------

		private bool HandleSiteUrl(string[] tokens) {
			bool bGotUser = CheckForUser(tokens[0]);
			if (! bGotUser) { return false; }
			if (CurSiteUrl.Length > 0) {
				Console.WriteLine($"Line {lineno}: Error: More than one SiteUrl: per user {CurUserName}");
				return false;
			}
			CurSiteUrl = tokens[1];
			UserInfo.SiteUrl = CurSiteUrl;
			return true;
		}

//---------------------------------------------------------------------------------------

		private bool HandleLogonID(string[] tokens) {
			bool bGotUser = CheckForUser(tokens[0]);
			if (! bGotUser) { return false; }
			if (CurLoginID.Length > 0) {
				Console.WriteLine($"Line {lineno}: Error: More than one LogonID: per user {CurUserName}");
				return false;
			}
			CurLoginID = tokens[1];
			UserInfo.LogonID = CurLoginID;
			return true;
		}

//---------------------------------------------------------------------------------------

		private bool HandlePassword(string[] tokens) {
			bool bGotUser = CheckForUser(tokens[0]);
			if (! bGotUser) { return false; }
			if (CurPassword.Length > 0) {
				Console.WriteLine($"Line {lineno}: Error: More than one Password: per user {CurUserName}");
				return false;
			}
			CurPassword = tokens[1];
			UserInfo.Password = CurPassword;
			return true;
		}

//---------------------------------------------------------------------------------------

		private bool HandleComment(string[] tokens) {
			// Note: Multiple Comment verbs are valid.
			bool bGotUser = CheckForUser(tokens[0]);
			if (! bGotUser) { return false; }
			UserInfo.Comment.AppendLine(string.Join(' ', tokens[1..]));
			return true;
		}

//---------------------------------------------------------------------------------------

		bool CheckForUser(string verb) {
			if (PrevUserName.Length == 0) {
				Console.WriteLine($"Line {lineno}: Error: The {verb.ToUpper()} verb must follow a USER: verb");
				return false;
			}
			return true;
		}
	}
}

