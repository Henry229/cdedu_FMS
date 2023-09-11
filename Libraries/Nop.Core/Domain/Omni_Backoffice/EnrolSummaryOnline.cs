namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class EnrolSummaryOnline : BaseEntity
    {
        public EnrolSummaryOnline() { }

        public EnrolSummaryOnline(
            string year,
            string branch,
            string grade,
            int T1,
            int T2,
            int T3,
            int T4
            )
        {
            this.year = year;
            this.branch = branch;
            this.grade = grade;
            this.T1 = T1;
            this.T2 = T2;
            this.T3 = T3;
            this.T4 = T4;
        }

        public string year { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string branch { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string grade { get; set; }

        public int T1 { get; set; }
        public int T2 { get; set; }
        public int T3 { get; set; }
        public int T4 { get; set; }

    }
}
