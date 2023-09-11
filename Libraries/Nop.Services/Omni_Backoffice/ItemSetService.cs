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
    public partial class ItemSetService : IItemSetService    
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        private const string ITEMSETS_BY_ID_KEY = "Nop.itemset.id-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string ITEMSETS_ALL_KEY = "Nop.itemset.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string ITEMSETS_PATTERN_KEY = "Nop.itemset.";


        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        private const string ITEMSET_DS_BY_ID_KEY = "Nop.itemset_d.id-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string ITEMSET_DS_ALL_KEY = "Nop.itemset_d.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string ITEMSET_DS_PATTERN_KEY = "Nop.itemset_d.";

        #endregion

        #region Fields

        private readonly IRepository<Item> _itemRepository;
        private readonly IRepository<ItemCategory> _itemCategoryRepository;
        private readonly IRepository<ItemSet> _itemSetRepository;
        private readonly IRepository<ItemSet_D> _itemSet_DRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;

        #endregion


        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="itemRepository">item repository</param>
        public ItemSetService(ICacheManager cacheManager, IEventPublisher eventPublisher,
            IRepository<Item> itemRepository, IRepository<ItemCategory> itemCategoryRepository,
            IRepository<ItemSet> itemsetRepository, IRepository<ItemSet_D> itemset_dRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._itemRepository = itemRepository;
            this._itemCategoryRepository = itemCategoryRepository;
            this._itemSetRepository = itemsetRepository;
            this._itemSet_DRepository = itemset_dRepository;
        }

        #endregion

        public ItemSet GetItemSetById(int itemsetID)
        {
            if (itemsetID == 0)
                return null;

            return _itemSetRepository.GetById(itemsetID);
        }

        public void InsertItemSet(ItemSet itemset)
        {
            if (itemset == null)
                throw new ArgumentNullException("itemset");

            try
            {
                _itemSetRepository.Insert(itemset);

                //cache

                _cacheManager.RemoveByPattern(ITEMSETS_PATTERN_KEY);

                //event notification
                _eventPublisher.EntityInserted(itemset);
            }catch(Exception ex)
            {
                string x = ex.Message;
            }
        }

        public void DeleteItemSet(ItemSet itemset)
        {
            if (itemset == null)
                throw new ArgumentNullException("itemset");

            _itemSetRepository.Delete(itemset);

            //cache
            _cacheManager.RemoveByPattern(ITEMSETS_PATTERN_KEY);


            //event notification
            _eventPublisher.EntityDeleted(itemset);
        }

        public void UpdateItemSet(ItemSet itemset)
        {
            if (itemset == null)
                throw new ArgumentNullException("itemset");

            _itemSetRepository.Update(itemset);

            //cache

            _cacheManager.RemoveByPattern(ITEMSETS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(itemset);
        }

        public void InsertItemSet_D(ItemSet_D itemset_d)
        {
            if (itemset_d == null)
                throw new ArgumentNullException("itemset_d");

            _itemSet_DRepository.Insert(itemset_d);

            //cache
            _cacheManager.RemoveByPattern(ITEMSET_DS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(itemset_d);
        }

        public void DeleteItemSet_D(ItemSet_D itemset_d)
        {
            if (itemset_d == null)
                throw new ArgumentNullException("itemset_d");

            _itemSet_DRepository.Delete(itemset_d);

            //cache
            _cacheManager.RemoveByPattern(ITEMSET_DS_PATTERN_KEY);


            //event notification
            _eventPublisher.EntityDeleted(itemset_d);
        }

        public void UpdateItemSet_D(ItemSet_D itemset_d)
        {
            if (itemset_d == null)
                throw new ArgumentNullException("itemset_d");

            _itemSet_DRepository.Update(itemset_d);

            //cache

            _cacheManager.RemoveByPattern(ITEMSET_DS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(itemset_d);
        }

        public ItemSet_D GetItemSet_DById(int itemsetdID)
        {
            if (itemsetdID == 0)
                return null;

            return _itemSet_DRepository.GetById(itemsetdID);
        }

        public IPagedList<ItemSet> GetAllItemSets(string term, string grade, string setcategory, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _itemSetRepository.Table;

            if (term != null && term != "")
            {
                query = query.Where(x => x.Term == term);
            }

            if (grade != null && grade != "")
            {
                query = query.Where(x => x.Grade == grade);
            }

            if (setcategory != null && setcategory != "")
            {
                query = query.Where(x => x.SetCategory == setcategory);
            }



            query = query.OrderBy(c => c.Term);
            query = query.OrderBy(c => c.Grade);

            try
            {
                var pageditemsets = new PagedList<ItemSet>(query, pageIndex, pageSize);
                return pageditemsets;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }

        public IPagedList<ItemSet_D> GetAllItemSet_Ds(ItemSet itemset, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _itemSet_DRepository.Table;
            query = query.Where(c => c.Set_Id == itemset.Id);
            query = query.OrderBy(c => c.Seq);

            try
            {
                var pageditemset_ds = new PagedList<ItemSet_D>(query, pageIndex, pageSize);
                return pageditemset_ds;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }
    }
}
