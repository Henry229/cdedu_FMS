
namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class ClassEnrol : BaseEntity
    {
        public ClassEnrol() { }

        public ClassEnrol(int id, string Year, string Term, int class_id, string stud_id, int Course_Id, decimal Unit_Price, int AttendWeek, decimal materialfee, string halfbook, string level
            , string Discount_Code, decimal Discount_Rate, string paymethod, decimal TotalAmount, string chk_hd, string chk_fm, string remarks, string paystatus, System.DateTime reg_date, string reg_source)
        {
            this.Id = id;
            this.Year = Year;
            this.Term = Term;
            this.Class_Id = class_id;
            this.Stud_Id = stud_id;
            this.Course_Id = Course_Id;
            this.Unit_Price = Unit_Price;
            this.AttendWeek = AttendWeek;
            this.MaterialFee = materialfee;
            this.HalfBook = halfbook;
            this.Level = level;
            this.Discount_Code = Discount_Code;
            this.Discount_Rate = Discount_Rate;
            this.PayMethod = paymethod;
            this.TotalAmount = TotalAmount;
            this.Remarks = remarks;
            this.Chk_FM = chk_fm;
            this.Chk_HD = chk_hd;
            this.PayStatus = paystatus;
            this.reg_date = reg_date;
            this.reg_source = reg_source;
        }
        public int Id { get; set; }

        public string Year { get; set; }

        public string Term { get; set; }
        
        public int Class_Id { get; set; }

        public string Stud_Id { get; set; }

        public int Course_Id { get; set; }

        public decimal Unit_Price { get; set; }

        public int AttendWeek { get; set; }

        public decimal MaterialFee { get; set; }

        public string HalfBook { get; set; }

        public string Level { get; set; }

        public string Discount_Code { get; set; }

        public decimal Discount_Rate { get; set; }

        public string PayMethod { get; set; }

        public decimal TotalAmount { get; set; }

        public string Remarks { get; set; }

        public string PayStatus { get; set; }

        public string Chk_HD { get; set; }

        public string Chk_FM { get; set; }

        public System.DateTime reg_date { get; set; }

        public string reg_source { get; set; }


    }
}
