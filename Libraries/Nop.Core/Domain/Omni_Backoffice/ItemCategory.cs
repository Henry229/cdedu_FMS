namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class ItemCategory : BaseEntity
    {
        public ItemCategory() { }

        public ItemCategory(string categorycode, string categoryname, string remarks, System.DateTime reg_date, string reg_source)
        {
            this.CategoryCode = categorycode;
            this.CategoryName = categoryname;
            this.Remarks = remarks;
            this.reg_date = reg_date;
            this.reg_source = reg_source;
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string CategoryName { get; set; }

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
            return CategoryName;
        }
    }
}
