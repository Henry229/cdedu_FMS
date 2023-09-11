
namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class Parent : BaseEntity
    {
        public Parent() { }

        public Parent(int id, string firstname, string lastname, string branch, string phoneno, string mobileno1, string mobileno2, string email, string remarks, System.DateTime reg_date, string reg_source)
        {
            this.Id = id;
            this.FirstName = firstname;
            this.LastName = lastname;
            this.Branch = branch;
            this.PhoneNo = phoneno;
            this.MobileNo1 = mobileno1;
            this.MobileNo2 = mobileno2;
            this.Email = email;
            this.Remarks = remarks; 
            this.reg_date = reg_date;
            this.reg_source = reg_source;
        }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Branch { get; set; }

        public string PhoneNo { get; set; }

        public string MobileNo1 { get; set; }

        public string MobileNo2 { get; set; }

        public string Email { get; set; }

        public string Remarks { get; set; }

        public System.DateTime reg_date { get; set; }

        public string reg_source { get; set; }

    }
}
