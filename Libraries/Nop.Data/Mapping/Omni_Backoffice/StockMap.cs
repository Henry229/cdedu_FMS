using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class StockMap : EntityTypeConfiguration<Stock>
    {
        public StockMap()
        {
            this.ToTable("omni_Stock");
            this.HasKey(s => s.Id);
            this.Property(s => s.ItemCode).IsRequired().HasMaxLength(10);
            this.Property(s => s.TranDate).IsRequired();
            this.Property(s => s.InQty).IsRequired();
            this.Property(s => s.OutQty).IsRequired();
            this.Property(s => s.Source_Type);
            this.Property(s => s.Source_Id);
            this.Property(s => s.Source_Seq);
            this.Property(s => s.Remarks).HasMaxLength(255);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
