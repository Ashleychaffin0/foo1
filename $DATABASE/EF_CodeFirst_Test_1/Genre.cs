using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_CodeFirst_Test_1 {
	class Genre {
		public int		GenreID { get; set; }
		public string	GenreName { get; set; }

//---------------------------------------------------------------------------------------

		public Genre() {
			// Empty
		}

//---------------------------------------------------------------------------------------

		public Genre(string GenreName) {
			this.GenreName = GenreName;
		}
	}
}
