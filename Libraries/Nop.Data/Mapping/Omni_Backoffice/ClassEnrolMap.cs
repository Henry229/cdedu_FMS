using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class ClassEnrolMap : EntityTypeConfiguration<ClassEnrol>
    {
        public ClassEnrolMap()
        {
            this.ToTable("omni_Class_Enrol");
            this.HasKey(s => s.Id);
            this.Property(s => s.Year).HasMaxLength(4);
            this.Property(s => s.Term).HasMaxLength(4);
            this.Property(s => s.Class_Id);
            this.Property(s => s.Stud_Id);
            this.Property(s => s.Course_Id);
            this.Property(s => s.Unit_Price);
            this.Property(s => s.AttendWeek);
            this.Property(s => s.MaterialFee);
            this.Property(s => s.HalfBook);
            this.Property(s => s.Level).HasMaxLength(4);
            this.Property(s => s.Discount_Code).HasMaxLength(4);
            this.Property(s => s.Discount_Rate);
            this.Property(s => s.PayMethod).HasMaxLength(4);
            this.Property(s => s.TotalAmount);
            this.Property(s => s.Chk_HD);
            this.Property(s => s.Chk_FM);
            this.Property(s => s.Remarks);
            this.Property(s => s.PayStatus).HasMaxLength(4);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
