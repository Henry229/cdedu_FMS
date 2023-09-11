using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;
namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class PrintingPaperMap : EntityTypeConfiguration<PrintingPaper>
    {
        public PrintingPaperMap()
        {
            this.ToTable("omni_PrintingPaper");
            this.HasKey(s => s.Id);
            this.Property(s => s.Year).IsRequired().HasMaxLength(4);
            this.Property(s => s.Term).IsRequired().HasMaxLength(4);
            this.Property(s => s.BranchCode).IsRequired().HasMaxLength(2);
            this.Property(s => s.Course_Id);
            this.Property(s => s.TestNo);
            this.Property(s => s.Qty);
            this.Property(s => s.Qty_Teacher);
            this.Property(s => s.Qty_NewBook);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
