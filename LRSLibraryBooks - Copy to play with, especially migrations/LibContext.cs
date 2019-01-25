using System;
using System.Configuration;

using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;				// TODO: Needed?
using Microsoft.Extensions.Logging;						// TODO: Needed?
using Microsoft.Extensions.DependencyInjection;         // TODO: Needed?

using Microsoft.Data.Entity.Migrations;
// using Microsoft.Data.Entity.Migrations.Builders;

// Note: First, Install-Package EntityFramework.Core

// Note: According to https://channel9.msdn.com/Blogs/Seth-Juarez/Migrations-in-Entity-Framework-7-with-Brice-Lambson,
//		 to do migrations in EF 7, you must first issue
//			Install-Package EntityFramework.Commands -pre


namespace LRSLibraryBooks {
	public class LibContext : DbContext {
		public DbSet<Book>		Books		{ get; set; }
		public DbSet<Author>	Authors		{ get; set; }
		public DbSet<Genre>		Genres		{ get; set; }
		public DbSet<MediaType>	MediaTypes	{ get; set; }
		public DbSet<UserID>	Owners		{ get; set; }

		public bool bIsBackup;

//---------------------------------------------------------------------------------------

		public LibContext(bool bIsBackup = false) {
			this.bIsBackup = bIsBackup;
		}

//---------------------------------------------------------------------------------------

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			string ConnectionStringName = bIsBackup ? "LRSLibraryBooksBackup" : "LRSLibraryBooks";
			var ConnectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ToString();
			// Note: To get UseSqlServer, must Nuget EntityFramework.MicrosoftSqlServer
			optionsBuilder.UseSqlServer(ConnectionString);

			// Note: Alternatively
/*
	optionsBuilder.UseSqlServer("" +
		new SqlConnectionStringBuilder {
			DataSource = @"(localdb)\MSSQLlLocalDB",
			InitialCatalog = "PeopleDatabase",
			IntegratedSecurity = true,
			MultipleActiveResultsSets = true)
		}):
*/
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			base.OnModelCreating(modelBuilder);
			// This next line says that a Book Title is unique. Of course this isn't
			// necessarily so, but should work for now.
			// TODO: Can we do the following with a data annotation instead?
			modelBuilder.Entity<Book>()     .HasIndex("Title")    .IsUnique(true);
			modelBuilder.Entity<Author>()	.HasIndex("AuthorLastName", "AuthorFirstName").IsUnique(true);
			modelBuilder.Entity<Genre>()    .HasIndex("GenreName").IsUnique(true);
			modelBuilder.Entity<MediaType>().HasIndex("MediaName").IsUnique(true);
			modelBuilder.Entity<UserID>()	.HasIndex("OwnerName").IsUnique(true);

			if (bIsBackup) {
				modelBuilder.Entity<Genre>().HasKey();	// No primary key for backup,
														// since we want to be able to
														// insert the Primary Keys
														// ourselves
			}
		}
	}

#if false
	public static class DbContextExtensions {
		public static void LogToConsole(this DbContext context) {
			var contextServices = ((IInfrastructure<IServiceProvider>)context).Instance;
			var loggerFactory = contextServices.GetRequiredService<ILoggerFactory>();
			loggerFactory.AddConsole(LogLevel.Verbose);
		}
	}
#endif
}

#if false
namespace MigrationsSandbox {
	public partial class AddPersonName : Migration {
		public override void Up(MigrationBuilder migration) {

		}
	}
}
#endif

