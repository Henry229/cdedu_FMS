
namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class Teacher : BaseEntity
    {
        public Teacher() { }

        public Teacher(string title, string firstname, string lastname, string gender, string teacherrole, string tutorrole, string markerrole, string teachinggrade
            , string subject, string workingcond, string address, string mobile, string homephone
            , string email, string branch, System.DateTime reg_date, string reg_source, string remarks)
        {
            this.Title = title;
            this.FirstName = firstname;
            this.LastName = lastname;
            this.Gender = gender;
            this.TeacherRole = teacherrole;
            this.TutorRole = tutorrole;
            this.MarkerRole = markerrole;
            this.TeachingGrade = teachinggrade;
            this.Subject = subject;
            this.WorkingCond = workingcond;
            this.Address = address;
            this.Mobile = mobile;
            this.HomePhone = homephone;
            this.eMail = email;
            this.Branch = branch;
            this.reg_date = reg_date;
            this.reg_source = reg_source;
            this.Remarks = remarks;
        }

        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string TeacherRole { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string TutorRole { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string MarkerRole { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string TeachingGrade { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string WorkingCond { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string HomePhone { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string eMail { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Remarks { get; set; }

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
            return FirstName + ", " + LastName;
        }

    }
}
