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
    public partial class OrderService_Omni : IOrderService_Omni
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        private const string ORDERS_BY_ID_KEY = "Nop.order.id-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string ORDERS_ALL_KEY = "Nop.order.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string ORDERS_PATTERN_KEY = "Nop.order.";


        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        private const string ORDER_DS_BY_ID_KEY = "Nop.order_d.id-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string ORDER_DS_ALL_KEY = "Nop.order_d.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string ORDER_DS_PATTERN_KEY = "Nop.order_d.";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        private const string ORDERBYITEMSETS_BY_ID_KEY = "Nop.orderbyitemset.id-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string ORDERBYITEMSETS_ALL_KEY = "Nop.orderbyitemset.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string ORDERBYITEMSETS_PATTERN_KEY = "Nop.orderbyitemset.";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        private const string STOCKS_BY_ID_KEY = "Nop.stock.id-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string STOCKS_ALL_KEY = "Nop.stock.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string STOCKS_PATTERN_KEY = "Nop.stock.";




        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        private const string PRINTINGPAPER_BY_ID_KEY = "Nop.printingpaper.id-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string PRINTINGPAPERS_ALL_KEY = "Nop.printingpaper.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string PRINTINGPAPERS_PATTERN_KEY = "Nop.printingpaper.";

        #endregion

        #region Fields

        private readonly IRepository<Item> _itemRepository;
        private readonly IRepository<ItemSet> _itemSetRepository;
        private readonly IRepository<ItemSet_D> _itemSet_DRepository;
        private readonly IRepository<Order_Omni> _orderRepository;
        private readonly IRepository<OrderbyItemSet> _orderbyitemSetRepository;
        private readonly IRepository<Order_D> _order_DRepository;
        private readonly IRepository<Stock> _stockRepository;
        private readonly IRepository<PrintingPaper> _printingpaperRepository;
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
        public OrderService_Omni(ICacheManager cacheManager, IEventPublisher eventPublisher,
            IRepository<Item> itemRepository, IRepository<ItemCategory> itemCategoryRepository,
            IRepository<ItemSet> itemsetRepository, IRepository<ItemSet_D> itemset_dRepository,
            IRepository<Order_Omni> orderRepository, IRepository<Order_D> order_dRepository
            , IRepository<OrderbyItemSet> orderbyitemsetRepository, IRepository<Stock> stock_Repository, IRepository<PrintingPaper> printingpaperRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._itemRepository = itemRepository;
            this._itemSetRepository = itemsetRepository;
            this._itemSet_DRepository = itemset_dRepository;
            this._orderRepository = orderRepository;
            this._order_DRepository = order_dRepository;
            this._orderbyitemSetRepository = orderbyitemsetRepository;
            this._stockRepository = stock_Repository;
            this._printingpaperRepository = printingpaperRepository;
        }

        #endregion

        public Order_Omni GetOrderById(int orderID)
        {
            if (orderID == 0)
                return null;

            return _orderRepository.GetById(orderID);
        }

        public void InsertOrder(Order_Omni order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            try
            {
                _orderRepository.Insert(order);

                //cache

                _cacheManager.RemoveByPattern(ORDERS_PATTERN_KEY);

                //event notification
                _eventPublisher.EntityInserted(order);
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
        }

        public void DeleteOrder(Order_Omni order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            _orderRepository.Delete(order);

            //cache
            _cacheManager.RemoveByPattern(ORDERS_PATTERN_KEY);


            //event notification
            _eventPublisher.EntityDeleted(order);
        }

        public void UpdateOrder(Order_Omni order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            _orderRepository.Update(order);

            //cache

            _cacheManager.RemoveByPattern(ORDERS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(order);
        }

        public void InsertOrder_D(Order_D order_d)
        {
            if (order_d == null)
                throw new ArgumentNullException("order_d");

            _order_DRepository.Insert(order_d);

            //cache
            _cacheManager.RemoveByPattern(ORDER_DS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(order_d);
        }

        public void DeleteOrder_D(Order_D order_d)
        {
            if (order_d == null)
                throw new ArgumentNullException("order_d");

            _order_DRepository.Delete(order_d);

            //cache
            _cacheManager.RemoveByPattern(ORDER_DS_PATTERN_KEY);


            //event notification
            _eventPublisher.EntityDeleted(order_d);
        }

        public void UpdateOrder_D(Order_D order_d)
        {
            if (order_d == null)
                throw new ArgumentNullException("order_d");

            _order_DRepository.Update(order_d);

            //cache

            _cacheManager.RemoveByPattern(ORDER_DS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(order_d);
        }

        public Order_D GetOrder_DById(int orderdID)
        {
            if (orderdID == 0)
                return null;

            return _order_DRepository.GetById(orderdID);
        }


        public void InsertOrderbyItemSet(OrderbyItemSet orderbyitemset)
        {
            if (orderbyitemset == null)
                throw new ArgumentNullException("orderbyitemset");

            _orderbyitemSetRepository.Insert(orderbyitemset);

            //cache
            _cacheManager.RemoveByPattern(ORDERBYITEMSETS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(orderbyitemset);
        }



        public void DeleteOrderbyItemSet(OrderbyItemSet orderbyitemset)
        {
            if (orderbyitemset == null)
                throw new ArgumentNullException("orderbyitemset");

            _orderbyitemSetRepository.Delete(orderbyitemset);

            //cache
            _cacheManager.RemoveByPattern(ORDERBYITEMSETS_PATTERN_KEY);


            //event notification
            _eventPublisher.EntityDeleted(orderbyitemset);
        }

        public void UpdateOrderbyItemSet(OrderbyItemSet orderbyitemset)
        {
            if (orderbyitemset == null)
                throw new ArgumentNullException("orderbyitemset");

            _orderbyitemSetRepository.Update(orderbyitemset);

            //cache

            _cacheManager.RemoveByPattern(ORDERBYITEMSETS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(orderbyitemset);
        }

        public OrderbyItemSet GetOrderbyItemSetById(int orderbyitemsetid)
        {
            if (orderbyitemsetid == 0)
                return null;

            return _orderbyitemSetRepository.GetById(orderbyitemsetid);
        }

        public IPagedList<Order_Omni> GetAllOrders(string branch, string year, string term, string ordertype, string orderstatus, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _orderRepository.Table;
            if (branch != null && branch != "")
            {
                query = query.Where(x => x.BranchCode == branch);
            }

            if (year != null && year != "")
            {
                query = query.Where(x => x.Year == year);
            }

            if (term != null && term != "")
            {
                query = query.Where(x => x.Term == term);
            }

            if (ordertype != null && ordertype != "")
            {
                query = query.Where(x => x.OrderType == ordertype);
            }

            if (orderstatus != null && orderstatus != "")
            {
                query = query.Where(x => x.Status == orderstatus);
            }



            query = query.OrderByDescending(c => c.OrderDate);


            try
            {
                var pageorders = new PagedList<Order_Omni>(query, pageIndex, pageSize);
                return pageorders;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }


        public List<Order_D> GetAllOrder_Ds(string branch, string year, string term, string ordertype, string orderstatus, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _orderRepository.Table;
            if (branch != null && branch != "")
            {
                query = query.Where(x => x.BranchCode == branch);
            }

            if (year != null && year != "")
            {
                query = query.Where(x => x.Year == year);
            }

            if (term != null && term != "")
            {
                query = query.Where(x => x.Term == term);
            }

            if (ordertype != null && ordertype != "")
            {
                query = query.Where(x => x.OrderType == ordertype);
            }

            if (orderstatus != null && orderstatus != "")
            {
                query = query.Where(x => x.Status == orderstatus);
            }


            var query2 = _order_DRepository.Table;

            query2 = query2.OrderBy(c => c.SEQ);

            var query3 = query.Join(query2, x => x.Id, y => y.Order_Id, (x, y) => new {y.Id,  y.Order_Id, y.SEQ,y.ItemCode, y.ItemSet_Id, y.ItemSet_Seq, y.Qty, y.UnitPrice, y.is_Half, y.reg_date, y.reg_source, y.Remarks})
                .OrderBy(x => x.ItemCode).ToList();

            try
            {
                var orderlist = query3.Select(x =>
                {
                    var itemorder = new Order_D()
                    {
                        Id = x.Id,
                        Order_Id = x.Order_Id,
                        SEQ = x.SEQ,
                        Remarks = x.Remarks,
                        ItemCode = x.ItemCode,
                        ItemSet_Id = x.ItemSet_Id,
                        ItemSet_Seq = x.ItemSet_Seq,
                        Qty = x.Qty,
                        UnitPrice = x.UnitPrice,
                        is_Half = x.is_Half,
                        reg_date = x.reg_date,
                        reg_source = x.reg_source
                    };

                    return itemorder;
                });

                return orderlist.ToList();
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }

        public IPagedList<Order_D> GetAllOrder_Ds(Order_Omni order, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _order_DRepository.Table;
            query = query.Where(c => c.Order_Id == order.Id);
            query = query.OrderBy(c => c.SEQ);

            try
            {
                var pagedorder_ds = new PagedList<Order_D>(query, pageIndex, pageSize);
                return pagedorder_ds;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }

        public IPagedList<OrderbyItemSet> GetAllOrderbyItemSets(Order_Omni order, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _orderbyitemSetRepository.Table;
            query = query.Where(c => c.Order_Id == order.Id);
            query = query.OrderBy(c => c.ItemSet_Id);

            try
            {
                var pagedorder_ds = new PagedList<OrderbyItemSet>(query, pageIndex, pageSize);
                return pagedorder_ds;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }


        public IPagedList<OrderbyItemSet> GetAllOrderbyItemSets( int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _orderbyitemSetRepository.Table.OrderBy( x => x.Order_Id);

            try
            {
                var pageditemset = new PagedList<OrderbyItemSet>(query, pageIndex, pageSize);
                return pageditemset;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }




        public PrintingPaper GetPrintingPaperById(int printingpaperID)
        {
            if (printingpaperID == 0)
                return null;

            return _printingpaperRepository.GetById(printingpaperID);
        }


        public void InsertPrintingPaper(PrintingPaper printingpaper)
        {
            if (printingpaper == null)
                throw new ArgumentNullException("printingpaper");

            try
            {
                _printingpaperRepository.Insert(printingpaper);

                //cache

                _cacheManager.RemoveByPattern(PRINTINGPAPER_BY_ID_KEY);

                //event notification
                _eventPublisher.EntityInserted(printingpaper);
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
        }

        public void DeletePrintingPaper(PrintingPaper printingpaper)
        {
            if (printingpaper == null)
                throw new ArgumentNullException("stock");

            _printingpaperRepository.Delete(printingpaper);

            //cache
            _cacheManager.RemoveByPattern(PRINTINGPAPER_BY_ID_KEY);


            //event notification
            _eventPublisher.EntityDeleted(printingpaper);
        }

        public void UpdatePrintingPaper(PrintingPaper printingpaper)
        {
            if (printingpaper == null)
                throw new ArgumentNullException("stock");

            _printingpaperRepository.Update(printingpaper);

            //cache

            _cacheManager.RemoveByPattern(PRINTINGPAPER_BY_ID_KEY);

            //event notification
            _eventPublisher.EntityUpdated(printingpaper);
        }


        public IPagedList<PrintingPaper> GetAllPrintingPapers(string branch, string year, string term, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _printingpaperRepository.Table;
            if ( !String.IsNullOrEmpty(branch) )
            {
                query = query.Where(c => c.BranchCode == branch);
            }
            if (!String.IsNullOrEmpty(year))
            {
                query = query.Where(c => c.Year == year);
            }
            if (!String.IsNullOrEmpty(term))
            {
                query = query.Where(c => c.Term == term);
            }

            query = query.OrderBy(c => c.BranchCode).ThenBy(c=>c.Course_Id).ThenBy(c=>c.TestNo);

            try
            {
                var printingpapers = new PagedList<PrintingPaper>(query, pageIndex, pageSize);
                return printingpapers;
            }
            catch (Exception ex)
            {
                return null;
            }
        }







        public Stock GetStockById(int stockID)
        {
            if (stockID == 0)
                return null;

            return _stockRepository.GetById(stockID);
        }

        public void InsertStock(Stock stock)
        {
            if (stock == null)
                throw new ArgumentNullException("stock");

            try
            {
                _stockRepository.Insert(stock);

                //cache

                _cacheManager.RemoveByPattern(STOCKS_PATTERN_KEY);

                //event notification
                _eventPublisher.EntityInserted(stock);
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
        }

        public void DeleteStock(Stock stock)
        {
            if (stock == null)
                throw new ArgumentNullException("stock");

            _stockRepository.Delete(stock);

            //cache
            _cacheManager.RemoveByPattern(STOCKS_PATTERN_KEY);


            //event notification
            _eventPublisher.EntityDeleted(stock);
        }

        public void UpdateStock(Stock stock)
        {
            if (stock == null)
                throw new ArgumentNullException("stock");

            _stockRepository.Update(stock);

            //cache

            _cacheManager.RemoveByPattern(STOCKS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(stock);
        }

        public IPagedList<Stock> GetAllStocks(string itemcode, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _stockRepository.Table;
            if (itemcode != null && itemcode != "")
            {
                query = query.Where(c => c.ItemCode == itemcode);
            }
            query = query.OrderBy(c => c.TranDate);

            try
            {
                var stocks = new PagedList<Stock>(query, pageIndex, pageSize);
                return stocks;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
