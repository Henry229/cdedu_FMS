using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class MarketingFeePaymentMap : EntityTypeConfiguration<MarketingFeePayment>
    {
        public MarketingFeePaymentMap()
        {
            this.ToTable("omni_MarketingFeePayment");
            this.HasKey(s => s.Id);
            this.Property(s => s.Fee_Id).IsRequired();
            this.Property(s => s.PayDate).IsRequired();
            this.Property(s => s.Remarks).HasMaxLength(255);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
