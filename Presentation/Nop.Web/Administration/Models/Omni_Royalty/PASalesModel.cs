using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.ComponentModel.DataAnnotations;
namespace Nop.Admin.Models.Omni_Royalty
{
    public class PASalesModel : BaseNopEntityModel
    {
        public PASalesModel()
        {
            AvailableBranch = new List<SelectListItem>();
            AvailableTerm = new List<SelectListItem>();
            AvailableYear = new List<SelectListItem>();
            AvailableLevel = new List<SelectListItem>();
        }


        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_PASales.Fields.Id")]
        [AllowHtml]
        public int Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_PASales.Fields.Year")]
        public string Year { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_PASales.Fields.Term")]
        public string Term { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_PASales.Fields.Branch")]
        [AllowHtml]
        public string Branch { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_PASales.Fields.Title")]
        public string Title { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_PASales.Fields.Level")]
        public string Level { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_PASales.Fields.Price")]
        public decimal Price { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_PASales.Fields.List_Price")]
        public decimal List_Price { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_PASales.Fields.Diff_Price")]
        public decimal Diff_Price { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_PASales.Fields.Amt_Total")]
        public decimal Amt_Total { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_PASales.Fields.Payment")]
        [AllowHtml]
        public string Payment { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_PASales.Fields.Stud_id")]
        [AllowHtml]
        public string Stud_id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_PASales.Fields.Stud_Name")]
        [AllowHtml]
        public string Stud_Name { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_PASales.Fields.Purchase_Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Purchase_Date { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_PASales.Fields.Purchase_Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Purchase_Date_From { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_PASales.Fields.Purchase_Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Purchase_Date_To { get; set; }




        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_PASales.Fields.Stud_id")]
        [AllowHtml]
        public string InvoiceNo { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.Closing.Fields.Branch")]
        public IList<SelectListItem> AvailableBranch { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Closing.Fields.Year")]
        public IList<SelectListItem> AvailableYear { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Closing.Fields.Term")]
        public IList<SelectListItem> AvailableTerm { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order_PASales.Fields.Level")]
        public IList<SelectListItem> AvailableLevel { get; set; }

        public bool is_admin { get; set; }

    }
}