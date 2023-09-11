using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class ClassScheduleRollcallMap : EntityTypeConfiguration<ClassScheduleRollcall>
    {
        public ClassScheduleRollcallMap()
        {
            this.ToTable("omni_Class_Rollcall");
            this.HasKey(s => s.Id);
            this.Property(s => s.Class_Id);
            this.Property(s => s.Class_D_Id);
            this.Property(s => s.Stud_Id);
            this.Property(s => s.Attend).IsRequired().HasMaxLength(1);
            this.Property(s => s.Remarks);
            this.Property(s => s.Makeup_Id);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
