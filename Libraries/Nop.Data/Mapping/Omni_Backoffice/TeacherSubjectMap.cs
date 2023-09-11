using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;
namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class TeacherSubjectMap : EntityTypeConfiguration<TeacherSubject>
    {
        public TeacherSubjectMap()
        {
            this.ToTable("omni_TeacherSubject");
            this.HasKey(s => s.Id);
            this.Property(s => s.Teacher_Id).IsRequired();
            this.Property(s => s.Subject).IsRequired().HasMaxLength(4);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
