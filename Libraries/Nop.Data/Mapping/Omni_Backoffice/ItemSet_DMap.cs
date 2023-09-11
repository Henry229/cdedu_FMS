using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class ItemSet_DMap : EntityTypeConfiguration<ItemSet_D>
    {
        public ItemSet_DMap()
        {
            this.ToTable("omni_ItemSet_D");
            this.HasKey(s => s.Id);
            this.Property(s => s.Set_Id).IsRequired();
            this.Property(s => s.Seq).IsRequired();
            this.Property(s => s.ItemCode).IsRequired().HasMaxLength(10);
            this.Property(s => s.Remarks).HasMaxLength(255);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }

    }
}
