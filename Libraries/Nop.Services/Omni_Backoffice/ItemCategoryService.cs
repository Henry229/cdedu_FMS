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
    public partial class ItemCategoryService : IItemCategoryService
    {
        #region Constants
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        private const string ITEMCATEGORYS_BY_ID_KEY = "Nop.itemcategory.id-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        private const string ITEMCATEGORYS_ALL_KEY = "Nop.itemcategory.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string ITEMCATEGORYS_PATTERN_KEY = "Nop.itemcategory.";

        #endregion

        #region Fields

        private readonly IRepository<ItemCategory> _itemcategoryRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="itemcategoryRepository">Setting repository</param>
        public ItemCategoryService(ICacheManager cacheManager, IEventPublisher eventPublisher,
            IRepository<ItemCategory> itemcategoryRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._itemcategoryRepository = itemcategoryRepository;
        }

        #endregion



        #region Methods
        /// <summary>
        /// Deletes a setting
        /// </summary>
        /// <param name="itemcategory">Setting</param>
        public virtual void DeleteItemCategory(ItemCategory itemcategory)
        {
            if (itemcategory == null)
                throw new ArgumentNullException("itemcategory");

            _itemcategoryRepository.Delete(itemcategory);

            //cache
            _cacheManager.RemoveByPattern(ITEMCATEGORYS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(itemcategory);
        }


        /// <summary>
        /// Adds a setting
        /// </summary>
        /// <param name="itemcategory">itemcategory</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        public virtual void InsertItemCategory(ItemCategory itemCategory)
        {
            if (itemCategory == null)
                throw new ArgumentNullException("itemcategory");

            _itemcategoryRepository.Insert(itemCategory);

            //cache

            _cacheManager.RemoveByPattern(ITEMCATEGORYS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(itemCategory);
        }

        /// <summary>
        /// Updates a setting
        /// </summary>
        /// <param name="itemcategory">Setting</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        public virtual void UpdateItemCategory(ItemCategory itemcategory)
        {
            if (itemcategory == null)
                throw new ArgumentNullException("itemcategory");

            _itemcategoryRepository.Update(itemcategory);

            //cache

            _cacheManager.RemoveByPattern(ITEMCATEGORYS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(itemcategory);
        }

        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns>Setting collection</returns>
        public virtual IPagedList<ItemCategory> GetAllItemCategorys(int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = from s in _itemcategoryRepository.Table
                        orderby s.CategoryCode
                        select s;
            try
            {
                var pageditems = new PagedList<ItemCategory>(query, pageIndex, pageSize);
                return pageditems;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }

        }

        /// <summary>
        /// Gets a setting by identifier
        /// </summary>
        /// <returns>itemcategory</returns>
        /// 
        public virtual ItemCategory GetItemCategoryById(int itemcategoryId)
        {
            if (itemcategoryId == 0)
                return null;

            return _itemcategoryRepository.GetById(itemcategoryId);
        }



        #endregion

    }
}
