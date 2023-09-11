namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class Item : BaseEntity
    {
        public Item() { }

        public Item(string itemcode, string itemname, string itemcategory, System.DateTime dt_from, System.DateTime dt_to
            , decimal unitprice, decimal unitprice_half, string grade, string term, string level, string subject, string remarks
            , System.DateTime reg_date, string reg_source)
        {
            this.ItemCode = itemcode;
            this.ItemName = itemname;
            this.ItemCategory = itemcategory;
            this.DT_From = dt_from;
            this.DT_To = dt_to;
            this.UnitPrice = unitprice;
            this.UnitPrice_Half = unitprice_half;
            this.Grade = grade;
            this.Term = term;
            this.Level = level;
            this.Subject = subject;
            this.Remarks = remarks;
            this.reg_date = reg_date;
            this.reg_source = reg_source;
        }
        
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string ItemCategory { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public System.DateTime DT_From { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public System.DateTime DT_To { get; set; }


        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public decimal UnitPrice { get; set; }


        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public decimal UnitPrice_Half { get; set; }


        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Term { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the value
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
            return ItemName;
        }
    }
}
