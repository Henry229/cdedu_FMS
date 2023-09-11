using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Printing;
namespace Nop.Data.Mapping.Omni_Printing
{
    public partial class PrintSpecMap : EntityTypeConfiguration<PrintSpec>
    {
        public PrintSpecMap()
        {
            this.ToTable("omnipr_Spec");
            this.HasKey(s => s.Id);
            this.Property(s => s.SpecName).IsRequired().HasMaxLength(50);
            this.Property(s => s.Specification).IsRequired().HasMaxLength(255);
            this.Property(s => s.SortOrder).IsRequired();
            this.Property(s => s.YN_Use).IsRequired().HasMaxLength(1);
            this.Property(s => s.Remarks);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
