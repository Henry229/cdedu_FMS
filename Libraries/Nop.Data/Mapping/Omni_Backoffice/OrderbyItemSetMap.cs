using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class OrderbyItemSetMap : EntityTypeConfiguration<OrderbyItemSet>
    {
        public OrderbyItemSetMap()
        {
            this.ToTable("omni_OrderbyItemSet");
            this.HasKey(s => s.Id);
            this.Property(s => s.Order_Id).IsRequired();
            this.Property(s => s.ItemSet_Id).IsRequired();
            this.Property(s => s.Qty).IsRequired();
            this.Property(s => s.Qty_Teacher).IsRequired();
            this.Property(s => s.Qty_NewBook).IsRequired();
            this.Property(s => s.Course_Id);
            this.Property(s => s.Week).IsRequired();
            this.Property(s => s.is_Half).IsRequired();
            this.Property(s => s.Remarks);
            this.Property(s => s.reg_date).IsRequired();
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
