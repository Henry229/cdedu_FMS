using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.ComponentModel.DataAnnotations;


namespace Nop.Admin.Models.Omni_Royalty
{
    public class PrimeVideoSummaryModel : BaseNopEntityModel
    {
        public PrimeVideoSummaryModel()
        {
            AvailableBranch = new List<SelectListItem>();
            AvailableTerm = new List<SelectListItem>();
            AvailableYear = new List<SelectListItem>();
        }


        [NopResourceDisplayName("Admin.Omni_Backoffice.PrimeVideo.Fields.Id")]
        [AllowHtml]
        public int Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.PrimeVideo.Fields.Year")]
        public string Year { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.PrimeVideo.Fields.Term")]
        public string Term { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.PrimeVideo.Fields.Branch")]
        [AllowHtml]
        public string Branch { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Closing.Fields.YN_Closing")]
        public string YN_Closing { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Closing.Fields.YN_Approval")]
        public string YN_Approval { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Closing.Fields.YN_Paid")]
        public string YN_Paid { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Closing.Fields.Amnt_adjust")]
        public decimal Amnt_adjust { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Closing.Fields.Amnt_Freight")]
        public decimal Amnt_Freight { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Closing.List.InvoiceNo")]
        [AllowHtml]
        public string InvoiceNo { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Closing.Fields.Remarks")]
        [AllowHtml]
        public string Remarks { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Closing.Fields.DueDate")]
        [AllowHtml]
        public string DueDate { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Closing.Fields.reg_date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime reg_date { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.Closing.Fields.Branch")]
        public IList<SelectListItem> AvailableBranch { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.Closing.Fields.Type")]
        public IList<SelectListItem> AvailableType { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Closing.Fields.Year")]
        public IList<SelectListItem> AvailableYear { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Closing.Fields.Term")]
        public IList<SelectListItem> AvailableTerm { get; set; }

        public bool isadmin { get; set; }
    }




}