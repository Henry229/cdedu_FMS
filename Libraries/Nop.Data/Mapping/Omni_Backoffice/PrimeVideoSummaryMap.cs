﻿using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Omni_Backoffice;

namespace Nop.Data.Mapping.Omni_Backoffice
{
    public partial class PrimeVideoSummaryMap : EntityTypeConfiguration<PrimeVideoSummary>
    {
        public PrimeVideoSummaryMap()
        {
            this.ToTable("vw_PrimeVideoSummary");
        }
    }
}
