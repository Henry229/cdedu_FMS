
namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class Closing : BaseEntity
    {
        public Closing() { }

        public Closing(string year, string term, string type, string branch, int seq, string yn_closing, string yn_approval, string yn_paid
            , decimal amnt_adjust, decimal amnt_freight, System.DateTime reg_date, string reg_source, string remarks, string duedate)
        {
            this.Year = year;
            this.Term = term;
            this.Type = type;
            this.Branch = branch;
            this.SEQ = seq;
            this.YN_Closing = yn_closing;
            this.YN_Approval = yn_approval;
            this.Amnt_adjust = amnt_adjust;
            this.Amnt_Freight = amnt_freight;
            this.reg_date = reg_date;
            this.reg_source = reg_source;
            this.Remarks = remarks;
            this.DueDate = duedate;
        }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Term { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public int SEQ { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string YN_Closing { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string YN_Approval { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string YN_Paid { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public decimal Amnt_adjust { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public decimal Amnt_Freight { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string DueDate { get; set; }

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
            return Year+"-"+Type+SEQ.ToString("000")+Branch;
        }

    }
}
