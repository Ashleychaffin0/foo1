using Microsoft.EntityFrameworkCore;

namespace GetGoComics {
	public class StripContext : DbContext {
		// Note: All of these must be Properties, not fields
#pragma warning disable CS8618 // Non-nullable property is uninitialized
		public DbSet<StripInfo> StripInfos { get; set; }
#pragma warning restore CS8618

//---------------------------------------------------------------------------------------

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			// The next line (and others like it) not needed because of [Table] annotation
			// modelBuilder.Entity<Url>().ToTable("tblUrls");

			// The following lines are example of what can be put here, courtesy of
			// https://www.roundthecode.com/dotnet/entity-framework/is-entity-framework-core-quicker-than-dapper

#if false
		// I suspect that many/most/all of these can be done with annotations
		modelBuilder.Entity<BlogCategory>().HasKey(entity => entity.Id);
 
		modelBuilder.Entity<BlogCategory>()
			.HasOne(blogCategory => blogCategory.Blog)
			.WithMany(blog => blog.BlogCategories)
			.HasPrincipalKey(blog => blog.Id)
			.HasForeignKey(blogCategory => blogCategory.BlogId);
 
		modelBuilder.Entity<BlogCategory>()
			.HasOne(blogCategory => blogCategory.Category)
			.WithMany()
			.HasPrincipalKey(category => category.Id)
			.HasForeignKey(blog => blog.CategoryId);

#endif

			// Call the base method if we've modified the ModelBuilder above
			base.OnModelCreating(modelBuilder);
		}

//---------------------------------------------------------------------------------------

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			optionsBuilder.UseSqlite($@"Data Source={TheApp.DbFilename}");
		}
	}
}
