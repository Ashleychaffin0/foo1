// Copyright (c) 2020 by Larry Smith
//
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace PWW {
	// Represents a user
	[Table("tblCategories")]
	public class Category {
		public int CategoryId { get; set; }
		[Required]
		public string CategoryName { get; set; }
		public int UserId { get; set; }

//---------------------------------------------------------------------------------------

		public Category() => CategoryName = "";

//---------------------------------------------------------------------------------------

		public Category(string name) => CategoryName = name;

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Adds the specified name to tblCategories, if necessary
		/// </summary>
		/// <param name="name">The User Name</param>
		/// <returns>null if the name already exists, else the User instance</returns>
		public static Category? Add(PWContext db, int userid, ImportUserInfo userinfo) {
			if (IsCategoryDefined(db, userinfo.Category, userid)) { return null; }
			var cat = new Category() {
				CategoryName = userinfo.Category,
				UserId = userid
			};
			db.Categories.Add(cat);
			return cat;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Given a user name, sees if it exists in the database
		/// </summary>
		/// <param name="UserName">The User's name</param>
		/// <returns>true if it exists, false otherwise</returns>
		public static bool IsCategoryDefined(PWContext db, string categoryName, int userid) {
			Category? isThere = db.Categories.Where(cat => (cat.CategoryName == categoryName)
				&& (cat.UserId == userid)).FirstOrDefault();
			return isThere != null;
		}

//---------------------------------------------------------------------------------------

		public override string ToString() => CategoryName;
	}
}

