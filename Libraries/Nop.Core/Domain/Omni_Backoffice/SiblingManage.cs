
namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class SiblingManage : BaseEntity
    {
        public SiblingManage() { }

        public SiblingManage(int id, int parent_id, int seq, string stud_id, string id_number, string remarks, System.DateTime reg_date, string reg_source)
        {
            this.Id = id;
            this.Parent_Id = parent_id;
            this.Seq = seq;
            this.Stud_Id = stud_id;
            this.Id_Number = id_number;
            this.Remarks = remarks; 
            this.reg_date = reg_date;
            this.reg_source = reg_source;
        }

        public int Id { get; set; }

        public int Parent_Id { get; set; }

        public int Seq { get; set; }

        public string Stud_Id { get; set; }

        public string Id_Number { get; set; }

         public string Remarks { get; set; }

        public System.DateTime reg_date { get; set; }

        public string reg_source { get; set; }

    }
}
