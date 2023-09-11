using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.ComponentModel.DataAnnotations;


namespace Nop.Admin.Models.Omni_Order
{
    public class OrderListModel : BaseNopEntityModel
    {
        public OrderListModel()
        {
            AvailableBranchCode = new List<SelectListItem>();
            AvailableOrderType = new List<SelectListItem>();
            AvailableOrderStatus = new List<SelectListItem>();
            AvailableOrderTerm = new List<SelectListItem>();
            AvailableOrderYear = new List<SelectListItem>();

        }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order.Fields.Id")]
        [AllowHtml]
        public int Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order.Fields.BranchCode")]
        [AllowHtml]
        public string BranchCode { get; set; }

        
        [NopResourceDisplayName("Admin.Omni_Backoffice.Order.Fields.OrderDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public  System.DateTime OrderDate { get; set; }

        /*Edited by Karen See 22/02/2021*/
        [NopResourceDisplayName("Admin.Omni_Backoffice.Order.Fields.Confirm_Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime? Confirm_Date { get; set; }
        /*End */


        [NopResourceDisplayName("Admin.Omni_Backoffice.Order.Fields.EstimateDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime? EstimateDate { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order.Fields.OrderType")]
        public string OrderType { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order.Fields.Year")]
        public string Year { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order.Fields.Term")]
        public string Term { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order.Fields.Week")]
        public int Week { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order.Fields.Status")]
        public string Status { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order.Fields.SumPrice")]
        public decimal SumPrice { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order.Fields.Print_date")]
        public System.DateTime? print_date { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order.Fields.Deliver_date")]
        public System.DateTime? deliver_date { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order.Fields.BranchCode")]
        public IList<SelectListItem> AvailableBranchCode { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.Order.Fields.OrderType")]
        public IList<SelectListItem> AvailableOrderType { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order.Fields.OrderStatus")]
        public IList<SelectListItem> AvailableOrderStatus { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order.Fields.Year")]
        public IList<SelectListItem> AvailableOrderYear { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Order.Fields.Term")]
        public IList<SelectListItem> AvailableOrderTerm { get; set; }

        public bool isadmin { get; set; }
        public bool issuperadmin { get; set; }
    }
}