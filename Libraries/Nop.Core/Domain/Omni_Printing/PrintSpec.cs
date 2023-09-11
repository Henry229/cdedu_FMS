
namespace Nop.Core.Domain.Omni_Printing
{
    public partial class PrintSpec : BaseEntity
    {
        public PrintSpec() { }

        public PrintSpec(string specname, string specification, int sortorder, string ynuse, string remarks, System.DateTime regdate, string regsource)
        {
            SpecName = specname;
            Specification = specification;
            SortOrder = sortorder;
            YN_Use = ynuse;
            Remarks = remarks;
            reg_date = regdate;
            reg_source = regsource;
        }

        public string SpecName { get; set; }

        public string Specification { get; set; }

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
            return SpecName;
        }

    }
}
