using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LRSLibraryBooks {
	public class UserID {
		public int		ID		{ get; set; }
		[MaxLength(10)]
		[Required]
		public string	OwnerName	{ get; set; }

//---------------------------------------------------------------------------------------

		public UserID() {
			// Empty ctor
		}

//---------------------------------------------------------------------------------------

		public UserID(string OwnerName) {
			this.OwnerName = OwnerName;
		}

//---------------------------------------------------------------------------------------

		public static bool Add(LibContext ctx, string OwnerName) {
			OwnerName = OwnerName.Trim();
			var qry = from o in ctx.Owners
					  where o.OwnerName == OwnerName
					  select o.OwnerName;
			bool bExists = qry.Any();
			if (!bExists) {
				ctx.Add(new UserID(OwnerName));
				ctx.SaveChanges(true);
				return true;
			}
			return false;
		}
	}
}
