namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class TestSummaryOnline : BaseEntity
    {
        public TestSummaryOnline() { }

        public TestSummaryOnline(
            int myear,
            string branch,
            string testtype,
            int test_no,
            int cnt
            )
        {
            this.myear = myear;
            this.branch = branch;
            this.testtype = testtype;
            this.test_no = test_no;
            this.cnt = cnt;
        }

        public int myear { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string branch { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string testtype { get; set; }

        public int test_no { get; set; }
        public int cnt { get; set; }

    }
}
