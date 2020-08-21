using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
// using Microsoft.Data.Sqlite;

namespace PwSites {
	public class UidPW {
		[Key]
		public int		UidPwId  { get; set; }        // Primary Key
		public string	UserID	 { get; set; }
		public string	Password { get; set; }
	}
}
