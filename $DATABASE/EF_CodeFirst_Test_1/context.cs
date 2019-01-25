using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_CodeFirst_Test_1 {
	class Context : DbContext {
		public DbSet<Issue>	   Issues	  { get; set; }
		public DbSet<Article>  Articles	  { get; set; }
		public DbSet<Genre>	   Genres	  { get; set; }

//---------------------------------------------------------------------------------------

		public Context() : base() {
			// Empty
		}
	}
}
