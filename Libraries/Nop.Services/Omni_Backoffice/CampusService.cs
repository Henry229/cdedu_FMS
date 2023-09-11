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
    public partial class CampusService : ICampusService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        private const string CAMPUS_BY_ID_KEY = "Nop.campus.id-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string CAMPUS_ALL_KEY = "Nop.campus.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CAMPUS_PATTERN_KEY = "Nop.campus.";

        #endregion

        #region Fields

        private readonly IRepository<Campus> _campusRepository;
        //private readonly IRepository<CampusCategory> _campusCategoryRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="campusRepository">campus repository</param>
        public CampusService(ICacheManager cacheManager, IEventPublisher eventPublisher,
            //IRepository<Campus> campusRepository, IRepository<CampusCategory> campusCategoryRepository)
            IRepository<Campus> campusRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._campusRepository = campusRepository;
            //this._campusCategoryRepository = campusCategoryRepository;
        }

        #endregion


        #region Methods

        /// <summary>
        /// Adds a campus
        /// </summary>
        /// <param name="campus">Campus</param>
        /// <param name="clearCache">A value indicating whether to clear cache after campus update</param>
        public virtual void InsertCampus(Campus campus)
        {
            if (campus == null)
                throw new ArgumentNullException("campus");

            try
            {
                _campusRepository.Insert(campus);

                //cache

                _cacheManager.RemoveByPattern(CAMPUS_PATTERN_KEY);

                //event notification
                _eventPublisher.EntityInserted(campus);
            }catch(Exception ex)
            {
                string asbd = ex.Message;
            }
        }


        /// <summary>
        /// Updates a campus
        /// </summary>
        /// <param name="campus">campus</param>
        /// <param name="clearCache">A value indicating whether to clear cache after campus update</param>
        public virtual void UpdateCampus(Campus campus)
        {
            if (campus == null)
                throw new ArgumentNullException("campus");

            _campusRepository.Update(campus);

            //cache

            _cacheManager.RemoveByPattern(CAMPUS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(campus);
        }

        /// <summary>
        /// Deletes a campus
        /// </summary>
        /// <param name="campus">campus</param>
        public virtual void DeleteCampus(Campus campus)
        {
            if (campus == null)
                throw new ArgumentNullException("campus");

            _campusRepository.Delete(campus);

            //cache
            _cacheManager.RemoveByPattern(CAMPUS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(campus);
        }


        /// <summary>
        /// Gets a campus by identifier
        /// </summary>
        /// <returns>Campus</returns>
        /// 
        public virtual Campus GetCampusById(int campusId)
        {
            if (campusId == 0)
                return null;

            return _campusRepository.GetById(campusId);
        }
        /*
        /// <summary>
        /// Gets a campus by identifier
        /// </summary>
        /// <returns>Campus</returns>
        /// 
        public virtual Campus GetCampusByCode(string campuscode)
        {
            if (campuscode == "")
                return null;

            var campus = _campusRepository.Table.Where(s => s.CampusCode == campuscode);
            var campuslist = campus.ToList();
            if (campuslist.Count != 1)
                return null;

            return campuslist[0];
        }


        public bool CheckDup(Campus campus)
        {
            var same = _campusRepository.Table.Where(s => s.CampusCode == campus.CampusCode);
            if (same.ToList().Count > 0)
                return true;
            return false;
        }

        public string GenerateCampusCode(string category)
        {
            if (category == null || category == "" || category.Length != 4)
                return "";

            var list = _campusRepository.Table.Where(s => s.CampusCode.StartsWith(category)).OrderByDescending(s => s.CampusCode).ToList();

            int seq = 1;
            if (list.Count != 0)
            {
                seq = Int32.Parse(list[0].CampusCode.Substring(4, 6));
                seq = seq + 1;
            }

            return category + seq.ToString("000000");
        }
        */

        /// <summary>
        /// Gets all campuss
        /// </summary>
        /// <returns>campus collection</returns>
        public virtual IPagedList<Campus> GetAllCampus(string title, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _campusRepository.Table;

            if (!String.IsNullOrWhiteSpace(title))
            {
                query = query.Where(c => c.Campus_Title.Contains(title));
            }

            query = query.OrderBy(c => c.Suburb);

            try
            {
                var pagedcampus = new PagedList<Campus>(query, pageIndex, pageSize);
                return pagedcampus;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// Gets all campuss
        /// </summary>
        /// <returns>campus collection</returns>
        public virtual IPagedList<Campus> GetAllCampus(int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = from s in _campusRepository.Table
                        orderby s.Id
                        select s;
            try
            {
                var pageditems = new PagedList<Campus>(query, pageIndex, pageSize);
                return pageditems;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /*
        /// <summary>
        /// Gets all campuss
        /// </summary>
        /// <returns>campus collection</returns>
        public virtual IPagedList<Campus> GetTermCampuss(string term, string grade, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _campusRepository.Table;
            query = query.Where(c => c.Term == term);
            query = query.Where(c => c.Grade == grade);
            query = query.OrderBy(c => c.CampusCode);

            try
            {
                var pagedcampuss = new PagedList<Campus>(query, pageIndex, pageSize);
                return pagedcampuss;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }
        */


        #endregion


    }
}
