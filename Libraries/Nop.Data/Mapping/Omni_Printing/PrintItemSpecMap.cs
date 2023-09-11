using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Printing;
namespace Nop.Data.Mapping.Omni_Printing
{
    public partial class PrintItemSpecMap : EntityTypeConfiguration<PrintItemSpec>
    {
        public PrintItemSpecMap()
        {
            this.ToTable("omnipr_ItemSpec");
            this.HasKey(s => s.Id);
            this.Property(s => s.Item_Id).IsRequired();
            this.Property(s => s.Spec_Id).IsRequired();
            this.Property(s => s.SortOrder).IsRequired();
            this.Property(s => s.YN_Use).IsRequired().HasMaxLength(1);
            this.Property(s => s.Remarks);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
