using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Omni_Order
{
    public partial class OrderGroupModel : BaseNopEntityModel 
    {

        public OrderGroupModel()
        {
            this.AvailableSetCategorys = new List<SelectListItem>();
            this.AvailableGrades = new List<SelectListItem>();
            this.AvailableLevels = new List<SelectListItem>();
            this.AvailableTerms = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Omni_Order.OrderGroup.List.Order_Id")]
        [AllowHtml]
        public int Order_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Order.OrderItem.List.Order_Type")]
        [AllowHtml]
        public string Order_Type { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Id")]
        [AllowHtml]
        public int ItemSet_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.SetName")]
        [AllowHtml]
        public string SetName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.SetCategory")]
        [AllowHtml]
        public string SetCategory { get; set; }
        public IList<SelectListItem> AvailableSetCategorys { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Grade")]
        [AllowHtml]
        public string Grade { get; set; }
        public IList<SelectListItem> AvailableGrades { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Grade")]
        [AllowHtml]
        public string Grade2 { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Term")]
        [AllowHtml]
        public string Term { get; set; }
        public IList<SelectListItem> AvailableTerms { get; set; }


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

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_D.Fields.Qty")]
        [AllowHtml]
        public int Qty_Teacher { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_D.Qty_NewBook")]
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