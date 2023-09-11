namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class PrimeVideoSummary : BaseEntity
    {
        public PrimeVideoSummary() { }

        public PrimeVideoSummary(
            string stud_id,
            int test_idx,
            int cnt_unit,
            
            System.DateTime activated_date,
            System.DateTime deactivated_date
            )
        {
            this.stud_id = stud_id;
            this.test_idx = test_idx;
            this.cnt_unit = cnt_unit;
            this.activated_date = activated_date;
            this.deactivated_date = deactivated_date;
        }


        public string stud_id { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public int test_idx { get; set; }

        
        public int cnt_unit { get; set; }

        public System.DateTime deactivated_date { get; set; }

        public System.DateTime activated_date { get; set; }


        public override string ToString()
        {
            return stud_id + "(" + test_idx + ")";
        }
    }
}
