using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
// using Microsoft.Data.Sqlite;

namespace PwSites {
	public class MapSiteToUidPW {
		[Key]
		public int UidPwId	{ get; set; }        // Primary Key
		public int IDID		{ get; set; }
		public int SiteId	{ get; set; }			// FK
	}
}
