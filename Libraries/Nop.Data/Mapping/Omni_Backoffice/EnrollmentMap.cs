using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;
namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class EnrollmentMap: EntityTypeConfiguration<Enrollment>
    {
        public EnrollmentMap()
        {
            this.ToTable("omni_Enrollment");
            this.HasKey(s => s.Id);
            this.Property(s => s.BranchCode).IsRequired().HasMaxLength(2);
            this.Property(s => s.Course_Id).IsRequired();
            this.Property(s => s.Week).IsRequired();
            this.Property(s => s.Qty_Book);
            this.Property(s => s.Qty_Modified);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
