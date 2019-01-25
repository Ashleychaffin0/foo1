using System;
using System.ComponentModel.DataAnnotations;

namespace LRSLibraryBooks {
	public class Book {
		public int		 BookID			{ get; set; }
		[Required]
		[MaxLength(100)]
		public string	 Title			{ get; set; }
		[Required]
		public string	 Owner			{ get; set; }		// BGA / LRS / WLS
		public int		 StoppedAtPage	{ get; set; }       // Page #
		[Required]
		// Note: This next field, to be a Foreign Key, must (a) have the name
		//		 <Foreign Table Name>+"ID". In this case, "GenreID". And (b) must be the
		//		 same type as the PK in that table. So this field must be of type int,
		//		 not Genre.
		public int		 GenreID		{ get; set; }
		[Required]
		public int		 MediumID		{ get; set; }
		public bool		 HasFullyRead	{ get; set; }
		public bool		 IsAnthology	{ get; set; }
		public DateTime? DueDate		{ get; set; }

		// public List<Author> Authors { get; set; }
		[Required]
		public Author	 Author			{ get; set; }

		// TODO: Multi-titles (e.g. anthologies (including different genres/story)
		// TODO: Not all books have an "Author". For example, anotholgies. OTOH an
		//		 anthology might have an Editor.
		// TODO: Some books have multiple authors (e.g. Niven & Pournelle)
		// TODO: 

//---------------------------------------------------------------------------------------

		public Book() {
			// Empty ctor
		}
	}
}
