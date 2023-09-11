using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class CourseMasterMap : EntityTypeConfiguration<CourseMaster>
    {
        public CourseMasterMap()
        {
            this.ToTable("omni_Course");
            this.HasKey(s => s.Id);
            this.Property(s => s.CourseName).IsRequired().HasMaxLength(100);
            this.Property(s => s.CourseCategory).IsRequired().HasMaxLength(4);
            this.Property(s => s.CourseID_P).IsRequired();
            this.Property(s => s.Year).IsRequired().HasMaxLength(4);
            this.Property(s => s.Term).IsRequired().HasMaxLength(4);
            this.Property(s => s.Grade).HasMaxLength(4);
            this.Property(s => s.Level).HasMaxLength(4);
            this.Property(s => s.StartWeek);
            this.Property(s => s.TotalWeek);
            this.Property(s => s.CourseFee);
            this.Property(s => s.Remarks).HasMaxLength(255);
            this.Property(s => s.YN_Use).IsRequired().HasMaxLength(1);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).IsRequired().HasMaxLength(20);
            this.Property(s => s.BookFee);
            this.Property(s => s.Facility);
            this.Property(s => s.NewBookFee);
        }
    }
}
