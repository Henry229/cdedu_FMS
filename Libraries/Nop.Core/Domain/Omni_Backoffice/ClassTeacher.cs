
namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class ClassTeacher : BaseEntity
    {
        public ClassTeacher() { }

        public ClassTeacher(int class_id, int teacher_id,  decimal duration, string remarks, System.DateTime reg_date, string reg_source)
        {
            this.Class_Id = class_id;
            this.Teacher_Id = teacher_id;
            //this.FirstName = firstname;
            //this.LastName = lastname;
            //this.Subject = subject;
            this.Duration = duration;
            this.Remarks = remarks;
            this.reg_date = reg_date;
            this.reg_source = reg_source;
        }

        public int Class_Id { get; set; }

        public int Teacher_Id { get; set; }

        //public string FirstName { get; set; }

        //public string LastName { get; set; }

        //public string Subject { get; set; }

        public decimal Duration { get; set; }

        public string Remarks { get; set; }

        public System.DateTime reg_date { get; set; }

        public string reg_source { get; set; }


    }
}
