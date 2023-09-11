
namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class ClassScheduleTeacher : BaseEntity
    {
        public ClassScheduleTeacher() { }

        public ClassScheduleTeacher(int class_d_id, int teacher_id, decimal duration, string remarks, System.DateTime reg_date, string reg_source)
        {
            this.Class_D_Id = class_d_id;
            this.Teacher_Id = teacher_id;
            this.Duration = duration;
            this.Remarks = remarks;
            this.reg_date = reg_date;
            this.reg_source = reg_source;
        }

        public int Class_D_Id { get; set; }

        public int Teacher_Id { get; set; }

        public decimal Duration { get; set; }

        public string Remarks { get; set; }

        public System.DateTime reg_date { get; set; }

        public string reg_source { get; set; }


    }
}
