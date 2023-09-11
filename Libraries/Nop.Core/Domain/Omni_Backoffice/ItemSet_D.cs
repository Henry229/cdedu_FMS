namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class ItemSet_D : BaseEntity
    {
        public ItemSet_D() { }

        public ItemSet_D(int set_id, int seq, string itemcode, string remarks, System.DateTime reg_date, string reg_source)
        {
            this.Set_Id = set_id;
            this.Seq = seq;
            this.ItemCode = itemcode;
            this.Remarks = remarks;
            this.reg_date = reg_date;
            this.reg_source = reg_source;
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public int Set_Id { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public int Seq { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string ItemCode { get; set; }

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
            return ItemCode;
        }
    }
}
