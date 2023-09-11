
namespace Nop.Core.Domain.Omni_Printing
{
    public partial class PrintRequest : BaseEntity
    {
        public PrintRequest() { }

        public PrintRequest(string title, string contenttext, System.DateTime reqdate, System.DateTime duedate, string status, int fileid, string userid, System.DateTime regdate, string regsource)
        {
            Title = title;
            ContentText = contenttext;
            ReqDate = reqdate;
            DueDate = duedate;
            Status = status;
            File_Id = fileid;
            User_Id = userid;
            reg_date = regdate;
            reg_source = regsource;
        }

        public string Title { get; set; }

        public string ContentText { get; set; }

        public System.DateTime ReqDate { get; set; }

        public System.DateTime DueDate { get; set; }

        public string Status { get; set; }

        public int File_Id { get; set; }

        public string User_Id { get; set; }



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
            return Title;
        }

    }
}
