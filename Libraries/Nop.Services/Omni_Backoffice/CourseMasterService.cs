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
    public partial class CourseMasterService : ICourseMasterService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        private const string CODEMASTERS_BY_ID_KEY = "Nop.coursemaster.id-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        private const string CODEMASTERS_ALL_KEY = "Nop.coursemaster.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CODEMASTERS_PATTERN_KEY = "Nop.coursemaster.";

        #endregion

        #region Fields

        private readonly IRepository<CourseMaster> _coursemasterRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="codeMasterRepository">coursemaster repository</param>
        public CourseMasterService(ICacheManager cacheManager, IEventPublisher eventPublisher,
            IRepository<CourseMaster> codeMasterRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._coursemasterRepository = codeMasterRepository;
        }

        #endregion



        #region Methods
        /// <summary>
        /// Deletes a coursemaster
        /// </summary>
        /// <param name="coursemaster">coursemaster</param>
        public virtual void DeleteCourseMaster(CourseMaster coursemaster)
        {
            if (coursemaster == null)
                throw new ArgumentNullException("coursemaster");

            _coursemasterRepository.Delete(coursemaster);

            //cache
            _cacheManager.RemoveByPattern(CODEMASTERS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(coursemaster);
        }


        public virtual IPagedList<CourseMaster> GetAllCourseMasters(int pageIndex = 0, int pageSize = 2147483647)
        {
            return GetAllCourseMasters("", "", "", "", pageIndex, pageSize);
        }

        /// <summary>
        /// Gets all coursemasters
        /// </summary>
        /// <returns>coursemaster collection</returns>
        public virtual IPagedList<CourseMaster> GetAllCourseMasters(string year, string term, string coursecategory, string grade, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _coursemasterRepository.Table;

            if (year != null && year != "")
            {
                query = query.Where(x => x.Year == year);
            }

            if (term != null && term != "")
            {
                query = query.Where(x => x.Term == term);
            }

            if (grade != null && grade != "")
            {
                query = query.Where(x => x.Grade == grade);
            }

            if (coursecategory != null && coursecategory != "")
            {
                query = query.Where(x => x.CourseCategory == coursecategory);
            }


            query = query.OrderBy(x => x.Grade);


            try
            {
                var pageditems = new PagedList<CourseMaster>(query, pageIndex, pageSize);
                return pageditems;
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        /// <summary>
        /// Gets a coursemaster by identifier
        /// </summary>
        /// <returns>CourseMaster</returns>
        /// 
        public virtual CourseMaster GetCourseMasterById(int coursemasterId)
        {
            if (coursemasterId == 0)
                return null;

            return _coursemasterRepository.GetById(coursemasterId);
        }


        /// <summary>
        /// Adds a coursemaster
        /// </summary>
        /// <param name="coursemaster">CourseMaster</param>
        /// <param name="clearCache">A value indicating whether to clear cache after coursemaster update</param>
        public virtual void InsertCourseMaster(CourseMaster coursemaster)
        {
            if (coursemaster == null)
                throw new ArgumentNullException("coursemaster");

            _coursemasterRepository.Insert(coursemaster);

            //cache
            _cacheManager.RemoveByPattern(CODEMASTERS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(coursemaster);
        }


        /// <summary>
        /// Updates a coursemaster
        /// </summary>
        /// <param name="coursemaster">coursemaster</param>
        /// <param name="clearCache">A value indicating whether to clear cache after coursemaster update</param>
        public virtual void UpdateCourseMaster(CourseMaster coursemaster)
        {
            if (coursemaster == null)
                throw new ArgumentNullException("coursemaster");

            _coursemasterRepository.Update(coursemaster);

            //cache
            _cacheManager.RemoveByPattern(CODEMASTERS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(coursemaster);
        }




        #endregion

    }
}
