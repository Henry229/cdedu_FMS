
namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class ClassEnrol_Pay : BaseEntity
    {
        public ClassEnrol_Pay() { }

        public ClassEnrol_Pay(int id, int id_enrol, int seq, System.DateTime paydate, decimal payamount, string remarks, System.DateTime reg_date, string reg_source)
        {
            this.Id = id;
            this.Id_Enrol = id_enrol;
            this.Seq = seq;
            this.PayDate = paydate;
            this.PayAmount = payamount;
            this.Remarks = remarks;
            this.reg_date = reg_date;
            this.reg_source = reg_source;
        }
        public int Id { get; set; }
        
        public int Id_Enrol { get; set; }

        public int Seq { get; set; }

        public System.DateTime PayDate { get; set; }

        public decimal PayAmount { get; set; }

        public string Remarks { get; set; }
        public System.DateTime reg_date { get; set; }

        public string reg_source { get; set; }


    }
}
