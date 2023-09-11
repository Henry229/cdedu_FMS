namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class Enrollment : BaseEntity
    {
        public Enrollment() { }

        public Enrollment(string branchcode, int course_id, int week, int qty_book, int qty_modified, System.DateTime reg_date, string reg_source)
        {

            this.BranchCode = branchcode;
            this.Course_Id = course_id;
            this.Week = week;
            this.Qty_Book = qty_book;
            this.Qty_Modified = qty_modified;
            this.reg_date = reg_date;
            this.reg_source = reg_source;
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string BranchCode { get; set; }

        public int Course_Id { get; set; }

        public int Week { get; set; }

        public int Qty_Book { get; set; }

        public int Qty_Modified { get; set; }

        public System.DateTime reg_date { get; set; }

        public string reg_source { get; set; }

    }
}
