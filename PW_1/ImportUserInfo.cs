
using System;
using System.Diagnostics;
using System.Text;

namespace PW_1 {
	//---------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------

	public class ImportUserInfo {
		internal string			Category;
		internal string			SiteName;
		internal string			SiteUrl;
		internal string			LogonID;
		internal string			Password;
		internal StringBuilder	Comment;

//---------------------------------------------------------------------------------------

		public ImportUserInfo() {
			Category = "";
			SiteName = "";
			SiteUrl  = "";
			LogonID  = "";
			Password = "";
			Comment  = new StringBuilder();
		}

//---------------------------------------------------------------------------------------

		public bool AreAllFieldsPresent() => 
			(Category.Length > 0)
			&& (SiteName.Length > 0) 
			&& (SiteUrl.Length  > 0) 
			&& (LogonID.Length  > 0)
			&& (Password.Length > 0);

//---------------------------------------------------------------------------------------

		internal void Dump() {
			if (SiteName.Length == 0) Debugger.Break();
			string cat = Category.ToString();
			Console.WriteLine($"\tCat: {cat}, Name: {SiteName}, Url: {SiteUrl}," +
			   $" ID: {LogonID}, Pass: {Password}, Comment: {Comment}");
		}
	}
}

