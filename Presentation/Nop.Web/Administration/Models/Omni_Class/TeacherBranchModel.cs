using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Omni_Item;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Nop.Admin.Models.Omni_Class
{
    public partial class TeacherBranchModel : BaseNopEntityModel
    {
        public TeacherBranchModel()
        {
            AvailableBranch = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TeacherBranch.Fields.Teacher_Id")]
        [AllowHtml]
        public int Teacher_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.Title")]
        [AllowHtml]
        public string Title { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.FirstName")]
        [AllowHtml]
        public string FirstName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.LastName")]
        [AllowHtml]
        public string LastName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TeacherBranch.Fields.Branch")]
        [AllowHtml]
        public string Branch { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TeacherBranch.Fields.Branch")]
        [AllowHtml]
        public string BranchPop { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TeacherBranch.Fields.Check")]
        [AllowHtml]
        public bool Check { get; set; }


        public Admin.Models.Omni_Basis.CampusModel campuspopup { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.TeacherBranch.Fields.Branch")]
        public IList<SelectListItem> AvailableBranch { get; set; }



    }
}