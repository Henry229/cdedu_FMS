using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Printing;
namespace Nop.Data.Mapping.Omni_Printing
{
    public partial class PrintRequestItemSpecMap : EntityTypeConfiguration<PrintRequestItemSpec>
    {
        public PrintRequestItemSpecMap()
        {
            this.ToTable("omnipr_RequestItemSpec");
            this.HasKey(s => s.Id);
            this.Property(s => s.Req_Id).IsRequired();
            this.Property(s => s.Item_Id).IsRequired();
            this.Property(s => s.Spec_Id).IsRequired();
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
