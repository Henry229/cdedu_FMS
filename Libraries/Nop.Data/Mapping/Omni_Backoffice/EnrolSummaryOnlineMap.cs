using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class EnrolSummaryOnlineMap : EntityTypeConfiguration<EnrolSummaryOnline>
    {
        public EnrolSummaryOnlineMap()
        {
            this.ToTable("vw_EnrolSummaryOnline");
        }
    }
}
