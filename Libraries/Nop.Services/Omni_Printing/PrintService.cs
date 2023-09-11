using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Omni_Printing;
using Nop.Services.Events;

namespace Nop.Services.Omni_Printing
{
    public partial class PrintService : IPrintService
    {
        #region Constants

        private const string PRINTITEM_BY_ID_KEY = "Nop.printitem.id-{0}";
        private const string PRINTITEMS_ALL_KEY = "Nop.printitem.all";
        private const string PRINTITEMS_PATTERN_KEY = "Nop.printitem.";

        private const string PRINTSPEC_BY_ID_KEY = "Nop.printspec.id-{0}";
        private const string PRINTSPECS_ALL_KEY = "Nop.printspec.all";
        private const string PRINTSPECS_PATTERN_KEY = "Nop.printspec.";

        private const string PRINTITEMSPEC_BY_ID_KEY = "Nop.printitemspec.id-{0}";
        private const string PRINTITEMSPECS_ALL_KEY = "Nop.printitemspec.all";
        private const string PRINTITEMSPECS_PATTERN_KEY = "Nop.printitemspec.";

        private const string PRINTREQUEST_BY_ID_KEY = "Nop.printrequest.id-{0}";
        private const string PRINTREQUESTS_ALL_KEY = "Nop.printrequest.all";
        private const string PRINTREQUESTS_PATTERN_KEY = "Nop.printrequest.";

        private const string PRINTREQUESTITEM_BY_ID_KEY = "Nop.printrequestitem.id-{0}";
        private const string PRINTREQUESTITEMS_ALL_KEY = "Nop.printrequestitem.all";
        private const string PRINTREQUESTITEMS_PATTERN_KEY = "Nop.printrequestitem.";

        private const string PRINTREQUESTITEMSPEC_BY_ID_KEY = "Nop.printrequestitemspec.id-{0}";
        private const string PRINTREQUESTITEMSPECS_ALL_KEY = "Nop.printrequestitemspec.all";
        private const string PRINTREQUESTITEMSPECS_PATTERN_KEY = "Nop.printrequestitemspec.";

        private const string PRINTQUOTATION_BY_ID_KEY = "Nop.printquotation.id-{0}";
        private const string PRINTQUOTATIONS_ALL_KEY = "Nop.printquotation.all";
        private const string PRINTQUOTATIONS_PATTERN_KEY = "Nop.printquotation.";

        #endregion

        #region Fields

        private readonly IRepository<PrintItem> _itemRepository;
        private readonly IRepository<PrintSpec> _specRepository;
        private readonly IRepository<PrintItemSpec> _itemspecRepository;
        private readonly IRepository<PrintRequest> _requestRepository;
        private readonly IRepository<PrintRequestItem> _requestitemRepository;
        private readonly IRepository<PrintRequestItemSpec> _requestitemspecRepository;
        private readonly IRepository<PrintQuotation> _quotationRepository;
        private readonly IRepository<PrintPicture> _printpictureRepository;
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
        public PrintService(ICacheManager cacheManager, IEventPublisher eventPublisher,
            IRepository<PrintItem> itemRepository, IRepository<PrintSpec> specRepository,
            IRepository<PrintItemSpec> itemspecRepository, IRepository<PrintRequest> requestRepository,
            IRepository<PrintRequestItem> requestitemRepository, IRepository<PrintRequestItemSpec> requestitemspecRepository,
            IRepository<PrintPicture> printpictureRepository, IRepository<PrintQuotation> quotationRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._itemRepository = itemRepository;
            this._specRepository = specRepository;
            this._itemspecRepository = itemspecRepository;
            this._requestRepository = requestRepository;
            this._requestitemRepository = requestitemRepository;
            this._requestitemspecRepository = requestitemspecRepository;
            this._quotationRepository = quotationRepository;

            this._printpictureRepository = printpictureRepository;

        }

        #endregion

        public PrintItem GetItemById(int itemID)
        {
            if (itemID == 0)
                return null;

            return _itemRepository.GetById(itemID);
        }

        public void InsertItem(PrintItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            try
            {
                _itemRepository.Insert(item);
                _cacheManager.RemoveByPattern(PRINTITEMS_PATTERN_KEY);
                _eventPublisher.EntityInserted(item);
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
        }

        public void DeleteItem(PrintItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            _itemRepository.Delete(item);
            _cacheManager.RemoveByPattern(PRINTITEMS_PATTERN_KEY);
            _eventPublisher.EntityDeleted(item);
        }

        public void UpdateItem(PrintItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            _itemRepository.Update(item);
            _cacheManager.RemoveByPattern(PRINTITEMS_PATTERN_KEY);
            _eventPublisher.EntityUpdated(item);
        }




        public IPagedList<PrintItem> GetAllItems(int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _itemRepository.Table;
            query = query.OrderByDescending(c => c.Id);

            try
            {
                var pageorders = new PagedList<PrintItem>(query, pageIndex, pageSize);
                return pageorders;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }




        public PrintSpec GetSpecById(int specID)
        {
            if (specID == 0)
                return null;

            return _specRepository.GetById(specID);
        }

        public void InsertSpec(PrintSpec spec)
        {
            if (spec == null)
                throw new ArgumentNullException("spec");

            try
            {
                _specRepository.Insert(spec);
                _cacheManager.RemoveByPattern(PRINTSPECS_PATTERN_KEY);
                _eventPublisher.EntityInserted(spec);
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
        }

        public void DeleteSpec(PrintSpec spec)
        {
            if (spec == null)
                throw new ArgumentNullException("spec");

            _specRepository.Delete(spec);
            _cacheManager.RemoveByPattern(PRINTSPECS_PATTERN_KEY);
            _eventPublisher.EntityDeleted(spec);
        }

        public void UpdateSpec(PrintSpec spec)
        {
            if (spec == null)
                throw new ArgumentNullException("spec");

            _specRepository.Update(spec);
            _cacheManager.RemoveByPattern(PRINTSPECS_PATTERN_KEY);
            _eventPublisher.EntityUpdated(spec);
        }




        public IPagedList<PrintSpec> GetAllSpecs(int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _specRepository.Table;
            query = query.OrderByDescending(c => c.Id);

            try
            {
                var pageorders = new PagedList<PrintSpec>(query, pageIndex, pageSize);
                return pageorders;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }




        public PrintItemSpec GetItemSpecById(int itemspecID)
        {
            if (itemspecID == 0)
                return null;

            return _itemspecRepository.GetById(itemspecID);
        }

        public void InsertItemSpec(PrintItemSpec itemspec)
        {
            if (itemspec == null)
                throw new ArgumentNullException("itemspec");

            try
            {
                _itemspecRepository.Insert(itemspec);
                _cacheManager.RemoveByPattern(PRINTITEMSPECS_PATTERN_KEY);
                _eventPublisher.EntityInserted(itemspec);
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
        }

        public void DeleteItemSpec(PrintItemSpec itemspec)
        {
            if (itemspec == null)
                throw new ArgumentNullException("itemspec");

            _itemspecRepository.Delete(itemspec);
            _cacheManager.RemoveByPattern(PRINTITEMSPECS_PATTERN_KEY);
            _eventPublisher.EntityDeleted(itemspec);
        }

        public void UpdateItemSpec(PrintItemSpec itemspec)
        {
            if (itemspec == null)
                throw new ArgumentNullException("itemspec");

            _itemspecRepository.Update(itemspec);
            _cacheManager.RemoveByPattern(PRINTITEMSPECS_PATTERN_KEY);
            _eventPublisher.EntityUpdated(itemspec);
        }




        public IPagedList<PrintItemSpec> GetAllItemSpecs(int itemtype, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _itemspecRepository.Table.Where(x => x.YN_Use == "Y");

            if (itemtype != null && itemtype > 0)
            {
                query = query.Where(x => x.Item_Id == itemtype);
            }

            query = query.OrderBy(x => x.Item_Id).ThenBy(x => x.SortOrder);

            try
            {
                var pageorders = new PagedList<PrintItemSpec>(query, pageIndex, pageSize);
                return pageorders;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }


        public PrintRequest GetRequestById(int requestID)
        {
            if (requestID == 0)
                return null;

            return _requestRepository.GetById(requestID);
        }

        public void InsertRequest(PrintRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            try
            {
                _requestRepository.Insert(request);
                _cacheManager.RemoveByPattern(PRINTREQUESTS_PATTERN_KEY);
                _eventPublisher.EntityInserted(request);
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
        }

        public void DeleteRequest(PrintRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            _requestRepository.Delete(request);
            _cacheManager.RemoveByPattern(PRINTREQUESTS_PATTERN_KEY);
            _eventPublisher.EntityDeleted(request);
        }

        public void UpdateRequest(PrintRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            _requestRepository.Update(request);
            _cacheManager.RemoveByPattern(PRINTREQUESTS_PATTERN_KEY);
            _eventPublisher.EntityUpdated(request);
        }




        public IPagedList<PrintRequest> GetAllRequests(DateTime fromdate, DateTime todate, string status, string userid, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _requestRepository.Table;
            if ( fromdate != null)
            {
                query = query.Where(x => x.ReqDate >= fromdate);
            }

            if (todate != null)
            {
                query = query.Where(x => x.ReqDate <= todate);
            }

            if (!String.IsNullOrEmpty(status))
            {
                query = query.Where(x => x.Status == status);
            }

            if ( !String.IsNullOrEmpty(userid))
            {
                query = query.Where(x => x.User_Id == userid);
            }



            query = query.OrderByDescending(c => c.ReqDate);

            try
            {
                var pageorders = new PagedList<PrintRequest>(query, pageIndex, pageSize);
                return pageorders;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }




        public PrintRequestItem GetRequestItemById(int requestitemID)
        {
            if (requestitemID == 0)
                return null;

            return _requestitemRepository.GetById(requestitemID);
        }

        public void InsertRequestItem(PrintRequestItem requestitem)
        {
            if (requestitem == null)
                throw new ArgumentNullException("requestitem");

            try
            {
                _requestitemRepository.Insert(requestitem);
                _cacheManager.RemoveByPattern(PRINTREQUESTITEMS_PATTERN_KEY);
                _eventPublisher.EntityInserted(requestitem);
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
        }

        public void DeleteRequestItem(PrintRequestItem requestitem)
        {
            if (requestitem == null)
                throw new ArgumentNullException("requestitem");

            _requestitemRepository.Delete(requestitem);
            _cacheManager.RemoveByPattern(PRINTREQUESTITEMS_PATTERN_KEY);
            _eventPublisher.EntityDeleted(requestitem);
        }

        public void UpdateRequestItem(PrintRequestItem requestitem)
        {
            if (requestitem == null)
                throw new ArgumentNullException("requestitem");

            _requestitemRepository.Update(requestitem);
            _cacheManager.RemoveByPattern(PRINTREQUESTITEMS_PATTERN_KEY);
            _eventPublisher.EntityUpdated(requestitem);
        }




        public IPagedList<PrintRequestItem> GetAllRequestItems( int req_id, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _requestitemRepository.Table;
            if (req_id != null && req_id > 0)
                query = query.Where(x => x.Req_Id == req_id);
            query = query.OrderByDescending(c => c.Id);

            try
            {
                var pageorders = new PagedList<PrintRequestItem>(query, pageIndex, pageSize);
                return pageorders;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }





        public PrintRequestItemSpec GetRequestItemSpecById(int requestitemspecID)
        {
            if (requestitemspecID == 0)
                return null;

            return _requestitemspecRepository.GetById(requestitemspecID);
        }

        public void InsertRequestItemSpec(PrintRequestItemSpec requestitemspec)
        {
            if (requestitemspec == null)
                throw new ArgumentNullException("requestitemspec");

            try
            {
                _requestitemspecRepository.Insert(requestitemspec);
                _cacheManager.RemoveByPattern(PRINTREQUESTITEMSPECS_PATTERN_KEY);
                _eventPublisher.EntityInserted(requestitemspec);
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
        }

        public void DeleteRequestItemSpec(PrintRequestItemSpec requestitemspec)
        {
            if (requestitemspec == null)
                throw new ArgumentNullException("requestitemspec");

            _requestitemspecRepository.Delete(requestitemspec);
            _cacheManager.RemoveByPattern(PRINTREQUESTITEMSPECS_PATTERN_KEY);
            _eventPublisher.EntityDeleted(requestitemspec);
        }

        public void UpdateRequestItemSpec(PrintRequestItemSpec requestitemspec)
        {
            if (requestitemspec == null)
                throw new ArgumentNullException("requestitemspec");

            _requestitemspecRepository.Update(requestitemspec);
            _cacheManager.RemoveByPattern(PRINTREQUESTITEMSPECS_PATTERN_KEY);
            _eventPublisher.EntityUpdated(requestitemspec);
        }




        public IPagedList<PrintRequestItemSpec> GetAllRequestItemSpecs(int req_id, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _requestitemspecRepository.Table;

            if ( req_id != null && req_id > 0)
            {
                query = query.Where(x => x.Req_Id == req_id);
            }

            query = query.OrderByDescending(c => c.Spec_Id);

            try
            {
                var pageorders = new PagedList<PrintRequestItemSpec>(query, pageIndex, pageSize);
                return pageorders;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }


        public PrintQuotation GetQuotationById(int quotationID)
        {
            if (quotationID == 0)
                return null;

            return _quotationRepository.GetById(quotationID);
        }

        public void InsertQuotation(PrintQuotation quotation)
        {
            if (quotation == null)
                throw new ArgumentNullException("quotation");

            try
            {
                _quotationRepository.Insert(quotation);
                _cacheManager.RemoveByPattern(PRINTQUOTATIONS_PATTERN_KEY);
                _eventPublisher.EntityInserted(quotation);
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
        }

        public void DeleteQuotation(PrintQuotation quotation)
        {
            if (quotation == null)
                throw new ArgumentNullException("quotation");

            _quotationRepository.Delete(quotation);
            _cacheManager.RemoveByPattern(PRINTQUOTATIONS_PATTERN_KEY);
            _eventPublisher.EntityDeleted(quotation);
        }

        public void UpdateQuotation(PrintQuotation quotation)
        {
            if (quotation == null)
                throw new ArgumentNullException("quotation");

            _quotationRepository.Update(quotation);
            _cacheManager.RemoveByPattern(PRINTQUOTATIONS_PATTERN_KEY);
            _eventPublisher.EntityUpdated(quotation);
        }




        public IPagedList<PrintQuotation> GetAllQuotations(int req_id, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _quotationRepository.Table;
            if (req_id != null && req_id > 0)
                query = query.Where(x => x.Req_Id == req_id);
            query = query.OrderByDescending(c => c.Id);

            try
            {
                var pageorders = new PagedList<PrintQuotation>(query, pageIndex, pageSize);
                return pageorders;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }


        #region Image Upload



        public virtual void DeleteProductPicture(PrintPicture printpicture)
        {
            if (printpicture == null)
                throw new ArgumentNullException("printpicture");

            _printpictureRepository.Delete(printpicture);

            //event notification
            _eventPublisher.EntityDeleted(printpicture);
        }

        public virtual IList<PrintPicture> GetPrintPicturesByRequestItemId(int requestitem)
        {
            var query = from pp in _printpictureRepository.Table
                        where pp.RequestItemId == requestitem
                        orderby pp.DisplayOrder
                        select pp;
            var printPictures = query.ToList();
            return printPictures;
        }

        public virtual PrintPicture GetPrintPictureById(int fileId)
        {
            if (fileId == 0)
                return null;

            return _printpictureRepository.GetById(fileId);
        }


        public virtual void InsertPrintPicture(PrintPicture printpicture)
        {
            if (printpicture == null)
                throw new ArgumentNullException("printpicture");

            _printpictureRepository.Insert(printpicture);

            //event notification
            _eventPublisher.EntityInserted(printpicture);
        }

        public virtual void UpdatePrintPicture(PrintPicture printpicture)
        {
            if (printpicture == null)
                throw new ArgumentNullException("productPicture");

            _printpictureRepository.Update(printpicture);

            //event notification
            _eventPublisher.EntityUpdated(printpicture);
        }

        #endregion


    }
}
