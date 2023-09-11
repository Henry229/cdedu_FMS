using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Validators.Omni_Item;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Admin.Models.Omni_Class
{
    public partial class ClassScheduleTeacherModel : BaseNopEntityModel
    {
        public ClassScheduleTeacherModel()
        {
            AvailableTeacher = new List<SelectListItem>();
            
        }
        //[NopResourceDisplayName("Admin.Omni_Backoffice.ClassTeacher.Fields.Id")]
        //[AllowHtml]
        //public int Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleTeacher.Fields.Class_D_Id")]
        [AllowHtml]
        public int Class_D_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleTeacher.Fields.Teacher_Id")]
        [AllowHtml]
        public int Teacher_Id { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.FirstName")]
        [AllowHtml]
        public string FirstName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.LastName")]
        [AllowHtml]
        public string LastName { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.Teacher.Fields.Subject")]
        [AllowHtml]
        public string Subject { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleTeacher.Fields.Duration")]
        [AllowHtml]
        public decimal Duration { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleTeacher.Fields.Remark")]
        [AllowHtml]
        public string Remark { get; set; }

        //[AllowHtml]
        //public string ClassTeacherPop { get; set; }

        [NopResourceDisplayName("Admin.Omni_Backoffice.ClassScheduleTeacher.Fields.Teacher")]
        public IList<SelectListItem> AvailableTeacher { get; set; }
       

    }

}