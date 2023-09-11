using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nop.Core.Domain.Omni_Backoffice;
using Nop.Core;


namespace Nop.Services.Omni_Backoffice
{
    public partial interface IOrderService_Omni
    {
        /// <summary>
        /// Gets a Item by identifier
        /// </summary>
        /// <param name="orderID">Order identifier</param>
        /// <returns>Setting</returns>
        Order_Omni GetOrderById(int orderID);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="order">Order</param>
        void InsertOrder(Order_Omni order);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="order">Order</param>
        void DeleteOrder(Order_Omni order);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="order">Order</param>
        void UpdateOrder(Order_Omni order);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="order">Order</param>
        void InsertOrder_D(Order_D order_d);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="order">Order</param>
        void DeleteOrder_D(Order_D order_d);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="order">Order</param>
        void UpdateOrder_D(Order_D order_d);


        /// <summary>
        /// Gets a Item by identifier
        /// </summary>
        /// <param name="order_dID">Order identifier</param>
        /// <returns>Setting</returns>
        Order_D GetOrder_DById(int order_dID);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="order">Order</param>
        void InsertOrderbyItemSet(OrderbyItemSet orderbyitemset);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="order">Order</param>
        void DeleteOrderbyItemSet(OrderbyItemSet orderbyitemset);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="order">Order</param>
        void UpdateOrderbyItemSet(OrderbyItemSet orderbyitemset);


        /// <summary>
        /// Gets a Item by identifier
        /// </summary>
        /// <param name="order_dID">Order identifier</param>
        /// <returns>Setting</returns>
        OrderbyItemSet GetOrderbyItemSetById(int orderbyitemsetid);



        /// <summary>
        /// Gets all order
        /// </summary>
        /// <returns>order</returns>
        IPagedList<Order_Omni> GetAllOrders(string branch, string year, string term, string ordertype, string orderstatus, int pageIndex = 0, int pageSize = 2147483647);

        /// <summary>
        /// Gets all order_d
        /// </summary>
        /// <returns>order_d</returns>
        List<Order_D> GetAllOrder_Ds(string branch, string year, string term, string ordertype, string orderstatus, int pageIndex = 0, int pageSize = 2147483647);

        /// <summary>
        /// Gets all order_d
        /// </summary>
        /// <returns>order_d</returns>
        IPagedList<Order_D> GetAllOrder_Ds(Order_Omni order, int pageIndex = 0, int pageSize = 2147483647);

        /// <summary>
        /// Gets all order_d
        /// </summary>
        /// <returns>order_d</returns>
        IPagedList<OrderbyItemSet> GetAllOrderbyItemSets(Order_Omni order, int pageIndex = 0, int pageSize = 2147483647);

        /// <summary>
        /// Gets all order_d
        /// </summary>
        /// <returns>order_d</returns>
        IPagedList<OrderbyItemSet> GetAllOrderbyItemSets(int pageIndex = 0, int pageSize = 2147483647);





        /// <summary>
        /// Gets a Item by identifier
        /// </summary>
        /// <param name="printingpaperID">Order identifier</param>
        /// <returns>Setting</returns>
        PrintingPaper GetPrintingPaperById(int printingpaperID);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="order">Order</param>
        void InsertPrintingPaper(PrintingPaper printingpaper);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="order">Order</param>
        void DeletePrintingPaper(PrintingPaper printingpaper);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="order">Order</param>
        void UpdatePrintingPaper(PrintingPaper printingpaper);

        /// <summary>
        /// Gets all order
        /// </summary>
        /// <returns>order</returns>
        IPagedList<PrintingPaper> GetAllPrintingPapers(string branch, string year, string term, int pageIndex = 0, int pageSize = 2147483647);





        /// <summary>
        /// Gets a stock by identifier
        /// </summary>
        /// <param name="stockID">Order identifier</param>
        /// <returns>Setting</returns>
        Stock GetStockById(int stockID);

        /// <summary>
        /// Deletes a stock
        /// </summary>
        /// <param name="stock">Order</param>
        void InsertStock(Stock stock);

        /// <summary>
        /// Deletes a stock
        /// </summary>
        /// <param name="stock">stock</param>
        void DeleteStock(Stock stock);

        /// <summary>
        /// Deletes a stock
        /// </summary>
        /// <param name="stock">stock</param>
        void UpdateStock(Stock stock);

        /// <summary>
        /// Gets all order
        /// </summary>
        /// <returns>order</returns>
        IPagedList<Stock> GetAllStocks(string itemcode, int pageIndex = 0, int pageSize = 2147483647);


    }
}
