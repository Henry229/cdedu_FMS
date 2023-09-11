namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class CourseMaster : BaseEntity
    {
        public CourseMaster() { }

        public CourseMaster(string CourseName, string CourseCategory, int CourseID_P, string Year, string Term, string Grade, string Level, int StartWeek, int TotalWeek, decimal CourseFee, string Remarks
            , string YN_Use, System.DateTime reg_date, string reg_source, decimal BookFee, decimal Facility, decimal NewBookFee)
        {
            this.CourseName = CourseName;
            this.CourseCategory = CourseCategory;
            this.CourseID_P = CourseID_P;
            this.Year = Year;
            this.Term = Term;
            this.Grade = Grade;
            this.Level = Level;
            this.StartWeek = StartWeek;
            this.TotalWeek = TotalWeek;
            this.CourseFee = CourseFee;
            this.Remarks = Remarks;
            this.YN_Use = YN_Use;
            this.reg_date = reg_date;
            this.reg_source = reg_source;
            this.BookFee = BookFee;
            this.Facility = Facility;
            this.NewBookFee = NewBookFee;
        }
        
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string CourseCategory { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public int CourseID_P { get; set; }

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
        public string Grade { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Level { get; set; }

        public int StartWeek { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public int TotalWeek { get; set; }


        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public decimal CourseFee { get; set; }


        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Remarks { get; set; }


        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string YN_Use { get; set; }

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
        public decimal BookFee { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public decimal Facility { get; set; }

        public decimal NewBookFee { get; set; }

        public override string ToString()
        {
            return CourseName;
        }
    }
}
