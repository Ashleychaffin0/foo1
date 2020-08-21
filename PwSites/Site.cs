using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
// using Microsoft.Data.Sqlite;

namespace PwSites {
	public class Site {
		[Key]
		public int		SiteId		{ get; set; }		// Primary Key
		public string	Name		{ get; set; }       // e.g. WLIW 21
		public string	Description { get; set; }
		public int		UidPwId		{ get; set; }       // FK: User ID and Password ID
	}
}
