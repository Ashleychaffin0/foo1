using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LRSLibraryBooks {
	public class Genre {
		public int ID { get; set; }
		[MaxLength(20)]
		[Required]
		public string GenreName { get; set; }
		// e.g None, SciFi,	Mystery, Physics, Math,	Science, Humor,	Food

//---------------------------------------------------------------------------------------

		public Genre() {
			// Empty ctor
		}

//---------------------------------------------------------------------------------------

		public Genre(string GenreName) {
			this.GenreName = GenreName;
		}

//---------------------------------------------------------------------------------------

		public static bool Add(LibContext ctx, string GenreName) {
			GenreName = GenreName.Trim();
			var qry = from g in ctx.Genres
					  where g.GenreName == GenreName
					  select g.GenreName;
			bool bExists = qry.Any();
			if (!bExists) {
				ctx.Add(new Genre(GenreName));
				ctx.SaveChanges(true);
				return true;
			}
			return false;
		}

//---------------------------------------------------------------------------------------

		// TODO: Needed?
		public static bool AddGenre(LibContext ctx, Genre Gen) => Add(ctx, Gen.GenreName);
	}
}
