using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Omni_Royalty
{
    public class MaterialFeeModel : BaseNopModel
    {
        public MaterialFeeModel()
        {
            AvailableBranchList = new List<SelectListItem>();
            AvailableTermList = new List<SelectListItem>();
            AvailableYearList = new List<SelectListItem>();
        }

            
        [NopResourceDisplayName("Admin.Omni_Royalty.MaterialFee.List.Id")]
        [AllowHtml]
        public int Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.MaterialFee.List.BranchCode")]
        public string BranchCode { get; set; }
        public IList<SelectListItem> AvailableBranchList { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.MaterialFee.List.Course_Id")]
        [AllowHtml]
        public int Course_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.MaterialFee.List.Course_Name")]
        [AllowHtml]
        public string Course_Name { get; set; }


        [NopResourceDisplayName("Admin.Omni_Royalty.MaterialFee.List.MaterialFee")]
        [AllowHtml]
        public decimal MaterialFee { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.MaterialFee.List.Facility")]
        [AllowHtml]
        public decimal Facility { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.MaterialFee.List.FeewithOutFacility")]
        [AllowHtml]
        public decimal FeewithOutFacility { get; set; }


        [NopResourceDisplayName("Admin.Omni_Royalty.MaterialFee.List.Qty_Student")]
        public decimal Qty_Student { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.MaterialFee.List.Amt_Student")]
        [AllowHtml]
        public decimal Amt_Student { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.MaterialFee.List.Qty_Half")]
        public decimal Qty_Half { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.MaterialFee.List.Amt_Half")]
        [AllowHtml]
        public decimal Amt_Half { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.MaterialFee.List.Qty_Teacher")]
        public decimal Qty_Teacher { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.MaterialFee.List.Amt_Teacher")]
        [AllowHtml]
        public decimal Amt_Teacher { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.MaterialFee.List.Qty_NewBook")]
        public decimal Qty_NewBook { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.MaterialFee.List.Amt_NewBook")]
        [AllowHtml]
        public decimal Amt_NewBook { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.MaterialFee.List.Amt_BeforeDiscount")]
        [AllowHtml]
        public decimal Amt_BeforeDiscount { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.MaterialFee.List.Discount")]
        [AllowHtml]
        public decimal Discount { get; set; }

        [NopResourceDisplayName("Admin.Omni_Royalty.MaterialFee.List.Amt_Total")]
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