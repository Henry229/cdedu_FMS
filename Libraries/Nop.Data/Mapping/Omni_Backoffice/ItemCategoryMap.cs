using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class ItemCategoryMap : EntityTypeConfiguration<ItemCategory>
    {
        public ItemCategoryMap()
        {
            this.ToTable("omni_ItemCategory");
            this.HasKey(s => s.Id);
            this.Property(s => s.CategoryCode).IsRequired().HasMaxLength(4);
            this.Property(s => s.CategoryName).IsRequired().HasMaxLength(50);
            this.Property(s => s.Remarks).HasMaxLength(255);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
