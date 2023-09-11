
namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class ClassRoom : BaseEntity
    {
        public ClassRoom() { }

        public ClassRoom(int id, string branch, string title, string remarks, System.DateTime reg_date, string reg_source)
        {
            this.Id = id;
            this.Branch = branch;
            this.Title = title;
            this.Remarks = remarks; 
            this.reg_date = reg_date;
            this.reg_source = reg_source;
        }

        public int Id { get; set; }

        public string Branch { get; set; }
        
        public string Title { get; set; }

        public string Remarks { get; set; }

        public System.DateTime reg_date { get; set; }

        public string reg_source { get; set; }

    }
}
