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
    public partial class RoyaltyService : IRoyaltyService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        private const string ENROLLMENTS_BY_ID_KEY = "Nop.order.id-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string ENROLLMENTS_ALL_KEY = "Nop.order.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string ENROLLMENTS_PATTERN_KEY = "Nop.order.";


        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        private const string CLOSING_BY_ID_KEY = "Nop.closing.id-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string CLOSING_ALL_KEY = "Nop.closing.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CLOSING_PATTERN_KEY = "Nop.closing.";


        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        private const string ORDER_PASALES_BY_ID_KEY = "Nop.order_pasales.id-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string ORDER_PASALES_ALL_KEY = "Nop.order_pasales.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string ORDER_PASALES_PATTERN_KEY = "Nop.order_pasales.";

        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        private const string PASALES_BY_ID_KEY = "Nop.pasales.id-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string PASALES_ALL_KEY = "Nop.pasales.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string PASALES_PATTERN_KEY = "Nop.pasales.";

        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        private const string MARKETINGFEE_BY_ID_KEY = "Nop.marketingfee.id-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string MARKETINGFEES_ALL_KEY = "Nop.marketingfee.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string MARKETINGFEES_PATTERN_KEY = "Nop.marketingfee.";


        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        private const string MARKETINGFEEPAYMENT_BY_ID_KEY = "Nop.marketingfeepayment.id-{0}";

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string MARKETINGFEEPAYMENTS_ALL_KEY = "Nop.marketingfeepayment.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string MARKETINGFEEPAYMENTS_PATTERN_KEY = "Nop.marketingfeepayment.";




        #endregion

        #region Fields

        private readonly IRepository<Enrollment> _enrollmentRepository;
        private readonly IRepository<CourseMaster> _courseRepository;
        private readonly IRepository<Closing> _closingRepository;
        private readonly IRepository<PASales> _pasalesRepository;
        private readonly IRepository<Order_PASales> _order_pasalesRepository;
        private readonly IRepository<StudentSummary> _studentsummaryRepository;
        private readonly IRepository<MarketingFee> _marketingfeeRepository;
        private readonly IRepository<MarketingFeePayment> _marketingfeepaymentRepository;

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
        public RoyaltyService(ICacheManager cacheManager, IEventPublisher eventPublisher,
            IRepository<Enrollment> enrollmentRepository, IRepository<CourseMaster> courseRepository,
            IRepository<Closing> closingRepository, IRepository<PASales> pasalesRepository, IRepository<Order_PASales> order_pasalesRepository,
            IRepository<StudentSummary> studentsummaryRepository, IRepository<MarketingFee> marketingfeeRepository
            , IRepository<MarketingFeePayment> marketingfeepaymentRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._courseRepository = courseRepository;
            this._enrollmentRepository = enrollmentRepository;
            this._closingRepository = closingRepository;
            this._pasalesRepository = pasalesRepository;
            this._order_pasalesRepository = order_pasalesRepository;
            this._studentsummaryRepository = studentsummaryRepository;
            this._marketingfeeRepository = marketingfeeRepository;
            this._marketingfeepaymentRepository = marketingfeepaymentRepository;
        }

        #endregion


        public Enrollment GetEnrollmentById(int enrollmentID)
        {
            if (enrollmentID == 0)
                return null;
            return _enrollmentRepository.GetById(enrollmentID);
        }


        public void InsertEnrollment(Enrollment enrollment)
        {
            if (enrollment == null)
                throw new ArgumentNullException("enrollment");

            try
            {
                _enrollmentRepository.Insert(enrollment);

                //cache

                _cacheManager.RemoveByPattern(ENROLLMENTS_PATTERN_KEY);

                //event notification
                _eventPublisher.EntityInserted(enrollment);
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
        }

        public void DeleteEnrollment(Enrollment enrollment)
        {
            if (enrollment == null)
                throw new ArgumentNullException("enrollment");

            _enrollmentRepository.Delete(enrollment);

            //cache
            _cacheManager.RemoveByPattern(ENROLLMENTS_PATTERN_KEY);


            //event notification
            _eventPublisher.EntityDeleted(enrollment);
        }

        public void UpdateEnrollment(Enrollment enrollment)
        {
            if (enrollment == null)
                throw new ArgumentNullException("enrollment");

            _enrollmentRepository.Update(enrollment);

            //cache

            _cacheManager.RemoveByPattern(ENROLLMENTS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(enrollment);
        }

        public IPagedList<Enrollment> GetAllEnrollments(string branch, int course_id, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _enrollmentRepository.Table;
            if (branch != null && branch != "")
            {
                query = query.Where(x => x.BranchCode == branch);
            }

            if (course_id != null && course_id != 0)
            {
                query = query.Where(x => x.Course_Id == course_id);
            }


            query = query.OrderBy(c => c.BranchCode);

            try
            {
                var pageenrollments = new PagedList<Enrollment>(query, pageIndex, pageSize);
                return pageenrollments;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }


        #region Closing
        /// <summary>
        /// Gets all invoiceno
        /// </summary>
        /// <returns>codemaster collection</returns>
        public virtual Closing GetClosing(string year, string term, string type, string branch)
        {
            var query = _closingRepository.Table;
            query = query.OrderBy( c => c.Branch);
            try
            {
                var pageditems = new PagedList<Closing>(query, 0, 100000000).OrderBy( x => x.Branch);

                var result = pageditems.Where(x => x.Year == year && x.Term == term && x.Type == type && x.Branch == branch).OrderBy( x => x.Branch).ToList();

                if (result == null || result.Count <= 0)
                    return null;
                return result[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public virtual void InsertClosing(Closing closing)
        {
            if (closing == null)
                throw new ArgumentNullException("closing");


            _closingRepository.Insert(closing);

            //cache

            _cacheManager.RemoveByPattern(CLOSING_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(closing);

        }


        public void DeleteClosing(Closing closing)
        {
            if (closing == null)
                throw new ArgumentNullException("closing");

            _closingRepository.Delete(closing);

            //cache
            _cacheManager.RemoveByPattern(CLOSING_PATTERN_KEY);


            //event notification
            _eventPublisher.EntityDeleted(closing);
        }

        public void UpdateClosing(Closing closing)
        {
            if (closing == null)
                throw new ArgumentNullException("enrollment");

            _closingRepository.Update(closing);

            //cache

            _cacheManager.RemoveByPattern(CLOSING_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(closing);
        }

        public IPagedList<Closing> GetAllClosings(string branch, string year, string term, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _closingRepository.Table;
            if (branch != null && branch != "")
            {
                query = query.Where(x => x.Branch == branch);
            }
            if (year != null && year != "")
            {
                query = query.Where(x => x.Year == year);
            }
            if (term != null && term != "")
            {
                query = query.Where(x => x.Term == term);
            }

            query = query.OrderBy(c => c.Branch);

            try
            {
                var pageclosings = new PagedList<Closing>(query, pageIndex, pageSize);
                return pageclosings;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }


        #endregion





        public void UpdatePA_Sales(PASales pasales)
        {
            if (pasales == null)
                throw new ArgumentNullException("pasales");

            _pasalesRepository.Update(pasales);

            //cache

            _cacheManager.RemoveByPattern(PASALES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(pasales);
        }

        public IPagedList<PASales> GetAllPA_Sales(int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _pasalesRepository.Table;
           
                query = query.Where(x => x.IF_Flag == "N");


            query = query.OrderBy(c => c.reg_date);

            try
            {
                var pageclosings = new PagedList<PASales>(query, pageIndex, pageSize);
                return pageclosings;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }

        public Order_PASales GetOrder_PASalesById(int orderpaID)
        {
            if (orderpaID == 0)
                return null;
            return _order_pasalesRepository.GetById(orderpaID);
        }


        public void InsertOrder_PASales(Order_PASales order_pasales)
        {
            if (order_pasales == null)
                throw new ArgumentNullException("order_pasales");


            _order_pasalesRepository.Insert(order_pasales);

            //cache

            _cacheManager.RemoveByPattern(ORDER_PASALES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(order_pasales);
        }

        public void DeleteOrder_PASales(Order_PASales order_pasales)
        {
            if (order_pasales == null)
                throw new ArgumentNullException("order_pasales");

            _order_pasalesRepository.Delete(order_pasales);

            //cache
            _cacheManager.RemoveByPattern(ORDER_PASALES_PATTERN_KEY);


            //event notification
            _eventPublisher.EntityDeleted(order_pasales);
        }

        public void UpdateOrder_PASales(Order_PASales order_pasales)
        {
            if (order_pasales == null)
                throw new ArgumentNullException("order_pasales");

            _order_pasalesRepository.Update(order_pasales);

            //cache

            _cacheManager.RemoveByPattern(ORDER_PASALES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(order_pasales);
        }

        public IPagedList<Order_PASales> GetAllOrder_PASales(string branch, string year, string term, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _order_pasalesRepository.Table;
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

            query = query.OrderBy(c => c.BranchCode);

            try
            {
                var pageclosings = new PagedList<Order_PASales>(query, pageIndex, pageSize);
                return pageclosings;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }

        public IPagedList<StudentSummary> GetStudentSummary(string year, string term, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _studentsummaryRepository.Table;

            if (year != null && year != "")
            {
                query = query.Where(x => x.Year == year);
            }
            if (term != null && term != "")
            {
                query = query.Where(x => x.Term == term);
            }

            query = query.OrderBy(c => c.Branch);

            try
            {
                var pagestudentsummary = new PagedList<StudentSummary>(query, 0, 2147483647);
                return pagestudentsummary;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }


        public MarketingFee GetMarketingFeeById(int marketingfeeID)
        {
            if (marketingfeeID == 0)
                return null;
            return _marketingfeeRepository.GetById(marketingfeeID);
        }


        public void InsertMarketingFee(MarketingFee marketingfee)
        {
            if (marketingfee == null)
                throw new ArgumentNullException("marketingfee");

            try
            {
                _marketingfeeRepository.Insert(marketingfee);

                //cache

                _cacheManager.RemoveByPattern(MARKETINGFEES_PATTERN_KEY);

                //event notification
                _eventPublisher.EntityInserted(marketingfee);
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
        }

        public void DeleteMarketingFee(MarketingFee marketingfee)
        {
            if (marketingfee == null)
                throw new ArgumentNullException("marketingfee");

            _marketingfeeRepository.Delete(marketingfee);

            //cache
            _cacheManager.RemoveByPattern(MARKETINGFEES_PATTERN_KEY);


            //event notification
            _eventPublisher.EntityDeleted(marketingfee);
        }

        public void UpdateMarketingFee(MarketingFee marketingfee)
        {
            if (marketingfee == null)
                throw new ArgumentNullException("marketingfee");

            _marketingfeeRepository.Update(marketingfee);

            //cache

            _cacheManager.RemoveByPattern(MARKETINGFEES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(marketingfee);
        }

        public IPagedList<MarketingFee> GetAllMarketingFees(string branch, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _marketingfeeRepository.Table;
            if (branch != null && branch != "")
            {
                query = query.Where(x => x.Branch == branch);
            }

            query = query.OrderBy(c => c.Branch);

            try
            {
                var pagemarketingfees = new PagedList<MarketingFee>(query, pageIndex, pageSize);
                return pagemarketingfees;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }


        public MarketingFeePayment GetMarketingFeePaymentById(int marketingfeepaymentID)
        {
            if (marketingfeepaymentID == 0)
                return null;
            return _marketingfeepaymentRepository.GetById(marketingfeepaymentID);
        }


        public void InsertMarketingFeePayment(MarketingFeePayment marketingfeepayment)
        {
            if (marketingfeepayment == null)
                throw new ArgumentNullException("marketingfeepayment");

            try
            {
                _marketingfeepaymentRepository.Insert(marketingfeepayment);

                //cache

                _cacheManager.RemoveByPattern(MARKETINGFEEPAYMENTS_PATTERN_KEY);

                //event notification
                _eventPublisher.EntityInserted(marketingfeepayment);
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
        }

        public void DeleteMarketingFeePayment(MarketingFeePayment marketingfeepayment)
        {
            if (marketingfeepayment == null)
                throw new ArgumentNullException("marketingfeepayment");

            _marketingfeepaymentRepository.Delete(marketingfeepayment);

            //cache
            _cacheManager.RemoveByPattern(MARKETINGFEEPAYMENTS_PATTERN_KEY);


            //event notification
            _eventPublisher.EntityDeleted(marketingfeepayment);
        }

        public void UpdateMarketingFeePayment(MarketingFeePayment marketingfeepayment)
        {
            if (marketingfeepayment == null)
                throw new ArgumentNullException("marketingfeepayment");

            _marketingfeepaymentRepository.Update(marketingfeepayment);

            //cache

            _cacheManager.RemoveByPattern(MARKETINGFEEPAYMENTS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(marketingfeepayment);
        }

        public IPagedList<MarketingFeePayment> GetAllMarketingFeePayments(int fee_id, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _marketingfeepaymentRepository.Table;
            if (fee_id != null && fee_id != 0)
            {
                query = query.Where(x => x.Fee_Id == fee_id);
            }

            query = query.OrderBy(c => c.PayDate);

            try
            {
                var pagemarketingfeepayments = new PagedList<MarketingFeePayment>(query, pageIndex, pageSize);
                return pagemarketingfeepayments;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }


    }
}
