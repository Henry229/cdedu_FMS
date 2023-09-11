using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Admin.Models.Omni_Printing
{

    public partial class PrintRequestItemModel : BaseNopEntityModel
    {
        public PrintRequestItemModel()
        {
            AvailableItemType = new List<SelectListItem>();
            AvailableSize = new List<SelectListItem>();
            AvailableGsm = new List<SelectListItem>();
            AvailableColor = new List<SelectListItem>();
            AvailableDocStyle = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Omni_Printing.PrintRequestItem.Fields.Req_Id")]
        public int Req_Id { get; set; }


        [NopResourceDisplayName("Admin.Omni_Printing.PrintRequestItem.Fields.Item_Type")]
        public int Item_Type { get; set; }

        [NopResourceDisplayName("Admin.Omni_Printing.PrintRequestItem.Fields.Remarks")]
        [AllowHtml]
        public string Remarks { get; set; }

        [NopResourceDisplayName("Admin.Omni_Printing.PrintRequestItem.Fields.Size")]
        [AllowHtml]
        public int PrintSize { get; set; }

        [NopResourceDisplayName("Admin.Omni_Printing.PrintRequestItem.Fields.Gsm")]
        [AllowHtml]
        public int PrintGsm { get; set; }

        [NopResourceDisplayName("Admin.Omni_Printing.PrintRequestItem.Fields.Color")]
        [AllowHtml]
        public int PrintColor { get; set; }

        [NopResourceDisplayName("Admin.Omni_Printing.PrintRequestItem.Fields.DocStyle")]
        [AllowHtml]
        public int PrintDocStyle { get; set; }


        [UIHint("Picture")]
        [NopResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.Picture")]
        public int PictureId { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.Picture")]
        public string PictureUrl { get; set; }


        [NopResourceDisplayName("Admin.Omni_Printing.PrintRequestItem.Fields.Item_Id")]
        public IList<SelectListItem> AvailableItemType { get; set; }

        [NopResourceDisplayName("Admin.Omni_Printing.PrintRequestItem.Fields.Size")]
        public IList<SelectListItem> AvailableSize { get; set; }

        [NopResourceDisplayName("Admin.Omni_Printing.PrintRequestItem.Fields.Gsm")]
        public IList<SelectListItem> AvailableGsm { get; set; }

        [NopResourceDisplayName("Admin.Omni_Printing.PrintRequestItem.Fields.Color")]
        public IList<SelectListItem> AvailableColor { get; set; }

        [NopResourceDisplayName("Admin.Omni_Printing.PrintRequestItem.Fields.DocStyle")]
        public IList<SelectListItem> AvailableDocStyle { get; set; }


        public bool isadmin { get; set; }

    }
}