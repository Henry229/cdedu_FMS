
namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class AdditionInfo : BaseEntity
    {
        public AdditionInfo() { }

        public AdditionInfo(int id, string stud_id, string remarks, string actual_grade, System.DateTime reg_date, string reg_source)
        {
            this.Id = id;
            this.Stud_Id = stud_id;
            this.Actual_Grade = actual_grade;
            this.Remarks = remarks;
            this.reg_date = reg_date;
            this.reg_source = reg_source;
        }
        public int Id { get; set; }

        public string Stud_Id { get; set; }

        public string Actual_Grade { get; set; }

        public string Remarks { get; set; }

        public System.DateTime reg_date { get; set; }

        public string reg_source { get; set; }


    }
}
