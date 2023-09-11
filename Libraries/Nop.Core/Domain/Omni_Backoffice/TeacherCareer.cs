
namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class TeacherCareer : BaseEntity
    {
        public TeacherCareer() { }

        public TeacherCareer(int teacher_id, string careertype, System.DateTime fromdate, System.DateTime todate, int class_id
            , System.DateTime reg_date, string reg_source, string remarks)
        {
            this.Teacher_Id = teacher_id;
            this.CareerType = careertype;
            this.FromDate = fromdate;
            this.ToDate = todate;
            this.Class_Id = class_id;
            this.reg_date = reg_date;
            this.reg_source = reg_source;
            this.Remarks = remarks;
        }

        public int Teacher_Id { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string CareerType { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public System.DateTime FromDate { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public System.DateTime ToDate { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public int Class_Id { get; set; }


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
            return Teacher_Id.ToString() + ", " + CareerType;
        }

    }
}
