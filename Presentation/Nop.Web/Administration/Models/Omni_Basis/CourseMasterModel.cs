using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Omni_Basis;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Admin.Models.Omni_Basis
{
    [Validator(typeof(CourseMasterValidator))]
    public partial class CourseMasterModel : BaseNopEntityModel
    {

        public CourseMasterModel()
        {
            AvailableYear = new List<SelectListItem>();
            AvailableGrade = new List<SelectListItem>();
            AvailableTerm = new List<SelectListItem>();
            AvailableLevel = new List<SelectListItem>();
            AvailableCourseCategory = new List<SelectListItem>();
            AvailableParentCourse = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CourseMaster.Fields.CourseName")]
        [AllowHtml]
        public string CourseName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CourseMaster.Fields.CourseCategory")]
        [AllowHtml]
        public string CourseCategory { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CourseMaster.Fields.CourseID_P")]
        [AllowHtml]
        public int CourseID_P { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CourseMaster.Fields.Year")]
        [AllowHtml]
        public string Year { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CourseMaster.Fields.Term")]
        [AllowHtml]
        public string Term { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CourseMaster.Fields.Grade")]
        [AllowHtml]
        public string Grade { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CourseMaster.Fields.Level")]
        [AllowHtml]
        public string Level { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CourseMaster.Fields.StartWeek")]
        [AllowHtml]
        public int StartWeek { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CourseMaster.Fields.TotalWeek")]
        [AllowHtml]
        public int TotalWeek { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CourseMaster.Fields.CourseFee")]
        [AllowHtml]
        public decimal CourseFee { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CourseMaster.Fields.Remarks")]
        [AllowHtml]
        public string Remarks { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CourseMaster.Fields.YN_Use")]
        [AllowHtml]
        public string YN_Use { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CourseMaster.Fields.reg_date")]
        [AllowHtml]
        public System.DateTime reg_date { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CourseMaster.Fields.reg_source")]
        [AllowHtml]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string reg_source { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CourseMaster.Fields.CourseFee")]
        [AllowHtml]
        public decimal BookFee { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CourseMaster.Fields.Facility")]
        [AllowHtml]
        public decimal Facility { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CourseMaster.Fields.NewBookFee")]
        [AllowHtml]
        public decimal NewBookFee { get; set; }


        public Admin.Models.Omni_Basis.CourseMasterModel coursepopup { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Year")]
        public IList<SelectListItem> AvailableYear { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Grade")]
        public IList<SelectListItem> AvailableGrade { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Term")]
        public IList<SelectListItem> AvailableTerm { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Level")]
        public IList<SelectListItem> AvailableLevel { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CourseMaster.Fields.CourseCategory")]
        public IList<SelectListItem> AvailableCourseCategory { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CourseMaster.Fields.CourseID_P")]
        public IList<SelectListItem> AvailableParentCourse { get; set; }
    }
}