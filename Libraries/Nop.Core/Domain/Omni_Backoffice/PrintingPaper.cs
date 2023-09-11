using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class PrintingPaper : BaseEntity
    {

        public PrintingPaper() { }

        public PrintingPaper(string year, string term, string branch, int course_Id, int testNo, int qty, int qty_teacher, DateTime reg_date, string reg_source)
        {
            this.Year = year;
            this.Term = term;
            this.BranchCode = branch;
            this.Course_Id = course_Id;
            this.TestNo = testNo;
            this.Qty = qty;
            this.Qty_Teacher = qty_teacher;
            
            this.reg_date = reg_date;
            this.reg_source = reg_source;

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
        public string BranchCode { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public int Course_Id { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public int TestNo { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public int Qty_Teacher { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public int Qty_NewBook { get; set; }

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
            return Year + "-" + Term + Course_Id.ToString() + TestNo.ToString();
        }
    }
}
