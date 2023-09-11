using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class MemberMap : EntityTypeConfiguration<Member>
    {
        public MemberMap()
        {
            this.ToTable("vw_member");
        }
    }
}
