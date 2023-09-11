namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class ItemSet : BaseEntity
    {
        public ItemSet() { }

        public ItemSet(string setcategory, string setname, string term, string grade, string level
            , int course, string remarks, System.DateTime reg_date, string reg_source)
        {
            this.SetCategory = setcategory;
            this.SetName = setname;
            this.Grade = grade;
            this.Term = term;
            this.Level = level;
            this.Course = course;
            this.Remarks = remarks;
            this.reg_date = reg_date;
            this.reg_source = reg_source;
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string SetCategory { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string SetName { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Term { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public int Course { get; set; }

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
            return SetName;
        }
    }
}
