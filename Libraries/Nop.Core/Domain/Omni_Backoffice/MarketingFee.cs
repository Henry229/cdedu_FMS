namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class MarketingFee : BaseEntity
    {
        public MarketingFee() { }

        public MarketingFee(string branch, System.DateTime issuedate, string item, decimal amount, string remarks, string yn_paid, System.DateTime reg_date, string reg_source)
        {
            this.Branch = branch;
            this.IssueDate = issuedate;
            this.Item = item;
            this.Amount = amount;
            this.Remarks = remarks;
            this.YN_Paid = yn_paid;
           
            this.reg_date = reg_date;
            this.reg_source = reg_source;
        }


        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public System.DateTime IssueDate { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Item { get; set; }

        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string YN_Paid { get; set; }

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
            return Branch + IssueDate.ToString("yyyy-MM-dd");
        }

    }
}
