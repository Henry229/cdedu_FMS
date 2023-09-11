using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Omni_Item;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Admin.Models.Omni_Class
{
    public partial class ClassScheduleRollcallModel : BaseNopEntityModel
    {
        public ClassScheduleRollcallModel()
        {
            //AvailableBranchs = new List<SelectListItem>();
            //AvailableClassRoom = new List<SelectListItem>();
            //AvailableBranch = new List<SelectListItem>();
            // AvailableTitle = new List<SelectListItem>();

        }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.Class_Id")]
        [AllowHtml]
        public int Class_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.Class_D_Id")]
        [AllowHtml]
        public int Class_D_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.Stud_Id")]
        [AllowHtml]
        public string Stud_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.Stud_No")]
        [AllowHtml]
        public string Stud_No { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.Stud_Name")]
        [AllowHtml]
        public string Stud_Name { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.Stud_Id")]
        [AllowHtml]
        public string Attend { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.Stud_Id")]
        [AllowHtml]
        public int Attend2 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.Remarks")]
        [AllowHtml]
        public string Remarks { get; set; }

    }

}