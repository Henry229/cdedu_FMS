using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Omni_Item;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Omni_Item
{
    [Validator(typeof(ItemCategoryValidator))]
    public partial class ItemCategoryModel : BaseNopEntityModel 
    {
        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemCategory.Fields.CategoryCode")]
        [AllowHtml]
        public string CategoryCode { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemCategory.Fields.CategoryName")]
        [AllowHtml]
        public string CategoryName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemCategory.Fields.Remarks")]
        [AllowHtml]
        public string Remarks { get; set; }

       
    }
}