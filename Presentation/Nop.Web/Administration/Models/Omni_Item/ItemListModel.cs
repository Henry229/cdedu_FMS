using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Omni_Item
{
    public class ItemListModel : BaseNopModel
    {
        public ItemListModel()
        {
            ItemCategoryList = new List<SelectListItem>();
            AvailableGradeList = new List<SelectListItem>();
            AvailableTermList = new List<SelectListItem>();
        }
        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.List.Id")]
        [AllowHtml]
        public int Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.List.ItemCode")]
        [AllowHtml]
        public string ItemCode { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.List.ItemName")]
        public string ItemName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.List.ItemCategory")]
        public string ItemCategoryId { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.List.ItemCategory")]
        public string ItemCategoryName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.Grade")]
        public string Grade { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.Term")]
        public string Term { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.List.Qty_Balance")]
        [AllowHtml]
        public int Qty_Balance { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.List.ItemCategory")]
        public IList<SelectListItem> ItemCategoryList { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Grade")]
        public IList<SelectListItem> AvailableGradeList { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Term")]
        public IList<SelectListItem> AvailableTermList { get; set; }

        public bool editable { get; set; }
    }
}