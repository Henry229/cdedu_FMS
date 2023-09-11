using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class Order_DMap : EntityTypeConfiguration<Order_D>
    {
        public Order_DMap()
        {
            this.ToTable("omni_Order_D");
            this.HasKey(s => s.Id);
            this.Property(s => s.Order_Id).IsRequired();
            this.Property(s => s.SEQ).IsRequired();
            this.Property(s => s.ItemCode).IsRequired().HasMaxLength(10);
            this.Property(s => s.ItemSet_Id);
            this.Property(s => s.ItemSet_Seq);
            this.Property(s => s.Qty).IsRequired();
            this.Property(s => s.UnitPrice);
            this.Property(s => s.is_Half);
            this.Property(s => s.Remarks);
            this.Property(s => s.reg_date).IsRequired();
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
