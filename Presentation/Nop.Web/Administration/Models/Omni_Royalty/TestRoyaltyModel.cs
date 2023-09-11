using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Omni_Royalty
{
    public class TestRoyaltyModel : BaseNopModel
    {
        public TestRoyaltyModel()
        {
            AvailableBranchList = new List<SelectListItem>();
            AvailableCourseList = new List<SelectListItem>();
            AvailableTermList = new List<SelectListItem>();
            AvailableYearList = new List<SelectListItem>();
        }


        [NopResourceDisplayName("Admin.Omni_Backoffice.Enrollment.Fields.Id")]
        [AllowHtml]
        public int Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Enrollment.Fields.BranchCode")]
        public string BranchCode { get; set; }
        public IList<SelectListItem> AvailableBranchList { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Enrollment.Fields.Course_Id")]
        [AllowHtml]
        public int Course_Id { get; set; }
        public IList<SelectListItem> AvailableCourseList { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.TestRoyalty.List.CourseName")]
        [AllowHtml]
        public string CourseName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TestRoyalty.List.Gubun")]
        [AllowHtml]
        public string Gubun { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TestRoyalty.List.Week1")]
        public int Week1 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TestRoyalty.List.Week2")]
        public int Week2 { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.TestRoyalty.List.Week3")]
        public int Week3 { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.TestRoyalty.List.Week4")]
        public int Week4 { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.TestRoyalty.List.Week5")]
        public int Week5 { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.TestRoyalty.List.Week6")]
        public int Week6 { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.TestRoyalty.List.Week7")]
        public int Week7 { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.TestRoyalty.List.Week8")]
        public int Week8 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TestRoyalty.List.Week9")]
        public int Week9 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TestRoyalty.List.Week10")]
        public int Week10 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TestRoyalty.List.Week11")]
        public int Week11 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TestRoyalty.List.Week12")]
        public int Week12 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TestRoyalty.List.Week13")]
        public int Week13 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TestRoyalty.List.Week14")]
        public int Week14 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TestRoyalty.List.Week15")]
        public int Week15 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TestRoyalty.List.Week16")]
        public int Week16 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TestRoyalty.List.WeekTotal")]
        public int WeekTotal { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TestRoyalty.List.TestFee")]
        public decimal TestFee { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TestRoyalty.List.Royalty")]
        public decimal Royalty { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TestRoyalty.List.Year")]
        public string Year { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TestRoyalty.List.Term")]
        public string Term { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.InvoiceNo")]
        [AllowHtml]
        public string InvoiceNo { get; set; }

        public IList<SelectListItem> AvailableYearList { get; set; }
        public IList<SelectListItem> AvailableTermList { get; set; }

        public bool is_admin { get; set; }

        public int cnt_Week { get; set; }


    }
}