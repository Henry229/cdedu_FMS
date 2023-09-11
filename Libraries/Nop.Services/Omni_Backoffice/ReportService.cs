using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Omni_Backoffice;
using Nop.Services.Events;

namespace Nop.Services.Omni_Backoffice
{
    public partial class ReportService : IReportService
    {
       

        #region Fields

        private readonly IRepository<EnrolSummaryOnline> _enrolsummaryRepository;
        private readonly IRepository<TestSummaryOnline> _testsummaryRepository;

        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;

        #endregion





        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="itemRepository">item repository</param>
        public ReportService(ICacheManager cacheManager, IEventPublisher eventPublisher,
            IRepository<EnrolSummaryOnline> enrolsummaryRepository,
            IRepository<TestSummaryOnline> testsummaryRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._enrolsummaryRepository = enrolsummaryRepository;
            this._testsummaryRepository = testsummaryRepository;
        }


        public IPagedList<EnrolSummaryOnline> GetAllEnrolSummary(string year, string branch, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _enrolsummaryRepository.Table;
            if (branch != null && branch != "")
            {
                query = query.Where(x => x.branch == branch);
            }

            if (year != null && year != "")
            {
                query = query.Where(x => x.year == year);
            }

            query = query.OrderBy(x => x.year);


            try
            {
                var pageenrollments = new PagedList<EnrolSummaryOnline>(query, pageIndex, pageSize);
                return pageenrollments;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }

        public IPagedList<TestSummaryOnline> GetAllTestSummary(int year, string branch, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _testsummaryRepository.Table;
            if (branch != null && branch != "")
            {
                query = query.Where(x => x.branch == branch);
            }

            if (year != 0)
            {
                query = query.Where(x => x.myear == year);
            }

            query = query.OrderBy(x => x.myear);


            try
            {
                var pagetestsummarys = new PagedList<TestSummaryOnline>(query, pageIndex, pageSize);
                return pagetestsummarys;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }

    }
}
