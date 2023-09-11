using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class MarketingFeeMap : EntityTypeConfiguration<MarketingFee>
    {
        public MarketingFeeMap()
        {
            this.ToTable("omni_MarketingFee");
            this.HasKey(s => s.Id);
            this.Property(s => s.Branch).IsRequired().HasMaxLength(2);
            this.Property(s => s.IssueDate).IsRequired();
            this.Property(s => s.Item).HasMaxLength(255).IsRequired();
            this.Property(s => s.Remarks).HasMaxLength(255);
            this.Property(s => s.YN_Paid).HasMaxLength(1).IsRequired();
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
