using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace PWW {
	// Represents a user
	[Table("tblUsers")]
	public class User {
		public int UserId { get; set; }
		[Required]
		public string UserName { get; set; }

//---------------------------------------------------------------------------------------

		public User() => UserName = "";

//---------------------------------------------------------------------------------------

		public User(string name) => UserName = name;

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Adds the specified name to tblUsers, if necessary
		/// </summary>
		/// <param name="name">The User Name</param>
		/// <returns>null if the name already exists, else the User instance</returns>
		public static User? Add(PWContext db, string name) {
			if (IsUserDefined(db, name)) { return null; }
			var user = new User(name);
			db.Users.Add(user);
			return user;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Given a user name, sees if it exists in the database
		/// </summary>
		/// <param name="UserName">The User's name</param>
		/// <returns>true if it exists, false otherwise</returns>
		public static bool IsUserDefined(PWContext db, string UserName) {
			User? isTThere = db.Users.Where(u => u.UserName == UserName).FirstOrDefault();
			return isTThere != null;
		}

//---------------------------------------------------------------------------------------

		public override string ToString() => UserName;
	}
}

