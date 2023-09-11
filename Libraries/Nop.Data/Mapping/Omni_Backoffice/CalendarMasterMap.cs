using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class CalendarMasterMap : EntityTypeConfiguration<CalendarMaster>
    {
        public CalendarMasterMap()
        {
            this.ToTable("omni_Calendar");
            this.HasKey(s => s.Id);
            this.Property(s => s.Year).IsRequired().HasMaxLength(4);
            this.Property(s => s.Term).IsRequired().HasMaxLength(4);
            this.Property(s => s.Week).IsRequired();
            this.Property(s => s.StartDate);
            this.Property(s => s.EndDate);
            this.Property(s => s.YN_Active);
            this.Property(s => s.YN_PA);
            this.Property(s => s.YN_Enrol);
            this.Property(s => s.reg_date);
            this.Property(s => s.reg_source).HasMaxLength(20);
            this.Property(s => s.Remarks).HasMaxLength(255);
      }
    }
}
