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
    public partial class CalendarMasterService : ICalendarMasterService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        private const string CODEMASTERS_BY_ID_KEY = "Nop.calendarmaster.id-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        private const string CODEMASTERS_ALL_KEY = "Nop.calendarmaster.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CODEMASTERS_PATTERN_KEY = "Nop.calendarmaster.";

        #endregion

        #region Fields

        private readonly IRepository<CalendarMaster> _calendarmasterRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<CodeMaster> _codemasterRepository;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="codeMasterRepository">calendarmaster repository</param>
        public CalendarMasterService(ICacheManager cacheManager, IEventPublisher eventPublisher,
            IRepository<CalendarMaster> calendarMasterRepository, IRepository<CodeMaster> codeMasterRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._calendarmasterRepository = calendarMasterRepository;
            this._codemasterRepository = codeMasterRepository;
        }

        #endregion



        #region Methods
        /// <summary>
        /// Deletes a calendarmaster
        /// </summary>
        /// <param name="calendarmaster">calendarmaster</param>
        public virtual void DeleteCalendarMaster(CalendarMaster calendarmaster)
        {
            if (calendarmaster == null)
                throw new ArgumentNullException("calendarmaster");

            _calendarmasterRepository.Delete(calendarmaster);

            //cache
            _cacheManager.RemoveByPattern(CODEMASTERS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(calendarmaster);
        }




        /// <summary>
        /// Gets all calendarmasters
        /// </summary>
        /// <returns>calendarmaster collection</returns>
        public virtual IPagedList<CalendarMaster> GetAllCalendarMasters(int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = from s in _calendarmasterRepository.Table
                        orderby s.Id
                        select s;
            try
            {
                var pageditems = new PagedList<CalendarMaster>(query, pageIndex, pageSize);
                return pageditems;
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        /// <summary>
        /// Gets a calendarmaster by identifier
        /// </summary>
        /// <returns>CalendarMaster</returns>
        /// 
        public virtual CalendarMaster GetCalendarMasterById(int calendarmasterId)
        {
            if (calendarmasterId == 0)
                return null;

            return _calendarmasterRepository.GetById(calendarmasterId);
        }


        /// <summary>
        /// Adds a calendarmaster
        /// </summary>
        /// <param name="calendarmaster">CalendarMaster</param>
        /// <param name="clearCache">A value indicating whether to clear cache after calendarmaster update</param>
        public virtual void InsertCalendarMaster(CalendarMaster calendarmaster)
        {
            if (calendarmaster == null)
                throw new ArgumentNullException("calendarmaster");

            _calendarmasterRepository.Insert(calendarmaster);

            //cache
            _cacheManager.RemoveByPattern(CODEMASTERS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(calendarmaster);
        }


        /// <summary>
        /// Updates a calendarmaster
        /// </summary>
        /// <param name="calendarmaster">calendarmaster</param>
        /// <param name="clearCache">A value indicating whether to clear cache after calendarmaster update</param>
        public virtual void UpdateCalendarMaster(CalendarMaster calendarmaster)
        {
            if (calendarmaster == null)
                throw new ArgumentNullException("calendarmaster");

            _calendarmasterRepository.Update(calendarmaster);

            //cache
            _cacheManager.RemoveByPattern(CODEMASTERS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(calendarmaster);
        }

        public virtual string GetCurrentWeek()
        {

            var query =  _calendarmasterRepository.Table;
            var today = new DateTime( DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var nextday = today.AddDays(1);

            query = query.Where(c => c.StartDate < nextday
                                     && c.EndDate >= today);

            query = query.OrderBy(c => c.Id);


            var pageditems = new PagedList<CalendarMaster>(query, 0, 2147483647).ToList();

            if (pageditems.Count > 0)
            {
                var curterm = pageditems[0];
                var datediff = DateTime.Now - curterm.StartDate;
                int weekdiff = (datediff.Days / 7) + 1;
                int weekdiff2 = weekdiff;

                if ( DateTime.Now.DayOfWeek != DayOfWeek.Saturday )
                {
                    weekdiff2 = weekdiff + 1;
                }

                var subquery = _codemasterRepository.Table.Where(x => x.CodeType == "CM02" && x.DetailCode == curterm.Term).ToList();

                string termstring = "";
                if ( subquery.Count > 0 )
                {
                    termstring = subquery[0].DetailName;
                }


                return " ( " + curterm.Year + "/" + termstring +  ( curterm.Term.Substring(0,1) == "V" ? " )" :  "-  Week " + weekdiff2.ToString() + " )" );
            }

            return "";

        }




        #endregion

    }
}
