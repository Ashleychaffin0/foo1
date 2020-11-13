using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace PW_1a {
	class Program {
		static void Main(string[] args) {
			using (var db = new PWContext()) {

				db.Database.EnsureDeleted();
				db.Database.EnsureCreated();
				var lrs = new User() {
					UserName = "LRS"
				};
				db.Users.Add(lrs);
				db.SaveChanges();
			}
		}
	}
	public class User {
		public int UserId { get; set; }
		[Required]
		public string UserName { get; set; }
		[Required]
		public int SiteId { get; set; }
	}

	public class Site {
		public int SiteId { get; set; }
		[Required]
		public int UrlId { get; set; }
		[Required]
		public int UserId { get; set; }
		[Required]
		public string LoginID { get; set; }
		[Required]
		public string Password { get; set; }
	}

	public class Question {
		public int QuestionId { get; set; }
		[Required]
		public int UserId { get; set; }
		[Required]
		public int SiteId { get; set; }
		[Required]
		public string Q { get; set; }
		[Required]
		public string A { get; set; }
	}

	public class Url {
		public int UrlId { get; set; }
		[Required]
		public string SiteUrl { get; set; }
		[Required]
		public string SiteName { get; set; }
	}

	public class PWContext : DbContext {
		// Note: All of these must be Properties, not fields
		public DbSet<User> Users { get; set; }
		public DbSet<Url> Urls { get; set; }
		public DbSet<Question> Questions { get; set; }
		public DbSet<Site> Sites { get; set; }

//---------------------------------------------------------------------------------------

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Url>().ToTable("tblUrls");
			modelBuilder.Entity<User>().ToTable("tblUsers");
			modelBuilder.Entity<Site>().ToTable("tblSites");
			modelBuilder.Entity<Question>().ToTable("tblQuestions");
			base.OnModelCreating(modelBuilder);
		}

//---------------------------------------------------------------------------------------

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			optionsBuilder.UseSqlite(@"Data Source=G:\lrs\PW1a.db");
		}
	}


}

