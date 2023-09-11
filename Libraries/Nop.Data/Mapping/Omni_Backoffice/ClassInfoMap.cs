using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class ClassInfoMap : EntityTypeConfiguration<ClassInfo>
    {
        public ClassInfoMap()
        {
            this.ToTable("omni_Class");
            this.HasKey(s => s.Id);
            this.Property(s => s.Year).IsRequired().HasMaxLength(4);
            this.Property(s => s.Term).IsRequired().HasMaxLength(4);
            this.Property(s => s.Branch).IsRequired().HasMaxLength(2);
            this.Property(s => s.Grade).IsRequired().HasMaxLength(4);
            this.Property(s => s.Name);
            this.Property(s => s.DayofWeek).IsRequired().HasMaxLength(4); ;
            this.Property(s => s.StartTime);
            this.Property(s => s.EndTime);
            this.Property(s => s.Duration);
            this.Property(s => s.Remarks);
            this.Property(s => s.Classroom_Id);
            this.Property(s => s.Course_Id);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
