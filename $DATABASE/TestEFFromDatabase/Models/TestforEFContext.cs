using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using TestEFFromDatabase.Models.Mapping;

namespace TestEFFromDatabase.Models
{
    public partial class TestforEFContext : DbContext
    {
        static TestforEFContext()
        {
            Database.SetInitializer<TestforEFContext>(null);
        }

        public TestforEFContext()
            : base("Name=TestforEFContext")
        {
        }

        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<tblArticle> tblArticles { get; set; }
        public DbSet<tblAuthorList> tblAuthorLists { get; set; }
        public DbSet<tblAuthor> tblAuthors { get; set; }
        public DbSet<tblIssue> tblIssues { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new sysdiagramMap());
            modelBuilder.Configurations.Add(new tblArticleMap());
            modelBuilder.Configurations.Add(new tblAuthorListMap());
            modelBuilder.Configurations.Add(new tblAuthorMap());
            modelBuilder.Configurations.Add(new tblIssueMap());
        }
    }
}
