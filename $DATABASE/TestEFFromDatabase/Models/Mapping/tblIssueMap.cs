using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace TestEFFromDatabase.Models.Mapping
{
    public class tblIssueMap : EntityTypeConfiguration<tblIssue>
    {
        public tblIssueMap()
        {
            // Primary Key
            this.HasKey(t => t.IssueID);

            // Properties
            this.Property(t => t.Cover_Description)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("tblIssues");
            this.Property(t => t.IssueID).HasColumnName("IssueID");
            this.Property(t => t.Issue_Date).HasColumnName("Issue Date");
            this.Property(t => t.Cover_Description).HasColumnName("Cover Description");
        }
    }
}
