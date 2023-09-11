using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Omni_Item;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Admin.Models.Omni_Item
{
    [Validator(typeof(ItemValidator))]
    public partial class ItemModel : BaseNopEntityModel 
    {

        public ItemModel()
        {
            this.AvailableCategorys = new List<SelectListItem>();
            this.AvailableGrades = new List<SelectListItem>();
            this.AvailableLevels = new List<SelectListItem>();
            this.AvailableTerms = new List<SelectListItem>();
            this.AvailableSubjects = new List<SelectListItem>();
            
        }
        

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.ItemCode")]
        [AllowHtml]
        public string ItemCode { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.ItemName")]
        [AllowHtml]
        public string ItemName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.ItemCategory")]
        [AllowHtml]
        public string ItemCategory { get; set; }
        public IList<SelectListItem> AvailableCategorys { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.DT_From")]
        [AllowHtml]
        public System.DateTime DT_From { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.DT_To")]
        [AllowHtml]
        public System.DateTime DT_To { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.UnitPrice")]
        [AllowHtml]
        public decimal UnitPrice { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.UnitPrice_Half")]
        [AllowHtml]
        public decimal UnitPrice_Half { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.Grade")]
        [AllowHtml]
        public string Grade { get; set; }
        public IList<SelectListItem> AvailableGrades { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.Term")]
        [AllowHtml]
        public string Term { get; set; }
        public IList<SelectListItem> AvailableTerms { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.Level")]
        [AllowHtml]
        public string Level { get; set; }
        public IList<SelectListItem> AvailableLevels { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.Subject")]
        [AllowHtml]
        public string Subject { get; set; }
        public IList<SelectListItem> AvailableSubjects { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.Item.Fields.Remarks")]
        [AllowHtml]
        public string Remarks { get; set; }

        public string PrimaryStoreCurrencyCode { get; set; }


    }
}