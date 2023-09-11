using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Omni_Item;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Admin.Models.Omni_Class
{
    public partial class ClassRollCallAccumModel : BaseNopEntityModel
    {
        public ClassRollCallAccumModel()
        {
            AvailableBranchList = new List<SelectListItem>();
            AvailableGrade = new List<SelectListItem>();
            AvailableYear = new List<SelectListItem>();
            AvailableTerm = new List<SelectListItem>();
            AvailableClass = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.Class_Id")]
        [AllowHtml]
        public int Class_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.Branch")]
        [AllowHtml]
        public string Branch { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.Year")]
        [AllowHtml]
        public string Year { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.Term")]
        [AllowHtml]
        public string Term { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.Grade")]
        [AllowHtml]
        public string Grade { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.Stud_Id")]
        [AllowHtml]
        public string Stud_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.Stud_No")]
        [AllowHtml]
        public string Stud_No { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.Stud_Name")]
        [AllowHtml]
        public string Stud_Name { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.U1")]
        [AllowHtml]
        public string Attend_U1 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.U1")]
        [AllowHtml]
        public string Attend2_U1 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.U2")]
        [AllowHtml]
        public string Attend_U2 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.U2")]
        [AllowHtml]
        public string Attend2_U2 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.U3")]
        [AllowHtml]
        public string Attend_U3 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.U3")]
        [AllowHtml]
        public string Attend2_U3 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.U4")]
        [AllowHtml]
        public string Attend_U4 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.U4")]
        [AllowHtml]
        public string Attend2_U4 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.U5")]
        [AllowHtml]
        public string Attend_U5 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.U5")]
        [AllowHtml]
        public string Attend2_U5 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.U6")]
        [AllowHtml]
        public string Attend_U6 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.U6")]
        [AllowHtml]
        public string Attend2_U6 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.U7")]
        [AllowHtml]
        public string Attend_U7 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.U7")]
        [AllowHtml]
        public string Attend2_U7 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.U8")]
        [AllowHtml]
        public string Attend_U8 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.U8")]
        [AllowHtml]
        public string Attend2_U8 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.U9")]
        [AllowHtml]
        public string Attend_U9 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.U9")]
        [AllowHtml]
        public string Attend2_U9 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.U10")]
        [AllowHtml]
        public string Attend_U10 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.U10")]
        [AllowHtml]
        public string Attend2_U10 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.U11")]
        [AllowHtml]
        public string Attend_U11 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.U11")]
        [AllowHtml]
        public string Attend2_U11 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.U12")]
        [AllowHtml]
        public string Attend_U12 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.U12")]
        [AllowHtml]
        public string Attend2_U12 { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleRollcall.Fields.Remarks")]
        [AllowHtml]
        public string Remarks { get; set; }



        public IList<SelectListItem> AvailableBranchList { get; set; }
        public IList<SelectListItem> AvailableGrade { get; set; }
        public IList<SelectListItem> AvailableYear { get; set; }
        public IList<SelectListItem> AvailableTerm { get; set; }

        public IList<SelectListItem> AvailableClass { get; set; }


        public bool is_admin { get; set; }

    }

}