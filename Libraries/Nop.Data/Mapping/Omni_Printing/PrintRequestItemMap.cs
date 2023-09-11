using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Printing;
namespace Nop.Data.Mapping.Omni_Printing
{
    public partial class PrintRequestItemMap : EntityTypeConfiguration<PrintRequestItem>
    {
        public PrintRequestItemMap()
        {
            this.ToTable("omnipr_RequestItem");
            this.HasKey(s => s.Id);
            this.Property(s => s.Req_Id).IsRequired();
            this.Property(s => s.Item_Type).IsRequired();
            this.Property(s => s.Remarks);
            this.Property(s => s.File_Id);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
