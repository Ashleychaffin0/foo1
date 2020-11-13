using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PW_1 {
	// Represents a site associated with a User. For example, different
	// users may have different ID/Password for the same site

	// Note: The methods in this class are almost identical (other than referencing
	//		 different fields) to those in User.cs. Please see comments there.

	[Table("tblSites")]
	public class Site {
		public int	  SiteId	{ get; set; }
		[Required]
		public string Category { get; set; }
		[Required]
		public string SiteName { get; set; }
		[Required]
		public string SiteUrl	{ get; set; }
		[Required]
		public string LogonID { get; set; }
		[Required]
		public string Password { get; set; }
		[Required]
		public int	  UserId	{ get; set; }	// TODO:Set as foreign key?
		[Required]
		public string Comment	{ get; set; }

//---------------------------------------------------------------------------------------

		public Site() {
			Category = "";
			SiteName = "";
			SiteUrl  = "";
			LogonID  = "";
			Password = "";
			Comment  = "";
		}

//---------------------------------------------------------------------------------------

		public static Site? Add(int userid, ImportUserInfo userinfo) {
			var SiteExists = IsSiteDefined(userinfo.SiteName);
			if (SiteExists != null) { return SiteExists; }
			var site = new Site() {
				UserId   = userid,
				Category = userinfo.Category,
				SiteName = userinfo.SiteName,
				SiteUrl  = userinfo.SiteUrl,
				LogonID  = userinfo.LogonID,
				Password = userinfo.Password,
				Comment  = userinfo.Comment.ToString()
			};
			TheApp.db.Sites.Add(site);			
			return site;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Given a site name, sees if it exists in the database
		/// </summary>
		/// <param name="UserName">The User's name</param>
		/// <returns>true if it exists, false otherwise</returns>
		public static Site? IsSiteDefined(string siteName) {
			Site? isItThere = TheApp.db.Sites.Where(u => u.SiteName == siteName).FirstOrDefault();
			// return isTThere != null;
			return isItThere;
		}

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

