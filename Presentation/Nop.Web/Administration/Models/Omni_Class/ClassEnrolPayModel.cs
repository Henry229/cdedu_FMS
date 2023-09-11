using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Omni_Item;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Admin.Models.Omni_Class
{
    public partial class ClassEnrolPayModel : BaseNopEntityModel
    {
        public ClassEnrolPayModel()
        {

        }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol_Pay.Fields.Stud_Id")]
        [AllowHtml]
        public string Stud_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol_Pay.Fields.Stud_Name")]
        [AllowHtml]
        public string Stud_Name { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol_Pay.Fields.Grade")]
        [AllowHtml]
        public string Grade { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol_Pay.Fields.Id_Enrol")]
        [AllowHtml]
        public int Id_Enrol { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol_Pay.Fields.Course")]
        [AllowHtml]
        public string CourseName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.TotalAmount")]
        [AllowHtml]
        public decimal TotalAmount { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol_Pay.Fields.Seq")]
        [AllowHtml]
        public int Seq { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol_Pay.Fields.PayDate")]
        [AllowHtml]
        public System.DateTime PayDate { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol_Pay.Fields.PayAmount")]
        [AllowHtml]
        public decimal PayAmount{ get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassEnrol.Fields.Remark")]
        [AllowHtml]
        public string Remark { get; set; }


    }
}