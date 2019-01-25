using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LRSLibraryBooks {

	public class MediaType {
		public int ID { get; set; }
		[MaxLength(10)]
		[Required]
		public string MediaName { get; set; }
		// e.g. Other, Book, eBook,	DVD, BluRay

//---------------------------------------------------------------------------------------

		public MediaType() {
			// Empty ctor
		}

//---------------------------------------------------------------------------------------

		public MediaType(string MediaName) {
			this.MediaName = MediaName;
		}

//---------------------------------------------------------------------------------------

		public static bool Add(LibContext ctx, string MediaName) {
			MediaName = MediaName.Trim();
			var qry = from m in ctx.MediaTypes
					  where m.MediaName == MediaName
					  select m.MediaName;
			bool bExists = qry.Any();
			if (!bExists) {
				ctx.Add(new MediaType(MediaName));
				ctx.SaveChanges(true);
				return true;
			}
			return false;
		}
	}
}


