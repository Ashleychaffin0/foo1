using System;
using System.Collections.Generic;
using System.Configuration;
// using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.Entity;
using Microsoft.Data.Entity.ChangeTracking;
using Microsoft.Data.Entity.Design;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Internal;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Query;
using Microsoft.Data.Entity.Storage;
using Microsoft.Data.Entity.Update;
using Microsoft.Data.Entity.ValueGeneration;

using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.OptionsModel;
// using EntityFramework7.Models;	// Seems to be part of Npgsql  (.Net Provider for
//									// PostgreSQL


namespace LRSLibraryBooks {
	public class Book {
		public int			BookID;
		public string		Title;
		public int			StoppedAtPage;      // Page #
		public GenreType	Genre;
		public MediumType	Medium;
		public bool			HasFullyRead;

		public virtual List<Author> Authors { get; set;	}

		public enum GenreType {
			None,
			SciFi,
			Mystery,
			Physics,
			Math,
			Science,
			Humor,
			Food
		}

		public enum MediumType {
			Other,
			Book,
			eBook,
			DVD,
			BluRay
		}
	}

	public class Author {
		public int AuthorID;
		public string AuthorName;

		public virtual Book Book { get; set; }
	}

	public class LibContext : DbContext {
		public DbSet<Book>		Books { get; set; }
		public DbSet<Author>	Authors { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			var connectionString = ConfigurationManager.ConnectionStrings["LRSLibraryBooks"];
			optionsBuilder.UseSqlServer();
			
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			base.OnModelCreating(modelBuilder);
		}
	}
}
