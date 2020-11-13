using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace PW_1 {
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
		public static User? Add(string name) {
			if (IsUserDefined(name)) { return null; }
			var user = new User(name);
			TheApp.db.Users.Add(user);
			return user;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Given a user name, sees if it exists in the database
		/// </summary>
		/// <param name="UserName">The User's name</param>
		/// <returns>true if it exists, false otherwise</returns>
		public static bool IsUserDefined(string UserName) {
			User? isTThere = TheApp.db.Users.Where(u => u.UserName == UserName).FirstOrDefault();
			return isTThere != null;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Given a user name, gets its UserId
		/// </summary>
		/// <param name="name">The user's name (e.g. "LRS")</param>
		/// <returns>The UserID autoassigned when the user was added to the database</returns>
		public static int GetUid(string name) => TheApp.db.Users.Where(u => u.UserName == name).First().UserId;
	}
}

