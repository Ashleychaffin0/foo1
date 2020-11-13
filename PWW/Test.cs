// Copyright (c) 2020 by Larry Smith
//

using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

// See g:\OneDrive\$Dev\$$$ C# Ongoing Projects\LRSLibraryBooks\LibContext.cs

namespace PWW {
	public class TestApp {
		TestContext db;

//---------------------------------------------------------------------------------------

		public TestApp() {
			db = new TestContext();
			db.Database.EnsureDeleted();
			db.Database.EnsureCreated();
		}

//---------------------------------------------------------------------------------------

		public void Doit() {
			var a1 = new Author("Heinlein");
			var a2 = new Author("Asimov");
			var a3 = new Author("Heinlein");

			db.Authors.Add(a1);
			db.Authors.Add(a2);
			db.SaveChanges();

			var xxx = db.Authors.Where(p => p.AuthorName == "Heinlein").FirstOrDefault();
			if (xxx is null) {
				db.Authors.Add(a3);
			}

			a1.Add(new Book("Time Enough for Love"));
			a1.Add(new Book("Beyond This Horizon"));

			a2.Add(new Book("Foundation"));
			a2.Add(new Book("The End of Eternity"));
			db.SaveChanges();
		}
	}

	public class TestContext : DbContext {
#pragma warning disable CS8618 // Non-nullable property is uninitialized
		public DbSet<Author> Authors { get; set; }
		public DbSet<Book>	 Books { get; set; }
#pragma warning restore CS8618

//---------------------------------------------------------------------------------------

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Author>().HasIndex(a => a.AuthorName).IsUnique();
			// Call the base method if we've modified the ModelBuilder above
			base.OnModelCreating(modelBuilder);
		}

//---------------------------------------------------------------------------------------

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			optionsBuilder.UseSqlite($@"Data Source=G:\lrs\Testdb.db");
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class Author {
		public int AuthorId { get; set; }
		public string AuthorName { get; set; }
		public List<Book> Books { get; set; }

//---------------------------------------------------------------------------------------

		public Author(string authorName) {
			AuthorName = authorName;
			Books = new List<Book>();
		}

//---------------------------------------------------------------------------------------

		public void Add(Book book) {
			Books.Add(book);
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class Book {
		public int BookId { get; set; }
		public string BookName { get; set; }

//---------------------------------------------------------------------------------------

		public Book() {
			BookName = "N/A";
		}

//---------------------------------------------------------------------------------------

		public Book(string name) {
			BookName = name;
		}
	}
}
