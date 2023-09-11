using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Admin.Models.Omni_Printing
{

    public partial class PrintRequestModel : BaseNopEntityModel
    {
        public PrintRequestModel()
        {
            AvailableStatus = new List<SelectListItem>();
            RequestItem = new PrintRequestItemModel();
            Quotation = new PrintQuotationModel();
        }

        [NopResourceDisplayName("Admin.Omni_Printing.PrintRequest.Fields.ReqDate")]
        public System.DateTime ReqDate { get; set; }

        [NopResourceDisplayName("Admin.Omni_Printing.PrintRequest.Fields.DueDate")]
        public System.DateTime DueDate { get; set; }


        [NopResourceDisplayName("Admin.Omni_Printing.PrintRequest.Fields.User_Id")]
        public string User_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Printing.PrintRequest.Fields.Status")]
        public string Status { get; set; }

        [NopResourceDisplayName("Admin.Omni_Printing.PrintRequest.Fields.Status")]
        public int StatusVal { get; set; }


        [NopResourceDisplayName("Admin.Omni_Printing.PrintRequest.Fields.Title")]
        [AllowHtml]
        public string Title { get; set; }

        [NopResourceDisplayName("Admin.Omni_Printing.PrintRequest.Fields.ContentText")]
        [AllowHtml]
        public string ContentText { get; set; }

        [NopResourceDisplayName("Admin.Omni_Printing.PrintRequest.Fields.Remarks")]
        [AllowHtml]
        public string Remarks { get; set; }


        [NopResourceDisplayName("Admin.Omni_Printing.PrintRequest.Fields.Status")]
        public IList<SelectListItem> AvailableStatus { get; set; }

        [NopResourceDisplayName("Admin.Omni_Printing.PrintRequest.Fields.ReqDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime FromDate { get; set; }

        [NopResourceDisplayName("Admin.Omni_Printing.PrintRequest.Fields.ReqDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime ToDate { get; set; }

        public PrintRequestItemModel RequestItem { get; set; }

        public PrintQuotationModel Quotation { get; set; }

        public bool isadmin { get; set; }

    }
}