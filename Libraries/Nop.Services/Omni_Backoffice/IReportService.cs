using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nop.Core.Domain.Omni_Backoffice;
using Nop.Core;


namespace Nop.Services.Omni_Backoffice
{
    public partial interface IReportService
    {
        
        /// <summary>
        /// Gets all enrollment
        /// </summary>
        /// <returns>enrollment</returns>
        IPagedList<EnrolSummaryOnline> GetAllEnrolSummary(string year, string branch, int pageIndex = 0, int pageSize = 2147483647);

        /// <summary>
        /// Gets all test
        /// </summary>
        /// <returns>enrollment</returns>
        IPagedList<TestSummaryOnline> GetAllTestSummary(int year, string branch, int pageIndex = 0, int pageSize = 2147483647);

    }
}
