using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;
namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class ClassRoomMap : EntityTypeConfiguration<ClassRoom>
    {
        public ClassRoomMap()
        {
            this.ToTable("omni_ClassRoom");
            this.HasKey(s => s.Id);
            this.Property(s => s.Branch).IsRequired().HasMaxLength(2);
            this.Property(s => s.Title).IsRequired().HasMaxLength(10);
            this.Property(s => s.Remarks);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
