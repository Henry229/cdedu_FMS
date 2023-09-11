using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Omni_Order
{
    public class OrderItemModel : BaseNopEntityModel 
    {

        public OrderItemModel()
        {
            this.AvailableGrades = new List<SelectListItem>();
            this.AvailableLevels = new List<SelectListItem>();
            this.AvailableTerms = new List<SelectListItem>();
            this.AvailableItems = new List<SelectListItem>();
            this.AvailableItemPrices = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Omni_Order.OrderItem.List.Order_Id")]
        [AllowHtml]
        public int Order_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Order.OrderItem.List.Order_Type")]
        [AllowHtml]
        public string Order_Type { get; set; }

        [NopResourceDisplayName("Admin.Omni_Order.OrderItem.List.Seq")]
        [AllowHtml]
        public int Seq { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.ItemCode")]
        [AllowHtml]
        public string ItemCode { get; set; }
        public IList<SelectListItem> AvailableItems { get; set; }
        public IList<SelectListItem> AvailableItemPrices { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.ItemName")]
        [AllowHtml]
        public string ItemName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Term")]
        [AllowHtml]
        public string Term { get; set; }
        public IList<SelectListItem> AvailableTerms { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Grade")]
        [AllowHtml]
        public string Grade { get; set; }
        public IList<SelectListItem> AvailableGrades { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Level")]
        [AllowHtml]
        public string Level { get; set; }
        public IList<SelectListItem> AvailableLevels { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_D.Fields.Week")]
        [AllowHtml]
        public int Week { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_D.Fields.Qty")]
        [AllowHtml]
        public int Qty { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_D.Fields.Qty_Half")]
        [AllowHtml]
        public int Qty_Half { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_D.Fields.Qty_Teacher")]
        [AllowHtml]
        public int Qty_Teacher { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_D.Fields.Qty_NewBook")]
        [AllowHtml]
        public int Qty_NewBook { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_D.Fields.is_Half")]
        [AllowHtml]
        public string is_Half { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_D.Fields.Remarks")]
        [AllowHtml]
        public string Remarks { get; set; }



        public bool is_Editable { get; set; }

    }
}