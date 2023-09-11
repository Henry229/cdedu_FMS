namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class CalendarMaster : BaseEntity
    {
        public CalendarMaster() { }

        public CalendarMaster(string Year, string Term, int Week, System.DateTime StartDate, System.DateTime EndDate
            , string YN_Active, string YN_PA, string YN_Enrol, System.DateTime reg_date, string reg_source
            , string Remarks)
        {
            this.Year = Year;
            this.Term = Term;
            this.Week = Week;
            this.StartDate = StartDate;
            this.EndDate = EndDate;
            this.reg_date = reg_date;
            this.reg_source = reg_source;
            this.Remarks = Remarks;
        }
        
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Term { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public int Week { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public System.DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public System.DateTime EndDate { get; set; }

        public string YN_Active { get; set; }
        public string YN_PA { get; set; }
        public string YN_Enrol { get; set; }
        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public System.DateTime reg_date { get; set; }


        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string reg_source { get; set; }


        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Remarks { get; set; }
    }
}
