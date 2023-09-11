using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class ClassTeacherMap : EntityTypeConfiguration<ClassTeacher>
    {
        public ClassTeacherMap()
        {
            this.ToTable("omni_Class_Teacher");
            this.HasKey(s => s.Id);
            this.Property(s => s.Class_Id);
            this.Property(s => s.Teacher_Id);
            this.Property(s => s.Duration);
            this.Property(s => s.Remarks);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
