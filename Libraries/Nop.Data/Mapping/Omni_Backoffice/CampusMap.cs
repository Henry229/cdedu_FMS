using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class CampusMap : EntityTypeConfiguration<Campus>
    {
        public CampusMap()
        {
            this.ToTable("omni_Campus");
            this.Property(s => s.Campus_Prefix).IsRequired().HasMaxLength(2);
            this.Property(s => s.Campus_Title).IsRequired().HasMaxLength(50);
            this.Property(s => s.Campus_EN).IsRequired().HasMaxLength(2);
            this.Property(s => s.Suburb).HasMaxLength(50);
            this.Property(s => s.State).HasMaxLength(50);
            this.Property(s => s.Address1).HasMaxLength(50);
            this.Property(s => s.Address2).HasMaxLength(50);
            this.Property(s => s.PostCode).HasMaxLength(10);
            this.Property(s => s.PhoneNo).HasMaxLength(20);
            this.Property(s => s.Email).HasMaxLength(50);
            this.Property(s => s.RoyaltyType).IsRequired().HasMaxLength(4);
            this.Property(s => s.RoyaltyType_HI).IsRequired().HasMaxLength(4);
            this.Property(s => s.RoyaltyType_PA).IsRequired().HasMaxLength(4);
            this.Property(s => s.RoyaltyType_TT).IsRequired().HasMaxLength(4);
            this.Property(s => s.YN_Use).IsRequired().HasMaxLength(1);
            this.Property(s => s.LicenseeCode).HasMaxLength(50);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).IsRequired().HasMaxLength(20);
        }
    }
}
