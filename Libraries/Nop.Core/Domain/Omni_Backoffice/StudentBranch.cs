
namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class StudentBranch : BaseEntity
    {
        public StudentBranch() { }

        public StudentBranch(string stud_id, string branch, string remarks, System.DateTime reg_date, string reg_source)
        {
            this.Stud_Id = stud_id;
            this.Branch = branch;
            this.Remarks = remarks;
            this.reg_date = reg_date;
            this.reg_source = reg_source;
        }
        public string Stud_Id { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Branch { get; set; }

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
            return Stud_Id.ToString() + ", " + Branch;
        }

    }
}
