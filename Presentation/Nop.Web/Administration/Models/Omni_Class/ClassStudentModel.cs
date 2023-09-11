using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Omni_Item;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Admin.Models.Omni_Class
{
    public partial class ClassStudentModel : BaseNopEntityModel
    {
        public ClassStudentModel()
        {
            AvailableBranch = new List<SelectListItem>();
            AvailableGrade = new List<SelectListItem>();
            AvailableClass = new List<SelectListItem>();
            AvailableStatus = new List<SelectListItem>();
            AvailableTimeType = new List<SelectListItem>();

            AvailableClass_Mod = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassSchedule.Fields.Class_Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Class_Date { get; set; }

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


        [NopResourceDisplayName("Admin.Omni_Backoffice.Student.Fields.Branch")]
        [AllowHtml]
        public string Branch { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Student.Fields.Class_Id")]
        [AllowHtml]
        public int Class_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Student.Fields.Class_Id")]
        [AllowHtml]
        public string Class_Id_Mod { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Student.Fields.Class_Name")]
        [AllowHtml]
        public string Class_Name { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Student.Fields.Class_D_Id")]
        [AllowHtml]
        public int Class_D_Id { get; set; }




        [NopResourceDisplayName("Admin.Omni_Backoffice.Student.Fields.HomePhone")]
        [AllowHtml]
        public string HomePhone { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Student.Fields.Mobile1")]
        [AllowHtml]
        public string Mobile1 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Student.Fields.Mobile2")]
        [AllowHtml]
        public string Mobile2 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Student.Fields.Status")]
        [AllowHtml]
        public string Status { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.Remarks")]
        [AllowHtml]
        public string Remarks { get; set; }

        public string TimeType { get; set; }



        [NopResourceDisplayName("Admin.Omni_Backoffice.Student.Fields.Branch")]
        public IList<SelectListItem> AvailableBranch { get; set; }

        public IList<SelectListItem> AvailableGrade { get; set; }

        public IList<SelectListItem> AvailableClass { get; set; }

        public IList<SelectListItem> AvailableClass_Mod { get; set; }

        public IList<SelectListItem> AvailableStatus { get; set; }

        public IList<SelectListItem> AvailableTimeType { get; set; }

        public bool is_Admin { get; set; }

    }
}