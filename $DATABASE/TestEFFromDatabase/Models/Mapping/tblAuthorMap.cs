using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TestEFFromDatabase.Models.Mapping
{
    public class tblAuthorMap : EntityTypeConfiguration<tblAuthor>
    {
        public tblAuthorMap()
        {
            // Primary Key
            this.HasKey(t => t.AuthorID);

            // Properties
            this.Property(t => t.Author_Name)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("tblAuthors");
            this.Property(t => t.AuthorID).HasColumnName("AuthorID");
            this.Property(t => t.Author_Name).HasColumnName("Author Name");
        }
    }
}
