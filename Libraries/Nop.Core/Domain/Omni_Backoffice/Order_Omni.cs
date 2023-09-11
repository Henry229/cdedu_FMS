using System;

namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class Order_Omni : BaseEntity
    {
        private DateTime purchase_Date1;
        private object p;
        private DateTime purchase_Date2;
        private DateTime purchase_Date3;
        private string v1;
        private string v2;
        private decimal price;
        private int v3;
        private string v4;
        private DateTime now;
        private string v5;

        public Order_Omni() { }

        public Order_Omni(string branchcode, System.DateTime orderdate, System.DateTime? confirm_date, System.DateTime estimatedate, string ordertype, string status, string year, string term, decimal sumprice
            , int week, string payment, System.DateTime reg_date, string reg_source)
        {
            this.BranchCode = branchcode;
            this.OrderDate = orderdate;
            this.confirm_date = confirm_date;
            this.EstimateDate = estimatedate;
            this.OrderType = ordertype;
            this.Status = status;
            this.Year = year;
            this.Term = term;
            this.SumPrice = sumprice;
            this.Week = week;
            this.Payment = payment;
            this.reg_date = reg_date;
            this.reg_source = reg_source;
        }

        public Order_Omni(string branchCode, DateTime purchase_Date1, object p, DateTime purchase_Date2, DateTime purchase_Date3, string v1, string v2, string year, string term, decimal price, int v3, string v4, DateTime now, string v5)
        {
            BranchCode = branchCode;
            this.purchase_Date1 = purchase_Date1;
            this.p = p;
            this.purchase_Date2 = purchase_Date2;
            this.purchase_Date3 = purchase_Date3;
            this.v1 = v1;
            this.v2 = v2;
            Year = year;
            Term = term;
            this.price = price;
            this.v3 = v3;
            this.v4 = v4;
            this.now = now;
            this.v5 = v5;
        }


        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string BranchCode { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public System.DateTime OrderDate { get; set; }

        public System.DateTime? EstimateDate { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string OrderType { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Term { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public decimal SumPrice { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public int? Week { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Payment { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public System.DateTime? confirm_date { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public System.DateTime? print_date { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public System.DateTime? deliver_date { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public System.DateTime reg_date { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string reg_source { get; set; }


        public override string ToString()
        {
            return BranchCode + OrderDate.ToString("yyyy-MM-dd");            
        }

    }
}
