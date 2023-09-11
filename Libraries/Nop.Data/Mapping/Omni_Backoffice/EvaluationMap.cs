using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;
namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class EvaluationMap : EntityTypeConfiguration<Evaluation>
    {
        public EvaluationMap()
        {
            this.ToTable("omni_Evaluation");
            this.HasKey(s => s.Id);

            this.Property(s => s.EvaluationType).IsRequired().HasMaxLength(4);
            this.Property(s => s.Teacher_Id).IsRequired();
            this.Property(s => s.Class_Id).IsRequired();
            this.Property(s => s.Evaluator).IsRequired().HasMaxLength(20);
            this.Property(s => s.Score).IsRequired();
            this.Property(s => s.Remarks);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
        }
    }
}
