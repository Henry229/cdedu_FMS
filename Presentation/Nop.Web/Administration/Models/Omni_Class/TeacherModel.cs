using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Omni_Item;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Nop.Admin.Models.Omni_Class
{
    public partial class TeacherModel : BaseNopEntityModel 
    {
        public TeacherModel()
        {
            AvailableBranch = new List<SelectListItem>();
            AvailableTitle = new List<SelectListItem>();
            AvailableGender = new List<SelectListItem>();
            AvailableWorkingCond = new List<SelectListItem>();
            AvailableTeachingGrade = new List<SelectListItem>();
            AvailableSubject = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.Title")]
        [AllowHtml]
        public string Title { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.FirstName")]
        [AllowHtml]
        public string FirstName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.LastName")]
        [AllowHtml]
        public string LastName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.Gender")]
        [AllowHtml]
        public string Gender { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.TeacherRole")]
        [AllowHtml]
        public string TeacherRole { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.TutorRole")]
        [AllowHtml]
        public string TutorRole { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.MarkerRole")]
        [AllowHtml]
        public string MarkerRole { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.TeachingGrade")]
        [AllowHtml]
        public string TeachingGrade { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.Subject")]
        [AllowHtml]
        public string Subject { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.WorkingCond")]
        [AllowHtml]
        public string WorkingCond { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.Address")]
        [AllowHtml]
        public string Address { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.Mobil")]
        [AllowHtml]
        public string Mobile { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.HomePhone")]
        [AllowHtml]
        public string HomePhone { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.eMail")]
        [AllowHtml]
        public string eMail { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.Branch")]
        [AllowHtml]
        public string Branch { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.Remarks")]
        [AllowHtml]
        public string Remarks { get; set; }

        public bool is_Admin { get; set; }




        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.Branch")]
        public IList<SelectListItem> AvailableBranch { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.Title")]
        public IList<SelectListItem> AvailableTitle { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.Gender")]
        public IList<SelectListItem> AvailableGender { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.WorkingCond")]
        public IList<SelectListItem> AvailableWorkingCond { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.TeachingGrade")]
        public IList<SelectListItem> AvailableTeachingGrade { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.Subject")]
        public IList<SelectListItem> AvailableSubject { get; set; }



    }
}