
namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class Evaluation : BaseEntity
    {
        public Evaluation() { }

        public Evaluation(string evaluationtype, int teacher_Id, int class_id, string evaluator, int score, System.DateTime reg_date, string reg_source, string remarks)
        {
            this.EvaluationType = evaluationtype;
            this.Teacher_Id = teacher_Id;
            this.Class_Id = class_id;
            this.Evaluator = evaluator;
            this.Score = score;
            
            this.reg_date = reg_date;
            this.reg_source = reg_source;
            this.Remarks = remarks;
        }

        public string EvaluationType { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public int Teacher_Id { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public int Class_Id { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Evaluator { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public int Score { get; set; }

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
            return EvaluationType + ", " + Teacher_Id.ToString();
        }

    }
}
