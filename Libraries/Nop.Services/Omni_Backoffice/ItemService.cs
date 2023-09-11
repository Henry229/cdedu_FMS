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
    public partial class ItemService : IItemService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        private const string ITEMS_BY_ID_KEY = "Nop.item.id-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string ITEMS_ALL_KEY = "Nop.item.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string ITEMS_PATTERN_KEY = "Nop.item.";

        #endregion

        #region Fields

        private readonly IRepository<Item> _itemRepository;
        private readonly IRepository<ItemCategory> _itemCategoryRepository;
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
        public ItemService(ICacheManager cacheManager, IEventPublisher eventPublisher,
            IRepository<Item> itemRepository, IRepository<ItemCategory> itemCategoryRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._itemRepository = itemRepository;
            this._itemCategoryRepository = itemCategoryRepository;
        }

        #endregion


        #region Methods

        /// <summary>
        /// Adds a item
        /// </summary>
        /// <param name="item">Item</param>
        /// <param name="clearCache">A value indicating whether to clear cache after item update</param>
        public virtual void InsertItem(Item item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            _itemRepository.Insert(item);

            //cache

            _cacheManager.RemoveByPattern(ITEMS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(item);
        }


        /// <summary>
        /// Updates a item
        /// </summary>
        /// <param name="item">item</param>
        /// <param name="clearCache">A value indicating whether to clear cache after item update</param>
        public virtual void UpdateItem(Item item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            _itemRepository.Update(item);

            //cache

            _cacheManager.RemoveByPattern(ITEMS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(item);
        }

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="item">item</param>
        public virtual void DeleteItem(Item item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            _itemRepository.Delete(item);

            //cache
            _cacheManager.RemoveByPattern(ITEMS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(item);
        }


        /// <summary>
        /// Gets a item by identifier
        /// </summary>
        /// <returns>Item</returns>
        /// 
        public virtual Item GetItemById(int itemId)
        {
            if (itemId == 0)
                return null;

            return _itemRepository.GetById(itemId);
        }

        /// <summary>
        /// Gets a item by identifier
        /// </summary>
        /// <returns>Item</returns>
        /// 
        public virtual Item GetItemByCode(string itemcode)
        {
            if (itemcode == "")
                return null;

            var item = _itemRepository.Table.Where(s => s.ItemCode == itemcode);
            var itemlist = item.ToList();
            if (itemlist.Count != 1)
                return null;

            return itemlist[0];
        }


        public bool CheckDup(Item item)
        {
            var same = _itemRepository.Table.Where(s => s.ItemCode == item.ItemCode);
            if (same.ToList().Count > 0)
                return true;
            return false;
        }

        public string GenerateItemCode(string category)
        {
            if (category == null || category == "" || category.Length != 4)
                return "";

            var list = _itemRepository.Table.Where(s => s.ItemCode.StartsWith(category)).OrderByDescending(s => s.ItemCode).ToList();

            int seq = 1;
            if ( list.Count != 0 )
            {
                seq = Int32.Parse(list[0].ItemCode.Substring(4, 6));
                seq = seq + 1;
            }

            return category + seq.ToString("000000");
        }


        /// <summary>
        /// Gets all items
        /// </summary>
        /// <returns>item collection</returns>
        public virtual IPagedList<Item> GetAllItems(string itemCategory, string itemcode, string itemname, string term, string grade, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _itemRepository.Table;

            if ( !String.IsNullOrWhiteSpace( itemCategory ) )
            {
                query = query.Where(c => c.ItemCategory.Contains(itemCategory));
            }
            if (!String.IsNullOrWhiteSpace(itemcode))
            {
                query = query.Where(c => c.ItemCode.Contains(itemcode));
            }
            if (!String.IsNullOrWhiteSpace(itemname))
            {
                query = query.Where(c => c.ItemName.Contains(itemname));
            }
            if (!String.IsNullOrWhiteSpace(term))
            {
                query = query.Where(c => c.Term.Contains(term));
            }
            if (!String.IsNullOrWhiteSpace(grade))
            {
                query = query.Where(c => c.Grade.Contains(grade));
            }


            query = query.OrderBy(c => c.ItemCode);

            try
            {
                var pageditems = new PagedList<Item>(query, pageIndex, pageSize);
                return pageditems;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }


        /// <summary>
        /// Gets all items
        /// </summary>
        /// <returns>item collection</returns>
        public virtual IPagedList<Item> GetTermItems(string term, string grade, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _itemRepository.Table;
            query = query.Where(c => c.Term == term);
            query = query.Where(c => c.Grade == grade);
            query = query.OrderBy(c => c.ItemCode);

            try
            {
                var pageditems = new PagedList<Item>(query, pageIndex, pageSize);
                return pageditems;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }



        
        

        #endregion


    }
}
