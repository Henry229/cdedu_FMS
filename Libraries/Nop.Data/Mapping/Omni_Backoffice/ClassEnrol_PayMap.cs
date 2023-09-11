using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class ClassEnrol_PayMap : EntityTypeConfiguration<ClassEnrol_Pay>
    {
        public ClassEnrol_PayMap()
        {
            this.ToTable("omni_Class_Enrol_Pay");
            this.HasKey(s => s.Id);
            this.Property(s => s.Id);
            this.Property(s => s.Seq);
            this.Property(s => s.PayDate);
            this.Property(s => s.PayAmount);
            this.Property(s => s.Remarks);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
