using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class ItemSetMap : EntityTypeConfiguration<ItemSet>
    {
        public ItemSetMap()
        {
            this.ToTable("omni_ItemSet");
            this.HasKey(s => s.Id);
            this.Property(s => s.SetName).IsRequired().HasMaxLength(50);
            this.Property(s => s.SetCategory).IsRequired().HasMaxLength(4);
            this.Property(s => s.Grade).HasMaxLength(4);
            this.Property(s => s.Term).HasMaxLength(4);
            this.Property(s => s.Level).HasMaxLength(4);
            this.Property(s => s.Course);
            this.Property(s => s.Remarks).HasMaxLength(255);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
