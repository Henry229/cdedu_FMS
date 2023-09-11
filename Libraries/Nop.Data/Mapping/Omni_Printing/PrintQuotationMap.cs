using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Printing;
namespace Nop.Data.Mapping.Omni_Printing
{
    public partial class PrintQuotationMap : EntityTypeConfiguration<PrintQuotation>
    {
        public PrintQuotationMap()
        {
            this.ToTable("omnipr_Quotation");
            this.HasKey(s => s.Id);
            this.Property(s => s.Req_Id).IsRequired();
            this.Property(s => s.Due_date).IsRequired();
            this.Property(s => s.ContentText).IsRequired();
            this.Property(s => s.Quot_Amount).IsRequired();
            this.Property(s => s.File_Id);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
