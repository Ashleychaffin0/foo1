using Microsoft.EntityFrameworkCore;
// using Microsoft.Data.Sqlite;

namespace PwSites {
	public class PwSitesContext : DbContext {
		public DbSet<Site>			Site			{ get; set; }
		public DbSet<UidPW>			UidPW			{ get; set; }
		public DbSet<MapSiteToUidPW> MapSiteToUidPW	{ get; set; }

//---------------------------------------------------------------------------------------

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			optionsBuilder
				.UseSqlite(@"Data Source=g:\lrs\SitePw.db;");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Site>().ToTable("TblSite");
			modelBuilder.Entity<UidPW>().ToTable("TblUidPW");
			modelBuilder.Entity<MapSiteToUidPW>().ToTable("TblMapSiteToUidPW");
		}
	}
}
