using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Omni_Royalty
{
    public class ItemFeeModel : BaseNopModel
    {
        public ItemFeeModel()
        {
            AvailableBranchList = new List<SelectListItem>();
            AvailableTermList = new List<SelectListItem>();
            AvailableYearList = new List<SelectListItem>();
        }


        [NopResourceDisplayName("Admin.Omni_Royalty.Item.List.Id")]
        [AllowHtml]
        public int Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.Item.List.BranchCode")]
        public string BranchCode { get; set; }
        public IList<SelectListItem> AvailableBranchList { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.Item.List.ItemCode")]
        [AllowHtml]
        public string ItemCode { get; set; }


        [NopResourceDisplayName("Admin.Omni_Royalty.Item.List.ItemName")]
        [AllowHtml]
        public string ItemName { get; set; }


        [NopResourceDisplayName("Admin.Omni_Royalty.Item.List.Price")]
        [AllowHtml]
        public decimal Price { get; set; }


        [NopResourceDisplayName("Admin.Omni_Royalty.Item.List.Qty")]
        public decimal Qty { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.Item.List.Amt_Total")]
        [AllowHtml]
        public decimal Amt_Total { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.InvoiceNo")]
        [AllowHtml]
        public string InvoiceNo { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.Enrollment.List.Year")]
        public string Year { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Enrollment.List.Term")]
        public string Term { get; set; }

        public IList<SelectListItem> AvailableYearList { get; set; }
        public IList<SelectListItem> AvailableTermList { get; set; }

        public bool is_admin { get; set; }

    }
}