
namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class ClassInfo : BaseEntity
    {
        public ClassInfo() { }

        public ClassInfo(string year, string term, string branch, string grade, string name, string dayofweek,
            System.DateTime starttime, System.DateTime endtime, 
            decimal duration, int classroom_id, int course_id, System.DateTime reg_date, string reg_source)
        {
            this.Year = year;
            this.Term = term;
            this.Branch = branch;
            this.Grade = grade;
            this.Name = name;
            this.DayofWeek = dayofweek;
            this.StartTime = starttime;
            this.EndTime = endtime;
            this.Duration = duration;
            this.Classroom_Id = classroom_id;
            this.Course_Id = course_id;
            this.reg_date = reg_date;
            this.reg_source = reg_source;
        }

        public string Year { get; set; }

        public string Term { get; set; }

        public string Branch { get; set; }

        public string Grade { get; set; }

        public string Name { get; set; }

        public string DayofWeek { get; set; }

        public System.DateTime StartTime { get; set; }

        public System.DateTime EndTime { get; set; }

        //public string StartTime { get; set; }

        //public string EndTime { get; set; }

        public decimal Duration { get; set; }

        public string Remarks { get; set; }

        public int Classroom_Id { get; set; }

        public int Course_Id { get; set; }

        public System.DateTime reg_date { get; set; }

        public string reg_source { get; set; }


    }
}
