using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;
namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class StudentSummaryMap : EntityTypeConfiguration<StudentSummary>
    {
        public StudentSummaryMap()
        {
            this.ToTable("vw_StudentSummary");
            this.HasKey(s => s.Id);
            this.Property(s => s.Branch).IsRequired().HasMaxLength(2);
            this.Property(s => s.Year).IsRequired().HasMaxLength(4);
            this.Property(s => s.Term).IsRequired().HasMaxLength(4);
            this.Property(s => s.Book);
            this.Property(s => s.Book);
            this.Property(s => s.Course);
            this.Property(s => s.Primary);
            this.Property(s => s.Secondary);

        }
    }
}
