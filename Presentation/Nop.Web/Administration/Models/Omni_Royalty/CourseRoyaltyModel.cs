using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Omni_Royalty
{
    public class CourseRoyaltyModel : BaseNopModel
    {
        public CourseRoyaltyModel()
        {
            AvailableBranchList = new List<SelectListItem>();
            AvailableTermList = new List<SelectListItem>();
            AvailableYearList = new List<SelectListItem>();
        }


        [NopResourceDisplayName("Admin.Omni_Royalty.CourseRoyalty.List.Id")]
        [AllowHtml]
        public int Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.CourseRoyalty.List.BranchCode")]
        public string BranchCode { get; set; }
        public IList<SelectListItem> AvailableBranchList { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.CourseRoyalty.List.Course_Id")]
        [AllowHtml]
        public int Course_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.CourseRoyalty.List.Course_Name")]
        [AllowHtml]
        public string Course_Name { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.CourseRoyalty.List.TotalWeek")]
        [AllowHtml]
        public int TotalWeek { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.CourseRoyalty.List.Attendence")]
        [AllowHtml]
        public int Attendence { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.CourseRoyalty.List.Tuition_Unit")]
        [AllowHtml]
        public decimal Tuition_Unit { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.CourseRoyalty.List.TotalTuition")]
        [AllowHtml]
        public decimal TotalTuition { get; set; }


        [NopResourceDisplayName("Admin.Omni_Royalty.CourseRoyalty.List.Discount")]
        [AllowHtml]
        public decimal Discount { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.CourseRoyalty.List.Earlybird")]
        [AllowHtml]
        public decimal Earlybird { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.CourseRoyalty.List.CourseFee_ExGST")]
        [AllowHtml]
        public decimal CourseFee_ExGST { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.CourseRoyalty.List.Discount")]
        [AllowHtml]
        public string RoyaltyType { get; set; }


        [NopResourceDisplayName("Admin.Omni_Royalty.CourseRoyalty.List.NetRoyalty")]
        [AllowHtml]
        public decimal NetRoyalty { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.CourseRoyalty.List.CourseRoyalty_IncGST")]
        [AllowHtml]
        public decimal CourseRoyalty_IncGST { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.Enrollment.List.Year")]
        public string Year { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Enrollment.List.Term")]
        public string Term { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.InvoiceNo")]
        [AllowHtml]
        public string InvoiceNo { get; set; }

        public IList<SelectListItem> AvailableYearList { get; set; }
        public IList<SelectListItem> AvailableTermList { get; set; }

        public bool is_admin { get; set; }
    }
}