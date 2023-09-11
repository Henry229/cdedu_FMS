using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class CodeMasterMap : EntityTypeConfiguration<CodeMaster>
    {
        public CodeMasterMap()
        {
            this.ToTable("omni_CodeMaster");
            this.HasKey(s => s.Id);
            this.Property(s => s.CodeType).IsRequired().HasMaxLength(4);
            this.Property(s => s.TypeName).IsRequired().HasMaxLength(50);
            this.Property(s => s.DetailCode).IsRequired().HasMaxLength(4);
            this.Property(s => s.DetailName).IsRequired().HasMaxLength(255);
            this.Property(s => s.String_01).HasMaxLength(50);
            this.Property(s => s.String_02).HasMaxLength(50);
            this.Property(s => s.String_03).HasMaxLength(50);
            this.Property(s => s.Val_01);
            this.Property(s => s.Val_02);
            this.Property(s => s.Val_03);
            this.Property(s => s.SortOrder).IsRequired();
            this.Property(s => s.YN_Use).IsRequired().HasMaxLength(1);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
