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
    public partial class CodeMasterService : ICodeMasterService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        private const string CODEMASTERS_BY_ID_KEY = "Nop.codemaster.id-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        private const string CODEMASTERS_ALL_KEY = "Nop.codemaster.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CODEMASTERS_PATTERN_KEY = "Nop.codemaster.";





        #endregion

        #region Fields

        private readonly IRepository<CodeMaster> _codemasterRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;

        private readonly IRepository<Campus> _campusRepository;



        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="codeMasterRepository">codemaster repository</param>
        public CodeMasterService(ICacheManager cacheManager, IEventPublisher eventPublisher,
            IRepository<CodeMaster> codeMasterRepository, IRepository<Campus> campusRepository )
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._codemasterRepository = codeMasterRepository;
            this._campusRepository = campusRepository;

        }

        #endregion

      

        #region Methods
        /// <summary>
        /// Deletes a codemaster
        /// </summary>
        /// <param name="codemaster">codemaster</param>
        public virtual void DeleteCodeMaster(CodeMaster codemaster)
        {
            if (codemaster == null)
                throw new ArgumentNullException("codemaster");

            _codemasterRepository.Delete(codemaster);

            //cache
            _cacheManager.RemoveByPattern(CODEMASTERS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(codemaster);
        }

        /// <summary>
        /// Gets all codemasters
        /// </summary>
        /// <returns>codemaster collection</returns>
        public virtual IPagedList<CodeMaster> GetAllCodeMasters(string yn_use = "Y", int pageIndex = 0, int pageSize = 2147483647)
        {
            return GetAllCodeMasters("", yn_use, pageIndex, pageSize);

        }



        /// <summary>
        /// Gets all codemasters
        /// </summary>
        /// <returns>codemaster collection</returns>
        public virtual IPagedList<CodeMaster> GetAllCodeMasters(string typecode = "" , string yn_use = "Y", int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = from s in _codemasterRepository.Table
                        where s.YN_Use.Contains( yn_use )
                        where s.CodeType.Contains(typecode)
                        orderby s.CodeType, s.SortOrder
                        select s;
            try
            {
                var pageditems = new PagedList<CodeMaster>(query, pageIndex, pageSize);
                return pageditems;
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        /// <summary>
        /// Gets a codemaster by identifier
        /// </summary>
        /// <returns>CodeMaster</returns>
        /// 
        public virtual CodeMaster GetCodeMasterById(int codemasterId)
        {
            if (codemasterId == 0)
                return null;

            return _codemasterRepository.GetById(codemasterId);
        }


        /// <summary>
        /// Adds a codemaster
        /// </summary>
        /// <param name="codemaster">CodeMaster</param>
        /// <param name="clearCache">A value indicating whether to clear cache after codemaster update</param>
        public virtual void InsertCodeMaster(CodeMaster codemaster)
        {
            if (codemaster == null)
                throw new ArgumentNullException("codemaster");

            _codemasterRepository.Insert(codemaster);

            //cache
            _cacheManager.RemoveByPattern(CODEMASTERS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(codemaster);
        }


        /// <summary>
        /// Updates a codemaster
        /// </summary>
        /// <param name="codemaster">codemaster</param>
        /// <param name="clearCache">A value indicating whether to clear cache after codemaster update</param>
        public virtual void UpdateCodeMaster(CodeMaster codemaster)
        {
            if (codemaster == null)
                throw new ArgumentNullException("codemaster");

            _codemasterRepository.Update(codemaster);

            //cache
            _cacheManager.RemoveByPattern(CODEMASTERS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(codemaster);
        }




        #endregion


        #region Campus
        /// <summary>
        /// Gets all codemasters
        /// </summary>
        /// <returns>codemaster collection</returns>
        public virtual IPagedList<Campus> GetAllCampus(int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = from s in _campusRepository.Table
                        orderby s.Campus_Title
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
        #endregion


    }
}
