namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class Member : BaseEntity
    {
        public Member() { }

        public Member(
            string stud_id,
            string stud_first_name,
            string stud_last_name,
            string stud_birth_year,
            string stud_birth_month,
            string stud_birth_day,
            string stud_school_name,
            string grade,
            string branch,
            string id_number,
            string telephone,
            string mobilephone,
            string parentmobilephone
            )
        {
            this.stud_id = stud_id;
            this.stud_first_name = stud_first_name;
            this.stud_last_name = stud_last_name;
            this.stud_birth_year = stud_birth_year;
            this.stud_birth_month = stud_birth_month;
            this.stud_birth_day = stud_birth_day;
            this.stud_school_name = stud_school_name;
            this.grade = grade;
            this.branch = branch;
            this.id_number = id_number;
            this.Telephone = telephone;
            this.MobilePhone = mobilephone;
            this.ParentMobilePhone = parentmobilephone;
            

        }

        public string stud_id { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string stud_first_name { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string stud_last_name { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string stud_birth_year { get; set; }
        public string stud_birth_month { get; set; }
        public string stud_birth_day { get; set; }
        public string stud_school_name { get; set; }
        public string grade { get; set; }
        public string branch { get; set; }
        public string id_number { get; set; }

        public string Telephone { get; set; }
        public string MobilePhone { get; set; }

        public string ParentMobilePhone { get; set; }


        public override string ToString()
        {
            return stud_first_name + ", " + stud_last_name;
        }
    }
}
