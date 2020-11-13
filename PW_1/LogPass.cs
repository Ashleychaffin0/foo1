using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PW_1 {
	// Represents a Logon/Password pair

	// Note: The methods in this class are almost identical (other than referencing
	//		 different fields) to those in User.cs. Please see comments there.

	[Table("tblLogPasses")]
	public class LogPass {
		public int LogPassId { get; set; }
		[Required]
		public string LogonID { get; set; }
		[Required]
		public string Password { get; set; }
		[Required]
		public int UserId { get; set; }
		[Required]
		public int SiteId { get; set; }

//---------------------------------------------------------------------------------------

		public LogPass() {
			LogonID  = "";
			Password = "";
		}

//---------------------------------------------------------------------------------------

		public static LogPass? Add(int userid, int siteid, ImportUserInfo userinfo) {
			// if (IsLogPassDefined(userinfo.LogonID)) { return null; }
			var logpass = new LogPass() {
				LogonID  = userinfo.LogonID,
				Password = userinfo.Password,
				UserId   = userid,
				SiteId   = siteid
			};
			TheApp.db.LogPasses.Add(logpass);
			return logpass;
		}

#if false
		//---------------------------------------------------------------------------------------

		/// <summary>
		/// Given a LogonID, sees if it exists in the database
		/// </summary>
		/// <param name="UserName">The LogonID</param>
		/// <returns>true if it exists, false otherwise</returns>
		public static bool IsLogPassDefined(string logonID) {
			// TODO: See problem in site.cs
			LogPass? isTThere = TheApp.db.LogPasses.Where(lp => lp.LogonID == logonID).FirstOrDefault();
			return isTThere != null;
		}
#endif
	}
}
