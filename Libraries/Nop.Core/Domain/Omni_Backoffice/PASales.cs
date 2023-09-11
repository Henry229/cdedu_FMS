using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class PASales : BaseEntity
    {
        
        public PASales() { }

        public PASales(int sales_id, string branch, string stud_advisor, string title, decimal price, decimal list_price, string payment, bool payment_flag, string stud_id, DateTime purchase_date, string if_flag, DateTime reg_date, string reg_source)
        {
            this.Sales_id = sales_id;
            this.Branch = branch;
            this.Stud_Advisor = stud_advisor;
            this.Title = title;
            this.Price = price;
            this.List_Price = list_price;
            this.Payment = payment;
            this.Payment_Flag = payment_flag;
            this.Stud_id = stud_id;
            this.Purchase_Date = purchase_date;
            this.IF_Flag = if_flag;
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
        public string Branch { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Stud_Advisor { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Title { get; set; }

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
        public bool Payment_Flag { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Stud_id { get; set; }


        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public System.DateTime Purchase_Date { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string IF_Flag { get; set; }


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
