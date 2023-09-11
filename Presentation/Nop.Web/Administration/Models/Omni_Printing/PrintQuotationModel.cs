using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Admin.Models.Omni_Printing
{

    public partial class PrintQuotationModel : BaseNopEntityModel
    {
        public PrintQuotationModel()
        {

        }

        [NopResourceDisplayName("Admin.Omni_Printing.PrintQuotation.Fields.Req_Id")]
        public int Req_Id { get; set; }


        [NopResourceDisplayName("Admin.Omni_Printing.PrintQuotation.Fields.Due_Date")]
        public System.DateTime Due_Date { get; set; }

        [NopResourceDisplayName("Admin.Omni_Printing.PrintQuotation.Fields.ContentText")]
        [AllowHtml]
        public string ContentText { get; set; }

        [NopResourceDisplayName("Admin.Omni_Printing.PrintQuotation.Fields.Quot_Amount")]
        [AllowHtml]
        public decimal Quot_Amount { get; set; }

        [UIHint("Picture")]
        [NopResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.Picture")]
        public int PictureId { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.Picture")]
        public string PictureUrl { get; set; }


        public bool isadmin { get; set; }

    }
}