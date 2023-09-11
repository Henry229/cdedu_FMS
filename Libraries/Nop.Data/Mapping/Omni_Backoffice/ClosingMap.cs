using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;
namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class ClosingMap : EntityTypeConfiguration<Closing>
    {
        public ClosingMap()
        {
            this.ToTable("omni_Closing");
            this.HasKey(s => s.Id);
            this.Property(s => s.Year).IsRequired().HasMaxLength(4);
            this.Property(s => s.Term).IsRequired().HasMaxLength(4);
            this.Property(s => s.Type).IsRequired().HasMaxLength(4);
            this.Property(s => s.Branch).IsRequired().HasMaxLength(2);
            this.Property(s => s.SEQ).IsRequired();
            this.Property(s => s.YN_Closing);
            this.Property(s => s.YN_Approval);
            this.Property(s => s.YN_Paid);
            this.Property(s => s.Amnt_adjust);
            this.Property(s => s.Amnt_Freight);
            this.Property(s => s.Remarks);
            this.Property(s => s.DueDate);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
