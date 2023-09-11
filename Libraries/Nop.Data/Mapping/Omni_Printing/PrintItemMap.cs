using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Printing;
namespace Nop.Data.Mapping.Omni_Printing
{
    public partial class PrintItemMap : EntityTypeConfiguration<PrintItem>
    {
        public PrintItemMap()
        {
            this.ToTable("omnipr_Item");
            this.HasKey(s => s.Id);
            this.Property(s => s.ItemName).IsRequired().HasMaxLength(50);
            this.Property(s => s.Remarks);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
