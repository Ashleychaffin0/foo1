using Microsoft.EntityFrameworkCore;

namespace PwSites2 {
	public class SiteContext : DbContext {
		// See https://entityframeworkcore.com/providers-sqlite
		// Also need to install NuGet package: Microsoft.EntityFrameworkCore.Tools
		public DbSet<Site> Sites { get; set; }
		public DbSet<UidPw> UidPws { get; set; }
		public DbSet<MapSiteToUidPw> MapSiteToUidPws { get; set; }

//---------------------------------------------------------------------------------------

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			optionsBuilder
				// Note: Absolute path works. Relative path doesn't. Is executable
				//		 folder being cleaned (i.e. deleted) during compilation???
				.UseSqlite(@"Data Source=G:\LRS\PwSites.db;");
				//.UseSqlite(@"Data Source=:memory:;");
		}

//---------------------------------------------------------------------------------------

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Site>().ToTable("TblSites");
			modelBuilder.Entity<UidPw>().ToTable("TblUidPws");
			modelBuilder.Entity<MapSiteToUidPw>().ToTable("TblMapSiteToUidPws");
		}
	}
}
