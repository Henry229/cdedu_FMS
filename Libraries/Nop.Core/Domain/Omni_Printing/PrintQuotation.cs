
namespace Nop.Core.Domain.Omni_Printing
{
    public partial class PrintQuotation : BaseEntity
    {
        public PrintQuotation() { }

        public PrintQuotation(int reqid, System.DateTime duedate, string contenttext, decimal quotamnt, int fileid, System.DateTime regdate, string regsource)
        {
            Req_Id = reqid;
            Due_date = duedate;
            ContentText = contenttext;
            File_Id = fileid;
            Quot_Amount = quotamnt;
            reg_date = regdate;
            reg_source = regsource;
        }

        public int Req_Id { get; set; }

        public System.DateTime Due_date { get; set; }

        public string ContentText { get; set; }

        public decimal Quot_Amount { get; set; }

        public int File_Id { get; set; }



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
            return "Quotation no." + this.Req_Id;
        }

    }
}
