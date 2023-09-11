
namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class TeacherBranch : BaseEntity
    {
        public TeacherBranch() { }

        public TeacherBranch(int teacher_id, string Branch, System.DateTime reg_date, string reg_source)
        {
            this.Teacher_Id = teacher_id;
            this.Branch = Branch;
            this.reg_date = reg_date;
            this.reg_source = reg_source;
        }

        public int Teacher_Id { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Branch { get; set; }

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
            return Teacher_Id.ToString() + ", " + Branch;
        }

    }
}
