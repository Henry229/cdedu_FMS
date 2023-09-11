namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class MarketingFeePayment : BaseEntity
    {
        public MarketingFeePayment() { }

        public MarketingFeePayment(int fee_id, System.DateTime paydate, decimal amount, string remarks, System.DateTime reg_date, string reg_source)
        {
            this.Fee_Id = fee_id;
            this.PayDate = paydate;
            this.Amount = amount;
            this.Remarks = remarks;

            this.reg_date = reg_date;
            this.reg_source = reg_source;
        }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public int Fee_Id { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public System.DateTime PayDate { get; set; }

        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Remarks { get; set; }

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
            return PayDate.ToString("yyyy-MM-dd");
        }

    }
}
