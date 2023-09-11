using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.ComponentModel.DataAnnotations;
namespace Nop.Admin.Models.Omni_Royalty
{
    public class MarketingFeePaymentModel : BaseNopEntityModel
    {

        [NopResourceDisplayName("Admin.Omni_Backoffice.MarketingFeePayment.Fields.Fee_id")]
        public int Fee_id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.MarketingFeePayment.Fields.Amount")]
        public decimal Amount { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.MarketingFeePayment.Fields.Remarks")]
        public string Remarks { get; set; }



        [NopResourceDisplayName("Admin.Omni_Backoffice.MarketingFeePayment.Fields.PayDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime PayDate { get; set; }

        public bool is_admin { get; set; }

    }
}