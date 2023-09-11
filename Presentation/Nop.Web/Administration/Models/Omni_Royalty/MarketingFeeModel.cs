using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.ComponentModel.DataAnnotations;
namespace Nop.Admin.Models.Omni_Royalty
{
    public class MarketingFeeModel : BaseNopEntityModel
    {
        public MarketingFeeModel()
        {
            AvailableBranch = new List<SelectListItem>();
        }


        [NopResourceDisplayName("Admin.Omni_Backoffice.MarketingFee.Fields.Branch")]
        [AllowHtml]
        public string Branch { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.MarketingFee.Fields.Item")]
        public string Item { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.MarketingFee.Fields.Amount")]
        public decimal Amount { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.MarketingFee.Fields.PaidAmount")]
        public decimal PaidAmount { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.MarketingFee.Fields.Balance")]
        public decimal Balance { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.MarketingFee.Fields.Remarks")]
        public string Remarks { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.MarketingFee.Fields.YN_Paid")]
        public string YN_Paid { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.MarketingFee.Fields.IssueDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime IssueDate { get; set; }



        [NopResourceDisplayName("Admin.Omni_Backoffice.MarketingFee.Fields.Branch")]
        public IList<SelectListItem> AvailableBranch { get; set; }


        public bool is_admin { get; set; }

    }
}