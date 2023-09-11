using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;
namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class SiblingManageMap : EntityTypeConfiguration<SiblingManage>
    {
        public SiblingManageMap()
        {
            this.ToTable("omni_Sibling");
            this.HasKey(s => s.Id);
            this.Property(s => s.Parent_Id);
            this.Property(s => s.Seq);
            this.Property(s => s.Stud_Id).IsRequired().HasMaxLength(50);
            this.Property(s => s.Id_Number).IsRequired().HasMaxLength(50);
            this.Property(s => s.Remarks);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
