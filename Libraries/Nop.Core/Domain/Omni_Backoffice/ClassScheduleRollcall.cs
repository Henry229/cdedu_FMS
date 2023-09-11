
namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class ClassScheduleRollcall : BaseEntity
    {
        public ClassScheduleRollcall() { }

        public ClassScheduleRollcall(int class_id, int class_d_id, string stud_id, string attend, string remarks, int makeupid, System.DateTime reg_date, string reg_source)
        {
            this.Class_Id = class_id;
            this.Class_D_Id = class_d_id;
            this.Stud_Id = stud_id;
            this.Attend = attend;
            this.Remarks = remarks;
            this.reg_date = reg_date;
            this.reg_source = reg_source;
            this.Makeup_Id = makeupid;
        }

        public int Class_Id { get; set; }

        public int Class_D_Id { get; set; }

        public string Stud_Id { get; set; }

        public string Attend { get; set; }

        public string Remarks { get; set; }

        public int Makeup_Id { get; set; }

        public System.DateTime reg_date { get; set; }

        public string reg_source { get; set; }


    }
}
