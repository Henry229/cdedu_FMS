
namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class ClassSchedule : BaseEntity
    {
        public ClassSchedule() { }

        public ClassSchedule(int class_id, System.DateTime class_date, System.DateTime class_starttime, System.DateTime class_endtime, int classroom_id, string yn_close, string remarks, System.DateTime reg_date, string reg_source)
        {
            this.Class_Id = class_id;
            this.Class_Date = class_date;
            this.Class_StartTime = class_starttime;
            this.Class_EndTime = class_endtime;
            this.Classroom_Id = classroom_id;
            this.YN_Close = yn_close;
            this.Remarks = remarks;
            this.reg_date = reg_date;
            this.reg_source = reg_source;
        }

        public int Class_Id { get; set; }

        public System.DateTime Class_Date { get; set; }

        public System.DateTime Class_StartTime { get; set; }

        public System.DateTime Class_EndTime { get; set; }

        public int Classroom_Id { get; set; }

        public string YN_Close { get; set; }

        public string Remarks { get; set; }

        public System.DateTime reg_date { get; set; }

        public string reg_source { get; set; }


    }
}
