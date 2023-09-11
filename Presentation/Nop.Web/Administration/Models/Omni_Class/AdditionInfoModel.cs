using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Omni_Item;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Admin.Models.Omni_Class
{
    public partial class AdditionInfoModel : BaseNopEntityModel
    {
        public AdditionInfoModel()
        {
            AvailableBranchs = new List<SelectListItem>();
            AvailableGrade = new List<SelectListItem>();
            AvailableActual_Grade = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Omni_Backoffice.AdditionInfo.Fields.Stud_Id")]
        [AllowHtml]
        public string Stud_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.AdditionInfo.Fields.Grade")]
        [AllowHtml]
        public string Grade { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.AdditionInfo.Fields.Actual_Grade")]
        [AllowHtml]
        public string Actual_Grade { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.AdditionInfo.Fields.Branch")]
        [AllowHtml]
        public string Branch { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.AdditionInfo.Fields.FirstName")]
        [AllowHtml]
        public string FirstName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.AdditionInfo.Fields.LastName")]
        [AllowHtml]
        public string LastName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.AdditionInfo.Fields.Remark")]
        [AllowHtml]
        public string Remarks { get; set; }

        public IList<SelectListItem> AvailableBranchs { get; set; }
        public IList<SelectListItem> AvailableGrade { get; set; }
        public IList<SelectListItem> AvailableActual_Grade { get; set; }

        public bool is_admin { get; set; }


    }
}