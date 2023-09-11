
namespace Nop.Core.Domain.Omni_Printing
{
    public partial class PrintRequestItemSpec : BaseEntity
    {
        public PrintRequestItemSpec() { }

        public PrintRequestItemSpec(int reqid, int itemid, int specid, System.DateTime regdate, string regsource)
        {
            Req_Id = reqid;
            Item_Id = itemid;
            Spec_Id = specid;
            reg_date = regdate;
            reg_source = regsource;
        }

        public int Req_Id { get; set; }

        public int Item_Id { get; set; }


        public int Spec_Id { get; set; }



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
            return "PrintRequestItemSpec";
        }

    }
}
