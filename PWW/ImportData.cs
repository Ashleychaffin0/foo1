
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace PWW {
	class ImportData {
		readonly PWContext db;
		internal Dictionary<string /* UserName */, List<ImportUserInfo>> UserValues;

		ImportUserInfo CurInfo;

		int lineno = 0;			// For debugging

		List<ImportUserInfo> CurUserValues;

		string CurUserName = "";
		string CurCategoryName;
		string CurSiteName;
		string CurSiteUrl;
		string CurLoginID;
		string CurPassword;

//---------------------------------------------------------------------------------------

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
		public ImportData(PWContext db) {
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
			UserValues	= new Dictionary<string, List<ImportUserInfo>>();
			CurInfo		= new ImportUserInfo();
			this.db		= db;
			ResetValues();
			CurUserValues = new List<ImportUserInfo>();
		}

//---------------------------------------------------------------------------------------

		internal bool Import(string fn) {
			bool rc = true;
			foreach (string src in File.ReadLines(fn)) {
				++lineno;
				string line = src.Trim();
				if (line.Length == 0) { continue; } // Skip blank lines
				string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				string verb = tokens[0].ToLower();
				switch (verb) {
				case "user:":
					if (HandleUser(tokens)) { rc = false; continue; }
					break;
				case "category:":
					if (HandleCategory(tokens)) { rc = false; continue; }
					break;
				case "sitename:":
					if (HandleSiteName(tokens)) { rc = false; continue; }
					break;
				case "siteurl:":
					if (HandleSiteUrl(tokens)) { rc = false; continue; }
					break;
				case "logonid:":
					if (HandleLogonID(tokens)) { rc = false; continue; }
					break;
				case "password:":
					if (HandlePassword(tokens)) { rc = false; continue; }
					break;
				case "comment:":
					if (HandleComment(tokens)) { rc = false; continue; }
					break;
				default:
					Console.WriteLine($"Error: '{tokens[0]}' on line {lineno} not recognized");
					rc = false;
					break;
				}
			}

			// Finish off last User entry
			rc &= AddCurInfo(CurUserName);

#if DEBUG
			Debug.WriteLine("");
			Debug.WriteLine("");
			Debug.WriteLine("*** Final UserValues ***");
			Debug.WriteLine("");
			Debug.WriteLine("");
			foreach (var username in UserValues.Keys) {
				dbgDumpUserValues(username);
			}
#endif
			return rc;
		}

//---------------------------------------------------------------------------------------

		private bool AddCurInfo(string userName) {
			string msg = CurInfo.AreAllFieldsPresent();
			if (msg.Length > 0) {
				Debug.WriteLine("The following fields are missing: ");
				Debug.WriteLine(msg);
				return false; 
			}
			CurInfo.dbgDump();
			UserValues[userName].Add(CurInfo);
			ResetValues();
			return true;
		}

//---------------------------------------------------------------------------------------

		private void CreateNewUser(string userName) {
			CurUserValues = new List<ImportUserInfo>();
			UserValues[userName] = CurUserValues;
			ResetValues();
		}

//---------------------------------------------------------------------------------------

		void ResetValues() {
			CurCategoryName = "";
			CurSiteName		= "";
			CurSiteUrl		= "";
			CurLoginID		= "";
			CurPassword		= "";
			CurInfo			= new ImportUserInfo();
		}

//---------------------------------------------------------------------------------------

		private bool HandleUser(string[] tokens) {
			bool rc = false;
			if (tokens.Length != 2) {
				Console.WriteLine("Error: The USER: verb must have exactly 1 argument");
				return false;
			}
			Debug.WriteLine($"In HandleUser, User: {tokens[1]}, CurUserName: {CurUserName}, LineNo: {lineno}");
			if (CurUserName.Length == 0) { 	// This is the first User: verb. No need to finish
											// off the previous one (which doesn't exist)
				Debug.WriteLine($"CurUserName is empty. Creating new user {tokens[1]}");
				CreateNewUser(tokens[1]);
			} else {
				// Finish off previous User, who's name is in CurUserName
				Debug.WriteLine($"Finishing off CurUserName: {CurUserName}");
				rc &= AddCurInfo(CurUserName);
				if (!UserValues.ContainsKey(tokens[1])) {
					// Haven't seen this User before. Start new one.
					Debug.WriteLine($"Creating new user {tokens[1]}");
					CreateNewUser(tokens[1]);
				}
			}
			CurUserName = tokens[1];
			return rc;
		}

//---------------------------------------------------------------------------------------

		private bool HandleCategory(string[] tokens) {
			// TODO: Just one db entry per category
			// TODO: Make sure that this is one of the valid categories
			// TODO: Put list of valid categories into config file
			if (tokens.Length < 2) {
				Console.WriteLine("Error: The CATEGORY: verb must have 1 or more arguments");
				return false;
			}
			CurCategoryName  = string.Join(' ', tokens[1..]);
			CurInfo.Category = CurCategoryName;
			return true;
		}

//---------------------------------------------------------------------------------------

		private bool HandleSiteName(string[] tokens) {
			bool bGotUser = CheckForUser(tokens[0]);
			if (!bGotUser) { return false; }
			if (CurSiteName.Length > 0) {
				Console.WriteLine($"Error: More than one SiteName: for user {CurUserName}");
				return false;
			}
			CurSiteName = string.Join(' ', tokens[1..]);
			CurInfo.SiteName = CurSiteName;
			return true;
		}

//---------------------------------------------------------------------------------------

		private bool HandleSiteUrl(string[] tokens) {
			bool bGotUser = CheckForUser(tokens[0]);
			if (!bGotUser) { return false; }
			if (CurSiteUrl.Length > 0) {
				Console.WriteLine($"Error: More than one SiteUrl: per user {CurUserName}");
				return false;
			}
			CurSiteUrl = tokens[1];
			CurInfo.SiteUrl = CurSiteUrl;
			return true;
		}

//---------------------------------------------------------------------------------------

		private bool HandleLogonID(string[] tokens) {
			bool bGotUser = CheckForUser(tokens[0]);
			if (!bGotUser) { return false; }
			if (CurLoginID.Length > 0) {
				Console.WriteLine($"Error: More than one LogonID: per user {CurUserName}");
				return false;
			}
			CurLoginID = tokens[1];
			CurInfo.LogonID = CurLoginID;
			return true;
		}

//---------------------------------------------------------------------------------------

		private bool HandlePassword(string[] tokens) {
			bool bGotUser = CheckForUser(tokens[0]);
			if (!bGotUser) { return false; }
			if (CurPassword.Length > 0) {
				Console.WriteLine($"Error: More than one Password: per user {CurUserName}");
				return false;
			}
			CurPassword = tokens[1];
			CurInfo.Password = CurPassword;
			return true;
		}

//---------------------------------------------------------------------------------------

		private bool HandleComment(string[] tokens) {
			// Note: Multiple Comment verbs are valid.
			bool bGotUser = CheckForUser(tokens[0]);
			if (!bGotUser) { return false; }
			CurInfo.comment.AppendLine(string.Join(' ', tokens[1..]));
			return true;
		}

//---------------------------------------------------------------------------------------

		bool CheckForUser(string verb) {
			if (CurUserName.Length == 0) { 
				Console.WriteLine($"Error: The {verb.ToUpper()} verb must follow a USER: verb");
				return false;
			}
			return true;
		}

//---------------------------------------------------------------------------------------

		[Conditional("DEBUG")]
		private void dbgDumpUserValues(string userName) {
			Debug.WriteLine($"\t------------ Dumping UserValues for user {userName}");
			List<ImportUserInfo> vals = UserValues[userName];
			foreach (ImportUserInfo val in vals) {
				val.dbgDump();
			}
			Debug.WriteLine("");
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class ImportUserInfo {
		internal string Category;
		internal string SiteName;
		internal string SiteUrl;
		internal string LogonID;
		internal string Password;
		internal StringBuilder comment;

//---------------------------------------------------------------------------------------

		public ImportUserInfo() {
			Category = "";
			SiteName = "";
			SiteUrl  = "";
			LogonID  = "";
			Password = "";
			comment  = new StringBuilder();
		}

//---------------------------------------------------------------------------------------

		public string AreAllFieldsPresent() {
			var sb = new StringBuilder();
			if (Category.Length == 0) { sb.AppendLine("Category"); }
			if (SiteName.Length == 0) { sb.AppendLine("SiteName"); }
			if (SiteUrl.Length  == 0) { sb.AppendLine("SiteUrl");  }
			if (LogonID.Length  == 0) { sb.AppendLine("LogonID");  }
			if (Password.Length == 0) { sb.AppendLine("Password"); }
			return sb.ToString();
		}

//---------------------------------------------------------------------------------------

		[Conditional("DEBUG")]
		internal void dbgDump() {
			Debug.WriteLine($"\tCategoryName: {Category}, SiteName: {SiteName}, SiteUrl: {SiteUrl}, LogonID: {LogonID}, Password: {Password}");
		}
	}
}

