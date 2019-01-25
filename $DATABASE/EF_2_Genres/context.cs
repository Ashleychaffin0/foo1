using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_2_Genres {
	class Context : DbContext {

		public DbSet<Book>		Books	  { get; set; }
		public DbSet<Genre>		Genres	  { get; set; }

		public Context() : base(@"(localdb)\v11.0\EF2") {
			// Empty
		}
	}
}
