
namespace Nop.Core.Domain.Omni_Printing
{
    public partial class PrintItemSpec : BaseEntity
    {
        public PrintItemSpec() { }

        public PrintItemSpec(int itemid, int specid, int sortorder, string ynuse, string remarks, System.DateTime regdate, string regsource)
        {
            Item_Id = itemid;
            Spec_Id = specid;
            SortOrder = sortorder;
            YN_Use = ynuse;
            Remarks = remarks;
            reg_date = regdate;
            reg_source = regsource;
        }

        public int Item_Id { get; set; }

        public int Spec_Id { get; set; }

        public int SortOrder { get; set; }

        public string YN_Use { get; set; }


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
            return "PrintItemSpec";
        }

    }
}
