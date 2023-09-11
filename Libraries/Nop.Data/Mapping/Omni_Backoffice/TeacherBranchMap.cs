using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;
namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class TeacherBranchMap : EntityTypeConfiguration<TeacherBranch>
    {
        public TeacherBranchMap()
        {
            this.ToTable("omni_TeacherBranch");
            this.HasKey(s => s.Id);
            this.Property(s => s.Teacher_Id).IsRequired();
            this.Property(s => s.Branch).IsRequired().HasMaxLength(2);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
