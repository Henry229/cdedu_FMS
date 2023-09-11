using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class Order_OmniMap : EntityTypeConfiguration<Order_Omni>
    {
        public Order_OmniMap()
        {
            this.ToTable("omni_Order");
            this.HasKey(s => s.Id);
            this.Property(s => s.BranchCode).IsRequired().HasMaxLength(2);
            this.Property(s => s.OrderDate).IsRequired();
            this.Property(s => s.OrderType).IsRequired();
            this.Property(s => s.Status).IsRequired();
            this.Property(s => s.Year).IsRequired();
            this.Property(s => s.Term).IsRequired();
            this.Property(s => s.SumPrice);
            this.Property(s => s.Week);
            this.Property(s => s.Payment);
            this.Property(s => s.confirm_date);
            this.Property(s => s.print_date);
            this.Property(s => s.deliver_date);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
