using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class PASalesMap : EntityTypeConfiguration<PASales>
    {
        public PASalesMap()
        {
            this.ToTable("omni_PASales");
            this.HasKey(s => s.Id);
            this.Property(s => s.Sales_id).IsRequired();
            this.Property(s => s.Branch).HasMaxLength(2);
            this.Property(s => s.Stud_Advisor).HasMaxLength(20);
            this.Property(s => s.Title).HasMaxLength(200);
            this.Property(s => s.Price);
            this.Property(s => s.List_Price);
            this.Property(s => s.Payment);
            this.Property(s => s.Payment_Flag);
            this.Property(s => s.Stud_id);
            this.Property(s => s.Purchase_Date);
            this.Property(s => s.IF_Flag).IsRequired();
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
