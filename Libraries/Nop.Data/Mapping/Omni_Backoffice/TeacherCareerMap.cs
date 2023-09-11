using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;
namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class TeacherCareerMap : EntityTypeConfiguration<TeacherCareer>
    {
        public TeacherCareerMap()
        {
            this.ToTable("omni_TeacherCareer");
            this.HasKey(s => s.Id);
            this.Property(s => s.Teacher_Id).IsRequired();
            this.Property(s => s.CareerType).IsRequired().HasMaxLength(4);
            this.Property(s => s.FromDate).IsRequired();
            this.Property(s => s.ToDate).IsRequired();
            this.Property(s => s.Class_Id);
            this.Property(s => s.Remarks);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
