using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PWW {
	// Represents a site associated with a User. For example, different
	// users may have different ID/Password for the same site

	// Note: The methods in this class are almost identical (other than referencing
	//		 different fields) to those in User.cs. Please see comments there.

	[Table("tblSites")]
	public class Site {
		public int SiteId { get; set; }
		[Required]
		public string SiteName { get; set; }
		[Required]
		public string SiteUrl { get; set; }
		[Required]
		public int UserId { get; set; } // TODO:Set as foreign key?

//---------------------------------------------------------------------------------------

		public Site() {
			SiteName = "";
			SiteUrl = "";
		}

//---------------------------------------------------------------------------------------

		public static Site? Add(PWContext db, int userid, ImportUserInfo userinfo) {
			if (IsSiteDefined(db, userinfo.SiteName, userid)) { return null; }
			var site = new Site() {
				SiteName = userinfo.SiteName,
				SiteUrl = userinfo.SiteUrl,
				UserId = userid
			};
			db.Sites.Add(site);
			return site;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Given a site name, sees if it exists in the database
		/// </summary>
		/// <param name="UserName">The User's name</param>
		/// <returns>true if it exists, false otherwise</returns>
		public static bool IsSiteDefined(PWContext db, string siteName, int userid) {
			Site? isTThere = db.Sites.Where(s => (s.SiteName == siteName) && (s.UserId == userid)).FirstOrDefault();
			return isTThere != null;
		}

//---------------------------------------------------------------------------------------

		public override string ToString() => SiteName;

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Given a user name, gets its UserId
		/// </summary>
		/// <param name="name">The user's name (e.g. "LRS")</param>
		/// <returns>The UserID autoassigned when the user was added to the database</returns>
#if false  // TODO: GetSiteId
	public static int GetSiteId(string name) => TheApp.db.Sites.Where(u => u.UserName == name).First().UserId;
#endif
	}
}

