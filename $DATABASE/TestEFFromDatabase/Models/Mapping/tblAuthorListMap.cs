using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TestEFFromDatabase.Models.Mapping
{
    public class tblAuthorListMap : EntityTypeConfiguration<tblAuthorList>
    {
        public tblAuthorListMap()
        {
            // Primary Key
            this.HasKey(t => t.AuthorListID);

            // Properties
            // Table & Column Mappings
            this.ToTable("tblAuthorList");
            this.Property(t => t.AuthorListID).HasColumnName("AuthorListID");
            this.Property(t => t.AuthorID).HasColumnName("AuthorID");
            this.Property(t => t.ArticleID).HasColumnName("ArticleID");

            // Relationships
            this.HasRequired(t => t.tblAuthor)
                .WithMany(t => t.tblAuthorLists)
                .HasForeignKey(d => d.AuthorID);

        }
    }
}
