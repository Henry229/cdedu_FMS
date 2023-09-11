namespace Nop.Core.Domain.Omni_Backoffice
{
    public partial class Campus : BaseEntity
    {
        public Campus() { }

        public Campus(
            string Campus_Prefix,
            string Campus_Title,
            string Campus_EN,
            string Suburb,
            string State,
            string Address1, 
            string Address2,
            string PostCode,
            string ABN,
            string PhoneNo,
            string Email,
            string RoyaltyType,
            string RoyaltyType_Hi,
            string RoyaltyType_PA,
            string RoyaltyType_TT,
            string YN_Use,
            string LicenseeCode,
            System.DateTime reg_date,
            string reg_source
            )
        {
            this.Campus_Prefix = Campus_Prefix ;
            this.Campus_Title = Campus_Title ;
            this.Campus_EN = Campus_EN;
            this.Suburb = Suburb ;
            this.State = State ;
            this.Address1 = Address1 ; 
            this.Address2 = Address2 ;
            this.PostCode = PostCode ;
            this.ABN = ABN;
            this.PhoneNo = PhoneNo ;
            this.Email = Email ;
            this.RoyaltyType = RoyaltyType ;
            this.RoyaltyType_HI = RoyaltyType_Hi;
            this.RoyaltyType_PA = RoyaltyType_PA ;
            this.RoyaltyType_TT = RoyaltyType_TT ;
            this.YN_Use = YN_Use ;
            this.LicenseeCode = LicenseeCode;
            this.reg_date = reg_date ;
            this.reg_source = reg_source ;
        }

        public string Campus_Title { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Campus_Prefix { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Campus_EN { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Suburb { get; set; }
        public string State { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostCode { get; set; }
        public string ABN { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string RoyaltyType { get; set; }
        public string RoyaltyType_HI { get; set; }
        public string RoyaltyType_PA { get; set; }
        public string RoyaltyType_TT { get; set; }
        public string YN_Use { get; set; }

        public string LicenseeCode { get; set; }
        public System.DateTime reg_date { get; set; }
        public string reg_source { get; set; }


        public override string ToString()
        {
            return Campus_Title;
        }
    }
}
