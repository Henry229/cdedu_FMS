using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Omni_Item;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Nop.Admin.Models.Omni_Class
{
    public partial class TeacherCareerModel : BaseNopEntityModel
    {
        public TeacherCareerModel()
        {
            AvailableCareerType = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TeacherCareer.Fields.Teacher_Id")]
        [AllowHtml]
        public int Teacher_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.Title")]
        [AllowHtml]
        public string Title { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.FirstName")]
        [AllowHtml]
        public string FirstName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.LastName")]
        [AllowHtml]
        public string LastName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TeacherCareer.Fields.CareerType")]
        [AllowHtml]
        public string CareerType { get; set; }


        [NopResourceDisplayName("Admin.Omni_Backoffice.TeacherCareer.Fields.FromDate")]
        [AllowHtml]
        public System.DateTime FromDate { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TeacherCareer.Fields.ToDate")]
        [AllowHtml]
        public System.DateTime ToDate { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TeacherCareer.Fields.Class_Id")]
        [AllowHtml]
        public int Class_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.TeacherCareer.Fields.Remarks")]
        [AllowHtml]
        public string Remarks { get; set; }




        [NopResourceDisplayName("Admin.Omni_Backoffice.TeacherCareer.Fields.CareerType")]
        public IList<SelectListItem> AvailableCareerType { get; set; }



    }
}