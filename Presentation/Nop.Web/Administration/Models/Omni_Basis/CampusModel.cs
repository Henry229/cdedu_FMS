using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.ComponentModel.DataAnnotations;
using Nop.Admin.Validators.Omni_Basis;

namespace Nop.Admin.Models.Omni_Basis
{
    //public class CampusModel : BaseNopModel
    public class CampusModel : BaseNopEntityModel
    {
        public CampusModel()
        {
            /*
            CampusCategoryList = new List<SelectListItem>();
            AvailableGradeList = new List<SelectListItem>();
            AvailableTermList = new List<SelectListItem>();
            */
            AvailableRoyaltyType = new List<SelectListItem>();
            AvailableRoyaltyType_PA = new List<SelectListItem>();
            AvailableRoyaltyType_TT = new List<SelectListItem>();

            //AvailableSuburb = new List<SelectListItem>();
        }

        /*
        [NopResourceDisplayName("Admin.Omni_Backoffice.Campus.Fields.Id")]
        [AllowHtml]
        public int Id { get; set; }
        */
        [NopResourceDisplayName("Admin.Omni_Backoffice.Campus.Fields.Campus_Prefix")]
        [AllowHtml]
        public string Campus_Prefix { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Campus.Fields.Campus_Title")]
        public string Campus_Title { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Campus.Fields.Campus_EN")]
        public string Campus_EN { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Campus.Fields.Suburb")]
        public string Suburb { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Campus.Fields.State")]
        public string State { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Campus.Fields.Address1")]
        public string Address1 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.Address2")]
        public string Address2 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.PostCode")]
        public string PostCode { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.ABN")]
        public string ABN { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.PhoneNo")]
        public string PhoneNo { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.Email")]
        public string Email { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.RoyaltyType")]
        public string RoyaltyType { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.RoyaltyType_HI")]
        public string RoyaltyType_HI { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.RoyaltyType_PA")]
        public string RoyaltyType_PA { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.RoyaltyType_TT")]
        public string RoyaltyType_TT { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.YN_Use")]
        public string YN_Use { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.LicenseeCode")]
        public string LicenseeCode { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.reg_date")]
        [AllowHtml]
        public System.DateTime reg_date { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.reg_source")]
        [AllowHtml]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string reg_source { get; set; }

        public IList<SelectListItem> AvailableRoyaltyType { get; set; }

        public IList<SelectListItem> AvailableRoyaltyType_PA { get; set; }

        public IList<SelectListItem> AvailableRoyaltyType_TT { get; set; }

        /*
        [NopResourceDisplayName("Admin.Omni_Backoffice.Campus.Fields.Suburb")]
        [AllowHtml]
        public IList<SelectListItem> AvailableSuburb { get; set; }
        */
        //public bool editable { get; set; }
    }
}