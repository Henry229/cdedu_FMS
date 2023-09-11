using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nop.Core.Domain.Omni_Backoffice;
using Nop.Core;


namespace Nop.Services.Omni_Backoffice
{
    public partial interface IRoyaltyService
    {
        /// <summary>
        /// Gets a Item by identifier
        /// </summary>
        /// <param name="enrollmentID">Enrollment identifier</param>
        /// <returns>Setting</returns>
        Enrollment GetEnrollmentById(int enrollmentID);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="enrollment">Enrollment</param>
        void InsertEnrollment(Enrollment enrollment);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="enrollment">Enrollment</param>
        void DeleteEnrollment(Enrollment enrollment);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="enrollment">Enrollment</param>
        void UpdateEnrollment(Enrollment enrollment);

        /// <summary>
        /// Gets all enrollment
        /// </summary>
        /// <returns>enrollment</returns>
        IPagedList<Enrollment> GetAllEnrollments(string branch, int course_id, int pageIndex = 0, int pageSize = 2147483647);


        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="closing">Closing</param>
        void InsertClosing(Closing closing);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="closing">Closing</param>
        void DeleteClosing(Closing closing);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="closing">Closing</param>
        void UpdateClosing(Closing closing);

        /// <summary>
        /// Gets all enrollment
        /// </summary>
        /// <returns>enrollment</returns>
        IPagedList<Closing> GetAllClosings(string branch, string year, string term, int pageIndex = 0, int pageSize = 2147483647);


        Closing GetClosing(string year, string term, string type, string branch);


        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="enrollment">Enrollment</param>
        void UpdatePA_Sales(PASales pasales);

        /// <summary>
        /// Gets all enrollment
        /// </summary>
        /// <returns>enrollment</returns>
        IPagedList<PASales> GetAllPA_Sales(int pageIndex = 0, int pageSize = 2147483647);

        /// <summary>
        /// Gets a Item by identifier
        /// </summary>
        /// <param name="orderpaID">Enrollment identifier</param>
        /// <returns>Setting</returns>
        Order_PASales GetOrder_PASalesById(int orderpaID);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="closing">Closing</param>
        void InsertOrder_PASales(Order_PASales order_pasales);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="closing">Closing</param>
        void DeleteOrder_PASales(Order_PASales order_pasales);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="closing">Closing</param>
        void UpdateOrder_PASales(Order_PASales order_pasales);

        /// <summary>
        /// Gets all enrollment
        /// </summary>
        /// <returns>enrollment</returns>
        IPagedList<Order_PASales> GetAllOrder_PASales(string branch, string year, string term, int pageIndex = 0, int pageSize = 2147483647);

        /// <summary>
        /// Gets all enrollment
        /// </summary>
        /// <returns>enrollment</returns>
        IPagedList<StudentSummary> GetStudentSummary(string year, string term, int pageIndex = 0, int pageSize = 2147483647);

        /// <summary>
        /// Gets a Item by identifier
        /// </summary>
        /// <param name="marketingfeeID">MarketingFee identifier</param>
        /// <returns>Setting</returns>
        MarketingFee GetMarketingFeeById(int marketingfeeID);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="marketingfee">MarketingFee</param>
        void InsertMarketingFee(MarketingFee marketingfee);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="marketingfee">MarketingFee</param>
        void DeleteMarketingFee(MarketingFee marketingfee);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="marketingfee">MarketingFee</param>
        void UpdateMarketingFee(MarketingFee marketingfee);

        /// <summary>
        /// Gets all marketingfee
        /// </summary>
        /// <returns>marketingfee</returns>
        IPagedList<MarketingFee> GetAllMarketingFees(string branch, int pageIndex = 0, int pageSize = 2147483647);

        /// <summary>
        /// Gets a Item by identifier
        /// </summary>
        /// <param name="marketingfeepaymentID">MarketingFeePayment identifier</param>
        /// <returns>Setting</returns>
        MarketingFeePayment GetMarketingFeePaymentById(int marketingfeepaymentID);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="marketingfeepayment">MarketingFeePayment</param>
        void InsertMarketingFeePayment(MarketingFeePayment marketingfeepayment);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="marketingfeepayment">MarketingFeePayment</param>
        void DeleteMarketingFeePayment(MarketingFeePayment marketingfeepayment);

        /// <summary>
        /// Deletes a item
        /// </summary>
        /// <param name="marketingfeepayment">MarketingFeePayment</param>
        void UpdateMarketingFeePayment(MarketingFeePayment marketingfeepayment);

        /// <summary>
        /// Gets all marketingfeepayment
        /// </summary>
        /// <returns>marketingfeepayment</returns>
        IPagedList<MarketingFeePayment> GetAllMarketingFeePayments(int fee_id, int pageIndex = 0, int pageSize = 2147483647);


    }
}
