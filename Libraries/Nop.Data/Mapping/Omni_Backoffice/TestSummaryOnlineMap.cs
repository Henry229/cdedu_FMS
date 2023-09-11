using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class TestSummaryOnlineMap : EntityTypeConfiguration<TestSummaryOnline>
    {
        public TestSummaryOnlineMap()
        {
            this.ToTable("vw_TestSummaryOnline");
        }
    }
}
