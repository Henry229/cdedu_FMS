using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class ClassScheduleMap : EntityTypeConfiguration<ClassSchedule>
    {
        public ClassScheduleMap()
        {
            this.ToTable("omni_Class_D");
            this.HasKey(s => s.Id);
            this.Property(s => s.Class_Id);
            this.Property(s => s.Class_Date);
            this.Property(s => s.Class_StartTime);
            this.Property(s => s.Class_EndTime);
            this.Property(s => s.Classroom_Id);
            this.Property(s => s.YN_Close);
            this.Property(s => s.Remarks);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
