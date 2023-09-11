
namespace Nop.Core.Domain.Omni_Printing
{
    public partial class PrintItem : BaseEntity
    {
        public PrintItem() { }

        public PrintItem(string itemname, string remarks, System.DateTime regdate, string regsource)
        {
            ItemName = itemname;
            Remarks = remarks;
            reg_date = regdate;
            reg_source = regsource;
        }

        public string ItemName { get; set; }

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
