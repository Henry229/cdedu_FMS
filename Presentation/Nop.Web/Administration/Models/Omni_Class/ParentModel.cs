using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Omni_Item;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Admin.Models.Omni_Class
{
    
    public partial class ParentModel : BaseNopEntityModel
    {
        public ParentModel()
        {
            AvailableBranchs = new List<SelectListItem>();
            
        }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Parent.Fields.FirstName")]
        [AllowHtml]
        public string FirstName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Parent.Fields.LastName")]
        [AllowHtml]
        public string LastName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Parent.Fields.Branch")]
        [AllowHtml]
        public string Branch { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Parent.Fields.PhoneNo")]
        [AllowHtml]
        public string PhoneNo { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Parent.Fields.MobileNo1")]
        [AllowHtml]
        public string MobileNo1 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Parent.Fields.MobileNo2")]
        [AllowHtml]
        public string MobileNo2 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Parent.Fields.Email")]
        [AllowHtml]
        public string Email { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Parent.Fields.Remarks")]
        [AllowHtml]
        public string Remarks { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Parent.Fields.Branch")]
        public IList<SelectListItem> AvailableBranchs { get; set; }


        public bool isadmin { get; set; }

    }
}