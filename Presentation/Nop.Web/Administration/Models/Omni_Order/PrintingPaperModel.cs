using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.ComponentModel.DataAnnotations;


namespace Nop.Admin.Models.Omni_Order
{
    public class PrintingPaperModel : BaseNopEntityModel
    {
        public PrintingPaperModel()
        {
            AvailableBranchCode = new List<SelectListItem>();
            AvailableOrderTerm = new List<SelectListItem>();
            AvailableOrderYear = new List<SelectListItem>();

        }

        [NopResourceDisplayName("Admin.Omni_Backoffice.PrintingPaper.Fields.Id")]
        [AllowHtml]
        public int Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.PrintingPaper.Fields.BranchCode")]
        [AllowHtml]
        public string BranchCode { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.PrintingPaper.Fields.Year")]
        public string Year { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.PrintingPaper.Fields.Term")]
        public string Term { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.PrintingPaper.Fields.Course")]
        public int Course_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.PrintingPaper.Fields.Course")]
        public string Course { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.PrintingPaper.Fields.TestNo")]
        public int TestNo { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.PrintingPaper.Fields.Qty")]
        public int Qty { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.PrintingPaper.Fields.Qty_Teacher")]
        public int Qty_Teacher { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.PrintingPaper.List.YN_Print")]
        public string YN_Print { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.Order.Fields.BranchCode")]
        public IList<SelectListItem> AvailableBranchCode { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order.Fields.Year")]
        public IList<SelectListItem> AvailableOrderYear { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order.Fields.Term")]
        public IList<SelectListItem> AvailableOrderTerm { get; set; }

        public bool isadmin { get; set; }
    }
}