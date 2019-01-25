using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_CodeFirst_Test_1 {
	class Author {
		public int		AuthorID  { get; set; }
		public string	FirstName { get; set; }
		public string	LastName  { get; set; }

//---------------------------------------------------------------------------------------

		public Author() {
			// Empty
		}
	}
}
