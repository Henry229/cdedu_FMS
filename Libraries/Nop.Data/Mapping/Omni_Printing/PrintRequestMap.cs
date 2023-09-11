using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Printing;
namespace Nop.Data.Mapping.Omni_Printing
{
    public partial class PrintRequestMap : EntityTypeConfiguration<PrintRequest>
    {
        public PrintRequestMap()
        {
            this.ToTable("omnipr_Request");
            this.HasKey(s => s.Id);
            this.Property(s => s.Title).IsRequired().HasMaxLength(255);
            this.Property(s => s.ContentText).IsRequired();
            this.Property(s => s.ReqDate).IsRequired();
            this.Property(s => s.DueDate).IsRequired();
            this.Property(s => s.File_Id);
            this.Property(s => s.User_Id).IsRequired();
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
