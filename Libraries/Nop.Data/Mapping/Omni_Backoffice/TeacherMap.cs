using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;
namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class TeacherMap : EntityTypeConfiguration<Teacher>
    {
        public TeacherMap()
        {
            this.ToTable("omni_Teacher");
            this.HasKey(s => s.Id);
            this.Property(s => s.Title).IsRequired().HasMaxLength(4);
            this.Property(s => s.FirstName).IsRequired().HasMaxLength(100);
            this.Property(s => s.LastName).IsRequired().HasMaxLength(100);
            this.Property(s => s.Gender).IsRequired().HasMaxLength(4);
            this.Property(s => s.TeacherRole).IsRequired().HasMaxLength(1);
            this.Property(s => s.TutorRole).IsRequired().HasMaxLength(1);
            this.Property(s => s.MarkerRole).IsRequired().HasMaxLength(1);
            this.Property(s => s.TeachingGrade).IsRequired().HasMaxLength(4);
            this.Property(s => s.Subject);
            this.Property(s => s.WorkingCond).IsRequired().HasMaxLength(4);
            this.Property(s => s.Address).HasMaxLength(100);
            this.Property(s => s.Mobile).IsRequired().HasMaxLength(20);
            this.Property(s => s.HomePhone).IsRequired().HasMaxLength(20);
            this.Property(s => s.eMail).IsRequired().HasMaxLength(50);
            this.Property(s => s.Branch).IsRequired().HasMaxLength(2);
            this.Property(s => s.Remarks);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
