using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Omni_Item;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Admin.Models.Omni_Class
{
    public partial class StudentModel : BaseNopEntityModel
    {
        public StudentModel()
        {
            AvailableBranch = new List<SelectListItem>();
            AvailableGrade = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Student.Fields.Stud_Id")]
        [AllowHtml]
        public string Stud_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Student.Fields.FirstName")]
        [AllowHtml]
        public string FirstName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Student.Fields.LastName")]
        [AllowHtml]
        public string LastName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Student.Fields.Grade")]
        [AllowHtml]
        public string Grade { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Student.Fields.BirthDay")]
        [AllowHtml]
        public string BirthDay { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Student.Fields.SchoolName")]
        [AllowHtml]
        public string SchoolName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Student.Fields.Branch")]
        [AllowHtml]
        public string Branch { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Student.Fields.ID_Number")]
        [AllowHtml]
        public string ID_Number { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.Student.Fields.Branch")]
        public IList<SelectListItem> AvailableBranch { get; set; }

        public IList<SelectListItem> AvailableGrade { get; set; }

        public bool is_Admin { get; set; }

    }
}