using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class AdditionInfoMap : EntityTypeConfiguration<AdditionInfo>
    {
        public AdditionInfoMap()
        {
            this.ToTable("omni_Member");
            this.HasKey(s => s.Id);
            this.Property(s => s.Stud_Id);
            this.Property(s => s.Actual_Grade);
            this.Property(s => s.Remarks);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
