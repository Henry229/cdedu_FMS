using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Omni_Item;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Admin.Models.Omni_Class
{
    public partial class ClassEnrolModel : BaseNopEntityModel
    {
        public ClassEnrolModel()
        {
            AvailableBranchList = new List<SelectListItem>();
            AvailablePayStatusList = new List<SelectListItem>();
            AvailableTermList = new List<SelectListItem>();
            AvailableYearList = new List<SelectListItem>();
            AvailableClassList = new List<SelectListItem>();
            AvailableDiscount_Code = new List<SelectListItem>();
            AvailablePayMethod = new List<SelectListItem>();
            AvailableLevel = new List<SelectListItem>();
            AvailableGrade = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.Year")]
        [AllowHtml]
        public string Year { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.Term")]
        [AllowHtml]
        public string Term { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.Grade")]
        [AllowHtml]
        public string Grade { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.Branch")]
        [AllowHtml]
        public string Branch { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.Class_Id")]
        [AllowHtml]
        public int Class_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.Category")]
        [AllowHtml]
        public string Category { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.Class_Id")]
        [AllowHtml]
        public string Course_Name { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.Stud_Id")]
        [AllowHtml]
        public string Stud_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.FirstName")]
        [AllowHtml]
        public string FirstName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.LastName")]
        [AllowHtml]
        public string LastName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.Course_Id")]
        [AllowHtml]
        public int Course_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.Unit_Price")]
        [AllowHtml]
        public decimal Unit_Price { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.AttendWeek")]
        [AllowHtml]
        public int AttendWeek { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.MaterialFee")]
        [AllowHtml]
        public decimal MaterialFee { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.HalfBook")]
        [AllowHtml]
        public string HalfBook { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.Level")]
        [AllowHtml]
        public string Level { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.Discount_Code")]
        [AllowHtml]
        public string Discount_Code { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.Discount_Rate")]
        [AllowHtml]
        public decimal Discount_Rate { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.Discount_CR")]
        [AllowHtml]
        public string PaymentMethod { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.Discount_ETC")]
        [AllowHtml]
        public decimal TotalAmount { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Student.Fields.HomePhone")]
        [AllowHtml]
        public string HomePhone { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Student.Fields.Mobile")]
        [AllowHtml]
        public string Mobile { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Student.Fields.Mobile2")]
        [AllowHtml]
        public string Mobile2 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.Chk_HD")]
        [AllowHtml]
        public string Chk_HD { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.Chk_FM")]
        [AllowHtml]
        public string Chk_FM { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.Remark")]
        [AllowHtml]
        public string Remark { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.PayStatus")]
        [AllowHtml]
        public string PayStatus { get; set; }

        public IList<SelectListItem> AvailableBranchList { get; set; }
        public IList<SelectListItem> AvailableYearList { get; set; }
        public IList<SelectListItem> AvailableTermList { get; set; }
        public IList<SelectListItem> AvailablePayStatusList { get; set; }
        public IList<SelectListItem> AvailableClassList { get; set; }
        public IList<SelectListItem> AvailableDiscount_Code { get; set; }
        public IList<SelectListItem> AvailablePayMethod { get; set; }
        public IList<SelectListItem> AvailableLevel { get; set; }
        public IList<SelectListItem> AvailableGrade { get; set; }

        public bool is_admin { get; set; }


    }
}