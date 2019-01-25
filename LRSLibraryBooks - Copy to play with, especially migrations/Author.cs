using System.ComponentModel.DataAnnotations;

namespace LRSLibraryBooks {
	public class Author {
		public int ID { get; set; }
		[MaxLength(30)]
		[Required]
		public string AuthorFirstName { get; set; }
		[MaxLength(30)]
		[Required]
		public string AuthorLastName  { get; set; }

//---------------------------------------------------------------------------------------

		public Author(string FirstName, string LastName) {
			AuthorFirstName = FirstName;
			AuthorLastName  = LastName;
		}

//---------------------------------------------------------------------------------------

		public Author() {
			// Empty ctor
		}
	}
}
