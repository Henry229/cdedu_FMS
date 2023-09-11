namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class Order_D :BaseEntity
    {
        public Order_D() { }

        public Order_D(int order_id, int seq, string itemcode, int itemset_id, int itemset_seq, int qty, decimal unitprice
            , string is_half, string remarks, System.DateTime reg_date, string reg_source) 
        {
            this.Order_Id = order_id;
            this.SEQ = seq;
            this.ItemCode = itemcode;
            this.ItemSet_Id = itemset_id;
            this.ItemSet_Seq = itemset_seq;
            this.Qty = qty;
            this.UnitPrice = unitprice;
            this.is_Half = is_half;

            this.Remarks = remarks;
            this.reg_date = reg_date;
            this.reg_source = reg_source;

        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public int Order_Id { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public int SEQ { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public int? ItemSet_Id { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public int? ItemSet_Seq { get; set; }
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string is_Half { get; set; }


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
            return Order_Id.ToString() + "-" + SEQ.ToString() + "-" + ItemCode;
        }
    }
}
