using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Omni_Item;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Nop.Admin.Models.Omni_Class
{
    public partial class StudentBranchModel : BaseNopEntityModel
    {
        public StudentBranchModel()
        {
            AvailableBranch = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Omni_Backoffice.StudentBranch.Fields.Stud_Id")]
        [AllowHtml]
        public string Stud_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.StudentBranch.Fields.FirstName")]
        [AllowHtml]
        public string FirstName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.StudentBranch.Fields.LastName")]
        [AllowHtml]
        public string LastName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.StudentBranch.Fields.Branch")]
        [AllowHtml]
        public string Branch { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.StudentBranch.Fields.Remarks")]
        [AllowHtml]
        public string Remarks { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.StudentBranch.Fields.Branch")]
        [AllowHtml]
        public string BranchPop { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.StudentBranch.Fields.Check")]
        [AllowHtml]
        public bool Check { get; set; }


        public Admin.Models.Omni_Basis.CampusModel campuspopup { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.StudentBranch.Fields.Branch")]
        public IList<SelectListItem> AvailableBranch { get; set; }



    }
}