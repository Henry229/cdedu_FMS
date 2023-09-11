
namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class TeacherSubject : BaseEntity
    {
        public TeacherSubject() { }

        public TeacherSubject(int teacher_id, string Subject, System.DateTime reg_date, string reg_source)
        {
            this.Teacher_Id = teacher_id;
            this.Subject = Subject;
            this.reg_date = reg_date;
            this.reg_source = reg_source;
        }

        public int Teacher_Id { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Subject { get; set; }

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
            return Teacher_Id.ToString() + ", " + Subject;
        }

    }
}
