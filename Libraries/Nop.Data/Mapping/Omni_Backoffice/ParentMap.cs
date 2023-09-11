using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;
namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class ParentMap : EntityTypeConfiguration<Parent>
    {
        public ParentMap()
        {
            this.ToTable("omni_Parent");
            this.HasKey(s => s.Id);
            this.Property(s => s.FirstName).IsRequired().HasMaxLength(100);
            this.Property(s => s.LastName).IsRequired().HasMaxLength(100);
            this.Property(s => s.Branch).IsRequired().HasMaxLength(2);
            this.Property(s => s.PhoneNo).IsRequired().HasMaxLength(50);
            this.Property(s => s.MobileNo1).IsRequired().HasMaxLength(50);
            this.Property(s => s.MobileNo2).IsRequired().HasMaxLength(50);
            this.Property(s => s.Email).IsRequired().HasMaxLength(50);
            this.Property(s => s.Remarks);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
