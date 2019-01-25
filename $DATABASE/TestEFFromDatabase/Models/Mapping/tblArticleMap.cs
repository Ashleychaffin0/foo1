using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TestEFFromDatabase.Models.Mapping
{
    public class tblArticleMap : EntityTypeConfiguration<tblArticle>
    {
        public tblArticleMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ArticleID, t.IssueID });

            // Properties
            this.Property(t => t.ArticleID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.IssueID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("tblArticles");
            this.Property(t => t.ArticleID).HasColumnName("ArticleID");
            this.Property(t => t.ArticleName).HasColumnName("ArticleName");
            this.Property(t => t.AuthorListID).HasColumnName("AuthorListID");
            this.Property(t => t.IssueID).HasColumnName("IssueID");

            // Relationships
            this.HasRequired(t => t.tblIssue)
                .WithMany(t => t.tblArticles)
                .HasForeignKey(d => d.IssueID);

        }
    }
}
