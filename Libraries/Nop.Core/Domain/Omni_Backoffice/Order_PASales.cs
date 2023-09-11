using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class Order_PASales : BaseEntity
    {
        
        public Order_PASales() { }

        public Order_PASales(int sales_id, string branchcode, string year, string term, string title, string level, decimal price, decimal list_price, string payment, string stud_id, DateTime purchase_date, DateTime reg_date, string reg_source)
        {
            this.Sales_id = sales_id;
            this.BranchCode = branchcode;
            this.Year = year;
            this.Term = term;

            this.Title = title;
            this.Level = level;
            this.Price = price;
            this.List_Price = list_price;
            this.Payment = payment;
            this.Stud_id = stud_id;
            this.Purchase_Date = purchase_date;
            
            this.reg_date = reg_date;
            this.reg_source = reg_source;
        }


        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public int Sales_id { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string BranchCode { get; set; }

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
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public decimal List_Price { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Payment { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Stud_id { get; set; }

        public System.DateTime Purchase_Date { get; set; }


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
            return Title + "/" + Stud_id;
        }
    }
}
