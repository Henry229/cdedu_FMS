using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nop.Core.Domain.Omni_Printing;
using Nop.Core;


namespace Nop.Services.Omni_Printing
{
    public partial interface IPrintService
    {

        PrintItem GetItemById(int itemID);

        void InsertItem(PrintItem item);

        void DeleteItem(PrintItem item);

        void UpdateItem(PrintItem item);

        IPagedList<PrintItem> GetAllItems(int pageIndex = 0, int pageSize = 2147483647);



        PrintSpec GetSpecById(int specID);

        void InsertSpec(PrintSpec spec);

        void DeleteSpec(PrintSpec spec);

        void UpdateSpec(PrintSpec spec);

        IPagedList<PrintSpec> GetAllSpecs(int pageIndex = 0, int pageSize = 2147483647);


        PrintItemSpec GetItemSpecById(int itemspecID);

        void InsertItemSpec(PrintItemSpec itemspec);

        void DeleteItemSpec(PrintItemSpec itemspec);

        void UpdateItemSpec(PrintItemSpec itemspec);

        IPagedList<PrintItemSpec> GetAllItemSpecs(int itemtype, int pageIndex = 0, int pageSize = 2147483647);


        PrintRequest GetRequestById(int requestID);

        void InsertRequest(PrintRequest request);

        void DeleteRequest(PrintRequest request);

        void UpdateRequest(PrintRequest request);

        IPagedList<PrintRequest> GetAllRequests(DateTime fromdate, DateTime todate, string status, string userid, int pageIndex = 0, int pageSize = 2147483647);


        PrintRequestItem GetRequestItemById(int requestitemID);

        void InsertRequestItem(PrintRequestItem requestitem);

        void DeleteRequestItem(PrintRequestItem requestitem);

        void UpdateRequestItem(PrintRequestItem requestitem);

        IPagedList<PrintRequestItem> GetAllRequestItems(int req_id, int pageIndex = 0, int pageSize = 2147483647);



        PrintRequestItemSpec GetRequestItemSpecById(int requestitemspecID);

        void InsertRequestItemSpec(PrintRequestItemSpec requestitemspec);

        void DeleteRequestItemSpec(PrintRequestItemSpec requestitemspec);

        void UpdateRequestItemSpec(PrintRequestItemSpec requestitemspec);

        IPagedList<PrintRequestItemSpec> GetAllRequestItemSpecs(int req_id, int pageIndex = 0, int pageSize = 2147483647);


        PrintQuotation GetQuotationById(int quotationID);

        void InsertQuotation(PrintQuotation quotation);

        void DeleteQuotation(PrintQuotation quotation);

        void UpdateQuotation(PrintQuotation quotation);

        IPagedList<PrintQuotation> GetAllQuotations(int req_id, int pageIndex = 0, int pageSize = 2147483647);



        #region print pictures

        /// <summary>
        /// Deletes a product picture
        /// </summary>
        /// <param name="productPicture">Product picture</param>
        void DeleteProductPicture(PrintPicture printPicture);

        /// <summary>
        /// Gets a product pictures by product identifier
        /// </summary>
        /// <param name="productId">The product identifier</param>
        /// <returns>Product pictures</returns>
        IList<PrintPicture> GetPrintPicturesByRequestItemId(int fileId);

        /// <summary>
        /// Gets a product picture
        /// </summary>
        /// <param name="productPictureId">Product picture identifier</param>
        /// <returns>Product picture</returns>
        PrintPicture GetPrintPictureById(int fileid);

        /// <summary>
        /// Inserts a product picture
        /// </summary>
        /// <param name="productPicture">Product picture</param>
        void InsertPrintPicture(PrintPicture printPicture);

        /// <summary>
        /// Updates a product picture
        /// </summary>
        /// <param name="productPicture">Product picture</param>
        void UpdatePrintPicture(PrintPicture printPicture);

        #endregion

    }
}
