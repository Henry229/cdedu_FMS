using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Omni_Order
{
    public class StockModel : BaseNopEntityModel
    {

        [NopResourceDisplayName("Admin.Omni_Backoffice.Stock.Fields.Id")]
        [AllowHtml]
        public int Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Stock.Fields.ItemCode")]
        [AllowHtml]
        public string ItemCode { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Stock.Fields.TranDate")]
        [AllowHtml]
        public System.DateTime TranDate { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Stock.Fields.InQty")]
        [AllowHtml]
        public int InQty { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Stock.Fields.OutQty")]
        [AllowHtml]
        public int OutQty { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Stock.Fields.RemainQty")]
        [AllowHtml]
        public int RemainQty { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Stock.Fields.Remark")]
        [AllowHtml]
        public string Remark { get; set; }


    }
}