// Copyright (c) 2020 by Larry Smith
//

// See https://docs.microsoft.com/en-us/microsoft-edge/webview2/gettingstarted/win32

#if DEBUG
#define DUMPINFO
#endif

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PWW {
	public class TheApp {
		public PWContext db = new PWContext();

		public static string DbFilename = "";
		public static string ImportFilename = @"G:\LRS\PWs.txt";    // TODO: OneDrive?

//---------------------------------------------------------------------------------------

		public TheApp() {
			DbFilename = Environment.UserName switch {
				// TODO: Put on to OneDrive
				"lrs5"    => @"G:\lrs\PW1.db",
				"BGA8700" => @"X:\lrs\PW1.db",
				_ => @"X:\lrs\PW1.db"
			};

#if DEBUG
			File.Delete(DbFilename);
#endif
			if (! File.Exists(DbFilename)) {
				PopulateDatabase(ImportFilename);
			}
		}

//---------------------------------------------------------------------------------------

		public bool PopulateDatabase(string fn) {
			db.Database.EnsureDeleted();
			db.Database.EnsureCreated();
			var imp = new ImportData(db);
			bool bOK = imp.Import(fn);
			if (bOK) { return false; }
			SaveChanges(imp.UserValues);

			TestQueries(db);

			return true;
		}

//---------------------------------------------------------------------------------------

		private void SaveChanges(Dictionary<string, List<ImportUserInfo>> userValues) {
			foreach (string key in userValues.Keys.OrderBy(key => key)) {
				var usr = User.Add(db, key)!;
				db.SaveChanges();        // To get UserId field populated
				Console.WriteLine(key);
				foreach (ImportUserInfo val in userValues[key]) {
					// 					Console.WriteLine($"\tSiteName: {val.SiteName}, SiteUrl: {val.SiteUrl}," +
					//						$" LogonID: {val.LogonID}, Password: {val.Password}, Comment: {val.comment}");
					Category.Add(db, usr.UserId, val);
					var site = Site.Add(db, usr.UserId, val)!;
					db.SaveChanges();    // To get SiteId
					LogPass.Add(db, usr.UserId, site.SiteId, val);
					Comment.Add(db, usr.UserId, site.SiteId, val);
				}
			}
			db.SaveChanges();
		}

//---------------------------------------------------------------------------------------

		// [Conditional("DUMPINFO")]
		public static void TestQueries(PWContext db) {
			TestDumpTables(db);
			TestQueryUsers(db);
			TestQueryUsersSites(db);
			TestQueryUsersSitesLogPass(db);
		}

//---------------------------------------------------------------------------------------

		private static void TestDumpTables(PWContext db) {
			Debug.WriteLine("***** TestDumpUsers *****");
			foreach (var user in db.Users) {
				Debug.WriteLine($"UserId: {user.UserId} UserName: {user.UserName}");
			}
			Debug.WriteLine("");
			Debug.WriteLine("***** TestDumpSites *****");
			foreach (var site in db.Sites) {
				Debug.WriteLine($"SiteId: {site.SiteId} SiteName: {site.SiteName}, UserId: {site.UserId}");
			}
			Debug.WriteLine("");
			Debug.WriteLine("***** TestDumpLogPasses *****");
			foreach (var lp in db.LogPasses) {
				Debug.WriteLine($"LogonID: {lp.LogonID} Password: {lp.Password} SiteId: {lp.SiteId}, UserId: {lp.UserId}");
			}
			Debug.WriteLine("");
			Debug.WriteLine("***** TestDumpComments *****");
			foreach (var comm in db.Comments) {
				Debug.WriteLine($"CommentID: {comm.CommentId} UserId: {comm.UserId} SiteId: {comm.SiteId}, Text: {comm.Text}");
			}
			Debug.WriteLine("");
			Debug.WriteLine("");
			Debug.WriteLine("***** TestDumpCategories *****");
			foreach (var cat in db.Categories) {
				Debug.WriteLine($"CommentID: {cat.CategoryId} UserId: {cat.UserId} SiteId: {cat.CategoryName}");
			}
			Debug.WriteLine("");
		}

//---------------------------------------------------------------------------------------

		private static void TestQueryUsers(PWContext db) {
			Debug.WriteLine("***** TestQueryUsers *****");
			var qryUsers = from user in db.Users
						   select user;
			foreach (var item in qryUsers) {
				Debug.WriteLine($"{item.UserId}: {item.UserName}");
			}
			Console.WriteLine();
		}

//---------------------------------------------------------------------------------------

		private static void TestQueryUsersSites(PWContext db) {
			Debug.WriteLine("***** TestQueryUsersSites *****");
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
			Debug.WriteLine("UserId  UserName  SiteId  SiteName  SiteUrl");
			foreach (var item in qrySitesForUsers) {
				Debug.WriteLine($"{item.UserId,6}  " +
					$"{item.UserName,8}  " +
					$"{item.SiteId,6}  " +
					$"{item.SiteName,-8}  " +
					$"{item.SiteUrl}");
			}
			Debug.WriteLine("");
		}

//---------------------------------------------------------------------------------------

		private static void TestQueryUsersSitesLogPass(PWContext db) {
			Debug.WriteLine("***** TestQueryUsersSitesLogPass *****");
			var qry = from user in db.Users
					  from site in db.Sites
					  from logpass in db.LogPasses
					  where user.UserId == site.UserId
						&& user.UserId == logpass.UserId
						&& site.SiteId == logpass.SiteId
					  select new {
						  user.UserId,
						  user.UserName,
						  site.SiteId,
						  site.SiteName,
						  site.SiteUrl,
						  logpass.LogPassId,
						  logpass.LogonID,
						  logpass.Password
					  };
			Debug.WriteLine("UserId  UserName  SiteId  SiteName  SiteUrl  LogPassId  LogonID  Password");
			foreach (var item in qry) {
				Debug.WriteLine($"{item.UserId,6}  " +
					$"{item.UserName,8}  " +
					$"{item.SiteId,6}  " +
					$"{item.SiteName,-8}  " +
					$"{item.SiteUrl}  " +
					$"{item.LogPassId,9}  " +
					$"{item.LogonID,-7}  " +
					$"{item.Password}");
			}
			Debug.WriteLine("");
		}
	}
}

