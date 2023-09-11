
namespace Nop.Core.Domain.Omni_Printing
{
    public partial class PrintRequestItem : BaseEntity
    {
        public PrintRequestItem() { }

        public PrintRequestItem(int reqid, int itemid, string remarks, int fileid, System.DateTime regdate, string regsource)
        {
            Req_Id = reqid;
            Item_Type = itemid;
            Remarks = remarks;
            File_Id = fileid;
            reg_date = regdate;
            reg_source = regsource;
        }

        public int Req_Id { get; set; }

        public int Item_Type { get; set; }

        public string Remarks { get; set; }

        public int File_Id { get; set; }



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
            return Remarks;
        }

    }
}
