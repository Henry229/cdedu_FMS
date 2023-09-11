using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Omni_Basis;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;

namespace Nop.Admin.Models.Omni_Basis
{
    [Validator(typeof(CalendarMasterValidator))]
    public partial class CalendarMasterModel : BaseNopEntityModel
    {
        public CalendarMasterModel()
        {
            AvailableYear = new List<SelectListItem>();
            AvailableTerm = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CalendarMaster.Fields.Year")]
        [AllowHtml]
        public string Year { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CalendarMaster.Fields.Term")]
        [AllowHtml]
        public string Term { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CalendarMaster.Fields.Week")]
        [AllowHtml]
        public int Week { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CalendarMaster.Fields.StartDate")]
        [AllowHtml]
        public System.DateTime StartDate { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CalendarMaster.Fields.EndDate")]
        [AllowHtml]
        public System.DateTime EndDate { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.CalendarMaster.Fields.YN_Active")]
        [AllowHtml]
        public string YN_Active { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CalendarMaster.Fields.YN_PA")]
        [AllowHtml]
        public string YN_PA { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CalendarMaster.Fields.YN_Enrol")]
        [AllowHtml]
        public string YN_Enrol { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.CalendarMaster.Fields.reg_date")]
        [AllowHtml]
        public System.DateTime reg_date { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CalendarMaster.Fields.reg_source")]
        [AllowHtml]
        public string reg_source { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.CalendarMaster.Fields.Remarks")]
        [AllowHtml]
        public string Remarks { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Year")]
        public IList<SelectListItem> AvailableYear { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ItemSet.Fields.Term")]
        public IList<SelectListItem> AvailableTerm { get; set; }
    }
}