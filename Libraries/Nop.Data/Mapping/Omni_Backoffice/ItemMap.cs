using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class ItemMap : EntityTypeConfiguration<Item>
    {
        public ItemMap()
        {
            this.ToTable("omni_Item");
            this.HasKey(s => s.Id);
            this.Property(s => s.ItemCode).IsRequired().HasMaxLength(10);
            this.Property(s => s.ItemName).IsRequired().HasMaxLength(50);
            this.Property(s => s.ItemCategory).IsRequired().HasMaxLength(4);
            this.Property(s => s.DT_From);
            this.Property(s => s.DT_To);
            this.Property(s => s.UnitPrice);
            this.Property(s => s.UnitPrice_Half);
            this.Property(s => s.Grade).HasMaxLength(4);
            this.Property(s => s.Term).HasMaxLength(4);
            this.Property(s => s.Level).HasMaxLength(4);
            this.Property(s => s.Subject).HasMaxLength(4);
            this.Property(s => s.Remarks).HasMaxLength(255);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
