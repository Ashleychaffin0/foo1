using System;
using System.Collections.Generic;
using System.Linq;

namespace PW_1 {
	partial class TheApp {
		internal static PWContext db = new PWContext();

		public static string DbFilename = "";
		public static string ImportFilename = @"G:\LRS\PWs.txt";	// TODO: OneDrive?

//---------------------------------------------------------------------------------------

		public TheApp() {
			DbFilename = Environment.UserName switch {
				// TODO: Put on to OneDrive
				"lrs5"	  => @"G:\lrs\PW1.db",
				"BGA8700" => @"X:\lrs\PW1.db",
				_ => @"X:\lrs\PW1.db"
			};
		}

//---------------------------------------------------------------------------------------

		public bool PopulateWithTestData(string fn) {
			db.Database.EnsureDeleted();
			db.Database.EnsureCreated();
			var imp = new ImportData();
			bool bOK = imp.Import(fn);
#if false   // For testing
			foreach (string key in imp.UserValues.Keys.OrderBy(key => key)) {
				Console.WriteLine(key);
				foreach (ImportUserInfo val in imp.UserValues[key]) {
					Console.WriteLine($"\tSiteName: {val.SiteName}, SiteUrl: {val.SiteUrl}," +
						$" LogonID: {val.LogonID}, Password: {val.Password}, Comment: {val.comment}");
				}
			}
#endif
			if (! bOK) { return false; }
			DumpUserInfo(imp.UserValues);
			SaveChanges(imp.UserValues);

			TestQueries();

			return true;
		}

//---------------------------------------------------------------------------------------

		private void DumpUserInfo(Dictionary<string, List<ImportUserInfo>> userValues) {
			var keys = userValues.Keys.OrderBy(key => key);
			foreach (string key in keys) {
				Box($"Dumping User: {key}");
				foreach (ImportUserInfo val in userValues[key]) {
					val.Dump();
				}
			}
		}

//---------------------------------------------------------------------------------------

		private void Box(string s, int width = 40, char padChar = '-') {
			string Padding = "".PadRight(width, padChar);
			Console.WriteLine(Padding);
			Console.WriteLine(s);
			Console.WriteLine(Padding);
		}

//---------------------------------------------------------------------------------------

		private void SaveChanges(Dictionary<string, List<ImportUserInfo>> userValues) {
			foreach (string key in userValues.Keys.OrderBy(key => key)) {
				var usr = User.Add(key)!;
				TheApp.db.SaveChanges();        // To get UserId field populated
				Box(key);
				foreach (ImportUserInfo val in userValues[key]) {
					var site = Site.Add(usr.UserId, val)!;
					TheApp.db.SaveChanges();    // To get SiteId
					if (!( site is null)) {
						LogPass.Add(usr.UserId, site.SiteId, val);
					}
				}
			}
			TheApp.db.SaveChanges();
		}

//---------------------------------------------------------------------------------------

		private void TestQueries() {
			TestQueryUsers();
			TestQueryUsersSites();
			TestQueryUsersSitesLogPass();
		}

//---------------------------------------------------------------------------------------

		private static void TestQueryUsers() {
			Console.WriteLine("***** TestQueryUsers *****");
			var qryUsers = from user in TheApp.db.Users
						   select user;
			foreach (var item in qryUsers) {
				Console.WriteLine($"{item.UserId}: {item.UserName}");
			}
			Console.WriteLine();
		}

//---------------------------------------------------------------------------------------

		private static void TestQueryUsersSites() {
			Console.WriteLine("***** TestQueryUsersSites *****");
			var qrySitesForUsers = from user in db.Users
								   from site in db.Sites
								   where site.UserId == user.UserId
								   select new {
									   user.UserId,
									   user.UserName,
									   site.SiteId,
									   site.SiteName,
									   site.SiteUrl
								   };
			Console.WriteLine("UserId  UserName  SiteId  SiteName  SiteUrl");
			foreach (var item in qrySitesForUsers) {
				Console.WriteLine($"{item.UserId,6}  " +
					$"{item.UserName,8}  " +
					$"{item.SiteId,6}  " +
					$"{item.SiteName,-8}  " +
					$"{item.SiteUrl}");
			}
			Console.WriteLine();
		}

//---------------------------------------------------------------------------------------

		private static void TestQueryUsersSitesLogPass() {
			Console.WriteLine("***** TestQueryUsersSitesLogPass *****");
			var qry = from user in db.Users
					  from site in db.Sites
					  from logpass in db.LogPasses
					  where user.UserId == site.UserId
					  &&   user.UserId  == logpass.UserId
					  &&   site.SiteId  == logpass.SiteId
					  select new {
						  user.UserId,
						  user.UserName,
						  site.SiteName,
						  site.SiteId,
						  site.SiteUrl,
						  logpass.LogPassId,
						  logpass.LogonID,
						  logpass.Password
					  };
			Console.WriteLine("UserId  UserName  SiteId  SiteName  SiteUrl  LogPassId  LogonID  Password");
			foreach (var item in qry) {
				Console.WriteLine($"{item.UserId,6}  " +
					$"{item.UserName,8}  " +
					$"{item.SiteId,6}  " +
					$"{item.SiteName,-8}  " +
					$"{item.SiteUrl}  " +
					$"{item.LogPassId,9}  " +
					$"{item.LogonID,-7}  " +
					$"{item.Password}");
			}
			Console.WriteLine();
		}
	}
}

